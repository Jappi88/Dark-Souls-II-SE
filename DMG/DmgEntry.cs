using System.Drawing;

namespace Dark_Souls_2_Resource_Extractor.DMG
{
    public class DmgEntry
    {
        public DmgEntry(DMGFile file, int index)
        {
            MainInstance = file;
            Index = file.MainStream.ReadInt32();
            ID1 = file.MainStream.ReadUInt32();
            ID2 = file.MainStream.ReadUInt32();
            entryindex = index;
        }

        public DmgEntry()
        {
        }

        public int entryindex { get; set; }
        public int Index { get; set; }
        public uint ID1 { get; set; }
        public uint ID2 { get; set; }
        public DMGFile MainInstance { get; private set; }
        public string Name { get; set; }
        public Bitmap Image { get; set; }
    }
}