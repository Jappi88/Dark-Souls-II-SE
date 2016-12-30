using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Dark_Souls_II_Save_Editor.Forms;
using Dark_Souls_II_Save_Editor.Properties;
using Dark_Souls_II_Save_Editor.SaveStuff;
using Telerik.WinControls;
using Telerik.WinControls.Enumerations;
using Telerik.WinControls.UI;

namespace Dark_Souls_II_Save_Editor.Controls
{
    public partial class InventoryViewer : UserControl
    {
        private bool _expanded;

        public InventoryViewer()
        {
            InitializeComponent();

            xitemcontainer.ListViewElement.ViewElement.Orientation = Orientation.Vertical;
            xinventorycontainer.ListViewElement.ViewElement.Orientation = Orientation.Vertical;
            foreach (var v in Enum.GetValues(typeof (ItemEntry.ItemElement)))
                xelements.Items.Add(Enum.GetName(typeof (ItemEntry.ItemElement), v));
            xelements.SelectedIndex = 0;
            WeaponInfoMenuExpand(false);
            //AsignShortCuts();
        }

        public SaveSlot SelectedSlot { get; private set; }
        public ItemTypeInstance[] ItemTypes { get; private set; }
        //internal Popup xpop;

        public bool IsItemBox
        {
            get { return SelectedSlot.Inventory.IsItemBox; }
            set { SelectedSlot.Inventory.IsItemBox = value; }
        }

        public ItemEntry SetItemDefault(ItemEntry item)
        {
            var xent = item;
            if (xent.Data == null)
                xent.GenerateData(item.IsInItemBox);
            if (item.Type.ItemType == ItemTypeInstance.ItemTypes.Spell)
            {
                if (xdefault1.Checked)
                {
                    xent.SpellUses = (byte) xspells.Value;
                }
                else
                    xent.SpellUses =
                        (byte)
                            (item.SpellParam != null
                                ? item.SpellParam.SpellUsesForEachLevel[item.SpellParam.SpellUsesForEachLevel.Length - 1
                                    ].SpellUses
                                : 99);
            }
            else if (item.Type.ItemType == ItemTypeInstance.ItemTypes.Shard ||
                     item.Type.ItemType == ItemTypeInstance.ItemTypes.Item ||
                     item.Type.ItemType == ItemTypeInstance.ItemTypes.Arrow)
            {
                if (item.CanHaveMoreThenOne)
                {
                    if (xdefault2.Checked)
                    {
                        xent.Quantity = (ushort) xquantity.Value;
                    }
                    else
                        xent.Quantity = 1;
                }
                else
                {
                    xent.Quantity = 1;
                }
            }
            else if (item.Type.ItemType != ItemTypeInstance.ItemTypes.Gesture &&
                     item.Type.ItemType != ItemTypeInstance.ItemTypes.Key &&
                     item.Type.ItemType != ItemTypeInstance.ItemTypes.Item &&
                     item.Type.ItemType != ItemTypeInstance.ItemTypes.None)
            {
                xent.Durability = xdefault3.Checked ? (float) xdurability.Value : 100.00f;
                xent.WeaponUpgrade = (byte) (xdefault3.Checked ? xupgrade.Value : 0);
                xent.Element =
                    (ItemEntry.ItemElement)
                        (byte) (xdefault3.Checked ? xelements.SelectedIndex > -1 ? xelements.SelectedIndex : 0 : 0);
            }
            return xent;
        }

        private void AsignShortCuts()
        {
            xaddallitems.Shortcuts.Add(new RadShortcut(Keys.Control, Keys.A));
            xaddallselected.Shortcuts.Add(new RadShortcut(Keys.Control, Keys.Alt, Keys.A));
            xswitchitembox.Shortcuts.Add(new RadShortcut(Keys.None, Keys.Space));
            xremoveallselected.Shortcuts.Add(new RadShortcut(Keys.None, Keys.Delete));
            xremovealldlc.Shortcuts.Add(new RadShortcut(Keys.Control, Keys.Delete));
            xremovesection.Shortcuts.Add(new RadShortcut(Keys.Alt, Keys.Delete));
            xremoveallduplicates.Shortcuts.Add(new RadShortcut(Keys.Shift, Keys.Delete));
            xwipeinventory.Shortcuts.Add(new RadShortcut(Keys.Control, Keys.Shift, Keys.Delete));

            xaddalldlc.Shortcuts.Add(new RadShortcut(Keys.Alt, Keys.A, Keys.D));
            xsetvieweddefault.Shortcuts.Add(new RadShortcut(Keys.Alt, Keys.S));
            xsetalldefault.Shortcuts.Add(new RadShortcut(Keys.Alt, Keys.A, Keys.S));
            xensureeveryitem.Shortcuts.Add(new RadShortcut(Keys.Alt, Keys.Z));
        }

