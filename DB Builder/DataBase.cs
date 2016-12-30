using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Dark_Souls_II_Save_Editor.SaveStuff;

namespace Dark_Souls_II_Save_Editor.DB_Builder
{
    public class DSDataBase
    {
        public static readonly int Magic = 0x42445344;
        private readonly string _filepath;
        //public MainFile MainInstance { get; private set; }
        public bool IsBussy;

        public DSDataBase(string file, ItemTypeInstance[] instances, Progressor p, bool readlng)
        {
            _filepath = file;
            Areas = new MapEntry[] {};
            P = p;
            ItemTypes = instances;
            //MainInstance = instance;
            ReadDB(readlng, p);
        }

        public int Version { get; private set; }
        public DateTime LastUpdated { get; internal set; }
        public ItemEntry[] Items { get; set; }
        public ItemEntry[] Gestures { get; set; }
        public ItemTypeInstance[] ItemTypes { get; internal set; }
        public MapEntry[] Areas { get; internal set; }
        public static Progressor P { get; set; }
        public event ItemEntry.TypeChangedArg OnItemChanged;
        public event EventHandler DatabaseLoaded;

        public virtual void OnDatabaseLoaded()
        {
            var handler = DatabaseLoaded;
            handler?.Invoke(this, EventArgs.Empty);
        }

        private bool XRebuildDb(int version, bool usex)
        {
            IsBussy = true;
            try
            {
                P?.DoProgress("Creating new stream...", 0, 0, 1);
                using (var ms = new MemoryStream())
                {
                    using (var bw = new BinaryWriter(ms))
                    {
                        var count = 0;
                        P?.DoProgress("Writing Header...", 0, 0, 1);
                        var ents = new List<ItemEntry>();
                        ents.AddRange(Items);
                        ents.AddRange(Gestures);
                        bw.Write(Magic);
                        bw.Write(version);
                        bw.Write(ents.Count);
                        bw.Write(LastUpdated.Ticks);
                        foreach (var ent in ents)
                        {
                            if (P != null)
                            {
                                if (P.Cancel)
                                {
                                    bw.Close();
                                    P.DoProgress("Operation has been canceled!", 0, 0, 0);
                                }
                                P.DoProgress("Writing Entry : " + ent.Name, Functions.GetPercentage(count, ents.Count),
                                    count, ents.Count);
                            }
                            var name = usex ? ent.xName : ent.Name;
                            var desc = usex ? ent.xDescription : ent.Description;
                            var simple = usex ? ent.xSimpleDescription : ent.SimpleDescription;
                            bw.Write(ent.BlockIndex);
                            bw.Write(name.Length*2);
                            bw.Write(Encoding.Unicode.GetBytes(name), 0, name.Length*2);
                            bw.Write(ent.Index);
                            bw.Write(ent.EntryIndex);
                            bw.Write(ent.ID1);
                            bw.Write(ent.ID2);
                            bw.Write(ent.Added.Ticks);
                            var flag = desc != null;
                            bw.Write(flag);
                            if (flag)
                            {
                                bw.Write(desc.Length*2);
                                bw.Write(Encoding.Unicode.GetBytes(desc), 0, desc.Length*2);
                            }
                            flag = simple != null;
                            bw.Write(flag);
                            if (flag)
                            {
                                bw.Write(simple.Length*2);
                                bw.Write(Encoding.Unicode.GetBytes(simple), 0, simple.Length*2);
                            }
                            flag = ent.ImageStream != null && ent.ImageStream.CanRead && ent.ImageStream.Length > 100;
                            bw.Write(flag);
                            if (flag)
                            {
                                bw.Write((int) ent.ImageStream.Length);
                                bw.Write(ent.ImageStream.ToArray(), 0, (int) ent.ImageStream.Length);
                            }
                            bw.Write(ent.MayCauseCorruption);
                            bw.Write(ent.IsIgnored);
                            bw.Write(ent.CanHaveMoreThenOne);
                            bw.Write(ent.IsDlc);
                            bw.Write(ent.DlcVersion);
                            flag = !string.IsNullOrEmpty(ent.DlcName);
                            bw.Write(flag);
                            if (flag)
                            {
                                bw.Write(ent.DlcName.Length*2);
                                bw.Write(Encoding.Unicode.GetBytes(ent.DlcName), 0, ent.DlcName.Length*2);
                            }
                            count++;
                        }

                        bw.Write(Areas.Length);
                        count = 0;
                        foreach (var v in Areas)
                        {
                            if (P != null)
                            {
                                if (P.Cancel)
                                {
                                    bw.Close();
                                    P.DoProgress("Operation has been canceled!", 0, 0, 0);
                                }
                                P.DoProgress("Writing Area : " + v.AreaName, Functions.GetPercentage(count, ents.Count),
                                    count, ents.Count);
                            }
                            var flag = v.AreaName != null;
                            bw.Write(flag);
                            if (flag)
                            {
                                bw.Write(v.AreaName.Length*2);
                                bw.Write(Encoding.Unicode.GetBytes(v.AreaName), 0, v.AreaName.Length*2);
                            }
                            bw.Write(v.AreaId);
                            flag = v.AreaImage != null;
                            bw.Write(flag);
                            if (flag)
                            {
                                bw.Write((int) v.ImageStream.Length);
                                bw.Write(v.ImageStream.ToArray(), 0, (int) v.ImageStream.Length);
                            }
                            bw.Write(v.BonfireList.Count);
                            foreach (var x in v.BonfireList)
                            {
                                flag = x.Name != null;
                                bw.Write(flag);
                                if (flag)
                                {
                                    bw.Write(x.Name.Length*2);
                                    bw.Write(Encoding.Unicode.GetBytes(x.Name), 0, x.Name.Length*2);
                                }
                                bw.Write(x.Id);
                                bw.Write(v.AreaId);
                                flag = x.BonfireImage != null;
                                bw.Write(flag);
                                if (flag)
                                {
                                    bw.Write((int) x.ImageStream.Length);
                                    bw.Write(x.ImageStream.ToArray(), 0, (int) x.ImageStream.Length);
                                }
                            }
                            bw.Write(v.AreaIDS.Count);
                            foreach (var x in v.AreaIDS)
                                bw.Write(x);
                            count++;
                        }
                        using (Stream outs = File.Create(_filepath))
                        {
                            DoCrypto(ms, outs, true, true, P);
                        }
                    }
                }
                P?.DoProgress("Building DataBase succesfully completed!", 100, 0, 0);
                IsBussy = false;
                return true;
            }
            catch
            {
                IsBussy = false;
                return false;
            }
        }

