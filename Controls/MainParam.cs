using System;
using System.Drawing;
using System.Windows.Forms;
using Dark_Souls_II_Save_Editor.Parameters;
using Dark_Souls_II_Save_Editor.SaveStuff;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace Dark_Souls_II_Save_Editor.Controls
{
    public partial class MainParam : UserControl
    {
        public MainParam()
        {
            InitializeComponent();
        }

        public MainParamManager Parameters { get; private set; }
        private SpellParamViewer viewer { get; set; }

        public void LoadBND4(MainParamManager manager)
        {
            Parameters = manager;
            xothers.PerformClick();
        }

        private void xothers_Click(object sender, EventArgs e)
        {
            var x = sender as RadTileElement;
            if (x == null)
                return;
            x.SetSelected(radPanorama1, Color.Blue);
            var mp = new AllObjects();
            mp.Dock = DockStyle.Fill;
            mp.ViewBNDS(Parameters.GameParameters, "");
            xcontainer.Controls.Clear();
            xcontainer.Controls.Add(mp);
        }

        private void xsave_Click(object sender, EventArgs e)
        {
            var c = xcontainer.Controls[0];
            try
            {
                //if (MainFile.Backup())
                //{

                //    xcontainer.Controls.Clear();
                //    var pv = new Progressview();
                //    pv.Dock = DockStyle.Fill;
                //    Parameters.MainFile.ProgressInstance.AtProgress -= pv.DoProgress;
                //    Parameters.MainFile.ProgressInstance.AtProgress += pv.DoProgress;
                //    xcontainer.Controls.Add(pv);
                //    Parameters.MainInstance.SaveAll(false);
                //    RadMessageBox.Show("Parameters succesfully saved!", "Saved", MessageBoxButtons.OK, RadMessageIcon.Info);
                //}
            }
            catch (Exception ex)
            {
                RadMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, RadMessageIcon.Error);
            }
            xcontainer.Controls.Clear();
            xcontainer.Controls.Add(c);
        }

        private void xspellparams_Click(object sender, EventArgs e)
        {
            var x = sender as RadTileElement;
            if (x == null)
                return;
            x.SetSelected(radPanorama1, Color.Blue);
            if (viewer == null)
            {
                viewer = new SpellParamViewer(Parameters.SpellParameters.SpellParameters);
                viewer.Dock = DockStyle.Fill;
                viewer.Resize += xspellparams_Resize;
            }
            else
                viewer.ReList();
            xcontainer.Controls.Clear();
            xcontainer.Controls.Add(viewer);
        }

        private void xspellparams_Resize(object sender, EventArgs e)
        {
            Application.DoEvents();
            Update();
            Refresh();
            Invalidate();
            xcontainer.Update();
            xcontainer.Refresh();
            xcontainer.Invalidate();
        }

        private void xitemparams_Click(object sender, EventArgs e)
        {
            var x = sender as RadTileElement;
            if (x == null)
                return;
            x.SetSelected(radPanorama1, Color.Blue);
            xcontainer.SuspendLayout();
            xcontainer.Controls.Clear();
            var i = new DataBaseControl();
            i.LoadItems(Parameters, MainFile.ItemTypes);
            i.Dock = DockStyle.Fill;
            xcontainer.Controls.Add(i);
            xcontainer.ResumeLayout();
        }
    }
}