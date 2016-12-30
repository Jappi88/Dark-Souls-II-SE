using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Dark_Souls_2_Resource_Extractor.DMG;
using Dark_Souls_II_Save_Editor.Controls;
using Dark_Souls_II_Save_Editor.DB_Builder;
using Dark_Souls_II_Save_Editor.Properties;
using Dark_Souls_II_Save_Editor.SaveStuff;
using HavenInterface.Interfaces;
using HavenInterface.Popup;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using ItemEntry = Dark_Souls_II_Save_Editor.SaveStuff.ItemEntry;
using Timer = System.Windows.Forms.Timer;

namespace Dark_Souls_II_Save_Editor
{
    public static class Functions
    {
        private static readonly object _lock = new object();

        public static void SetSelected(this RadTileElement element, RadPanorama container, Color color)
        {
            var c = Color.FromArgb(0, 122, 204);
            foreach (var v in container.Items)
                v.BackColor = v.Name == element.Name ? color : c;
        }

        public static Image DrawTextOnImage(Image source, string text, byte fontsize, bool usebold, Point location)
        {
            using (var g = Graphics.FromImage(source))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                var rectf = new RectangleF(location, new Size(100, 30));
                g.DrawString(text, new Font("Tahoma", fontsize, usebold ? FontStyle.Bold : FontStyle.Underline),
                    Brushes.White, rectf);
                g.Flush();
            }
            return source;
        }

        /// <summary>
        ///     Maps the source object to target object.
        /// </summary>
        /// <typeparam name="T">Type of target object.</typeparam>
        /// <typeparam name="TU">Type of source object.</typeparam>
        /// <param name="target">Target object.</param>
        /// <param name="source">Source object.</param>
        /// <returns>Updated target object.</returns>
        public static T Map<T, TU>(this T target, TU source)
        {
            // get property list of the target object.
            // this is a reflection extension which simply gets properties (CanWrite = true).
            var tprops = target.GetType().GetProperties();

            tprops.ToList().ForEach(prop =>
            {
                // check whether source object has the the property
                var sp = source.GetType().GetProperty(prop.Name);
                if (sp != null)
                {
                    // if yes, copy the value to the matching property
                    var value = sp.GetValue(source, null);
                    target.GetType().GetProperty(prop.Name).SetValue(target, value, null);
                }
            });

            return target;
        }

        private static Image DownloadRemoteImageFile(string uri)
        {
            var request = (HttpWebRequest) WebRequest.Create(uri);
            using (var response = (HttpWebResponse) request.GetResponse())
            {
                // Check that the remote file was found. The ContentType
                // check is performed since a request for a non-existent
                // image file might be redirected to a 404-page, which would
                // yield the StatusCode "OK", even though the image was not
                // found.
                if ((response.StatusCode == HttpStatusCode.OK ||
                     response.StatusCode == HttpStatusCode.Moved ||
                     response.StatusCode == HttpStatusCode.Redirect) &&
                    response.ContentType.StartsWith("image", StringComparison.OrdinalIgnoreCase))
                {
                    // if the remote file was found, download it
                    using (var inputStream = response.GetResponseStream())
                    {
                        if (inputStream != null)
                        {
                            Image img;
                            using (var outputStream = new MemoryStream())
                            {
                                var buffer = new byte[4096];
                                int bytesRead;
                                do
                                {
                                    bytesRead = inputStream.Read(buffer, 0, buffer.Length);
                                    outputStream.Write(buffer, 0, bytesRead);
                                } while (bytesRead != 0);
                                img = Image.FromStream(outputStream);
                            }
                            return img;
                        }
                    }
                }
            }
            return null;
        }

        private static string GetLineFromtd(string line)
        {
            var x = line.Replace("\t", "");
            if (x.StartsWith("<td"))
            {
                var index = x.IndexOf(">", 0, StringComparison.Ordinal);
                if (index > -1)
                {
                    var end = x.IndexOf("</td>", index + 1, StringComparison.Ordinal);
                    if (end > -1)
                    {
                        x = x.Substring(index + 1, end - (index + 1));
                    }
                }
            }
            return x;
        }

