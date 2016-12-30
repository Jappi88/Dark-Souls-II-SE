using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Dark_Souls_2_Resource_Extractor.DMG;
using Dark_Souls_II_Save_Editor.SaveStuff;
using Telerik.WinControls.UI;

namespace Dark_Souls_II_Save_Editor.Controls
{
    public partial class AreasUI : UserControl
    {
        public AreasUI()
        {
            InitializeComponent();
        }

        public AreasUI(DMGFile areas, DMGFile bonfires, SaveSlot slot)
        {
            InitializeComponent();
            var sb = new StringBuilder();
            var xavailaible = slot.MapInfo.AvailibleAreas().OrderBy(x => GetID(x.AreaId));
            for (var i = 0; i < areas.Entries.Length; i++)
            {
                var ent = xavailaible.FirstOrDefault(t => IDToShort(areas.Entries[i].ID1) == t.AreaId);
                if (ent != null)
                    sb.AppendLine(ent.AreaId + " : " + areas.Entries[i].ID1 + " : " + ent.AreaIDS.Count);
                else
                {
                    sb.AppendLine(areas.Entries[i].ID1 + " : " + IDToShort(areas.Entries[i].ID1));
                }
            }
            Console.WriteLine(sb.ToString());
            //var xarea = areas.Entries.Where(x => x.Image != null).ToArray();
            var xarea = areas.Entries.OrderBy(x => x.ID1).ToArray();

            var count = 0;
            foreach (var v in xavailaible)
            {
                var ent = xarea.FirstOrDefault(t => IDToShort(t.ID1) == v.AreaId);
                //if (ent != null)
                //{
                //    var item = new ListViewDataItem();
                //    item.Tag = ent;
                //    item.Image = ent.Image;
                //    item.Text = ent.Name;
                //    xareas.Items.Add(item);
                //}
                //else
                //{
                if (ent != null)
                {
                    var item = new ListViewDataItem();
                    item.Tag = v;
                    item.Image = ent.Image;
                    item.Text = ent.Name;
                    xareas.Items.Add(item);
                    count++;
                }
                else
                {
                    Console.WriteLine(v.AreaId);
                }
                //}
            }
            //var list = new List<DmgEntry>() { };
            //foreach (var ent in areas.Entries)
            //{
            //    if (string.IsNullOrEmpty(ent.Name) || ent.Image == null)
            //        continue;
            //    list.Add(ent);
            //    var item = new ListViewDataItem();
            //    item.Tag = ent;
            //    item.Image = ent.Image;
            //    item.Text = ent.Name;
            //    xareas.Items.Add(item);
            //}

            foreach (var v in slot.MapInfo.AvailibleBonfires)
            {
                var ent = bonfires.Entries.FirstOrDefault(t => t.ID1 == v.Id);
                if (ent != null)
                {
                    var item = new ListViewDataItem();
                    item.Tag = ent;
                    item.Image = ent.Image;
                    item.Text = ent.Name;
                    xbonfires.Items.Add(item);
                }
            }
            areas.MainStream.Close();
            bonfires.MainStream.Close();
        }

        public AreasUI(SaveSlot slot)
        {
            InitializeComponent();
        }

        private uint GetID(ushort mapid)
        {
            var id = mapid.ToString("X2").PadLeft(4, '0');
            var first = byte.Parse(id.Substring(0, 2), NumberStyles.HexNumber);
            var second = byte.Parse(id.Substring(2, 2), NumberStyles.HexNumber);
            var fullid = "0";
            fullid += first.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0');
            fullid += second.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0');
            fullid = fullid.PadRight(9, '0');
            return fullid.GetIdFromName();
        }

        private ushort IDToShort(uint id)
        {
            var fullid = id.ToString(CultureInfo.InvariantCulture);
            var x1 = byte.Parse(fullid.Substring(0, 2)).ToString("X2");
            var x2 = byte.Parse(fullid.Substring(2, 2));
            var x3 = byte.Parse(fullid.Substring(fullid.Length - 2, 2));
            x1 += (x2 + x3).ToString("X2");
            return ushort.Parse(x1, NumberStyles.HexNumber);
        }

        private void xareas_SelectedItemChanged(object sender, EventArgs e)
        {
            if (xareas.SelectedItem == null)
                xareaproperty.SelectedObject = null;
            else
                xareaproperty.SelectedObject = xareas.SelectedItem.Tag as DmgEntry;
        }

        private void xbonfires_SelectedItemChanged(object sender, EventArgs e)
        {
            if (xbonfires.SelectedItem == null)
                xbonfireproperty.SelectedObject = null;
            else
                xbonfireproperty.SelectedObject = xbonfires.SelectedItem.Tag as DmgEntry;
        }
    }
}