        private static void DoCrypto(Stream instream, Stream outstream, bool encrypt, bool doevent, Progressor p)
        {
            var v = Aes.Create();
            v.IV = Functions.GenerateKey(Magic, 5564, 16);
            v.Key = Functions.GenerateKey(Magic, 57521, 16);
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
                    if (p != null)
                        p.DoProgress(
                            (encrypt ? "Finalizing DataBase..." : "Initializing DataBase...") + "(" +
                            Functions.GetPercentage(done, instream.Length) + "%)",
                            Functions.GetPercentage(done, instream.Length), 0, 1);
                done += count;
                outstream.Write(buffer, 0, count);
            }
            instream.Seek(0, SeekOrigin.Begin);
            outstream.Seek(0, SeekOrigin.Begin);
        }

        public static int DBVersion(string dbpath)
        {
            var version = 1;
            try
            {
                using (var ms = new MemoryStream())
                {
                    using (var fs = new FileStream(dbpath, FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        DoCrypto(fs, ms, false, false, P);
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

        private bool XReadDb(ref string error, bool readlng, Progressor p)
        {
            IsBussy = true;
            try
            {
                var ents = new List<ItemEntry>();
                var gestures = new List<ItemEntry>();
                LanguageDB xdb = null;
                if (readlng && MainForm.AppPreferences.Language != "English")
                {
                    var path = MainForm.Settings.ResourcePath + "\\" + MainForm.AppPreferences.Language +
                               ".lng";
                    if (!File.Exists(path))
                    {
                        MainForm.Mainpage.DownloadDatabase(MainForm.AppPreferences.Language + ".lng",
                            MainForm.Settings.ResourcePath, false);
                    }
                    xdb = new LanguageDB(path, P);
                    xdb.FileNotFound += xdb_FileNotFound;
                    xdb.Load();
                    while (xdb.IsBussy)
                        Application.DoEvents();
                }
                using (var ms = new MemoryStream())
                {
                    using (var fs = new FileStream(_filepath, FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        DoCrypto(fs, ms, false, true, p);
                    }
                    // File.WriteAllBytes(filepath + "_Decrypted.bin", ms.ToArray());
                    using (var br = new BinaryReader(ms))
                    {
                        if (br.ReadInt32() != Magic)
                            throw new Exception("Invalid DB Header!");
                        Version = br.ReadInt32();
                        if (Version < MainFile.MinimalDBversion && !_filepath.EndsWith(".lng"))
                            throw new Exception("Database has been outdated!");
                        if (!_filepath.EndsWith(".lng") && Version < MainForm.Mainpage.Dbversion &&
                            MainForm.Mainpage.Dbversion > -1)
                        {
                            MainForm.Mainpage.DownloadDatabase();
                            br.Dispose();
                            XReadDb(ref error, readlng, p);
                        }
                        else
                        {
                            var islng = _filepath.EndsWith(".lng");
                            if (!islng)
                                MainForm.Mainpage.Dbversion = Version;
                            var count = br.ReadInt32();
                            LastUpdated = new DateTime(br.ReadInt64());
                            MemoryStream image = null;
                            for (var i = 0; i < count; i++)
                            {
                                if (p != null)
                                {
                                    if (p.Cancel)
                                    {
                                        br.Close();
                                        p.DoProgress("Operation has been canceled!", 0, 0, 0);
                                    }
                                    p.DoProgress("Reading entries.. ", Functions.GetPercentage(i, count), i, count);
                                }
                                br.BaseStream.Position ++;
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
                                    Added = new DateTime(br.ReadInt64()),
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
                                flag = br.ReadBoolean();
                                if (flag)
                                {
                                    length = br.ReadInt32();
                                    image = new MemoryStream(br.ReadBytes(length));
                                    ent.ImageStream = image;
                                }
                                else
                                {
                                    if (image != null)
                                        ent.ImageStream = image;
                                }
                                ent._cor = br.ReadBoolean();
                                ent.IsIgnored = br.ReadBoolean();
                                if (!islng)
                                {
                                    ent.CanHaveMoreThenOne = br.ReadBoolean();
                                    ent.IsDlc = br.ReadBoolean();
                                }
                                ent.DlcVersion = br.ReadInt32();
                                flag = br.ReadBoolean();
                                if (flag)
                                {
                                    length = br.ReadInt32();
                                    ent.xDlcName = Encoding.Unicode.GetString(br.ReadBytes(length));
                                }
                                ent.Type = Functions.GetInstance(ItemTypes, ent.EntryIndex);
                                if (ent.Type.ItemType != ItemTypeInstance.ItemTypes.Item &&
                                    ent.Type.ItemType != ItemTypeInstance.ItemTypes.Arrow &&
                                    ent.Type.ItemType != ItemTypeInstance.ItemTypes.Shard)
                                    ent.CanHaveMoreThenOne = false;
                                ent.OnTypeIndexChanged += ResignType;
                                ent.OnImageChanged += ent_OnImageChanged;
                                ent.OnValueChanged += ent_OnImageChanged;
                                ent.OnBoxChanged += ent_OnImageChanged;
                                if (isvalid)
                                {
                                    if (ent.Type.ItemType == ItemTypeInstance.ItemTypes.Gesture)
                                        gestures.Add(ent);
                                    else
                                        ents.Add(ent);
                                }
                                if (xdb != null)
                                {
                                    var x =
                                        xdb.Items.FirstOrDefault(
                                            t => t.ID1 == ent.ID1 && !t.Name.Contains("(") && !t.Name.Contains(")"));
                                    if (x != null)
                                    {
                                        ent.Name = x.Name;
                                        ent.Description = x.Description;
                                        ent.SimpleDescription = x.SimpleDescription;
                                    }
                                }
                            }
                            count = br.ReadInt32();
                            var areas = new List<MapEntry>();
                            for (var i = 0; i < count; i++)
                            {
                                if (p != null)
                                {
                                    if (p.Cancel)
                                    {
                                        br.Close();
                                        p.DoProgress("Operation has been canceled!", 0, 0, 0);
                                    }
                                    p.DoProgress("Reading entries.. ", Functions.GetPercentage(i, count), i, count);
                                }
                                var flag = br.ReadBoolean();
                                var map = new MapEntry();
                                int length;
                                if (flag)
                                {
                                    length = br.ReadInt32();
                                    map.AreaName = Encoding.Unicode.GetString(br.ReadBytes(length));
                                }
                                map.AreaId = br.ReadUInt16();
                                flag = br.ReadBoolean();
                                if (flag)
                                {
                                    length = br.ReadInt32();
                                    map.ImageStream = new MemoryStream(br.ReadBytes(length));
                                }
                                length = br.ReadInt32();
                                for (var j = 0; j < length; j++)
                                {
                                    var bonfire = new Bonfire();
                                    flag = br.ReadBoolean();
                                    int x;
                                    if (flag)
                                    {
                                        x = br.ReadInt32();
                                        bonfire.Name = Encoding.Unicode.GetString(br.ReadBytes(x));
                                    }
                                    bonfire.Id = br.ReadUInt16();
                                    bonfire.AreaId = br.ReadUInt16();
                                    flag = br.ReadBoolean();
                                    if (flag)
                                    {
                                        x = br.ReadInt32();
                                        bonfire.ImageStream = new MemoryStream(br.ReadBytes(x));
                                    }
                                    map.BonfireList.Add(bonfire);
                                }
                                length = br.ReadInt32();
                                for (var j = 0; j < length; j++)
                                    map.AreaIDS.Add(br.ReadUInt16());
                                areas.Add(map);
                            }
                            Areas = areas.ToArray();
                            if (p != null)
                                p.DoProgress("Reading database succesfully completed!", 100, count, count);
                        }
                    }
                }
                Gestures = gestures.ToArray();
                Items = ents.ToArray();
                IsBussy = false;
                return true;
            }
            catch (Exception Exception)
            {
                IsBussy = false;
                error = Exception.Message;
                return false;
            }
        }

        private void xdb_FileNotFound(object instance, string filepath, bool read)
        {
            var db = instance as LanguageDB;
            if (db != null)
            {
                MainForm.Mainpage.DownloadDatabase(Path.GetFileName(filepath), MainForm.Settings.ResourcePath,
                    false);
                if (read)
                    db.Load();
            }
        }

        private void ent_OnImageChanged(ItemEntry sender, int asignedindex)
        {
            OnItemChanged?.Invoke(sender, asignedindex);
        }

        private void ResignType(ItemEntry ent, int newindex)
        {
            ent.Type = Functions.GetInstance(ItemTypes, newindex);
            OnItemChanged?.Invoke(ent, newindex);
        }

        public bool WriteDB(int version, bool usex)
        {
            var x = false;
            if (version == 0)
                version = 1;
            IsBussy = true;
            var t = new Thread(() => x = XRebuildDb(version, usex));
            t.Start();
            t.Join();
            IsBussy = false;
            return x;
        }

        public void ReadDB(bool readlng)
        {
            ReadDB(readlng, P);
        }

        public void ReadDB(bool readlng, Progressor p)
        {
            IsBussy = true;
            try
            {
                var x = true;
                var message = "";
                var t = new Thread(() => x = XReadDb(ref message, readlng, p));
                t.Start();
                while (t.ThreadState != ThreadState.Stopped)
                    Application.DoEvents();
                IsBussy = false;
                if (!x)
                    MainFile.DB_OnDBCorrupted(message, this);
                P?.DoCompleted(this, x, false, x ? null : new Exception(message));
            }
            catch
            {
                IsBussy = false;
                MainFile.DB_OnDBCorrupted("Corrupted Database, could not read db properly", this);
            }
        }

        public void Close()
        {
            Gestures = null;
            Items = null;
            ItemTypes = null;
        }
    }
}