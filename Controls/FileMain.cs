using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Dark_Souls_II_Save_Editor.Properties;
using Dark_Souls_II_Save_Editor.SaveStuff;
using HavenInterface.Fatx;
using HavenInterface.FtpClient;
using HavenInterface.Popup;
using Telerik.WinControls;

namespace Dark_Souls_II_Save_Editor.Controls
{
    public partial class FileMain : UserControl
    {
        public delegate void FileActionArg(object sender, FileAction action);

        public delegate void ResourceCheckArg();

        public enum FileAction
        {
            Loading,
            Loaded,
            Closed,
            Saving,
            Saved
        }

        private bool isbusy;
        public Control Owner;
        public Popup xpop;
        public PopupContainer xpopcontainer;

        public FileMain(Control owner)
        {
            InitializeComponent();
            Owner = owner;

            xpopcontainer = new PopupContainer();
            xpopcontainer.OnClose += xpopcontainer_OnClose;
            xpopcontainer.OnMinimize += xpopcontainer_OnMinimize;
            xpop = new Popup(xpopcontainer);
            xpop.AutoClose = false;
            xpop.FocusOnOpen = true;
            xpop.BackColor = Color.Transparent;
            xpop.ShowingAnimation = PopupAnimations.BottomToTop | PopupAnimations.Slide;
            xpop.HidingAnimation = PopupAnimations.TopToBottom | PopupAnimations.Slide;
        }

        public FTPControl FTPPage { get; private set; }
        public USBManager UsbPage { get; private set; }
        public MainFile DarkSoulsFile { get; private set; }
        public event FileActionArg OnSlotChange;
        public event FileActionArg OnMainFileChange;
        public event ResourceCheckArg OnResourceCheck;

        private void xpopcontainer_OnMinimize(object sender, EventArgs e)
        {
            if (xpop.Visible)
            {
                var flag = xpop.Height > 25;
                xpop.Close();
                if (flag)
                {
                    xpop.Size = new Size(500, 25);
                    xpop.Update();
                    var loc = Owner.PointToScreen(Owner.Location);
                    xpop.Show(loc.X + (Owner.Width - 500), loc.Y + (Owner.Height - 60));
                }
                else
                {
                    xpop.Size = new Size(500, 300);
                    xpop.Update();
                    var loc = Owner.PointToScreen(Owner.Location);
                    xpop.Show(loc.X + (Owner.Width - 500), loc.Y + (Owner.Height - 335));
                }
            }
        }

        private void xpopcontainer_OnClose(object sender, EventArgs e)
        {
            if (xpop.Visible)
                xpop.Close();
        }

        private void xloadxbox_Click(object sender, EventArgs e)
        {
            if (isbusy)
                return;
            var ofd = new OpenFileDialog();
            ofd.Title = @"Load Dark Souls II STFS Package";
            ofd.FileName = "-GAME_0000";
            if (ofd.ShowDialog() == DialogResult.OK)
                LoadSave(ofd.FileName, MainFile.ConsoleType.XBOX360);
        }

