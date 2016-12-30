using System.Runtime.InteropServices;
using HavenInterface.Utils;

namespace Dark_Souls_II_Save_Editor.STFSPackage
{
    public class HashEntry
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)] private readonly byte[] _CurrentBlock = new byte[3];
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)] internal byte[] _Flags;
        [MarshalAs(UnmanagedType.I8)] private byte _Level;

        public HashEntry()
        {
            _Flags = new byte[] {0, 0, 0, 0};
        }

        public HashEntry(uint xFlagIn)
        {
            Flags = xFlagIn;
        }

        public HashEntry(BlockStatusLevelZero xStatus, uint xNext)
        {
            Flags = (uint) xStatus << 30 | (xNext & 0xFFFFFF);
        }

        public byte[] BlockHash { get; set; }

        public uint CurrentBlock
        {
            get { return (uint) (_CurrentBlock[0] << 16 | _CurrentBlock[1] << 8 | _CurrentBlock[2]); }
            set
            {
                _CurrentBlock[0] = (byte) ((value >> 16) & 0xFF);
                _CurrentBlock[1] = (byte) ((value >> 8) & 0xFF);
                _CurrentBlock[2] = (byte) (value & 0xFF);
            }
        }

        public Level CurrentLevel
        {
            get { return (Level) _Level; }
            set { _Level = (byte) value; }
        }

        public byte Indicator
        {
            get { return (byte) (_Flags[0] >> 6); }
            set { _Flags[0] = (byte) ((value & 3) << 6); }
        }

        public uint Flags
        {
            get { return (uint) (_Flags[0] << 24 | _Flags[1] << 16 | _Flags[2] << 8 | _Flags[3]); }
            set { _Flags = XFunctions.UInt32ToBytesArray(value, true); }
        }

        /* Data Block Stuff */

        public BlockStatusLevelZero Status
        {
            get { return (BlockStatusLevelZero) _Flags[0]; }
            set { _Flags[0] = (byte) value; }
        }

        public uint NextBlock
        {
            get { return (uint) (_Flags[1] << 16 | _Flags[2] << 8 | _Flags[3]); }
            set { Flags = (uint) ((_Flags[0] << 24) | (int) (value & 0xFFFFFF)); }
        }

        /* Table Stuff */

        public byte Index => (byte) (Indicator & 1);

        public HashFlag AllocationFlag
        {
            get { return (HashFlag) Indicator; }
            set { Indicator = (byte) value; }
        }

        public int BlocksFree
        {
            get { return (int) ((Flags >> 15) & 0x7FFF); }
            set
            {
                // Sets unused/free blocks in each table, checks for errors
                // if blocksfree is 0xAA for Level 1 or 0x70E4 for L2, whole table can be full o shit, cause theres no used blocks :P
                if (value < 0)
                    value = 0;
                Flags = (uint) (Indicator << 30 | value << 15);
            }
        }

        public void MarkOld()
        {
            Status = BlockStatusLevelZero.Unallocated;
            NextBlock = 0xFFFFFF;
        }

        public bool Switch()
        {
            try
            {
                switch (AllocationFlag)
                {
                    case HashFlag.Unallocated:
                        Flags = (uint) ((1 << 30) | BlocksFree << 15);
                        return true;
                    case HashFlag.AllocatedFree:
                        Flags = (uint) ((2 << 30) | BlocksFree << 15);
                        return true;
                    case HashFlag.AllocatedInUseOld:
                        Flags = (uint) ((3 << 30) | BlocksFree << 15);
                        return true;
                    case HashFlag.AllocatedInUseCurrent:
                        Flags = (uint) ((2 << 30) | BlocksFree << 15);
                        return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}