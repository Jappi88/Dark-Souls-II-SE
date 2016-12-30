using System;
using System.Windows.Forms;

namespace Dark_Souls_II_Save_Editor.Controls
{
    public partial class HelpPage : UserControl
    {
        public HelpPage()
        {
            InitializeComponent();
        }

        private void MainPage_Resize(object sender, EventArgs e)
        {
            SuspendLayout();
        }

        private void MainPage_SizeChanged(object sender, EventArgs e)
        {
            ResumeLayout(false);
        }
    }
}