        public void LoadSave(string path, MainFile.ConsoleType type)
        {
            if (ProgressPopupUI.IsBusy)
            {
                RadMessageBox.Show(Resources.ToolBusy, "busy", MessageBoxButtons.OK, RadMessageIcon.Exclamation);
                return;
            }
            if (MainFile.ProgressInstance != null)
            {
                if (MainFile.ProgressInstance.isbusy)
                {
                    RadMessageBox.Show(Resources.ToolBusy, "busy", MessageBoxButtons.OK, RadMessageIcon.Exclamation);
                    return;
                }
            }
            var db = MainForm.Settings.ResourcePath + "\\DS2Items_English.db";
            var dbtypes = MainForm.Settings.ResourcePath + "\\ItemTypes.db";
            if (!File.Exists(db) || !File.Exists(dbtypes))
            {
                RadMessageBox.Show(
                    "Some vital database resources are missing!\nThe missing resources will need to be downloaded first",
                    "No Resources", MessageBoxButtons.OK, RadMessageIcon.Exclamation);
                OnResourceCheck?.Invoke();
            }
            else
            {
                OnMainFileChange?.Invoke(DarkSoulsFile, FileAction.Loading);
                Close();
                Control xx = null;
                if (xcontainer.Controls.Count > 0)
                    xx = xcontainer.Controls[0];
                try
                {
                    var controls = new Control[] {};
                    xcontainer.Controls.CopyTo(controls, 0);
                    OnMainFileChange?.Invoke(DarkSoulsFile, FileAction.Closed);
                    //xcontainer.Controls.Clear();
                    var p = new Progressview();
                    p.Dock = DockStyle.Fill;
                    //Progressor x = new Progressor();
                    ProgressPopupUI.Progress.AtProgress += p.DoProgress;
                    xcontainer.Controls.Add(p);
                    isbusy = true;
                    ProgressPopupUI.IsBusy = true;
                    DarkSoulsFile = new MainFile(path, db, dbtypes, type, ProgressPopupUI.Progress);
                    if (DarkSoulsFile.Isvalid)
                    {
                        DarkSoulsFile.OnMainFileChange += OnMainFileChange;
                        ProgressPopupUI.Progress.DoProgress("Loading Availible Slots, Please Wait...", 100, 0, 0);
                        ProgressPopupUI.Progress.AtProgress -= p.DoProgress;
                        if (DarkSoulsFile.Isvalid)
                        {
                            if (DarkSoulsFile.UsedSlots.Length == 0)
                            {
                                throw new Exception(
                                    "Your loaded Dark souls II Savegame does not contain any used slots...please make a slot before loading it.");
                            }
                            xselectslot.PerformClick();
                            xsavefile.Visibility = ElementVisibility.Visible;
                            xselectslot.Visibility = ElementVisibility.Visible;
                            xclosefile.Visibility = ElementVisibility.Visible;
                            OnMainFileChange?.Invoke(DarkSoulsFile, FileAction.Loaded);
                        }
                        else
                        {
                            xcontainer.Controls.Clear();
                            xcontainer.Controls.AddRange(controls);
                            OnMainFileChange?.Invoke(DarkSoulsFile, FileAction.Closed);
                        }
                    }
                    else
                    {
                        xcontainer.Controls.Clear();
                        xcontainer.Controls.AddRange(controls);
                        OnMainFileChange?.Invoke(DarkSoulsFile, FileAction.Closed);
                    }
                    isbusy = false;
                    ProgressPopupUI.IsBusy = false;
                }
                catch (Exception ex)
                {
                    isbusy = false;
                    xcontainer.Controls.Clear();
                    if (xx != null)
                        xcontainer.Controls.Add(xx);
                    OnMainFileChange?.Invoke(DarkSoulsFile, FileAction.Closed);
                    RadMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, RadMessageIcon.Error);
                }
            }
        }

        private void LoadSLot(SaveSlot slot)
        {
            OnSlotChange?.Invoke(slot, FileAction.Loaded);
        }

