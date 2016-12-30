using System;
using System.Windows.Forms;
using Dark_Souls_II_Save_Editor.SaveStuff;
using Dark_Souls_II_Save_Editor.STFSPackage;
using HavenInterface.Fatx;
using HavenInterface.FtpClient;
using Telerik.WinControls;

namespace Dark_Souls_II_Save_Editor.Controls
{
    public partial class FTPSaveFile : UserControl
    {
        public FTPSaveFile()
        {
            InitializeComponent();
        }

        public FTPSaveFile(FTPSaveInfo dir)
        {
            InitializeComponent();
            try
            {
                SaveInstance = dir;
                dir.OnProgress -= Client_TransferProgress;
                dir.OnProgress += Client_TransferProgress;
                dir.OnSaveDeleted -= FTPSaveFile_OnSaveDeleted;
                dir.OnSaveReplaced -= FTPSaveFile_OnSaveReplaced;
                dir.OnSaveDeleted += FTPSaveFile_OnSaveDeleted;
                dir.OnSaveReplaced += FTPSaveFile_OnSaveReplaced;
                xdescription.Text =
                    string.Format(
                        "Title : {0}\n" + (dir.IsPS3 ? "AccountID" : "ProfileID") + " : {2}\nModified : {3}\n{1}",
                        dir.Title, dir.FullName, dir.ProfileID, dir.Modified);
                IsValid = true;
                IsFTP = true;
            }
            catch
            {
                IsValid = false;
            }
        }

        public FTPSaveFile(FileEntry file)
        {
            InitializeComponent();
            try
            {
                SaveInstance = file;
                var s = new StfsPackage(file.GetStream(), 0);
                var h = s.Header;
                xdescription.Text = string.Format("Title : {0}\nProfileID : {2}\nModified : {3}\n{1}", h.TitleName,
                    file.Path, BitConverter.ToString(h.ProfileId).Replace("-", ""), file.ModifiedDate);
                IsValid = true;
                IsFTP = false;
            }
            catch
            {
                IsValid = false;
            }
        }

        public bool IsFTP { get; }
        public object SaveInstance { get; }
        public bool IsValid { get; private set; }
        public event FTPSave.FTPSaveChanged OnSaveLoaded;
        public event FTPSave.FTPSaveChanged OnSaveDeleted;
        public event FTPSave.FTPSaveChanged OnSaveReplaced;

        private void FTPSaveFile_OnSaveReplaced(object dir, bool isftp)
        {
            OnSaveReplaced?.Invoke(dir, isftp);
        }

        private void FTPSaveFile_OnSaveDeleted(object dir, bool isftp)
        {
            OnSaveDeleted?.Invoke(dir, isftp);
        }

        private void Client_TransferProgress(FtpTransferInfo e)
        {
            var percent = e.Percentage > 100 ? 100 : (int) e.Percentage;
            if (radProgressBar1.InvokeRequired)
            {
                if (e.Complete)
                {
                    radProgressBar1.Invoke(new MethodInvoker(() => radProgressBar1.Text = ""));
                    radProgressBar1.Invoke(new MethodInvoker(() => radProgressBar1.Value1 = 0));
                }
                else
                {
                    radProgressBar1.Invoke(
                        new MethodInvoker(
                            () =>
                                radProgressBar1.Text =
                                    string.Format(e.TransferType + "ing SaveData ({0}%)", new object[] {percent})));
                    radProgressBar1.Invoke(new MethodInvoker(() => radProgressBar1.Value1 = percent));
                }
                radProgressBar1.Invoke(new MethodInvoker(() => radProgressBar1.Invalidate()));
            }
            else
            {
                if (e.Complete)
                {
                    radProgressBar1.Text = "";
                    radProgressBar1.Value1 = 0;
                }
                else
                {
                    radProgressBar1.Text = string.Format(e.TransferType + "ing SaveData ({0}%)", new object[] {percent});
                    radProgressBar1.Value1 = percent;
                }
                radProgressBar1.Invalidate();
            }
        }