        public void LoadItems(SaveSlot slot, ItemTypeInstance[] xinstances)
        {
            SelectedSlot = slot;
            SelectedSlot.OnItemsChanged -= SelectedSlot_OnItemsChanged;
            SelectedSlot.OnItemsChanged += SelectedSlot_OnItemsChanged;
            IsItemBox = false;
            xdbname.Text = @"DataBase Version " + MainFile.DataBase.Version + @"[Total Of : " +
                           MainFile.DataBase.Items.Length + @" Items]";
            var instances = new List<ItemTypeInstance>();
            var type = xinstances.FirstOrDefault(y => y.ItemType == ItemTypeInstance.ItemTypes.Item);
            instances.Add(type);
            instances.AddRange(
                xinstances.Where(v => v.ItemType != ItemTypeInstance.ItemTypes.Item)
                    .Where(v => v.StartID < 1000 && (v.StartID < 20 || v.StartID > 120) && v.StartID != 160));
            xtypecontainer.Items.Clear();
            foreach (var v in instances)
            {
                if (v == null)
                    continue;
                var element = new RadTileElement();
                element.Name = v.ItemType.ToString();
                element.Click += Relist;
                element.Image = v.Image;
                element.Tag = v;
                element.ToolTipText = v.ItemType.ToString();
                xtypecontainer.Items.Add(element);
            }

            if (instances.Count > 0)
                xtypecontainer.Items[0].PerformClick();
        }

        private void SelectedSlot_OnItemsChanged(SaveSlot slot)
        {
            slot.Inventory.OnInventoryChanged();
        }

        private void Relist(object sender, EventArgs e)
        {
            var item = sender as RadTileElement;
            if (item == null)
                return;
            var type = item.Tag as ItemTypeInstance;
            if (type == null)
                return;
            item.SetSelected(xtypecontainer, Color.Red);
            ListItems(type, xsearchtext1.Text.Replace(" ", ""), xsearchtext2.Text.Replace(" ", ""));
        }

        public void Relist()
        {
            var i = (RadTileElement) xtypecontainer.Items.FirstOrDefault(t => t.BackColor == Color.Red);
            if (i != null)
                Relist(i, EventArgs.Empty);
            else if (xtypecontainer.Items.Count > 0)
                xtypecontainer.Items[0].PerformClick();
        }

