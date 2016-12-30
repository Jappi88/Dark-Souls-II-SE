using System.Drawing;

namespace Dark_Souls_2_Resource_Extractor.DMG
{
    public class ItemEntry
    {
        public int entryindex { get; set; }
        public int Index { get; internal set; }
        public uint ID1 { get; internal set; }
        public uint ID2 { get; internal set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SimpleInfo { get; set; }
        public Image Image { get; set; }
    }
}