using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Dark_Souls_II_Save_Editor.Controls;
using Dark_Souls_II_Save_Editor.Parameters;
using Dark_Souls_II_Save_Editor.SaveStuff;
using Dark_Souls_II_Save_Editor.STFSPackage;
using HavenInterface;
using HavenInterface.Fatx;
using HavenInterface.Interfaces;
using HavenInterface.Interfaces.ServerPlugin;
using HavenInterface.Interfaces.SEPlugin;
using HavenInterface.Popup;
using HavenInterface.Utils;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using Telerik.WinControls.UI.Docking;
using ProgressChangedHandler = HavenInterface.ProgressChangedHandler;
using Timer = System.Windows.Forms.Timer;

namespace Dark_Souls_II_Save_Editor
{
    [Serializable]
    public partial class MainForm : RadForm, IFileEditor
    {
        private const int ClosepopupTime = 4000;
        public static Settings Settings = new Settings();
        public static ProgressPopupUI Mainpage;
        public static IPluginManager SCManager;
        public static ILogin Server;
        private readonly object _locker = new object();
        public FileMain Main;

        public MainForm()
        {
            ThemeResolutionService.ApplicationThemeName = Themes.VisualStudio2012DarkTheme.ThemeName;
            InitializeComponent();
            Load += (x, y) => InitMain();
            Shown += MainForm_Shown;
        }

        //private string _error;
        public static Popup ProgressPopup { get; private set; }
        public Popup UIPopup { get; set; }
        public Popup MessagePopup { get; set; }
        public static ILogin Live => SCManager?.Server;

        public override sealed string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        public bool LoadFile(string filepath)
        {
            try
            {
                if (Main?.DarkSoulsFile != null && Main.DarkSoulsFile.LoadedType != MainFile.ConsoleType.None)
                {
                    var result = RadMessageBox.Show(
                        "File is currently loaded...\ndo you wish to save changes before loading another one?", "Save",
                        MessageBoxButtons.YesNoCancel, RadMessageIcon.Exclamation);
                    if (result == DialogResult.Yes)
                        Main.DarkSoulsFile.SaveAll(true);
                    else if (result == DialogResult.Cancel)
                        return false;
                    return true;
                }
                SetWindow("xfile", "Dark Souls II Save Editor By Jappi88", Main, false, true);
                if (Main != null)
                {
                    if (Directory.Exists(filepath) && File.Exists(filepath + "\\Param.PFD"))
                        Main.LoadSave(filepath, MainFile.ConsoleType.PS3);
                    else if (File.Exists(filepath))
                    {
                        var stfs = new StfsPackage(filepath, 0);
                        var flag = stfs.Header.TitleId == 0x465307E4;
                        stfs.Close();
                        if (flag)
                            Main.LoadSave(filepath, MainFile.ConsoleType.XBOX360);
                        else
                            RadMessageBox.Show("Invalid Dark Souls II Savegame!", "Error", MessageBoxButtons.OK,
                                RadMessageIcon.Error);
                        return flag;
                    }
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                RadMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, RadMessageIcon.Error);
                return false;
            }
        }

        private void Instance_SaveSettingsRequest(Settings setting, string settingpath, ref bool saved)
        {
            saved = true;
            AppPreferences = AppPreferences.Map(setting);
            AppPreferences.Save();
        }

