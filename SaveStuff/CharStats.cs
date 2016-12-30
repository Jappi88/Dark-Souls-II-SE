namespace Dark_Souls_II_Save_Editor.SaveStuff
{
    public class CharStats
    {
        public SaveSlot.Class Class { get; internal set; }
        public uint Level { get; set; }
        public ushort VGR { get; set; }
        public ushort END { get; set; }
        public ushort VIT { get; set; }
        public ushort ATN { get; set; }
        public ushort STR { get; set; }
        public ushort DEX { get; set; }
        public ushort ADP { get; set; }
        public ushort INT { get; set; }
        public ushort FTH { get; set; }
        public uint SoulsM { get; set; }
    }
}