        private void ListItems(ItemTypeInstance instance, string filter1, string filter2)
        {
            if (SelectedSlot == null)
                return;
            if (filter1 != null)
            {
                var dbitems =
                    MainFile.DataBase.Items.Where(
                        t => t.EntryIndex >= instance.StartID && t.EntryIndex <= instance.EndID && !t.IsIgnored)
                        .ToArray();
                xdbname.Text = "DataBase Items [Total Of : " + MainFile.DataBase.Items.Length + "]";
                xaddallitems.Text = "Add All " + instance.ItemType + " (" + dbitems.Length + ")";
                xitemcontainer.SelectedItem = null;
                xitemcontainer.BeginUpdate();
                xitemcontainer.Items.Clear();
                if (dbitems.Length > 0)
                {
                    foreach (var i in dbitems)
                    {
                        if (!string.IsNullOrEmpty(filter1) && filter1 != "Finditems..")
                            if (!i.Name.ToLower().Replace(" ", "").Contains(filter1.ToLower()))
                                continue;
                        var lv = new ListViewDataItem();
                        lv.Image = i.GetImage(false);
                        lv.ImageAlignment = ContentAlignment.MiddleLeft;
                        lv.Tag = i;
                        lv.Font = new Font(Font, FontStyle.Bold);
                        lv.Text = i.Name;
                        lv.TextImageRelation = TextImageRelation.ImageAboveText;
                        xitemcontainer.Items.Add(lv);
                    }
                }
                xitemcontainer.EndUpdate();
            }
            if (string.IsNullOrEmpty(filter2) && filter2 != "Finditems..")
                xinventorycontainer.Focus();
            var items = SelectedSlot.Inventory.GetItems(instance.ItemType);
            var flag = weaponStatEditor1.Item != null &&
                       items.FirstOrDefault(x => x.ID1 == weaponStatEditor1.Item.ID1 && !x.IsIgnored) != null;
            if (!flag)
            {
                xweaponinfobutton.Text = "";
                _expanded = false;
                WeaponInfoMenuExpand(_expanded);
            }
            xinname.Text = (IsItemBox ? "ItemBox" : "Inventory") + " Items [Total Items : " +
                           SelectedSlot.Inventory.InventoryCount + "/" + 0x6FC + " | Viewed " +
                           instance.ItemType + " : " + items.Length + "]";
            xinventorycontainer.BeginUpdate();
            xinventorycontainer.Items.Clear();
            if (items.Length > 0)
            {
                ListViewDataItem selected = null;
                foreach (var i in items)
                {
                    if (i.IsDlc && !SelectedSlot.AllowdDlcIndexes.Contains(i.DlcVersion))
                        continue;
                    if (!string.IsNullOrEmpty(filter2) && filter2 != "Finditems..")
                        if (!i.Name.ToLower().Replace(" ", "").Contains(filter2.ToLower()))
                            continue;
                    var lv = new ListViewDataItem();
                    lv.Image = i.ItemImage;
                    lv.ImageAlignment = ContentAlignment.MiddleLeft;
                    lv.Tag = i;
                    lv.Font = new Font(Font, FontStyle.Bold);
                    lv.Text = i.Name;
                    lv.TextImageRelation = TextImageRelation.ImageAboveText;
                    xinventorycontainer.Items.Add(lv);
                    if (flag && selected == null)
                        if (i.ID1 == weaponStatEditor1.Item.ID1)
                            selected = lv;
                }
                xinventorycontainer.SelectedItem = selected;
            }
            xinventorycontainer.EndUpdate();
            xinventorycontainer.Tag = instance;
            //if (items.Length > 0)
            //{
            //    xremoveall.Text = "Remove All " + instance.ItemType.ToString() + "(s) (" + items.Length + ")";
            //    xmaxall.Text = "Set All " + instance.ItemType.ToString() + "(s) Default Stat(" + items.Length + ")";
            //}
            //else
            //{
            //    xremoveall.Text = "Remove All " + instance.ItemType.ToString();
            //    xmaxall.Text = "Set All " + instance.ItemType.ToString();
            //}
            //if (xinventorycontainer.SelectedItems.Count == 0)
            //{
            //    xremoveselected.Text = "Remove Selected Items";
            //}
            //else
            //{
            //    xremoveselected.Text = "Remove Selected Items (" + xinventorycontainer.SelectedItems.Count + ")";
            //}
        }

        private void xitemcontainer_OnItemsDraged(ItemEntry[] items, object destination)
        {
            var rv = destination as RadListView;
            if (rv == null || SelectedSlot == null)
                return;
            if (rv.Name == xinventorycontainer.Name && items.Length > 0)
            {
                try
                {
                    var county = 0;
                    foreach (var ent in items)
                    {
                        var xent = ent.Clone();
                        SetItemDefault(xent);
                        SelectedSlot.AddItem(xent, IsItemBox, county == items.Length - 1, 1);
                        xent.IsInItemBox = IsItemBox;
                        county++;
                    }
                }
                catch (Exception ex)
                {
                    RadMessageBox.Show(ex.Message, "Full", MessageBoxButtons.OK, RadMessageIcon.Exclamation);
                }
                foreach (var v in xinventorycontainer.Items)
                {
                    if (v.Tag is ItemEntry)
                    {
                        foreach (var t in items.Where(t => (v.Tag as ItemEntry).ID1 == t.ID1))
                        {
                            xinventorycontainer.SelectedItem = v;
                            xinventorycontainer.ListViewElement.EnsureItemVisible(v);
                        }
                    }
                }
                var i = (RadTileElement) xtypecontainer.Items.FirstOrDefault(t => t.BackColor == Color.Red);
                if (i != null)
                {
                    var type = i.Tag as ItemTypeInstance;
                    if (type != null)
                        ListItems(type, null, xsearchtext2.Text.Replace(" ", ""));
                }
            }
        }

