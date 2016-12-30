using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dark_Souls_II_Save_Editor.SaveStuff;
using HavenInterface.Fatx;
using HavenInterface.Utils;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using ThreadState = System.Threading.ThreadState;

namespace Dark_Souls_II_Save_Editor.Controls
{
    public partial class USBManager : UserControl
    {
        public delegate void ScanCompleted(FTPSaveFile[] files, bool found);

        public delegate void SearchComplete(FatxDrive[] drives);

        private FTPSaveFile[] files;

        public USBManager()
        {
            InitializeComponent();
        }

        public FatxDrive[] Drives { get; private set; }
        public bool IsBussy { get; private set; }
        public event FTPSave.FTPSaveChanged OnLoaded;
        public event ScanCompleted OnScanFinished;

        public Task<T> BeginInvokeExWithReturnValue<T>(Func<T> actionFunction)
        {
            var task = new Task<T>(actionFunction);
            task.Start();
            return task;
        }

        private FTPSaveFile[] scan()
        {
            var pname = Process.GetProcessesByName("Horizon");
            if (pname.Length > 0)
            {
                if (
                    RadMessageBox.Show(
                        "Horizon has been detected!\n Horizon needs to be closed first, because it may interfere with the device detection...\nwould you like to Close Horizon ?",
                        "Close Horizon", MessageBoxButtons.YesNo, RadMessageIcon.Exclamation) == DialogResult.Yes)
                {
                    foreach (var p in pname)
                    {
                        p.CloseMainWindow();
                        var thread = new Thread(() => p.WaitForExit());
                        thread.Start();
                        while (thread.ThreadState != ThreadState.Stopped)
                        {
                            Application.DoEvents();
                            p.CloseMainWindow();
                        }
                    }
                }
            }
            pname = Process.GetProcessesByName("Modio");
            if (pname.Length > 0)
            {
                if (
                    RadMessageBox.Show(
                        "Modio has been detected!\n Modio needs to be closed first, because it may interfere with the device detection...\nwould you like to Close Modio ?",
                        "Close Modio", MessageBoxButtons.YesNo, RadMessageIcon.Exclamation) == DialogResult.Yes)
                {
                    foreach (var p in pname)
                    {
                        p.CloseMainWindow();
                        var thread = new Thread(() => p.WaitForExit());
                        thread.Start();
                        while (thread.ThreadState != ThreadState.Stopped)
                        {
                            Application.DoEvents();
                            p.CloseMainWindow();
                        }
                    }
                }
            }
            var x = new FTPSaveFile[] {};
            var t = new Thread(() => x = DoScan());
            t.Start();
            while (t.ThreadState != ThreadState.Stopped)
            {
                Application.DoEvents();
            }
            OnScanFinished?.Invoke(x, x != null && x.Length > 0);
            return x;
        }

        private FTPSaveFile[] DoScan()
        {
            var xdrives = new List<FatxDrive>();
            var savefiles = new List<FTPSaveFile>();
            try
            {
                CloseDrives();
                var drives = FATXManagement.GetFATXDrives(); //List all fatx drives
                var error = "Connected drive does not contain any Dark Souls II Savedata";
                foreach (var drive in drives) //All valid drives.
                {
                    var dir = drive.GetFolderByPath(@"Content");
                    if (dir == null)
                    {
                        drive.Close();
                        throw new Exception(error);
                    }
                    var profiles = dir.GetFolders();
                    if (profiles == null || profiles.Length == 0)
                    {
                        drive.Close();
                        throw new Exception(error);
                    }
                    profiles =
                        profiles.Where(
                            profile =>
                                profile.Name != "0000000000000000" && profile.Name.Length == 16 &&
                                profile.Name.IsValidHex()).ToArray(); //all valid profiles
                    foreach (var profile in profiles)
                    {
                        dir = profile.GetDirectory("465307E4", true);
                        if (dir != null)
                        {
                            dir = dir.GetDirectory("00000001", true);
                            if (dir != null)
                            {
                                var file = dir.GetFile("-GAME_0000");
                                if (file != null && file.IsSTFS)
                                {
                                    if (!xdrives.Contains(drive))
                                        xdrives.Add(drive);
                                    var x = new FTPSaveFile(file);
                                    x.Dock = DockStyle.Top;
                                    x.OnSaveDeleted += x_OnSaveDeleted;
                                    x.OnSaveLoaded += x_OnSaveLoaded;
                                    x.OnSaveReplaced += x_OnSaveReplaced;
                                    savefiles.Add(x);
                                }
                            }
                        }
                    }
                    if (!xdrives.Contains(drive))
                        drive.Close();
                }
                Drives = drives.ToArray();
            }
            catch
            {
            }
            return savefiles.ToArray();
        }

        private void x_OnSaveReplaced(object file, bool isftp)
        {
            DoList(false);
        }

        private void x_OnSaveLoaded(object file, bool isftp)
        {
            OnLoaded?.Invoke(file, isftp);
        }

        private void x_OnSaveDeleted(object file, bool isftp)
        {
            DoList(false);
        }

        private void CloseDrives()
        {
            if (Drives != null && Drives.Length > 0)
                foreach (var v in Drives)
                    v.Close();
            Drives = null;
        }

        private void xscan_Click(object sender, EventArgs e)
        {
            if (IsBussy)
                return;
            DoShit(true);
        }

        public void DoList(bool showerror)
        {
            if (IsBussy)
                return;
            if (files != null && files.Length > 0)
            {
                try
                {
                    files = files.Where(x =>
                    {
                        var fileEntry = x.SaveInstance as FileEntry;
                        return fileEntry != null && fileEntry.IsSTFS;
                    }).ToArray();
                }
                catch
                {
                    files = null;
                }
            }
            if (files == null || files.Length == 0)
            {
                DoShit(showerror);
            }
            else
            {
                OnScanFinished?.Invoke(files, files != null && files.Length > 0);
            }
        }

        private void DoShit(bool showerror)
        {
            if (IsBussy)
                return;
            xscan.Text = "Searching For DSII SaveGames...";
            IsBussy = true;
            Wait();
            files = scan();
            xcontainer.PerformLayout();
            xcontainer.Controls.Clear();
            if (files != null && files.Length > 0)
            {
                foreach (var s in files)
                {
                    xcontainer.Controls.Add(s);
                }
            }
            else
            {
                if (showerror)
                    RadMessageBox.Show(
                        "No DSII SavedGames Found!\nMake sure the device is pluged in and not used by any other programs.",
                        "No Saves", MessageBoxButtons.OK, RadMessageIcon.Exclamation);
            }
            xcontainer.ResumeLayout(false);
            xscan.Text = "Search For DSII SaveGames";
            IsBussy = false;
        }

        private void Wait()
        {
            xcontainer.PerformLayout();
            xcontainer.Controls.Clear();
            var x = new RadWaitingBar();
            x.Dock = DockStyle.Top;
            x.Height = 80;
            x.WaitingIndicatorSize = new Size(150, 80);
            xcontainer.Controls.Add(x);
            xcontainer.ResumeLayout(false);
            Application.DoEvents();
            x.StartWaiting();
        }
    }
}