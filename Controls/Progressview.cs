using System;
using System.Windows.Forms;

namespace Dark_Souls_II_Save_Editor.Controls
{
    public partial class Progressview : UserControl
    {
        public Progressview()
        {
            InitializeComponent();
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
            percent = percent > 100 ? 100 : percent;
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
    }
}