using System;
using System.Drawing;
using System.Windows.Forms;

namespace Dark_Souls_II_Save_Editor.Controls
{
    public partial class PopupContainer : UserControl
    {
        public PopupContainer()
        {
            InitializeComponent();
        }

        public string Title
        {
            get { return radTitleBar1.Text; }
            set { radTitleBar1.Text = value; }
        }

        public event EventHandler OnClose;
        public event EventHandler OnMinimize;

        private void radTitleBar1_Minimize(object sender, EventArgs args)
        {
            OnMinimize?.Invoke(this, args);
        }

        private void radTitleBar1_Close(object sender, EventArgs args)
        {
            OnClose?.Invoke(this, args);
        }

        public void Populate(Control[] controls)
        {
            xcontrolcontainer.SuspendLayout();
            xcontrolcontainer.Controls.Clear();
            foreach (var c in controls)
            {
                c.Dock = DockStyle.Top;
                xcontrolcontainer.Controls.Add(c);
                xcontrolcontainer.Controls.Add(new Label {BackColor = Color.Transparent, Text = ""});
            }
            xcontrolcontainer.ResumeLayout(false);
        }
    }
}