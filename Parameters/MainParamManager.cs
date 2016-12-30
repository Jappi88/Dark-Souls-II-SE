using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Windows.Forms;
using Dark_Souls_II_Save_Editor.BND4;
using Dark_Souls_II_Save_Editor.SaveStuff;
using Dark_Souls_II_Save_Editor.STFSPackage;
using HavenInterface.Compressions.Zlib;
using HavenInterface.IOPackage;
using PS3FileSystem;

namespace Dark_Souls_II_Save_Editor.Parameters
{
    public class MainParamManager
    {
        public MainParamManager(MainBnd4 gameparams)
        {
            ProgressInstance = MainFile.ProgressInstance;
            GameParameters = gameparams;
            var t = new Thread(() => ItemParameters = GetItemParameters());
            t.Start();
            while (t.ThreadState != ThreadState.Stopped)
                Application.DoEvents();
            SpellParameters = new MainSpellParam(this);
            SoulsRequiredPerLevel = new SoulsRequired(this);
        }

        public MainBnd4 GameParameters { get; set; }
        public ItemParameter[] ItemParameters { get; private set; }
        public MainSpellParam SpellParameters { get; }
        public SoulsRequired SoulsRequiredPerLevel { get; private set; }
        public Progressor ProgressInstance { get; set; }

        public ItemParameter[] GetItemParameters()
        {
            if (GameParameters == null)
                return null;
            if (ProgressInstance != null)
                ProgressInstance.DoProgress("Allocating Item Parameter Entry..", 0, 0, 0);
            var ent = GameParameters.Entries.FirstOrDefault(t => t.Name.ToLower() == "itemparam.param");
            if (ent == null)
                return null;
            var ents = new List<ItemParameter>();
            var io = ent.MainInstance.MainIO;
            io.Position = ent.DataOffset + 0xa;
            var count = io.ReadUInt16();
            for (var i = 0; i < count; i++)
            {
                if (ProgressInstance != null)
                    ProgressInstance.DoProgress(
                        "Loading Item Parameters..(" + Functions.GetPercentage(i, count) + "%)",
                        Functions.GetPercentage(i, count), i, count);
                io.Position = ent.DataOffset + 0x40 + i*12;
                var entry = new ParameterEntry
                {
                    ID = io.ReadUInt32(),
                    Offset = io.ReadInt32(),
                    ParameterFileID = io.ReadUInt32()
                };
                if (entry.ID < 1)
                    continue;
                io.Position = ent.DataOffset + entry.Offset;
                var entrydata = io.ReadBytes(0x54, false);
                var item = new ItemParameter(entrydata, entry, false, i);
                if (item.ID1 < 1)
                    continue;
                var x = MainFile.DataBase.Items.GetEntryById((uint) item.ID1, false);
                if (x != null)
                    x.Parameter = item;
                ents.Add(item);
                foreach (var j in item.InternalIDS)
                {
                    if (j < 1)
                        continue;
                    item = new ItemParameter(entrydata, entry, true, i);
                    x = MainFile.DataBase.Items.GetEntryById((uint) j, false);
                    if (x != null)
                        x.Parameter = item;
                    ents.Add(item);
                }
            }
            return ents.ToArray();
        }

