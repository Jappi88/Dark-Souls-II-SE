using System;
using System.Collections.Generic;
using System.Linq;
using Dark_Souls_II_Save_Editor.SaveStuff;
using HavenInterface.IOPackage;

namespace Dark_Souls_II_Save_Editor.Parameters
{
    public class MainSpellParam
    {
        public MainSpellParam(MainParamManager instance)
        {
            MainInstance = instance;
            var entry = instance.GameParameters.Entries.FirstOrDefault(t => t.Name == "SpellParam.param");
            if (entry == null)
                throw new Exception("Failed to retrieve Spell Parameters");
            MainIO = new StreamIO(entry.Extract(), true);
            SpellParameters = ReadParams();
        }

        public MainParamManager MainInstance { get; }
        public SpellParameter[] SpellParameters { get; private set; }
        public StreamIO MainIO { get; }

        private SpellParameter[] ReadParams()
        {
            if (MainInstance == null || MainInstance.GameParameters == null || MainIO == null)
                return null;
            var xpar = new List<SpellParameter>();
            MainIO.Position = 10;
            var count = MainIO.ReadUInt16();
            for (var i = 0; i < count; i++)
            {
                MainIO.Position = 0x40 + 0xc*i;
                xpar.Add(new SpellParameter(MainIO, MainFile.DataBase,
                    new ParameterEntry
                    {
                        ID = MainIO.ReadUInt32(),
                        Offset = MainIO.ReadInt32(),
                        ParameterFileID = MainIO.ReadUInt32()
                    }));
            }
            return xpar.ToArray();
        }

        public bool Save()
        {
            if (MainInstance == null || MainInstance.GameParameters == null || MainIO == null)
                return false;
            var entry = MainInstance.GameParameters.Entries.FirstOrDefault(t => t.Name == "SpellParam.param");
            if (entry == null)
                return false;
            return entry.Replace(MainIO.ReadAllBytes());
        }
    }
}