        private void xinventorycontainer_ToolTipTextNeeded(object sender, ToolTipTextNeededEventArgs e)
        {
            try
            {
                var lvi = sender as IconListViewVisualItem;
                if (lvi == null)
                    return;
                var lv = lvi.Data;
                if (lv != null)
                {
                    var ent = lv.Tag as ItemEntry;
                    e.ToolTip.UseAnimation = true;
                    e.ToolTip.ReshowDelay = 0;
                    e.ToolTip.InitialDelay = 0;
                    e.ToolTip.StripAmpersands = true;
                    e.ToolTip.ToolTipTitle = ent.Name;
                    e.ToolTip.AutoPopDelay = 10000;
                    e.ToolTipText = ent.Description;
                }
            }
            catch
            {
            }
        }

        private void xsearchtext_Enter(object sender, EventArgs e)
        {
            var t = sender as RadTextBox;
            if (sender != null)
            {
                if (t.Text.ToLower().Replace(" ", "") == "finditems..")
                {
                    t.Text = "";
                }
            }
        }

        private void xsearchtext_Leave(object sender, EventArgs e)
        {
            var t = sender as RadTextBox;
            if (sender != null)
            {
                if (t.Text.Replace(" ", "") == "")
                {
                    t.Text = "Find items..";
                }
            }
        }

        private void xinventorycontainer_SelectedItemsChanged(object sender, EventArgs e)
        {
            if (xinventorycontainer.SelectedItems.Count == 0)
            {
                xremoveallselected.Text = @"All Selected Items";
                xweaponinfobutton.Text = "";
                _expanded = false;
            }
            else if (xinventorycontainer.SelectedItems.Count == 1)
            {
                var entry = xinventorycontainer.SelectedItems[0].Tag as ItemEntry;
                if (entry == null)
                    return;
                xremoveallselected.Text = "Remove " + entry.Name;
                xweaponinfobutton.Text = entry.Name;
                weaponStatEditor1.SetItem(entry, SelectedSlot);
                _expanded = xautoopen.ToggleState == ToggleState.On;
            }
            else
            {
                xremoveallselected.Text = "All Selected Items (" + xinventorycontainer.SelectedItems.Count + ")";
                var itemEntry = xinventorycontainer.SelectedItems[0].Tag as ItemEntry;
                if (itemEntry == null)
                    return;
                xweaponinfobutton.Text = itemEntry.Name;
                weaponStatEditor1.SetItem(itemEntry, SelectedSlot);
                _expanded = xautoopen.ToggleState == ToggleState.On;
            }
            WeaponInfoMenuExpand(_expanded);
        }

        private void SetDefaultValues(int index)
        {
            switch (index)
            {
                case 1:
                    if (!xdefault1.Checked)
                        xspells.Value = 1;
                    break;
                case 2:
                    if (!xdefault2.Checked)
                        xquantity.Value = 1;
                    break;
                case 3:
                    if (!xdefault3.Checked)
                    {
                        xdurability.Value = 100;
                        xupgrade.Value = 0;
                        xelements.SelectedIndex = 0;
                    }
                    break;
            }
        }

        private void xitemcontainer_SelectedItemChanged(object sender, EventArgs e)
        {
            for (var i = 1; i < 4; i++)
                SetDefaultValues(i);

            if (xitemcontainer.SelectedItems.Count == 0)
            {
                xaddallselected.Text = @"Add All Selected";
            }
            else if (xitemcontainer.SelectedItems.Count == 1)
            {
                var entry = xitemcontainer.SelectedItems[0].Tag as ItemEntry;
                if (entry == null)
                    return;
                xaddallselected.Text = @"Add " + entry.Name;
            }
            else
            {
                xaddallselected.Text = @"Add All Selected (" + xitemcontainer.SelectedItems.Count + ")";
            }
        }

