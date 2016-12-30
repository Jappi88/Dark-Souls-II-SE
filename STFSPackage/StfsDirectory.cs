using System.Collections.Generic;
using System.IO;
using System.Linq;
using HavenInterface.IOPackage;

namespace Dark_Souls_II_Save_Editor.STFSPackage
{
    public class StfsDirectory : StfsEntry
    {
        public StfsDirectory(StfsEntry ent) : base(ent)
        {
            ListingRead = false;
            Directories = new StfsDirectory[] {};
            Files = new StfsFile[] {};
        }

        public StfsDirectory[] Directories { get; private set; }
        public StfsFile[] Files { get; private set; }
        public bool ListingRead { get; internal set; }

        public void ReadFileListing(StfsPackage package)
        {
            var files = new List<StfsFile>();
            var dirs = new List<StfsDirectory>();
            // setup the entry for the block chain
            //var entry = new StfsEntry();
            //entry.StartingBlockNum = package.Header.stfsVolumeDescriptor.fileTableBlockNum;
            //entry.FileSize = (uint) (package.Header.stfsVolumeDescriptor.fileTableBlockCount*0x1000);

            // generate a block chain for the full file listing
            var block = (uint) package.Header.StfsVolumeDescriptor.fileTableBlockNum;
            for (uint x = 0; x < package.Header.StfsVolumeDescriptor.fileTableBlockCount; x++)
            {
                var currentAddr = package.BlockToAddress(block);
                package.Xio.Position = currentAddr;

                for (uint i = 0; i < 0x40; i++)
                {
                    var fe = new StfsEntry(this);

                    // set the current position
                    fe.FileEntryAddress = currentAddr + i*0x40;

                    // calculate the entry index (in the file listing)
                    fe.EntryIndex = (int) (x*0x40 + i);
                    package.Xio.Position = fe.FileEntryAddress;
                    // read the name, if the length is 0 then break
                    fe.Name = package.Xio.ReadString(StringType.Ascii, 0x28).Replace("\0", "");

                    // read the name length
                    fe.NameLen = package.Xio.ReadUInt8();
                    if ((fe.NameLen & 0x3F) == 0)
                    {
                        package.Xio.Position = currentAddr + (i + 1)*0x40;
                        continue;
                    }
                    if (fe.Name.Length == 0)
                        break;

                    // check for a mismatch in the total allocated blocks for the file
                    fe.BlocksForFile = package.Xio.ReadInt24(false);
                    package.Xio.Position += 3;

                    // read more information
                    fe.StartingBlockNum = package.Xio.ReadInt24(false);
                    fe.PathIndicator = package.Xio.ReadUInt16();
                    fe.FileSize = package.Xio.ReadUInt32();
                    fe.CreatedTimeStamp = package.Xio.ReadUInt32();
                    fe.AccessTimeStamp = package.Xio.ReadUInt32();

                    // get the flags
                    fe.Flags = (byte) (fe.NameLen >> 6);

                    // bits 6 and 7 are flags, clear them
                    fe.NameLen &= 0x3F;
                    if (EntryIndex != fe.PathIndicator)
                        continue;
                    if (!fe.IsDirectory)
                    {
                        var xfile = new StfsFile(fe);
                        files.Add(xfile);
                    }
                    else
                    {
                        var xdir = new StfsDirectory(fe);
                        xdir.ReadFileListing(package);
                        dirs.Add(xdir);
                    }
                }

                block = package.GetHashEntryOfBlock(block).NextBlock;
            }

            Files = files.ToArray();
            Directories = dirs.ToArray();
            ListingRead = true;
            // WrittenToFile = fileListing;
        }

        public StfsFile GetFile(string name, bool recursive)
        {
            var file = Files.FirstOrDefault(x => x.Name.ToLower() == name.ToLower());
            if (file != null)
                return file;
            if (!recursive)
                return null;
            foreach (var dir in Directories)
            {
                file = dir.GetFile(name, true);
                if (file != null)
                    return file;
            }
            return null;
        }

        public void Extract(string path)
        {
            if (!ListingRead)
                ReadFileListing(Instance);
            foreach (var v in Files)
            {
                var fpath = path + "\\" + v.Name;
                v.Extract(fpath);
            }
            foreach (var dir in Directories)
            {
                var dpath = path + "\\" + dir.Name;
                if (!Directory.Exists(dpath))
                    Directory.CreateDirectory(dpath);
                dir.Extract(dpath);
            }
        }

        public void AddFileToList(StfsFile file)
        {
            var list = Files.ToList();
            list.Add(file);
            Files = list.ToArray();
        }

        public void AddDirectoryToList(StfsDirectory dir)
        {
            var list = Directories.ToList();
            list.Add(dir);
            Directories = list.ToArray();
        }

        public void CleanUp()
        {
            Files = new StfsFile[] {};
            Directories = new StfsDirectory[] {};
            ListingRead = false;
        }
    }
}