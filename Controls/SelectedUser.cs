using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Dark_Souls_2_Resource_Extractor.DMG;
using Dark_Souls_II_Save_Editor.DB_Builder;
using Dark_Souls_II_Save_Editor.Properties;
using Dark_Souls_II_Save_Editor.SaveStuff;
using HavenInterface.Popup;
using Telerik.WinControls;
using Telerik.WinControls.UI.Docking;
using ItemEntry = Dark_Souls_II_Save_Editor.SaveStuff.ItemEntry;

namespace Dark_Souls_II_Save_Editor.Controls
{
    public partial class SelectedUser : UserControl
    {
        public DataBaseControl DataBase;
        public EquipmentUI Equipment;
        public InventoryViewer Inventory;
        public PlayerStatus Playerstats;
        public SaveSlot SelectedSlot;
        //internal Popup xpop;
        internal Popup xservermessage = null;
        //internal Controls.Progressview xview;

        public SelectedUser()
        {
            InitializeComponent();

            //xview = new Controls.Progressview();
            //xview.Size = new System.Drawing.Size(500, 60);
            //xpop = new Popup(xview);
            //xpop.AutoClose = false;
            //xpop.FocusOnOpen = false;
            //xpop.BackColor = Color.Transparent;
            //xpop.ShowingAnimation = PopupAnimations.RightToLeft | PopupAnimations.Slide;
            //xpop.HidingAnimation = PopupAnimations.LeftToRight | PopupAnimations.Slide;
            //MainForm.Mainpage.OnServerMessagerecieved += Mainpage_OnServerMessagerecieved;
            //Load += (x, y) => MainForm.Mainpage.GetServerMessage();
        }

        public ItemEntry SetItemDefault(ItemEntry ent)
        {
            if (Inventory == null)
                return ent;
            return Inventory.SetItemDefault(ent);
        }

        public void LoadSlot(SaveSlot slot, bool reload = false)
        {
            xdock.BeginUpdate();
            if (Playerstats == null)
                Playerstats = new PlayerStatus();
            if (Inventory == null)
                Inventory = new InventoryViewer();
            if (DataBase == null)
                DataBase = new DataBaseControl();
            if (Equipment == null)
                Equipment = new EquipmentUI();
            SelectedSlot = slot;
            Playerstats.SetSlot(slot);
            Inventory.LoadItems(slot, MainFile.ItemTypes);
            Equipment.LoadEquipments(slot);
            DataBase.LoadItems(slot, MainFile.ItemTypes);
            if (!reload)
            {
                for (var i = MainForm.AppPreferences.ViewTabs.Length - 1; i >= 0; i--)
                {
                    switch (MainForm.AppPreferences.ViewTabs[i])
                    {
                        case 0:
                            SetWindow("playerstats", SelectedSlot.Name + @" STATS", Playerstats, true);
                            break;
                        case 1:
                            SetWindow("equipment", SelectedSlot.Name + @" EQUIPMENT", Equipment, false);
                            break;
                        case 2:
                            SetWindow("inventory", SelectedSlot.Name + @" INVENTORY", Inventory, false);
                            break;
                        case 3:
                            SetWindow("database", @"Database Version " + MainFile.DataBase.Version, DataBase, false);
                            break;
                        case 4:
                            SetWindow("tools", "TOOLS", new ToolsMenu(SelectedSlot.MainInstance, SelectedSlot), false);
                            break;
                    }
                }
                Application.DoEvents();
                SelectedSlot.MainInstance.OnMainFileChange += MainInstance_OnMainFileChange;
                SelectedSlot.Inventory.InventoryChanged += Inventory_InventoryChanged;
                SelectedSlot.OnBlockChanged += SelectedSlot_OnBlockChanged;
                SelectedSlot.OnNameChanged += slot_OnNameChanged;
                Disposed += SelectedUser_Disposed;
                xdock.EndUpdate();
                Application.DoEvents();
            }
        }

        public void Reload()
        {
            LoadSlot(SelectedSlot, false);
        }

        private void SelectedUser_Disposed(object sender, EventArgs e)
        {
            SelectedSlot.MainInstance.OnMainFileChange -= MainInstance_OnMainFileChange;
            SelectedSlot.Inventory.InventoryChanged -= Inventory_InventoryChanged;
            SelectedSlot.OnBlockChanged -= SelectedSlot_OnBlockChanged;
            SelectedSlot.OnNameChanged -= slot_OnNameChanged;
        }

        private void Inventory_InventoryChanged(object sender, EventArgs e)
        {
            Inventory.Relist();
            Playerstats.SetSlot(SelectedSlot);
            Equipment.LoadEquipments(SelectedSlot);
        }