        private void radButton5_Click(object sender, EventArgs e)
        {
            SetDefaultValues(1);
        }

        private void radButton4_Click(object sender, EventArgs e)
        {
            SetDefaultValues(2);
        }

        private void radButton3_Click(object sender, EventArgs e)
        {
            SetDefaultValues(3);
        }

        private void radButton8_Click(object sender, EventArgs e)
        {
            xspells.Value = 99;
        }

        private void radButton6_Click(object sender, EventArgs e)
        {
            xquantity.Value = 999;
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            xdurability.Value = 999;
        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            xupgrade.Value = 10;
        }

        private void xsearchtext2_TextChanged(object sender, EventArgs e)
        {
            var re = (RadTileElement) xtypecontainer.Items.FirstOrDefault(t => t.BackColor == Color.Red);
            if (re == null)
                return;
            var instance = re.Tag as ItemTypeInstance;
            if (instance != null)
            {
                ListItems(instance, xsearchtext1.Text.Replace(" ", ""), xsearchtext2.Text.Replace(" ", ""));
            }
        }

        private void xremoveselected_ToolTipTextNeeded(object sender, ToolTipTextNeededEventArgs e)
        {
            var rb = sender as RadButtonElement;
            if (rb != null)
                e.ToolTipText = rb.Text;
        }

        private void xinventorycontainer_ItemMouseDoubleClick(object sender, ListViewItemEventArgs e)
        {
            if (e.Item != null)
            {
                var item = e.Item.Tag as ItemEntry;
                if (item != null)
                {
                    var t = new SetWeaponInfo(item, SelectedSlot);
                    t.weaponStatEditor1.OnItemChanged += weaponStatEditor1_OnItemChanged;
                    t.ShowDialog();
                }
            }
        }

        //private void radCheckBox1_ToggleStateChanged(object sender, StateChangedEventArgs args)
        //{
        //    SelectedSlot.OverwriteItems = args.ToggleState != ToggleState.On;
        //    if (args.ToggleState == ToggleState.On)
        //    {
        //        var result = RadMessageBox.Show("Would you like to remove all duplicate items from your inventory?",
        //            "Remove Duplicates", MessageBoxButtons.YesNoCancel, RadMessageIcon.Question);
        //        switch (result)
        //        {
        //            case DialogResult.Yes:
        //                int count = SelectedSlot.Inventory.RemoveDuplicates();
        //                RadMessageBox.Show("Total of " + count + " duplicate(s) have been removed!",
        //                    "Duplicates Removed", MessageBoxButtons.OK, RadMessageIcon.Info);
        //                break;
        //            case DialogResult.Cancel:
        //                xpreventdup.ToggleState = ToggleState.Off;
        //                break;
        //        }
        //    }
        //}

        //private void xpreventdlc_ToggleStateChanged(object sender, StateChangedEventArgs args)
        //{
        //    SelectedSlot.AllowDlc = args.ToggleState != ToggleState.On;
        //    if (args.ToggleState == ToggleState.On)
        //    {
        //        var result = RadMessageBox.Show("Would you like to remove all DLC items from your inventory?",
        //            "Remove DLC", MessageBoxButtons.YesNoCancel, RadMessageIcon.Question);
        //        switch (result)
        //        {
        //            case DialogResult.Yes:
        //                int count = SelectedSlot.Inventory.RemoveAllDlcItems();
        //                RadMessageBox.Show("Total of " + count + " Dlc item(s) have been removed!",
        //                    "DLC Removed", MessageBoxButtons.OK, RadMessageIcon.Info);
        //                break;
        //            case DialogResult.Cancel:
        //                xpreventdup.ToggleState = ToggleState.Off;
        //                break;
        //        }
        //    }
        //}

        private void xremovesection_Click(object sender, EventArgs e)
        {
            if (
                RadMessageBox.Show(
                    "Are you sure you want to remove all  " + xinventorycontainer.SelectedItems.Count +
                    " viewed items?\nThe Items will be gone for good!\nDo it Anyway?",
                    "Remove Viewed", MessageBoxButtons.YesNo, RadMessageIcon.Exclamation) == DialogResult.Yes)
            {
                if (xinventorycontainer.Items.Count == 0)
                    return;
                foreach (var item in xinventorycontainer.Items)
                {
                    SelectedSlot.RemoveItem(item.Tag as ItemEntry, false, false);
                }
                SelectedSlot.Inventory.OnInventoryChanged();
            }
        }

