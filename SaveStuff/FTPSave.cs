using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using HavenInterface.FtpClient;
using HavenInterface.Utils;

namespace Dark_Souls_II_Save_Editor.SaveStuff
{
    public class FTPSave
    {
        public FTPSave(string user, string pass, string adress, int port)
        {
            Client = new FtpClient(user, pass, adress, port);
            Client.TransferProgress += ftp_TransferProgress;
            Client.ConnectionClosed += ftp_ConnectionClosed;
            Client.ResponseReadTimeout = 1000;
            Client.WriteTimeout = 1000;
            Client.ReadTimeout = 1000;
            Client.DataChannelReadTimeout = 1000;
            SupportedSaveDirs = new[]
            {
                "465307e4", "BLUS41045-GAME_000", "BLES01959-GAME_000", "NPUB31358-GAME_000", "NPEB01853-GAME_000",
                "BLJM61113-GAME_000", "NPJB00555-GAME_000", "BLAS50640-GAME_000"
            };
        }

        private void Alive()
        {
            while (KeepAlive)
            {
                Thread.Sleep(5000);
                Application.DoEvents();
                if (Client != null && Client.Connected && !Isbusy &&
                    SaveGames.Where(t => t.IsBusy).ToArray().Length == 0)
                    try
                    {
                        Client.SetWorkingDirectory(Client.CurrentDirectory.FullName);
                    }
                    catch
                    {
                    }
            }
        }

        public void Connect()
        {
            if (Client == null || Client.Connected || Isbusy)
                return;
            try
            {
                Isbusy = true;
                if (Client.Port == 7564)
                    Client.DataChannelType = FtpDataChannelType.Active;
                else
                    Client.DataChannelType = FtpDataChannelType.Passive;
                Isbusy = true;
                var td = new ThreadDelegate(Client.Connect);
                td.BeginInvoke(thread_callback, td);
            }
            catch
            {
                Isbusy = false;
            }
        }

        public bool Disconnect()
        {
            if (Client == null)
                return false;
            if (!Client.Connected)
                return true;
            try
            {
                Isbusy = true;
                Client.Disconnect();
                SaveGames = null;
                Isbusy = false;
                return true;
            }
            catch
            {
                Isbusy = false;
                return false;
            }
        }

