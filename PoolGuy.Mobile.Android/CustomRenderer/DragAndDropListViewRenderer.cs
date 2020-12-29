using Android.Content;
using Android.OS;
using Android.Widget;
using PoolGuy.Mobile.CustomControls;
using PoolGuy.Mobile.Droid.CustomRenderer;
using System.Collections;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using static PoolGuy.Mobile.CustomControls.DragAndDropListView;

[assembly: ExportRenderer(typeof(DragAndDropListView), typeof(DragAndDropListViewRenderer))]
namespace PoolGuy.Mobile.Droid.CustomRenderer
{
    public class DragAndDropListViewRenderer : ListViewRenderer
    {
        private IList Items;

        public DragAndDropListViewRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.ListView> e)
        {
            base.OnElementChanged(e);
            
            if (Control == null)
            {
                return;
            }

            if (e.OldElement != null)
            {
                Control.ItemClick -= Control_ItemClick;
                Control.ItemLongClick -= Control_ItemLongClick;
                ((NativeDraggableListView)Control).ItemDroppedEvent -= ScrollableListView_ItemDroppedEvent;
            }

            if (e.NewElement != null)
            {
                Items = e.NewElement.ItemsSource as IList;
                NativeDraggableListView scrollableListView = new NativeDraggableListView(Context)
                {
                    GetListCount = ((count) => GetListCount(count))
                };
                scrollableListView.ItemDroppedEvent += ScrollableListView_ItemDroppedEvent;
                scrollableListView.Adapter = Control.Adapter;

                SetNativeControl(scrollableListView);
                Control.ItemLongClick += Control_ItemLongClick;
                Control.ItemClick += Control_ItemClick;
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == "ItemsSource")
            {
                if (Element == null)
                {
                    Items = null;
                }
                else
                {
                    Items = Element.ItemsSource as IList;
                }
            }
        }

        private void ScrollableListView_ItemDroppedEvent(object sender, DragAndDropEventArgs e)
        {
            if (Element == null)
            {
                return;
            }

            if (((DragAndDropListView)Element).DragEventUsage != DragAndDropListView.eDragAndDropEventUsage.TriggerEventOnly)
            {
                object firstItem = Items[e.StartIndex];

                if (firstItem != null)
                {
                    Items.RemoveAt(e.StartIndex);
                    Items.Insert(e.EndIndex, firstItem);
                }
            }

            if (((DragAndDropListView)Element).DragEventUsage != DragAndDropListView.eDragAndDropEventUsage.None)
            {
                ((DragAndDropListView)Element).InvokeDragEnded(e.StartIndex, e.EndIndex);
            }
        }

        private void Control_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            {
                ClipData data = ClipData.NewPlainText(e.Position.ToString(), string.Empty);
                NativeDragShadowBuilder myShadownScreen = new NativeDragShadowBuilder(e.View);
                if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.N)
                {
                    e.View.StartDragAndDrop(data, myShadownScreen, null, 0);
                }
                else
                {
                    e.View.StartDrag(data, myShadownScreen, null, 0);
                }
            };
        }

        private void Control_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            ((DragAndDropListView)Element).SetSelectedItem(e.Position - 1);
        }

        public void GetListCount(ListCount listCount)
        {
            if (Items != null)
            {
                listCount.Count = Items.Count;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (Control != null)
            {
                Control.ItemClick -= Control_ItemClick;
                Control.ItemLongClick -= Control_ItemLongClick;
                ((NativeDraggableListView)Control).ItemDroppedEvent -= ScrollableListView_ItemDroppedEvent;
            }

            base.Dispose(disposing);
        }

        public class ListCount
        {
            public int Count { set; get; }
        }
    }
}