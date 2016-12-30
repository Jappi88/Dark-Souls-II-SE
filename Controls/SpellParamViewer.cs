using System;
using System.Windows.Forms;
using Dark_Souls_II_Save_Editor.Parameters;
using Dark_Souls_II_Save_Editor.SaveStuff;
using Telerik.WinControls.UI;

namespace Dark_Souls_II_Save_Editor.Controls
{
    public partial class SpellParamViewer : UserControl
    {
        public SpellParamViewer(SpellParameter[] xparams)
        {
            InitializeComponent();
            ResizeRedraw = true;
            Parameters = xparams;

            ReList();
        }

        public SpellParameter[] Parameters { get; }

        public void ReList()
        {
            if (Parameters == null)
                return;
            xitems.BeginUpdate();
            xitems.Items.Clear();
            foreach (var param in Parameters)
            {
                if (param.DBInstances != null && param.DBInstances.Length > 0)
                {
                    foreach (var ent in param.DBInstances)
                    {
                        Application.DoEvents();
                        var lv = new ListViewDataItem();
                        lv.Text = ent.Name;
                        lv.Image = ent.GetImage(true);
                        lv.Tag = new object[] {param, ent};
                        xitems.Items.Add(lv);
                    }
                }
            }
            xitems.EndUpdate();
        }

        private void radListView1_SelectedItemChanged(object sender, EventArgs e)
        {
            if (xitems.SelectedItem == null)
                return;
            if (xitems.SelectedItem.Tag == null)
                return;
            var x = xitems.SelectedItem.Tag as object[];
            if (x == null || x.Length != 2)
                return;
            spellProperties1.SetSpellParam(x[0] as SpellParameter, x[1] as ItemEntry);
        }

        private void spellProperties1_Resize(object sender, EventArgs e)
        {
            Application.DoEvents();
            spellProperties1.Update();
            spellProperties1.Invalidate();
        }

        private void SpellParamViewer_SizeChanged(object sender, EventArgs e)
        {
            Application.DoEvents();
            Update();
            Invalidate();
        }
    }
}