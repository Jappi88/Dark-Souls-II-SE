using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Dark_Souls_II_Save_Editor.DB_Builder;
using Dark_Souls_II_Save_Editor.SaveStuff;
using HavenInterface.Interfaces;
using Telerik.WinControls;

namespace Dark_Souls_II_Save_Editor.Controls
{
    public partial class ProgressPopupUI : UserControl
    {
        public delegate void ServerMessageRecieved(string[] message);

        public delegate void UpdateArg(string exepath, string changelog, string version);

        public delegate void UpdateFoundArg();

        internal int Dbversion = -1;
        public bool IsChecking;
        internal int LNGVersion = -1;

        public ProgressPopupUI()
        {
            InitializeComponent();
            Progress = new Progressor();
            Progress.AtProgress += DoProgress;
            radProgressBar1.Text = "Dark Souls II Save Editor Version " +
                                   Assembly.GetExecutingAssembly().GetName().Version + " By Jappi88";
        }

        public bool Canceled { get; set; }
        public static bool IsBusy { get; set; }
        public static Progressor Progress { get; set; }
        public event ServerMessageRecieved OnServerMessagerecieved;
        public event UpdateArg DownloadCompleted;

        protected virtual void OnDownloadCompleted(string exepath, string changelog, string version)
        {
            var handler = DownloadCompleted;
            handler?.Invoke(exepath, changelog, version);
        }

        public event UpdateFoundArg OnUpdateFound;
        public event UpdateFoundArg OnDbUpdateFound;
        public static event EventHandler ViewRequest;
        public static event EventHandler ViewClose;
        public static event EventHandler ViewHide;
        public static event EventHandler ViewShow;

        public static void OnViewShow()
        {
            var handler = ViewShow;
            handler?.Invoke(Progress, EventArgs.Empty);
        }

        public static void OnViewHide()
        {
            var handler = ViewHide;
            handler?.Invoke(Progress, EventArgs.Empty);
        }

        public static void OnViewClose()
        {
            var handler = ViewClose;
            handler?.Invoke(Progress, EventArgs.Empty);
        }

        public void DoProgress(string message, int percent, long current, long max)
        {
            var x = false;
            if (InvokeRequired)
                Invoke(new Action(() => x = IsDisposed));
            else
                x = IsDisposed;
            if (x)
                return;
            DoButtonPercent(current, max);
            if (!radProgressBar1.InvokeRequired)
            {
                radProgressBar1.Text = message;
                radProgressBar1.Value1 = percent;
            }
            else
            {
                radProgressBar1.Invoke(new MethodInvoker(() => radProgressBar1.Text = message));
                radProgressBar1.Invoke(new MethodInvoker(() => radProgressBar1.Value1 = percent));
            }
        }

        public void CheckForResources(object sender, EventArgs e)
        {
            IsChecking = true;
            var path = MainForm.Settings.ResourcePath;
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            if (!File.Exists(path + "\\" + "ItemTypes.db"))
                DownloadDatabase("ItemTypes.db", path, true);
            if (!File.Exists(path + "\\" + "DS2Items_English.db"))
                DownloadDatabase("DS2Items_English.db", path, true);
            else
                CheckDBVersion("DS2Items_English.db");
            if (!File.Exists(path + "\\" + "Parameters.db"))
                DownloadDatabase("Parameters.db", path, true);
        }

        public void CheckDBVersion(string name)
        {
            if (MainForm.Live != null && MainForm.Live.Serverinfo.ApplicationInfo != null &&
                MainForm.Live.Serverinfo.ApplicationInfo.Resources != null)
            {
                var path = MainForm.Settings.ResourcePath;
                var resource =
                    MainForm.Live.Serverinfo.ApplicationInfo.Resources.FirstOrDefault(
                        t => t.Title.ToLower() == name.ToLower());
                if (resource != null)
                {
                    try
                    {
                        if (resource.Version != DSDataBase.DBVersion(path + "\\" + name).ToString())
                            DownloadDatabase(resource, path, true, Progress);
                    }
                    catch (Exception)
                    {
                        DownloadDatabase(resource, path, true, Progress);
                    }
                }
            }
        }

        public void GetServerMessage()
        {
            var bw = new BackgroundWorker();
            bw.DoWork += bw_DoWork;
            bw.RunWorkerCompleted += bw_RunWorkerCompleted;
            bw.RunWorkerAsync();
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result != null && OnServerMessagerecieved != null)
                OnServerMessagerecieved(e.Result as string[]);
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = null;
            try
            {
                var rex = WebRequest.Create("http://site.smartdevio.com/gameinfo/index.php?TitleID=" + "12345678");
                var res = rex.GetResponse();
                var length = res.ContentLength;
                var data = new byte[length];
                var responseStream = res.GetResponseStream();
                if (responseStream != null) responseStream.Read(data, 0, (int) length);
                res.Close();
                var values = Encoding.UTF8.GetString(data).Split('|');
                if (values.Length > 3)
                    e.Result = values[2].Split(':');
            }
            catch
            {
            }
        }

