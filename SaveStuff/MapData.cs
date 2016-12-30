using System.Collections.Generic;
using HavenInterface.IOPackage;

namespace Dark_Souls_II_Save_Editor.SaveStuff
{
    public class MapData : MainBlock
    {
        public MapData(byte[] file, SaveSlot slot)
        {
            Parse(new StreamIO(file, true));
            SlotInstance = slot;
            var block = GetBlock(8, 0x6B);
            if (block != null)
            {
                var bons = new List<Bonfire>();
                using (var io = block.GetStream(0x1598, true))
                {
                    while (io.PeekUInt16() > 0)
                    {
                        bons.Add(new Bonfire(io.ReadUInt16()));
                    }
                    io.Position = 0x1798;
                    foreach (var b in bons)
                    {
                        b.IsUnlocked = io.ReadInt8() != 0;
                    }
                    AvailibleBonfires = bons.ToArray();
                }
            }
        }

        public SaveSlot SlotInstance { get; }
        public string FileName => GetName(10);
        public Bonfire[] AvailibleBonfires { get; }

        public ushort CurrentAreaId
        {
            get
            {
                var block = GetBlock(8, 0x6B);
                if (block == null)
                    return ushort.MaxValue;
                return block.GetStream(block.BlockLength - 0x30, true).ReadUInt16();
            }
            set
            {
                var block = GetBlock(8, 0x6B);
                if (block != null)
                    block.GetStream(block.BlockLength - 0x30, true).WriteUInt16(value);
            }
        }

        public ushort CurrentBonfireId
        {
            get
            {
                var block = GetBlock(8, 0x6B);
                if (block == null)
                    return ushort.MaxValue;
                return block.GetStream(block.BlockLength - 0x2A, true).ReadUInt16();
            }
            set
            {
                var block = GetBlock(8, 0x6B);
                if (block != null)
                    block.GetStream(block.BlockLength - 0x2A, true).WriteUInt16(value);
            }
        }

        public string GetName(byte aditional)
        {
            var nr = (byte) SlotInstance.SlotName.GetIdFromName();
            nr += aditional;
            //var name = new string(SlotInstance.SlotName.Where(c => !Char.IsDigit(c)).ToArray());
            var name = "";
            var degitreplaced = false;
            foreach (var c in SlotInstance.SlotName)
            {
                if (char.IsDigit(c) && !degitreplaced)
                {
                    name += nr.ToString("X2");
                    degitreplaced = true;
                }
                if (!char.IsDigit(c))
                    name += c;
            }
            return name;
        }

        public override byte[] Rebuild(MainFile.ConsoleType type)
        {
            var block = GetBlock(8, 0x6B);
            if (block != null)
            {
                var bons = new List<Bonfire>();
                using (var io = block.GetStream(0x1798, true))
                {
                    foreach (var b in AvailibleBonfires)
                    {
                        io.WriteUInt8((byte) (b.IsUnlocked ? 1 : 0));
                    }
                }
            }
            return base.Rebuild(type);
        }

        public int AvailibleMapCount()
        {
            if (SaveBlocks == null || SaveBlocks.Length == 0)
                return 0;
            var count = 0;
            using (var io = SaveBlocks[0].GetStream(0, true))
            {
                var rounds = SaveBlocks[0].BlockLength/0xB10;
                for (var i = 0; i < rounds; i++)
                {
                    io.Position = i*0xB10;
                    if (io.PeekInt32() != -1)
                        count++;
                }
            }
            return count;
        }

        public MapEntry[] AvailibleAreas()
        {
            if (SaveBlocks == null || SaveBlocks.Length == 0)
                return new MapEntry[] {};
            var maps = new List<MapEntry>();
            using (var io = SaveBlocks[0].GetStream(0, true))
            {
                var rounds = SaveBlocks[0].BlockLength/0xB10;
                for (var i = 0; i < rounds; i++)
                {
                    io.Position = i*0xB10;
                    if (io.PeekInt32() != -1)
                        maps.Add(new MapEntry(io.ReadBytes(0xB10)) {Index = i});
                }
            }
            return maps.ToArray();
        }
    }
}