        private void xremoveallselected_Click(object sender, EventArgs e)
        {
            if (xinventorycontainer.SelectedItems.Count == 0)
                return;
            var itemEntry = xinventorycontainer.SelectedItems[0].Tag as ItemEntry;
            if (itemEntry != null)
            {
                var message = xinventorycontainer.SelectedItems.Count == 1
                    ? "Are you sure you want to remove " + itemEntry.Name +
                      "?"
                    : "Are you sure you want to remove the " + xinventorycontainer.SelectedItems.Count +
                      " Selected Items?";
                if (
                    RadMessageBox.Show(
                        message + "\nThe Items will be gone for good!\nDo it Anyway?",
                        "Remove Selected", MessageBoxButtons.YesNo, RadMessageIcon.Exclamation) == DialogResult.Yes)
                {
                    foreach (var item in xinventorycontainer.SelectedItems)
                    {
                        SelectedSlot.RemoveItem(item.Tag as ItemEntry, false, false);
                    }
                    SelectedSlot.Inventory.OnInventoryChanged();
                }
            }
        }

        private void xaddalldlc_Click(object sender, EventArgs e)
        {
            if (SelectedSlot.AllowdDlcIndexes.Length == 0)
            {
                RadMessageBox.Show("You have disabled DLC Items, allow dlc item first then try again.", "DLC Blocked",
                    MessageBoxButtons.OK, RadMessageIcon.Exclamation);
            }
            else
            {
                var count = SelectedSlot.Inventory.EnsureEveryDlc();
                RadMessageBox.Show("Total of " + count + " DLC Item(s) have been Added!", "DLC Added",
                    MessageBoxButtons.OK,
                    RadMessageIcon.Info);
            }
        }

        private void xremovealldlc_Click(object sender, EventArgs e)
        {
            if (
                RadMessageBox.Show(
                    "Are you sure you want to remove all DLC Items from your inventory?\nAll Items will be gone for good!\nDo it Anyway?",
                    "Remove All DLC", MessageBoxButtons.YesNo, RadMessageIcon.Exclamation) == DialogResult.Yes)
            {
                var count = SelectedSlot.Inventory.RemoveAllDlcItems();
                RadMessageBox.Show("Total of " + count + " DLC Item(s) have been removed!", "DLC Removed",
                    MessageBoxButtons.OK,
                    RadMessageIcon.Info);
            }
        }

        private void xremoveallduplicates_Click(object sender, EventArgs e)
        {
            if (
                RadMessageBox.Show(
                    "Are you sure you want to remove all duplicates from your inventory?\nAll Items will be gone for good!\nDo it Anyway?",
                    "Remove Duplicates", MessageBoxButtons.YesNo, RadMessageIcon.Exclamation) == DialogResult.Yes)
            {
                var count = SelectedSlot.Inventory.RemoveDuplicates();
                RadMessageBox.Show("Total of " + count + " Duplicate Item(s) have been removed!", "Duplicates Removed",
                    MessageBoxButtons.OK,
                    RadMessageIcon.Info);
            }
        }

        private void xwipeinventory_Click(object sender, EventArgs e)
        {
            if (
                RadMessageBox.Show(
                    "Are you sure you want to Wipe Out your Inventory?\nAll Items will be gone for good!\nDo it Anyway?",
                    "Wipe out Inventory", MessageBoxButtons.YesNo, RadMessageIcon.Exclamation) == DialogResult.Yes)
                SelectedSlot.Inventory.WipeOutInventory();
        }

        private void radMenuItem2_Click(object sender, EventArgs e)
        {
            if (
                RadMessageBox.Show(
                    "Are you sure you want set all viewed items to your preset default stats?",
                    "Set Viewed Default Stats", MessageBoxButtons.YesNo, RadMessageIcon.Exclamation) == DialogResult.Yes)
            {
                if (xinventorycontainer.Items.Count == 0)
                    return;
                foreach (var item in xinventorycontainer.Items)
                {
                    SetItemDefault(item.Tag as ItemEntry);
                }
                SelectedSlot.Inventory.OnInventoryChanged();
            }
        }