        private void xloadusb_Click(object sender, EventArgs e)
        {
            if (isbusy)
                return;
            DeviceConnectionChanged(true, true);
            //if (Mainpage.IsBusy)
            //{
            //    RadMessageBox.Show(Resources.ToolBusy, "busy", MessageBoxButtons.OK, RadMessageIcon.Exclamation);
            //    return;
            //}
            //if (MainFile.ProgressInstance != null)
            //{
            //    if (MainFile.ProgressInstance.isbusy)
            //    {
            //        RadMessageBox.Show(Resources.ToolBusy, "busy", MessageBoxButtons.OK, RadMessageIcon.Exclamation);
            //        return;
            //    }
            //}
            //string db = MainForm.AppSettings.ResourcePath + "\\DS2Items_English.db";
            //string dbtypes = MainForm.AppSettings.ResourcePath + "\\ItemTypes.db";
            //if (!File.Exists(db) || !File.Exists(dbtypes))
            //{
            //    RadMessageBox.Show(
            //        "Some vital database resources are missing!\nThe missing resources will need to be downloaded first",
            //        "No Resources", MessageBoxButtons.OK, RadMessageIcon.Exclamation);
            //    if (OnResourceCheck != null)
            //        OnResourceCheck();
            //}
            //else
            //{
            //    xloadusb.SetSelected(radPanorama1, Color.Blue);
            //    if (xpop.Visible)
            //        xpop.Close();
            //    xcontainer.PerformLayout();
            //    xcontainer.Controls.Clear();
            //    if (UsbPage == null)
            //    {
            //        UsbPage = new USBManager();
            //        UsbPage.Dock = DockStyle.Fill;
            //        UsbPage.OnLoaded += FTPPage_OnSaveLoaded;
            //        UsbPage.OnScanFinished += UsbPage_OnScanFinished;
            //    }
            //    xcontainer.Controls.Add(UsbPage);
            //    xcontainer.ResumeLayout(false);
            //    Update();
            //    UsbPage.DoList(true);
            //}
        }

        private void xloadps3_Click(object sender, EventArgs e)
        {
            if (isbusy)
                return;
            var fb = new FolderBrowserDialog {Description = @"Load Dark Souls II Ps3 Save Directory"};
            if (fb.ShowDialog() == DialogResult.OK)
                LoadSave(fb.SelectedPath, MainFile.ConsoleType.PS3);
        }

