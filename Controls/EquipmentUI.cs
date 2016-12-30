using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Dark_Souls_II_Save_Editor.Forms;
using Dark_Souls_II_Save_Editor.SaveStuff;
using HavenInterface.Popup;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace Dark_Souls_II_Save_Editor.Controls
{
    public partial class EquipmentUI : UserControl
    {
        private Point _clickedon;
        private bool _isleftclick;
        public Popup _Popup;

        public EquipmentUI()
        {
            InitializeComponent();
        }

        public SaveSlot SlotInstance { get; private set; }

        public void LoadEquipments(SaveSlot slot)
        {
            SlotInstance = slot;
            slot.Equipments = new Equipment(slot, slot.SaveBlocks[0].GetStream(0x16c, false).ReadBytes(0xb0));
            var weapons = new ItemTypeInstance {StartID = 10, EndID = 159};
            var armors = new ItemTypeInstance {StartID = 160, EndID = 499};
            var rings = new ItemTypeInstance {StartID = 500, EndID = 599};
            var spells = new ItemTypeInstance {StartID = 800, EndID = 999};
            var belt = new ItemTypeInstance {StartID = 700, EndID = 709};
            var arrows = new ItemTypeInstance {StartID = 600, EndID = 699};
            var head = new ItemTypeInstance {StartID = 400, EndID = 409};
            var chest = new ItemTypeInstance {StartID = 410, EndID = 419};
            var hands = new ItemTypeInstance {StartID = 420, EndID = 429};
            var legs = new ItemTypeInstance {StartID = 430, EndID = 499};

            var x = SlotInstance.Equipments;
            var totalspells = x.AtuneSpells.Where(v => v != null && v.EntryIndex > 0).ToArray();
            var totalslots = totalspells.Sum(v => v.SpellParam == null ? 1 : v.SpellParam.SlotUsed);
            for (var i = 0; i < 14; i++)
            {
                var radTileElement = xspells.Items[i] as RadTileElement;
                if (radTileElement != null)
                {
                    radTileElement.Visibility = 14 - i > totalslots - totalspells.Length
                        ? ElementVisibility.Visible
                        : ElementVisibility.Hidden;
                    SetTile((RadTileElement) xspells.Items[i], x.AtuneSpells[i], i, spells, false);
                }
                //ItemEntry xempty = new ItemEntry(xinstance);
                //elements.Add(xempty);
                //SetTile(xspells.Items[count + i] as RadTileElement, xempty, count + i, xinstance, true);
                //WriteEquipIndex(xempty, count + i);
            }

            int[] values = {0, 2, 4, 6, 8, 1, 3, 5, 7, 9};
            for (var i = 0; i < 10; i++)
            {
                var v = x.BeltSlots[i];
                SetTile((RadTileElement) xbelt.Items[values[i]], v, values[i], belt, false);
            }

            SetTile((RadTileElement) xrings.Items[0], x.Rings[0], 0, rings, false);
            SetTile((RadTileElement) xrings.Items[1], x.Rings[2], 1, rings, false);
            SetTile((RadTileElement) xrings.Items[2], x.Rings[1], 2, rings, false);
            SetTile((RadTileElement) xrings.Items[3], x.Rings[3], 3, rings, false);

            SetTile((RadTileElement) xweapon.Items[0], x.RightWeapon1, 0, weapons, false);
            SetTile((RadTileElement) xweapon.Items[1], x.LeftWeapon1, 1, weapons, false);
            SetTile((RadTileElement) xweapon.Items[2], x.RightWeapon2, 2, weapons, false);
            SetTile((RadTileElement) xweapon.Items[3], x.LeftWeapon2, 3, weapons, false);
            SetTile((RadTileElement) xweapon.Items[4], x.RightWeapon3, 4, weapons, false);
            SetTile((RadTileElement) xweapon.Items[5], x.LeftWeapon3, 5, weapons, false);

            SetTile((RadTileElement) xarmor.Items[0], x.Head, 0, head, false);
            SetTile((RadTileElement) xarmor.Items[1], x.Chest, 1, chest, false);
            SetTile((RadTileElement) xarmor.Items[2], x.Hands, 2, hands, false);
            SetTile((RadTileElement) xarmor.Items[3], x.Legs, 3, legs, false);

            SetTile((RadTileElement) xarrows.Items[0], x.Arrow1, 0, arrows, false);
            SetTile((RadTileElement) xarrows.Items[2], x.Arrow2, 1, arrows, false);
            SetTile((RadTileElement) xarrows.Items[1], x.Bolt1, 2, arrows, false);
            SetTile((RadTileElement) xarrows.Items[3], x.Bolt2, 3, arrows, false);
        }

        private void SetTile(RadTileElement element, ItemEntry item, int index, ItemTypeInstance type, bool writeid)
        {
            //ItemEntry temp = item.Clone();
            var flag = item.IsDlc && !SlotInstance.AllowdDlcIndexes.Contains(item.DlcVersion);
            if (writeid)
                WriteEquipId(flag ? new ItemEntry(item.Type) : item, index);
            if (item.EntryIndex != 0 && !flag)
            {
                element.Tag = new object[] {item, type, index};
                element.AutoToolTip = true;
                if (item.Description == null)
                    element.ToolTipText = item.Name;
                else
                    element.ToolTipText = item.Name + "\n\n" + item.Description;
                element.Image = item.GetImage(true);
                element.ImageLayout = ImageLayout.Stretch;
            }
            else
            {
                element.Tag = new object[] {new ItemEntry(type), type, index};
                element.Image = null;
                element.ToolTipText = @"Empty Slot";
            }
            element.MouseDown -= element_MouseDown;
            element.MouseDown += element_MouseDown;
            element.Click -= LoadItemAdder;
            element.Click += LoadItemAdder;
        }

        private void element_MouseDown(object sender, MouseEventArgs e)
        {
            var element = sender as RadTileElement;
            if (element == null)
                return;
            if (e.Button == MouseButtons.Right)
            {
                _isleftclick = false;
                var instance = (element.Tag as object[])[1] as ItemTypeInstance;
                var item = (element.Tag as object[])[0] as ItemEntry;
                if (instance == null || item == null)
                    return;
                if (item.EntryIndex == 0)
                    return;
                var flag = instance.ItemType == ItemTypeInstance.ItemTypes.Item && item.CanHaveMoreThenOne;
                if (flag || instance.ItemType == ItemTypeInstance.ItemTypes.Arrow ||
                    instance.ItemType == ItemTypeInstance.ItemTypes.Shard)
                {
                    var x = new Set_Quantity(item, instance);
                    if (x.ShowDialog() == DialogResult.OK)
                    {
                        item.Quantity = (ushort) x.SelectedAmmount;
                        SetTile(element, item, (int) ((object[]) element.Tag)[2], instance, false);
                    }
                }
            }
            else if (e.Button == MouseButtons.Left)
            {
                _clickedon = e.Location;
                _isleftclick = true;
            }
        }

        private void LoadItemAdder(object sender, EventArgs e)
        {
            if (!_isleftclick)
                return;
            _isleftclick = false;
            var element = sender as RadTileElement;
            if (element == null)
                return;
            var objects1 = element.Tag as object[];
            if (objects1 != null)
            {
                var instance = objects1[1] as ItemTypeInstance;
                if (instance == null)
                    return;
                var objects = objects1;
                var item = objects[0] as ItemEntry;
                if (item == null)
                    return;
                var i = new ItemChooser(element, element.ElementTree.Control as RadPanorama, MainFile.DataBase.Items,
                    instance, item, SlotInstance);
                i.Dock = DockStyle.Fill;
                i.OnItemChoosed += i_OnItemChoosed;
                i.OnFocusLost += i_OnFocusLost;
                i.Width = Width;
                _Popup = new Popup(i);
            }
            _Popup.AutoClose = false;
            _Popup.FocusOnOpen = true;
            _Popup.BackColor = Color.Transparent;
            _Popup.ShowingAnimation = PopupAnimations.RightToLeft | PopupAnimations.Slide;
            _Popup.HidingAnimation = PopupAnimations.LeftToRight | PopupAnimations.Slide;
            _Popup.Show(PointToScreen(Location).X,
                PointToScreen(Location).Y);
        }

        private void i_OnFocusLost(object sender, EventArgs e)
        {
            if (_Popup != null && _Popup.Visible)
            {
                _Popup.Close();
            }
        }

        private void i_OnItemChoosed(RadTileElement element, RadPanorama rp, ItemChooser instance, ItemEntry item,
            DialogResult result)
        {
            try
            {
                i_OnFocusLost(null, null);
                var objects2 = element.Tag as object[];
                if (objects2 == null)
                    return;
                var xinstance = objects2[1] as ItemTypeInstance;
                if (instance == null)
                    return;
                var objects1 = objects2;
                {
                    var xitem = objects1[0] as ItemEntry;
                    if (xitem == null)
                        return;
                }
                var flag = item.IsDlc && !SlotInstance.AllowdDlcIndexes.Contains(item.DlcVersion);
                if (result == DialogResult.OK && !flag)
                {
                    if (instance.OldItem != null && instance.OldItem.EntryIndex > 0)
                    {
                        instance.OldItem.IsEquiped = false;
                        var values = instance.Element.Tag as object[];
                        if (values != null)
                            SetTile(instance.Element, new ItemEntry(instance.OldItem.Type), (int) values[2],
                                values[1] as ItemTypeInstance, true);
                    }
                    item.IsEquiped = item.EntryIndex != 0;
                    if (item.EntryIndex > 0)
                    {
                        item.IsInItemBox = false;
                        foreach (var radItem in rp.Items)
                        {
                            var x = (RadTileElement) radItem;
                            var values = x.Tag as object[];
                            if (values != null)
                            {
                                var j = values[0] as ItemEntry;
                                if (j != null && j.ID1 == item.ID1)
                                {
                                    j.IsEquiped = false;
                                    SetTile(x, new ItemEntry(j.Type), (int) values[2],
                                        values[1] as ItemTypeInstance, true);
                                }
                            }
                        }
                        var entry = SlotInstance.Inventory.GetItem(item, false);
                        if (entry == null)
                        {
                            try
                            {
                                if (item.IsDlc && !SlotInstance.AllowdDlcIndexes.Contains(item.DlcVersion))
                                    throw new Exception(
                                        "You have disabled '" + item.DlcName +
                                        "' Items!, and could therefore not add item : " +
                                        item.Name + "\n" +
                                        "You can Enable DLC Adding inside your profile page");
                                item = SlotInstance.AddItem(item, false, true, 1);
                            }
                            catch (Exception ex)
                            {
                                RadMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, RadMessageIcon.Exclamation);
                                return;
                            }
                        }
                        else
                        {
                            entry.IsEquiped = true;
                            entry.IsInItemBox = false;
                            item = entry;
                        }
                        if (item.Type.ItemType == ItemTypeInstance.ItemTypes.Spell)
                        {
                            var elements =
                                SlotInstance.Equipments.AtuneSpells.Where(
                                    t => t.ID1 != item.ID1 && instance.OldItem.ID1 != t.ID1 && t.EntryIndex > 0)
                                    .ToList();
                            var slotused = (byte) (item.SpellParam == null ? 1 : item.SpellParam.SlotUsed);
                            var count = elements.Sum(v => v.SpellParam == null ? 1 : v.SpellParam.SlotUsed);
                            if (instance.OldItem != null && (count + slotused > 14))
                            {
                                // item = SlotInstance.Inventory.GetItem(item);
                                item.IsEquiped = false;
                                RadMessageBox.Show(
                                    "You dont have enough free slots availible, " + item.Name + " requires " +
                                    slotused + " free slots", "Free Slot", MessageBoxButtons.OK,
                                    RadMessageIcon.Exclamation);
                            }
                            else
                            {
                                item.IsEquiped = true;
                                for (var x = 0; x < elements.Count; x++)
                                {
                                    SetTile(xspells.Items[x] as RadTileElement, elements[x], x, xinstance, true);
                                    //WriteEquipIndex(elements[x], x);
                                }

                                SetTile(xspells.Items[elements.Count] as RadTileElement, item, elements.Count, xinstance,
                                    true);
                                elements.Add(item);
                                count = elements.Sum(v => v.SpellParam == null ? 1 : v.SpellParam.SlotUsed);
                                for (var i = 0; i < 14; i++)
                                {
                                    var radTileElement = xspells.Items[i] as RadTileElement;
                                    if (radTileElement != null)
                                    {
                                        radTileElement.Visibility = 14 - i > count - elements.Count
                                            ? ElementVisibility.Visible
                                            : ElementVisibility.Hidden;
                                    }
                                    //ItemEntry xempty = new ItemEntry(xinstance);
                                    //elements.Add(xempty);
                                    //SetTile(xspells.Items[count + i] as RadTileElement, xempty, count + i, xinstance, true);
                                    //WriteEquipIndex(xempty, count + i);
                                }
                                for (var i = 0; i < 14 - elements.Count; i++)
                                    elements.Add(new ItemEntry(xinstance));
                                SlotInstance.Equipments.AtuneSpells = elements.ToArray();
                            }
                        }
                        else
                        {
                            var index = (int) ((object[]) element.Tag)[2];
                            item.IsInItemBox = false;
                            item.IsEquiped = true;
                            SetTile(element, item, index, xinstance, true);
                            // WriteEquipIndex(item, index);
                        }
                    }
                    else
                    {
                        if (instance.OldItem != null && xinstance != null &&
                            xinstance.ItemType == ItemTypeInstance.ItemTypes.Spell &&
                            instance.OldItem.SpellParam != null)
                        {
                            var id = instance.OldItem.ID1;
                            var elements =
                                SlotInstance.Equipments.AtuneSpells.Where(
                                    t =>
                                        t != null && t.EntryIndex > 0 && t.IsEquiped && t.ID1 != id && t.ID1 != item.ID1)
                                    .ToList();
                            var count =
                                elements.Sum(
                                    v => v.SpellParam == null ? 1 : v.SpellParam.SlotUsed);
                            var slots = elements.Count;
                            item.IsEquiped = false;
                            instance.OldItem.IsEquiped = false;
                            for (var i = 0; i < 14 - slots; i++)
                                elements.Add(new ItemEntry(xinstance));
                            SlotInstance.Equipments.AtuneSpells = elements.ToArray();
                            for (var i = 0; i < 14; i++)
                            {
                                var radTileElement = xspells.Items[i] as RadTileElement;
                                if (radTileElement != null)
                                {
                                    SetTile(radTileElement, elements[i], i, xinstance, true);
                                    radTileElement.Visibility = 14 - i > count - slots
                                        ? ElementVisibility.Visible
                                        : ElementVisibility.Hidden;
                                }
                            }
                        }
                        else
                        {
                            var index = (int) ((object[]) element.Tag)[2];
                            item.IsEquiped = false;
                            SetTile(element, item, index, xinstance, true);
                            //WriteEquipIndex(item, index);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RadMessageBox.Show(ex.Message, "Full", MessageBoxButtons.OK, RadMessageIcon.Exclamation);
            }
        }

        private void WriteEquipIndex(ItemEntry item, int index)
        {
            if (item.EquipIndexBaseOffset > -1)
            {
                var offset = 0x10004 + item.EquipIndexBaseOffset + index*2;
                var itemindex =
                    (short) (item.EntryIndex > 0 && item.IsEquiped ? SlotInstance.Inventory.GetItemIndex(item) : -1);
                SlotInstance.SaveBlocks[4].GetStream(offset, true).WriteInt16(itemindex);
            }
        }

        private void WriteEquipId(ItemEntry item, int index)
        {
            if (item.EquipIdBaseOffset > -1)
            {
                var offset = 0x16C + item.EquipIdBaseOffset + index*4;
                var id = -1;
                var val = 0xA7DD0C;
                if (item.EquipIdBaseOffset == 0)
                    id = 0x33E140;
                else if (item.EquipIdBaseOffset == 24)
                    id = val + index;
                SlotInstance.SaveBlocks[0].GetStream(offset, true)
                    .WriteInt32(item.EntryIndex > 0 && item.IsEquiped
                        ? item.EquipIdBaseOffset == 24 ? (int) MakeArmorId(item.ID1, false) : (int) item.ID1
                        : id);
            }
        }

        internal uint MakeArmorId(uint id, bool equiped)
        {
            var values = id.ToString().ToArray();
            if (equiped)
                values[0] = (char) ((byte) values[0] + 1);
            else
                values[0] = (char) ((byte) values[0] - 1);
            uint value = 0;
            var number = values.Aggregate("", (current, v) => current + v);
            if (uint.TryParse(number, out value))
                return value;
            return id;
        }
    }
}