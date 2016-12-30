using System.IO;
using HavenInterface.Compressions.Zlib;

namespace Dark_Souls_II_Save_Editor.SaveStuff
{
    public class PCSlot : PCEntry
    {
        public PCSlot(PCSave Save, PCEntry entry) : base(entry)
        {
            GameSave = Save;
        }

        public PCSave GameSave { get; }

        public byte[] Extract()
        {
            using (
                var br =
                    new BinaryReader(new FileStream(GameSave.FilePath, FileMode.Open, FileAccess.Read, FileShare.Read)))
            {
                br.BaseStream.Position = DataOffset;
                var buffer = br.ReadBytes(DataLength);
                buffer = ZlibStream.UncompressBuffer(buffer);
                br.Close();
                br.Dispose();
                return buffer;
            }
        }
    }
}