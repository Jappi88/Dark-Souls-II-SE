using Dark_Souls_II_Save_Editor.SaveStuff;
using Telerik.WinControls.UI;

namespace Dark_Souls_II_Save_Editor.Forms
{
    public partial class SetWeaponInfo : RadForm
    {
        public SetWeaponInfo(ItemEntry item, SaveSlot slot)
        {
            InitializeComponent();

            Text = "Set Weapon Stats : " + item.Name;
            weaponStatEditor1.SetItem(item, slot);
        }
    }
}