        private FTPSaveInfo[] GetDirs(string[] names, string parent, bool x1, bool x2, bool x3, bool x4, ref bool isps3)
        {
            var dirs = new List<FTPSaveInfo>();
            try
            {
                Isbusy = true;
                Client.SetWorkingDirectory(parent);
                if (parent == "/")
                {
                    parent = "";
                }
                var items = Client.GetListing();
                var first = new[]
                {
                    ".", "..", "smb", "conx", "devkit", "flash", "game",
                    "hddx", "sysext", "usb0", "usb1", "usb2", "usb3", "fflash", "fhddaux", "fhddsys", "fhddsysex",
                    "dev_bdvd", "dev_flash",
                    "dev_flash1", "dev_flash2", "dev_flash3", "dev_flash4", "dev_hdd1", "dev_hdd2", "dev_ps2disc"
                };
                var second = new[] {"content", "home"};
                var last = "savedata";
                bool hasreachedfirst = x1, hasreachedsecond = x2, hasreachedlast = x3, final = x4;
                foreach (var item in items)
                {
                    if (hasreachedfirst)
                    {
                        if (first.Contains(item.Name.ToLower()) || first.Contains(item.Name.ToLower().TrimStart('f')))
                            continue;
                        if (item.Type == FtpObjectType.Directory)
                        {
                            var ftpdir = GetDirs(names, parent + "/" + item.Name, false, true, false, false, ref isps3);
                            if (ftpdir != null && ftpdir.Length > 0)
                                dirs.AddRange(ftpdir);
                        }
                    }
                    if (hasreachedsecond)
                    {
                        if (!second.Contains(item.Name.ToLower()))
                            continue;
                        if (item.Type == FtpObjectType.Directory)
                        {
                            if (item.Name.ToLower() == "home")
                                isps3 = true;
                            else
                                isps3 = false;
                            var ftpdir = GetDirs(names, parent + "/" + item.Name, false, false, true, false, ref isps3);
                            if (ftpdir != null && ftpdir.Length > 0)
                                dirs.AddRange(ftpdir);
                        }
                    }
                    if (isps3 && hasreachedlast)
                    {
                        if (item.Name.Length == 8 && item.Name.IsValidHex())
                        {
                            var ftpdir = GetDirs(names, parent + "/" + item.Name, false, false, false, true, ref isps3);
                            if (ftpdir != null && ftpdir.Length > 0)
                                dirs.AddRange(ftpdir);
                        }
                    }
                    if (isps3 && final)
                    {
                        if (last == item.Name.ToLower())
                        {
                            var ftpdir = GetDirs(names, parent + "/" + item.Name, false, false, false, false, ref isps3);
                            if (ftpdir != null && ftpdir.Length > 0)
                                dirs.AddRange(ftpdir);
                        }
                    }
                    if (!isps3 && hasreachedlast)
                    {
                        if (item.Name.ToLower() == "0000000000000000" || !item.Name.IsValidHex())
                            continue;
                        if (item.Type == FtpObjectType.Directory)
                        {
                            if (item.Name.ToLower() == last)
                                isps3 = true;
                            else
                                isps3 = false;
                            var ftpdir = GetDirs(names, parent + "/" + item.Name, false, false, false, true, ref isps3);
                            if (ftpdir != null && ftpdir.Length > 0)
                                dirs.AddRange(ftpdir);
                        }
                    }
                    if (names.Where(t => t.ToLower() == item.Name.ToLower()).FirstOrDefault() != null &&
                        item.Type == FtpObjectType.Directory)
                        dirs.Add(new FTPSaveInfo(new FtpDirectory(Client, parent + "/" + item.Name), isps3));
                }
            }
            catch
            {
            }
            Isbusy = false;
            return dirs.ToArray();
        }

        private void ftp_ConnectionClosed(FtpChannel c)
        {
            KeepAlive = false;
            Isbusy = false;
            OnDisconnect?.Invoke(c);
        }

        private void ftp_TransferProgress(FtpTransferInfo e)
        {
            Isbusy = !e.Complete;
            OnProgress?.Invoke(e);
        }

        private void thread_callback(IAsyncResult result)
        {
            Isbusy = true;
            var td = (ThreadDelegate) result.AsyncState;
            try
            {
                td.EndInvoke(result);
                if (Client.Connected)
                {
                    var isps3 = false;
                    var t =
                        new Thread(
                            () => SaveGames = GetDirs(SupportedSaveDirs, "/", true, false, false, false, ref isps3));
                    t.Start();
                    while (t.ThreadState != ThreadState.Stopped)
                        Application.DoEvents();
                    IsPS3 = isps3;
                    t = new Thread(() => Alive());
                    t.IsBackground = true;
                    KeepAlive = true;
                    t.Priority = ThreadPriority.Lowest;
                    t.Start();
                }
                Isbusy = false;
            }
            catch (Exception ex)
            {
                Isbusy = false;
                throw ex;
            }
            OnConnected?.Invoke(Client);
        }

        private delegate void ThreadDelegate();

        #region Properties

        public FtpClient Client { get; }
        public IAsyncResult ConnectionAsync { get; private set; }
        public bool Isbusy { get; private set; }
        public bool KeepAlive { get; set; }
        public FTPSaveInfo[] SaveGames { get; private set; }
        public bool IsPS3 { get; private set; }
        public string[] SupportedSaveDirs { get; set; }

        #endregion

        #region Events

        public delegate void FTPSaveChanged(object file, bool isftp);

        public event FtpChannelConnected OnConnected;
        public event FtpTransferProgress OnProgress;
        public event FtpChannelDisconnected OnDisconnect;
        //public event FTPSaveChanged OnSaveReplaced;
        //public event FTPSaveChanged OnSaveDeleted;

        #endregion
    }
}