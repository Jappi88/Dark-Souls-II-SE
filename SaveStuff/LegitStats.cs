using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Dark_Souls_II_Save_Editor.Controls;
using Dark_Souls_II_Save_Editor.Properties;

namespace Dark_Souls_II_Save_Editor.SaveStuff
{
    public class LegitStats
    {
        internal SaveSlot _slot;
        internal PlayerStatus _status;
        internal bool isenabled;

        public LegitStats(SaveSlot slot, PlayerStatus status)
        {
            _slot = slot;
            _status = status;
            slot.OnValueChanged -= slot_OnValueChanged;
            slot.OnValueChanged += slot_OnValueChanged;
        }

        public bool Enabled
        {
            get { return isenabled; }
            set { enable(value); }
        }

        public CharStats DefaultStats { get; private set; }

        public int SoulsNeeded
        {
            get
            {
                if (_slot == null)
                    return 0;
                return
                    _slot.MainInstance.ParamManager.SoulsRequiredPerLevel.Values.Where(t => t.Key == _slot.Level)
                        .FirstOrDefault()
                        .Value;
            }
        }

        public void slot_OnValueChanged(SaveSlot slot)
        {
            if (Enabled)
            {
                if (DefaultStats == null)
                    return;
                if (_slot._ADP > 99)
                    _slot._ADP = 99;
                if (_slot._ATN > 99)
                    _slot._ATN = 99;
                if (_slot._END > 99)
                    _slot._END = 99;
                if (_slot._FTH > 99)
                    _slot._FTH = 99;
                if (_slot._INT > 99)
                    _slot._INT = 99;
                if (_slot._STR > 99)
                    _slot._STR = 99;
                if (_slot._VGR > 99)
                    _slot._VGR = 99;
                if (_slot._VIT > 99)
                    _slot._VIT = 99;
                if (_slot._DEX > 99)
                    _slot._DEX = 99;

                if (_slot._ADP < DefaultStats.ADP)
                    _slot._ADP = DefaultStats.ADP;
                if (_slot._ATN < DefaultStats.ATN)
                    _slot._ATN = DefaultStats.ATN;
                if (_slot._END < DefaultStats.END)
                    _slot._END = DefaultStats.END;
                if (_slot._FTH < DefaultStats.FTH)
                    _slot._FTH = DefaultStats.FTH;
                if (_slot._INT < DefaultStats.INT)
                    _slot._INT = DefaultStats.INT;
                if (_slot._STR < DefaultStats.STR)
                    _slot._STR = DefaultStats.STR;
                if (_slot._VGR < DefaultStats.VGR)
                    _slot._VGR = DefaultStats.VGR;
                if (_slot._VIT < DefaultStats.VIT)
                    _slot._VIT = DefaultStats.VIT;
                if (_slot._DEX < DefaultStats.DEX)
                    _slot._DEX = DefaultStats.DEX;

                var shouldbe = 0;
                shouldbe += _slot._ADP - DefaultStats.ADP;
                shouldbe += _slot._ATN - DefaultStats.ATN;
                shouldbe += _slot._END - DefaultStats.END;
                shouldbe += _slot._FTH - DefaultStats.FTH;
                shouldbe += _slot._INT - DefaultStats.INT;
                shouldbe += _slot._STR - DefaultStats.STR;
                shouldbe += _slot._VGR - DefaultStats.VGR;
                shouldbe += _slot._VIT - DefaultStats.VIT;
                shouldbe += _slot._DEX - DefaultStats.DEX;
                _status.xlevel.Minimum = DefaultStats.Level;
                if (shouldbe == 0)
                    SetDefault();
                else
                    _status.xlevel.Value = DefaultStats.Level + shouldbe;
                _status.xsoulsneeded.Value = SoulsNeeded;
                var value = TotalSoulsUsed(_slot._Level) + _slot.Souls;
                if (value >= uint.MaxValue)
                    value = uint.MaxValue;
                _status.xsoulsmemory.Value = value;
                if (_status.InvokeRequired)
                    _status.Invoke(new MethodInvoker(() => _status.Update()));
                else
                    _status.Update();
            }
        }

        public uint TotalSoulsUsed(uint level)
        {
            if (DefaultStats == null)
                return _slot._SoulsMemory;
            uint souls = 0;
            var val = 0;
            foreach (var v in _slot.MainInstance.ParamManager.SoulsRequiredPerLevel.Values)
            {
                if (v.Key == level - 1)
                    val += v.Value;
                if (v.Key > DefaultStats.Level)
                    souls += (uint) val;
                if (v.Key == level)
                    break;
            }
            return souls;
        }

        public void SetDefault()
        {
            if (DefaultStats != null)
            {
                _status.xlevel.Value = DefaultStats.Level;
                _status.xvgr.Value = DefaultStats.VGR;
                _status.xstr.Value = DefaultStats.STR;
                _status.xvit.Value = DefaultStats.VIT;
                _status.xadp.Value = DefaultStats.ADP;
                _status.xatn.Value = DefaultStats.ATN;
                _status.xdex.Value = DefaultStats.DEX;
                _status.xend.Value = DefaultStats.END;
                _status.xfth.Value = DefaultStats.FTH;
                _status.xint.Value = DefaultStats.INT;
            }
        }

        public void SetCurrent()
        {
            enable(Enabled);
        }

