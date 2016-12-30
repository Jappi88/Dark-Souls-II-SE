using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using Dark_Souls_II_Save_Editor.Parameters;
using Dark_Souls_II_Save_Editor.Properties;
using HavenInterface.IOPackage;
using HavenInterface.Utils;

namespace Dark_Souls_II_Save_Editor.SaveStuff
{
    [Serializable]
    public class ItemEntry
    {
        public delegate void TypeChangedArg(ItemEntry sender, int asignedindex);

        public enum ItemElement : byte
        {
            None,
            Fire,
            Magic,
            Lightning,
            Dark,
            Poison,
            Bleed,
            Raw,
            Enchanted,
            Mundane
        }

        internal bool _canhavemore = true;
        internal bool _cor;
        internal int _entryindex;
        internal bool _ignored;
        internal uint _index;
        internal bool _isdlc;
        internal MemoryStream ImageStream;

        public ItemEntry(ItemEntry item)
        {
            item.CopyOver(this, true);
        }

        public ItemEntry(ItemTypeInstance type)
        {
            Type = type;
            Added = DateTime.Now;
        }

        internal int EquipIdBaseOffset
        {
            get
            {
                switch (Type.ItemType)
                {
                    case ItemTypeInstance.ItemTypes.Weapon:
                    case ItemTypeInstance.ItemTypes.Bow:
                    case ItemTypeInstance.ItemTypes.Shield:
                    case ItemTypeInstance.ItemTypes.Staff:
                        return 0;
                    case ItemTypeInstance.ItemTypes.Head:
                    case ItemTypeInstance.ItemTypes.Chest:
                    case ItemTypeInstance.ItemTypes.Arm:
                    case ItemTypeInstance.ItemTypes.Legs:
                        return 24;
                    case ItemTypeInstance.ItemTypes.Arrow:
                        return 48;
                    case ItemTypeInstance.ItemTypes.Ring:
                        return 64;
                    case ItemTypeInstance.ItemTypes.Item:
                        return 80;
                    case ItemTypeInstance.ItemTypes.Spell:
                        return 120;
                }
                return -1;
            }
        }

        internal int EquipIndexBaseOffset
        {
            get
            {
                switch (Type.ItemType)
                {
                    case ItemTypeInstance.ItemTypes.Weapon:
                    case ItemTypeInstance.ItemTypes.Bow:
                    case ItemTypeInstance.ItemTypes.Shield:
                    case ItemTypeInstance.ItemTypes.Staff:
                        return 0;
                    case ItemTypeInstance.ItemTypes.Head:
                    case ItemTypeInstance.ItemTypes.Chest:
                    case ItemTypeInstance.ItemTypes.Arm:
                    case ItemTypeInstance.ItemTypes.Legs:
                        return 12;
                    case ItemTypeInstance.ItemTypes.Ring:
                        return 20;
                    case ItemTypeInstance.ItemTypes.Arrow:
                        return 28;
                    case ItemTypeInstance.ItemTypes.Item:
                        return 36;
                    case ItemTypeInstance.ItemTypes.Spell:
                        return 56;
                    case ItemTypeInstance.ItemTypes.Gesture:
                        return 84;
                }
                return -1;
            }
        }

        /// <summary>
        ///     used for type
        /// </summary>
        internal int EntryIndex
        {
            get { return _entryindex; }
            set
            {
                if (value != _entryindex)
                {
                    _entryindex = value;
                    OnTypeIndexChanged?.Invoke(this, _entryindex);
                }
            }
        }

        internal Image _baseimage => ImageStream == null ? null : Image.FromStream(ImageStream);
        internal Image ItemImage => GetImage(true);

        /// <summary>
        ///     The name of the item
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     The Description of the item
        /// </summary>
        public string Description { get; set; }

        internal bool IsDlc
        {
            get { return _isdlc; }
            set
            {
                if (value != _isdlc)
                {
                    _isdlc = value;
                    OnImageChanged?.Invoke(this, 0);
                }
            }
        }

        public string DlcName => Functions.AvailibleDLCName(DlcVersion);
        internal string xDlcName { get; set; }
        public bool IsNew => Added.AddDays(7) >= DateTime.Now;

        /// <summary>
        ///     Item Simple Description
        /// </summary>
        public string SimpleDescription { get; set; }