        private void xsavefile_Click(object sender, EventArgs e)
        {
            if (isbusy)
                return;
            if (ProgressPopupUI.IsBusy)
            {
                RadMessageBox.Show(Resources.ToolBusy, "Busy", MessageBoxButtons.OK,
                    RadMessageIcon.Exclamation);
                return;
            }
            if (MainFile.ProgressInstance != null)
            {
                if (MainFile.ProgressInstance.isbusy)
                {
                    RadMessageBox.Show(Resources.ToolBusy, "Busy", MessageBoxButtons.OK,
                        RadMessageIcon.Exclamation);
                    return;
                }
            }
            if (DarkSoulsFile == null)
                return;
            try
            {
                OnMainFileChange?.Invoke(DarkSoulsFile, FileAction.Saving);
                if (DarkSoulsFile.Backup())
                {
                    //Control c = xcontainer.Controls[0];
                    // xcontainer.Controls.Clear();
                    //Progressview p = new Progressview { Dock = DockStyle.Fill };
                    //MainFile.ProgressInstance.AtProgress -= p.DoProgress;
                    //MainFile.ProgressInstance.AtProgress += p.DoProgress;
                    // xcontainer.Controls.Add(p);
                    //var popup = MainFile.ProgressInstance.CreateProgressPopup();
                    //Point location = this.PointToScreen(this.Location);
                    //popup.Show(location.X + (this.Width - 500), location.Y + (this.Height - 60));
                    DarkSoulsFile.SaveAll(true);
                    if (DarkSoulsFile.Isvalid)
                    {
                        //xcontainer.Controls.Clear();
                        //xcontainer.Controls.Add(c);
                        OnMainFileChange?.Invoke(DarkSoulsFile, FileAction.Saved);
                        RadMessageBox.Show("All changes have succesfully been saved\nFile is ready to be played!",
                            "Saved",
                            MessageBoxButtons.OK, RadMessageIcon.Info);
                    }
                }
            }
            catch (Exception ex)
            {
                RadMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, RadMessageIcon.Error);
            }
        }

        private void xclosefile_Click(object sender, EventArgs e)
        {
            //if (DarkSoulsFile.DB != null)
            //{
            //    var fb = new FolderBrowserDialog();
            //    if (fb.ShowDialog() == DialogResult.OK)
            //    {
            //        string[] files = Directory.GetFiles(fb.SelectedPath, "*.PNG");
            //        var items = DarkSoulsFile.DB.Items.ToList();
            //        int index = 0;

            //        //foreach (var v in files)
            //        //{
            //        //    uint id = Path.GetFileName(v).GetIDFromName();
            //        //    var item = items.FirstOrDefault(x => x.ID1 == id);
            //        //    if (item == null)
            //        //    {
            //        //        items.Add(new ItemEntry(new ItemTypeInstance())
            //        //        {
            //        //            ID1 = id,
            //        //            ID2 = id,
            //        //            IsDLC = true,
            //        //            Name = "Unknown #" + index,
            //        //            xName = "Unknown #" + index,
            //        //            Description = "No description availible",
            //        //            xDescription = "No description availible",
            //        //            SimpleDescription = "No simple description availible",
            //        //            xSimpleDescription = "No simple description availible",
            //        //            ImageStream = new MemoryStream(File.ReadAllBytes(v)),
            //        //        });
            //        //        index++;
            //        //    }
            //        //}
            //        index = 0;
            //        string lastimage = null;
            //        foreach (var v in items)
            //        {
            //            if (!v.Name.ToLower().Contains("unknown"))
            //                continue;
            //            string image =
            //                files.FirstOrDefault(x => x.Contains(v.ID1.ToString(CultureInfo.InvariantCulture)));
            //            if (image == null)
            //                continue;
            //            v.ImageStream = new MemoryStream(File.ReadAllBytes(image));
            //            index++;
            //        }
            //        if (index > 0)
            //            DarkSoulsFile.DB.Items = items.ToArray();
            //        RadMessageBox.Show(index + " Items have succesfully been added!\nDont forget to save the changes!");
            //    }
            //}
            Close();
            //Functions.UpdateDbWithDmg(DarkSoulsFile.DB.Version,
            //   "C:\\Users\\Jappi88\\Downloads\\tu00000008_00000000 Contents\\menu\\Text",
            //    DarkSoulsFile);
            //RadMessageBox.Show("DB's rebuilding Complete!");
        }

        private void xloadftp_Click(object sender, EventArgs e)
        {
            if (isbusy)
                return;
            if (ProgressPopupUI.IsBusy)
            {
                RadMessageBox.Show(Resources.ToolBusy, "Busy", MessageBoxButtons.OK, RadMessageIcon.Exclamation);
                return;
            }
            if (MainFile.ProgressInstance != null)
            {
                if (MainFile.ProgressInstance.isbusy)
                {
                    RadMessageBox.Show(Resources.ToolBusy, "Busy", MessageBoxButtons.OK, RadMessageIcon.Exclamation);
                    return;
                }
            }
            xloadftp.SetSelected(radPanorama1, Color.Blue);
            xcontainer.PerformLayout();
            xcontainer.Controls.Clear();
            if (FTPPage == null)
            {
                FTPPage = new FTPControl();
                FTPPage.OnProgress += FTPPage_OnProgress;
                FTPPage.OnSaveLoaded += FTPPage_OnSaveLoaded;
            }
            FTPPage.Dock = DockStyle.Fill;
            xcontainer.Controls.Add(FTPPage);
            xcontainer.ResumeLayout(false);
            Update();
        }

        private void FTPPage_OnProgress(FtpTransferInfo e)
        {
            if (MainFile.ProgressInstance != null)
            {
                MainFile.ProgressInstance.DoProgress(
                    e.TransferType + "ing Dark Souls II SaveGame (" + (int) e.Percentage + "%)", (int) e.Percentage,
                    e.Transferred, e.Length);
            }
        }

        private void FTPPage_OnSaveLoaded(object dir, bool isftp)
        {
            if (isbusy)
                return;
            xpopcontainer_OnMinimize(this, null);
            if (ProgressPopupUI.IsBusy)
            {
                RadMessageBox.Show(Resources.ToolBusy, "Busy", MessageBoxButtons.OK, RadMessageIcon.Exclamation);
                return;
            }
            if (MainFile.ProgressInstance != null)
            {
                if (MainFile.ProgressInstance.isbusy)
                {
                    RadMessageBox.Show(Resources.ToolBusy, "Busy", MessageBoxButtons.OK, RadMessageIcon.Exclamation);
                    return;
                }
            }
            var db = MainForm.Settings.ResourcePath + "\\DS2Items_English.db";
            var dbtypes = MainForm.Settings.ResourcePath + "\\ItemTypes.db";
            if (!File.Exists(db) || !File.Exists(dbtypes))
            {
                RadMessageBox.Show(
                    "Some vital database resources are missing!\nThe missing resources will need to be downloaded first",
                    "No Resources", MessageBoxButtons.OK, RadMessageIcon.Exclamation);
                OnResourceCheck?.Invoke();
            }
            else
            {
                OnMainFileChange?.Invoke(DarkSoulsFile, FileAction.Loading);
                try
                {
                    isbusy = true;
                    Control c = null;
                    if (xcontainer.Controls.Count > 0)
                        c = xcontainer.Controls[0];
                    if (isftp)
                    {
                        var path = MainForm.Settings.ResourcePath + "\\";
                        var v = dir as FTPSaveInfo;
                        if (v == null)
                            return;
                        if (v.IsPS3)
                        {
                            v.Extract(path);
                            path += v.Name;
                        }
                        else
                        {
                            path += "-GAME_0000";
                            v.Extract(path);
                        }
                        Close();
                        var p = new Progressview {Dock = DockStyle.Fill};
                        // var x = new Progressor();
                        ProgressPopupUI.Progress.AtProgress += p.DoProgress;
                        xcontainer.Controls.Add(p);
                        DarkSoulsFile = new MainFile(v, path, db, dbtypes, ProgressPopupUI.Progress);
                    }
                    else
                    {
                        OnMainFileChange?.Invoke(DarkSoulsFile, FileAction.Closed);
                        xcontainer.Controls.Clear();
                        var p = new Progressview {Dock = DockStyle.Fill};
                        // var x = new Progressor();
                        ProgressPopupUI.Progress.AtProgress += p.DoProgress;
                        xcontainer.Controls.Add(p);
                        DarkSoulsFile = new MainFile(dir as FileEntry, db, dbtypes, ProgressPopupUI.Progress);
                    }
                    xcontainer.Controls.Clear();
                    if (DarkSoulsFile != null && DarkSoulsFile.Isvalid)
                    {
                        DarkSoulsFile.OnMainFileChange += OnMainFileChange;
                        if (DarkSoulsFile.Isvalid)
                        {
                            if (DarkSoulsFile.UsedSlots.Length == 0)
                            {
                                RadMessageBox.Show(
                                    "Your loaded Dark souls II Savegame does not contain any used slots! Please make a slot before loading it.",
                                    "Empty", MessageBoxButtons.OK, RadMessageIcon.Exclamation);
                            }
                            else
                            {
                                xsavefile.Visibility = ElementVisibility.Visible;
                                xselectslot.Visibility = ElementVisibility.Visible;
                                xclosefile.Visibility = ElementVisibility.Visible;
                                xselectslot.PerformClick();
                                OnMainFileChange?.Invoke(DarkSoulsFile, FileAction.Loaded);
                            }
                        }
                        else
                        {
                            if (c != null)
                                xcontainer.Controls.Add(c);
                            OnMainFileChange?.Invoke(DarkSoulsFile, FileAction.Closed);
                        }
                    }
                    else
                    {
                        if (c != null)
                            xcontainer.Controls.Add(c);
                        OnMainFileChange?.Invoke(DarkSoulsFile, FileAction.Closed);
                    }
                    isbusy = false;
                }
                catch (Exception ex)
                {
                    isbusy = false;
                    xcontainer.Controls.Clear();
                    RadMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, RadMessageIcon.Error);
                }
            }
        }

        private void xselectslot_Click(object sender, EventArgs e)
        {
            if (DarkSoulsFile == null || DarkSoulsFile.UsedSlots.Length == 0 || !DarkSoulsFile.Isvalid)
            {
                xselectslot.Enabled = false;
                return;
            }
            xselectslot.SetSelected(radPanorama1, Color.Blue);
            var s = new SlotSelector();
            s.OnSlotSelected += LoadSLot;
            s.Dock = DockStyle.Fill;
            s.LoadSlots(DarkSoulsFile);
            xcontainer.Controls.Clear();
            xcontainer.Controls.Add(s);
        }

        public void Close()
        {
            if (ProgressPopupUI.IsBusy)
            {
                RadMessageBox.Show(Resources.ToolBusy, "Busy", MessageBoxButtons.OK, RadMessageIcon.Exclamation);
                return;
            }
            if (MainFile.ProgressInstance != null)
            {
                if (MainFile.ProgressInstance.isbusy)
                {
                    RadMessageBox.Show(Resources.ToolBusy, "Busy", MessageBoxButtons.OK, RadMessageIcon.Exclamation);
                    return;
                }
            }
            xsavefile.Visibility = ElementVisibility.Hidden;
            xselectslot.Visibility = ElementVisibility.Hidden;
            xclosefile.Visibility = ElementVisibility.Hidden;
            if (xcontainer.Controls.Count > 0)
            {
                if (xcontainer.Controls[0].GetType() == typeof (SlotSelector))
                    xcontainer.Controls.Clear();
            }
            OnMainFileChange?.Invoke(DarkSoulsFile, FileAction.Closed);
        }

        public void DeviceConnectionChanged(bool connected, bool showerror)
        {
            if (connected)
            {
                if (UsbPage == null)
                {
                    UsbPage = new USBManager();
                    UsbPage.Dock = DockStyle.Fill;
                    UsbPage.OnLoaded += FTPPage_OnSaveLoaded;
                    UsbPage.OnScanFinished += UsbPage_OnScanFinished;
                }
                xpopcontainer.Populate(new Control[] {UsbPage});
                xpopcontainer.Title = "Loading DSII SaveGames From USB..";
                xpop.Size = new Size(500, 300);
                xpop.Update();
                var loc = Owner.PointToScreen(Owner.Location);
                xpop.Show(loc.X + (Owner.Width - 500), loc.Y + (Owner.Height - 35));
                UsbPage.DoList(showerror);
            }
            else
            {
                if (xpop.Visible)
                    xpop.Close();
            }
        }

        public void FileMain_SizeChanged(object sender, EventArgs e)
        {
            if (xpop.Visible)
            {
                var c = sender as Control;
                if (c != null)
                {
                    var flag = xpop.Height > 25;
                    xpop.Close();
                    if (!flag)
                    {
                        xpop.Size = new Size(500, 25);
                        var loc = c.PointToScreen(c.Location);
                        xpop.Show(loc.X + (c.Width - 500), loc.Y + (c.Height - 60));
                    }
                    else
                    {
                        xpop.Size = new Size(500, 300);
                        var loc = c.PointToScreen(c.Location);
                        xpop.Show(loc.X + (c.Width - 500), loc.Y + (c.Height - 335));
                    }
                }
            }
        }

        private void UsbPage_OnScanFinished(FTPSaveFile[] files, bool found)
        {
            xpopcontainer.Title = !found ? "No DSII SaveGames Found :(" : files.Length + " DSII SaveGames Found!";
            if (!found && xpop.Visible)
                xpop.Close();
        }
    }
}