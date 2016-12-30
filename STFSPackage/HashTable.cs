namespace Dark_Souls_II_Save_Editor.STFSPackage
{
    public class HashTable
    {
        public Level Level { get; set; }
        public uint TrueBlockNumber { get; set; }
        public uint EntryCount { get; set; }
        public HashEntry[] Entries { get; set; }
        public uint AddressInFile { get; set; }
    }
}