        /// <summary>
        ///     When enabled, the tool will show a warning sign and also warns you when you try to add it, to prevent you from
        ///     screwing up
        /// </summary>
        public bool MayCauseCorruption
        {
            get { return _cor; }
            set
            {
                if (value != _cor)
                {
                    _cor = value;
                    OnImageChanged?.Invoke(this, 0);
                }
            }
        }

        /// <summary>
        ///     This value will allow you to enable/disable item quantity editing.
        ///     it will only apply to items.
        /// </summary>
        public bool CanHaveMoreThenOne
        {
            get { return _canhavemore; }
            set
            {
                if (value != _canhavemore)
                {
                    _canhavemore = value;
                    OnImageChanged?.Invoke(this, 0);
                }
            }
        }

        internal string xName { get; set; }
        internal string xDescription { get; set; }
        internal string xSimpleDescription { get; set; }

        /// <summary>
        ///     Entry index
        /// </summary>
        internal uint Index
        {
            get { return _index; }
            set
            {
                if (_index != value)
                {
                    _index = value;
                }
            }
        }

        internal uint ID1 { get; set; }
        internal uint ID2 { get; set; }

        internal ItemElement Element
        {
            get { return Data != null ? (ItemElement) Data[9] : ItemElement.None; }
            set
            {
                if (Data != null)
                {
                    Data[9] = (byte) value;
                }
            }
        }

        internal byte WeaponUpgrade
        {
            get { return (byte) (Data != null ? Data[8] : 0); }
            set
            {
                if (Data != null)
                {
                    Data[8] = value;
                }
            }
        }

        internal bool xIsInItemBox
        {
            get { return Data != null && Data[1] == 0x80; }
            set
            {
                if (Data != null) Data[1] = (byte) (value ? 0x80 : 0);
                OnBoxChanged?.Invoke(this, 0);
            }
        }

        internal bool IsInItemBox
        {
            get { return Data != null && Data[1] == 0x80; }
            set { if (Data != null) Data[1] = (byte) (value ? 0x80 : 0); }
        }

        internal ushort Quantity
        {
            get
            {
                if (Data != null)
                {
                    using (var io = new StreamIO(Data, true) {Position = 4})
                    {
                        return io.ReadUInt16();
                    }
                }
                return 0;
            }
            set
            {
                if (Data != null)
                {
                    using (var io = new StreamIO(Data, true) {Position = 4})
                    {
                        io.WriteUInt16(value);
                    }
                }
            }
        }

        internal bool IsEquiped { get; set; }
        public DateTime Added { get; internal set; }
        public int DlcVersion { get; internal set; }

        internal byte SpellUses
        {
            get
            {
                return
                    (byte)
                        (Data != null
                            ? Data[4]
                            : SpellParam != null
                                ? SpellParam.SpellUsesForEachLevel[SpellParam.SpellUsesForEachLevel.Length - 1]
                                    .SpellUses
                                : 0);
            }
            set
            {
                if (Data != null)
                {
                    Data[4] = value;
                }
            }
        }

        internal float Durability
        {
            get
            {
                if (Data != null)
                {
                    using (var io = new StreamIO(Data, true) {Position = 4})
                    {
                        return io.ReadSingle();
                    }
                }
                return 0;
            }
            set
            {
                if (Data != null)
                {
                    using (var io = new StreamIO(Data, true) {Position = 4})
                    {
                        io.WriteSingle(value);
                    }
                }
            }
        }

        internal byte[] Data { get; set; }
        internal ItemTypeInstance Type { get; set; }
        internal ItemParameter Parameter { get; set; }
        internal SpellParameter SpellParam { get; set; }
        internal WeaponEffect WeaponParam { get; set; }

        internal bool IsIgnored
        {
            get { return _ignored; }
            set
            {
                if (value != _ignored)
                {
                    _ignored = value;
                    OnImageChanged?.Invoke(this, 0);
                }
            }
        }

        internal int Offset
        {
            get
            {
                var x = 0;
                if (Type.ItemType == ItemTypeInstance.ItemTypes.Gesture ||
                    Type.ItemType == ItemTypeInstance.ItemTypes.Key)
                    x = 0xF004;
                return (int) Index*0x10 + x;
            }
        }

