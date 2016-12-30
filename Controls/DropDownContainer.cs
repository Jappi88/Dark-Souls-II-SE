using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Dark_Souls_II_Save_Editor.Properties;
using Dark_Souls_II_Save_Editor.SaveStuff;
using Telerik.WinControls.UI;

namespace Dark_Souls_II_Save_Editor.Controls
{
    public partial class DropDownContainer : UserControl
    {
        public DropDownContainer(ItemEntry[] items, Control[] equiped, int rows, Control owner)
        {
            InitializeComponent();
            Height = 15 + rows*100;
            itemcontainer.LostFocus += itemcontainer_LostFocus;
            itemcontainer.RowsCount = rows;
            var rb = new RadTileElement();
            rb.ImageAlignment = ContentAlignment.MiddleCenter;
            rb.AutoToolTip = true;
            rb.ToolTipText = @"Empty Slot";
            rb.Size = new Size(80, 100);
            rb.Image = Resources.Recycle_Bin_Empty_icon;
            rb.Click += rb_Click;
            rb.Tag = new object[] {owner.Text, null};
            itemcontainer.Items.Add(rb);
            foreach (var item in items)
            {
                Application.DoEvents();
                rb = new RadTileElement();
                rb.AutoToolTip = true;
                rb.ImageAlignment = ContentAlignment.MiddleLeft;
                rb.ImageLayout = ImageLayout.Stretch;
                rb.ToolTipText = item.Name + "\n\n" + item.Description;
                rb.Tag = new object[] {owner.Text, item};
                var text =
                    (from s in equiped let x = s.Tag as ItemEntry where x != null where x.ID1 == item.ID1 select s.Text)
                        .FirstOrDefault();
                rb.Image = text == null
                    ? item.GetImage(false)
                    : Functions.DrawTextOnImage(item.GetImage(false), text, 10, false, new Point(37, 75));
                rb.Click += rb_Click;
                itemcontainer.Items.Add(rb);
            }
        }

        public event EventHandler OnItemChoosed;
        public event EventHandler OnMouseEnterd;
        public event EventHandler OnMouseLeft;
        public event EventHandler OnFocusLost;

        protected virtual void OnOnItemChoosed(object sender)
        {
            OnItemChoosed?.Invoke(sender, EventArgs.Empty);
        }

        private void itemcontainer_LostFocus(object sender, EventArgs e)
        {
            OnFocusLost?.Invoke(sender, e);
        }

        private void rb_Click(object sender, EventArgs e)
        {
            OnOnItemChoosed(sender);
        }

        private void radScrollablePanel1_MouseEnter(object sender, EventArgs e)
        {
            OnMouseEnterd?.Invoke(this, e);
        }

        private void radScrollablePanel1_MouseLeave(object sender, EventArgs e)
        {
            OnMouseLeft?.Invoke(this, e);
        }
    }
}