        private void Instance_UserUpdated(ServerInfo info)
        {
            var val = info.TotalDonated;
            var ox = new int[] {};
            if (AppPreferences.AllowedDlcIndexes == null)
                AppPreferences.AllowedDlcIndexes = new int[] {};
            if (val >= 1.0f && !AppPreferences.AllowedDlcIndexes.Contains(1))
                ox = new[] {1};
            if (val >= 2.0f && !AppPreferences.AllowedDlcIndexes.Contains(2))
                ox = new[] {1, 2};
            if (val >= 3.0f && !AppPreferences.AllowedDlcIndexes.Contains(3))
                ox = new[] {1, 2, 3};
            if (ox.Length > AppPreferences.AllowedDlcIndexes.Length)
            {
                var newdlcs = ox.Where(t => !AppPreferences.AllowedDlcIndexes.Contains(t));
                var sb = new StringBuilder();
                sb.AppendLine("You have unlocked the fallowing DLC('s) : ");
                foreach (var x in newdlcs)
                {
                    sb.AppendLine(Functions.AvailibleDLCName(x));
                }
                AppPreferences.AllowedDlcIndexes = ox;
                AppPreferences.Save();
                if (Main?.DarkSoulsFile != null && Main.DarkSoulsFile.Isvalid)
                {
                    Main.DarkSoulsFile.Reload();
                }
                Live.ShowMessagePopup(sb.ToString(), "Availible DLC's", HavenInterface.Interfaces.ServerPlugin.MessageType.Info);
            }
        }

        private void InitMain()
        {
            const int flags =
                WinAnimation.AW_HOR_NEGATIVE | WinAnimation.AW_VER_POSITIVE | WinAnimation.AW_CENTER |
                WinAnimation.AW_ACTIVATE;
            WinAnimation.AnimateWindow(Handle, 250, flags);
            var mes = Process.GetProcessesByName(AppGlobalInfo.GlobalInfo.Title);
            if (mes.Length > 1)
            {
                RadMessageBox.Show(
                    "Another instance of " + AppGlobalInfo.GlobalInfo.Title +
                    " has been detected!\nYou are only allowed to run 1 instance at the same time.\nPlease close any running \"" +
                    AppGlobalInfo.GlobalInfo.Title + "\", and try again.", "Warning", MessageBoxButtons.OK,
                    RadMessageIcon.Exclamation);
                Dispose();
            }
            else
            {
                if (AppGlobalInfo.GlobalInfo.Build != "Debug" && Host == null)
                    Dispose();
                else
                {
                    ResizeRedraw = true;
                    if (!AppPreferences.LicenceAgreed)
                    {
                        var x = new About {StartPosition = FormStartPosition.CenterParent};
                        if (x.ShowDialog() != DialogResult.OK)
                            Dispose();
                        else
                        {
                            AppPreferences.LicenceAgreed = true;
                            AppPreferences.Save();
                        }
                    }
                    Main = new FileMain(xdock);
                    Main.OnSlotChange += FileSlotActed;
                    Main.OnMainFileChange += MainFileActed;
                    Main.OnResourceCheck += Main_OnResourceCheck;
                    Mainpage = new ProgressPopupUI {Dock = DockStyle.Fill};
                    ProgressPopupUI.ViewRequest += (x, y) => ShowProgressPopup(true);
                    ProgressPopupUI.ViewClose += (x, y) =>
                    {
                        if (ProgressPopup != null && ProgressPopup.Visible)
                        {
                            if (ProgressPopup.InvokeRequired)
                                ProgressPopup.Invoke(new MethodInvoker(ProgressPopup.Close));
                            else
                                ProgressPopup.Close();
                        }
                    };
                    ProgressPopupUI.ViewHide += (x, y) => ShowProgressPopup(false);
                    ProgressPopupUI.ViewShow += (x, y) => ShowProgressPopup(true);
                    ProgressPopup = new Popup(Mainpage)
                    {
                        AutoClose = false,
                        FocusOnOpen = false,
                        ShowingAnimation = PopupAnimations.RightToLeft | PopupAnimations.Slide,
                        HidingAnimation = PopupAnimations.LeftToRight | PopupAnimations.Slide,
                        AnimationDuration = 200
                    };
                    ThemeResolutionService.ApplicationThemeName = Themes.VisualStudio2012DarkTheme.ThemeName;
                    //loadingCircle1.InnerCircleRadius = 20;
                    //loadingCircle1.OuterCircleRadius = 10;
                    //loadingCircle1.NumberSpoke = 15;
                    Text = AppGlobalInfo.GlobalInfo.Title + @" By Jappi88  [Version : " +
                           AppGlobalInfo.GlobalInfo.Version + @"]";
                    Application.DoEvents();
                }
            }
        }