        internal byte BlockIndex
        {
            get
            {
                if (Type == null)
                    return 0;
                if (Type.ItemType == ItemTypeInstance.ItemTypes.Weapon ||
                    Type.ItemType == ItemTypeInstance.ItemTypes.Shield ||
                    Type.ItemType == ItemTypeInstance.ItemTypes.Bow ||
                    Type.ItemType == ItemTypeInstance.ItemTypes.Staff ||
                    Type.ItemType == ItemTypeInstance.ItemTypes.Ring)
                    return 0;
                if (Type.ItemType == ItemTypeInstance.ItemTypes.Item ||
                    Type.ItemType == ItemTypeInstance.ItemTypes.Shard ||
                    Type.ItemType == ItemTypeInstance.ItemTypes.Arrow ||
                    Type.ItemType == ItemTypeInstance.ItemTypes.Spell)
                    return 1;
                if (Type.ItemType == ItemTypeInstance.ItemTypes.Head ||
                    Type.ItemType == ItemTypeInstance.ItemTypes.Chest ||
                    Type.ItemType == ItemTypeInstance.ItemTypes.Arm ||
                    Type.ItemType == ItemTypeInstance.ItemTypes.Legs)
                    return 2;
                return 0;
            }
        }

        public event TypeChangedArg OnTypeIndexChanged;
        public event TypeChangedArg OnValueChanged;
        public event TypeChangedArg OnImageChanged;
        public event TypeChangedArg OnBoxChanged;

        public ItemEntry Clone()
        {
            var data = new List<byte>();
            if (Data != null)
                data.AddRange(Data);
            var ent = new ItemEntry(Type)
            {
                ID1 = ID1,
                ID2 = ID2,
                Data = Data != null ? data.ToArray() : null,
                Description = Description,
                Element = Element,
                EntryIndex = EntryIndex,
                Index = Index,
                ImageStream = ImageStream,
                SimpleDescription = SimpleDescription,
                Name = Name,
                Parameter = Parameter,
                SpellParam = SpellParam,
                WeaponParam = WeaponParam,
                _cor = MayCauseCorruption,
                IsInItemBox = IsInItemBox,
                _ignored = IsIgnored,
                IsEquiped = IsEquiped,
                CanHaveMoreThenOne = CanHaveMoreThenOne,
                IsDlc = IsDlc,
                Added = Added,
                DlcVersion = DlcVersion,
                xDlcName = xDlcName
            };
            ent.OnBoxChanged += OnBoxChanged;
            ent.OnImageChanged += OnImageChanged;
            ent.OnTypeIndexChanged += OnTypeIndexChanged;
            ent.OnValueChanged += OnValueChanged;
            return ent;
        }

        public void ClearEvents()
        {
            OnBoxChanged = null;
            OnImageChanged = null;
            OnTypeIndexChanged = null;
            OnValueChanged = null;
        }

        public void CopyOver(ItemEntry ent, bool includedata)
        {
            ent.ClearEvents();
            ent.ID1 = ID1;
            ent.ID2 = ID2;
            if (includedata)
            {
                if (Data != null)
                {
                    var d = Data.ToList();
                    ent.Data = d.ToArray();
                }
                ent.Index = Index;
            }
            ent.Description = Description;
            ent.Element = Element;
            ent.EntryIndex = EntryIndex;
            ent.ImageStream = ImageStream;
            ent.SimpleDescription = SimpleDescription;
            ent.Name = Name;
            ent.Type = Type;
            ent.Parameter = Parameter;
            ent.WeaponParam = WeaponParam;
            ent.SpellParam = SpellParam;
            ent._cor = MayCauseCorruption;
            ent.IsInItemBox = IsInItemBox;
            ent.IsIgnored = IsIgnored;
            ent.IsEquiped = IsEquiped;
            ent.CanHaveMoreThenOne = CanHaveMoreThenOne;
            ent.IsDlc = IsDlc;
            ent.Added = Added;
            ent.DlcVersion = DlcVersion;
            ent.xDlcName = xDlcName;
            ent.OnBoxChanged += OnBoxChanged;
            ent.OnImageChanged += OnImageChanged;
            ent.OnTypeIndexChanged += OnTypeIndexChanged;
            ent.OnValueChanged += OnValueChanged;
        }

