using System;
using System.Windows.Forms;
using Dark_Souls_II_Save_Editor.SaveStuff;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using PositionChangedEventArgs = Telerik.WinControls.UI.Data.PositionChangedEventArgs;

namespace Dark_Souls_II_Save_Editor.Controls
{
    public partial class WeaponStatEditor : UserControl
    {
        public delegate void ItemChangedArg(ItemEntry item, SaveSlot slot, object sender);

        public WeaponStatEditor()
        {
            InitializeComponent();

            foreach (var v in Enum.GetValues(typeof (ItemEntry.ItemElement)))
                xelem.Items.Add(Enum.GetName(typeof (ItemEntry.ItemElement), v));
        }

        public ItemEntry Item { get; private set; }
        public SaveSlot Slot { get; set; }
        public event EventHandler xMouseLeave;
        public event EventHandler xMouseEnter;
        public event ItemChangedArg OnItemChanged;

        public void SetItem(ItemEntry item, SaveSlot slot)
        {
            Item = null;
            xquantity.Enabled = true;
            xquantitylabel.Text = "Quantity";
            xstatspanel.Visible = false;
            xammountpanel.Visible = true;
            xmove.Items.Clear();
            xcopy.Items.Clear();
            if (item != null)
            {
                var flag = item.Type.ItemType == ItemTypeInstance.ItemTypes.Item && item.CanHaveMoreThenOne;
                if (item.Type.ItemType == ItemTypeInstance.ItemTypes.Arrow ||
                    flag ||
                    item.Type.ItemType == ItemTypeInstance.ItemTypes.Shard ||
                    item.Type.ItemType == ItemTypeInstance.ItemTypes.Key ||
                    item.Type.ItemType == ItemTypeInstance.ItemTypes.Gesture ||
                    item.Type.ItemType == ItemTypeInstance.ItemTypes.None)
                {
                    xquantity.Maximum = 999;
                    if (item.Type.ItemType == ItemTypeInstance.ItemTypes.Key ||
                        item.Type.ItemType == ItemTypeInstance.ItemTypes.Gesture || !flag)
                    {
                        var items = slot.Inventory.GetItems(item);
                        xquantity.Value = items == null ? 0 : items.Length > 999 ? 999 : items.Length;
                    }

                    else if (item.Type.ItemType == ItemTypeInstance.ItemTypes.Spell)
                    {
                        xquantity.Maximum = 999;
                        xquantity.Value = item.SpellUses > xquantity.Maximum ? xquantity.Maximum : item.SpellUses;
                    }
                    else
                        xquantity.Value = item.Quantity > xquantity.Maximum ? xquantity.Maximum : item.Quantity;
                }
                else
                {
                    try
                    {
                        xquantity.Maximum = 999;
                        var items = slot.Inventory.GetItems(item);
                        xquantity.Value = items == null ? 0 : items.Length > 999 ? 999 : items.Length;
                        xstatspanel.Visible = item.Type.ItemType != ItemTypeInstance.ItemTypes.Spell &&
                                              item.Type.ItemType != ItemTypeInstance.ItemTypes.Item;
                        xelem.SelectedIndex = (int) ((byte) item.Element > xelem.Items.Count ? 0 : item.Element);
                        xdura.Value =
                            (decimal) item.Durability > xdura.Maximum ? xdura.Maximum : (decimal) item.Durability;
                        xupgr.Value =
                            item.WeaponUpgrade > (byte) xupgr.Maximum ? xupgr.Maximum : item.WeaponUpgrade;
                    }
                    catch
                    {
                    }
                }
                xduplicate.Visible = flag ||
                                     item.Type.ItemType == ItemTypeInstance.ItemTypes.Shard ||
                                     item.Type.ItemType == ItemTypeInstance.ItemTypes.Arrow;
                xdescription.Text = item.Description;
                xitembox.Text = "Move To " + (item.IsInItemBox ? "Inventory" : "ItemBox");
                ximage.Image = item.ItemImage;
                if (slot != null && slot.MainInstance.UsedSlots.Length > 1)
                {
                    foreach (var x in slot.MainInstance.UsedSlots)
                    {
                        if (x.SlotName == slot.SlotName)
                            continue;
                        var r1 = new RadMenuItem();
                        r1.Text = x.Name;
                        r1.Tag = x;
                        r1.Click += Move_Click;
                        r1.PopupDirection = RadDirection.Down;
                        xmove.Items.Add(r1);
                        r1 = new RadMenuItem();
                        r1.PopupDirection = RadDirection.Down;
                        r1.Text = x.Name;
                        r1.Tag = x;
                        r1.Click += Copy_Click;
                        xcopy.Items.Add(r1);
                    }
                    xmove.Update();
                    xcopy.Update();
                }
            }
            else
            {
                xitembox.Text = "Move To ";
                xstatspanel.Visible = false;
                xammountpanel.Visible = false;
                xdescription.Text = "";
                ximage.Image = null;
            }
            Item = item;
            Slot = slot;
        }

        private void Move_Click(object sender, EventArgs e)
        {
            if (Item == null)
                return;
            var ri = sender as RadItem;
            if (ri != null)
            {
                var save = ri.Tag as SaveSlot;
                if (save != null)
                {
                    try
                    {
                        save.AddItem(Item, Item.IsInItemBox, true, 1);
                        Slot.RemoveItem(Item, false, true);
                        RadMessageBox.Show(Item.Name + " has been moved to " + save.Name);
                        Item = null;
                        change();
                    }
                    catch (Exception ex)
                    {
                        RadMessageBox.Show(ex.Message, "Full", MessageBoxButtons.OK, RadMessageIcon.Exclamation);
                    }
                }
            }
        }

