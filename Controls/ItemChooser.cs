using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Dark_Souls_II_Save_Editor.Forms;
using Dark_Souls_II_Save_Editor.Properties;
using Dark_Souls_II_Save_Editor.SaveStuff;
using Telerik.WinControls.UI;

namespace Dark_Souls_II_Save_Editor.Controls
{
    public partial class ItemChooser : UserControl
    {
        public delegate void ItemChoosed(
            RadTileElement element, RadPanorama rp, ItemChooser instance, ItemEntry item, DialogResult result);

        internal ItemTypeInstance _type;
        private bool listing;
        private bool rightclick;

        public ItemChooser(RadTileElement element, RadPanorama rp, ItemEntry[] dbitems, ItemTypeInstance type,
            ItemEntry old, SaveSlot slot)
        {
            InitializeComponent();
            _type = type;
            OldItem = old;
            Element = element;
            ElementContainer = rp;
            SlotInstance = slot;

            itemcontainer.LostFocus += radListView1_LostFocus;
            itemcontainer.MouseClick += (sender, e) =>
            {
                if (e.Button == MouseButtons.Right)
                    rightclick = true;
                else
                    rightclick = false;
            };
            ListItems(dbitems, type);
        }

        public ItemEntry[] Items { get; private set; }
        public DialogResult DialogResult { get; internal set; }
        public RadTileElement Element { get; internal set; }
        public RadPanorama ElementContainer { get; internal set; }
        public SaveSlot SlotInstance { get; internal set; }
        public ItemEntry SelectedItem { get; private set; }
        public ItemEntry OldItem { get; private set; }
        public event EventHandler OnFocusLost;
        public event ItemChoosed OnItemChoosed;

        private void radListView1_LostFocus(object sender, EventArgs e)
        {
            if (OnFocusLost != null && !listing)
            {
                OnFocusLost(sender, e);
            }
        }

        private void ListItems(ItemEntry[] dbitems, ItemTypeInstance type)
        {
            var x = new List<ItemEntry>();
            if (type.ItemType == ItemTypeInstance.ItemTypes.Weapon)
                x = dbitems.Where(t => t.Type != null && (t.Type.ItemType == type.ItemType ||
                                                          t.Type.ItemType == ItemTypeInstance.ItemTypes.Shield ||
                                                          t.Type.ItemType == ItemTypeInstance.ItemTypes.Bow ||
                                                          t.Type.ItemType == ItemTypeInstance.ItemTypes.Staff) &&
                                       !t.IsIgnored).ToList();
            else
            {
                var isarrow = ((int) ((object[]) Element.Tag)[2] < 2) &&
                              type.ItemType == ItemTypeInstance.ItemTypes.Arrow;
                if (type.ItemType == ItemTypeInstance.ItemTypes.Arrow)
                {
                    x = isarrow
                        ? dbitems.Where(
                            t =>
                                t.Type != null && t.Type.ItemType == type.ItemType && !t.IsIgnored &&
                                !t.xName.ToLower().Contains("bolt")).ToList()
                        : dbitems.Where(
                            t =>
                                t.Type != null && t.Type.ItemType == type.ItemType && !t.IsIgnored &&
                                t.xName.ToLower().Contains("bolt")).ToList();
                }
                else
                    x =
                        dbitems.Where(t => t.Type != null && t.Type.ItemType == type.ItemType && !t.IsIgnored).ToList();
            }
            x.Sort((t1, t2) => t1.Type.ItemType.CompareTo(t2.Type.ItemType));
            //int rows = 4;
            //if (Items.Length > 30 && Items.Length <= 50)
            //    this.Width = 200;
            //else if (Items.Length > 50 && Items.Length <= 120)
            //    this.Width = 400;
            //else if (Items.Length <= 30)
            //    this.Width = 100;
            //else
            //    this.Width = 600;
            Items = x.ToArray();
            Height = 25 + 5*100;
            itemcontainer.RowsCount = 5;
            ListItems("");
        }

        private void ListItems(string filter)
        {
            if (Items == null)
                return;
            listing = true;
            var lv = new RadTileElement
            {
                Tag = new ItemEntry(new ItemTypeInstance()),
                Image = Functions.DrawTextOnImage(Resources.box_empty_icon, "Empty Slot", 10, true, new Point(0, 70)),
                TextImageRelation = TextImageRelation.ImageAboveText,
                ToolTipText = @"Select Empty Slot"
            };
            lv.Click += lv_Click;
            itemcontainer.Items.Add(lv);
            foreach (var v in Items)
            {
                if (!string.IsNullOrEmpty(filter) && filter != "Finditems..")
                    if (!v.Name.ToLower().Replace(" ", "").Contains(filter.ToLower()))
                        continue;
                var img = v.GetImage(false);
                img = Functions.DrawTextOnImage(img, v.Name, 8, true, new Point(0, v.Name.Length > 14 ? 65 : 80));
                var inv = SlotInstance.Inventory.GetItem(v);
                if (inv != null && inv.IsEquiped)
                    img = SetEquiped(img);
                lv = new RadTileElement
                {
                    Tag = v,
                    Text = v.Name,
                    ImageLayout = ImageLayout.Stretch,
                    Image = img,
                    ToolTipText = v.Name + "\n\n" + v.Description
                };
                lv.Click += lv_Click;
                itemcontainer.Items.Add(lv);
            }
            listing = false;
        }

        private Image SetEquiped(Image a)
        {
            using (var g = Graphics.FromImage(a))
            {
                var x = Resources.section_image;
                x.MakeTransparent();
                g.DrawImage(x, new Rectangle(new Point(0, 0), new Size(24, 24)));
                g.Flush();
            }
            return a;
        }

        private void lv_Click(object sender, EventArgs e)
        {
            if (rightclick)
            {
                Focus();
            }
            else
            {
                var rt = sender as RadTileElement;
                if (rt != null)
                {
                    SelectedItem = rt.Tag as ItemEntry;
                    if (SelectedItem != null && SelectedItem.EntryIndex > 1)
                    {
                        var existed = SlotInstance.Inventory.GetItem(SelectedItem);
                        if (existed != null && existed.EntryIndex > 0)
                            SelectedItem = existed;
                        var hasdata = SelectedItem.Data != null;
                        if (!hasdata)
                            SelectedItem.GenerateData(false);
                        var flag = SelectedItem.Type.ItemType == ItemTypeInstance.ItemTypes.Item &&
                                   SelectedItem.CanHaveMoreThenOne;
                        if (flag ||
                            _type.ItemType == ItemTypeInstance.ItemTypes.Arrow
                            || _type.ItemType == ItemTypeInstance.ItemTypes.Shard)
                        {
                            var x = new Set_Quantity(SelectedItem.Clone(), _type);
                            if (x.ShowDialog() == DialogResult.OK)
                            {
                                SelectedItem.Quantity = (ushort) x.SelectedAmmount;
                                DialogResult = DialogResult.OK;
                            }
                            else if (!hasdata)
                            {
                                SelectedItem.Data = null;
                                return;
                            }
                        }
                        else
                            DialogResult = DialogResult.OK;
                    }
                    else if (SelectedItem != null && SelectedItem.EntryIndex == 1)
                        DialogResult = DialogResult.Cancel;
                    else
                        DialogResult = DialogResult.OK;
                    OnItemChoosed?.Invoke(Element, ElementContainer, this, SelectedItem, DialogResult);
                }
            }
        }

        private void itemcontainer_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Focus();
            }
        }
    }
}