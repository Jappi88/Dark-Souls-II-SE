using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using Dark_Souls_II_Save_Editor;
using HavenInterface.IOPackage;

namespace Dark_Souls_2_Resource_Extractor.DMG
{
    public class DMGFile
    {
        public string _resourcepath;

        public DMGFile(string file, string resourcepath)
        {
            MainStream = new StreamIO(file, true);
            _resourcepath = resourcepath;
            Init();
        }

        public StreamIO MainStream { get; }
        public int Version { get; private set; }
        public int Size { get; private set; }
        public int Flag { get; private set; }
        public int IndexCount { get; private set; }
        public int NameTableOffset { get; private set; }
        public DmgEntry[] Entries { get; set; }
        public string[] Names { get; set; }
        //private void Init()
        //{
        //    if (MainStream == null)
        //        return;
        //    var files = new FileInfo[] { };
        //    if (_resourcepath != null)
        //        files = new System.IO.DirectoryInfo(_resourcepath).GetFiles("*.png", System.IO.SearchOption.AllDirectories);
        //    MainStream.Position = 0;
        //    MainStream.IsBigEndian = true;
        //    Version = MainStream.ReadInt32();
        //    Size = MainStream.ReadInt32();
        //    Flag = MainStream.ReadInt32();
        //    int count = MainStream.ReadInt32();
        //    IndexCount = MainStream.ReadInt32();
        //    NameTableOffset = MainStream.ReadInt32();
        //    List<string> names = new List<string> { };
        //    //read names
        //    int current = NameTableOffset;
        //    for (int i = 0; i < IndexCount; i++)
        //    {
        //        MainStream.Position = NameTableOffset + (i * 4);
        //        int offset = MainStream.ReadInt32();
        //        MainStream.Position = offset;
        //        names.Add(ReadName(MainStream));
        //    }
        //    //read entries;
        //    var ents = new List<DmgEntry> { };
        //    int index = 0;
        //    for (int i = 0; i < count; i++)
        //    {
        //        MainStream.Position = 0x1c + (i * 0xc);
        //        var ent = new DmgEntry(this, i);
        //        uint difference = ent.ID2 - ent.ID1;
        //        //if (difference > 0)
        //        //{
        //        FileInfo xfi = files.FirstOrDefault(t => t.Name.GetIdFromName() == ent.ID1);
        //        Image ximage = null;
        //        if (xfi != null)
        //            ximage = (Bitmap)Image.FromFile(xfi.FullName);
        //        for (int j = 0; j <= difference; j++)
        //        {
        //            var xent = new DmgEntry();
        //            xent.Name = names[ent.Index + j];
        //            xent.ID1 = ent.ID1 + (uint)j;
        //            xent.ID2 = ent.ID2;
        //            xent.Index = ent.Index;
        //            xent.entryindex = ent.Index + j;
        //            xfi = files.FirstOrDefault(t => t.Name.GetIdFromName() == xent.ID1);
        //            //if (xfi == null)
        //            //{
        //            //    string xid = xent.ID1.ToString();
        //            //    foreach (var fi in files)
        //            //    {

        //            //        string fileid = fi.Name.GetIdFromName().ToString();
        //            //        if (fileid.Length > xid.Length)
        //            //        {
        //            //            fileid = fileid.Substring(fileid.Length - xid.Length, xid.Length);
        //            //        }
        //            //        if (xid == fileid)
        //            //        {
        //            //            xfi = fi;
        //            //            break;
        //            //        }

        //            //    }
        //            //}
        //            if (xfi != null)
        //            {
        //                xent.Image = (Bitmap)Image.FromFile(xfi.FullName);
        //                ximage = xent.Image;
        //            }
        //            else
        //            {
        //                if (ximage == null)
        //                {
        //                    var t = ents.FirstOrDefault(x => x.Name.ToLower() == xent.Name.ToLower());
        //                    if (t != null)
        //                        xent.Image = t.Image;

        //                }
        //                else
        //                {
        //                    xent.Image = (Bitmap)ximage;
        //                }
        //            }
        //            ents.Add(xent);

