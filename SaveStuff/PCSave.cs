using System;
using System.Collections.Generic;
using System.IO;

namespace Dark_Souls_II_Save_Editor.SaveStuff
{
    public class PCSave
    {
        public readonly uint Magic = 876891714;

        public PCSave(string filepath)
        {
            FileStream io = null;
            try
            {
                io = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.Read);
                var slots = new List<PCSlot>();
                using (var br = new BinaryReader(io))
                {
                    if (br.ReadUInt32() != Magic)
                        throw new Exception("Invalid DSII SaveGame!");
                    io.Position = 0xc;
                    var count = br.ReadInt32();
                    for (var i = 0; i < count; i++)
                    {
                        var ent = new PCEntry(io, i);
                        slots.Add(new PCSlot(this, ent));
                    }
                    Slots = slots.ToArray();
                    FilePath = filepath;
                }
                io.Dispose();
            }
            catch (Exception ex)
            {
                if (io != null)
                    io.Close();
                throw ex;
            }
        }

        public string FilePath { get; private set; }
        public PCSlot[] Slots { get; internal set; }
    }
}