        private void slot_OnNameChanged(SaveSlot slot)
        {
            if (SelectedSlot == null)
                return;
            if (xdock.Contains("playerstats"))
                SetWindow("playerstats", SelectedSlot.Name + @" STATS", Playerstats, false);
            if (xdock.Contains("equipment"))
                SetWindow("equipment", SelectedSlot.Name + @" EQUIPMENT", Equipment, false);
            if (xdock.Contains("inventory"))
                SetWindow("inventory", SelectedSlot.Name + @" INVENTORY", Inventory, false);
        }

        private void MainInstance_OnMainFileChange(object sender, FileMain.FileAction action)
        {
            if (action == FileMain.FileAction.Closed)
            {
                Playerstats.Dispose();
                Inventory.Dispose();
                DataBase.Dispose();
                Equipment.Dispose();
                Playerstats = null;
                Inventory = null;
                DataBase = null;
                Equipment = null;
                xdock.CloseAllWindows();
            }
        }

        private void SelectedSlot_OnBlockChanged(SaveBlock block)
        {
            switch (block.BlockIndex)
            {
                case 0:
                case 1:
                    Playerstats.SetSlot(SelectedSlot);
                    Equipment.LoadEquipments(SelectedSlot);
                    break;
                case 5:
                    break;
                case 6:
                    break;
                case 10:
                    Inventory.LoadItems(SelectedSlot, MainFile.ItemTypes);
                    break;
                case 12:
                    break;
                case 14:
                    break;
                case 13:
                    break;
            }
        }

        private void xplayerstatus_Click(object sender, EventArgs e)
        {
            if (SelectedSlot == null)
                return;
            Playerstats.SetSlot(SelectedSlot);
            SetWindow("playerstats", SelectedSlot.Name + @" STATS", Playerstats, true);
        }

        private void xequipment_Click(object sender, EventArgs e)
        {
            if (SelectedSlot == null)
                return;
            // Equipment.LoadEquipments(SelectedSlot);
            // Equipment.Dock = DockStyle.Fill;
            SetWindow("equipment", SelectedSlot.Name + @" EQUIPMENT", Equipment, true);
        }

        private void xinventory_Click(object sender, EventArgs e)
        {
            if (SelectedSlot == null)
                return;
            //Inventory.LoadItems(SelectedSlot, SelectedSlot.MainFile.ItemTypes);
            //Inventory.Dock = DockStyle.Fill;
            SetWindow("inventory", SelectedSlot.Name + @" INVENTORY", Inventory, true);
        }

        private void xdatabase_Click(object sender, EventArgs e)
        {
            if (SelectedSlot == null)
                return;
            // DataBase.LoadItems(SelectedSlot, SelectedSlot.MainFile.ItemTypes);
            // DataBase.Dock = DockStyle.Fill;
            SetWindow("database", @"Database Version " + MainFile.DataBase.Version, DataBase, true);
        }

        private void xtools_Click(object sender, EventArgs e)
        {
            if (SelectedSlot == null)
                return;
            SetWindow("tools", "TOOLS", new ToolsMenu(SelectedSlot.MainInstance, SelectedSlot), true);
        }

        private void xsavechanges_Click(object sender, EventArgs e)
        {
            if (SelectedSlot == null)
                return;
            if (ProgressPopupUI.IsBusy)
            {
                RadMessageBox.Show(Resources.ToolBusy, "busy", MessageBoxButtons.OK,
                    RadMessageIcon.Exclamation);
                return;
            }
            if (MainFile.ProgressInstance != null)
            {
                if (MainFile.ProgressInstance.isbusy)
                {
                    RadMessageBox.Show(
                        "The Tool is still busy doing stuff... please wait for the operation to complete.", "busy",
                        MessageBoxButtons.OK, RadMessageIcon.Exclamation);
                    return;
                }
            }
            if (SelectedSlot == null)
                return;
            try
            {
                if (!SelectedSlot.MainInstance.Backup() || !SelectedSlot.MainInstance.Isvalid)
                    return;
                //if (!xpop.Visible)
                //{
                //    Point location = this.PointToScreen(this.Location);
                //    xpop.Show(location.X + (this.Width - 500), location.Y + (this.Height - 60));
                //}
                SelectedSlot.MainInstance.SaveSlot(SelectedSlot);
                //xpop.Close();
                if (SelectedSlot.MainInstance.Isvalid)
                    RadMessageBox.Show("All changes have succesfully been saved\nFile is ready to be played", "Saved",
                        MessageBoxButtons.OK, RadMessageIcon.Info);
            }
            catch (Exception ex)
            {
                RadMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, RadMessageIcon.Error);
            }
        }