        private void _live_ConnectionEvent(string message, string title, HavenInterface.Interfaces.ServerPlugin.MessageType type)
        {
            try
            {
                //if (loadingCircle1.InvokeRequired)
                //    loadingCircle1.Invoke(
                //        new MethodInvoker(() => loadingCircle1.Visible = type == MessageType.Bussy));
                //else
                //    loadingCircle1.Visible = type == MessageType.Bussy;
                //Application.DoEvents();
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public void ShowMessagePopup(string message, string title, MessageType type)
        {
            if (MessagePopup != null)
            {
                while (MessagePopup != null && MessagePopup.Visible)
                    Application.DoEvents();
            }
            var xui = new PopupMessage();
            xui.ShowMessage(message, title, type);
            MessagePopup = new Popup(xui)
            {
                FocusOnOpen = false,
                ShowingAnimation = PopupAnimations.Blend,
                HidingAnimation = PopupAnimations.Blend,
                AnimationDuration = 300,
                AutoClose = false
            };
            var loc = Location;
            if (MessagePopup.TopLevelControl != null)
            {
                loc.X += Width - (MessagePopup.TopLevelControl.Width + 2);
                loc.Y += Height - (MessagePopup.TopLevelControl.Height + 5);
            }
            xui.OnClose += (s, x) => { if (MessagePopup.Visible) MessagePopup.Close(); };
            MessagePopup.Show(loc);
            StartPopupCount();
        }

        public void ShowUIPopup(Control ui, string title, Point loc, bool autoclose = false)
        {
            if (UIPopup != null && UIPopup.Visible)
                UIPopup.Close();
            var rt = new RadTitleBar {ThemeName = Themes.VisualStudio2012DarkTheme.ThemeName};
            rt.TitleBarElement.MaximizeButton.Visibility = ElementVisibility.Hidden;
            rt.TitleBarElement.MinimizeButton.Visibility = ElementVisibility.Hidden;
            rt.Close += (x, y) =>
            {
                if (UIPopup != null && UIPopup.Visible)
                    UIPopup.Close();
            };
            rt.Text = title;
            rt.Dock = DockStyle.Top;
            var p = new RadPanel
            {
                ThemeName = Themes.VisualStudio2012DarkTheme.ThemeName,
                BackColor = Color.FromArgb(45, 45, 48),
                Size = new Size(ui.Width, ui.Height + 30)
            };
            UIPopup = p.CreatePopup(true, autoclose);
            UIPopup.Text = title;
            UIPopup.AllowTransparency = true;
            UIPopup.AutoClose = autoclose;
            UIPopup.Opacity = 50;
            ui.Dock = DockStyle.Fill;
            ui.Tag = UIPopup;
            var addControl = GetAddControl(p);
            addControl(rt);
            addControl = GetAddControl(p);
            addControl(ui);
            //p.Controls.AddRange(new Control[] { rt, ui });
            rt.SendToBack();
            if (InvokeRequired)
                Invoke(new Action(() => UIPopup.Show(loc)));
            else
                UIPopup.Show(loc);
        }

        public Action<Control> GetAddControl(Control c)
        {
            var context = SynchronizationContext.Current;
            return control => context.Send(_ => c.Controls.Add(control), null);
        }

        private void _live_LogedOut(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                //Invoke(new Action(() => xlogin.Text = @"Log In"));
                Invoke(
                    new Action(
                        () =>
                            Text =
                                AppGlobalInfo.GlobalInfo.Title + @" By Jappi88  [Version : " +
                                AppGlobalInfo.GlobalInfo.Version + @"]"));
                //Invoke(
                //    new Action(
                //        () =>
                //            xlogin.ToolTipText = @"Login or create new account"));
                if (UIPopup != null && UIPopup.Visible)
                    UIPopup.Invoke(new Action(() => UIPopup.Close()));
            }
            else
            {
                //xlogin.Text = @"Log In";
                Text = AppGlobalInfo.GlobalInfo.Title + @" By Jappi88  [Version : " + AppGlobalInfo.GlobalInfo.Version +
                       @"]";
                //xlogin.ToolTipText = @"Login or create new account";
                if (UIPopup != null && UIPopup.Visible)
                    UIPopup.Close();
            }
        }

        private void _live_LogedIn(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                //Invoke(new Action(() => xlogin.Text = @"Profile"));
                Invoke(
                    new Action(
                        () =>
                            Text =
                                AppGlobalInfo.GlobalInfo.Title + @" By Jappi88  [Version : " +
                                AppGlobalInfo.GlobalInfo.Version + @"]   Logged In As : " +
                                Live.MainSettings.User.Username));
                //Invoke(
                //    new Action(
                //        () =>
                //            xlogin.ToolTipText =
                //                @"Check out your profile,donations,unlockables, change password or chat with other users, its all up to you!"));
            }
            else
            {
                //xlogin.Text = @"Profile";
                Text = AppGlobalInfo.GlobalInfo.Title + @" By Jappi88  [Version : " + AppGlobalInfo.GlobalInfo.Version +
                       @"]   Logged In As : " +
                       Live.MainSettings.User.Username;
                //xlogin.ToolTipText =
                //    @"Check out your profile,donations,unlockables, change password or chat with other users, its all up to you!";
            }
        }

