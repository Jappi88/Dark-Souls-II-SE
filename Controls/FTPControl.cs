using System;
using System.Drawing;
using System.Windows.Forms;
using Dark_Souls_II_Save_Editor.SaveStuff;
using HavenInterface.FtpClient;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace Dark_Souls_II_Save_Editor.Controls
{
    public partial class FTPControl : UserControl
    {
        public FTPControl()
        {
            InitializeComponent();
            xip.Text = MainForm.Settings.FtpSettings[0].IpAdress;
            xusername.Text = MainForm.Settings.FtpSettings[0].Username;
            xpass.Text = MainForm.Settings.FtpSettings[0].Password;
            xport.Value = MainForm.Settings.FtpSettings[0].Port;
        }

        public FTPSave Client { get; private set; }
        public bool Isbusy { get; private set; }
        public event FTPSave.FTPSaveChanged OnSaveLoaded;
        public event FtpTransferProgress OnProgress;

        private void xconnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (Client != null && Client.Isbusy)
                    return;
                if (xconnect.Text == "&Disconnect")
                {
                    if (Client != null && Client.Client.Connected)
                    {
                        xconnect.Text = "Disconnecting...";
                        Isbusy = true;
                        Client.Disconnect();
                        ftp_ConnectionClosed(null);
                        Isbusy = false;
                    }
                    else
                        ftp_ConnectionClosed(null);
                }
                else
                {
                    Client = new FTPSave(xusername.Text, xpass.Text, xip.Text, (int) xport.Value);
                    Client.OnDisconnect += ftp_ConnectionClosed;
                    Client.OnConnected += Client_OnConnected;
                    Client.OnProgress += ftp_TransferProgress;
                    Wait();
                    xconnect.Text = "Connecting...";
                    Client.Connect();
                }
            }
            catch (Exception ex)
            {
                ftp_ConnectionClosed(null);
                RadMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, RadMessageIcon.Error);
            }
        }

        private void Client_OnConnected(FtpChannel c)
        {
            if (c.Connected)
            {
                if (xconnect.InvokeRequired)
                    xconnect.Invoke(new MethodInvoker(() => xconnect.Text = "&Disconnect"));
                else
                    xconnect.Text = "&Disconnect";
                DoList();
                if (xsavecontainer.Controls.Count == 0)
                {
                    ftp_ConnectionClosed(null);
                    RadMessageBox.Show("No Save Files found for Dark Souls II", "No Save", MessageBoxButtons.OK,
                        RadMessageIcon.Exclamation);
                }
            }
            else
            {
                ftp_ConnectionClosed(null);
                RadMessageBox.Show(
                    "Unable to connect to host, make sure the device is turned on before trying to connect", "Error",
                    MessageBoxButtons.OK, RadMessageIcon.Error);
            }
        }

        private void DoList()
        {
            MainForm.Settings.FtpSettings[0].IpAdress = xip.Text;
            MainForm.Settings.FtpSettings[0].Username = xusername.Text;
            MainForm.Settings.FtpSettings[0].Password = xpass.Text;
            MainForm.Settings.FtpSettings[0].Port = (ushort) xport.Value;
            MainForm.Settings.Save();
            if (xsavecontainer.InvokeRequired)
            {
                xsavecontainer.Invoke(new MethodInvoker(() => xsavecontainer.PerformLayout()));
                xsavecontainer.Invoke(new MethodInvoker(() => xsavecontainer.Controls.Clear()));
            }
            else
            {
                xsavecontainer.PerformLayout();
                xsavecontainer.Controls.Clear();
            }
            if (Client.SaveGames != null && Client.SaveGames.Length > 0)
            {
                foreach (var v in Client.SaveGames)
                {
                    var save = new FTPSaveFile(v);
                    save.Dock = DockStyle.Top;
                    save.Height = 100;
                    save.OnSaveLoaded += save_OnSaveLoaded;
                    save.OnSaveDeleted += save_OnSaveDeleted;
                    save.OnSaveReplaced += save_OnSaveDeleted;
                    if (save.IsValid)
                    {
                        if (xsavecontainer.InvokeRequired)
                        {
                            xsavecontainer.Invoke(new MethodInvoker(() => xsavecontainer.Controls.Add(save)));
                            xsavecontainer.Invoke(
                                new MethodInvoker(
                                    () =>
                                        xsavecontainer.Controls.Add(new Label
                                        {
                                            AutoSize = false,
                                            Height = 5,
                                            BackColor = Color.Transparent,
                                            Dock = DockStyle.Top
                                        })));
                        }
                        else
                        {
                            xsavecontainer.Controls.Add(save);
                            xsavecontainer.Controls.Add(new Label
                            {
                                AutoSize = false,
                                Height = 5,
                                BackColor = Color.Transparent,
                                Dock = DockStyle.Top
                            });
                        }
                    }
                }
            }
            if (xsavecontainer.InvokeRequired)
            {
                xsavecontainer.Invoke(new MethodInvoker(() => xsavecontainer.ResumeLayout(false)));
                Invoke(new MethodInvoker(Update));
            }
            else
            {
                xsavecontainer.ResumeLayout(false);
                Update();
            }
        }

        private void save_OnSaveDeleted(object dir, bool isftp)
        {
            Wait();
            Client.Connect();
            DoList();
        }

        private void save_OnSaveLoaded(object dir, bool isftp)
        {
            OnSaveLoaded?.Invoke(dir, isftp);
        }

        private void Wait()
        {
            if (xsavecontainer.InvokeRequired)
            {
                xsavecontainer.Invoke(new MethodInvoker(() => xsavecontainer.PerformLayout()));
                xsavecontainer.Invoke(new MethodInvoker(() => xsavecontainer.Controls.Clear()));
            }
            else
            {
                xsavecontainer.PerformLayout();
                xsavecontainer.Controls.Clear();
            }
            var x = new RadWaitingBar();
            x.Dock = DockStyle.Top;
            x.Height = 80;
            x.WaitingIndicatorSize = new Size(150, 80);
            if (xsavecontainer.InvokeRequired)
            {
                xsavecontainer.Invoke(new MethodInvoker(() => xsavecontainer.Controls.Add(x)));
                xsavecontainer.Invoke(new MethodInvoker(() => xsavecontainer.ResumeLayout(false)));
            }
            else
            {
                xsavecontainer.Controls.Add(x);
                xsavecontainer.ResumeLayout(false);
            }
            Application.DoEvents();
            x.StartWaiting();
        }

        private void ftp_ResponseReceived(string status, string response)
        {
            Console.WriteLine("Status : {0} | Response : {1}", status, response);
        }

        private void ftp_ConnectionClosed(FtpChannel c)
        {
            if (InvokeRequired)
            {
                xconnect.Invoke(new MethodInvoker(() => xconnect.Text = "&Connect"));
                xsavecontainer.Invoke(new MethodInvoker(() => xsavecontainer.PerformLayout()));
                xsavecontainer.Invoke(new MethodInvoker(() => xsavecontainer.Controls.Clear()));
                xsavecontainer.Invoke(new MethodInvoker(() => xsavecontainer.ResumeLayout(false)));
                Invoke(new MethodInvoker(() => Update()));
            }
            else
            {
                xconnect.Text = "&Connect";
                xsavecontainer.PerformLayout();
                xsavecontainer.Controls.Clear();
                xsavecontainer.ResumeLayout(false);
                Update();
            }
        }

        private void ftp_TransferProgress(FtpTransferInfo e)
        {
            OnProgress?.Invoke(e);
        }
    }
}