using System;
using System.Windows.Forms;
using Telerik.WinControls.Enumerations;
using Telerik.WinControls.UI;

namespace Dark_Souls_II_Save_Editor
{
    public partial class About : RadForm
    {
        public About()
        {
            InitializeComponent();
            Select();
        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            if (radCheckBox1.Checked)
                DialogResult = DialogResult.OK;
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void radCheckBox1_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            radButton2.Enabled = args.ToggleState == ToggleState.On;
        }
    }
}