        public static void ShowProgressPopup(bool full)
        {
            if (ProgressPopup.Visible)
            {
                if (ProgressPopup.InvokeRequired)
                    ProgressPopup.Invoke(new MethodInvoker(ProgressPopup.Close));
                else
                    ProgressPopup.Close();
            }
            if (ProgressPopup.InvokeRequired)
                ProgressPopup.Invoke(new MethodInvoker(() => ProgressPopup.Size = new Size(full ? 600 : 40, 50)));
            else
                ProgressPopup.Size = new Size(full ? 600 : 40, 50);
            var x = Screen.PrimaryScreen.WorkingArea.Width - ProgressPopup.Width;
            var y = Screen.PrimaryScreen.WorkingArea.Height - 50;
            if (ProgressPopup.InvokeRequired)
                ProgressPopup.Invoke(new MethodInvoker(() => ProgressPopup.Show(new Point(x, y))));
            else
                ProgressPopup.Show(new Point(x, y));
        }

        private void xfile_Click(object sender, EventArgs e)
        {
            SetWindow("xfile", "Dark Souls II Save Editor By Jappi88", Main, false, true);
        }

        private void Main_OnResourceCheck()
        {
            Mainpage.CheckForResources(this, EventArgs.Empty);
        }

        public void SetPlayetStatusPage(SaveSlot slot)
        {
            if (slot == null)
                return;
            var slotname = slot.Name;
            if (slotname == "")
                slotname = "Slot Without A Name #" + (slot.SlotIndex + 1).ToString("X2");
            if (slot.UserUI == null || slot.UserUI.IsDisposed)
                slot.UserUI = new SelectedUser();
            if (SetWindow(slot.SlotName.Replace(" ", "_"), slotname, slot.UserUI, true, true))
                slot.UserUI.LoadSlot(slot);
            Application.DoEvents();
        }

        private void slot_OnNameChanged(SaveSlot slot)
        {
            if (slot?.UserUI == null || slot.UserUI.IsDisposed)
                return;
            SetWindow(slot.SlotName.Replace(" ", "_"), slot.Name, slot.UserUI, false, false);
        }

        public void SetParamPage(MainParamManager manager)
        {
            if (manager == null)
                return;
            var status = new MainParam();
            if (SetWindow("xgameparams", "Dark Souls II Game Parameters", status, true, true))
                status.LoadBND4(manager);
        }

