
using System;
using System.Linq;
using Xamarin.Forms;

namespace PoolGuy.Mobile.CustomControls
{
    public class DragAndDropListView : ListView
    {
        public DragAndDropListView() : base(DragAndDropListViewCachingStrategy)
        {
        }

        public static ListViewCachingStrategy DragAndDropListViewCachingStrategy => Device.RuntimePlatform == Device.Android ? ListViewCachingStrategy.RecycleElement : ListViewCachingStrategy.RetainElement;

        public event Action ViewCellSizeChangedEvent;

        public void NotifyCellSizedChanged()
        {
            ViewCellSizeChangedEvent?.Invoke();
        }

        public eDragAndDropEventUsage DragEventUsage { get; set; } = eDragAndDropEventUsage.None;

        public event EventHandler<DragAndDropEventArgs> DragEnded;
        public void InvokeDragEnded(int startIndex, int endIndex)
        {
            DragEnded?.Invoke(this, new DragAndDropEventArgs(startIndex, endIndex));
        }

        public void SetSelectedItem(int index)
        {
            SelectedItem = ItemsSource.Cast<object>().ToList().ElementAt(index);
        }

        public class DragAndDropEventArgs : EventArgs
        {
            public DragAndDropEventArgs(int startIndex, int endIndex)
            {
                StartIndex = startIndex;
                EndIndex = endIndex;
            }

            public int StartIndex
            {
                get; private set;
            }

            public int EndIndex
            {
                get; private set;
            }
        }

        public enum eDragAndDropEventUsage
        {
            TriggerEventOnly,
            TriggerEventAndMoveItem,
            None
        }
    }
}