        private static string GetLinkFromLine(string line)
        {
            line = line.Replace("\t", "");
            var index = line.IndexOf("src=\"", StringComparison.Ordinal);
            if (index > -1)
            {
                var end = line.IndexOf("\"", index + 5, StringComparison.Ordinal);
                if (end > -1)
                {
                    var info = line.Substring(index + 5, end - (index + 5));
                    index = info.LastIndexOf('/');
                    if (index > -1)
                    {
                        return
                            "https://az545221.vo.msecnd.net/skype-faq-media/faq_content/skype/screenshots/fa12330/emoticons/" +
                            info.Substring(index, info.Length - index);
                    }
                    return info;
                }
            }
            return line;
        }

        public static ItemTypeInstance[] GetItemTypes(string db)
        {
            var instances = new List<ItemTypeInstance>();
            using (var br = new BinaryReader(new FileStream(db, FileMode.Open)))
            {
                var count = br.ReadInt32();
                for (var i = 0; i < count; i++)
                {
                    var id = br.ReadUInt32();
                    if (i > 0)
                        instances[i - 1].EndID = id;
                    var length = br.ReadInt32();
                    var instance = new ItemTypeInstance();
                    instance.StartID = id;
                    instance.Image = Image.FromStream(new MemoryStream(br.ReadBytes(length)));
                    instances.Add(instance);
                }
                instances[instances.Count - 1].EndID = 9999;
                return instances.ToArray();
            }
        }

        public static void UpdateDbWithDmg(int version, string dirpath, string imgpath)
        {
            var t = new Thread(() => XUpdateDbWithDmg(version, dirpath, imgpath));
            t.Start();
            t.Join();
            if (MainFile.DataBase != null)
                MainFile.DataBase.OnDatabaseLoaded();
        }

        private static void XUpdateDbWithDmg(int version, string dirpath, string imgpath)
        {
            var dirs = Directory.GetDirectories(dirpath, "*.*", SearchOption.TopDirectoryOnly);
            foreach (var dir in dirs)
            {
                var name = Path.GetFileName(dir);
                var dbpath = MainForm.Settings.ResourcePath + "\\" + name + ".lng";
                var files = Directory.GetFiles(dir, "*.fmg", SearchOption.TopDirectoryOnly);
                if (files.Length > 0)
                {
                    LanguageDB lng = null;
                    var Items = new List<ItemEntry>();
                    if (name == "English")
                    {
                        if (MainFile.DataBase == null)
                            MainFile.LoadDb(MainForm.Settings.ResourcePath + "\\DS2Items_English.db",
                                MainForm.Settings.ResourcePath + "\\ItemTypes.db");
                        //new DSDataBase(MainForm.AppSettings.ResourcePath + "\\DS2Items_English.db",
                        //    MainFile.ItemTypes, new Progressor(), false);
                        if (MainFile.DataBase != null)
                        {
                            foreach (var item in MainFile.DataBase.Items)
                            {
                                if (item.IsDlc)
                                    item.DlcVersion = 1;
                                Items.Add(item);
                            }
                            Items.AddRange(MainFile.DataBase.Gestures);
                        }
                    }
                    else
                    {
                        lng = new LanguageDB(dbpath, MainFile.ProgressInstance);
                        lng.Load();
                        while (lng.IsBussy)
                            Application.DoEvents();
                        Items.AddRange(lng.Items);
                    }
                    var namepath = dir + "\\ItemName.fmg";
                    var descriptionpath = dir + "\\DetailedExplanation.fmg";
                    var simplepath = dir + "\\SimpleExplanation.fmg";
                    var file1 = new DMGFile(namepath, lng == null ? imgpath : null);
                    var file2 = new DMGFile(descriptionpath, null);
                    var file3 = new DMGFile(simplepath, null);
                    Image last = null;
                    foreach (var item in file1.Entries)
                    {
                        var description = file2.Entries.FirstOrDefault(x => x.ID1 == item.ID1);
                        var sdesc = file3.Entries.FirstOrDefault(x => x.ID1 == item.ID1);
                        var newitem = new ItemEntry(new ItemTypeInstance())
                        {
                            ID1 = item.ID1,
                            ID2 = item.ID2,
                            Name = item.Name,
                            xName = item.Name,
                            IsDlc = true,
                            DlcVersion = 3
                        };
                        if (lng == null)
                        {
                            if (item.Image != null)
                            {
                                newitem.ImageStream = new MemoryStream(item.Image.ToByteArray());
                                last = item.Image;
                            }
                            else
                            {
                                //continue;
                                if (last != null)
                                    newitem.ImageStream = new MemoryStream(last.ToByteArray());
                            }
                        }
                        if (description != null)
                        {
                            newitem.Description = description.Name;
                            newitem.xDescription = description.Name;
                        }

                        if (sdesc != null)
                        {
                            newitem.SimpleDescription = sdesc.Name;
                            newitem.xSimpleDescription = sdesc.Name;
                        }
                        //if (lng == null && item.ID1 == 53300000)
                        //    Console.WriteLine("here");
                        if (Items.All(x => x.ID1 != item.ID1))
                            Items.Add(newitem);
                    }
                    if (MainFile.DataBase != null && lng == null)
                    {
                        MainFile.DataBase.Items =
                            Items.Where(x => x.Type.ItemType != ItemTypeInstance.ItemTypes.Gesture).ToArray();
                        MainFile.DataBase.Gestures =
                            Items.Where(x => x.Type.ItemType == ItemTypeInstance.ItemTypes.Gesture).ToArray();
                        //xdb.WriteDB(version + 1, false);
                    }
                    if (lng != null)
                    {
                        Console.WriteLine(lng.FilePath);
                        lng.CopyToLng(Items.ToArray(), new MapEntry[0], version + 1);
                        while (lng.IsBussy)
                            Application.DoEvents();
                    }
                }
            }
        }

