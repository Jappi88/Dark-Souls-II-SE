using System.Windows.Forms;
using Dark_Souls_II_Save_Editor.SaveStuff;

namespace Dark_Souls_II_Save_Editor.Controls
{
    public partial class ItemParamProperties : UserControl
    {
        public ItemParamProperties()
        {
            InitializeComponent();
        }

        public void SetProperties(ItemEntry item, object data)
        {
            if (item == null)
            {
                xproperty.SelectedObjects = null;
                ximage.Image = null;
                xselectedname.Text = "";
                xselectedinfo.Text = "";
            }
            else
            {
                ximage.Image = item.GetImage(false);
                xproperty.SelectedObject = data;
                xselectedinfo.Text = item.Description;
                xselectedname.Text = item.Name;
            }
        }
    }
}