        //        }
        //        //}
        //        //else
        //        //{
        //        //    ent.Name = names[ent.Index ];
        //        //    FileInfo fi = files.FirstOrDefault(t => t.Name.GetIDFromName() == ent.ID1);
        //        //    if (fi != null)
        //        //        ent.Image = (Bitmap)Bitmap.FromFile(fi.FullName);
        //        //    ents.Add(ent);
        //        //    index++;
        //        //}
        //    }
        //    Names = names.ToArray();
        //    Entries = ents.ToArray();
        //}

        private void Init()
        {
            if (MainStream == null)
                return;
            var files = new FileInfo[] {};
            if (_resourcepath != null)
                files = new DirectoryInfo(_resourcepath).GetFiles("*.png",
                    SearchOption.AllDirectories);
            MainStream.Position = 0;
            MainStream.IsBigEndian = true;
            Version = MainStream.ReadInt32();
            Size = MainStream.ReadInt32();
            Flag = MainStream.ReadInt32();
            var count = MainStream.ReadInt32();
            IndexCount = MainStream.ReadInt32();
            NameTableOffset = MainStream.ReadInt32();
            var names = new List<string>();
            //read names
            for (var i = 0; i < IndexCount; i++)
            {
                MainStream.Position = NameTableOffset + i*4;
                var offset = MainStream.ReadInt32();
                MainStream.Position = offset;
                names.Add(ReadName(MainStream));
            }
            //read entries;
            var ents = new List<DmgEntry>();
            for (var i = 0; i < count; i++)
            {
                MainStream.Position = 0x1c + i*0xc;
                var ent = new DmgEntry(this, i);
                var difference = ent.ID2 - ent.ID1;
                var fi = files.FirstOrDefault(t => t.Name.GetIdFromName() == ent.ID1);
                if (fi != null)
                    ent.Image = (Bitmap) Image.FromFile(fi.FullName);
                ent.Name = names[ent.Index];
                var isvalid = true;
                for (var j = 1; j <= difference; j++)
                {
                    var xent = new DmgEntry();
                    MainStream.Position = NameTableOffset + (ent.Index + j)*4;
                    xent.Name = names[ent.Index + j];
                    isvalid = xent.Name.ToLower() != "no item" && xent.Name.ToLower() != "no spell" &&
                              xent.Name.ToLower() != "no text" && xent.Name.ToLower() != "none text" &&
                              xent.Name.Length >= 4;
                    if (!isvalid)
                        continue;
                    xent.ID1 = ent.ID1 + (uint) j;
                    xent.ID2 = ent.ID1 + (uint) j;
                    xent.Index = ent.Index + j;
                    xent.entryindex = i + j;
                    //var xfiles = files.Where(x => x.Name.GetIdFromName() == xent.ID1).ToArray();
                    //if (xfiles.Length > 1)
                    //    foreach (var v in xfiles)
                    //        Console.WriteLine(v);
                    fi = files.FirstOrDefault(t => t.Name.GetIdFromName() == xent.ID1);
                    if (fi != null)
                        xent.Image = (Bitmap) Image.FromFile(fi.FullName);
                    //else
                    //{
                    //    xent.Image = ent.Image;
                    //}
                    ents.Add(xent);
                }
                isvalid = ent.Name.ToLower() != "no item" && ent.Name.ToLower() != "no spell" &&
                          ent.Name.ToLower() != "no text" && ent.Name.ToLower() != "none text" && ent.Name.Length >= 4;
                if (!isvalid)
                    continue;
                ents.Add(ent);
            }
            Entries = ents.ToArray();
        }

        private Bitmap GetImage(Bitmap image)
        {
            var bm = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppArgb);
            bm.MakeTransparent();
            using (var g = Graphics.FromImage(bm))
            {
                g.DrawImage(image, new Point(0, 0));
            }
            return bm;
        }

        private string[] GetNames()
        {
            if (MainStream == null)
                return null;
            MainStream.Position = NameTableOffset;
            var names = new List<string>();
            for (var i = 0; i < IndexCount; i++)
            {
                if (MainStream.ReadInt16() == 0)
                    break;
                MainStream.Position -= 2;
                names.Add(ReadName(MainStream));
                MainStream.Position += 2;
            }
            return names.ToArray();
        }

        private string ReadName(StreamIO x)
        {
            var count = 0;
            while (x.ReadInt16() != 0)
                count += 2;
            count += 2;
            x.Position -= count;
            return Encoding.BigEndianUnicode.GetString(x.ReadBytes(count - 2, false));
        }
    }
}