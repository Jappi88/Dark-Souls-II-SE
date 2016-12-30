using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Dark_Souls_II_Save_Editor.STFSPackage;
using HavenInterface.FtpClient;
using HavenInterface.IOPackage;
using PS3FileSystem;

namespace Dark_Souls_II_Save_Editor.SaveStuff
{
    public class FTPSaveInfo : FtpDirectory
    {
        public FTPSaveInfo(FtpDirectory dir, bool isps3)
            : base(dir.Client, dir.FullName)
        {
            try
            {
                IsPS3 = isps3;
                var found = false;
                IsBusy = true;
                Client.SetWorkingDirectory(FullName);
                if (isps3)
                {
                    var files = Files.ToArray();
                    if (files != null && files.Length > 0)
                    {
                        foreach (var v in files)
                        {
                            if (v.Name.ToLower() == "param.sfo")
                            {
                                using (var ms = new MemoryStream(v.DownloadBytes()))
                                {
                                    var sfo = new PARAM_SFO(ms);
                                    Title = sfo.Title;
                                    Description = sfo.Detail;
                                    ProfileID = sfo.AccountID;
                                    Modified = v.LastWriteTime;
                                    found = true;
                                }
                                break;
                            }
                        }
                    }
                }
                else
                {
                    var path = FullName;
                    path += "/00000001";
                    Client.SetWorkingDirectory("/");
                    Client.SetWorkingDirectory(FullName);
                    if (DirectoryExists(path))
                    {
                        Client.SetWorkingDirectory(path);
                        var saves = new FtpDirectory(Client, path).Files.ToArray();
                        if (saves != null && saves.Length > 0)
                        {
                            foreach (var file in saves)
                            {
                                if (file.Name.ToLower() == "-game_0000")
                                {
                                    STFSHeader = new ContentHeader(
                                        new StreamIO(file.DownloadBytes()), 0);
                                    Title = STFSHeader.TitleName;
                                    Description = STFSHeader.DisplayName;
                                    ProfileID = BitConverter.ToString(STFSHeader.ProfileId).Replace("-", "");
                                    Modified = file.LastWriteTime;
                                    found = true;
                                }
                            }
                        }
                    }
                }
                IsBusy = false;
                if (!found)
                    throw new Exception("Unable to locate savegame");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public event FTPSave.FTPSaveChanged OnSaveDeleted;
        public event FTPSave.FTPSaveChanged OnSaveReplaced;
        public event FtpTransferProgress OnProgress;

        private void Client_TransferProgress(FtpTransferInfo e)
        {
            OnProgress?.Invoke(e);
        }

        public void Extract()
        {
            try
            {
                if (IsPS3)
                {
                    var fb = new FolderBrowserDialog();
                    fb.Description = "Extract Dark Souls II SaveGame";
                    if (fb.ShowDialog() == DialogResult.OK)
                    {
                        Extract(fb.SelectedPath);
                    }
                }
                else
                {
                    var ofd = new SaveFileDialog();
                    ofd.FileName = "-GAME_0000";
                    ofd.Title = "Extract Dark Souls II SaveGame";
                    if (ofd.ShowDialog() != DialogResult.OK)
                        return;
                    Extract(ofd.FileName);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Extract(string fpath)
        {
            Client.TransferProgress += Client_TransferProgress;
            IsBusy = true;
            if (IsPS3)
            {
                if (!Directory.Exists(fpath + "\\" + Name))
                    Directory.CreateDirectory(fpath + "\\" + Name);
                Client.SetWorkingDirectory(FullName);
                var files = Files.ToArray();
                if (files != null && files.Length > 0)
                {
                    foreach (var v in files)
                    {
                        var t = new Thread(() => v.Download(fpath + "\\" + Name + "\\" + v.Name));
                        t.Start();
                        while (t.ThreadState != ThreadState.Stopped)
                            Application.DoEvents();
                    }
                }
                Client.TransferProgress -= Client_TransferProgress;
            }
            else
            {
                var path = FullName + "/00000001";
                Client.SetWorkingDirectory("/");
                Client.SetWorkingDirectory(FullName);
                if (DirectoryExists(path))
                {
                    Client.SetWorkingDirectory(path);
                    var saves = new FtpDirectory(Client, path).Files.ToArray();
                    if (saves != null && saves.Length > 0)
                    {
                        foreach (var file in saves)
                        {
                            if (file.Name.ToLower() == "-game_0000")
                            {
                                var t = new Thread(() => file.Download(fpath));
                                t.Start();
                                while (t.ThreadState != ThreadState.Stopped)
                                    Application.DoEvents();
                            }
                        }
                    }
                    Client.TransferProgress -= Client_TransferProgress;
                }
                else
                {
                    IsBusy = false;
                    throw new Exception("Invalid Root Directory");
                }
            }
            IsBusy = false;
        }

        public void DeleteSave()
        {
            try
            {
                IsBusy = true;
                var t = new Thread(() => Parent.Delete(this, true));
                t.Start();
                while (t.ThreadState != ThreadState.Stopped)
                    Application.DoEvents();
                IsBusy = false;
                OnSaveDeleted?.Invoke(this, true);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }

        public void Replace()
        {
            IsBusy = true;
            try
            {
                if (IsPS3)
                {
                    var fb = new FolderBrowserDialog();
                    fb.Description = "Extract Dark Souls II SaveGame";
                    if (fb.ShowDialog() == DialogResult.OK)
                    {
                        Replace(fb.SelectedPath);
                    }
                }
                else
                {
                    var ofd = new OpenFileDialog();
                    ofd.FileName = "-GAME_0000";
                    ofd.Title = "Extract Dark Souls II SaveGame";
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            var s = new StfsPackage(ofd.FileName, 0);
                            if (STFSHeader != null)
                            {
                                if (s.Header.TitleId != STFSHeader.TitleId)
                                    throw new Exception(
                                        "Selected file is not valid, source must contain the same titleid as the destination file");
                                s.Header.ProfileId = STFSHeader.ProfileId;
                                s.Header.ConsoleId = STFSHeader.ConsoleId;
                                s.Header.DeviceId = STFSHeader.DeviceId;
                            }
                            s.RehashAndResign();
                            s.Close();
                            Replace(ofd.FileName);
                            OnSaveReplaced?.Invoke(this, true);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            IsBusy = false;
        }

        public void Replace(string fpath)
        {
            Client.TransferProgress += Client_TransferProgress;
            IsBusy = true;
            if (IsPS3)
            {
                var files = Directory.GetFiles(fpath);
                if (files != null && files.Length > 0)
                {
                    foreach (var v in files)
                    {
                        var t = new Thread(() => Client.Upload(v, FullName + "/" + new FileInfo(v).Name));
                        t.Start();
                        while (t.ThreadState != ThreadState.Stopped)
                            Application.DoEvents();
                    }
                }
                Client.TransferProgress -= Client_TransferProgress;
            }
            else
            {
                var path = FullName + "/00000001";
                Client.SetWorkingDirectory("/");
                Client.SetWorkingDirectory(FullName);
                if (DirectoryExists(path))
                {
                    Client.SetWorkingDirectory(path);
                    var saves = new FtpDirectory(Client, path).Files.ToArray();
                    var found = false;
                    if (saves != null && saves.Length > 0)
                    {
                        foreach (var file in saves)
                        {
                            if (file.Name.ToLower() == "-game_0000")
                            {
                                var t = new Thread(() => file.Upload(fpath));
                                t.Start();
                                while (t.ThreadState != ThreadState.Stopped)
                                    Application.DoEvents();
                                found = true;
                                break;
                            }
                        }
                        if (!found)
                        {
                            var t = new Thread(() => Client.Upload(fpath));
                            t.Start();
                            while (t.ThreadState != ThreadState.Stopped)
                                Application.DoEvents();
                        }
                        OnSaveReplaced?.Invoke(this, true);
                    }
                    else
                    {
                        CreateDirectory(path);
                        Client.SetWorkingDirectory(path);
                        var t = new Thread(() => Client.Upload(fpath));
                        t.Start();
                        while (t.ThreadState != ThreadState.Stopped)
                            Application.DoEvents();
                        OnSaveReplaced?.Invoke(this, true);
                    }
                    Client.TransferProgress -= Client_TransferProgress;
                }
                else
                {
                    IsBusy = false;
                    throw new Exception("Invalid Root Directory");
                }
            }
            IsBusy = false;
        }

        #region Properties

        public string Title { get; private set; }
        public string Description { get; private set; }
        public string ProfileID { get; private set; }
        public DateTime Modified { get; private set; }
        public bool IsPS3 { get; }
        public bool IsBusy { get; private set; }
        //internal HeaderData STFSHeader { get; set; }
        internal ContentHeader STFSHeader { get; set; }

        #endregion
    }
}