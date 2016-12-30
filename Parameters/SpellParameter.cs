using System.Collections.Generic;
using System.Linq;
using Dark_Souls_II_Save_Editor.DB_Builder;
using Dark_Souls_II_Save_Editor.SaveStuff;
using HavenInterface.IOPackage;

namespace Dark_Souls_II_Save_Editor.Parameters
{
    public class SpellParameter
    {
        public SpellParameter(StreamIO io, DSDataBase db, ParameterEntry entry)
        {
            MainIO = io;
            ParamEntry = entry;
            DB = db;
            DBInstances = db.Items.Where(t => t.ID1 >= StartID && t.ID1 <= EndID).ToArray();
            foreach (var en in DBInstances)
                en.SpellParam = this;
        }

        public ItemEntry[] DBInstances { get; }
        public StreamIO MainIO { get; }
        public ParameterEntry ParamEntry { get; }
        public ParameterEntry Entry { get; internal set; }
        public DSDataBase DB { get; internal set; }

        public uint StartID
        {
            get
            {
                MainIO.Position = ParamEntry.Offset + 0xC;
                return MainIO.ReadUInt32();
            }
        }

        public uint EndID
        {
            get
            {
                MainIO.Position = ParamEntry.Offset + 0x10;
                return MainIO.ReadUInt32();
            }
        }

        public ChildSpell[] ChildSpells
        {
            get
            {
                var spells = new List<ChildSpell>();
                for (var i = 0; i < 3; i++)
                    spells.Add(new ChildSpell(MainIO, DB, ParamEntry, i));
                return spells.ToArray();
            }
        }

        public SpellUsesLevel[] SpellUsesForEachLevel
        {
            get
            {
                MainIO.Position = ParamEntry.Offset + 0xF1;
                var x = new List<SpellUsesLevel>();
                for (var i = 1; i <= 10; i++)
                {
                    MainIO.Position = ParamEntry.Offset + 0xF1 + (i - 1);
                    x.Add(new SpellUsesLevel(MainIO, (int) MainIO.Position, i));
                    MainIO.Position++;
                }
                return x.ToArray();
            }
        }

        public byte SlotUsed
        {
            get
            {
                MainIO.Position = ParamEntry.Offset + 0xF0;
                return MainIO.ReadUInt8();
            }
            set
            {
                MainIO.Position = ParamEntry.Offset + 0xF0;
                MainIO.WriteUInt8(value);
            }
        }

        public float Value1
        {
            get
            {
                MainIO.Position = ParamEntry.Offset + 20;
                return MainIO.ReadSingle();
            }
            set
            {
                MainIO.Position = ParamEntry.Offset + 20;
                MainIO.WriteSingle(value);
            }
        }

        public float Value2
        {
            get
            {
                MainIO.Position = ParamEntry.Offset + 24;
                return MainIO.ReadSingle();
            }
            set
            {
                MainIO.Position = ParamEntry.Offset + 24;
                MainIO.WriteSingle(value);
            }
        }
    }

    public class ChildSpell
    {
        public int index;

        public ChildSpell(StreamIO io, DSDataBase db, ParameterEntry entry, int entindex)
        {
            MainIO = io;
            ParamEntry = entry;
            index = entindex;
            var ents = new List<ItemEntry>();
            //int count = (int)(EndID - StartID);
            //for(int i = 0; i <= count; i++)
            //{
            //    ItemEntry ent = db.Items.GetEntryByID((uint)(StartID + i));
            //    if (ent != null)
            //        ents.Add(ent);
            //}
            DBInstances = ents.ToArray();
        }

        public ItemEntry[] DBInstances { get; private set; }
        public StreamIO MainIO { get; }
        public ParameterEntry ParamEntry { get; }

        public uint StartID
        {
            get
            {
                MainIO.Position = ParamEntry.Offset + 0x1c + index*0x14;
                return MainIO.ReadUInt32();
            }
        }

        public uint EndID
        {
            get
            {
                MainIO.Position = ParamEntry.Offset + 0x20 + index*0x14;
                return MainIO.ReadUInt32();
            }
        }

        public float Value1
        {
            get
            {
                MainIO.Position = ParamEntry.Offset + 0x24 + index*0x14;
                return MainIO.ReadSingle();
            }
            set
            {
                MainIO.Position = ParamEntry.Offset + 0x24 + index*0x14;
                MainIO.WriteSingle(value);
            }
        }

        public float Value2
        {
            get
            {
                MainIO.Position = ParamEntry.Offset + 0x28 + index*0x14;
                return MainIO.ReadSingle();
            }
            set
            {
                MainIO.Position = ParamEntry.Offset + 0x28 + index*0x14;
                MainIO.WriteSingle(value);
            }
        }

        public float Value3
        {
            get
            {
                MainIO.Position = ParamEntry.Offset + 0x2c + index*0x14;
                return MainIO.ReadSingle();
            }
            set
            {
                MainIO.Position = ParamEntry.Offset + 0x2c + index*0x14;
                MainIO.WriteSingle(value);
            }
        }
    }

    public class SpellUsesLevel
    {
        internal StreamIO _io;
        internal int _offset;

        public SpellUsesLevel(StreamIO io, int offset, int level)
        {
            _io = io;
            _offset = offset;
            Level = level;
        }

        public int Level { get; internal set; }

        public byte SpellUses
        {
            get
            {
                _io.Position = _offset;
                return _io.ReadUInt8();
            }
            set
            {
                if (value > 99)
                    value = 99;
                _io.Position = _offset;
                _io.WriteUInt8(value);
            }
        }
    }
}