using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Dark_Souls_II_Save_Editor.BND4
{
    public class BndEntry
    {
        public BndEntry(MainBnd4 bnd)
        {
            var io = bnd.MainIO;
            Flag = io.ReadInt64();
            Length = io.ReadInt64();
            DataOffset = io.ReadInt32();
            NameOffset = io.ReadInt32();
            MainInstance = bnd;
            Name = GetName();
        }

        public long Flag { get; private set; }
        public long Length { get; }
        public int DataOffset { get; }
        public long NameOffset { get; }
        public string Name { get; set; }
        public MainBnd4 MainInstance { get; }

        private string GetName()
        {
            var io = MainInstance.MainIO;
            io.Position = NameOffset;
            var count = 0;
            while (io.ReadInt8() != 0)
                count++;
            io.Position -= count + 1;
            return Encoding.ASCII.GetString(io.ReadBytes(count, false));
        }

        public bool ExtractToStream(Stream output)
        {
            try
            {
                var s = MainInstance.MainIO.BaseStream;
                s.Position = DataOffset;
                var fs = output;
                fs.Position = 0;
                long totalread = 0;
                var blocks = Length/2048;
                var buffer = new byte[2048];
                for (long i = 0; i < blocks; i++)
                {
                    s.Read(buffer, 0, 2048);
                    Application.DoEvents();
                    Doprogress("Extracting " + Name + "...", Functions.GetPercentage(totalread, Length), totalread,
                        Length);
                    fs.Write(buffer, 0, buffer.Length);
                    totalread += 2048;
                }
                if (Length%2048 != 0)
                {
                    buffer = new byte[(int) (Length%2048)];
                    s.Read(buffer, 0, buffer.Length);
                    Application.DoEvents();
                    Doprogress("Extracting " + Name + "...", Functions.GetPercentage(totalread, Length), totalread,
                        Length);
                    fs.Write(buffer, 0, buffer.Length);
                    totalread += buffer.Length;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public virtual bool Extract(string path)
        {
            var x = false;
            var t = new Thread(() => x = Xextract(path));
            t.Start();
            while (t.ThreadState != ThreadState.Stopped)
                Application.DoEvents();
            return x;
        }

        public byte[] Extract()
        {
            MainInstance.MainIO.Position = DataOffset;
            return MainInstance.MainIO.ReadBytes((int) Length, false);
        }

        public bool Replace(byte[] data)
        {
            if (data == null || data.Length != Length)
                return false;
            MainInstance.MainIO.Position = DataOffset;
            MainInstance.MainIO.WriteBytes(data, false);
            return true;
        }

        private bool Xextract(string path)
        {
            try
            {
                if (Length == 0)
                    return false;
                var names = Name.Split('\\');
                var current = path;
                for (var i = 0; i < names.Length - 1; i++)
                {
                    current += "\\" + names[i];
                    if (!Directory.Exists(current))
                        Directory.CreateDirectory(current);
                }
                current += "\\" + names[names.Length - 1];
                var s = MainInstance.MainIO.BaseStream;
                s.Position = DataOffset;
                var succes = false;
                using (var fs = File.Create(current))
                {
                    succes = ExtractToStream(fs);
                }
                return succes;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void Doprogress(string message, int percent, long current, long max)
        {
            OnProgress?.Invoke(message, percent, current, max);
        }

        public event ProgressHandler OnProgress;
    }
}