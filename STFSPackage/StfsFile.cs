using System.IO;

namespace Dark_Souls_II_Save_Editor.STFSPackage
{
    public class StfsFile : StfsEntry
    {
        public StfsFile(StfsEntry ent) : base(ent)
        {
        }

        public void Extract(string filepath)
        {
            Instance.ExtractFile(this, filepath);
        }

        public byte[] Extract()
        {
            byte[] buffer = null;
            using (var ms = new MemoryStream())
            {
                Instance.ExtractFile(this, ms);
                buffer = ms.ToArray();
            }
            return buffer;
        }

        public void Replace(byte[] file)
        {
            using (var ms = new MemoryStream(file))
            {
                Instance.ReplaceFile(ms, this);
            }
        }
    }
}