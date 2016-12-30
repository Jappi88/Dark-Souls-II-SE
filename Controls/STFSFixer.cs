using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using Dark_Souls_II_Save_Editor.STFSPackage;
using HavenInterface.Fatx;
using Telerik.WinControls;
using Telerik.WinControls.Enumerations;

namespace Dark_Souls_II_Save_Editor.Controls
{
    public partial class STFSFixer : UserControl
    {
        private bool _isbussy;

        public STFSFixer()
        {
            InitializeComponent();
            var ofd = radBrowseEditor1.Dialog as OpenFileDialog;
            if (ofd != null)
            {
                ofd.Title = @"Load DSII STFS File";
                ofd.FileName = "-GAME_0000";
            }
            Size = new Size(700, 400);
        }

        public string FilePath
        {
            get { return radBrowseEditor2.Value; }
            set { radBrowseEditor2.Value = value; }
        }

        public object SaveInstance
        {
            get { return radBrowseEditor2.Tag; }
            internal set { radBrowseEditor2.Tag = value; }
        }

        public event EventHandler OnFixStarted;
        public event EventHandler OnFixCompleted;

        private void radButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (_isbussy)
                    return;
                if (string.IsNullOrWhiteSpace(radBrowseEditor1.Value))
                    throw new Exception("Please select the Corrupted STFS File");
                if (string.IsNullOrWhiteSpace(radBrowseEditor2.Value))
                    throw new Exception("Please select the newly created STFS File");
                if (radBrowseEditor1.Value.ToLower() == radBrowseEditor2.Value.ToLower())
                    throw new Exception("You cannot choose the same files to restore...please choose 2 different files!");
                //string res = MainForm.AppSettings.ResourcePath + "\\BaseFile.BF";
                if (!File.Exists(radBrowseEditor1.Value))
                    throw new Exception("Could not locate the corrupted STFS File, Please specify a valid path!");
                if (!File.Exists(radBrowseEditor2.Value))
                    throw new Exception("Could not locate the newly created STFS File, Please specify a valid path!");
                //    else return;
                _isbussy = true;
                OnFixStarted?.Invoke(this, EventArgs.Empty);
                if (SaveInstance is string)
                    CopyOver(radBrowseEditor1.Value, radBrowseEditor2.Value, radCheckBox1.ToggleState == ToggleState.On);
                else if (SaveInstance is FileEntry)
                {
                    var temp = Path.GetTempFileName();
                    var file = SaveInstance as FileEntry;
                    SetProgress("Extracting file from usb device..", 0);
                    if (file.Extract(temp))
                        CopyOver(temp, radBrowseEditor2.Value, radCheckBox1.ToggleState == ToggleState.On);
                    else
                    {
                        throw new Exception("Failed to retrieve file from usb device!");
                    }
                    SetProgress("Replacing new save with old one", 99);
                    if (!file.Replace(radBrowseEditor2.Value))
                        throw new Exception("Failed to replace old savegame with the new one!");
                }
                _isbussy = false;
                SetProgress("Fixing STFS File succesfully completed!", 100);
                OnFixCompleted?.Invoke(this, EventArgs.Empty);
                RadMessageBox.Show("Fixing STFS File succesfully completed!", "Succes", MessageBoxButtons.OK,
                    RadMessageIcon.Info);
                SetProgress("", 0);
            }
            catch (Exception ex)
            {
                _isbussy = false;
                xstatus.ShowMessageWithTimer(ex.Message);
            }
        }

        public void CopyOver(string source, string des, bool delete)
        {
            var error = "";
            var failed = false;
            var x = new Thread(() => failed = !XCopyOver(source, des, ref error, delete));
            x.Start();
            while (x.ThreadState != ThreadState.Stopped)
                Application.DoEvents();
            if (failed)
                throw new Exception(error);
        }

        private bool XCopyOver(string source, string des, ref string error, bool delete)
        {
            StfsPackage s1 = null, s2 = null;
            try
            {
                s1 = new StfsPackage(source, 0);
                if (s1.Header.TitleId != 0x465307E4)
                {
                    s1.Close();
                    throw new Exception("Invalid DSII Savegame!");
                }
                s2 = new StfsPackage(des, 0);
                s2.Header.ProfileId = s1.Header.ProfileId;
                s2.Header.ConsoleId = s1.Header.ConsoleId;
                s2.Header.DeviceId = s1.Header.DeviceId;
                var count = 0;
                foreach (var file in s2.Root.Files)
                {
                    Application.DoEvents();
                    SetProgress("Transfering " + file.Name + "..",
                        Functions.GetPercentage(count, s1.Root.Files.Length));
                    if (file.Name.ToLower() == "user_data15")
                        continue;
                    try
                    {
                        var data = s1.ExtractFile(file.Name);
                        if (data != null)
                            file.Replace(data);
                    }
                    catch
                    {
                    }
                    count++;
                }
                s1.Close();
                SetProgress("Rehashing and Resigning file..", 92);
                s2.RehashAndResign();
                s2.Close();
                SetProgress("Moving file to " + source, 98);
                if (delete)
                    File.Delete(source);
                return true;
            }
            catch (Exception ex)
            {
                if (s1 != null)
                    s1.Close();
                if (s2 != null)
                    s2.Close();
                error = ex.Message;
                return false;
            }
        }

        private void DownloadBaseFile()
        {
            var error = "";
            var failed = false;
            _isbussy = true;
            var t = new Thread(() => downloadbf(ref error, ref failed));
            t.Start();
            while (_isbussy)
                Application.DoEvents();
            if (failed)
                throw new Exception(error);
        }

        private void downloadbf(ref string error, ref bool failed)
        {
            try
            {
                _isbussy = true;
                var url = "https://dl.dropboxusercontent.com/u/31282351/Dark%20Souls%20II%20Resources/BaseFile.BF";
                var req = WebRequest.Create(url);
                using (var rs = req.GetResponse())
                {
                    var filelength = rs.ContentLength;
                    using (var io = rs.GetResponseStream())
                    {
                        var count = 0;
                        long read = 0;
                        using (var v = File.Create(MainForm.Settings.ResourcePath + "\\BaseFile.BF"))
                        {
                            var buffer = new byte[1024];
                            while ((count = io.Read(buffer, 0, 1024)) > 0)
                            {
                                if (IsDisposed)
                                    break;
                                SetProgress(
                                    "Downloading BaseFile..",
                                    Functions.GetPercentage(read, filelength));
                                read += count;
                                v.Write(buffer, 0, count);
                            }
                        }
                    }
                }
                _isbussy = false;
            }
            catch (Exception ex)
            {
                failed = true;
                error = ex.Message;
                SetProgress("", 0);
                _isbussy = false;
            }
        }

        private void SetProgress(string text, int percentage)
        {
            if (radProgressBar1.InvokeRequired)
            {
                radProgressBar1.Invoke(
                    new Action(() => radProgressBar1.Text = text == "" ? "" : text + " (" + percentage + "%)"));
                radProgressBar1.Invoke(new Action(() => radProgressBar1.Value1 = percentage));
            }
            else
            {
                radProgressBar1.Text = text == "" ? "" : text + " (" + percentage + "%)";
                radProgressBar1.Value1 = percentage;
            }
        }

        private void radBrowseEditor1_ValueChanged(object sender, EventArgs e)
        {
            SaveInstance = radBrowseEditor1.Value;
        }
    }
}