using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Windows.Forms;
using Dark_Souls_II_Save_Editor.BND4;
using Dark_Souls_II_Save_Editor.Controls;
using Dark_Souls_II_Save_Editor.DB_Builder;
using Dark_Souls_II_Save_Editor.Parameters;
using Dark_Souls_II_Save_Editor.STFSPackage;
using HavenInterface.Compressions.Zlib;
using HavenInterface.Fatx;
using HavenInterface.Interfaces.ServerPlugin;
using HavenInterface.IOPackage;
using PS3FileSystem;
using Telerik.WinControls;

namespace Dark_Souls_II_Save_Editor.SaveStuff
{
    public class MainFile
    {
        public enum ConsoleType
        {
            XBOX360,
            Usb,
            PS3,
            XBOX360FTP,
            PS3FTP,
            PC,
            None
        }

        public static readonly int MinimalDBversion = 14;
        public bool Isvalid;

        public MainFile(string savefile, string dbpath, string itemtype, ConsoleType type, Progressor p)
        {
            ProgressInstance = p;
            if (File.Exists(savefile))
            {
                using (var br = new BinaryReader(new FileStream(savefile, FileMode.Open)))
                {
                    if (br.ReadInt32() == 876891714)
                        type = ConsoleType.PC;
                }
            }
            LoadSave(savefile, dbpath, itemtype, type);
            FilePath = savefile;
            TypePath = itemtype;
            DBPath = dbpath;
        }

        public MainFile(FTPSaveInfo file, string filepath, string dbpath, string itemtype, Progressor p)
        {
            ProgressInstance = p;
            LoadSave(file, filepath, dbpath, itemtype);
            FilePath = filepath;
            TypePath = itemtype;
            DBPath = dbpath;
        }

        public MainFile(FileEntry file, string dbpath, string itemtype, Progressor p)
        {
            ProgressInstance = p;
            LoadSave(file, dbpath, itemtype);
            TypePath = itemtype;
            DBPath = dbpath;
        }

        public ConsoleType LoadedType { get; internal set; }
        public object Saveinstance { get; internal set; }
        public string FilePath { get; internal set; }
        public string DBPath { get; set; }
        public string TypePath { get; set; }
        public SaveSlot[] UsedSlots { get; set; }
        public static DSDataBase DataBase { get; set; }
        public static ItemTypeInstance[] ItemTypes { get; set; }
        public static Progressor ProgressInstance { get; set; }
        public MainParamManager ParamManager { get; set; }
        public SaveSlotInfo SlotInfo { get; internal set; }
        public bool IsBigEndian => LoadedType != ConsoleType.PC;
        public event FileMain.FileActionArg OnMainFileChange;
        public event SaveSlot.SlotChangedHandler OnNameChanged;