        public static string AvailibleDLCName(int index)
        {
            switch (index)
            {
                case 0:
                    return "";
                case 1:
                    return "Crown of the Sunken King";
                case 2:
                    return "Crown of the Old Iron King";
                case 3:
                    return "Crown of the Ivory King";
            }
            return "";
        }

        internal static void UpdateDatabeWithAreas(string imgpath, string langdir, int version)
        {
        }

        public static decimal RoundUp(this decimal number, int places)
        {
            var factor = RoundFactor(places);
            number *= factor;
            number = Math.Ceiling(number);
            number /= factor;
            return number;
        }

        public static decimal RoundDown(this decimal number, int places)
        {
            var factor = RoundFactor(places);
            number *= factor;
            number = Math.Floor(number);
            number /= factor;
            return number;
        }

        internal static decimal RoundFactor(int places)
        {
            var factor = 1m;

            if (places < 0)
            {
                places = -places;
                for (var i = 0; i < places; i++)
                    factor /= 10m;
            }

            else
            {
                for (var i = 0; i < places; i++)
                    factor *= 10m;
            }

            return factor;
        }

        public static void UpdateDbImages(string dbpath, string imgpath, string dmgpath)
        {
            //string[] images = Directory.GetFiles(imgpath, "*.PNG", SearchOption.AllDirectories);
            var count = 0;
            if (MainFile.DataBase == null || MainFile.DataBase.Items == null || MainFile.DataBase.Items.Length == 0)
            {
                var types = MainForm.Settings.ResourcePath + "\\Itemtypes.DB";
                MainFile.LoadDb(dbpath, types);
            }
            if (MainFile.DataBase != null)
            {
                var dmg = new DMGFile(dmgpath, imgpath);
                foreach (var item in MainFile.DataBase.Items)
                {
                    //   if (item.Name == "Llewellyn Gloves")
                    //  Console.WriteLine("Here");
                    //Console.WriteLine(item.ID1.ToString().Length);
                    //string imagepath = (from s in images let name = Path.GetFileName(s) let id = name.GetIDFromName() where id == item.ID1 || id == item.ID2 || (id >= item.ID1 && id <= item.ID1) select s).FirstOrDefault();
                    var ent =
                        dmg.Entries.FirstOrDefault(
                            x => x.ID1 == item.ID1);
                    if (ent != null && ent.Image != null)
                    {
                        count++;
                        item.ImageStream = new MemoryStream(ent.Image.ToByteArray());
                    }
                }
                var list = new List<ItemEntry>();
                foreach (var item in MainFile.DataBase.Items.Where(item => list.All(x => x.ID1 != item.ID1)))
                {
                    list.Add(item);
                }
                count = MainFile.DataBase.Items.Length - list.Count;
                MainFile.DataBase.Items = list.ToArray();
                MainFile.DataBase.OnDatabaseLoaded();
            }
            RadMessageBox.Show(count + " Items have been removed");
        }

