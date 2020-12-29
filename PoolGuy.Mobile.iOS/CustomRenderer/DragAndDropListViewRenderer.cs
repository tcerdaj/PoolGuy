using Foundation;
using PoolGuy.Mobile.CustomControls;
using PoolGuy.Mobile.iOS.CustomRenderer;
using System;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(DragAndDropListView), typeof(DragAndDropListViewRenderer))]
namespace PoolGuy.Mobile.iOS.CustomRenderer
{
    public class DragAndDropListViewRenderer : ListViewRenderer
    {
        private class ReorderableTableViewSource : UITableViewSource
        {
            public WeakReference<DragAndDropListView> View { get; set; }

            public UITableViewSource Source { get; set; }

            #region A replacement UITableViewSource which enables drag and drop to reorder rows

            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
            {
                return Source.GetCell(tableView, indexPath);
            }

            public override UITableViewCellEditingStyle EditingStyleForRow(UITableView tableView, NSIndexPath indexPath)
            {
                return UITableViewCellEditingStyle.None;
            }

            public override bool ShouldIndentWhileEditing(UITableView tableView, NSIndexPath indexPath)
            {
                return false;
            }

            public override nfloat GetHeightForHeader(UITableView tableView, nint section)
            {
                return Source.GetHeightForHeader(tableView, section);
            }

            public override UIView GetViewForHeader(UITableView tableView, nint section)
            {
                return Source.GetViewForHeader(tableView, section);
            }

            public override nint NumberOfSections(UITableView tableView)
            {
                return Source.NumberOfSections(tableView);
            }

            public override void RowDeselected(UITableView tableView, NSIndexPath indexPath)
            {
                Source.RowDeselected(tableView, indexPath);
            }

            public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
            {
                Source.RowSelected(tableView, indexPath);
            }

            public override nint RowsInSection(UITableView tableview, nint section)
            {
                return Source.RowsInSection(tableview, section);
            }

            public override string[] SectionIndexTitles(UITableView tableView)
            {
                return Source.SectionIndexTitles(tableView);
            }

            public override void MoveRow(UITableView tableView, NSIndexPath sourceIndexPath, NSIndexPath destinationIndexPath)
            {
                DragAndDropListView listView;
                View.TryGetTarget(out listView);

                if (listView != null)
                {
                    var move = listView.ItemsSource
                                        .GetType()
                                        .GetMethod("Move");

                    if (listView.DragEventUsage != DragAndDropListView.eDragAndDropEventUsage.TriggerEventOnly)
                    {
                        move?.Invoke(listView.ItemsSource, new object[] { sourceIndexPath.Row, destinationIndexPath.Row });

                    }
                    else
                    {
                        tableView.ReloadData();
                    }

                    if (listView.DragEventUsage != DragAndDropListView.eDragAndDropEventUsage.None)
                    {
                        listView.InvokeDragEnded(sourceIndexPath.Row, destinationIndexPath.Row);
                    }
                }
            }
        }

        #endregion

        private new DragAndDropListView Element => base.Element as DragAndDropListView;

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.ListView> args)
        {
            base.OnElementChanged(args);

            if (Control == null)
            {
                return;
            }

            if (args.OldElement != null)
            {
                ((DragAndDropListView)args.OldElement).ViewCellSizeChangedEvent -= DragAndDropListViewRenderer_ViewCellSizeChangedEvent;
            }

            if (args.NewElement != null)
            {
                ((DragAndDropListView)args.NewElement).ViewCellSizeChangedEvent += DragAndDropListViewRenderer_ViewCellSizeChangedEvent;
                Control.ScrollEnabled = true;
                Control.Editing = true;
                Control.AllowsSelectionDuringEditing = true;
                // Make row reorderable
                Control.Source = new ReorderableTableViewSource { View = new WeakReference<DragAndDropListView>(Element), Source = Control.Source };
            }
        }

        private void DragAndDropListViewRenderer_ViewCellSizeChangedEvent()
        {
            var tableView = Control as UITableView;
            if (tableView == null)
            {
                return;
            }

            tableView.BeginUpdates();
            tableView.EndUpdates();
        }
    }
}