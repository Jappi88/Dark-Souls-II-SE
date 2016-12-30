using System;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Dark_Souls_II_Save_Editor.Controls;
using HavenInterface.IOPackage;
using Telerik.WinControls;

namespace Dark_Souls_II_Save_Editor.SaveStuff
{
    [Serializable]
    public class SaveSlot : MainBlock
    {
        public delegate void SlotChangedHandler(SaveSlot slot);

        public SaveSlot(MainFile file, string slotname, int slotindex, byte[] savedata, string timeplayed,
            byte[] mapdata = null)
        {
            using (var io = new StreamIO(savedata, true))
            {
                if (savedata == null)
                    IsValid = false;
                else
                {
                    try
                    {
                        Parse(io);
                        if (SaveBlocks == null || SaveBlocks.Length == 0)
                            throw new Exception();
                        foreach (var v in SaveBlocks)
                            v.OnBlockChanged += b_OnBlockChanged;
                        SlotIndex = slotindex;
                        MainInstance = file;
                        SlotName = slotname;
                        TimePayed = timeplayed;
                        Inventory = new Inventory(this);
                        ReadSaveStats();
                        SlotEdited = false;
                        IsValid = Name.Length <= 0x10;
                        UserUI = new SelectedUser();
                        //if (IsValid && mapdata != null)
                        //    MapInfo = new MapData(mapdata, this);
                    }
                    catch
                    {
                        IsValid = false;
                    }
                }
            }
        }

        public bool OverwriteItems => !MainForm.AppPreferences.PreventDuplicates;
        public int[] AllowdDlcIndexes => new[] {0, 1, 2, 3};
        public event SlotChangedHandler OnItemsChanged;
        public event SlotChangedHandler OnValueChanged;
        public event SlotChangedHandler OnNameChanged;
        public event SaveBlock.BlockChangedArg OnBlockChanged;
        public event SlotChangedHandler OnDBLanguageChanged;

        private void b_OnBlockChanged(SaveBlock block)
        {
            if (block.BlockIndex == 0)
                ReadSaveStats();
            switch (block.BlockIndex)
            {
                case 0:
                case 1:
                    ReadSaveStats();
                    break;
                case 5:
                    break;
                case 6:
                    break;
                case 10:
                    if (Inventory != null)
                        Inventory.ReadItems();
                    else
                        Inventory = new Inventory(this);
                    break;
                case 12:
                    break;
                case 14:
                    break;
                case 13:
                    break;
            }
            OnBlockChanged?.Invoke(block);
        }

        public void Reload()
        {
            UserUI.Reload();
        }

        public SaveSlot CopyTo(SaveSlot slot)
        {
            for (var i = 0; i < SaveBlocks.Length; i++)
            {
                slot.SaveBlocks[i].SetBlock(SaveBlocks[i].BlockData);
            }

            slot.TimePayed = TimePayed;
            slot.SlotName = SlotName;
            slot.MainInstance = MainInstance;

            slot.IsValid = !(VGR == 0 || END == 0 || Name.Length > 0x10);
            return slot;
        }

