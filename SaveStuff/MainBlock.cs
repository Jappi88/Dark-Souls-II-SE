using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using HavenInterface.IOPackage;

namespace Dark_Souls_II_Save_Editor.SaveStuff
{
    public class MainBlock
    {
        public MainBlock()
        {
            SaveBlocks = new SaveBlock[] {};
        }

        public SaveBlock[] SaveBlocks { get; internal set; }

        public void Parse(StreamIO io)
        {
            io.Position = 0;
            var blocks = new List<SaveBlock>();
            while (io.Position < io.Length - 0x10)
            {
                blocks.Add(new SaveBlock(io));
                if (ComputeLength(blocks, blocks.Count) > io.Length - 0x10)
                {
                    blocks[blocks.Count - 1].BlockLength = (int) (io.Length -
                                                                  (ComputeLength(blocks, blocks.Count - 1) + 0x14 + 0xc +
                                                                   0x10));
                    blocks[blocks.Count - 1].BlockData = new byte[blocks[blocks.Count - 1].BlockLength];
                }
            }
            SaveBlocks = blocks.ToArray();
        }

        private int ComputeLength(List<SaveBlock> blocks, int count)
        {
            if (blocks == null || blocks.Count == 0)
                return 0;
            var length = 0;
            for (var i = 0; i < count; i++)
            {
                if (i > blocks.Count)
                    break;
                length += blocks[i].BlockLength + 0x14 + 0xc;
            }
            return length;
        }

        public virtual byte[] Rebuild(MainFile.ConsoleType type)
        {
            if (SaveBlocks == null || SaveBlocks.Length == 0)
                return null;
            byte[] buffer = null;
            using (var ms = new MemoryStream())
            {
                using (var io = new StreamIO(ms, true))
                {
                    foreach (var s in SaveBlocks)
                    {
                        io.WriteInt32(s.BlockIndex);
                        io.WriteInt32(s.BlockID);
                        io.WriteInt32(s.BlockData.Length);
                        io.WriteBytes(new byte[0x14], false);
                        io.WriteBytes(s.BlockData, false);
                    }
                    if (type == MainFile.ConsoleType.XBOX360 ||
                        type == MainFile.ConsoleType.XBOX360FTP ||
                        type == MainFile.ConsoleType.Usb)
                    {
                        var hash = MD5.Create().ComputeHash(ms.ToArray());
                        ms.Position = ms.Length;
                        ms.Write(hash, 0, hash.Length);
                    }
                    buffer = ms.ToArray();
                }
            }
            return buffer;
        }

        public SaveBlock GetBlock(int index, int id)
        {
            if (SaveBlocks == null || SaveBlocks.Length == 0)
                return null;
            return SaveBlocks.FirstOrDefault(t => t.BlockIndex == index);
        }
    }
}