        internal static byte[] GenerateKey(int Magic, int initialvalue, int length)
        {
            long r1 = 0, r2 = 0, r3 = 0, r4 = initialvalue;
            var xb = new List<byte>();
            for (var i = 0; i < length; i++)
            {
                r1 = ((i*r2 + 88) << 2) ^ Magic;
                r2 = (r1 ^ 0x54) | r4;
                r3 = r1 + ((r2 << 2) ^ Magic);
                r1 ^= (r3 >> 4) ^ r4;
                r2 &= 0x74 ^ r1;
                r3 = r2 ^ r3 ^ r1;
                r4 ^= r3;
                xb.Add((byte) r3);
            }
            return xb.ToArray();
        }

        public static ItemEntry GetEntryById(this ItemEntry[] entries, uint id, bool isequiped)
        {
            if (id == uint.MaxValue)
                return new ItemEntry(new ItemTypeInstance {StartID = 0, EndID = 0, Image = Resources.unknown})
                {
                    Name = "Unknown Item",
                    ID1 = id,
                    EntryIndex = 0,
                    Description =
                        "Item has not been added to the database yet, inform jappi about this , and make to sure to provide your save along with it."
                };
            ItemEntry ent = null;
            for (var i = 0; i < entries.Length; i++)
            {
                if (entries[i] != null && (entries[i].ID1 == id || id == entries[i].ID2))
                {
                    ent = entries[i];
                    ent.Index = (uint) i;
                    break;
                }
            }
            if (ent == null)
                ent = new ItemEntry(new ItemTypeInstance {StartID = 0, EndID = 0, Image = Resources.unknown})
                {
                    Name = "Unknown Item",
                    ID1 = id,
                    ID2 = id,
                    EntryIndex = 0,
                    Description =
                        "Item has not been added to the database yet, inform jappi about this , and make to sure to provide your save along with it."
                };
            ent.IsEquiped = isequiped;
            return ent;
        }

        public static ItemTypeInstance GetInstance(ItemTypeInstance[] instances, int id)
        {
            if (instances.Length == 0)
                return null;
            var type = instances.FirstOrDefault(t => id >= t.StartID && id <= t.EndID);
            if (type == null)
                return new ItemTypeInstance {StartID = 0, EndID = 0, Image = Resources.unknown};
            return type;
        }

        public static TResult GetPropertyThreadSafe<TControl, TResult>(this TControl self,
            Func<TControl, TResult> getter)
            where TControl : Control
        {
            if (self.InvokeRequired)
            {
                return (TResult) self.Invoke(getter, self);
            }
            return getter(self);
        }

        public static byte[] ToByteArray(this Image imageIn)
        {
            return (byte[]) new ImageConverter().ConvertTo(imageIn, typeof (byte[]));
        }

        public static bool BackupFolder(string filepath)
        {
            var r =
                RadMessageBox.Show(
                    "Would you like to backup your save directory?\r\nIt is highly recommended to do so!", "Backup",
                    MessageBoxButtons.YesNoCancel, RadMessageIcon.Question);
            if (r == DialogResult.Yes)
            {
                var b = new FolderBrowserDialog();
                b.Description = @"Select where to backup your directory";
                if (b.ShowDialog() == DialogResult.OK)
                {
                    var count = 0;
                    var d = new DirectoryInfo(filepath);
                    var xpath = b.SelectedPath + "\\" + d.Name;
                    while (Directory.Exists(xpath))
                    {
                        xpath = b.SelectedPath + "\\" + d.Name.Replace(".bak", "") + "(" + count + ").bak";
                        count++;
                    }
                    var files = d.GetFiles();
                    d = Directory.CreateDirectory(xpath);
                    foreach (var f in files)
                    {
                        f.CopyTo(xpath + "\\" + f.Name);
                    }
                }
            }
            else if (r == DialogResult.Cancel)
                return false;
            return true;
        }

