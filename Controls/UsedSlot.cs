using System;
using System.Windows.Forms;
using Dark_Souls_II_Save_Editor.SaveStuff;
using Telerik.WinControls;

//using System.Linq;

namespace Dark_Souls_II_Save_Editor.Controls
{
    public partial class UsedSlot : UserControl
    {
        public delegate void SlotChangedHandler(SaveSlot slot);

        public UsedSlot(SaveSlot slot)
        {
            InitializeComponent();
            Tag = slot;
            xdescription.Text = string.Format("Name : {0}\nLevel : {1}\nClass : {2}\nPlayed : {3}",slot.Name,slot.Level, Enum.GetName(typeof(SaveSlot.Class), slot.CurrentClass), slot.TimePayed);
            slot.OnValueChanged += slot_OnValueChanged;
            xload.Text = "Load " + slot.Name;
            xextract.Text = "Extract " + slot.Name;
            xreplace.Text = "Replace " + slot.Name;
            xdelete.Text = "Delete " + slot.Name;
        }

        public event SlotChangedHandler OnSlotChanged;
        public event SlotChangedHandler OnSlotSelected;

        private void slot_OnValueChanged(SaveSlot slot)
        {
            Application.DoEvents();
            if (xdescription.InvokeRequired)
            {
                xload.Invoke(new MethodInvoker(() => xload.Text = "Load " + slot.Name));
                xextract.Invoke(new MethodInvoker(() => xextract.Text = "Extract " + slot.Name));
                xreplace.Invoke(new MethodInvoker(() => xreplace.Text = "Replace " + slot.Name));
                xdelete.Invoke(new MethodInvoker(() => xdelete.Text = "Delete " + slot.Name));
                xdescription.Invoke(
                    new MethodInvoker(
                        () =>
                            xdescription.Text = string.Format("Name : {0}\nLevel : {1}\nClass : {2}\nPlayed : {3}", slot.Name, slot.Level, Enum.GetName(typeof(SaveSlot.Class), slot.CurrentClass), slot.TimePayed)
                ))
                ;
            }
            else
            {
                xload.Text = "Load " + slot.Name;
                xextract.Text = "Extract " + slot.Name;
                xreplace.Text = "Replace " + slot.Name;
                xdelete.Text = "Delete " + slot.Name;
                xdescription.Text = string.Format("Name : {0}\nLevel : {1}\nClass : {2}\nPlayed : {3}", slot.Name, slot.Level, Enum.GetName(typeof(SaveSlot.Class), slot.CurrentClass), slot.TimePayed);
            }
            if (InvokeRequired)
                Invoke(new MethodInvoker(Update));
            else
                Update();
        }

        private void xextract_Click(object sender, EventArgs e)
        {
            var s = Tag as SaveSlot;
            if (s != null)
            {
                var ofd = new SaveFileDialog {Title = "Extract " + s.Name, FileName = s.Name, Filter = "CHR File|*.CHR"};
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        s.MainInstance.ExtractSlot(s, ofd.FileName);
                        RadMessageBox.Show(s.Name + " Successfully extracted", "Extracted", MessageBoxButtons.OK,
                            RadMessageIcon.Info);
                    }
                    catch (Exception ex)
                    {
                        RadMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, RadMessageIcon.Error);
                    }
                }
            }
        }

        private void xreplace_Click(object sender, EventArgs e)
        {
            var s = Tag as SaveSlot;
            if (s != null)
            {
                var ofd = new OpenFileDialog {Title = "Replace " + s.Name, FileName = s.Name, Filter = "CHR File|*.CHR"};
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var save = s.MainInstance.ReplaceSlot(s, ofd.FileName);
                        if (OnSlotChanged != null && save != null)
                            OnSlotChanged(save);
                        RadMessageBox.Show(s.Name + " Successfully replaced", "Replaced", MessageBoxButtons.OK,
                            RadMessageIcon.Info);
                    }
                    catch (Exception ex)
                    {
                        RadMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, RadMessageIcon.Error);
                    }
                }
            }
        }

        private void xdelete_Click(object sender, EventArgs e)
        {
            RadMessageBox.Show("Not implemented yet");
            if (Tag == null)
                return;
        }

        private void xload_Click(object sender, EventArgs e)
        {
            var s = Tag as SaveSlot;
            if (s != null)
            {
                Application.DoEvents();
                xload.Text = "Loading " + s.Name + "...";
                xload.Invalidate();
                Application.DoEvents();
                OnSlotSelected?.Invoke(s);
                Application.DoEvents();
                xload.Text = "Load " + s.Name;
                xload.Invalidate();
            }
        }

        private void UsedSlot_DoubleClick(object sender, EventArgs e)
        {
            xload_Click(sender, e);
        }
    }
}