using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Dark_Souls_II_Save_Editor.SaveStuff;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace Dark_Souls_II_Save_Editor.Controls
{
    public partial class ToolsMenu : UserControl
    {
        public SaveSlot Slot;

        public ToolsMenu(MainFile file, SaveSlot slot)
        {
            InitializeComponent();
            Slot = slot;
            if (file != null && file.UsedSlots.Length > 0)
            {
                foreach (var x in file.UsedSlots)
                {
                    if (x.SlotName == slot.SlotName)
                        continue;
                    RadButtonItem item = new RadMenuButtonItem {Text = x.Name + "(" + x.Level + ")"};
                    item.Click += item1_Click;
                    item.Tag = x;
                    item.Size = new Size(150, 35);
                    item.Font = new Font("Arial", 10, FontStyle.Bold);
                    xcopystats.Items.Add(item);

                    item = new RadMenuButtonItem {Text = x.Name + "(" + x.Level + ")"};
                    item.Click += item2_Click;
                    item.Tag = x;
                    item.Size = new Size(150, 35);
                    item.Font = new Font("Arial", 10, FontStyle.Bold);
                    xcopystats.Items.Add(item);
                    xcopyinventory.Items.Add(item);

                    item = new RadMenuButtonItem {Text = x.Name + "(" + x.Level + ")"};
                    item.Click += item3_Click;
                    item.Tag = x;
                    item.Size = new Size(150, 35);
                    item.Font = new Font("Arial", 10, FontStyle.Bold);
                    xcopystats.Items.Add(item);
                    xcopyprogress.Items.Add(item);

                    item = new RadMenuButtonItem {Text = x.Name + "(" + x.Level + ")"};
                    item.Click += item4_Click;
                    item.Tag = x;
                    item.Size = new Size(150, 35);
                    item.Font = new Font("Arial", 10, FontStyle.Bold);
                    xcopystats.Items.Add(item);
                    xcopybuild.Items.Add(item);
                }
            }
        }

        private void item1_Click(object sender, EventArgs e)
        {
            var bi = sender as RadMenuButtonItem;
            if (bi != null)
            {
                var slot = bi.Tag as SaveSlot;
                if (slot != null)
                {
                    try
                    {
                        slot.SaveBlocks[0].SetBlock(Slot.SaveBlocks[0].BlockData);
                        RadMessageBox.Show(Slot.Name + " Stats has been succesfully copied to " + slot.Name);
                    }
                    catch (Exception ex)
                    {
                        RadMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, RadMessageIcon.Error);
                    }
                }
            }
        }

        private void item2_Click(object sender, EventArgs e)
        {
            var bi = sender as RadMenuButtonItem;
            if (bi != null)
            {
                var slot = bi.Tag as SaveSlot;
                if (slot != null)
                {
                    try
                    {
                        slot.SaveBlocks[4].SetBlock(Slot.SaveBlocks[4].BlockData);
                        RadMessageBox.Show(Slot.Name + " Inventory has been succesfully copied to " + slot.Name);
                    }
                    catch (Exception ex)
                    {
                        RadMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, RadMessageIcon.Error);
                    }
                }
            }
        }

        private void item3_Click(object sender, EventArgs e)
        {
            var bi = sender as RadMenuButtonItem;
            if (bi != null)
            {
                var slot = bi.Tag as SaveSlot;
                if (slot != null)
                {
                    try
                    {
                        slot.MainInstance.SetSlotProgress(Slot.MainInstance.GetSlotProgress(Slot), slot);
                        RadMessageBox.Show(Slot.Name + " Game Progress has been succesfully copied to " + slot.Name);
                    }
                    catch (Exception ex)
                    {
                        RadMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, RadMessageIcon.Error);
                    }
                }
            }
        }

        private void item4_Click(object sender, EventArgs e)
        {
            var bi = sender as RadMenuButtonItem;
            if (bi != null)
            {
                var slot = bi.Tag as SaveSlot;
                if (slot != null)
                {
                    try
                    {
                        slot.SaveBlocks[3].SetBlock(Slot.SaveBlocks[3].BlockData);
                        RadMessageBox.Show(Slot.Name + " Build has been succesfully copied to " + slot.Name);
                    }
                    catch (Exception ex)
                    {
                        RadMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, RadMessageIcon.Error);
                    }
                }
            }
        }

        private void Extract(byte[] data, string title, string filter, string filename)
        {
            var x = new SaveFileDialog();
            x.Title = title;
            x.Filter = filter;
            x.FileName = filename;
            if (x.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    File.WriteAllBytes(x.FileName, data);
                }
                catch (Exception ex)
                {
                    RadMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, RadMessageIcon.Error);
                }
            }
        }

        private byte[] Replace(string title, string filter, string filename)
        {
            var ofd = new OpenFileDialog();
            ofd.Title = title;
            ofd.Filter = filter;
            ofd.FileName = filename;
            if (ofd.ShowDialog() != DialogResult.OK)
                return null;
            return File.ReadAllBytes(ofd.FileName);
        }

        private void xextractchar_Click(object sender, EventArgs e)
        {
            if (Slot == null || Slot.SaveBlocks == null || Slot.SaveBlocks.Length != 8)
                return;
            Extract(Slot.SaveBlocks[0].BlockData, "Extract Character Statics", "CHRST|*.CHRST", Slot.Name + "_Statics");
        }

        private void xextractinventory_Click(object sender, EventArgs e)
        {
            if (Slot == null || Slot.SaveBlocks == null || Slot.SaveBlocks.Length != 8)
                return;
            Extract(Slot.SaveBlocks[4].BlockData, "Extract Inventory", "CHRIT|*.CHRIT", Slot.Name + "_Inventory");
        }

        private void xextractgameprogress_Click(object sender, EventArgs e)
        {
            if (Slot == null || Slot.SaveBlocks == null || Slot.SaveBlocks.Length != 8)
                return;
            Extract(Slot.MainInstance.GetSlotProgress(Slot), "Extract Game Progress", "GPRS|*.GPRS",
                Slot.Name + "_Progress");
        }

        private void xextractbuild_Click(object sender, EventArgs e)
        {
            if (Slot == null || Slot.SaveBlocks == null || Slot.SaveBlocks.Length != 8)
                return;
            Extract(Slot.SaveBlocks[3].BlockData, "Extract Character Build", "CHRBLD|*.CHRBLD", Slot.Name + "_Build");
        }

        private void xreplacechar_Click(object sender, EventArgs e)
        {
            if (Slot == null || Slot.SaveBlocks == null || Slot.SaveBlocks.Length != 8)
                return;
            var data = Replace("Replace Character Statics", "CHRST|*.CHRST", Slot.Name + "_Statics");
            if (data != null)
            {
                try
                {
                    Slot.SaveBlocks[0].SetBlock(data);
                    RadMessageBox.Show("Character Statics succesfully replaced", "Replaced", MessageBoxButtons.OK,
                        RadMessageIcon.Info);
                }
                catch (Exception ex)
                {
                    RadMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, RadMessageIcon.Error);
                }
            }
        }

        private void xreplaceinventory_Click(object sender, EventArgs e)
        {
            if (Slot == null || Slot.SaveBlocks == null || Slot.SaveBlocks.Length != 8)
                return;
            var data = Replace("Replace Inventory", "CHRIT|*.CHRIT", Slot.Name + "_Inventory");
            if (data != null)
            {
                try
                {
                    Slot.SaveBlocks[4].SetBlock(data);
                    RadMessageBox.Show("Character Inventory succesfully replaced", "Replaced", MessageBoxButtons.OK,
                        RadMessageIcon.Info);
                }
                catch (Exception ex)
                {
                    RadMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, RadMessageIcon.Error);
                }
            }
        }

        private void xreplacegameprogress_Click(object sender, EventArgs e)
        {
            if (Slot == null || Slot.SaveBlocks == null || Slot.SaveBlocks.Length != 8)
                return;
            var data = Replace("Replace Game Progress", "GPRS|*.GPRS", Slot.Name + "_Progress");
            if (data != null)
            {
                try
                {
                    Slot.MainInstance.SetSlotProgress(data, Slot);
                    RadMessageBox.Show("Character GameProgress succesfully replaced", "Replaced", MessageBoxButtons.OK,
                        RadMessageIcon.Info);
                }
                catch (Exception ex)
                {
                    RadMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, RadMessageIcon.Error);
                }
            }
        }

        private void xreplacebuild_Click(object sender, EventArgs e)
        {
            if (Slot == null || Slot.SaveBlocks == null || Slot.SaveBlocks.Length != 8)
                return;
            var data = Replace("Replace Character Build", "CHRBLD|*.CHRBLD", Slot.Name + "_Build");
            if (data != null)
            {
                try
                {
                    Slot.SaveBlocks[3].SetBlock(data);
                    RadMessageBox.Show("Character Build succesfully replaced", "Replaced", MessageBoxButtons.OK,
                        RadMessageIcon.Info);
                }
                catch (Exception ex)
                {
                    RadMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, RadMessageIcon.Error);
                }
            }
        }
    }
}