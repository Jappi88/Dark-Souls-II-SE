using System.Collections.Generic;
using System.Linq;
using Dark_Souls_II_Save_Editor.SaveStuff;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace Dark_Souls_II_Save_Editor.Controls
{
    public class DragAndDropListview : RadListView
    {
        public delegate void ItemsDraged(ItemEntry[] items, object destination);

        public delegate void SectionDragHandler(ItemEntry[] items, RadTileElement section, RadPanorama container);

        public DragAndDropListview()
        {
            MultiSelect = true;

            var svc = ListViewElement.DragDropService;
            svc.PreviewDragStart += svc_PreviewDragStart;
            svc.PreviewDragDrop += svc_PreviewDragDrop;
            svc.PreviewDragOver += svc_PreviewDragOver;
        }

        public event ItemsDraged OnItemsDraged;
        public event SectionDragHandler OnSectionHover;
        public event SectionDragHandler OnSectionDrop;

        private void svc_PreviewDragStart(object sender, PreviewDragStartEventArgs e)
        {
            e.CanStart = true;
        }

        private void svc_PreviewDragOver(object sender, RadDragOverEventArgs e)
        {
            var rowElement = e.DragInstance as IconListViewVisualItem;
            if (rowElement == null || rowElement.ElementTree == null)
                return;
            var dragGrid = rowElement.ElementTree.Control as RadListView;
            if (e.DragInstance is IconListViewVisualItem)
                ListViewElement.EnsureItemVisible((e.DragInstance as IconListViewVisualItem).Data, true);
            if (e.HitTarget is IconListViewVisualItem &&
                ((e.HitTarget as IconListViewVisualItem).ElementTree.Control as RadListView).Name != Name)
            {
                var x = e.HitTarget as IconListViewVisualItem;
                var y = x.ElementTree.Control as RadListView;
                y.ListViewElement.ViewElement.DragHint = new RadImageShape {Image = x.Image};
                e.CanDrop = true;
            }
            else if (e.HitTarget is SimpleListViewVisualItem &&
                     ((e.HitTarget as SimpleListViewVisualItem).ElementTree.Control as RadListView).Name != Name)
            {
                e.CanDrop = true;
            }
            else if (e.HitTarget is IconListViewElement &&
                     ((e.HitTarget as IconListViewElement).ElementTree.Control as RadListView).Name != Name)
            {
                (e.HitTarget as IconListViewElement).DragHint = new RadImageShape();
                e.CanDrop = true;
            }
            else if (e.HitTarget is RadTileElement)
            {
                var des = e.HitTarget as RadTileElement;
                if (dragGrid != null && dragGrid.SelectedItems.Count > 0)
                {
                    var despan = des.ElementTree.Control as RadPanorama;
                    if (OnSectionHover != null && despan != null)
                    {
                        var items = new List<ItemEntry>();
                        items.AddRange(dragGrid.SelectedItems.Select(item => item.Tag as ItemEntry));
                        OnSectionHover(items.ToArray(), des, despan);
                    }
                    e.CanDrop = true;
                }
                else
                    e.CanDrop = false;
            }
            else e.CanDrop = false;
        }

        private void svc_PreviewDragDrop(object sender, RadDropEventArgs e)
        {
            var rowElement = e.DragInstance as IconListViewVisualItem;
            if (rowElement == null || rowElement.ElementTree == null)
                return;
            e.Handled = true;
            var dragGrid = rowElement.ElementTree.Control as RadListView;
            if (dragGrid == null)
                return;
            RadListView desGrid = null;
            if (e.HitTarget is IconListViewElement)
            {
                var second = e.HitTarget as IconListViewElement;
                desGrid = second.ElementTree.Control as RadListView;
            }
            else if (e.HitTarget is SimpleListViewVisualItem)
            {
                var des = e.HitTarget as SimpleListViewVisualItem;
                desGrid = des.ElementTree.Control as RadListView;
            }
            else if (e.HitTarget is IconListViewVisualItem)
            {
                var des = e.HitTarget as IconListViewVisualItem;
                desGrid = des.ElementTree.Control as RadListView;
            }
            else if (e.HitTarget is RadTileElement)
            {
                var des = e.HitTarget as RadTileElement;
                {
                    var despan = des.ElementTree.Control as RadPanorama;
                    if (OnSectionDrop != null && despan != null)
                    {
                        var items = new List<ItemEntry>();
                        items.AddRange(dragGrid.SelectedItems.Select(item => item.Tag as ItemEntry));
                        OnSectionDrop(items.ToArray(), des, despan);
                    }
                }
            }
            if (desGrid != null)
            {
                if (dragGrid.SelectedItems.Count > 0)
                {
                    if (OnItemsDraged != null)
                    {
                        var items = new List<ItemEntry>();
                        items.AddRange(dragGrid.SelectedItems.Select(item => item.Tag as ItemEntry));
                        OnItemsDraged(items.ToArray(), desGrid);
                    }
                }
            }
        }
    }
}