using System;
using System.Windows.Forms;

namespace Dark_Souls_II_Save_Editor.Controls
{
    public partial class PopupMessage : UserControl
    {
        public PopupMessage()
        {
            InitializeComponent();
        }

        public event EventHandler OnClose;
        public event EventHandler OnShow;

        private void radButton1_Click(object sender, EventArgs e)
        {
            OnClose?.Invoke(this, EventArgs.Empty);
        }

        public void ShowMessage(string message, string title, MessageType type)
        {
            var index = -1;
            switch (type)
            {
                case MessageType.Error:
                    index = 2;
                    break;
                case MessageType.Info:
                    index = 1;
                    break;
                case MessageType.Message:
                    index = 0;
                    break;
                case MessageType.NewMessage:
                    index = 0;
                    break;
                case MessageType.Warning:
                    index = 3;
                    break;
            }
            if (InvokeRequired)
            {
                radTitleBar1.Invoke(new Action(() => radTitleBar1.Text = title));
                label1.Invoke(new Action(() => label1.Text = message));
                if (index > -1)
                    ximage.Invoke(new Action(() => ximage.Image = imageList1.Images[index]));
            }
            else
            {
                radTitleBar1.Text = title;
                label1.Text = message;
                if (index > -1)
                    ximage.Image = imageList1.Images[index];
            }
            OnShow?.Invoke(this, EventArgs.Empty);
        }

        private void radTitleBar1_Close(object sender, EventArgs args)
        {
            OnClose?.Invoke(this, EventArgs.Empty);
        }
    }
}