        public static bool BackupFile(string filepath)
        {
            var r =
                RadMessageBox.Show("Would you like to backup your save File?\r\nIt is highly recommended to do so!",
                    "Backup", MessageBoxButtons.YesNoCancel, RadMessageIcon.Question);
            if (r == DialogResult.Yes)
            {
                var x = new SaveFileDialog();
                x.Title = @"Save Backup";
                x.FileName = new FileInfo(filepath).Name;
                x.Filter = ".Bak|*.Bak|All Files|*.*";
                x.OverwritePrompt = false;
                if (x.ShowDialog() == DialogResult.OK)
                {
                    var count = 0;
                    var path = x.FileName;
                    while (File.Exists(path))
                    {
                        path = x.FileName.Replace(".Bak", "") + "(" + count + ").Bak";
                        count++;
                    }
                    File.Copy(filepath, path);
                }
            }
            else if (r == DialogResult.Cancel)
                return false;
            return true;
        }

        public static bool Backup(string name)
        {
            try
            {
                var dir = MainForm.Settings.BackupPath;
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                var newfile = string.Format("{0}\\{1}[{2}]", dir, Path.GetFileName(name), DateTime.Now).CleanFileName();
                if (Directory.Exists(name))
                {
                    Directory.CreateDirectory(newfile);
                    var files = Directory.GetFiles(name);
                    foreach (var f in files)
                        File.Copy(f, newfile + "\\" + Path.GetFileName(f), true);
                    return true;
                }
                if (File.Exists(name))
                {
                    File.Copy(name, newfile, true);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool CompareWith(this byte[] a, byte[] b)
        {
            if (a == null || b == null || a.Length != b.Length)
                return false;
            if (a.Where((t, i) => t != b[i]).Any())
                return false;
            return true;
        }

        public static Popup CreateProgressPopup(this Progressor p)
        {
            var x = new Progressview();
            x.Dock = DockStyle.Fill;
            p.AtProgress += x.DoProgress;
            var xprogresspopup = new Popup(x);
            xprogresspopup.AutoClose = false;
            xprogresspopup.FocusOnOpen = false;
            xprogresspopup.BackColor = Color.Transparent;
            xprogresspopup.ShowingAnimation = PopupAnimations.RightToLeft | PopupAnimations.Slide;
            xprogresspopup.HidingAnimation = PopupAnimations.LeftToRight | PopupAnimations.Slide;
            return xprogresspopup;
        }

        public static Image CombineWith(this Image image, Image secondimage)
        {
            var img3 = new Bitmap(image.Width + secondimage.Width, image.Height);
            img3.MakeTransparent();
            using (var g = Graphics.FromImage(img3))
            {
                g.DrawImage(secondimage, new Point(0, image.Height/2 - secondimage.Height/2));
                g.DrawImage(image, new Point(secondimage.Width, 0));
            }
            return img3;
        }

        public static Image GetImageFromSource(this Bitmap image, Size size)
        {
            if (image.Size == size)
                return image;
            return image.Clone(new Rectangle(new Point(image.Size.Width - size.Width, 0), size),
                PixelFormat.Format32bppArgb);
        }

        public static uint GetIdFromName(this string name)
        {
            uint value = 0;
            uint.TryParse(new string(name.Where(c => char.IsDigit(c)).ToArray()), out value);
            return value;
        }

        public static byte[] ReverseBytes(this byte[] data)
        {
            var buffer = data;
            Array.Reverse(data);
            return data;
        }

        public static ushort SwapByteOrder(this ushort value)
        {
            return (ushort) (((value & 0xff) << 8) | ((value & 0xff00) >> 8));
        }

        public static uint SwapByteOrder(this uint value)
        {
            return
                (uint)
                    (((value & 0xff) << 0x18) | ((value & 0xff00) << 8) | ((value & 0xff0000) >> 8) |
                     ((value & -16777216) >> 0x18));
        }

        public static IPAddress GetIpAddress()
        {
            return
                Dns.GetHostEntry(Dns.GetHostName())
                    .AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
        }

        public static ulong SwapByteOrder(this ulong value)
        {
            return ((value & 0xFF00000000000000) >> 0x38) | ((value & 0xff000000000000L) >> 40) |
                   ((value & 0xff0000000000L) >> 0x18) | ((value & 0xff00000000L) >> 8) |
                   ((value & 0xff000000L) << 8) | ((value & 0xff0000L) << 0x18) | ((value & 0xff00L) << 40) |
                   ((value & 0xffL) << 0x38);
        }

        public static int GetPercentage(long current, long max)
        {
            return (int) Math.Round((double) (100*current)/max);
        }

        public static string CleanFileName(this string fileName)
        {
            return Path.GetInvalidFileNameChars()
                .Aggregate(fileName, (current, c) => current.Replace(c.ToString(), "_"));
        }

        public static string CleanPathName(this string fileName)
        {
            return Path.GetInvalidPathChars().Aggregate(fileName, (current, c) => current.Replace(c.ToString(), ""));
        }

        public static void ShowMessageWithTimer(this Control control, string message, int time = 2000)
        {
            var t = new Timer();
            t.Interval = time;
            t.Tag = control;
            t.Tick += (x, y) =>
            {
                var timer = x as Timer;
                if (timer != null)
                {
                    var c = timer.Tag as Control;
                    if (c != null)
                        c.Text = "";
                    timer.Stop();
                }
            };
            control.Text = message;
            Application.DoEvents();
            t.Start();
        }

        public static Popup CreatePopup(this Control control, bool focusonopen, PopupAnimations show,
            PopupAnimations hide, int duration, bool autoclose = true)
        {
            var x = new Popup(control);
            x.FocusOnOpen = focusonopen;
            x.ShowingAnimation = show;
            x.HidingAnimation = hide;
            x.AnimationDuration = duration;
            x.AutoClose = autoclose;
            //x.BackColor = System.Drawing.Color.Transparent;
            x.BackColor = Color.FromArgb(45, 45, 48);
            return x;
        }

        public static Popup CreatePopup(this Control control, bool focusonopen, bool autoclose = true)
        {
            return CreatePopup(control, focusonopen, PopupAnimations.RightToLeft | PopupAnimations.Roll,
                PopupAnimations.Blend, 200, autoclose);
        }

        public static Preferences LoadSettings(string path)
        {
            var x = new Preferences();
            try
            {
                if (File.Exists(path))
                    x = Serializer.Deserialize<Preferences>(File.ReadAllText(path).Decrypt("GlU8e45M;#_j1$"));
            }
            catch
            {
            }

            //if (string.IsNullOrWhiteSpace(x.ResourcePath))
            //    x.ResourcePath = AppDomain.CurrentDomain.BaseDirectory + "\\Resources";
            //if (!Directory.Exists(x.ResourcePath))
            //    Directory.CreateDirectory(x.ResourcePath);

            //if (string.IsNullOrWhiteSpace(x.BackupPath))
            //    x.BackupPath = AppDomain.CurrentDomain.BaseDirectory + "\\Backups";
            //if (!Directory.Exists(x.BackupPath))
            //    Directory.CreateDirectory(x.BackupPath);
            return x;
        }

        public static void SaveSettings(string path, Preferences settings)
        {
            lock (_lock)
            {
                settings.SettingsAdjusted = true;
                File.WriteAllText(path, Serializer.Serialize(settings).Encrypt("GlU8e45M;#_j1$"), Encoding.UTF8);
            }
        }

        public static void SaveSettings()
        {
            new Thread(() => SaveSettings(MainForm.SettingPath, MainForm.AppPreferences)).Start();
        }

        public static byte[] StringToByteArray(this string hex)
        {
            if (hex.Length%2 != 0) hex = hex.PadLeft(hex.Length + 1, '0');
            return Enumerable.Range(0, hex.Length)
                .Where(x => x%2 == 0)
                .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                .ToArray();
        }

        public static string Base64Encode(this string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(this string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string ToSha1(this string value)
        {
            var x = string.Empty;
            var hash = new SHA1CryptoServiceProvider().ComputeHash(Encoding.ASCII.GetBytes(value));
            x = hash.Aggregate(x, (current, b) => current + b.ToString("x2"));
            return x;
        }

    }
}