        private void Copy_Click(object sender, EventArgs e)
        {
            if (Item == null)
                return;
            var ri = sender as RadItem;
            if (ri != null)
            {
                var save = ri.Tag as SaveSlot;
                if (save != null)
                {
                    try
                    {
                        save.AddItem(Item, Item.IsInItemBox, true, 1);
                        RadMessageBox.Show(Item.Name + " has been copied to " + save.Name);
                    }
                    catch (Exception ex)
                    {
                        RadMessageBox.Show(ex.Message, "Full", MessageBoxButtons.OK, RadMessageIcon.Exclamation);
                    }
                }
            }
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            if (Item == null)
                return;
            xdura.Value = xdura.Maximum;
        }

        private void radButton4_Click(object sender, EventArgs e)
        {
            if (Item == null)
                return;
            xupgr.Value = xupgr.Maximum;
        }

        private void xelem_SelectedIndexChanged(object sender, PositionChangedEventArgs e)
        {
            if (Item == null)
                return;
            Item.Element = (ItemEntry.ItemElement) xelem.SelectedIndex;
        }

        private void xupgr_ValueChanged(object sender, EventArgs e)
        {
            if (Item == null)
                return;
            Item.WeaponUpgrade = (byte) xupgr.Value;
        }

        private void xdura_ValueChanged(object sender, EventArgs e)
        {
            if (Item == null)
                return;
            Item.Durability = (float) xdura.Value;
        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            if (Item == null)
                return;
            xquantity.Value = xquantity.Maximum;
        }

        private void xquantity_ValueChanged(object sender, ValueChangingEventArgs e)
        {
            if (Item == null)
                return;
            try
            {
                var flag = Item.Type.ItemType == ItemTypeInstance.ItemTypes.Item && Item.CanHaveMoreThenOne;
                if (Item.Type.ItemType == ItemTypeInstance.ItemTypes.Arrow ||
                    flag ||
                    Item.Type.ItemType == ItemTypeInstance.ItemTypes.Shard ||
                    Item.Type.ItemType == ItemTypeInstance.ItemTypes.Gesture ||
                    Item.Type.ItemType == ItemTypeInstance.ItemTypes.None)
                {
                    Item.Quantity = ushort.Parse(e.NewValue.ToString());
                }
                else
                {
                    var items = Slot.Inventory.GetItems(Item);
                    var count = int.Parse(e.NewValue.ToString());
                    if (items != null && items.Length > 0)
                    {
                        if (items.Length == count)
                            return;
                        var delete = items.Length > count;
                        if (delete)
                        {
                            for (var i = 0; i < items.Length - count; i++)
                                Slot.RemoveItem(Item, false, i == items.Length - count - 1);
                        }
                        else
                        {
                            if (items.Length >= 1 && !Slot.OverwriteItems)
                            {
                                e.Cancel = true;
                                return;
                            }
                            count -= items.Length;
                            if (Slot.Inventory.InventoryCount + count > 0x6FC)
                            {
                                throw new Exception("Unable to multiply " + Item.Name + " by " + count +
                                                    "... you have not enough space in your inventory");
                            }
                            Slot.AddItem(Item, Item.IsInItemBox, true, count);
                        }
                    }
                    else
                    {
                        if (Slot.Inventory.InventoryCount + count > 0x6FC)
                        {
                            throw new Exception("Unable to multiply " + Item.Name + " by " + count +
                                                "... you have not enough space in your inventory");
                        }
                        Slot.AddItem(Item, Item.IsInItemBox, true, count);
                    }
                }
                change();
            }
            catch (Exception ex)
            {
                e.Cancel = true;
                RadMessageBox.Show(ex.Message, "Full", MessageBoxButtons.OK, RadMessageIcon.Exclamation);
            }
        }

        private void xremove_Click(object sender, EventArgs e)
        {
            if (Item == null)
                return;
            var r = RadMessageBox.Show("Would you like to remove all duplicates of \"" + Item.Name + "\" as well?",
                "Remove", MessageBoxButtons.YesNoCancel, RadMessageIcon.Exclamation);
            if (r == DialogResult.Cancel)
                return;
            Slot.RemoveItem(Item, r == DialogResult.Yes, false);
            Item = null;
            change();
        }

        private void change()
        {
            if (Item != null)
                ximage.Image = Item.ItemImage;
            else
                ximage.Image = null;
            OnItemChanged?.Invoke(Item, Slot, Parent);
        }

        private void xstatspanel_MouseEnter(object sender, EventArgs e)
        {
            xMouseEnter?.Invoke(sender, e);
        }

        private void xstatspanel_MouseLeave(object sender, EventArgs e)
        {
            xMouseLeave?.Invoke(sender, e);
        }

        private void xitembox_Click(object sender, EventArgs e)
        {
            if (Item.IsInItemBox)
                Item.IsInItemBox = false;
            else
                Item.IsInItemBox = true;
            xitembox.Text = Item.IsInItemBox ? "Move To Inventory" : "Move To ItemBox";
            change();
        }

        private void xduplicate_Click(object sender, EventArgs e)
        {
            Slot.Inventory.CloneItem(Item);
            change();
        }
    }
}