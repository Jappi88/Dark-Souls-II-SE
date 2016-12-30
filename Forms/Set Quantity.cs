using System;
using System.Windows.Forms;
using Dark_Souls_II_Save_Editor.SaveStuff;
using Telerik.WinControls.UI;

namespace Dark_Souls_II_Save_Editor.Forms
{
    public partial class Set_Quantity : RadForm
    {
        internal ItemTypeInstance _type;
        internal ItemEntry ent;

        public Set_Quantity(ItemEntry item, ItemTypeInstance type)
        {
            InitializeComponent();
            if (item.Data == null)
                item.GenerateData(item.IsInItemBox);
            ent = item;
            _type = type;
            var flag = item.Type.ItemType == ItemTypeInstance.ItemTypes.Item && item.CanHaveMoreThenOne;
            if (_type.ItemType == ItemTypeInstance.ItemTypes.Spell)
            {
                radLabel1.Text = "How many times would you like to use " + ent.Name + "?";
                Text = "Set Spell Uses : " + ent.Name;
                radSpinEditor1.Maximum = 99;
                if (item.SpellUses == 0)
                    item.SpellUses = 1;
                else if (item.SpellUses > 99)
                    item.SpellUses = 99;
                radSpinEditor1.Value = item.SpellUses;
            }
            else if (flag || _type.ItemType == ItemTypeInstance.ItemTypes.Shard ||
                     _type.ItemType == ItemTypeInstance.ItemTypes.Arrow)
            {
                radLabel1.Text = "How many " + item.Name + " do you need?";
                Text = "Set Quantity : " + item.Name;

                if (item.Type.ItemType == ItemTypeInstance.ItemTypes.Arrow)
                {
                    radSpinEditor1.Maximum = 9999;
                    if (item.Quantity > 9999)
                        item.Quantity = 9999;
                }
                else
                {
                    radSpinEditor1.Maximum = 99;
                    if (item.Quantity > 99)
                        item.Quantity = 99;
                }
                if (item.Quantity == 0)
                    item.Quantity = 1;
                radSpinEditor1.Value = item.Quantity;
            }
            else
            {
                radLabel1.Text = "How many " + item.Name + " do you need?";
                radSpinEditor1.Maximum = 999;
                radSpinEditor1.Value = 1;
            }
        }

        public decimal SelectedAmmount => radSpinEditor1.Value;

        private void radButton2_Click(object sender, EventArgs e)
        {
            //if(_type.ItemType == ItemTypeInstance.ItemTypes.Item || _type.ItemType == ItemTypeInstance.ItemTypes.Arrow || _type.ItemType == ItemTypeInstance.ItemTypes.Shard)
            //    ent.Quantity = (ushort)radSpinEditor1.Value;
            DialogResult = DialogResult.OK;
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            radSpinEditor1.Value = radSpinEditor1.Maximum;
        }

        private void radButton3_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void radSpinEditor1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                DialogResult = DialogResult.OK;
        }
    }
}