        private void enable(bool enabled)
        {
            isenabled = enabled;
            _status.xsoulsneeded.Enabled = !enabled;
            _status.xlevel.Enabled = !enabled;
            _status.xmaxlevel.Enabled = !enabled;
            _status.xsoulsmemory.Enabled = !enabled;
            if (enabled)
            {
                DefaultStats = GetDefaultValues().FirstOrDefault(t =>
                {
                    var name = Enum.GetName(typeof (SaveSlot.Class), t.Class);
                    var s = Enum.GetName(typeof (SaveSlot.Class), _slot.CurrentClass);
                    return s != null && name != null &&
                           string.Equals(name, s, StringComparison.CurrentCultureIgnoreCase);
                });
                if (DefaultStats == null)
                    return;
                _status.xsoulsmemory.Maximum = uint.MaxValue;
                _status.xvgr.Maximum = 99;
                _status.xstr.Maximum = 99;
                _status.xvit.Maximum = 99;
                _status.xadp.Maximum = 99;
                _status.xatn.Maximum = 99;
                _status.xdex.Maximum = 99;
                _status.xend.Maximum = 99;
                _status.xfth.Maximum = 99;
                _status.xint.Maximum = 99;

                _status.xlevel.Minimum = DefaultStats.Level;
                _status.xvgr.Minimum = DefaultStats.VGR;
                _status.xstr.Minimum = DefaultStats.STR;
                _status.xvit.Minimum = DefaultStats.VIT;
                _status.xadp.Minimum = DefaultStats.ADP;
                _status.xatn.Minimum = DefaultStats.ATN;
                _status.xdex.Minimum = DefaultStats.DEX;
                _status.xend.Minimum = DefaultStats.END;
                _status.xfth.Minimum = DefaultStats.FTH;
                _status.xint.Minimum = DefaultStats.INT;
            }
            else
            {
                DefaultStats = null;
                _status.xsoulsmemory.Maximum = uint.MaxValue;
                _status.xvgr.Maximum = 999;
                _status.xstr.Maximum = 999;
                _status.xvit.Maximum = 999;
                _status.xadp.Maximum = 999;
                _status.xatn.Maximum = 999;
                _status.xdex.Maximum = 999;
                _status.xend.Maximum = 999;
                _status.xfth.Maximum = 999;
                _status.xint.Maximum = 999;

                _status.xlevel.Minimum = 1;
                _status.xvgr.Minimum = 6;
                _status.xstr.Minimum = 6;
                _status.xvit.Minimum = 6;
                _status.xadp.Minimum = 6;
                _status.xatn.Minimum = 6;
                _status.xdex.Minimum = 6;
                _status.xend.Minimum = 6;
                _status.xfth.Minimum = 6;
                _status.xint.Minimum = 6;
            }
        }

        public void SetStats(CharStats stats)
        {
            if (stats != null)
            {
                _status.xlevel.Value = stats.Level;
                _status.xvgr.Value = stats.VGR;
                _status.xstr.Value = stats.STR;
                _status.xvit.Value = stats.VIT;
                _status.xadp.Value = stats.ADP;
                _status.xatn.Value = stats.ATN;
                _status.xdex.Value = stats.DEX;
                _status.xend.Value = stats.END;
                _status.xfth.Value = stats.FTH;
                _status.xint.Value = stats.INT;
                if (stats.SoulsM > 0)
                    _status.xsoulsmemory.Value = stats.SoulsM;
                slot_OnValueChanged(_slot);
            }
        }

        public static CharStats[] GetDefaultValues()
        {
            return GetDefaultValues(Resources.CharacterBaseStats);
        }

        public static CharStats[] GetDefaultValues(string text)
        {
            var stats = new List<CharStats>();
            using (var sr = new StringReader(text))
            {
                try
                {
                    while (sr.Peek() > -1)
                    {
                        var xclass = sr.ReadLine().Trim('\n', '[', ']');
                        var level = uint.Parse(sr.ReadLine().Split('=')[1].Replace(" ", ""));
                        var vgr = ushort.Parse(sr.ReadLine().Split('=')[1].Replace(" ", ""));
                        var end = ushort.Parse(sr.ReadLine().Split('=')[1].Replace(" ", ""));
                        var vit = ushort.Parse(sr.ReadLine().Split('=')[1].Replace(" ", ""));
                        var atn = ushort.Parse(sr.ReadLine().Split('=')[1].Replace(" ", ""));
                        var str = ushort.Parse(sr.ReadLine().Split('=')[1].Replace(" ", ""));
                        var dex = ushort.Parse(sr.ReadLine().Split('=')[1].Replace(" ", ""));
                        var adp = ushort.Parse(sr.ReadLine().Split('=')[1].Replace(" ", ""));
                        var INT = ushort.Parse(sr.ReadLine().Split('=')[1].Replace(" ", ""));
                        var fth = ushort.Parse(sr.ReadLine().Split('=')[1].Replace(" ", ""));
                        var soulmemory = uint.Parse(sr.ReadLine().Split('=')[1].Replace(" ", ""));
                        stats.Add(new CharStats
                        {
                            Class = (SaveSlot.Class) Enum.Parse(typeof (SaveSlot.Class), xclass),
                            Level = level,
                            VGR = vgr,
                            END = end,
                            VIT = vit,
                            ATN = atn,
                            STR = str,
                            DEX = dex,
                            ADP = adp,
                            INT = INT,
                            FTH = fth,
                            SoulsM = soulmemory
                        });
                    }
                }
                catch
                {
                }
                return stats.ToArray();
            }
        }
    }
}