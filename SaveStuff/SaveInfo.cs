using System;
using System.Collections.Generic;
using HavenInterface.IOPackage;

namespace Dark_Souls_II_Save_Editor.SaveStuff
{
    public class SaveSlotInfo : MainBlock
    {
        public delegate void SaveInfoArg(SaveSlotInfo info, int changedslotindex);

        public SaveSlotInfo(MainFile maininstance, StreamIO io)
        {
            Parse(io);
            MainInstance = maininstance;
            StreamIO blockstream = null;
            var b = GetBlock(4, 0x6d);
            if (b == null)
                return;
            blockstream = b.GetStream(0x10, maininstance.IsBigEndian);

            var slots = new List<int>();
            if (blockstream != null)
            {
                for (var i = 0; i < 10; i++)
                {
                    blockstream.Position = 0x1dc + i*0x1f0;
                    if (blockstream.ReadUInt32() > 0)
                        slots.Add(i + 1);
                }
                blockstream.Close();
            }
            UsedSlots = slots.ToArray();
        }

        public MainFile MainInstance { get; internal set; }
        public int[] UsedSlots { get; internal set; }
        public event SaveInfoArg OnSaveSlotInfoChanged;

        public void ReplaceSlotInfo(byte[] data, int slotindex)
        {
            if (SaveBlocks == null || SaveBlocks.Length == 0)
                return;
            var b = GetBlock(4, 0x6d);
            if (b == null)
                return;
            var x = b.GetStream(0x10 + slotindex*0x1f0, MainInstance.IsBigEndian);
            x.WriteBytes(data, false);
            OnSaveSlotInfoChanged?.Invoke(this, slotindex);
        }

        public byte[] ExtractSlotInfo(int slotindex)
        {
            if (slotindex > UsedSlots.Length || SaveBlocks == null || SaveBlocks.Length != 3)
                return null;
            var b = GetBlock(4, 0x6d);
            if (b == null)
                return null;
            return b.GetStream(0x10 + slotindex*0x1f0, MainInstance.IsBigEndian).ReadBytes(0x1f0, false);
        }

        public string GetTimePlayed(int index)
        {
            var b = GetBlock(4, 0x6d);
            if (b == null)
                return "0:0:0";
            var x = b.GetStream(0x1dc + index*0x1f0, MainInstance.IsBigEndian);
            var seconds = x.ReadInt32();
            var t = TimeSpan.FromSeconds(seconds);
            return string.Format("{0}:{1}:{2}", t.Hours, t.Minutes, t.Seconds);
        }

        public void Close()
        {
            if (SaveBlocks != null)
                foreach (var v in SaveBlocks)
                    v.BlockData = null;
            SaveBlocks = null;
        }
    }
}