        public void Save()
        {
            if (MainInstance == null)
                return;
            try
            {
                MainInstance.SaveSlot(this);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ReadSaveStats()
        {
            if (SaveBlocks == null || SaveBlocks.Length != 8)
                return;
            var io = SaveBlocks[0].GetStream(0, MainInstance.IsBigEndian);
            _VGR = io.ReadUInt16();
            _END = io.ReadUInt16();
            _VIT = io.ReadUInt16();
            _ATN = io.ReadUInt16();
            _STR = io.ReadUInt16();
            _DEX = io.ReadUInt16();
            _INT = io.ReadUInt16();
            _FTH = io.ReadUInt16();
            _ADP = io.ReadUInt16();
            _Unknown1 = io.ReadUInt16();
            _Unknown2 = io.ReadUInt16();
            _Unknown3 = io.ReadUInt16();
            _Level = io.ReadUInt32();
            _Souls = io.ReadUInt32();
            _SoulsMemory = io.ReadUInt32();
            _SoulsNeeded = io.ReadUInt32();
            _Health = io.ReadUInt32();
            io.Position = 0xc7;
            Colors = ReadColors(io, 22);
            io.Position = 0x128;
            _slotFlag = io.ReadUInt8();
            io.Position = 0x15A;
            _Sex = (Gender) io.ReadUInt8();
            _HollowLv = io.ReadUInt8();
            io.Position = 0x16C;
            Equipments = new Equipment(this, io.ReadBytes(0xB0, false));
            io = SaveBlocks[1].GetStream(0x24, MainInstance.IsBigEndian);
            var count = 0;
            while (io.ReadUInt16() != 0 && count < 0x21)
                count += 2;
            io.Position -= 2;
            io.Position -= count;
            _Name = Encoding.BigEndianUnicode.GetString(io.ReadBytes(count, false));
            io.Position = 0x64;
            var value = io.ReadUInt32();
            _CurrentClass = value == 0 ? Class.Deprived : (Class) value;
            _BonfireIntensity = io.ReadUInt32();
            _TorchTime =
                SaveBlocks[4].GetStream(SaveBlocks[4].BlockLength - 0x54, MainInstance.IsBigEndian).ReadSingle();
        }

        public ItemEntry AddItem(ItemEntry item, bool initembox, bool raiseevent, int ammount)
        {
            if (item.IsDlc && !AllowdDlcIndexes.Contains(item.DlcVersion))
                throw new Exception("Could not add " + item.Name + "\nDLC Items are only allowed for donators.");
            var temp = Inventory.GetItem(item, initembox);
            var itemexist = temp != null && temp.EntryIndex != 0;
            const int max = 0x6FC;
            var flag = item.CanHaveMoreThenOne && item.Type.ItemType == ItemTypeInstance.ItemTypes.Item;
            if (!itemexist || (!flag &&
                               item.Type.ItemType != ItemTypeInstance.ItemTypes.Arrow &&
                               item.Type.ItemType != ItemTypeInstance.ItemTypes.Shard))
            {
                if (itemexist && !OverwriteItems)
                    return null;
                if (Inventory.InventoryCount >= max)
                {
                    item.Data = null;
                    throw new Exception(item.IsInItemBox
                        ? "ItemBox"
                        : "Inventory" + " max has been reached! Could not add item : " + item.Name);
                }
                if (item.MayCauseCorruption)
                    if (
                        RadMessageBox.Show(
                            item.Name + " may case corruption if added...are you sure you want to add " + item.Name +
                            "?!", "Warning!", MessageBoxButtons.YesNo, RadMessageIcon.Exclamation) == DialogResult.No)
                        return null;
                if (item.Data == null)
                    item.GenerateData(initembox);
                else item.IsInItemBox = initembox;
                temp = Inventory.AddItem(item, ammount, OverwriteItems);
            }
            else
            {
                if (item.Data == null)
                    item.GenerateData(initembox);
                else
                    item.IsInItemBox = initembox;
                if (item.Type.ItemType == ItemTypeInstance.ItemTypes.Shard)
                {
                    if (temp.Quantity + item.Quantity <= 999)
                        temp.Quantity += item.Quantity;
                    else
                        temp.Quantity = 999;
                }
                else if (flag || item.Type.ItemType == ItemTypeInstance.ItemTypes.Arrow)
                {
                    if (temp.Quantity + item.Quantity <= 9999)
                        temp.Quantity += item.Quantity;
                    else
                        temp.Quantity = 9999;
                }
            }
            if (OnItemsChanged != null && raiseevent)
                OnItemsChanged(this);
            return temp;
        }

        internal string Covenant()
        {
            switch (SaveBlocks[0].GetStream(0, MainInstance.IsBigEndian).ReadInt32())
            {
                case 0:
                    return "(none)";

                case 1:
                    return "Heirs of the Sun";

                case 2:
                    return "Blue Sentinels";

                case 3:
                    return "Brotherhood of Blood";

                case 4:
                    return "Way of the Blue";

                case 5:
                    return "Rat King";

                case 6:
                    return "Bell Keeper";

                case 7:
                    return "Dragon Remnants";

                case 8:
                    return "Company of Champions";

                case 9:
                    return "Pilgrims of Dark";

                case 10:
                    return "null:0x0A";
            }
            return "(none)";
        }

        public Color[] ReadColors(StreamIO io, int count)
        {
            var c = new Color[count];
            for (var i = 0; i < count; i++)
                c[i] = Color.FromArgb(io.ReadUInt8(), io.ReadUInt8(), io.ReadUInt8());
            return c;
        }

        public void RemoveItem(ItemEntry item, bool all, bool raiseevent)
        {
            if (item == null)
                return;
            Inventory.DeleteItem(item, all);
            if (OnItemsChanged != null && raiseevent)
                OnItemsChanged(this);
        }

        public void WriteToUserData(byte[] data, bool modarmor)
        {
            WriteToUserData(data, SlotIndex, modarmor);
        }

        public void WriteToUserData(byte[] blockdata, int index, bool modarmor)
        {
            using (var io = new StreamIO(blockdata, true))
            {
                io.Position = index*0x1F0 + 0x70;
                io.WriteUInt8(_slotFlag);
                io.Position = index*0x1F0 + 0xB4;
                Equipments.WriteEquipment(io, modarmor);
                io.Position = index*0x1F0 + 0x1E6;
                io.WriteUInt16((ushort) CurrentClass);
                io.Position = index*0x1F0 + 0x184;
                io.WriteUInt16(VGR);
                io.WriteUInt16(END);
                io.WriteUInt16(VIT);
                io.WriteUInt16(ATN);
                io.WriteUInt16(STR);
                io.WriteUInt16(DEX);
                io.WriteUInt16(INT);
                io.WriteUInt16(FTH);
                io.WriteUInt16(ADP);
                io.WriteUInt16(Unknown1);
                io.WriteUInt16(Unknown2);
                io.WriteBytes(new byte[0x20]);
                io.Position -= 0x20;
                io.WriteBytes(Encoding.BigEndianUnicode.GetBytes(Name), false);
                io.Flush();
            }
        }

        public bool WriteSaveStats()
        {
            if (SaveBlocks == null || SaveBlocks.Length != 8)
                return false;
            try
            {
                var io = SaveBlocks[0].GetStream(0, MainInstance.IsBigEndian);
                io.WriteUInt16(VGR);
                io.WriteUInt16(END);
                io.WriteUInt16(VIT);
                io.WriteUInt16(ATN);
                io.WriteUInt16(STR);
                io.WriteUInt16(DEX);
                io.WriteUInt16(INT);
                io.WriteUInt16(FTH);
                io.WriteUInt16(ADP);
                io.WriteUInt16(Unknown1);
                io.WriteUInt16(Unknown2);
                io.WriteUInt16(Unknown3);
                io.WriteUInt32(Level);
                io.WriteUInt32(Souls);
                io.WriteUInt32(SoulsMemory);
                io.WriteUInt32(SoulsNeeded);
                io.WriteUInt32(Health);

                io.Position = 0x15A;
                io.WriteUInt8((byte) Sex);
                io.WriteUInt8(HollowLv);
                io.Position = 0x16C;

                io = SaveBlocks[1].GetStream(0x24, MainInstance.IsBigEndian);
                io.WriteBytes(new byte[0x20]);
                io.Position -= 0x20;
                io.WriteBytes(Encoding.BigEndianUnicode.GetBytes(Name), false);
                io.Position = 0x64;
                io.WriteUInt32((uint) CurrentClass);
                io.WriteUInt32(BonfireIntensity);

                //io = SaveBlocks[3].GetStream(0x1D4, MainInstance.IsBigEndian);
                //io.WriteBytes(new byte[0x20]);
                //io.Position -= 0x20;
                //io.WriteBytes(Encoding.BigEndianUnicode.GetBytes(Name), false);
                SaveBlocks[4].GetStream(SaveBlocks[4].BlockLength - 0x54, MainInstance.IsBigEndian)
                    .WriteSingle(_TorchTime);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public byte[] RebuildSave(bool writechanges)
        {
            if (SaveBlocks == null || SaveBlocks.Length != 8)
                throw new Exception("Invalid save slot, slot does not contain any valid blocks");
            DoProgress("Writing " + Name + " status..", 0, 1, 1);
            if (writechanges)
            {
                Inventory.Build();
                if (!WriteSaveStats())
                    throw new Exception("Failed to save character save stats");
            }
            return Rebuild(MainInstance.LoadedType);
        }

        internal void DoProgress(string message, int percentage, long current, long max)
        {
            if (MainInstance != null)
                if (MainFile.ProgressInstance != null)
                    MainFile.ProgressInstance.DoProgress(message, percentage, current, max);
        }

        internal void DbLanguageChanged()
        {
            //foreach (ItemEntry c in Inventory.Items)
            //{
            //    if (c == null)
            //        continue;
            //    ItemEntry temp = MainInstance.DB.Items.FirstOrDefault(t => t.ID1 == c.ID1);
            //    if (temp != null)
            //    {
            //        if (temp.Name.Contains('(') || temp.Name.Contains(')'))
            //        {
            //            c.Name = c.xName;
            //            c.Description = c.xDescription;
            //            c.SimpleDescription = c.xSimpleDescription;
            //        }
            //        else
            //        {
            //            c.Name = temp.Name;
            //            c.Description = temp.Description;
            //            c.SimpleDescription = temp.SimpleDescription;
            //        }
            //    }
            //    else
            //    {
            //        c.Name = c.xName;
            //        c.Description = c.xDescription;
            //        c.SimpleDescription = c.xSimpleDescription;
            //    }
            //}

            //foreach (ItemEntry c in Inventory.Others)
            //{
            //    if (c == null)
            //        continue;
            //    ItemEntry temp = MainInstance.DB.Gestures.FirstOrDefault(t => t.ID1 == c.ID1);
            //    if (temp == null)
            //        temp = MainInstance.DB.Items.FirstOrDefault(t => t.ID1 == c.ID1);
            //    if (temp != null)
            //    {
            //        if (temp.Name.Contains('(') || temp.Name.Contains(')'))
            //        {
            //            c.Name = c.xName;
            //            c.Description = c.xDescription;
            //            c.SimpleDescription = c.xSimpleDescription;
            //        }
            //        else
            //        {
            //            c.Name = temp.Name;
            //            c.Description = temp.Description;
            //            c.SimpleDescription = temp.SimpleDescription;
            //        }
            //    }
            //    else
            //    {
            //        c.Name = c.xName;
            //        c.Description = c.xDescription;
            //        c.SimpleDescription = c.xSimpleDescription;
            //    }
            //}
            Inventory.Reload();
            OnDBLanguageChanged?.Invoke(this);
        }

        #region Enums

        public enum Gender : byte
        {
            Male = 0,
            Female = 1
        }

        public enum Class
        {
            Deprived = 10,
            Warrior = 1,
            Knight = 2,
            Bandit = 4,
            Cleric = 6,
            Sorcerer = 7,
            Explorer = 8,
            Swordsman = 9,
            None = 0
        }

        public enum Gift
        {
            Nothing,
            Life_Ring,
            Human_Effigy,
            Healing_Wares,
            Homeward_Bone,
            Seed_of_a_Tree_of_Giants,
            Bonfire_Ascetic,
            Petrified_Something
        }

        #endregion

        #region Variables

        internal bool _edited;
        internal ushort _VGR;
        internal ushort _END;
        internal ushort _VIT;
        internal ushort _ATN;
        internal ushort _STR;
        internal ushort _DEX;
        internal ushort _ADP;
        internal ushort _INT;
        internal ushort _FTH;
        internal ushort _Unknown1;
        internal ushort _Unknown2;
        internal ushort _Unknown3;
        internal uint _Level;
        internal uint _Souls;
        internal uint _SoulsMemory;
        internal uint _SoulsNeeded;
        internal uint _Health;
        internal string _Name;
        internal Class _CurrentClass;
        internal uint _BonfireIntensity;
        internal float _TorchTime;
        internal Gift _CurrentGift;
        internal Gender _Sex;
        internal byte _HollowLv;
        internal byte _slotFlag;

        #endregion

        #region Save Slot Properties

        public bool SlotEdited
        {
            get
            {
                Func<SaveSlot, bool> lambdaGet = x => x._edited;
                return lambdaGet(this);
            }
            set
            {
                Action<SaveSlot, bool> lambdaSet = (x, val) => x._edited = val;
                lambdaSet(this, value);
                OnValueChanged?.Invoke(this);
            }
        }

        public ushort VGR
        {
            get { return _VGR; }
            set
            {
                if (_VGR != value)
                {
                    _VGR = value;
                    SlotEdited = true;
                }
            }
        }

        public ushort END
        {
            get { return _END; }
            set
            {
                if (_END != value)
                {
                    _END = value;
                    SlotEdited = true;
                }
            }
        }

        public ushort VIT
        {
            get { return _VIT; }
            set
            {
                if (_VIT != value)
                {
                    _VIT = value;
                    SlotEdited = true;
                }
            }
        }

        public ushort ATN
        {
            get { return _ATN; }
            set
            {
                if (_ATN != value)
                {
                    _ATN = value;
                    SlotEdited = true;
                }
            }
        }

        public ushort STR
        {
            get { return _STR; }
            set
            {
                if (_STR != value)
                {
                    _STR = value;
                    SlotEdited = true;
                }
            }
        }

        public ushort DEX
        {
            get { return _DEX; }
            set
            {
                if (_DEX != value)
                {
                    _DEX = value;
                    SlotEdited = true;
                }
            }
        }

        public ushort ADP
        {
            get { return _ADP; }
            set
            {
                if (_ADP != value)
                {
                    _ADP = value;
                    SlotEdited = true;
                }
            }
        }

        public ushort INT
        {
            get { return _INT; }
            set
            {
                if (_INT != value)
                {
                    _INT = value;
                    SlotEdited = true;
                }
            }
        }

        public ushort FTH
        {
            get { return _FTH; }
            set
            {
                if (_FTH != value)
                {
                    _FTH = value;
                    SlotEdited = true;
                }
            }
        }

        public ushort Unknown1
        {
            get { return _Unknown1; }
            set
            {
                if (_Unknown1 != value)
                {
                    _Unknown1 = value;
                    SlotEdited = true;
                }
            }
        }

        public ushort Unknown2
        {
            get { return _Unknown2; }
            set
            {
                if (_Unknown2 != value)
                {
                    _Unknown2 = value;
                    SlotEdited = true;
                }
            }
        }

        public ushort Unknown3
        {
            get { return _Unknown3; }
            set
            {
                if (_Unknown3 != value)
                {
                    _Unknown3 = value;
                    SlotEdited = true;
                }
            }
        }

        public uint Level
        {
            get { return _Level; }
            set
            {
                if (_Level != value)
                {
                    _Level = value;
                    SlotEdited = true;
                }
            }
        }

        public uint Souls
        {
            get { return _Souls; }
            set
            {
                if (_Souls != value)
                {
                    _Souls = value;
                    SlotEdited = true;
                }
            }
        }

        public uint SoulsMemory
        {
            get { return _SoulsMemory; }
            set
            {
                if (_SoulsMemory != value)
                {
                    _SoulsMemory = value;
                    SlotEdited = true;
                }
            }
        }

        public uint SoulsNeeded
        {
            get { return _SoulsNeeded; }
            set
            {
                if (_SoulsNeeded != value)
                {
                    _SoulsNeeded = value;
                    SlotEdited = true;
                }
            }
        }

        public uint Health
        {
            get { return _Health; }
            set
            {
                if (_Health != value)
                {
                    _Health = value;
                    SlotEdited = true;
                }
            }
        }

        public string Name
        {
            get { return _Name; }
            set
            {
                if (_Name != value)
                {
                    _Name = value;
                    OnNameChanged?.Invoke(this);
                    SlotEdited = true;
                }
            }
        }

        public Class CurrentClass
        {
            get { return _CurrentClass; }
            set
            {
                if (_CurrentClass != value)
                {
                    _CurrentClass = value;
                    SlotEdited = true;
                }
            }
        }

        public uint BonfireIntensity
        {
            get { return _BonfireIntensity; }
            set
            {
                if (_BonfireIntensity != value)
                {
                    _BonfireIntensity = value;
                    SlotEdited = true;
                }
            }
        }

        public float TorchTime
        {
            get { return _TorchTime; }
            set
            {
                if (_TorchTime != value)
                {
                    _TorchTime = value;
                    SlotEdited = true;
                }
            }
        }

        public Gift CurrentGift
        {
            get { return _CurrentGift; }
            set
            {
                if (_CurrentGift != value)
                {
                    _CurrentGift = value;
                    SlotEdited = true;
                }
            }
        }

        public Gender Sex
        {
            get { return _Sex; }
            set
            {
                if (_Sex != value)
                {
                    _Sex = value;
                    SlotEdited = true;
                }
            }
        }

        public byte HollowLv
        {
            get { return _HollowLv; }
            set
            {
                if (_HollowLv != value)
                {
                    _HollowLv = value;
                    SlotEdited = true;
                }
            }
        }

        public Point Location { get; set; }
        public Inventory Inventory { get; internal set; }
        public Equipment Equipments { get; internal set; }
        public MainFile MainInstance { get; private set; }
        public MapData MapInfo { get; private set; }
        public string SlotName { get; set; }
        public int SlotIndex { get; set; }
        public bool IsValid { get; private set; }
        public string TimePayed { get; internal set; }
        public Color[] Colors { get; private set; }

        public byte SlotFlag => _slotFlag;

        public SelectedUser UserUI { get; set; }

        #endregion
    }
}