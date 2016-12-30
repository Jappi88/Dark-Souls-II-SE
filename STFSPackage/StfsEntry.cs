namespace Dark_Souls_II_Save_Editor.STFSPackage
{
    public class StfsEntry
    {
        public StfsEntry(StfsPackage instance)
        {
            Instance = instance;
        }

        public StfsEntry(StfsEntry ent)
        {
            Instance = ent.Instance;
            EntryIndex = ent.EntryIndex;
            Name = ent.Name;
            NameLen = ent.NameLen;
            Flags = ent.Flags;
            BlocksForFile = ent.BlocksForFile;
            StartingBlockNum = ent.StartingBlockNum;
            PathIndicator = ent.PathIndicator;
            FileSize = ent.FileSize;
            CreatedTimeStamp = ent.CreatedTimeStamp;
            AccessTimeStamp = ent.AccessTimeStamp;
            FileEntryAddress = ent.FileEntryAddress;
        }

        public int EntryIndex { get; set; }
        public string Name { get; set; }
        public byte NameLen { get; set; }
        public byte Flags { get; set; }
        public int BlocksForFile { get; set; }
        public int StartingBlockNum { get; set; }
        public ushort PathIndicator { get; set; }
        public uint FileSize { get; set; }
        public uint CreatedTimeStamp { get; set; }
        public uint AccessTimeStamp { get; set; }
        public uint FileEntryAddress { get; set; }
        internal StfsPackage Instance { get; private set; }
        public bool IsDirectory => (Flags & 2) != 0;

        public void CopyTo(StfsEntry ent)
        {
            ent.Instance = Instance;
            ent.EntryIndex = EntryIndex;
            ent.Name = Name;
            ent.NameLen = NameLen;
            ent.Flags = Flags;
            ent.BlocksForFile = BlocksForFile;
            ent.StartingBlockNum = StartingBlockNum;
            ent.PathIndicator = PathIndicator;
            ent.FileSize = FileSize;
            ent.CreatedTimeStamp = CreatedTimeStamp;
            ent.AccessTimeStamp = AccessTimeStamp;
            ent.FileEntryAddress = FileEntryAddress;
        }
    }
}