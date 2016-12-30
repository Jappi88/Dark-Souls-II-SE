using System.IO;
using HavenInterface.IOPackage;

namespace Dark_Souls_II_Save_Editor.Parameters
{
    public class ItemParameter
    {
        public ItemParameter(byte[] data, ParameterEntry entry, bool isinternal, int index)
        {
            Index = index;
            using (var io = new StreamIO(data, true))
            {
                ID1 = io.ReadInt32();
                InternalIDS = new int[10];
                for (var i = 0; i < 10; i++)
                    InternalIDS[i] = io.ReadInt32();
                Parameter0 = io.ReadInt32();
                Parameter1 = io.ReadInt32();
                Parameter2 = io.ReadInt32();
                Parameter3 = io.ReadSingle();
                Parameter4 = io.ReadInt64();
                Parameter5 = io.ReadInt32();
                Parameter6 = io.ReadUInt16();
                Parameter7 = io.ReadUInt16();
                Parameter8 = io.ReadUInt16();
                Parameter9 = io.ReadUInt16();
                Parameter10 = io.ReadUInt16();
                Parameter11 = io.ReadUInt16();
                Entry = entry;
                IsInternal = isinternal;
            }
        }

        public byte[] GetItemData()
        {
            using (var v = new MemoryStream())
            {
                using (var io = new StreamIO(v, true))
                {
                    io.WriteInt32(ID1);
                    for (var i = 0; i < 10; i++)
                        io.WriteInt32(InternalIDS[i]);
                    io.WriteInt32(Parameter0);
                    io.WriteInt32(Parameter1);
                    io.WriteInt32(Parameter2);
                    io.WriteSingle(Parameter3);
                    io.WriteInt64(Parameter4);
                    io.WriteInt32(Parameter5);
                    io.WriteUInt16(Parameter6);
                    io.WriteUInt16(Parameter7);
                    io.WriteUInt16(Parameter8);
                    io.WriteUInt16(Parameter9);
                    io.WriteUInt16(Parameter10);
                    io.WriteUInt16(Parameter11);
                }
                return v.ToArray();
            }
        }

        #region Properties

        internal bool IsInternal { get; set; }
        internal int ID1 { get; set; }
        internal int Index { get; set; }

        internal int[] InternalIDS { get; set; }

        public int Parameter0 { get; set; }

        public int Parameter1 { get; set; }

        public int Parameter2 { get; set; }

        public float Parameter3 { get; set; }

        public long Parameter4 { get; set; }

        public int Parameter5 { get; set; }

        public ushort Parameter6 { get; set; }

        public ushort Parameter7 { get; set; }

        public ushort Parameter8 { get; set; }

        public ushort Parameter9 { get; set; }

        public ushort Parameter10 { get; set; }

        public ushort Parameter11 { get; set; }

        internal ParameterEntry Entry { get; set; }

        #endregion Properties
    }
}