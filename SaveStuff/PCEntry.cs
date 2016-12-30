using System.IO;
using System.Text;

namespace Dark_Souls_II_Save_Editor.SaveStuff
{
    public class PCEntry
    {
        public PCEntry(Stream io, int index)
        {
            var br = new BinaryReader(io);
            io.Position = 0x40 + index*0x20;
            ID = br.ReadInt32();
            Flag = br.ReadInt32();
            DataLength = br.ReadInt32();
            io.Position += 4;
            DataOffset = br.ReadInt32();
            NameOffset = br.ReadInt32();
            io.Position = NameOffset;
            var count = 0;
            while (br.ReadInt16() != 0)
                count += 2;
            br.BaseStream.Position -= count + 2;
            Name = Encoding.Unicode.GetString(br.ReadBytes(count));
            Index = index;
        }

        public PCEntry(PCEntry entry)
        {
            ID = entry.ID;
            Flag = entry.Flag;
            DataLength = entry.DataLength;
            DataOffset = entry.DataOffset;
            NameOffset = entry.NameOffset;
            Index = entry.Index;
            Name = entry.Name;
        }

        public int ID { get; set; }
        public int Flag { get; set; }
        public int DataLength { get; internal set; }
        public int DataOffset { get; internal set; }
        public int NameOffset { get; internal set; }
        public int Index { get; internal set; }
        public string Name { get; internal set; }
    }
}