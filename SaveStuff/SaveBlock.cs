using System;
using HavenInterface.IOPackage;

namespace Dark_Souls_II_Save_Editor.SaveStuff
{
    public class SaveBlock
    {
        public delegate void BlockChangedArg(SaveBlock block);

        public SaveBlock(StreamIO io)
        {
            BlockIndex = io.ReadInt32();
            BlockID = io.ReadInt32();
            BlockLength = io.ReadInt32();
            io.Position += 0x14;
            BlockData = io.ReadBytes(BlockLength, false);
        }

        public int BlockIndex { get; internal set; }
        public int BlockID { get; internal set; }
        public int BlockLength { get; internal set; }
        public byte[] BlockData { get; internal set; }
        public event BlockChangedArg OnBlockChanged;

        public StreamIO GetStream(long offset, bool isbigendian)
        {
            return new StreamIO(BlockData, isbigendian) {Position = offset};
        }

        public void SetBlock(byte[] Data)
        {
            if (Data == null || Data.Length != BlockLength)
                throw new Exception("Invalid input data\nInput Data must be 0x" + BlockLength.ToString("X2") +
                                    " In Length");
            BlockData = new byte[Data.Length];
            Array.Copy(Data, 0, BlockData, 0, Data.Length);
            OnBlockChanged?.Invoke(this);
        }
    }
}