        private void xsavedb_Click(object sender, EventArgs e)
        {
            if (MainFile.DataBase.IsBussy || ProgressPopupUI.IsBusy)
                return;
            ProgressPopupUI.IsBusy = true;
            //if (!xpop.Visible)MainForm.AppSettings.ResourcePath
            //{
            //    Point location = this.PointToScreen(this.Location);
            //    xpop.Show(location.X + (this.Width - 500), location.Y + (this.Height - 60));
            //}
            ProgressPopupUI.OnViewShow();
            //MainForm.Mainpage.Dbversion
            var flag = MainForm.AppPreferences.Language != "English";
            DSDataBase.P = MainFile.ProgressInstance;
            if (MainFile.DataBase.WriteDB(MainForm.Mainpage.Dbversion, flag))
            {
                if (flag)
                {
                    var lng = new LanguageDB(
                        MainForm.Settings.ResourcePath + "\\" + MainForm.AppPreferences.Language + ".lng",
                        MainFile.ProgressInstance);
                    var items = new List<ItemEntry>();
                    items.AddRange(MainFile.DataBase.Items);
                    items.AddRange(MainFile.DataBase.Gestures);
                    var version = MainFile.DataBase.Version;
                    lng.CopyToLng(items.ToArray(), lng.Areas ?? new MapEntry[0], version);
                    while (lng.IsBussy)
                        Application.DoEvents();
                    items.Clear();
                }
                ProgressPopupUI.IsBusy = false;
                ProgressPopupUI.OnViewClose();
                //RadMessageBox.Show("Database succesfully saved!", "Saved", MessageBoxButtons.OK, RadMessageIcon.Info);
            }
            else
            {
                ProgressPopupUI.IsBusy = false;
                ProgressPopupUI.OnViewClose();
                RadMessageBox.Show("Failed to save Database Changes!", "Error", MessageBoxButtons.OK,
                    RadMessageIcon.Error);
            }
        }

        private void SetWindow(string name, string text, Control c, bool ensurevisible)
        {
            var w = xdock.GetWindow<HostWindow>(name);
            if (w == null)
            {
                w = xdock.DockControl(c, new DockPosition());
                w.Name = name;
            }
            w.Text = text;
            w.DockState = DockState.TabbedDocument;
            if (ensurevisible)
                w.EnsureVisible();
            Application.DoEvents();
        }

        private void xmapinfo_Click(object sender, EventArgs e)
        {
            try
            {
                if (SelectedSlot.MapInfo == null)
                    throw new Exception("Current slot does not have any Map Info!");
                var x = SelectedSlot.MapInfo.AvailibleAreas();
                if (x == null || x.Length == 0)
                    throw new Exception("Slot does not have any availible areas");

                //var ids = new List<int> {};
                //using (var sw = new StreamWriter(File.Create(Application.StartupPath + "\\MapOUtput.txt")))
                //{
                //    foreach (var v in x)
                //    {
                //        sw.WriteLine(@"###########################################");
                //        sw.WriteLine(v.MapId);
                //        sw.WriteLine(@"===========================================");
                //        foreach (var j in v.BonfireList)
                //        {
                //            if(!ids.Contains(j.Id))
                //                ids.Add(j.Id);
                //            sw.WriteLine(j.Id);
                //        }
                //        sw.WriteLine(@"===========================================");
                //        sw.WriteLine(@"###########################################");
                //    }
                //    sw.WriteLine("Total Different bonfire id's : " + ids.Count);
                //}
                var bfn =
                    "C:\\Users\\Jappi\\Dropbox\\4E51B35F66572764BD7BD3786114A21ADB8C784746 Contents\\UPDATE\\tu00000008_00000000 Contents\\menu\\Text\\English\\BonfireName.fmg";
                var an =
                    "C:\\Users\\Jappi\\Dropbox\\4E51B35F66572764BD7BD3786114A21ADB8C784746 Contents\\UPDATE\\tu00000008_00000000 Contents\\menu\\Text\\English\\MapName.fmg";
                var ar =
                    "C:\\Users\\Jappi\\Dropbox\\4E51B35F66572764BD7BD3786114A21ADB8C784746 Contents\\pngs\\Areas\\";
                var bf =
                    "C:\\Users\\Jappi\\Dropbox\\4E51B35F66572764BD7BD3786114A21ADB8C784746 Contents\\pngs\\Bonfires\\";
                var bonfires = new DMGFile(bfn, bf);
                var areas = new DMGFile(an, ar);
                SetWindow("travel", @"Travel", new AreasUI(areas, bonfires, SelectedSlot), true);
            }
            catch (Exception ex)
            {
                RadMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, RadMessageIcon.Error);
            }
        }
    }
}