        public void LoadDataBase()
        {
            if (IsBusy)
                return;
            IsBusy = true;
            ViewRequest?.Invoke(this, EventArgs.Empty);
            var db = MainForm.Settings.ResourcePath + "\\DS2Items_English.db";
            var dbtypes = MainForm.Settings.ResourcePath + "\\ItemTypes.db";
            try
            {
                MainFile.LoadDb(db, dbtypes, Progress);
                OnViewClose();
                IsBusy = false;
                MainFile.DataBase.OnDatabaseLoaded();
            }
            catch (Exception ex)
            {
                IsBusy = false;
                OnViewClose();
                RadMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, RadMessageIcon.Error);
            }
        }

        public void DownloadDatabase()
        {
            var path = MainForm.Settings.ResourcePath;
            DownloadDatabase("DS2Items_English.db", path, true);
        }

        public void DownloadDatabase(string dbname, string outpath, bool onshowp)
        {
            if (MainForm.Live != null && MainForm.Live.Serverinfo.ApplicationInfo != null &&
                MainForm.Live.Serverinfo.ApplicationInfo.Resources != null)
            {
                var path = MainForm.Settings.ResourcePath;
                var resource =
                    MainForm.Live.Serverinfo.ApplicationInfo.Resources.FirstOrDefault(
                        t => t.Title.ToLower() == dbname.ToLower());
                if (resource != null)
                    DownloadDatabase(resource, path, true, Progress);
            }
        }

        public void DownloadDatabase(Resource resource, string outpath, bool onshowp)
        {
            DownloadDatabase(resource, outpath, onshowp, Progress);
        }

        public void DownloadDatabase(Resource resource, string outpath, bool onshowp, Progressor p)
        {
            IsBusy = true;
            var t = new Thread(() => XDownloadDatabase(resource, outpath, onshowp, p));
            t.IsBackground = true;
            t.Start();
            while (IsBusy)
                Application.DoEvents();
        }

        public static void XDownloadDatabase(Resource resource, string outpath, bool showp, Progressor p)
        {
            try
            {
                IsBusy = true;
                if (ViewRequest != null && showp)
                    ViewRequest(null, EventArgs.Empty);
                p.DoProgress("Downloading " + resource.Title + "..", 0, 0, 0);
                var url = resource.Download;
                var req = WebRequest.Create(url + "&download");
                req.ContentType = "application/x-www-form-urlencoded";
                req.Credentials = CredentialCache.DefaultCredentials;
                using (var rs = req.GetResponse())
                {
                    var filelength = rs.ContentLength;
                    using (var io = rs.GetResponseStream())
                    {
                        long read = 0;
                        using (var v = File.Create(outpath + "\\" + resource.Title))
                        {
                            var buffer = new byte[1024];
                            var count = 0;
                            while (io != null && (count = io.Read(buffer, 0, 1024)) > 0)
                            {
                                p.DoProgress(
                                    "Downloading " + resource.Title + "..(" + Functions.GetPercentage(read, filelength) +
                                    "%)",
                                    Functions.GetPercentage(read, filelength), 0, 0);
                                read += count;
                                v.Write(buffer, 0, count);
                            }
                        }
                    }
                }
                p.DoProgress(
                    "Dark Souls II Save Editor Version " + Assembly.GetExecutingAssembly().GetName().Version +
                    " By Jappi88", 100, 0, 0);
                IsBusy = false;
                OnViewClose();
            }
            catch (Exception ex)
            {
                p.DoProgress(ex.Message, 100, 0, 0);
                IsBusy = false;
                OnViewClose();
            }
        }

        internal void SetProgress(string text, long low, long max)
        {
            if (IsDisposed || radProgressBar1.IsDisposed)
                return;
            var percentage = Functions.GetPercentage(low, max);
            if (radProgressBar1.InvokeRequired)
            {
                if (IsDisposed || radProgressBar1.IsDisposed)
                    return;
                radProgressBar1.Invoke(new Action(() => radProgressBar1.Text = text));
                if (IsDisposed || radProgressBar1.IsDisposed)
                    return;
                radProgressBar1.Invoke(new Action(() => radProgressBar1.Value1 = percentage));
                if (IsDisposed || radProgressBar1.IsDisposed)
                    return;
                radProgressBar1.Invoke(new Action(() => radProgressBar1.Invalidate()));
            }
            else
            {
                if (IsDisposed || radProgressBar1.IsDisposed)
                    return;
                radProgressBar1.Text = text;
                radProgressBar1.Value1 = percentage;
                radProgressBar1.Invalidate();
            }
        }

        private void DoButtonPercent(long read, long filelength)
        {
            if (Width < 100)
            {
                if (InvokeRequired)
                {
                    radButton1.Invoke(
                        new MethodInvoker(
                            () => radButton1.Text = Functions.GetPercentage(read, filelength) + "%"));
                }
                else
                {
                    radButton1.Text = Functions.GetPercentage(read, filelength) + "%";
                }
                Application.DoEvents();
            }
            else
            {
                if (InvokeRequired)
                {
                    radButton1.Invoke(
                        new MethodInvoker(
                            () => radButton1.Text = @">>"));
                }
                else
                {
                    radButton1.Text = @">>";
                }
                Application.DoEvents();
            }
        }

        private void MainPage_Resize(object sender, EventArgs e)
        {
            SuspendLayout();
        }

        private void MainPage_SizeChanged(object sender, EventArgs e)
        {
            ResumeLayout(false);
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            if (Width > 100)
            {
                OnViewHide();
                radButton1.Text = @"<<";
            }
            else
            {
                OnViewShow();
                radButton1.Text = @">>";
            }
        }

        protected virtual void OnOnUpdateFound()
        {
            OnUpdateFound?.Invoke();
        }

        protected virtual void OnOnDbUpdateFound()
        {
            OnDbUpdateFound?.Invoke();
        }
    }
}