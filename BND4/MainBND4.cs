using System.Collections.Generic;
using Dark_Souls_II_Save_Editor.SaveStuff;
using HavenInterface.IOPackage;

namespace Dark_Souls_II_Save_Editor.BND4
{
    public class MainBnd4
    {
        public readonly byte[] Magic = {0x42, 0x4E, 0x44, 0x34};

        public MainBnd4()
        {
        }

        public MainBnd4(StreamIO stream)
        {
            stream.Position = 0;
            if (MainFile.ProgressInstance != null)
                MainFile.ProgressInstance.DoProgress("Reading Game parameters..", 0, 0, 1);
            if (!stream.ReadBytes(4, false).CompareWith(Magic))
            {
                IsValid = false;
                return;
            }
            MainIO = stream;
            MainIO.Position += 8;
            var count = MainIO.ReadInt32();
            var offset = MainIO.ReadInt64();
            MainIO.Position = offset;
            var ents = new List<BndEntry>();
            for (var i = 0; i < count; i++)
            {
                MainIO.Position = offset + i*0x18;
                var ent = new BndEntry(this);
                ent.OnProgress += Doprogress;
                if (MainFile.ProgressInstance != null)
                    MainFile.ProgressInstance.DoProgress("Reading Game Parameters : " + ent.Name,
                        Functions.GetPercentage(i, count), i, count);
                ents.Add(ent);
            }
            Entries = ents.ToArray();
            IsValid = true;
        }

        public StreamIO MainIO { get; }
        public BndEntry[] Entries { get; set; }
        public bool IsValid { get; private set; }

        public void Doprogress(string message, int percent, long current, long max)
        {
            OnProgress?.Invoke(message, percent, current, max);
        }

        public byte[] ToByteArray()
        {
            return MainIO.ReadAllBytes(false);
        }

        public event ProgressHandler OnProgress;
    }
}