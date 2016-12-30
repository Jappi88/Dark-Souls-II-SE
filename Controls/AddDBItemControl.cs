using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Dark_Souls_II_Save_Editor.SaveStuff;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace Dark_Souls_II_Save_Editor.Controls
{
    public partial class AddDBItemControl : UserControl
    {
        public delegate void ItemAddedHandler(ItemEntry item);

        public AddDBItemControl()
        {
            InitializeComponent();
        }

        public event ItemAddedHandler OnItemAdded;

        public void LoadInstance()
        {
            if (MainFile.ItemTypes == null)
                return;
            xtypes.Items.Clear();
            foreach (var v in MainFile.ItemTypes)
            {
                if (v.StartID < 1000 && (v.StartID < 20 || v.StartID > 120) && v.StartID != 160)
                {
                    xtypes.Items.Add(new RadListDataItem(v.ItemType.ToString()) {Tag = v});
                }
            }
        }

        private void xname_Enter(object sender, EventArgs e)
        {
            if (xname.Text == "Enter Name..")
                xname.Text = "";
        }

        private void xname_Leave(object sender, EventArgs e)
        {
            if (xname.Text.Replace(" ", "") == "")
                xname.Text = "Enter Name..";
        }

        private void xdescription_Leave(object sender, EventArgs e)
        {
            if (xdescription.Text.Replace(" ", "") == "")
                xdescription.Text = "Enter Description..";
        }

        private void xdescription_Enter(object sender, EventArgs e)
        {
            if (xdescription.Text == "Enter Description..")
                xdescription.Text = "";
        }

        private void xsetimage_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Title = "Choose Item Image";
            ofd.Filter = "PNG Image|*.PNG";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var i = Image.FromFile(ofd.FileName);
                    ximage.Image = i.GetThumbnailImage(100, 100, null, IntPtr.Zero);
                }
                catch (Exception ex)
                {
                    RadMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, RadMessageIcon.Error);
                }
            }
        }

        private void xadd_Click(object sender, EventArgs e)
        {
            try
            {
                if (xname.Text == "Enter Name.." || string.IsNullOrWhiteSpace(xname.Text))
                    throw new Exception("Please enter a valid item name");
                if (xid.Value == 0 || xid.Value == xid.Maximum)
                    throw new Exception("Please enter valid item id");
                if (xtypes.SelectedIndex == -1)
                    throw new Exception("Please select the item catagory");
                if (ximage.Image == null)
                    throw new Exception("Please select valid item image");
                if (xdescription.Text == "Enter Description.." || string.IsNullOrWhiteSpace(xdescription.Text))
                    throw new Exception("Please enter valid item description");
                if (MainFile.DataBase.Items.FirstOrDefault(t => t.ID1 == xid.Value) != null)
                    throw new Exception("Item Already Exist inside the Database, Please provide a new ID");
                if (OnItemAdded != null)
                {
                    var item =
                        new ItemEntry(Functions.GetInstance(MainFile.ItemTypes,
                            (int) (xtypes.SelectedItem.Tag as ItemTypeInstance).StartID + 1))
                        {
                            Name = xname.Text,
                            ID1 = (uint) xid.Value,
                            ID2 = (uint) xid.Value,
                            Description = xdescription.Text,
                            Index = (uint) MainFile.DataBase.Items.Length,
                            EntryIndex = (int) (xtypes.SelectedItem.Tag as ItemTypeInstance).StartID + 1
                        };
                    item.ImageStream = new MemoryStream();
                    ximage.Image.GetThumbnailImage(64, 128, null, IntPtr.Zero).Save(item.ImageStream, ImageFormat.Png);
                    OnItemAdded(item);
                }
            }
            catch (Exception ex)
            {
                RadMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, RadMessageIcon.Exclamation);
            }
        }
    }
}