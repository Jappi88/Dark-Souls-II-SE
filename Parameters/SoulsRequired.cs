using System.Collections.Generic;
using System.Linq;
using Dark_Souls_II_Save_Editor.BND4;
using HavenInterface.IOPackage;

namespace Dark_Souls_II_Save_Editor.Parameters
{
    public class SoulsRequired
    {
        public SoulsRequired(MainParamManager manager)
        {
            var ent =
                manager.GameParameters.Entries.FirstOrDefault(t => t.Name.ToLower() == "playerlevelupsoulsparam.param");
            if (ent != null)
            {
                Values = new Dictionary<ushort, int>();
                using (var io = new StreamIO(ent.Extract(), true))
                {
                    io.Position = 10;
                    var count = io.ReadUInt16();
                    io.Position = 0x30;
                    var offset = io.ReadInt32();
                    io.Position = offset;
                    for (var i = 0; i < count; i++)
                    {
                        var level = io.ReadUInt16();
                        io.Position += 6;
                        var souls = io.ReadInt32();
                        Values.Add(level, souls);
                    }
                }
            }
        }

        public Dictionary<ushort, int> Values { get; internal set; }
        public BndEntry Entry { get; internal set; }
    }
}