        public void FileSlotActed(object sender, FileMain.FileAction action)
        {
            switch (action)
            {
                case FileMain.FileAction.Closed:
                    xdock.CloseAllWindows();
                    SetWindow("xfile", "Dark Souls II Save Editor By Jappi88", Main, false, true);
                    break;
                case FileMain.FileAction.Loaded:
                    if (Mainpage != null && ProgressPopupUI.IsBusy)
                        RadMessageBox.Show("Downloading Resources.. please wait for the operation to Complete.", "Busy",
                            MessageBoxButtons.OK, RadMessageIcon.Exclamation);
                    else
                    {
                        var file = sender as SaveSlot;
                        if (file != null)
                        {
                            SetPlayetStatusPage(file);
                            Application.DoEvents();
                        }
                    }
                    break;
                case FileMain.FileAction.Saved:

                    break;
            }
        }

        public void MainFileActed(object sender, FileMain.FileAction action)
        {
            switch (action)
            {
                case FileMain.FileAction.Loading:
                    SetWindow("xfile", "Dark Souls II Save Editor By Jappi88", Main, false, true);
                    break;
                case FileMain.FileAction.Closed:
                    xdock.CloseAllWindows();
                    SetWindow("xfile", "Dark Souls II Save Editor By Jappi88", Main, false, true);
                    if (sender as MainFile != null)
                    {
                        var file = (MainFile) sender;
                        if (file.UsedSlots != null && file.UsedSlots.Length > 0)
                            foreach (var s in file.UsedSlots)
                                s.UserUI.Dispose();
                        file.UsedSlots = null;
                        file.Saveinstance = null;
                        file.FilePath = null;
                        file.LoadedType = MainFile.ConsoleType.None;
                    }
                    // xgameparams.Enabled = false;
                    break;
                case FileMain.FileAction.Loaded:
                    var mainFile = sender as MainFile;
                    //if (mainFile != null && mainFile.ParamManager != null && mainFile.ParamManager.GameParameters == null)
                    //    xgameparams.Enabled = false;
                    //else
                    //    xgameparams.Enabled = true;
                    //Load UIS
                    if (mainFile == null)
                        return;
                    foreach (var t in mainFile.UsedSlots)
                    {
                        t.OnNameChanged += slot_OnNameChanged;
                        Application.DoEvents();
                    }
                    break;
                case FileMain.FileAction.Saved:

                    break;
            }
        }

        //private void xdonate_Click(object sender, EventArgs e)
        //{
        //    //string imgpath = "C:\\Users\\Jappi\\Dropbox\\4E51B35F66572764BD7BD3786114A21ADB8C784746 Contents\\pngs";
        //    //string dbpath = AppSettings.ResourcePath + "\\DS2Items_English.DB";
        //    //if (MainFile.DataBase == null)
        //    //    MainFile.LoadDb(dbpath, MainForm.AppSettings.ResourcePath + "\\ItemTypes.DB");
        //    //string dmgpath = "C:\\Users\\Jappi\\Desktop\\tu00000008_00000000 Contents\\menu\\Text\\";
        //    //Functions.UpdateDbWithDmg(11, dmgpath, imgpath);
        //    //Functions.UpdateDbImages(dbpath, imgpath, dmgpath + "English\\ItemName.fmg");
        //    if (Live != null)
        //        Live.DonateInterface();
        //}

        private void xgameparams_Click(object sender, EventArgs e)
        {
            //if (Main == null || Main.DarkSoulsFile == null)
            //    return;
            ////SetParamPage(Main.DarkSoulsFile.ParamManager);
            //RadMessageBox.Show(
            //    "Editing Parameters has been disabled for now.\nIt will be enabled once it has been finished.",
            //    "Disabled", MessageBoxButtons.OK, RadMessageIcon.Info);

            var loc = xoptioncontainer.PointToScreen(xsettings.Location);
            loc.Y += xsettings.Size.Height + 5;
            loc.X += 150;
            ShowSettings(loc);
        }