        private void LoadSave(string savefile, string dbpath, string itemtypepath, ConsoleType type)
        {
            StfsPackage s = null;
            try
            {
                if (ProgressInstance != null)
                    ProgressInstance.isbusy = true;
                ProgressPopupUI.IsBusy = true;
                switch (type)
                {
                    case ConsoleType.PC:
                        var save = new PCSave(savefile);
                        File.WriteAllBytes(savefile + "_" + save.Slots[0].Name, save.Slots[0].Extract());
                        Saveinstance = save;
                        break;
                    case ConsoleType.PS3FTP:
                    case ConsoleType.PS3:
                        if (!Directory.Exists(savefile))
                            throw new Exception("Invalid PS3 Save directory!");
                        var ps3 = new Ps3SaveManager(savefile,
                            new byte[]
                            {
                                0xB7, 0xFD, 0x46, 0x3E, 0x4A, 0x9C, 0x11, 0x02, 0xDF, 0x17, 0x39, 0xE5, 0xF3, 0xB2, 0xA5,
                                0x0F
                            });
                        if (ps3.Param_SFO.Title != "DARK SOULS II")
                            throw new Exception("Invalid Dark Souls II Save Directory!");
                        if (MainForm.Live == null || (Isvalid = MainForm.Live.EnsureLogin(EnsureType.Loading)))
                        {
                            Isvalid = true;
                            if (DataBase == null || DataBase.Items == null || DataBase.Items.Length == 0)
                                LoadDb(dbpath, itemtypepath);
                            UsedSlots = LoadPS3Slots(ps3);
                        }
                        break;
                    case ConsoleType.XBOX360FTP:
                    case ConsoleType.XBOX360:
                        if (!File.Exists(savefile))
                            throw new Exception("File does not exist at : " + savefile);
                        s = new StfsPackage(savefile, 0);
                        if (s.Header.TitleId != 0x465307E4)
                            throw new Exception("Invalid Dark Souls 2 Savegame!");
                        if (MainForm.Live == null || (Isvalid = MainForm.Live.EnsureLogin(EnsureType.Loading)))
                        {
                            Isvalid = true;
                            if (DataBase == null || DataBase.Items == null || DataBase.Items.Length == 0)
                                LoadDb(dbpath, itemtypepath);
                            UsedSlots = LoadStfsFile(s);
                        }
                        s.Close();
                        break;
                }
                LoadedType = type;
                FilePath = savefile;
                ProgressPopupUI.IsBusy = false;
                if (ProgressInstance != null)
                    ProgressInstance.isbusy = false;
                OnMainFileChange?.Invoke(this, FileMain.FileAction.Loaded);
            }
            catch (Exception ex)
            {
                ProgressPopupUI.IsBusy = false;
                LoadedType = ConsoleType.None;
                if (s != null)
                    s.Close();
                Saveinstance = null;
                if (ProgressInstance != null)
                    ProgressInstance.isbusy = false;
                OnMainFileChange?.Invoke(this, FileMain.FileAction.Closed);
                throw ex;
            }
        }

        public void Reload()
        {
            try
            {
                if (Isvalid && UsedSlots != null && UsedSlots.Length > 0)
                {
                    foreach (var v in UsedSlots)
                        if (v.UserUI != null && !v.UserUI.IsDisposed)
                            v.Reload();
                }
            }
            catch
            {
            }
        }

        public static void LoadDb(string db, string type)
        {
            LoadDb(db, type, ProgressInstance);
        }

        public static void LoadDb(string db, string type, Progressor p)
        {
            ProgressPopupUI.IsBusy = true;
            if (DataBase != null)
            {
                DataBase.ReadDB(true, p);
            }
            else
            {
                ItemTypes = Functions.GetItemTypes(type);
                DataBase = new DSDataBase(db, ItemTypes, p, true);
            }
            ProgressPopupUI.IsBusy = false;
            if (DataBase != null && (DataBase.Items == null || DataBase.Items.Length == 0))
                throw new Exception("Invalid database!");
        }

        public static void DB_OnDBCorrupted(string message, DSDataBase db)
        {
            if (MainForm.Mainpage.IsDisposed)
                return;
            RadMessageBox.Show(
                message + "\nMost recent version will be downloaded, please wait for the operation to complete",
                "DB Error", MessageBoxButtons.OK, RadMessageIcon.Exclamation);
            MainForm.Mainpage.DownloadDatabase();
        }

