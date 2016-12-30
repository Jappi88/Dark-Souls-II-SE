using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Dark_Souls_II_Save_Editor.DB_Builder;
using Dark_Souls_II_Save_Editor.Forms;
using Dark_Souls_II_Save_Editor.Parameters;
using Dark_Souls_II_Save_Editor.Properties;
using Dark_Souls_II_Save_Editor.SaveStuff;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace Dark_Souls_II_Save_Editor.Controls
{
    public partial class DataBaseControl : UserControl
    {
        public enum ViewTypes
        {
            Inventory,
            DataBase,
            ItemParameters
        }

        private ItemTypeInstance _currentsection;

        public DataBaseControl()
        {
            InitializeComponent();

            MainFile.DataBase.OnItemChanged += DB_OnItemChanged;
            MainFile.DataBase.DatabaseLoaded += DataBase_DatabaseLoaded;
            Disposed += (x, y) =>
            {
                MainFile.DataBase.OnItemChanged -= DB_OnItemChanged;
                MainFile.DataBase.DatabaseLoaded -= DataBase_DatabaseLoaded;
            };
        }

        public SaveSlot SelectedSlot { get; private set; }
        public MainParamManager Manager { get; private set; }
        public ItemTypeInstance[] ItemTypes { get; private set; }
        public ViewTypes ViewType { get; internal set; }

        private void DataBase_DatabaseLoaded(object sender, EventArgs e)
        {
            if (SelectedSlot != null && MainFile.ItemTypes != null)
            {
                LoadItems(SelectedSlot, MainFile.ItemTypes);
                if (SelectedSlot.Inventory != null)
                    SelectedSlot.Inventory.Reload();
            }
        }

        public void LoadItems(SaveSlot slot, ItemTypeInstance[] xinstances)
        {
            ViewType = ViewTypes.DataBase;
            SelectedSlot = slot;
            DoLngProgress("Current Language : " + MainForm.Settings.Language, 0, 0, 0);
            xdbdescription.Text = "DataBase Version " + MainFile.DataBase.Version + "  [Total Of : " +
                                  MainFile.DataBase.Items.Length + " Items]";
            var instances = new List<ItemTypeInstance>();
            instances.Add(new ItemTypeInstance {Image = Resources.recycle_bin_f, StartID = 9999, EndID = 9999});
            // instances.Add(new ItemTypeInstance() {Image = Resources.unknown,StartID = 0,EndID = 0});
            instances.AddRange(
                xinstances.Where(v => v.StartID < 1000 && (v.StartID < 20 || v.StartID > 120) && v.StartID != 160));
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
            addDBItemControl1.LoadInstance();
            if (instances.Count > 1)
                xtypecontainer.Items[1].PerformClick();
        }

        private void DB_OnItemChanged(ItemEntry sender, int asignedindex)
        {
            foreach (var v in xitemcontainer.Items.Where(v => v.Tag.GetType() == typeof (ItemEntry)).Where(v =>
            {
                var itemEntry = v.Tag as ItemEntry;
                return itemEntry != null && itemEntry.ID1 == sender.ID1;
            }))
            {
                //foreach (var ent in SelectedSlot.Inventory.GetItems(sender))
                //{
                //    ent.Name = sender.Name;
                //    ent.Description = sender.Description;
                //    ent.SimpleDescription = sender.SimpleDescription;
                //    ent.MayCauseCorruption = sender.MayCauseCorruption;
                //    ent.CanHaveMoreThenOne = sender.CanHaveMoreThenOne;
                //    ent.IsDlc = sender.IsDlc;
                //}
                v.Image = sender.GetImage(false);
            }
            if (SelectedSlot != null)
                SelectedSlot.Inventory.Reload();
        }

        public void LoadItems(MainParamManager manager, ItemTypeInstance[] xinstances)
        {
            if (manager == null)
                return;
            Manager = manager;
            ViewType = ViewTypes.ItemParameters;
            xdbdescription.Text = "DataBase Version " + MainFile.DataBase.Version + "  [Total Of : " +
                                  MainFile.DataBase.Items.Length + " Items]";
            var instances = new List<ItemTypeInstance>();
            instances.AddRange(
                xinstances.Where(v => v.StartID < 1000 && (v.StartID < 20 || v.StartID > 120) && v.StartID != 160));
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

        public void Relist()
        {
            var i = (RadTileElement) xtypecontainer.Items.FirstOrDefault(t => t.BackColor == Color.Red);
            if (i != null)
                Relist(i, EventArgs.Empty);
            else
                xtypecontainer.Items[0].PerformClick();
        }

        private void Relist(object sender, EventArgs e)
        {
            var item = sender as RadTileElement;
            if (item == null)
                return;
            var type = item.Tag as ItemTypeInstance;
            if (type == null)
                return;
            _currentsection = type;
            item.SetSelected(xtypecontainer, Color.Red);
            ListItems(type, xsearchtext.Text.Replace(" ", ""));
        }

        private void ListItems(ItemTypeInstance instance, string filter)
        {
            if (SelectedSlot == null)
                return;
            ItemEntry[] dbitems = null;
            if (instance.ItemType == ItemTypeInstance.ItemTypes.Ignored)
                dbitems = MainFile.DataBase.Items.Where(t => t.IsIgnored).ToArray();
            else
                dbitems =
                    MainFile.DataBase.Items.Where(t => t.Type.ItemType == instance.ItemType && !t.IsIgnored).ToArray();
            xitemcontainer.SelectedItem = null;
            xitemcontainer.BeginUpdate();
            xitemcontainer.Items.Clear();
            if (dbitems.Length > 0)
            {
                foreach (var i in dbitems)
                {
                    if (!string.IsNullOrEmpty(filter) && filter != "Finditems..")
                        if (!i.Name.ToLower().Replace(" ", "").Contains(filter.ToLower()))
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

        private void xitemcontainer_SelectedItemChanged(object sender, EventArgs e)
        {
            if (xitemcontainer.SelectedItems.Count == 0)
            {
                itemParamProperties1.SetProperties(null, null);
            }
            else
            {
                var entry = xitemcontainer.SelectedItems[0].Tag as ItemEntry;
                if (entry == null)
                    itemParamProperties1.SetProperties(null, null);
                else
                {
                    if (ViewType == ViewTypes.ItemParameters)
                        itemParamProperties1.SetProperties(entry, entry.Parameter);
                    else
                        itemParamProperties1.SetProperties(entry, entry);
                }
            }
        }

        private void radContextMenu1_DropDownOpening(object sender, CancelEventArgs e)
        {
            if (xitemcontainer.SelectedItem == null || ViewType == ViewTypes.ItemParameters)
                e.Cancel = true;
            else
            {
                var delete = ViewType == ViewTypes.Inventory;
                radContextMenu1.Items.Clear();
                RadMenuItem i = null;
                if (delete)
                {
                    i = new RadMenuItem("Remove Selected Item(s)");
                    i.Click += RemoveItems;
                    i.Image = Resources.Delete_icon;
                    radContextMenu1.Items.Add(i);
                }
                var ent = xitemcontainer.SelectedItem.Tag as ItemEntry;
                if (ent != null)
                {
                    if (ent.Parameter != null)
                    {
                        i = new RadMenuItem("View Item Parameters");
                        i.Click += ViewItemParams;
                        i.Image = Resources.item_Param_icon;
                        radContextMenu1.Items.Add(i);
                    }
                    if (ent.SpellParam != null)
                    {
                        i = new RadMenuItem("View Spell Parameters");
                        i.Click += ViewSpellParams;
                        i.Image = Resources.Spells_icon;
                        radContextMenu1.Items.Add(i);
                    }
                }
            }
        }

        private void ViewItemParams(object sender, EventArgs e)
        {
            if (xitemcontainer.SelectedItems.Count == 0)
                return;
            var ent = xitemcontainer.SelectedItem.Tag as ItemEntry;
            if (ent != null)
            {
                if (ent.Parameter != null)
                {
                    var ip = new ItemParamProperties();
                    ip.SetProperties(ent, ent.Parameter);
                    CreateDialogWithPropery(ent.Name + " Item Parameters", ip);
                }
            }
        }

        private void ViewSpellParams(object sender, EventArgs e)
        {
            if (xitemcontainer.SelectedItems.Count == 0)
                return;
            var ent = xitemcontainer.SelectedItem.Tag as ItemEntry;
            if (ent != null)
            {
                if (ent.SpellParam != null)
                {
                    var sp = new SpellProperties();
                    sp.SetSpellParam(ent.SpellParam, ent);
                    CreateDialogWithPropery(ent.Name + " Skill Parameters", sp);
                }
            }
        }

        private DialogResult CreateDialogWithPropery(string text, Control control)
        {
            var rf = new RadForm();
            rf.Text = text;
            rf.Size = new Size(500, 500);
            rf.StartPosition = FormStartPosition.CenterParent;
            rf.MaximizeBox = false;
            rf.FormBorderStyle = FormBorderStyle.FixedDialog;
            control.Dock = DockStyle.Fill;
            rf.Controls.Add(control);
            return rf.ShowDialog();
        }

        private void RemoveItems(object sender, EventArgs e)
        {
            if (xitemcontainer.SelectedItems.Count == 0)
                return;
            var result = RadMessageBox.Show("Would you like to remove all duplicate items aswell ?", "Duplicate Removal",
                MessageBoxButtons.YesNoCancel, RadMessageIcon.Question);
            if (result == DialogResult.Cancel)
                return;
            for (var i = 0; i < xitemcontainer.SelectedItems.Count; i++)
            {
                var ent = xitemcontainer.SelectedItems[i].Tag as ItemEntry;
                if (ent != null)
                {
                    SelectedSlot.Inventory.DeleteItem(ent, result == DialogResult.Yes);
                }
            }
            SelectedSlot.Inventory.OnInventoryChanged();
            Relist();
        }

        private void xsearchtext_Enter(object sender, EventArgs e)
        {
            if (xsearchtext.Text == "Find items..")
            {
                xsearchtext.Text = "";
            }
        }

        private void xsearchtext_Leave(object sender, EventArgs e)
        {
            if (xsearchtext.Text == "")
            {
                xsearchtext.Text = "Find items..";
            }
        }

        private void xsearchtext_TextChanging(object sender, TextChangingEventArgs e)
        {
            var re = (RadTileElement) xtypecontainer.Items.FirstOrDefault(t => t.BackColor == Color.Red);
            if (re == null)
                return;
            var instance = re.Tag as ItemTypeInstance;
            if (instance != null)
            {
                ListItems(instance, xsearchtext.Text.Replace(" ", ""));
            }
        }

        private void xitemcontainer_ToolTipTextNeeded(object sender, ToolTipTextNeededEventArgs e)
        {
            var v = sender as SimpleListViewVisualItem;
            if (v == null)
                return;
            var lv = v.Data;
            if (lv != null)
            {
                var ent = lv.Tag as ItemEntry;
                if (ent != null)
                {
                    if (ent.Description != null)
                        e.ToolTipText = ent.Description;
                    else if (ent.SimpleDescription != null)
                        e.ToolTipText = ent.SimpleDescription;
                    else e.ToolTipText = ent.Name;
                }
            }
        }

        private void xitemcontainer_OnSectionDrop(ItemEntry[] items, RadTileElement section, RadPanorama container)
        {
            try
            {
                var type = section.Tag as ItemTypeInstance;
                if (type != null && items != null && items.Length > 0)
                {
                    foreach (var item in items)
                    {
                        if (type.StartID == 9999 && type.EndID == 9999 || item.IsIgnored)
                            item.IsIgnored = !item.IsIgnored;
                        else
                            item.EntryIndex = (int) type.StartID + 1;
                    }
                    Relist();
                }
            }
            catch (Exception)
            {
            }
        }

        private void xaddbutton_Click(object sender, EventArgs e)
        {
            Application.DoEvents();
            xadditempanel.PerformLayout();
            if (xadditempanel.Height == 165)
            {
                xadditempanel.Height = 24;
                xaddbutton.Image = Resources._1397693761_chevron_down;
            }
            else
            {
                xadditempanel.Height = 165;
                xaddbutton.Image = Resources._1397693748_chevron_up;
            }
            xadditempanel.EndInit();
            xadditempanel.ResumeLayout(false);
            Update();
        }

        private void addDBItemControl1_OnItemAdded(ItemEntry item)
        {
            var items = new List<ItemEntry>();
            items.AddRange(MainFile.DataBase.Items);
            items.Add(item);
            MainFile.DataBase.Items = items.ToArray();
            if (_currentsection.StartID == item.Type.StartID)
                Relist();
            Application.DoEvents();
            xadditempanel.PerformLayout();
            xadditempanel.Height = 24;
            xaddbutton.Image = Resources._1397693761_chevron_down;
            xadditempanel.EndInit();
            xadditempanel.ResumeLayout(false);
            Update();
            SelectedSlot.Inventory.Reload();
        }

        private void xitemcontainer_ItemMouseDoubleClick(object sender, ListViewItemEventArgs e)
        {
            if (e.Item == null || ViewType == ViewTypes.ItemParameters)
                return;
            var item = e.Item.Tag as ItemEntry;
            if (item != null)
            {
                var x = new Set_Quantity(item, item.Type);
                if (x.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        if (item.Type.ItemType == ItemTypeInstance.ItemTypes.Item ||
                            item.Type.ItemType == ItemTypeInstance.ItemTypes.Arrow ||
                            item.Type.ItemType == ItemTypeInstance.ItemTypes.Shard ||
                            item.Type.ItemType == ItemTypeInstance.ItemTypes.Spell)
                        {
                            SelectedSlot.AddItem(item, item.IsInItemBox, true, 1);
                        }
                        else
                        {
                            SelectedSlot.AddItem(item, item.IsInItemBox, true, (int) x.SelectedAmmount);
                        }
                    }
                    catch (Exception ex)
                    {
                        RadMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, RadMessageIcon.Error);
                    }
                }
            }
        }

        private void radButton1_ToolTipTextNeeded(object sender, ToolTipTextNeededEventArgs e)
        {
            var rb = sender as RadButtonElement;
            if (rb != null)
                e.ToolTipText = rb.Text;
        }

        private void DoLngProgress(string message, int percentage, long low, long max)
        {
            if (radProgressBar1.InvokeRequired)
            {
                radProgressBar1.Invoke(new Action(() => radProgressBar1.Value1 = percentage));
                radProgressBar1.Invoke(new Action(() => radProgressBar1.Text = message));
                radProgressBar1.Invoke(new Action(() => radProgressBar1.Invalidate()));
            }
            else
            {
                radProgressBar1.Value1 = percentage;
                radProgressBar1.Text = message;
                radProgressBar1.Invalidate();
            }
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            var rb = sender as RadButton;
            if (rb != null)
            {
                if (rb.Text == "English")
                    SetDefaultlanguage();
                else
                {
                    var files = Directory.GetFiles(MainForm.Settings.ResourcePath);
                    var found = false;
                    string file = null;
                    if (files.Length > 0)
                    {
                        file = files.FirstOrDefault(x => new FileInfo(x).Name == rb.Text + ".lng");
                        found = file != null;
                    }
                    if (!found)
                    {
                        if (
                            RadMessageBox.Show(
                                "There is no " + rb.Text + " language found, would you like to download?",
                                "Download Language", MessageBoxButtons.YesNo, RadMessageIcon.Exclamation) ==
                            DialogResult.Yes)
                        {
                            MainForm.Mainpage.DownloadDatabase(rb.Text + ".lng", MainForm.Settings.ResourcePath,
                                false);
                            DoLngProgress("Current Language : " + MainForm.Settings.Language, 0, 0, 0);
                            LoadDblng(MainForm.Settings.ResourcePath + "\\" + rb.Text + ".lng");
                        }
                    }
                    else
                    {
                        LoadDblng(file);
                    }
                }
            }
        }

        private void SetDefaultlanguage()
        {
            foreach (var item in MainFile.DataBase.Items)
            {
                item.Name = item.xName;
                item.Description = item.xDescription;
                item.SimpleDescription = item.xSimpleDescription;
            }

            foreach (var item in MainFile.DataBase.Gestures)
            {
                item.Name = item.xName;
                item.Description = item.xDescription;
                item.SimpleDescription = item.xSimpleDescription;
            }
            MainForm.AppPreferences.Language = "English";
            MainForm.AppPreferences.Save();
            DoLngProgress("Current Language : " + MainForm.AppPreferences.Language, 0, 0, 0);
            SelectedSlot.DbLanguageChanged();
            Relist();
        }

        private void LoadDblng(string db)
        {
            if (MainFile.DataBase != null && File.Exists(db))
            {
                MainFile.ProgressInstance.AtProgress += DoLngProgress;
                var x = new LanguageDB(db, MainFile.ProgressInstance);
                x.Load();
                while (x.IsBussy)
                {
                    Thread.Sleep(10);
                    Application.DoEvents();
                }
                if (x.Version != MainFile.DataBase.Version)
                {
                    MainForm.Mainpage.DownloadDatabase(Path.GetFileName(db), MainForm.Settings.ResourcePath, false);
                    x.Load();
                    while (x.IsBussy)
                    {
                        Application.DoEvents();
                    }
                }
                MainFile.ProgressInstance.AtProgress -= DoLngProgress;
                int count;
                if (x.Items != null && x.Items.Length > 0)
                {
                    count = 0;
                    foreach (var item in MainFile.DataBase.Items)
                    {
                        DoLngProgress(
                            "Loading (" + Functions.GetPercentage(count, MainFile.DataBase.Items.Length) + ")",
                            Functions.GetPercentage(count, MainFile.DataBase.Items.Length), 0, 0);
                        var temp = x.Items.FirstOrDefault(t => t.ID1 == item.ID1);
                        if (temp != null)
                        {
                            if (temp.Name.Contains('(') || temp.Name.Contains(')'))
                            {
                                item.Name = item.xName;
                                item.Description = item.xDescription;
                                item.SimpleDescription = item.xSimpleDescription;
                            }
                            else
                            {
                                item.Name = temp.Name;
                                item.Description = temp.Description;
                                item.SimpleDescription = temp.SimpleDescription;
                            }
                        }
                        else
                        {
                            item.Name = item.xName;
                            item.Description = item.xDescription;
                            item.SimpleDescription = item.xSimpleDescription;
                        }
                        count++;
                    }
                }
                count = 0;
                foreach (var item in MainFile.DataBase.Gestures)
                {
                    DoLngProgress("Loading (" + Functions.GetPercentage(count, MainFile.DataBase.Items.Length) + ")",
                        Functions.GetPercentage(count, MainFile.DataBase.Items.Length), 0, 0);
                    if (x.Items != null)
                    {
                        var temp = x.Items.FirstOrDefault(t => t.ID1 == item.ID1);
                        if (temp != null)
                        {
                            if (temp.Name.Contains('(') || temp.Name.Contains(')'))
                            {
                                item.Name = item.xName;
                                item.Description = item.xDescription;
                                item.SimpleDescription = item.xSimpleDescription;
                            }
                            else
                            {
                                item.Name = temp.Name;
                                item.Description = temp.Description;
                                item.SimpleDescription = temp.SimpleDescription;
                            }
                        }
                        else
                        {
                            item.Name = item.xName;
                            item.Description = item.xDescription;
                            item.SimpleDescription = item.xSimpleDescription;
                        }
                    }
                    count++;
                }
                MainForm.AppPreferences.Language = new FileInfo(db).Name.Replace(".lng", "");
                MainForm.AppPreferences.Save();
                DoLngProgress("Current Language : " + MainForm.AppPreferences.Language, 0, 0, 0);
                SelectedSlot.DbLanguageChanged();
                Relist();
            }
        }

        private void xitemcontainer_OnItemsDraged(ItemEntry[] items, object destination)
        {
            var rv = destination as RadListView;
            if (rv == null || SelectedSlot == null)
                return;
            if (rv.Name == "xinventorycontainer" && items.Length > 0)
            {
                try
                {
                    var county = 0;
                    foreach (var ent in items)
                    {
                        var xent = ent.Clone();
                        if (SelectedSlot.UserUI != null)
                            SelectedSlot.UserUI.SetItemDefault(xent);
                        SelectedSlot.AddItem(xent, SelectedSlot.Inventory.IsItemBox, county == items.Length - 1, 1);
                        xent.IsInItemBox = SelectedSlot.Inventory.IsItemBox;
                        county++;
                    }
                }
                catch (Exception ex)
                {
                    RadMessageBox.Show(ex.Message, "Full", MessageBoxButtons.OK, RadMessageIcon.Exclamation);
                }
                foreach (var v in from v in rv.Items
                    let itemEntry = v.Tag as ItemEntry
                    where v.Tag is ItemEntry
                    from t in items.Where(t => itemEntry != null && itemEntry.ID1 == t.ID1)
                    select v)
                {
                    rv.SelectedItem = v;
                    rv.ListViewElement.EnsureItemVisible(v);
                }
            }
        }

        private void radButton12_Click(object sender, EventArgs e)
        {
            if (xitemcontainer.SelectedItem == null)
                return;
            var item = xitemcontainer.SelectedItem;
            var ofd = new OpenFileDialog();
            ofd.Filter = "PNG|*.PNG";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                var xitem = item.Tag as ItemEntry;
                if (xitem != null)
                {
                    xitem.ImageStream = new MemoryStream(File.ReadAllBytes(ofd.FileName));
                    item.Image = xitem.ItemImage;
                    Application.DoEvents();
                }
            }
        }
    }
}