        private void ShowSettings(Point loc)
        {
            if (UIPopup != null && UIPopup.Visible)
                UIPopup.Close();
            var x = new SettingsUI();
            x.CloseRequest += (z, y) =>
            {
                var ui = z as SettingsUI;
                if (ui != null)
                {
                    if (UIPopup != null && UIPopup.Visible)
                    {
                        UIPopup.Close();
                    }
                }
            };
            ShowUIPopup(x, "Settings", loc, true);
        }

        //#region WINPROC

        //// device state change
        //private const int WM_DEVICECHANGE = 0x0219;

        //// logical volume(A disk has been inserted, such a usb key or external HDD)
        //private const int DBT_DEVTYP_VOLUME = 0x00000002;

        //// detected a new device
        //private const int DBT_DEVICEARRIVAL = 0x8000;

        //// preparing to remove
        //private const int DBT_DEVICEQUERYREMOVE = 0x8001;

        //// removed
        //private const int DBT_DEVICEREMOVECOMPLETE = 0x8004;

        //protected override void WndProc(ref Message message)
        //{
        //    base.WndProc(ref message);

        //    if ((message.Msg != WM_DEVICECHANGE) || (message.LParam == IntPtr.Zero))
        //        return;

        //    var volume =
        //        (DEV_BROADCAST_VOLUME)Marshal.PtrToStructure(message.LParam, typeof(DEV_BROADCAST_VOLUME));

        //    if (volume.dbcv_devicetype == DBT_DEVTYP_VOLUME)
        //    {
        //        switch (message.WParam.ToInt32())
        //        {
        //            // New device inserted...
        //            case DBT_DEVICEARRIVAL:
        //                SetWindow("xfile", "Dark Souls II Save Editor By Jappi88", Main, false, true);
        //                Main.DeviceConnectionChanged(true,false);
        //                break;

        //            // Device Removed.
        //            case DBT_DEVICEREMOVECOMPLETE:
        //                if (Main != null)
        //                    Main.DeviceConnectionChanged(false,false);
        //                break;
        //        }
        //    }
        //}

        //// Convert to the Drive name (”D:”, “F:”, etc)
        //private string ToDriveName(int mask)
        //{
        //    var offset = 0;
        //    while ((offset < 26) && ((mask & 0x00000001) == 0))
        //    {
        //        mask = mask >> 1;
        //        offset++;
        //    }

        //    if (offset < 26)
        //        return String.Format("{0}:", Convert.ToChar(Convert.ToInt32('A') + offset));

        //    return "?";
        //}

        //// Contains information about a logical volume.
        //[StructLayout(LayoutKind.Sequential)]
        //private struct DEV_BROADCAST_VOLUME
        //{
        //    public int dbcv_size;
        //    public int dbcv_devicetype;
        //    public int dbcv_reserved;
        //    public int dbcv_unitmask;
        //}

        //#endregion

        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Move : DragDropEffects.None;
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            var s = (string[]) e.Data.GetData(DataFormats.FileDrop, false);
            if (Main?.DarkSoulsFile != null && Main.DarkSoulsFile.LoadedType != MainFile.ConsoleType.None &&
                s.Length > 0)
            {
                var result = RadMessageBox.Show(
                    "File is currently loaded...\ndo you wish to save changes before loading another one?", "Save",
                    MessageBoxButtons.YesNoCancel, RadMessageIcon.Exclamation);
                if (result == DialogResult.Yes)
                    Main.DarkSoulsFile.SaveAll(true);
                else if (result == DialogResult.Cancel)
                    return;
            }
            foreach (var x in s)
            {
                try
                {
                    if (Main != null)
                    {
                        if (Directory.Exists(x) && File.Exists(x + "\\Param.PFD"))
                        {
                            SetWindow("xfile", "Dark Souls II Save Editor By Jappi88", Main, false, true);
                            Main.LoadSave(x, MainFile.ConsoleType.PS3);
                        }
                        else if (File.Exists(x))
                        {
                            SetWindow("xfile", "Dark Souls II Save Editor By Jappi88", Main, false, true);
                            var stfs = new StfsPackage(x, 0);
                            var flag = stfs.Header.TitleId == 0x465307E4;
                            stfs.Close();
                            if (flag)
                                Main.LoadSave(x, MainFile.ConsoleType.XBOX360);
                        }
                        if (Main.DarkSoulsFile != null && Main.DarkSoulsFile.LoadedType != MainFile.ConsoleType.None)
                            break;
                    }
                }
                catch
                {
                    // ignored
                }
            }
        }