        private void xsetalldefault_Click(object sender, EventArgs e)
        {
            if (
                RadMessageBox.Show(
                    "Are you sure you want set all items to your preset default stats?",
                    "Set All Items Default Stats", MessageBoxButtons.YesNo, RadMessageIcon.Exclamation) ==
                DialogResult.Yes)
            {
                foreach (var item in SelectedSlot.Inventory.Items)
                {
                    SetItemDefault(item);
                }
                SelectedSlot.Inventory.OnInventoryChanged();
            }
        }

        private void xensureeveryitem_Click(object sender, EventArgs e)
        {
            if (
                RadMessageBox.Show(
                    "You are about to gain every item you dont own yet, are you sure you want to proceed?",
                    "Own All Items", MessageBoxButtons.YesNo, RadMessageIcon.Exclamation) == DialogResult.Yes)
            {
                var count = SelectedSlot.Inventory.EnsureEveryItem();
                RadMessageBox.Show("Total of " + count + " Item(s) have been Added to your Inventory!", "Items Added",
                    MessageBoxButtons.OK,
                    RadMessageIcon.Info);
            }
        }

        private void xaddallitems_Click(object sender, EventArgs e)
        {
            var re = (RadTileElement) xtypecontainer.Items.FirstOrDefault(t => t.BackColor == Color.Red);
            if (re == null)
                return;
            var instance = re.Tag as ItemTypeInstance;
            var items =
                MainFile.DataBase.Items.Where(
                    t => instance != null && t.Type.StartID == instance.StartID && !t.IsIgnored)
                    .ToArray();
            if (items.Length > 0)
            {
                try
                {
                    var count = 0;
                    foreach (var ent in items)
                    {
                        if (ent.IsDlc && !SelectedSlot.AllowdDlcIndexes.Contains(ent.DlcVersion))
                            continue;
                        var xent = ent.Clone();
                        SetItemDefault(xent);
                        SelectedSlot.AddItem(xent, IsItemBox, count == items.Length - 1, 1);
                        xent.IsInItemBox = IsItemBox;
                        count++;
                    }
                }
                catch (Exception ex)
                {
                    RadMessageBox.Show(ex.Message, "Full", MessageBoxButtons.OK, RadMessageIcon.Exclamation);
                }
                finally
                {
                    ListItems(re.Tag as ItemTypeInstance, null, xsearchtext2.Text.Replace(" ", ""));
                }
            }
        }

        private void xswitchitembox_Click(object sender, EventArgs e)
        {
            if (IsItemBox)
            {
                xswitchitembox.Text = @"Switch To ItemBox";
                IsItemBox = false;
            }
            else
            {
                xswitchitembox.Text = @"Switch To Inventory";
                IsItemBox = true;
            }
        }

        private void xaddallselected_Click(object sender, EventArgs e)
        {
            try
            {
                if (xitemcontainer.SelectedItems.Count == 0)
                    return;
                foreach (var item in xitemcontainer.SelectedItems)
                {
                    SelectedSlot.AddItem(item.Tag as ItemEntry, IsItemBox, false, 1);
                }
                var i = (RadTileElement) xtypecontainer.Items.FirstOrDefault(t => t.BackColor == Color.Red);
                if (i != null)
                {
                    var type = i.Tag as ItemTypeInstance;
                    if (type != null)
                        ListItems(type, null, xsearchtext2.Text.Replace(" ", ""));
                }
            }
            catch (Exception ex)
            {
                RadMessageBox.Show(ex.Message, "Full", MessageBoxButtons.OK, RadMessageIcon.Exclamation);
            }
        }