        public bool WriteItemParams(ItemParameter[] parameters)
        {
            try
            {
                if (ProgressInstance != null)
                    ProgressInstance.DoProgress("Getting Item parameter File..", 0, 2, 2);
                var bnd = GameParameters.Entries.FirstOrDefault(t => t.Name.ToLower() == "itemparam.param");
                if (bnd == null)
                    return false;
                if (ProgressInstance != null)
                    ProgressInstance.DoProgress("Extracting Item parameter File..", 0, 2, 2);
                var buffer = bnd.Extract();
                if (ProgressInstance != null)
                    ProgressInstance.DoProgress("Writing Parameters..", 0, 2, 2);
                if (!WriteItemParams(buffer, parameters))
                    return false;
                if (ProgressInstance != null)
                    ProgressInstance.DoProgress("Replacing Item parameter File..", 0, 2, 2);
                if (!bnd.Replace(buffer))
                    return false;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool WriteItemParams(byte[] data, ItemParameter[] parameters)
        {
            if (parameters == null || parameters.Length == 0)
                return false;
            using (var io = new MemoryStream(data, true))
            {
                var count = 0;
                foreach (var v in parameters)
                {
                    if (v.IsInternal)
                        continue;
                    if (ProgressInstance != null)
                        ProgressInstance.DoProgress("Writing parameter : " + v.Entry.ID,
                            Functions.GetPercentage(count, parameters.Length), count, parameters.Length);
                    io.Position = v.Entry.Offset;
                    io.Write(v.GetItemData(), 0, 0x54);
                    count++;
                }
            }
            return true;
        }

        private bool xSaveParameterFile(StfsFile file)
        {
            try
            {
                byte[] buffer = null;
                using (var ms = new MemoryStream())
                {
                    using (var io = new StreamIO(ms, true))
                    {
                        buffer = GameParameters.ToByteArray();
                        var uncompressed = buffer.Length;
                        var t =
                            new Thread(
                                () => buffer = MainFile.CompressData(buffer, CompressionMode.Compress, uncompressed/5));
                        t.Start();
                        while (t.ThreadState != ThreadState.Stopped)
                            Application.DoEvents();
                        io.WriteInt32(buffer.Length);
                        io.WriteInt32(uncompressed);
                        io.WriteBytes(buffer, false);
                        if (ProgressInstance != null)
                            ProgressInstance.DoProgress("Rebuilding Main parameter file..", 70, 2, 2);
                        using (var xent = new StreamIO(file.Extract(), true))
                        {
                            var length = xent.ReadInt32();
                            xent.Position += length + 4;
                            io.WriteBytes(xent.ReadBytes((int) (xent.Length - xent.Position - 0x10), false), false);
                        }
                        io.Position = 0;
                        if (ProgressInstance != null)
                            ProgressInstance.DoProgress("Fixing File MD5 Hash..", 90, 2, 2);
                        var hash = MD5.Create().ComputeHash(io.ReadAllBytes(false));
                        io.Position = io.Length;
                        io.WriteBytes(hash, false);
                    }
                    if (ProgressInstance != null)
                        ProgressInstance.DoProgress("Replacing file into STFS Package...", 95, 2, 2);
                    file.Replace(ms.ToArray());
                    if (ProgressInstance != null)
                        ProgressInstance.DoProgress("Replacing file into STFS Package Completed", 100, 2, 2);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool xSaveParameterFile(Ps3File file)
        {
            try
            {
                byte[] buffer = null;
                using (var ms = new MemoryStream())
                {
                    using (var io = new StreamIO(ms, true))
                    {
                        buffer = GameParameters.ToByteArray();
                        var uncompressed = buffer.Length;
                        var t =
                            new Thread(
                                () => buffer = MainFile.CompressData(buffer, CompressionMode.Compress, uncompressed/5));
                        t.Start();
                        while (t.ThreadState != ThreadState.Stopped)
                            Application.DoEvents();
                        io.WriteInt32(buffer.Length);
                        io.WriteInt32(uncompressed);
                        io.WriteBytes(buffer, false);
                        if (ProgressInstance != null)
                            ProgressInstance.DoProgress("Rebuilding Main parameter file..", 70, 2, 2);
                        using (var xent = new StreamIO(file.DecryptToBytes(), true))
                        {
                            var length = xent.ReadInt32();
                            xent.Position += length + 4;
                            io.WriteBytes(xent.ReadBytes((int) (xent.Length - xent.Position), false), false);
                        }
                    }
                    if (ProgressInstance != null)
                        ProgressInstance.DoProgress("Encrypting Parameter File", 95, 2, 2);
                    if (!file.Encrypt(ms.ToArray()))
                        return false;
                    if (!file.Manager.ReBuildChanges())
                        return false;
                    if (ProgressInstance != null)
                        ProgressInstance.DoProgress("Encrypting Parameter File Complete!", 100, 2, 2);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool SaveParametersFile(StfsFile file)
        {
            var x = false;
            var t = new Thread(() => x = xSaveParameterFile(file));
            t.Start();
            while (t.ThreadState != ThreadState.Stopped)
                Application.DoEvents();
            return x;
        }

        private bool SaveParametersFile(Ps3File file)
        {
            var x = false;
            var t = new Thread(() => x = xSaveParameterFile(file));
            t.Start();
            while (t.ThreadState != ThreadState.Stopped)
                Application.DoEvents();
            return x;
        }

        public bool SaveParameters(object sender, MainFile.ConsoleType type)
        {
            if (ProgressInstance != null)
                ProgressInstance.isbusy = true;
            var x = false;
            var t = new Thread(() => x = XSaveParameters(sender, type));
            t.Start();
            while (t.ThreadState != ThreadState.Stopped)
                Application.DoEvents();
            if (ProgressInstance != null)
                ProgressInstance.isbusy = false;
            return x;
        }

        private bool XSaveParameters(object sender, MainFile.ConsoleType type)
        {
            try
            {
                if (ProgressInstance != null)
                    ProgressInstance.isbusy = true;
                if (!WriteItemParams(ItemParameters))
                    return false;
                if (!SpellParameters.Save())
                    return false;
                switch (type)
                {
                    case MainFile.ConsoleType.PC:

                        break;
                    case MainFile.ConsoleType.PS3:
                    case MainFile.ConsoleType.PS3FTP:
                        var manager = sender as Ps3SaveManager;
                        if (manager == null)
                            return false;
                        foreach (var p in manager.Files)
                        {
                            if (p.PFDEntry.file_name.GetIdFromName() == 15)
                            {
                                if (ProgressInstance != null)
                                    ProgressInstance.DoProgress("Writing Parameters..", 0, 2, 2);
                                if (!SaveParametersFile(p))
                                    return false;
                            }
                        }
                        break;
                    case MainFile.ConsoleType.Usb:
                    case MainFile.ConsoleType.XBOX360:
                    case MainFile.ConsoleType.XBOX360FTP:
                        if (!SaveParamToStfs(sender as StfsPackage))
                            return false;
                        break;
                    case MainFile.ConsoleType.None:
                        return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool SaveParamToStfs(StfsPackage stfs)
        {
            try
            {
                if (stfs == null)
                    return false;
                foreach (var f in stfs.Root.Files)
                {
                    if (f.Name.GetIdFromName() == 15)
                    {
                        if (ProgressInstance != null)
                            ProgressInstance.DoProgress("Writing Parameters..", 0, 2, 2);
                        if (SaveParametersFile(f))
                        {
                            if (ProgressInstance != null)
                                ProgressInstance.DoProgress("Rehash/Resigning STFS File...", 100, 2, 2);
                            stfs.RehashAndResign();
                            return true;
                        }
                        break;
                    }
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