        private SaveSlot[] LoadPS3Slots(Ps3SaveManager ps3)
        {
            if (ProgressInstance != null)
                ProgressInstance.isbusy = true;
            ProgressPopupUI.IsBusy = true;
            var slots = new List<SaveSlot>();
            var x =
                ps3.Files.FirstOrDefault(
                    t => t.PFDEntry.file_name == "1USER.DAT" || t.PFDEntry.file_name == "2USER.DAT");
            if (x != null)
            {
                var io = new StreamIO(x.DecryptToBytes(), true);
                SlotInfo = new SaveSlotInfo(this, io);
                io.Dispose();
                if (SlotInfo.UsedSlots.Length > 0)
                {
                    foreach (var file in ps3.Files)
                    {
                        var id = file.PFDEntry.file_name.GetIdFromName();
                        if (id == 15)
                        {
                            try
                            {
                                using (var v = new StreamIO(file.DecryptToBytes(), true))
                                {
                                    var compressed = v.ReadInt32();
                                    var decompressed = v.ReadInt32();
                                    if (compressed == 0)
                                        compressed = (int) v.Length - 0x8;
                                    var data = v.ReadBytes(compressed, false);
                                    var t =
                                        new Thread(
                                            () =>
                                                data = CompressData(data, CompressionMode.Decompress, decompressed));
                                    t.Start();
                                    while (t.ThreadState != ThreadState.Stopped)
                                        Application.DoEvents();
                                    var gameParameters = new MainBnd4(new StreamIO(data, true));
                                    ParamManager = new MainParamManager(gameParameters);
                                    break;
                                }
                            }
                            catch
                            {
                                if (!Loadparameters())
                                    new Exception(
                                        "Invalid Parameter file!\nThis is mostly caused by using faulty apps that has damaged the Parameter file.");
                            }
                        }
                    }
                    for (var i = 0; i < SlotInfo.UsedSlots.Length; i++)
                    {
                        var file =
                            ps3.Files.FirstOrDefault(
                                t => t.PFDEntry.file_name == SlotInfo.UsedSlots[i].ToString("X2") + "USER.DAT" ||
                                     t.PFDEntry.file_name ==
                                     (SlotInfo.UsedSlots[i] + 0x100).ToString("X3") + "USER.DAT" ||
                                     t.PFDEntry.file_name ==
                                     (SlotInfo.UsedSlots[i] + 0x200).ToString("X3") + "USER.DAT");

                        var map =
                            ps3.Files.FirstOrDefault(
                                t =>
                                    t.PFDEntry.file_name == (SlotInfo.UsedSlots[i] + 10).ToString("X2") + "USER.DAT" ||
                                    t.PFDEntry.file_name ==
                                    (SlotInfo.UsedSlots[i] + 0x10A).ToString("X3") + "USER.DAT" ||
                                    t.PFDEntry.file_name ==
                                    (SlotInfo.UsedSlots[i] + 0x20A).ToString("X3") + "USER.DAT");
                        if (file != null)
                        {
                            var save = new SaveSlot(this, file.PFDEntry.file_name, SlotInfo.UsedSlots[i] - 1,
                                file.DecryptToBytes(), SlotInfo.GetTimePlayed(SlotInfo.UsedSlots[i] - 1),
                                map == null ? null : map.DecryptToBytes());
                            if (save.IsValid)
                            {
                                save.OnNameChanged += save_OnNameChanged;
                                slots.Add(save);
                            }
                        }
                    }
                }
            }
            return slots.ToArray();
        }

        private bool Loadparameters()
        {
            var path = MainForm.Settings.ResourcePath + "\\Parameters.db";
            MainForm.Mainpage.CheckForResources(this, EventArgs.Empty);
            try
            {
                using (var v = new StreamIO(path, true))
                {
                    var compressed = v.ReadInt32();
                    var decompressed = v.ReadInt32();
                    if (compressed == 0)
                        compressed = (int) v.Length - 0x8;
                    var data = v.ReadBytes(compressed, false);
                    var t =
                        new Thread(
                            () => data = CompressData(data, CompressionMode.Decompress, decompressed));
                    t.Start();
                    while (t.ThreadState != ThreadState.Stopped)
                        Application.DoEvents();
                    var gameParameters = new MainBnd4(new StreamIO(data, true));
                    ParamManager = new MainParamManager(gameParameters);
                }
                return ParamManager != null;
            }
            catch
            {
                return false;
            }
        }

        private void save_OnNameChanged(SaveSlot slot)
        {
            OnNameChanged?.Invoke(slot);
        }