        private void xdbinfo_Click(object sender, EventArgs e)
        {
            if (MainFile.DataBase == null || MainFile.DataBase.Items == null)
                return;
            var x = MainFile.DataBase;
            var types = new List<ItemTypeInstance.ItemTypes>();
            types.AddRange(
                ((ItemTypeInstance.ItemTypes[]) Enum.GetValues(typeof (ItemTypeInstance.ItemTypes))).Where(
                    num => num != ItemTypeInstance.ItemTypes.None));
            var total = 0;
            var sb = new StringBuilder();
            sb.AppendLine("DataBase Version : " + x.Version);
            sb.AppendLine("Last Updated at : " + x.LastUpdated);
            sb.AppendLine("Created By Jappi88");
            sb.AppendLine("Current Item Count : ");
            foreach (var num in types)
            {
                var count = 0;
                switch (num)
                {
                    case ItemTypeInstance.ItemTypes.Gesture:
                        count = x.Gestures == null ? 0 : x.Gestures.Length;
                        break;
                    case ItemTypeInstance.ItemTypes.Ignored:
                        count = x.Items == null ? 0 : x.Items.Where(t => t.IsIgnored).ToArray().Length;
                        break;
                    default:
                        count = x.Items == null ? 0 : x.Items.Where(t => t.Type.ItemType == num).ToArray().Length;
                        break;
                }
                if (num != ItemTypeInstance.ItemTypes.Ignored)
                    total += count;
                sb.AppendLine(Enum.GetName(typeof (ItemTypeInstance.ItemTypes), num) + "s : " + count);
            }
            sb.AppendLine("Total Items : " + total);
            RadMessageBox.Show(sb.ToString(), "DataBase Info", MessageBoxButtons.OK, RadMessageIcon.Info);
        }

        private void xstorageinfo_Click(object sender, EventArgs e)
        {
            var types = new List<ItemTypeInstance.ItemTypes>();
            var total = 0;
            var sb = new StringBuilder();
            foreach (var v in SelectedSlot.Inventory.GetAllItems())
            {
                if (!types.Contains(v.Type.ItemType))
                    types.Add(v.Type.ItemType);
            }
            types.Add(ItemTypeInstance.ItemTypes.Key);
            types.Add(ItemTypeInstance.ItemTypes.Gesture);
            var x = SelectedSlot.Inventory;
            sb.AppendLine((x.IsItemBox ? "ItemBox" : "Inventory") + " Owner : " + SelectedSlot.Name);
            sb.AppendLine("Current Item Count : ");
            foreach (var num in types)
            {
                var count = 0;
                switch (num)
                {
                    case ItemTypeInstance.ItemTypes.Gesture:
                        count = x.Gestures == null ? 0 : x.Gestures.Length;
                        break;
                    case ItemTypeInstance.ItemTypes.Key:
                        count = x.Keys == null ? 0 : x.Keys.Where(t => t.IsInItemBox == x.IsItemBox).ToArray().Length;
                        break;
                    default:
                        count = x.Items == null
                            ? 0
                            : x.GetAllItems().Where(t => t.Type.ItemType == num).ToArray().Length;
                        break;
                }
                if (count > 0)
                    sb.AppendLine(Enum.GetName(typeof (ItemTypeInstance.ItemTypes), num) + "s : " + count);
                total += count;
            }
            sb.AppendLine("Total Items : " + total);
            RadMessageBox.Show(sb.ToString(), (x.IsItemBox ? "ItemBox" : "Inventory") + " Info", MessageBoxButtons.OK,
                RadMessageIcon.Info);
        }

        #region Menu Bars

        private void xweaponinfobutton_Click(object sender, EventArgs e)
        {
            if (xweaponinfobutton.Text == "")
                return;
            WeaponInfoMenuExpand(!_expanded);
        }

        private void WeaponInfoMenuExpand(bool expand)
        {
            xiteminfopanel.PerformLayout();
            if (!expand)
            {
                xiteminfopanel.Height = 25;
                xweaponinfobutton.Image = Resources._1397693761_chevron_down;
            }
            else
            {
                xiteminfopanel.Height = 150;
                xweaponinfobutton.Image = Resources._1397693748_chevron_up;
            }
            _expanded = xiteminfopanel.Height == 150;
            xiteminfopanel.ResumeLayout(false);
            xiteminfopanel.EndInit();
        }


        private void weaponStatEditor1_OnItemChanged(ItemEntry item, SaveSlot slot, object sender)
        {
            if (item == null || item.IsInItemBox != IsItemBox)
            {
                if (sender is RadForm)
                    (sender as RadForm).Close();
                xweaponinfobutton.Text = "";
                WeaponInfoMenuExpand(false);
            }
            Relist();
        }

        #endregion
    }
}