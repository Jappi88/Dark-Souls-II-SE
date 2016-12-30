using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using Dark_Souls_II_Save_Editor.BND4;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace Dark_Souls_II_Save_Editor.Controls
{
    public partial class AllObjects : UserControl
    {
        public AllObjects()
        {
            InitializeComponent();
        }

        public MainBnd4 MainInstance { get; private set; }

        public void ViewBNDS(MainBnd4 bnd, string filter)
        {
            radListView1.Items.Clear();
            MainInstance = bnd;
            radListView1.BeginUpdate();
            foreach (var v in MainInstance.Entries)
            {
                Application.DoEvents();
                if (!string.IsNullOrEmpty(filter))
                {
                    if (!v.Name.ToLower().Replace(" ", "").Contains(filter.ToLower().Replace(" ", "")))
                        continue;
                }
                radListView1.Items.Add(new ListViewDataItem(v.Name) {Tag = v});
            }
            radListView1.EndUpdate();
        }

        private void radTextBox1_TextChanged(object sender, EventArgs e)
        {
            Application.DoEvents();
            if ((sender as RadTextBox).Text.Replace(" ", "").Length == 0)
                return;
            ViewBNDS(MainInstance, (sender as RadTextBox).Text);
        }

        private void radContextMenu1_DropDownOpening(object sender, CancelEventArgs e)
        {
            radContextMenu1.Items.Clear();
            if (radListView1.SelectedItem == null)
                e.Cancel = true;
            else
            {
                var rb = new RadMenuItem();
                rb.Text = "Extract Entry";
                rb.Click += radContextMenu1_ItemClick;
                radContextMenu1.Items.Add(rb);
                rb = new RadMenuItem();
                rb.Text = "Replace Entry";
                rb.Click += radContextMenu1_ItemClick;
                radContextMenu1.Items.Add(rb);
            }
        }

        private void radContextMenu1_ItemClick(object sender, EventArgs e)
        {
            if (radListView1.SelectedItem == null)
                return;
            var entry = radListView1.SelectedItems[0].Tag as BndEntry;
            if (entry == null)
                return;
            var ri = sender as RadMenuItem;
            if (ri.Text == "Extract Entry")
            {
                var sv = new SaveFileDialog();
                sv.FileName = entry.Name;
                sv.Title = "Extract Entry";
                if (sv.ShowDialog() == DialogResult.OK)
                {
                    var x = false;
                    using (var fs = File.Create(sv.FileName))
                    {
                        x = entry.ExtractToStream(fs);
                    }
                    if (x)
                        RadMessageBox.Show(entry.Name + " Succesfully extracted", "Extracted", MessageBoxButtons.OK,
                            RadMessageIcon.Info);
                    else
                        RadMessageBox.Show("Failed to extracted " + entry.Name, "Error", MessageBoxButtons.OK,
                            RadMessageIcon.Error);
                }
            }
            else
            {
                var sv = new OpenFileDialog();
                sv.FileName = entry.Name;
                sv.Title = "Replace Entry";
                if (sv.ShowDialog() == DialogResult.OK)
                {
                    var x = false;
                    if (new FileInfo(sv.FileName).Length != entry.Length)
                        RadMessageBox.Show("Extry to replace must be the same size as " + entry.Name, "Error",
                            MessageBoxButtons.OK, RadMessageIcon.Error);
                    else
                    {
                        x = entry.Replace(File.ReadAllBytes(sv.FileName));
                        if (x)
                            RadMessageBox.Show(entry.Name + " Succesfully Replaced", "Extracted", MessageBoxButtons.OK,
                                RadMessageIcon.Info);
                        else
                            RadMessageBox.Show("Failed to Replace " + entry.Name, "Error", MessageBoxButtons.OK,
                                RadMessageIcon.Error);
                    }
                }
            }
        }
    }
}