        private SaveSlot[] LoadStfsFile(StfsPackage s)
        {
            if (ProgressInstance != null)
                ProgressInstance.isbusy = true;
            ProgressPopupUI.IsBusy = true;
            var slots = new List<SaveSlot>();
            var files = s.Root.Files.Where(x => x.Name == "USER_DATA" || x.Name == "USER_DATA16").ToArray();
            if (files.Length == 0)
                throw new Exception("STFS does not contain any slot info files!");
            var io = new StreamIO(files[0].Extract(), true);
            SlotInfo = new SaveSlotInfo(this, io);
            io.Dispose();
            if (SlotInfo.UsedSlots.Length > 0)
            {
                foreach (var ent in s.Root.Files)
                {
                    if (!Isvalid)
                        break;
                    var id = ent.Name.GetIdFromName();
                    if (id == 15)
                    {
                        var buffer = ent.Extract();
                        if (buffer == null)
                        {
                            if (!Loadparameters())
                            {
                                new Exception("Invalid Parameter file!\nThe Paramater file has been damaged!");
                            }
                        }
                        else
                        {
                            using (var v = new StreamIO(buffer, true))
                            {
                                var compressed = v.ReadInt32();
                                if (compressed == 0)
                                    compressed = (int) (ent.FileSize - 8);
                                var decompressed = v.ReadInt32();
                                var data = v.ReadBytes(compressed, false);
                                var t =
                                    new Thread(
                                        () => data = CompressData(data, CompressionMode.Decompress, decompressed));
                                t.Start();
                                while (t.ThreadState != ThreadState.Stopped)
                                    Application.DoEvents();
                                var gameParameters = new MainBnd4(
                                    new StreamIO(data, true));
                                ParamManager = new MainParamManager(gameParameters);
                                break;
                            }
                        }
                    }
                }

                foreach (var t1 in SlotInfo.UsedSlots)
                {
                    if (!Isvalid)
                        break;
                    if (s.Root.Files != null)
                    {
                        var ent = s.Root.Files.FirstOrDefault(t => t.Name == "USER_DATA" + t1.ToString("X2"));
                        if (ent == null)
                            continue;
                        var map =
                            s.Root.Files.FirstOrDefault(
                                t => t.Name.ToLower() == ("USER_DATA" + (t1 + 10).ToString("X2")).ToLower());
                        var data = ent.Extract();
                        if (data != null)
                        {
                            var save = new SaveSlot(this, ent.Name, t1 - 1, data, SlotInfo.GetTimePlayed(t1 - 1),
                                map == null ? null : map.Extract());
                            save.OnNameChanged += save_OnNameChanged;
                            if (save.IsValid)
                                slots.Add(save);
                        }
                    }
                }
            }
            return slots.ToArray();
        }

        private void LoadSave(FileEntry file, string dbpath, string itemtypepath)
        {
            StfsPackage s = null;
            try
            {
                if (ProgressInstance != null)
                    ProgressInstance.isbusy = true;
                FilePath = Path.GetTempPath() + "\\DSIIS.tmp";
                if (!file.Extract(FilePath))
                    throw new Exception("Failed to extract " + file.Name + " From usb device!");
                s = new StfsPackage(FilePath, 0);
                if (s.Header.TitleId != 0x465307E4)
                    throw new Exception("Invalid Dark Souls 2 Savegame!");
                if (MainForm.Live == null || (Isvalid = MainForm.Live.EnsureLogin(EnsureType.Loading)))
                {
                    if (DataBase == null || DataBase.Items == null || DataBase.Items.Length == 0)
                        LoadDb(dbpath, itemtypepath);
                    UsedSlots = LoadStfsFile(s);
                    Saveinstance = file;
                    LoadedType = ConsoleType.Usb;
                    OnMainFileChange?.Invoke(this, FileMain.FileAction.Loaded);
                }
                s.Close();
                if (!Isvalid)
                    File.Delete(FilePath);
                if (ProgressInstance != null)
                    ProgressInstance.isbusy = false;
                ProgressPopupUI.IsBusy = false;
            }
            catch (Exception ex)
            {
                ProgressPopupUI.IsBusy = false;
                LoadedType = ConsoleType.None;
                Saveinstance = null;
                if (ProgressInstance != null)
                    ProgressInstance.isbusy = false;
                OnMainFileChange?.Invoke(this, FileMain.FileAction.Closed);
                if (s != null)
                    s.Close();
                throw ex;
            }
        }

        private void LoadSave(FTPSaveInfo file, string filepath, string dbpath, string itemtypepath)
        {
            Saveinstance = file;
            FilePath = filepath;
            LoadSave(filepath, dbpath, itemtypepath, file.IsPS3 ? ConsoleType.PS3FTP : ConsoleType.XBOX360FTP);
        }

        public void SaveSlot(SaveSlot slot)
        {
            SaveAll(true, slot);
        }