        public void GenerateData(bool isitembox)
        {
            Data = new byte[12];
            using (var io = new StreamIO(Data, true))
            {
                io.Position = 1;
                io.WriteUInt8((byte) (isitembox ? 0x80 : 0));
                io.Position = 4;
                if (Type.ItemType == ItemTypeInstance.ItemTypes.Item ||
                    Type.ItemType == ItemTypeInstance.ItemTypes.Arrow
                    || Type.ItemType == ItemTypeInstance.ItemTypes.Shard ||
                    Type.ItemType == ItemTypeInstance.ItemTypes.Key
                    || Type.ItemType == ItemTypeInstance.ItemTypes.Gesture)
                    io.WriteUInt16(1);
                else if (Type.ItemType == ItemTypeInstance.ItemTypes.Spell)
                {
                    byte value = 10;
                    if (SpellParam != null)
                        value = SpellParam.SpellUsesForEachLevel[SpellParam.SpellUsesForEachLevel.Length - 1].SpellUses;
                    io.WriteUInt8(value);
                }
                else
                {
                    io.WriteSingle(100.00f);
                }
            }
        }

        public Image GetImage(bool drawammount)
        {
            if (Type == null)
                return _baseimage;
            var bm = new Bitmap(100, 100);
            if (_baseimage == null)
                return bm;
            var amount = "";
            if (Type.ItemType == ItemTypeInstance.ItemTypes.Item || Type.ItemType == ItemTypeInstance.ItemTypes.Arrow
                || Type.ItemType == ItemTypeInstance.ItemTypes.Shard)
                amount = Quantity.ToString();
            else if (Type.ItemType == ItemTypeInstance.ItemTypes.Spell)
                amount = SpellUses.ToString();
            using (var g = Graphics.FromImage(bm))
            {
                using (var g1 = Graphics.FromImage(_baseimage))
                {
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    g.DrawImage(_baseimage, new Rectangle(16, 0, 64, 100), new Rectangle(0, 14, 64, 100),
                        GraphicsUnit.Pixel);
                    var flag = Type.ItemType == ItemTypeInstance.ItemTypes.Item && !CanHaveMoreThenOne;
                    if (drawammount && !flag)
                    {
                        if (Type.ItemType == ItemTypeInstance.ItemTypes.Item ||
                            Type.ItemType == ItemTypeInstance.ItemTypes.Arrow
                            || Type.ItemType == ItemTypeInstance.ItemTypes.Shard ||
                            Type.ItemType == ItemTypeInstance.ItemTypes.Spell)
                        {
                            if (!Name.ToLower().Contains("gesture"))
                            {
                                var rectf = new RectangleF(37, 75, 50, 25);
                                g.DrawString(amount, new Font("Tahoma", 12, FontStyle.Bold), Brushes.WhiteSmoke, rectf);
                            }
                        }
                    }
                    if (MayCauseCorruption)
                    {
                        g.DrawImage(Resources.Exclamation_icon,
                            new Rectangle(new Point(0, _baseimage.Height - 56), new Size(32, 32)));
                    }
                    if (IsDlc)
                    {
                        var rectf = new RectangleF(65, 0, 35, 20);
                        g.DrawString("DLC", new Font("Tahoma", 10, FontStyle.Bold | FontStyle.Underline), Brushes.Gold,
                            rectf);
                    }
                    if (IsNew)
                    {
                        var rectf = new RectangleF(60, IsDlc ? 20 : 0, 40, 20);
                        g.DrawString("NEW", new Font("Tahoma", 10, FontStyle.Bold | FontStyle.Underline), Brushes.Gold,
                            rectf);
                    }
                    g.Flush();
                }
            }
            return bm;
        }

        public byte[] ToArray()
        {
            var data = new List<byte>();
            if (Data == null)
                GenerateData(IsInItemBox);
            data.AddRange(XFunctions.UInt32ToBytesArray(ID1, true));
            data.AddRange(EntryIndex == 0 ? new byte[0x0c] : Data ?? new byte[0x0c]);
            return data.ToArray();
        }

        private void Change()
        {
            OnValueChanged?.Invoke(this, EntryIndex);
        }
    }
}