using System.Collections.Generic;
using System.Drawing;
using System.IO;
using HavenInterface.IOPackage;

namespace Dark_Souls_II_Save_Editor.SaveStuff
{
    public class MapEntry
    {
        public MapEntry(byte[] data)
        {
            BonfireList = new List<Bonfire>();
            AreaIDS = new List<ushort>();
            using (var io = new StreamIO(data, true))
            {
                AreaId = io.ReadUInt16();
                var count = io.ReadInt32();
                io.Position += 0xA;
                for (var i = 0; i < count; i++)
                    AreaIDS.Add(io.ReadUInt16());
            }
        }

        public MapEntry(string name, ushort id, byte[] image)
        {
            BonfireList = new List<Bonfire>();
            AreaIDS = new List<ushort>();
            AreaId = id;
            AreaName = name;
            ImageStream = new MemoryStream(image);
        }

        public MapEntry()
        {
            BonfireList = new List<Bonfire>();
            AreaIDS = new List<ushort>();
        }

        internal MemoryStream ImageStream { get; set; }
        public ushort AreaId { get; set; }
        public string AreaName { get; set; }
        public List<Bonfire> BonfireList { get; private set; }
        public List<ushort> AreaIDS { get; }
        public int Index { get; internal set; }
        public Image AreaImage => ImageStream == null ? null : Image.FromStream(ImageStream);
    }
}