        public void SaveSlot(SaveSlot slot, StfsPackage s)
        {
            try
            {
                if (MainForm.Live != null && !(Isvalid = MainForm.Live.EnsureLogin(EnsureType.Saving)))
                    return;
                SaveBlock infoblock;
                switch (LoadedType)
                {
                    case ConsoleType.PC:

                        break;
                    case ConsoleType.PS3FTP:
                    case ConsoleType.PS3:
                        var manager = new Ps3SaveManager(FilePath,
                            new byte[]
                            {
                                0xB7, 0xFD, 0x46, 0x3E, 0x4A, 0x9C, 0x11, 0x02, 0xDF, 0x17, 0x39, 0xE5, 0xF3, 0xB2, 0xA5,
                                0x0F
                            });
                        var file1 = manager.Files.FirstOrDefault(t => t.PFDEntry.file_name == "1USER.DAT");
                        var file2 = manager.Files.FirstOrDefault(t => t.PFDEntry.file_name == "2USER.DAT");
                        var file3 = manager.Files.FirstOrDefault(t => t.PFDEntry.file_name == "16USER.DAT");
                        if (file1 != null || file2 != null || file3 != null)
                        {
                            var save1 = manager.Files.FirstOrDefault(t => t.PFDEntry.file_name == slot.SlotName);
                            Ps3File map1 = null, map2 = null, map3 = null;
                            if (slot.MapInfo != null)
                            {
                                map1 = manager.Files.FirstOrDefault(t => t.PFDEntry.file_name == slot.MapInfo.FileName);
                                map2 =
                                    manager.Files.FirstOrDefault(t => t.PFDEntry.file_name == slot.MapInfo.GetName(110));
                                map3 =
                                    manager.Files.FirstOrDefault(t => t.PFDEntry.file_name == slot.MapInfo.GetName(210));
                            }

                            var save2 =
                                manager.Files.FirstOrDefault(
                                    t => t.PFDEntry.file_name == "10" + (slot.SlotIndex + 1).ToString() + "USER.DAT");
                            var save3 =
                                manager.Files.FirstOrDefault(
                                    t => t.PFDEntry.file_name == "20" + (slot.SlotIndex + 1).ToString() + "USER.DAT");
                            if (save1 != null || save2 != null || save3 != null)
                            {
                                infoblock = SlotInfo.GetBlock(4, 0x6d);
                                if (infoblock == null)
                                    throw new Exception("Failed to save changes! user info datablock is not present!");
                                slot.WriteToUserData(infoblock.BlockData, false);
                                if (file3 != null)
                                {
                                    var newinfo = new SaveSlotInfo(this, new StreamIO(file3.DecryptToBytes(), true));
                                    infoblock = newinfo.GetBlock(4, 0x6d);
                                    if (infoblock == null)
                                        throw new Exception(
                                            "Failed to save changes! user info datablock is not present!");
                                    slot.WriteToUserData(infoblock.BlockData, true);
                                    file3.Encrypt(newinfo.Rebuild(LoadedType));
                                }
                                var savedata = slot.RebuildSave(true);
                                if (save1 != null)
                                    save1.Encrypt(savedata);
                                if (save2 != null)
                                    save2.Encrypt(savedata);
                                if (save3 != null)
                                    save3.Encrypt(savedata);
                                var userdata = SlotInfo.Rebuild(LoadedType);
                                if (file1 != null)
                                    file1.Encrypt(userdata);
                                if (file2 != null)
                                    file2.Encrypt(userdata);
                                if (map1 != null)
                                    map1.Encrypt(slot.MapInfo.Rebuild(LoadedType));
                                if (map2 != null)
                                    map2.Encrypt(slot.MapInfo.Rebuild(LoadedType));
                                if (map3 != null)
                                    map3.Encrypt(slot.MapInfo.Rebuild(LoadedType));
                            }
                            manager.ReBuildChanges();
                        }
                        break;
                    case ConsoleType.XBOX360FTP:
                    case ConsoleType.XBOX360:
                    case ConsoleType.Usb:
                        infoblock = SlotInfo.GetBlock(4, 0x6d);
                        if (infoblock == null)
                            throw new Exception("Failed to save changes! user info datablock is not present!");
                        var xfile1 = s.Root.Files.FirstOrDefault(t => t.Name == "USER_DATA");
                        var xfile2 = s.Root.Files.FirstOrDefault(t => t.Name == "USER_DATA16");
                        if (xfile1 == null && xfile2 == null)
                            throw new Exception("STFS does not contain any save info files!");
                        SaveSlotInfo slotinfo = null;
                        if (xfile2 != null)
                            slotinfo = new SaveSlotInfo(this, new StreamIO(xfile2.Extract(), true));
                        SaveBlock newblock = null;
                        if (slotinfo != null)
                            newblock = slotinfo.GetBlock(4, 0x6d);
                        foreach (var f in s.Root.Files.Where(f => f.Name.ToLower() == slot.SlotName.ToLower()))
                        {
                            f.Replace(slot.RebuildSave(true));
                            slot.WriteToUserData(infoblock.BlockData, false);
                            if (newblock != null)
                                slot.WriteToUserData(newblock.BlockData, true);
                        }
                        if (slot.MapInfo != null)
                            s.ReplaceFile(slot.MapInfo.Rebuild(LoadedType), slot.MapInfo.FileName);
                        if (xfile1 != null)
                            xfile1.Replace(SlotInfo.Rebuild(LoadedType));
                        if (xfile2 != null)
                            xfile2.Replace(slotinfo.Rebuild(LoadedType));
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static byte[] CompressData(byte[] data, CompressionMode mode, int decompresssize)
        {
            if (ProgressInstance != null)
            {
                ProgressPopupUI.IsBusy = true;
                ProgressInstance.isbusy = true;
                ProgressInstance.DoProgress(Enum.GetName(typeof (CompressionMode), mode) + "ing Parameter File...", 0,
                    2, 2);
            }
            using (var output = new MemoryStream())
            {
                using (var ms = new MemoryStream(data))
                {
                    using (var zs = new ZlibStream(ms, mode, CompressionLevel.Level5, false))
                    {
                        var count = 0;
                        var transfered = 0;
                        var buffer = new byte[2048];
                        while ((count = zs.Read(buffer, 0, 2048)) > 0)
                        {
                            if (decompresssize > 0)
                            {
                                if (ProgressInstance != null)
                                    ProgressInstance.DoProgress(
                                        Enum.GetName(typeof (CompressionMode), mode) + "ing Parameter File...",
                                        Functions.GetPercentage(transfered, decompresssize), 2, 2);
                            }
                            transfered += count;
                            output.Write(buffer, 0, count);
                        }
                    }
                }
                ProgressPopupUI.IsBusy = false;
                return output.ToArray();
            }
        }

        public bool Backup()
        {
            return Backup(LoadedType == ConsoleType.Usb ? Saveinstance : FilePath, LoadedType);
        }

        public static bool Backup(object sender, ConsoleType type)
        {
            try
            {
                if (type == ConsoleType.PS3 || type == ConsoleType.PS3FTP)
                    return Functions.Backup(sender as string);
                if (type == ConsoleType.Usb)
                {
                    var ent = sender as FileEntry;
                    if (ent != null)
                    {
                        var temp = Path.GetTempPath() + "\\" + ent.Name;
                        ent.Extract(temp);
                        var valid = Functions.Backup(temp);
                        File.Delete(temp);
                        return valid;
                    }
                }
                else if (type == ConsoleType.XBOX360 || type == ConsoleType.XBOX360FTP)
                    return Functions.Backup(sender as string);
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed To Make Backup!\n" + ex.Message);
            }
        }

        private bool xsaveall(ref string error, bool includeslots, SaveSlot onlyslot = null)
        {
            try
            {
                if (ProgressPopupUI.Progress != null)
                    ProgressInstance.isbusy = true;
                ProgressPopupUI.IsBusy = true;
                ProgressPopupUI.OnViewShow();
                if (UsedSlots == null || UsedSlots.Length == 0)
                    return false;
                var xreturn = true;
                try
                {
                    StfsPackage s = null;
                    if (File.Exists(FilePath) && LoadedType != ConsoleType.PC && LoadedType != ConsoleType.PS3 &&
                        LoadedType != ConsoleType.PS3FTP)
                        s = new StfsPackage(FilePath, 0);
                    //ParamManager.SaveParameters(s);
                    if (includeslots)
                    {
                        foreach (var v in UsedSlots)
                        {
                            if (!Isvalid)
                                break;
                            if (onlyslot != null && onlyslot.SlotName != v.SlotName)
                                continue;
                            SaveSlot(v, s);
                        }
                    }
                    if (s != null)
                    {
                        s.Header.TransferFlag = TransferLock.AllowTransfer;
                        s.RehashAndResign();
                        s.Close();
                    }
                    if (LoadedType == ConsoleType.XBOX360FTP || LoadedType == ConsoleType.PS3FTP)
                    {
                        var x = Saveinstance as FTPSaveInfo;
                        if (x == null)
                            return false;
                        x.Replace(FilePath);
                    }
                    else if (LoadedType == ConsoleType.Usb)
                    {
                        var fileEntry = Saveinstance as FileEntry;
                        if (fileEntry != null)
                            xreturn = fileEntry.Replace(FilePath);
                        //(Saveinstance as CLKsFATXLib.File).Parent.CreateNewFile(temp, "-GAME_0000");
                    }
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                    xreturn = false;
                }
                ProgressPopupUI.IsBusy = false;
                ProgressPopupUI.OnViewClose();
                return xreturn;
            }
            catch (Exception ex)
            {
                ProgressPopupUI.IsBusy = false;
                error = ex.Message;
                ProgressPopupUI.OnViewClose();
                return false;
            }
        }

        public void SaveAll(bool includeslots, SaveSlot slotonly = null)
        {
            var error = "";
            try
            {
                if (MainForm.Live != null && !(Isvalid = MainForm.Live.EnsureLogin(EnsureType.Saving)))
                    return;
                var x = false;
                var t = new Thread(() => x = xsaveall(ref error, includeslots, slotonly));
                t.Start();
                while (t.ThreadState != ThreadState.Stopped)
                    Application.DoEvents();
                if (x)
                {
                    OnMainFileChange?.Invoke(this, FileMain.FileAction.Saved);
                }
                if (ProgressInstance != null)
                    ProgressInstance.isbusy = false;
                if (!x)
                {
                    ProgressPopupUI.OnViewClose();
                    throw new Exception(error);
                }
            }
            catch (Exception ex)
            {
                ProgressPopupUI.OnViewClose();
                throw ex;
            }
        }

        public void ExtractSlot(SaveSlot slot, string filepath)
        {
            try
            {
                if (!Isvalid)
                    return;
                using (var ms = new MemoryStream())
                {
                    var data = SlotInfo.ExtractSlotInfo(slot.SlotIndex);
                    ms.Write(data, 0, 0x1f0);
                    data = slot.RebuildSave(false);
                    var flag = LoadedType == ConsoleType.XBOX360FTP || LoadedType == ConsoleType.XBOX360 ||
                               LoadedType == ConsoleType.Usb;
                    ms.Write(data, 0, flag ? data.Length - 0x10 : data.Length);
                    data = GetSlotProgress(slot);
                    ms.Write(data, 0, flag ? data.Length - 0x10 : data.Length);
                    File.WriteAllBytes(filepath, ms.ToArray());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public byte[] GetSlotProgress(SaveSlot slot)
        {
            if (!Isvalid)
                return null;
            switch (LoadedType)
            {
                case ConsoleType.PS3:
                case ConsoleType.PS3FTP:
                    var manager = new Ps3SaveManager(FilePath,
                        new byte[]
                        {0xB7, 0xFD, 0x46, 0x3E, 0x4A, 0x9C, 0x11, 0x02, 0xDF, 0x17, 0x39, 0xE5, 0xF3, 0xB2, 0xA5, 0x0F});
                    var file =
                        manager.Files.FirstOrDefault(
                            t => t.PFDEntry.file_name == (11 + slot.SlotIndex).ToString("X2") + "USER.DAT");
                    if (file == null)
                        file =
                            manager.Files.FirstOrDefault(
                                t => t.PFDEntry.file_name == "1" + (11 + slot.SlotIndex).ToString("X2") + "USER.DAT");
                    if (file == null)
                        file =
                            manager.Files.FirstOrDefault(
                                t => t.PFDEntry.file_name == "2" + (11 + slot.SlotIndex).ToString("X2") + "USER.DAT");
                    if (file != null)
                        return file.DecryptToBytes();
                    return null;
                case ConsoleType.XBOX360:
                case ConsoleType.XBOX360FTP:
                    var s = new StfsPackage(FilePath, 0);
                    var data = s.ExtractFile("USER_DATA" + (11 + slot.SlotIndex).ToString("X2"));
                    s.Close();
                    Array.Resize(ref data, data.Length - 0x10);
                    return data;
                case ConsoleType.Usb:
                    var x = Saveinstance as FileEntry;
                    if (x != null)
                    {
                        var buffer =
                            new StfsPackage(x.GetStream(), 0).ExtractFile("USER_DATA" +
                                                                          (11 + slot.SlotIndex).ToString("X2"));
                        Array.Resize(ref buffer, buffer.Length - 0x10);
                        return buffer;
                    }
                    break;
                default:
                    return null;
            }
            return null;
        }

        public void SetSlotProgress(byte[] data, SaveSlot slot)
        {
            if (data == null || data.Length != 0x7A8A8 || !Isvalid)
                return;
            switch (LoadedType)
            {
                case ConsoleType.PS3:
                case ConsoleType.PS3FTP:
                    var manager = new Ps3SaveManager(FilePath,
                        new byte[]
                        {0xB7, 0xFD, 0x46, 0x3E, 0x4A, 0x9C, 0x11, 0x02, 0xDF, 0x17, 0x39, 0xE5, 0xF3, 0xB2, 0xA5, 0x0F});
                    var file =
                        manager.Files.FirstOrDefault(
                            t => t.PFDEntry.file_name == (11 + slot.SlotIndex).ToString("X2") + "USER.DAT");
                    if (file != null)
                        file.Encrypt(data);
                    file =
                        manager.Files.FirstOrDefault(
                            t => t.PFDEntry.file_name == "1" + (11 + slot.SlotIndex).ToString("X2") + "USER.DAT");
                    if (file != null)
                        file.Encrypt(data);
                    file =
                        manager.Files.FirstOrDefault(
                            t => t.PFDEntry.file_name == "2" + (11 + slot.SlotIndex).ToString("X2") + "USER.DAT");
                    if (file != null)
                        file.Encrypt(data);
                    break;
                case ConsoleType.XBOX360:
                case ConsoleType.XBOX360FTP:
                    var hash = MD5.Create().ComputeHash(data);
                    Array.Resize(ref data, data.Length + 0x10);
                    Array.Copy(hash, 0, data, data.Length - 0x10, 0x10);
                    var s = new StfsPackage(FilePath, 0);
                    s.ReplaceFile(data, "USER_DATA" + (11 + slot.SlotIndex).ToString("X2"));
                    s.RehashAndResign();
                    s.Close();
                    break;
                case ConsoleType.Usb:
                    var xhash = MD5.Create().ComputeHash(data);
                    Array.Resize(ref data, data.Length + 0x10);
                    Array.Copy(xhash, 0, data, data.Length - 0x10, 0x10);
                    byte[] temp = null;
                    var fileEntry = Saveinstance as FileEntry;
                    if (fileEntry != null)
                        temp = fileEntry.Extract();
                    s = new StfsPackage(temp, 0);
                    s.ReplaceFile(data, "USER_DATA" + (11 + slot.SlotIndex).ToString("X2"));
                    s.RehashAndResign();
                    s.Close();
                    if (fileEntry != null && temp != null)
                    {
                        using (var ms = new MemoryStream(temp))
                        {
                            fileEntry.Replace(ms);
                        }
                    }
                    break;
            }
        }

        public SaveSlot ReplaceSlot(SaveSlot slot, string filepath)
        {
            try
            {
                if (!Isvalid)
                    return slot;
                var savedata = File.ReadAllBytes(filepath);
                var userdata = new byte[0x1F0];
                Array.Copy(savedata, 0, userdata, 0, 0x1f0);
                var slotdata = new byte[0x1B2BC];
                Array.Copy(savedata, 0x1f0, slotdata, 0, slotdata.Length);
                if (savedata.Length >= userdata.Length + slotdata.Length + 0x7A8A8)
                {
                    var progress = new byte[0x7A8A8];
                    Array.Copy(savedata, 0x1B4AC, progress, 0, progress.Length);
                    SetSlotProgress(progress, slot);
                }
                SlotInfo.ReplaceSlotInfo(userdata, slot.SlotIndex);
                var x = new SaveSlot(this, slot.SlotName, slot.SlotIndex, slotdata,
                    SlotInfo.GetTimePlayed(slot.SlotIndex));
                if (!x.IsValid)
                    throw new Exception("Selected Slot is invalid : " + filepath);
                x.CopyTo(slot);
                return slot;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}