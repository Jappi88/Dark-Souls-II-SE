using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Dark_Souls_II_Save_Editor.SaveStuff;

namespace Dark_Souls_II_Save_Editor.DB_Builder
{
    public class LanguageDB
    {
        public delegate void FileNotFoundHandler(object instance, string filepath, bool read);

        public static int Magic = 0x4C4E4746;
        private BackgroundWorker _bw;

        public LanguageDB(string file, Progressor p)
        {
            FilePath = file;
            P = p;
        }

        public int Version { get; private set; }
        public string FilePath { get; }
        public ItemEntry[] Items { get; set; }
        public MapEntry[] Areas { get; set; }
        public bool IsBussy => _bw != null && _bw.IsBusy;
        public static Progressor P { get; set; }
        public event FileNotFoundHandler FileNotFound;

        protected virtual void OnFileNotFound(string filepath, bool read)
        {
            FileNotFound?.Invoke(this, filepath, read);
        }

        public void Load()
        {
            _bw = new BackgroundWorker();
            _bw.DoWork += (x, y) => ReadDb();
            _bw.RunWorkerCompleted += (x, y) => { P?.DoCompleted(this, y.Result, y.Cancelled, y.Error); };
            _bw.RunWorkerAsync();
        }

        public void Save(int version)
        {
            _bw = new BackgroundWorker();
            _bw.DoWork += (x, y) => { y.Result = XRebuildDb(version, Items, Areas); };
            _bw.RunWorkerCompleted += (x, y) => { P?.DoCompleted(this, y.Result, y.Cancelled, y.Error); };
            _bw.RunWorkerAsync();
        }

