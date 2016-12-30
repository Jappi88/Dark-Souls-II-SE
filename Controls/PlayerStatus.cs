using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Dark_Souls_II_Save_Editor.Properties;
using Dark_Souls_II_Save_Editor.SaveStuff;
using HavenInterface.Popup;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using PositionChangedEventArgs = Telerik.WinControls.UI.Data.PositionChangedEventArgs;

namespace Dark_Souls_II_Save_Editor.Controls
{
    public partial class PlayerStatus : UserControl
    {
        public Popup xpop;

        public PlayerStatus()
        {
            InitializeComponent();
            Application.DoEvents();

            foreach (var type in Enum.GetValues(typeof (SaveSlot.Class)))
                xclass.Items.Add(Enum.GetName(typeof (SaveSlot.Class), type));
            xgender.Items.Add("Male");
            xgender.Items.Add("Female");
        }

        public SaveSlot UsedSlot { get; private set; }
        public LegitStats Legit { get; private set; }

        public void SetValue(RadSpinEditor editor, decimal value)
        {
            if (value < editor.Minimum)
                value = 0;
            editor.Value = value > editor.Maximum ? editor.Maximum : value;
        }

        public void SetSlot(SaveSlot slot)
        {
            try
            {
                slot.OnDBLanguageChanged -= slot_OnDBLanguageChanged;
                slot.OnDBLanguageChanged += slot_OnDBLanguageChanged;
                Legit = new LegitStats(slot, this);
                SetValue(xlevel, slot.Level);
                SetValue(xsoulsmemory, slot._SoulsMemory);
                SetValue(xsoulsneeded, slot.SoulsNeeded);
                SetValue(xsouls, slot.Souls);
                SetValue(xvgr, slot.VGR);
                SetValue(xend, slot.END);
                SetValue(xvit, slot.VIT);
                SetValue(xatn, slot.ATN);
                SetValue(xstr, slot.STR);
                SetValue(xdex, slot.DEX);
                SetValue(xadp, slot.ADP);
                SetValue(xint, slot.INT);
                SetValue(xfth, slot.FTH);
                xname.Text = slot.Name;
                foreach (var v in xclass.Items)
                {
                    if (v.Text == Enum.GetName(typeof (SaveSlot.Class), slot.CurrentClass))
                    {
                        v.Selected = true;
                        break;
                    }
                }
                xgender.SelectedIndex = (int) slot.Sex;
                xhollowlv.Value = slot.HollowLv > xhollowlv.Maximum
                    ? xhollowlv.Maximum
                    : slot.HollowLv;
                hollowcounter.Text = slot.HollowLv == 0
                    ? "Undead"
                    : "Hollow Lv:" + slot.HollowLv.ToString(CultureInfo.InvariantCulture);
                xintensity.Value = slot.BonfireIntensity > xintensity.Maximum
                    ? xintensity.Maximum
                    : slot.BonfireIntensity;
                playthroughcounter.Text = slot.BonfireIntensity.ToString(CultureInfo.InvariantCulture);
                SetValue(xtorchtime, Convert.ToDecimal(slot.TorchTime));
                UsedSlot = slot;
                //load Covenants
                foreach (Control c in xcovenants.Controls)
                {
                    if (c is RadDropDownButton)
                    {
                        var x = c as RadDropDownButton;
                        var index = byte.Parse(c.Tag.ToString());
                        var locked = UsedSlot.SaveBlocks[0].GetStream(0x9f + index, true).ReadUInt8() == 0;
                        var value = UsedSlot.SaveBlocks[0].GetStream(0xA9 + index, true).ReadUInt8();
                        x.Image = CombineC(GetCDDBImage(index), GetCIcon(locked ? 0 : value > 0 ? value : -1));
                    }
                }

                //load gestures
                foreach (Control c in xgestures.Controls)
                {
                    if (c is RadDropDownButton)
                    {
                        var x = c as RadDropDownButton;
                        var index = (byte) (byte.Parse(x.Text) - 1);
                        var gindex = UsedSlot.SaveBlocks[4].GetStream(0x10058 + index*2, true).ReadUInt16();
                        if (gindex >= 3840)
                            gindex -= 3840;
                        var gesture =
                            UsedSlot.Inventory.Gestures.FirstOrDefault(t => t != null && t.Index == gindex);
                        if (gesture != null)
                        {
                            x.Image = Functions.DrawTextOnImage(gesture.GetImage(false), x.Text, 12, true,
                                new Point(37, 75));
                            x.Tag = new object[] {index.ToString(CultureInfo.InvariantCulture), gesture};
                        }
                        else
                        {
                            x.Image = Resources.Recycle_Bin_Empty_icon;
                            x.Tag = new object[] {index.ToString(CultureInfo.InvariantCulture), null};
                        }
                    }
                }

                //foreach (var c in UsedSlot.Colors)
                //{
                //    var x = new ListViewDataItem(c.Name);
                //    x.BackColor = c;
                //    x.Tag = c;
                //    radListView1.Items.Add(x);
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void slot_OnDBLanguageChanged(SaveSlot slot)
        {
            foreach (Control c in xgestures.Controls)
            {
                if (c is RadDropDownButton)
                {
                    var x = c as RadDropDownButton;
                    var index = (byte) (byte.Parse(x.Text) - 1);
                    var gindex = slot.SaveBlocks[4].GetStream(0x10058 + index*2, true).ReadUInt16();
                    if (gindex >= 3840)
                        gindex -= 3840;
                    var gesture =
                        slot.Inventory.Gestures.FirstOrDefault(t => t.Index == gindex);
                    if (gesture != null)
                    {
                        x.Image = Functions.DrawTextOnImage(gesture.GetImage(false), x.Text, 12, true, new Point(37, 75));
                        x.Tag = gesture;
                    }
                    else
                    {
                        x.Image = Resources.Recycle_Bin_Empty_icon;
                        x.Tag = null;
                    }
                }
            }
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            if (UsedSlot == null)
                return;
            xvgr.Value = xvgr.Maximum;
        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            if (UsedSlot == null)
                return;
            xend.Value = xend.Maximum;
        }

        private void radButton3_Click(object sender, EventArgs e)
        {
            if (UsedSlot == null)
                return;
            xvit.Value = xvit.Maximum;
        }

        private void radButton4_Click(object sender, EventArgs e)
        {
            if (UsedSlot == null)
                return;
            xatn.Value = xatn.Maximum;
        }

        private void radButton5_Click(object sender, EventArgs e)
        {
            if (UsedSlot == null)
                return;
            xstr.Value = xstr.Maximum;
        }

        private void radButton6_Click(object sender, EventArgs e)
        {
            if (UsedSlot == null)
                return;
            xdex.Value = xdex.Maximum;
        }

        private void radButton7_Click(object sender, EventArgs e)
        {
            if (UsedSlot == null)
                return;
            xadp.Value = xadp.Maximum;
        }

        private void radButton8_Click(object sender, EventArgs e)
        {
            if (UsedSlot == null)
                return;
            xint.Value = xint.Maximum;
        }

        private void radButton9_Click(object sender, EventArgs e)
        {
            if (UsedSlot == null)
                return;
            xfth.Value = xfth.Maximum;
        }

        private void radButton10_Click(object sender, EventArgs e)
        {
            if (UsedSlot == null)
                return;
            xtorchtime.Value = xtorchtime.Maximum;
        }

        private void radButton11_Click(object sender, EventArgs e)
        {
            if (UsedSlot == null)
                return;
            xlevel.Value = xlevel.Maximum;
        }

        private void radButton12_Click(object sender, EventArgs e)
        {
            if (UsedSlot == null)
                return;
            xsouls.Value = xsouls.Maximum;
        }

        private void xvgr_ValueChanged(object sender, EventArgs e)
        {
            if (UsedSlot == null)
                return;
            UsedSlot.VGR = (ushort) xvgr.Value;
        }

        private void xend_ValueChanged(object sender, EventArgs e)
        {
            if (UsedSlot == null)
                return;
            UsedSlot.END = (ushort) xend.Value;
        }

        private void xvit_ValueChanged(object sender, EventArgs e)
        {
            if (UsedSlot == null)
                return;
            UsedSlot.VIT = (ushort) xvit.Value;
        }

        private void xatn_ValueChanged(object sender, EventArgs e)
        {
            if (UsedSlot == null)
                return;
            UsedSlot.ATN = (ushort) xatn.Value;
        }

        private void xstr_ValueChanged(object sender, EventArgs e)
        {
            if (UsedSlot == null)
                return;
            UsedSlot.STR = (ushort) xstr.Value;
        }

        private void xdex_ValueChanged(object sender, EventArgs e)
        {
            if (UsedSlot == null)
                return;
            UsedSlot.DEX = (ushort) xdex.Value;
        }

        private void xadp_ValueChanged(object sender, EventArgs e)
        {
            if (UsedSlot == null)
                return;
            UsedSlot.ADP = (ushort) xadp.Value;
        }

        private void xint_ValueChanged(object sender, EventArgs e)
        {
            if (UsedSlot == null)
                return;
            UsedSlot.INT = (ushort) xint.Value;
        }

        private void xfth_ValueChanged(object sender, EventArgs e)
        {
            if (UsedSlot == null)
                return;
            UsedSlot.FTH = (ushort) xfth.Value;
        }

        private void xlevel_ValueChanged(object sender, EventArgs e)
        {
            if (UsedSlot == null)
                return;
            UsedSlot.Level = (uint) xlevel.Value;
        }

        private void xsouls_ValueChanged(object sender, EventArgs e)
        {
            if (UsedSlot == null)
                return;
            UsedSlot.Souls = (uint) xsouls.Value;
        }

        private void xsoulsneeded_ValueChanged(object sender, EventArgs e)
        {
            if (UsedSlot == null)
                return;
            UsedSlot.SoulsNeeded = (uint) xsoulsneeded.Value;
        }

        private void xname_TextChanged(object sender, EventArgs e)
        {
            if (UsedSlot == null)
                return;
            UsedSlot.Name = xname.Text;
        }

        private void xclass_SelectedIndexChanged(object sender, PositionChangedEventArgs e)
        {
            if (UsedSlot == null || xclass.SelectedItem == null)
                return;
            UsedSlot.CurrentClass = (SaveSlot.Class) Enum.Parse(typeof (SaveSlot.Class), xclass.SelectedItem.Text);
            if (Legit != null)
                Legit.SetCurrent();
        }

        private void xgender_SelectedIndexChanged(object sender, PositionChangedEventArgs e)
        {
            if (UsedSlot == null)
                return;
            UsedSlot.Sex = (SaveSlot.Gender) xgender.SelectedIndex;
        }

        private void xintensity_ValueChanged(object sender, EventArgs e)
        {
            if (UsedSlot == null)
                return;
            UsedSlot.BonfireIntensity = Convert.ToUInt32(xintensity.Value);
            playthroughcounter.Text = UsedSlot.BonfireIntensity.ToString();
        }

        private void UpdateHollowLv(object sender, EventArgs e)
        {
            if (UsedSlot == null)
                return;
            UsedSlot.HollowLv = Convert.ToByte(xhollowlv.Value);
            hollowcounter.Text = UsedSlot.HollowLv == 0
                ? "Undead"
                : "Hollow Lv:" + UsedSlot.HollowLv;
        }

        private void xtorchtime_ValueChanged(object sender, EventArgs e)
        {
            if (UsedSlot == null)
                return;
            UsedSlot.TorchTime = Convert.ToSingle(xtorchtime.Value);
        }

        private void radButton13_Click(object sender, EventArgs e)
        {
            var enabled = radButton13.Text.ToLower().Contains("enabled");
            if (!enabled)
            {
                if (RadMessageBox.Show(
                    "You are about to enable legit stat calculation!\nYour level,Souls Needed and Souls Memory will be adjusted automaticly when you change your stats.\nWould you like to proceed ?",
                    "Enable Legit Stats", MessageBoxButtons.YesNo, RadMessageIcon.Exclamation) != DialogResult.Yes)
                    return;
                radButton13.Text = @"Legit Stats Calculation Enabled";
                radButton13.Image = Resources.Accept_icon;
            }
            else
            {
                radButton13.Text = @"Legit Stats Calculation Disabled!";
                radButton13.Image = Resources.Exclamation_icon;
            }
            if (Legit != null)
                Legit.Enabled = !enabled;
        }

        private void xsoulsmemory_ValueChanged(object sender, EventArgs e)
        {
            if (UsedSlot == null)
                return;
            UsedSlot.SoulsMemory = (uint) xsoulsmemory.Value;
        }

        private void xc_DropDownOpening(object sender, EventArgs e)
        {
            var rb = sender as RadDropDownButton;
            if (rb != null)
            {
                rb.Items.Clear();
                RadButtonItem rbi = new RadMenuButtonItem
                {
                    Image = GetCIcon(0),
                    ImageAlignment = ContentAlignment.MiddleCenter,
                    DisplayStyle = DisplayStyle.Image,
                    Size = new Size(24, 24),
                    Tag = rb
                };
                rbi.Click += CLock_Click;
                rb.Items.Add(rbi);

                rbi = new RadMenuButtonItem
                {
                    Image = GetCIcon(-1),
                    ImageAlignment = ContentAlignment.MiddleCenter,
                    DisplayStyle = DisplayStyle.Image,
                    Size = new Size(24, 24),
                    Tag = rb
                };
                rbi.Click += CULock_Click;
                rb.Items.Add(rbi);

                //rbi = new RadMenuButtonItem()
                //{
                //    Image = GetCIcon(1),
                //    ImageAlignment = ContentAlignment.MiddleCenter,
                //    DisplayStyle = DisplayStyle.Image,
                //    Size = new Size(16, 16),
                //    Tag = rb,
                //};
                //rbi.Click += C1_Click;
                //rb.Items.Add(rbi);
                //rbi = new RadMenuButtonItem()
                //{
                //    Image = GetCIcon(2),
                //    ImageAlignment = ContentAlignment.MiddleCenter,
                //    DisplayStyle = DisplayStyle.Image,
                //    Size = new Size(32, 16),
                //    Tag = rb,
                //};
                //rbi.Click += C2_Click;
                //rb.Items.Add(rbi);
                rbi = new RadMenuButtonItem
                {
                    Image = GetCIcon(3),
                    ImageAlignment = ContentAlignment.MiddleCenter,
                    DisplayStyle = DisplayStyle.Image,
                    Size = new Size(48, 16),
                    Tag = rb
                };
                rbi.Click += C3_Click;
                rb.Items.Add(rbi);
            }
        }

        private void CLock_Click(object sender, EventArgs e)
        {
            var bi = sender as RadMenuButtonItem;
            if (bi != null)
            {
                var ddb = bi.Tag as RadDropDownButton;
                if (ddb != null)
                {
                    var index = byte.Parse(ddb.Tag.ToString());
                    UsedSlot.SaveBlocks[0].GetStream(0x9f + index, true).WriteUInt8(0);
                    UsedSlot.SaveBlocks[0].GetStream(0xA9 + index, true).WriteUInt8(0);
                    UsedSlot.SaveBlocks[0].GetStream(0xB3 + index*2, true).WriteUInt16(0);
                    ddb.Image = CombineC(GetCDDBImage(index), GetCIcon(0));
                }
            }
        }

        private void CULock_Click(object sender, EventArgs e)
        {
            var bi = sender as RadMenuButtonItem;
            if (bi != null)
            {
                var ddb = bi.Tag as RadDropDownButton;
                if (ddb != null)
                {
                    var index = byte.Parse(ddb.Tag.ToString());
                    UsedSlot.SaveBlocks[0].GetStream(0x9f + index, true).WriteUInt8(1);
                    UsedSlot.SaveBlocks[0].GetStream(0xA9 + index, true).WriteUInt8(0);
                    UsedSlot.SaveBlocks[0].GetStream(0xB3 + index*2, true).WriteUInt16(0);
                    ddb.Image = CombineC(GetCDDBImage(index), GetCIcon(-1));
                }
            }
        }

        private void C1_Click(object sender, EventArgs e)
        {
            var bi = sender as RadMenuButtonItem;
            if (bi != null)
            {
                var ddb = bi.Tag as RadDropDownButton;
                if (ddb != null)
                {
                    var index = byte.Parse(ddb.Tag.ToString());
                    UsedSlot.SaveBlocks[0].GetStream(0x9f + index, true).WriteUInt8(1);
                    UsedSlot.SaveBlocks[0].GetStream(0xA9 + index, true).WriteUInt8(1);
                    UsedSlot.SaveBlocks[0].GetStream(0xB3 + index*2, true).WriteUInt16(0x1155*1 + 1);
                    ddb.Image = CombineC(GetCDDBImage(index), GetCIcon(1));
                }
            }
        }

        private void C2_Click(object sender, EventArgs e)
        {
            var bi = sender as RadMenuButtonItem;
            if (bi != null)
            {
                var ddb = bi.Tag as RadDropDownButton;
                if (ddb != null)
                {
                    var index = byte.Parse(ddb.Tag.ToString());
                    UsedSlot.SaveBlocks[0].GetStream(0x9f + index, true).WriteUInt8(1);
                    UsedSlot.SaveBlocks[0].GetStream(0xA9 + index, true).WriteUInt8(2);
                    UsedSlot.SaveBlocks[0].GetStream(0xB3 + index*2, true).WriteUInt16(0x1155*2 + 1);
                    ddb.Image = CombineC(GetCDDBImage(index), GetCIcon(2));
                }
            }
        }

        private void C3_Click(object sender, EventArgs e)
        {
            var bi = sender as RadButtonItem;
            if (bi != null)
            {
                var ddb = bi.Tag as RadDropDownButton;
                if (ddb != null)
                {
                    var index = byte.Parse(ddb.Tag.ToString());
                    UsedSlot.SaveBlocks[0].GetStream(0x9f + index, true).WriteUInt8(1);
                    UsedSlot.SaveBlocks[0].GetStream(0xA9 + index, true).WriteUInt8(3);
                    UsedSlot.SaveBlocks[0].GetStream(0xB4 + index*2, true).WriteUInt16(0x1155*3 + 1);
                    ddb.Image = CombineC(GetCDDBImage(index), GetCIcon(3));
                }
            }
        }

        private Image GetCIcon(int count)
        {
            if (count == 0)
                return Resources.lock_icon;
            if (count == -1)
                return Resources.lock_unlock_icon;
            if (count > 0 && count < 4)
            {
                var bm = new Bitmap(16*count, 16);
                using (var g = Graphics.FromImage(bm))
                {
                    for (var i = 0; i < count; i++)
                        g.DrawImage(Resources.star_icon, new Point(i*16, 0));
                    g.Flush();
                }
                return bm;
            }
            return Resources.star_icon;
        }

        private Image CombineC(Image a, Image b)
        {
            if (b != null)
            {
                using (var g = Graphics.FromImage(a))
                {
                    g.DrawImage(b, new Rectangle(new Point(8, a.Height - (b.Height + 4)), new Size(b.Width, b.Height)));
                    g.Flush();
                }
            }
            return a;
        }

        private Image GetCDDBImage(int index)
        {
            switch (index)
            {
                case 0:
                    return Resources.VI_0001_tpf;

                case 1:
                    return Resources.VI_0002_tpf;

                case 2:
                    return Resources.VI_0003_tpf;

                case 3:
                    return Resources.VI_0004_tpf;

                case 4:
                    return Resources.VI_0005_tpf;

                case 5:
                    return Resources.VI_0006_tpf;

                case 6:
                    return Resources.VI_0007_tpf;

                case 7:
                    return Resources.VI_0008_tpf;

                case 8:
                    return Resources.VI_0009_tpf;

                default:
                    return null;
            }
        }

        private void xg_Click(object sender, EventArgs e)
        {
            var rb = sender as RadDropDownButton;
            if (rb != null)
            {
                var controls = new List<Control>();
                controls.AddRange(xgestures.Controls.Cast<Control>());
                var loc = rb.PointToScreen(rb.Location);
                var db = new DropDownContainer(MainFile.DataBase.Gestures, controls.ToArray(), 2, rb);
                db.Width = Width;
                db.OnItemChoosed += rbi_Click;
                db.OnFocusLost += PlayerStatus_Click;
                if (xpop != null && xpop.Visible)
                    xpop.Close();
                xpop = new Popup(db);
                xpop.LostFocus += PlayerStatus_Click;
                xpop.AutoClose = false;
                xpop.FocusOnOpen = true;
                xpop.BackColor = Color.Transparent;
                xpop.ShowingAnimation = PopupAnimations.RightToLeft | PopupAnimations.Slide;
                xpop.HidingAnimation = PopupAnimations.LeftToRight | PopupAnimations.Slide;
                xpop.Show(PointToScreen(Location).X, loc.Y + 80);
            }
        }

        private void rbi_Click
            (object sender, EventArgs e)
        {
            try
            {
                PlayerStatus_Click(sender, e);
                var rb = sender as RadTileElement;
                if (rb != null)
                {
                    var vars = rb.Tag as object[];
                    if (vars != null)
                    {
                        var text = vars[0] as string;
                        if (text != null)
                        {
                            var item = vars[1] as ItemEntry;
                            RadDropDownButton xrb = null;
                            foreach (Control c in xgestures.Controls)
                            {
                                if (c is RadDropDownButton)
                                {
                                    if (c.Text == text)
                                        xrb = c as RadDropDownButton;
                                    if (item != null)
                                    {
                                        var values = (c as RadDropDownButton).Tag as object[];
                                        if (values != null && values.Length == 2)
                                        {
                                            var current = values[1] as ItemEntry;
                                            if (current != null && current.ID1 == item.ID1)
                                            {
                                                current.IsEquiped = false;
                                                var rdb = c as RadDropDownButton;
                                                var xindex = (byte) (byte.Parse(rdb.Text) - 1);
                                                rdb.Tag = new object[]
                                                {xindex.ToString(CultureInfo.InvariantCulture), null};
                                                ;
                                                rdb.Image = Resources.Recycle_Bin_Empty_icon;
                                                rdb.ImageAlignment = ContentAlignment.MiddleCenter;
                                            }
                                        }
                                    }
                                }
                            }
                            if (xrb == null)
                                return;
                            var index = (byte) (byte.Parse(text) - 1);
                            if (item != null)
                            {
                                xrb.RootElement.Tag = item;
                                xrb.DropDownButtonElement.Tag = item;
                                xrb.Image = Functions.DrawTextOnImage(item.GetImage(false), text, 12, true,
                                    new Point(37, 75));
                                var x = UsedSlot.Inventory.GetItem(item);
                                if (x == null)
                                    item = UsedSlot.Inventory.AddItem(item, 1, false);
                                else
                                    item = x;
                                item.IsEquiped = true;
                                xrb.ImageAlignment = ContentAlignment.TopCenter;
                                item.Index = index;
                                xrb.Tag = new object[] {index.ToString(CultureInfo.InvariantCulture), item};
                            }
                            else
                            {
                                var objects = xrb.Tag as object[];
                                if (objects != null)
                                    item = objects[1] as
                                        ItemEntry;
                                if (item != null)
                                    item.IsEquiped = false;
                                xrb.Tag = new object[] {index.ToString(CultureInfo.InvariantCulture), null};
                                ;
                                xrb.Image = Resources.Recycle_Bin_Empty_icon;
                                xrb.ImageAlignment = ContentAlignment.MiddleCenter;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RadMessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, RadMessageIcon.Exclamation);
            }
            finally
            {
                UpdateGestures();
            }
        }

        private void UpdateGestures()
        {
            var list = new List<ItemEntry>();
            var old = UsedSlot.Inventory.Gestures.ToList();
            using (var stream = UsedSlot.SaveBlocks[4].GetStream(0, true))
            {
                foreach (var x in xgestures.Controls)
                {
                    if (x is RadDropDownButton)
                    {
                        var v = x as RadDropDownButton;
                        var values = v.Tag as object[];
                        if (values != null && values.Length == 2)
                        {
                            var index = byte.Parse((string) values[0]);
                            var item = values[1] as ItemEntry;
                            stream.Position = 0x10058 + index*2;
                            stream.WriteUInt16((ushort) (item != null && item.EntryIndex > 0 ? index + 3840 : 0));
                            if (item != null)
                                list.Add(item);
                        }
                    }
                }
            }
            list.Reverse();
            foreach (var t in list)
            {
                for (var j = 0; j < old.Count; j++)
                {
                    if (old[j].ID1 == t.ID1)
                    {
                        old.RemoveAt(j);
                        break;
                    }
                }
            }
            list.AddRange(old.ToArray());
            uint count = 0;
            foreach (var t in list)
                t.Index = count++;
            UsedSlot.Inventory.Gestures = list.ToArray();
        }

        private void PlayerStatus_Click
            (object sender, EventArgs e)
        {
            if (xpop != null && xpop.Visible)
                xpop.Close();
        }

        private void CovenantTooltips(object sender, ToolTipTextNeededEventArgs e)
        {
            var rbi = sender as ActionButtonElement;
            if (rbi == null) return;
            e.ToolTip.ToolTipTitle = "Covenant";
            e.ToolTipText = rbi.Text;
        }

        private void Gesture_ToolTipTextNeeded(object sender, ToolTipTextNeededEventArgs e)
        {
            var element = sender as ActionButtonElement;
            if (element != null)
            {
                var rdb = element.ElementTree.Control as RadDropDownButton;
                if (rdb != null)
                {
                    var vars = rdb.Tag as object[];
                    if (vars != null && vars.Length == 2)
                    {
                        var item = vars[1] as ItemEntry;
                        if (item != null && item.EntryIndex > 0)
                        {
                            e.ToolTip.ToolTipIcon = ToolTipIcon.Info;
                            e.ToolTip.ToolTipTitle = item.Name;
                            e.ToolTipText = item.Description;
                        }
                    }
                }
            }
        }

        private void radColorBox1_ValueChanged(object sender, EventArgs e)
        {
        }

        private void radListView1_SelectedItemChanged(object sender, EventArgs e)
        {
            //    var x = sender as RadListViewElement;
            //    if (x != null && x.CurrentItem != null && x.CurrentItem.Tag != null)
            //        radColorBox1.Value = (Color)x.CurrentItem.Tag;
        }
    }
}