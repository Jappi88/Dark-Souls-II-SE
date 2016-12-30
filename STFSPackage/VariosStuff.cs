using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using HavenInterface.IOPackage;

namespace Dark_Souls_II_Save_Editor.STFSPackage
{
    public static class VariosStuff
    {
        public static DateTime FiletimEtoTimeT(uint highdt, uint lowdt)
        {
            var i64 = ((long) highdt << 32) + lowdt;
            return new DateTime((i64 - 116444736000000000)/10000000);
        }

        public static TimeT TimeTtoFiletime(DateTime time)
        {
            var ll = (ulong) time.Ticks*10000000 + 116444736000000000;
            return new TimeT
            {
                dwHighDateTime = (uint) ll,
                dwLowDateTime = (uint) ll >> 32
            };
        }

        public static byte[] ImageToByte(Bitmap img)
        {
            byte[] byteArray;
            using (var stream = new MemoryStream())
            {
                img.Save(stream, ImageFormat.Png);
                stream.Close();

                byteArray = stream.ToArray();
            }
            return byteArray;
        }

        public static ushort SwapByteOrder(this ushort value)
        {
            return (ushort) ((value & 0xFFU) << 8 | (value & 0xFF00U) >> 8);
        }

        public static uint SwapByteOrder(this uint value)
        {
            return (value & 0x000000FFU) << 24 | (value & 0x0000FF00U) << 8 |
                   (value & 0x00FF0000U) >> 8 | (value & 0xFF000000U) >> 24;
        }

        public static ulong SwapByteOrder(this ulong value)
        {
            return
                ((value & 0xff00000000000000L) >> 56) |
                ((value & 0x00ff000000000000L) >> 40) |
                ((value & 0x0000ff0000000000L) >> 24) |
                ((value & 0x000000ff00000000L) >> 8) |
                ((value & 0x00000000ff000000L) << 8) |
                ((value & 0x0000000000ff0000L) << 24) |
                ((value & 0x000000000000ff00L) << 40) |
                ((value & 0x00000000000000ffL) << 56);
        }

        public static bool CompareBytes(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
                return false;
            return !a.Where((t, i) => t != b[i]).Any();
        }

        public static byte[] SimpleScramble(this byte[] sourceArrayPiece, bool reverse)
        {
            if (sourceArrayPiece.Length%8 != 0)
                throw new Exception("Input not divisible by 8");
            var xStream = new StreamIO(sourceArrayPiece, false, true);
            for (var i = 0; i < sourceArrayPiece.Length/2; i += 8)
            {
                xStream.Position = i;
                var xPart1 = xStream.ReadBytes(8);
                xStream.Position = sourceArrayPiece.Length - i - 8;
                var xPart2 = xStream.ReadBytes(8);
                xStream.Position = i;
                xStream.WriteBytes(xPart2);
                xStream.Position = sourceArrayPiece.Length - i - 8;
                xStream.WriteBytes(xPart1);
                xStream.Flush();
            }
            xStream.Close();
            if (reverse)
                for (var i = 0; i < sourceArrayPiece.Length; i += 8)
                    Array.Reverse(sourceArrayPiece, i, 8);
            return sourceArrayPiece;
        }

        public static void ReverseGenericArray(this byte[] data, int elemSize, int len)
        {
            var temp = new byte[elemSize];
            for (var i = 0; i < len/2; i++)
            {
                Array.Copy(data, i*elemSize, temp, 0, elemSize);
                //memcpy(temp, ((char*)arr) + (i * elemSize), elemSize);
                Array.Copy(data, (len - 1)*elemSize - i*elemSize, data, i*elemSize, elemSize);
                //  memcpy(((char*)arr) + (i * elemSize), ((char*)arr) + (((len - 1) * elemSize) - (i * elemSize)),
                //       elemSize);
                Array.Copy(temp, 0, data, (len - 1)*elemSize - i*elemSize, elemSize);
                // memcpy(((char*)arr) + (((len - 1) * elemSize) - (i * elemSize)), temp, elemSize);
            }
        }

        public static byte[] ReverseGenericArray(this uint value, int elemsize, int len)
        {
            var data = BitConverter.GetBytes(value);
            data.ReverseGenericArray(elemsize, len);
            return data;
        }
    }
}