        public static int DbVersion(string dbpath)
        {
            var version = 1;
            try
            {
                using (var ms = new MemoryStream())
                {
                    using (var fs = new FileStream(dbpath, FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        DoCrypto(fs, ms, false, false);
                    }
                    using (var br = new BinaryReader(ms))
                    {
                        if (br.ReadInt32() != Magic)
                            throw new Exception("Invalid DB Header!");
                        version = br.ReadInt32();
                    }
                }
            }
            catch
            {
                return -1;
            }
            return version;
        }

        public void CopyToLng(ItemEntry[] items, MapEntry[] maps, int version)
        {
            var succes = false;
            var thread = new Thread(() => succes = XRebuildDb(version, items, maps));
            thread.Start();
            thread.Join();
            if (P != null) P.DoCompleted(this, this, !succes, new Exception("Failed to save Language DB!"));
        }

        private void ReadDb()
        {
            var ents = new List<ItemEntry>();
            var xareas = new List<MapEntry>();
            if (!File.Exists(FilePath))
                OnFileNotFound(FilePath, true);
            else
            {
                using (var ms = new MemoryStream())
                {
                    using (var fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        DoCrypto(fs, ms, false, true);
                    }
                    using (var br = new BinaryReader(ms))
                    {
                        if (br.ReadInt32() != Magic)
                            throw new Exception("Invalid DB Header!");
                        Version = br.ReadInt32();
                        MainForm.Mainpage.LNGVersion = Version;
                        var count = br.ReadInt32();
                        for (var i = 0; i < count; i++)
                        {
                            if (P != null)
                            {
                                if (P.Cancel)
                                {
                                    br.Close();
                                    P.DoProgress("Operation has been canceled!", 0, 0, 0);
                                    break;
                                }
                                P.DoProgress("Reading entries.. ", Functions.GetPercentage(i, count), i, count);
                            }
                            var length = br.ReadInt32();
                            var isvalid = length > 3;
                            var name = Encoding.Unicode.GetString(br.ReadBytes(length));
                            if (isvalid)
                                isvalid = name.ToLower() != "no item" && name.ToLower() != "no spell" &&
                                          name.ToLower() != "no text" && name.ToLower() != "none text" &&
                                          name.Length >= 4;
                            var ent = new ItemEntry(new ItemTypeInstance())
                            {
                                Index = br.ReadUInt32(),
                                EntryIndex = br.ReadInt32(),
                                ID1 = br.ReadUInt32(),
                                ID2 = br.ReadUInt32(),
                                Name = name,
                                xName = name
                            };
                            var flag = br.ReadBoolean();
                            if (flag)
                            {
                                length = br.ReadInt32();
                                ent.xDescription = Encoding.Unicode.GetString(br.ReadBytes(length));
                                ent.Description = ent.xDescription;
                            }
                            flag = br.ReadBoolean();
                            if (flag)
                            {
                                length = br.ReadInt32();
                                ent.xSimpleDescription = Encoding.Unicode.GetString(br.ReadBytes(length));
                                ent.SimpleDescription = ent.xSimpleDescription;
                            }
                            //count = br.ReadInt32();
                            //for (int j = 0; j < count; j++)
                            //{
                            //    flag = br.ReadBoolean();
                            //    if (flag)
                            //    {
                            //        var map = new MapEntry();
                            //        int x = br.ReadInt32();
                            //        map.AreaName = Encoding.Unicode.GetString(br.ReadBytes(x));
                            //        int xcount = br.ReadInt32();
                            //        for (int y = 0; y < xcount; y++)
                            //        {
                            //            flag = br.ReadBoolean();
                            //            if (flag)
                            //            {
                            //                var b = new Bonfire();
                            //                x = br.ReadInt32();
                            //                b.Name = Encoding.Unicode.GetString(br.ReadBytes(x));
                            //                map.BonfireList.Add(b);
                            //            }
                            //        }
                            //        xareas.Add(map);
                            //    }
                            //}
                            if (isvalid)
                                ents.Add(ent);
                        }
                        if (P != null)
                        {
                            P.DoProgress("Reading database succesfully completed!", 100, count, count);
                            P.DoCompleted(this, ents, P.Cancel, null);
                        }
                    }
                }
            }
            Areas = xareas.ToArray();
            Items = ents.ToArray();
        }

        private bool XRebuildDb(int version, ItemEntry[] items, MapEntry[] maps)
        {
            try
            {
                if (P != null)
                    P.DoProgress("Creating new stream...", 0, 0, 1);
                using (var ms = new MemoryStream())
                {
                    using (var bw = new BinaryWriter(ms))
                    {
                        var count = 0;
                        if (P != null)
                            P.DoProgress("Writing Header...", 0, 0, 1);
                        bw.Write(Magic);
                        bw.Write(version);
                        bw.Write(items.Length);
                        foreach (var ent in items)
                        {
                            if (P != null)
                            {
                                if (P.Cancel)
                                {
                                    bw.Close();
                                    P.DoProgress("Operation has been canceled!", 0, 0, 0);
                                }
                                P.DoProgress("Writing Entry : " + ent.Name, Functions.GetPercentage(count, items.Length),
                                    count, items.Length);
                            }
                            bw.Write(ent.Name.Length*2);
                            bw.Write(Encoding.Unicode.GetBytes(ent.Name), 0, ent.Name.Length*2);
                            bw.Write(ent.Index);
                            bw.Write(ent.EntryIndex);
                            bw.Write(ent.ID1);
                            bw.Write(ent.ID2);
                            var flag = ent.Description != null;
                            bw.Write(flag);
                            if (flag)
                            {
                                bw.Write(ent.Description.Length*2);
                                bw.Write(Encoding.Unicode.GetBytes(ent.Description), 0, ent.Description.Length*2);
                            }
                            flag = ent.SimpleDescription != null;
                            bw.Write(flag);
                            if (flag)
                            {
                                bw.Write(ent.SimpleDescription.Length*2);
                                bw.Write(Encoding.Unicode.GetBytes(ent.SimpleDescription), 0,
                                    ent.SimpleDescription.Length*2);
                            }
                            count++;
                        }
                        bw.Write(maps.Length);
                        foreach (var v in maps)
                        {
                            var flag = !string.IsNullOrEmpty(v.AreaName);
                            bw.Write(flag);
                            if (flag)
                            {
                                bw.Write(v.AreaName.Length*2);
                                bw.Write(Encoding.Unicode.GetBytes(v.AreaName), 0, v.AreaName.Length*2);
                                bw.Write(v.BonfireList.Count);
                                foreach (var x in v.BonfireList)
                                {
                                    flag = !string.IsNullOrEmpty(x.Name);
                                    bw.Write(flag);
                                    if (flag)
                                    {
                                        bw.Write(x.Name.Length*2);
                                        bw.Write(Encoding.Unicode.GetBytes(x.Name), 0, x.Name.Length*2);
                                    }
                                }
                            }
                        }
                        using (Stream outs = File.Create(FilePath))
                        {
                            DoCrypto(ms, outs, true, true);
                        }
                    }
                }
                P?.DoProgress("Building DataBase succesfully completed!", 100, 0, 0);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static void DoCrypto(Stream instream, Stream outstream, bool encrypt, bool doevent)
        {
            var v = Aes.Create();
            v.IV = Functions.GenerateKey(0x4C4E4746, 9843, 16);
            v.Key = Functions.GenerateKey(0x4C4E4746, 64755, 16);
            v.Padding = PaddingMode.Zeros;
            var c = encrypt ? v.CreateEncryptor() : v.CreateDecryptor();
            var done = 0;
            var count = 0;
            instream.Seek(0, SeekOrigin.Begin);
            outstream.Seek(0, SeekOrigin.Begin);
            var x = new CryptoStream(instream, c, CryptoStreamMode.Read);
            var buffer = new byte[2048];
            while ((count = x.Read(buffer, 0, 2048)) > 0)
            {
                if (doevent)
                {
                    P?.DoProgress(
                        (encrypt ? "Finalizing DataBase..." : "Initializing DataBase...") + "(" +
                        Functions.GetPercentage(done, instream.Length) + "%)",
                        Functions.GetPercentage(done, instream.Length), 0, 1);
                }
                done += count;
                outstream.Write(buffer, 0, count);
            }
            instream.Seek(0, SeekOrigin.Begin);
            outstream.Seek(0, SeekOrigin.Begin);
        }
    }
}