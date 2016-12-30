using System;
using System.Windows.Forms;
using Dark_Souls_II_Save_Editor.Parameters;
using Dark_Souls_II_Save_Editor.SaveStuff;
using Telerik.WinControls.UI;

namespace Dark_Souls_II_Save_Editor.Controls
{
    public partial class SpellProperties : UserControl
    {
        public SpellProperties()
        {
            InitializeComponent();
            ResizeRedraw = true;
        }

        public SpellParameter Instance { get; private set; }

        public void SetSpellParam(SpellParameter x, ItemEntry ent)
        {
            xname.Text = ent.Name;
            xspelldescription.Text = ent.Description;
            ximage.Image = ent.GetImage(true);
            xvalue1.Value = Convert.ToDecimal(x.Value1);
            xvalue2.Value = Convert.ToDecimal(x.Value2);
            xslotused.Value = x.SlotUsed;
            radGridView1.DataSource = x.SpellUsesForEachLevel;
            Instance = x;
        }

        private void radGridView1_ValueChanging(object sender, ValueChangingEventArgs e)
        {
            if (Convert.ToInt32(e.NewValue) > 99)
                e.Cancel = true;
        }

        private void SpellProperties_Resize(object sender, EventArgs e)
        {
            Application.DoEvents();
            Refresh();
        }

        private void xvalue1_ValueChanged(object sender, EventArgs e)
        {
            if (Instance == null)
                return;
            Instance.Value1 = Convert.ToSingle(xvalue1.Value);
        }

        private void xvalue2_ValueChanged(object sender, EventArgs e)
        {
            if (Instance == null)
                return;
            Instance.Value2 = Convert.ToSingle(xvalue2.Value);
        }

        private void xslotused_ValueChanged(object sender, EventArgs e)
        {
            if (Instance == null)
                return;
            Instance.SlotUsed = (byte) xslotused.Value;
        }
    }
}