        private void xrestore_Click(object sender, EventArgs e)
        {
            var form = new STFSFixer();
            if (Main?.DarkSoulsFile != null && (Main.DarkSoulsFile.LoadedType == MainFile.ConsoleType.XBOX360 ||
                                                Main.DarkSoulsFile.LoadedType == MainFile.ConsoleType.XBOX360FTP))
            {
                form.FilePath = Main.DarkSoulsFile.FilePath;
                form.SaveInstance = Main.DarkSoulsFile.FilePath;
            }
            else if (Main?.DarkSoulsFile != null && Main.DarkSoulsFile.LoadedType == MainFile.ConsoleType.Usb)
            {
                var j = Main.DarkSoulsFile.Saveinstance as FileEntry;
                if (j != null)
                {
                    form.FilePath = j.Path;
                    form.SaveInstance = j;
                }
            }
            var loc = xrestore.PointToScreen(xrestore.Location);
            loc.Y += xrestore.Size.Height + 20;
            ShowUIPopup(form, "STFS Fixer", loc, true);
        }

        private bool SetWindow(string name, string text, Control c, bool disposeafterclose, bool ensurevisble)
        {
            var xreturn = false;
            var w = xdock.GetWindow<HostWindow>(name);
            xdock.BeginUpdate();
            if (w == null)
            {
                xdock.SuspendLayout();
                w = xdock.DockControl(c, new DockPosition());
                w.Name = name;
                xreturn = true;
                w.CloseAction = disposeafterclose ? DockWindowCloseAction.CloseAndDispose : DockWindowCloseAction.Close;
                xdock.ResumeLayout(false);
            }
            w.Text = text;
            w.DockState = DockState.TabbedDocument;
            xdock.EndUpdate();
            if (ensurevisble)
                w.EnsureVisible();
            return xreturn;
        }

        private void xloaddb_Click(object sender, EventArgs e)
        {
            if (ProgressPopupUI.IsBusy)
                return;
            Mainpage.LoadDataBase();
        }

        private void x_Tick(object sender, EventArgs e)
        {
            var x = sender as Timer;
            x?.Stop();
            if (MessagePopup != null && MessagePopup.Visible)
                MessagePopup.Close();
        }

        private void StartPopupCount()
        {
            var x = new Timer {Interval = ClosepopupTime};
            x.Tick += x_Tick;
            x.Start();
        }

        private void loadingCircle1_VisibleChanged(object sender, EventArgs e)
        {
            // loadingCircle1.Active = loadingCircle1.Visible;
        }

        private void xbanned_Click(object sender, EventArgs e)
        {
            var loc = xoptioncontainer.PointToScreen(xbanned.Location);
            loc.Y += xbanned.Size.Height + 5;
            loc.X += 380;
            ShowUIPopup(new PreventBan(), "How can i get Banned?", loc, true);
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            if (Host?.PluginManager != null)
            {
                SCManager = Host.PluginManager;
                SCManager.PluginActionChanged += X_PluginActionChanged;
                Server = SCManager.Server;
                Settings = Server.MainSettings;
                if (Server == null)
                {
                    var x = SCManager.Plugins.FirstOrDefault(t => t.LiveInstance.Type == 1);
                    x?.MountAsync();
                }
                else
                    SetServer();
            }
        }

