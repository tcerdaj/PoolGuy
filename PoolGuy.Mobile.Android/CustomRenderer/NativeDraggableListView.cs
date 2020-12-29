using Android.Content;
using Android.Views;
using Android.Widget;
using System;
using static PoolGuy.Mobile.CustomControls.DragAndDropListView;
using static PoolGuy.Mobile.Droid.CustomRenderer.DragAndDropListViewRenderer;

namespace PoolGuy.Mobile.Droid.CustomRenderer
{
    public class NativeDraggableListView : ListView
    {
        private bool isScrolling;
        private float lastY;
        private int scrollBy;
        private int startIndex = -1;
        private int endIndex = -1;
        public event EventHandler<DragAndDropEventArgs> ItemDroppedEvent;
        public Action<ListCount> GetListCount { get; set; }

        public NativeDraggableListView(Context context) : base(context)
        {
            Drag += OnDrag;
            SetOnScrollListener(new OnScrollListener(this));
        }

        private void OnDrag(object sender, DragEventArgs args)
        {
            ListView listView = sender as ListView;
            if (listView == null)
            {
                return;
            }

            switch (args.Event.Action)
            {
                case DragAction.Drop:
                    if (endIndex == -1)
                    {
                        ListCount listCount = new ListCount();
                        GetListCount?.Invoke(listCount);
                        endIndex = listView.PointToPosition((int)args.Event.GetX(), (int)args.Event.GetY()) - 1;
                        if (endIndex >= listCount.Count)
                        {
                            endIndex = listCount.Count - 1;
                        }
                    }

                    if (startIndex >= 0 && endIndex >= 0)
                    {
                        ItemDroppedEvent?.Invoke(this, new DragAndDropEventArgs(startIndex, endIndex));
                    }

                    startIndex = -1;
                    endIndex = -1;
                    break;
                case DragAction.Ended:
                    isScrolling = false;
                    break;
                case DragAction.Entered:
                    if (startIndex == -1)
                    {
                        startIndex = int.Parse(args.Event.ClipDescription.Label);
                        startIndex--;
                    }
                    break;
                case DragAction.Exited:
                    break;
                case DragAction.Location:
                    lastY = args.Event.GetY();
                    ScrollUpdate(lastY);
                    break;
                case DragAction.Started:
                    break;
                default:
                    break;
            }
        }

        private void ScrollUpdate(float y)
        {
            int height = Height;
            int scrollBorder = height / 3;
            int position = (int)y;

            scrollBy = 0;

            if (position < scrollBorder)
            {
                scrollBy = -(scrollBorder - position);
            }
            else if (position > height - scrollBorder)
            {
                scrollBy = position - (height - scrollBorder);
            }

            if (scrollBy == 0)
            {
                isScrolling = false;
                return;
            }

            // Math incoming: A quadratic function with some division eases to smooth out scroll speed
            scrollBy = (int)(Math.Pow(scrollBy / 7d, 2) / (scrollBorder / 10d)) * (scrollBy < 0 ? -1 : 1);

            if (isScrolling)
            {
                SmoothScrollBy(scrollBy, 0);
            }
            isScrolling = true;
        }

        private class OnScrollListener : Java.Lang.Object, IOnScrollListener
        {
            private readonly NativeDraggableListView super;
            private int prevFirstVisibleItem;
            private int prevLastVisibleItem;

            public OnScrollListener(NativeDraggableListView super)
            {
                this.super = super;
            }

            public void OnScroll(AbsListView view, int firstVisibleItem, int visibleItemCount, int totalItemCount)
            {
                int lastVisibleItem = firstVisibleItem + visibleItemCount - 1;

                Android.Views.View child = null;
                if (firstVisibleItem != prevFirstVisibleItem)
                {
                    child = super.GetChildAt(0);
                }

                if (prevLastVisibleItem != lastVisibleItem)
                {
                    child = super.GetChildAt(super.ChildCount - 1);
                }

                if (child != null)
                {
                    // Workaround to set newly scrolled items as valid drop targets
                    child.Visibility = ViewStates.Invisible;
                    child.Visibility = ViewStates.Visible;
                }

                prevLastVisibleItem = lastVisibleItem;
                prevFirstVisibleItem = firstVisibleItem;
            }

            public void OnScrollStateChanged(AbsListView view, ScrollState scrollState)
            {
                if (super.isScrolling)
                {
                    super.SmoothScrollBy(super.scrollBy, 0);
                }
            }
        }
    }
}