        private void xload_Click(object sender, EventArgs e)
        {
            if (OnSaveLoaded != null && !(IsFTP && (SaveInstance as FTPSaveInfo).IsBusy))
            {
                xload.Text = "Loading Save...";
                OnSaveLoaded(SaveInstance, IsFTP);
                xload.Text = "Load Save";
            }
        }

        private void xextract_Click(object sender, EventArgs e)
        {
            try
            {
                if (SaveInstance == null || (IsFTP && (SaveInstance as FTPSaveInfo).IsBusy))
                    return;
                if (IsFTP)
                {
                    var f = SaveInstance as FTPSaveInfo;
                    f.Client.Connect();
                    if (f != null)
                        f.Extract();
                }
                else
                {
                    var file = SaveInstance as FileEntry;
                    if (file != null)
                    {
                        var ofd = new SaveFileDialog();
                        ofd.Title = "Extract DSII Savegame";
                        ofd.FileName = file.Name;
                        if (ofd.ShowDialog() == DialogResult.OK)
                        {
                            file.Extract(ofd.FileName);
                            RadMessageBox.Show(file.Name + " Succesfully Extracted", "Extracted", MessageBoxButtons.OK,
                                RadMessageIcon.Info);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RadMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, RadMessageIcon.Error);
            }
        }

        private void xreplace_Click(object sender, EventArgs e)
        {
            try
            {
                if (SaveInstance == null || (IsFTP && (SaveInstance as FTPSaveInfo).IsBusy))
                    return;
                if (IsFTP)
                {
                    var f = SaveInstance as FTPSaveInfo;
                    f.Client.Connect();
                    if (f != null)
                        f.Replace();
                }
                else
                {
                    var ofd = new OpenFileDialog();
                    ofd.Title = @"Select DSII Savegame for replacement";
                    ofd.FileName = "-GAME_0000";
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        var s = new StfsPackage(ofd.FileName, 0);
                        if (s.Header.TitleId != 0x465307E4)
                        {
                            s.Close();
                            throw new Exception("Invalid DSII Savegame");
                        }
                        var file = SaveInstance as FileEntry;
                        if (file != null)
                        {
                            var x = new StfsPackage(file.GetStream(), 0);
                            s.Header.ProfileId = x.Header.ProfileId;
                            s.Header.ConsoleId = x.Header.ConsoleId;
                            s.Header.DeviceId = x.Header.DeviceId;
                            s.RehashAndResign();
                            s.Close();
                            file.Replace(ofd.FileName);
                            // file.Parent.CreateNewFile(ofd.FileName, "-GAME_0000");
                            OnSaveReplaced?.Invoke(file, false);
                            RadMessageBox.Show(file.Name + " Succesfully replaced", "Replaced", MessageBoxButtons.OK,
                                RadMessageIcon.Info);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RadMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, RadMessageIcon.Error);
            }
        }

        private void xdelete_Click(object sender, EventArgs e)
        {
            if (SaveInstance == null || (IsFTP && (SaveInstance as FTPSaveInfo).IsBusy))
                return;
            if (
                RadMessageBox.Show(
                    "Are you sure you want to delete your savegame ?\nThe whole savedata will be lost for good!",
                    "Delete", MessageBoxButtons.YesNo, RadMessageIcon.Exclamation) == DialogResult.Yes)
            {
                try
                {
                    if (IsFTP)
                    {
                        var f = SaveInstance as FTPSaveInfo;
                        f.Client.Connect();
                        f.Delete(true);
                        OnSaveDeleted?.Invoke(f, true);
                    }
                    else
                    {
                        var file = SaveInstance as FileEntry;
                        if (file != null)
                        {
                            file.Delete();
                            OnSaveDeleted?.Invoke(file, false);
                        }
                    }
                }
                catch (Exception ex)
                {
                    RadMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, RadMessageIcon.Error);
                }
            }
        }
    }
}