        private void SetServer()
        {
            Server.ServerInfoRecieved += Instance_UserUpdated;
            Server.LoggedIn += _live_LogedIn;
            Server.LoggedOut += _live_LogedOut;
            Server.MessageRecieved += _live_ConnectionEvent;
            Server.SaveSettingsRequest += Instance_SaveSettingsRequest;
            Server.AllowOfflineUsage = true;
            if (InvokeRequired)
                Invoke(new Action(() => xlogin.Enabled = true));
            else
                xlogin.Enabled = true;
            try
            {
                Invoke(new Action(Server.InitServer));
            }
            catch
            {
                // ignored
            }
        }

        private void X_PluginActionChanged(IPluginManager manager, AppInfo plugin, PluginActionType action,
            Exception error = null, object param = null)
        {
            if (error != null)
            {
                RadMessageBox.Show(error.Message, "Error", MessageBoxButtons.OK, RadMessageIcon.Error);
            }
            else
            {
                switch (action)
                {
                    case PluginActionType.Enabled:


                        break;
                    case PluginActionType.Disabled:
                        if (plugin.Guid.ToLower() == AppGlobalInfo.GlobalInfo.Guid.ToLower())
                            Close();
                        break;
                    case PluginActionType.Disposed:
                        break;
                    case PluginActionType.Mounting:
                        break;
                    case PluginActionType.Mounted:
                        x_PluginMounted(manager, plugin, param);
                        break;
                    case PluginActionType.Unmounting:
                        break;
                    case PluginActionType.Unmounted:
                        x_PluginUnmounted(manager, plugin);
                        break;
                    case PluginActionType.Loading:
                        break;
                    case PluginActionType.Loaded:
                        break;
                    case PluginActionType.Removing:
                        break;
                    case PluginActionType.Removed:
                        break;
                    case PluginActionType.Installing:
                        break;
                    case PluginActionType.Installed:
                        break;
                    case PluginActionType.Updating:
                        break;
                    case PluginActionType.Updated:
                        break;
                    case PluginActionType.Exception:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(action), action, null);
                }
            }
        }

        private void x_PluginUnmounted(IPluginManager manager, AppInfo plugin)
        {
            if (plugin.Type == 1)
            {
                Server = SCManager.Server;
                if (Server == null)
                {
                    if (InvokeRequired)
                        Invoke(new Action(() => xlogin.Enabled = false));
                    else
                        xlogin.Enabled = false;
                }
            }
        }

        private void x_PluginMounted(IPluginManager manager, AppInfo plugin, object param = null)
        {
            if (plugin.Type == 1 && param != null)
            {
                Server = (ILogin) param;
                if (Server != null)
                    SetServer();
            }
        }

        private void xlogin_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=WUW5AHY877F92");
        }

        public static readonly string SettingPath = AppDomain.CurrentDomain.BaseDirectory + "\\DS2SE.Xml";
        public static Preferences AppPreferences = Functions.LoadSettings(SettingPath);

        #region IPlugin Implementation

        public IPluginHost Host { get; set; }
        public IWin32Window IWindow => this;
        public Control IControl => this;

        public void CloseFile()
        {
            if (Main?.DarkSoulsFile != null)
                Main.Close();
        }

        public GlobalAppSettings AppSettings => AppGlobalInfo.GlobalInfo;

        public bool LoadFile(Stream inputstream)
        {
            return false;
        }

        public event ProgressChangedHandler ProgressChanged;

        protected virtual void OnProgressChanged(ProgressArg arg)
        {
            ProgressChangedHandler x;
            lock (_locker)
            {
                x = ProgressChanged;
            }
            x?.Invoke(this, arg);
        }

        #endregion
    }
}