using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using HavenInterface.IOPackage;

namespace Dark_Souls_II_Save_Editor.STFSPackage
{
    public class StfsPackage
    {
        #region Events

        public event ProgressChangedHandler ProgressChanged;

        protected virtual void OnProgressChanged(object instance, ProgressChangedArg e)
        {
            ProgressChanged?.Invoke(instance, e);
        }

        public event ProgressCompletedHandler ProgressCompleted;

        protected virtual void OnProgressCompleted(object instance, ProgressCompletedArg e)
        {
            ProgressCompleted?.Invoke(instance, e);
        }

        #endregion

        #region Internal variables

        internal readonly uint[] DataBlocksPerHashTreeLevel = {0xAA, 0x70E4, 0x4AF768};
        internal StreamIO Xio;
        internal bool IoPassedIn;
        internal Sex PackageSex;
        internal uint[] BlockStep = new uint[2];
        internal uint FirstHashTableAddress;
        internal byte HashOffset;
        internal Level TopLevel;
        internal HashTable TopTable;
        internal HashTable Cached;
        internal uint[] TablesPerLevel = new uint[3];
        internal uint Flags;

        #endregion

        #region Public Variables

        public ContentHeader Header { get; private set; }
        public StfsDirectory Root { get; private set; }
        public bool AllowConsecutive { get; set; }

        #endregion

        #region Constructors

        public StfsPackage(string file, uint flags)
        {
            Flags = flags;
            IoPassedIn = false;
            Xio = new StreamIO(file, true);
            try
            {
                Init();
            }
            catch (Exception ex)
            {
                Close();
                throw;
            }
        }

        public StfsPackage(byte[] file, uint flags)
        {
            Flags = flags;
            IoPassedIn = false;
            Xio = new StreamIO(file, true);
            try
            {
                Init();
            }
            catch (Exception ex)
            {
                Close();
                throw;
            }
        }

        public StfsPackage(Stream filestream, uint flags)
        {
            Flags = flags;
            IoPassedIn = true;
            Xio = new StreamIO(filestream, true);
            try
            {
                Init();
            }
            catch (Exception ex)
            {
                Close();
                throw;
            }
        }

        public StfsPackage(string file) : this(file, 0)
        {
        }

        public StfsPackage(byte[] file) : this(file, 0)
        {
        }

        public StfsPackage(Stream filestream) : this(filestream, 0)
        {
        }

        #endregion

        #region Private/Internal Methods

        private void Init()
        {
            if ((Flags & (uint) StfsPackageFlags.StfsPackageCreate) == 1)
            {
                var headerSize =
                    (uint)
                        ((Flags & (uint) StfsPackageFlags.StfsPackagePec) == 1
                            ? ((Flags & (uint) StfsPackageFlags.StfsPackageFemale) == 1 ? 0x2000 : 0x1000)
                            : ((
                                Flags & (uint) StfsPackageFlags.StfsPackageFemale) == 1
                                ? 0xB000
                                : 0xA000));
                var zeroBuffer = new byte[0x1000];

                // Write all null bytes for the header
                for (uint i = 0;
                    i < (headerSize >> 0xC) + ((Flags & (uint) StfsPackageFlags.StfsPackageFemale) == 1 ? 1 : 2) + 1;
                    i++)
                    Xio.WriteBytes(zeroBuffer);

                // if it's female, then we need to Write it to the volume descriptor
                Xio.Position = (Flags & (uint) StfsPackageFlags.StfsPackagePec) == 1 ? 0x246 : 0x37B;
                Xio.WriteUInt32((Flags & (uint) StfsPackageFlags.StfsPackageFemale) >> 2);
            }

            Parse();
        }

        private void Parse()
        {
            if ((Flags & (uint) StfsPackageFlags.StfsPackageCreate) == 1)
                Header = new ContentHeader(Xio,
                    (Flags & (uint) StfsPackageFlags.StfsPackagePec) | (uint) ContentFlags.MetadataSkipRead |
                    (uint) ContentFlags.MetadataDontFreeThumbnails);
            else
                Header = new ContentHeader(Xio, Flags & (uint) StfsPackageFlags.StfsPackagePec);

            // if the pacakge was created, then give all the metadata a default value
            if ((Flags & (uint) StfsPackageFlags.StfsPackageCreate) == 1)
            {
                Header.Magic = Magic.CON;
                Header.Certificate.publicKeyCertificateSize = 0x1A8;
                Header.Certificate.ownerConsoleType = ConsoleType.Retail;
                Header.Certificate.consoleTypeFlags = 0;

                Header.LicenseData = new LicenseEntry[0x10];
                Header.LicenseData[0].type = LicenseType.Unrestricted;
                Header.LicenseData[0].data = 0xFFFFFFFFFFFF;

                uint headerSize;
                if ((Flags & (uint) StfsPackageFlags.StfsPackagePec) == 1)
                    headerSize = (uint) ((Flags & (uint) StfsPackageFlags.StfsPackageFemale) == 1 ? 0x2000 : 0x1000);
                else
                    headerSize = (uint) ((Flags & (uint) StfsPackageFlags.StfsPackageFemale) == 1 ? 0xAD0E : 0x971A);

                Header.HeaderSize = headerSize;
                Header.ContentType = 0;
                Header.MetaDataVersion = 2;
                Header.ContentSize = 0;
                Header.MediaId = 0;
                Header.Version = 0;
                Header.BaseVersion = 0;
                Header.TitleId = 0;
                Header.Platform = 0;
                Header.ExecutableType = 0;
                Header.DiscNumber = 0;
                Header.DiscInSet = 0;
                Header.SavegameId = 0;
                Header.ConsoleId = new byte[5];
                Header.ProfileId = new byte[8];
                // volume descriptor
                Header.StfsVolumeDescriptor.size = 0x24;
                Header.StfsVolumeDescriptor.blockSeperation =
                    (byte) ((Flags & (uint) StfsPackageFlags.StfsPackageFemale) >> 2);
                Header.StfsVolumeDescriptor.fileTableBlockCount = 1;
                Header.StfsVolumeDescriptor.fileTableBlockNum = 0;
                Header.StfsVolumeDescriptor.allocatedBlockCount = 1;
                Header.StfsVolumeDescriptor.unallocatedBlockCount = 0;

                Header.FileSystem = FileSystem.FileSystemStfs;
                Header.DataFileCount = 0;
                Header.DataFileCombinedSize = 0;
                Header.SeasonNumber = 0;
                Header.EpisodeNumber = 0;
                Header.SeasonId = new byte[0x10];
                Header.SeriesId = new byte[0x10];
                Header.DeviceId = new byte[0x14];
                Header.DisplayName = "";
                Header.DisplayDescription = "";
                Header.PublisherName = "";
                Header.TitleName = "";
                Header.TransferFlags = 0;
                Header.ThumbnailImage = null;
                Header.ThumbnailImageSize = 0;
                Header.TitleThumbnailImage = null;
                Header.TitleThumbnailImageSize = 0;

                Header.WriteMetaData();

                // set the first block to allocated
                Xio.Position = ((headerSize + 0xFFF) & 0xFFFFF000) + 0x14;
                Xio.WriteUInt32(0x80FFFFFF);
            }

            // make sure the file system is STFS
            if (Header.FileSystem != FileSystem.FileSystemStfs && (Flags & (uint) StfsPackageFlags.StfsPackagePec) == 0)
                throw new Exception("STFS: Invalid file system header.\n");
            var basebyte = (byte) (((Header.HeaderSize + 0xFFF) & 0xF000) >> 0xC);
            var idx = (byte) (Header.StfsVolumeDescriptor.blockSeperation & 3);
            if (basebyte == 0xB)
            {
                if (idx == 1)
                {
                    PackageSex = Sex.StfsFemale;
                    BlockStep[0] = 0xAB;
                    BlockStep[1] = 0x718F;
                }
                else throw new Exception("Invalid Stfs File!");
            }
            else if (basebyte == 0xA)
            {
                if (idx == 0 || idx == 2)
                {
                    PackageSex = Sex.StfsMale;
                    BlockStep[0] = 0xAC;
                    BlockStep[1] = 0x723A;
                }
                else throw new Exception("Invalid Stfs File!");
            }

            //Grab Real Block Count
            for (var i = Header.StfsVolumeDescriptor.allocatedBlockCount - 1; i > 0; i--)
            {
                Header.StfsVolumeDescriptor.allocatedBlockCount = i + 1;
                if (BlockToAddress(i) < Xio.Length)
                    break;
            }

            // address of the first hash table in the package, comes right after the header
            FirstHashTableAddress = (Header.HeaderSize + 0x0FFF) & 0xFFFFF000;

            // calculate the number of tables per level
            TablesPerLevel[0] =
                (uint)
                    (Header.StfsVolumeDescriptor.allocatedBlockCount/0xAA +
                     (Header.StfsVolumeDescriptor.allocatedBlockCount%0xAA != 0
                         ? 1
                         : 0));
            TablesPerLevel[1] = (uint) (TablesPerLevel[0]/0xAA + (TablesPerLevel[0]%0xAA != 0 &&
                                                                  Header.StfsVolumeDescriptor.allocatedBlockCount >
                                                                  0xAA
                ? 1
                : 0));
            TablesPerLevel[2] = (uint) (TablesPerLevel[1]/0xAA + (TablesPerLevel[1]%0xAA != 0 &&
                                                                  Header.StfsVolumeDescriptor.allocatedBlockCount >
                                                                  0x70E4
                ? 1
                : 0));

            // calculate the level of the top table
            TopTable = new HashTable();
            TopLevel = CalcualateTopLevel();

            // read in the top hash table
            TopTable.TrueBlockNumber = ComputeLevelNBackingHashBlockNumber(0, TopLevel);
            TopTable.Level = TopLevel;

            var baseAddress = (TopTable.TrueBlockNumber << 0xC) + FirstHashTableAddress;
            TopTable.AddressInFile = (uint) (baseAddress + ((Header.StfsVolumeDescriptor.blockSeperation & 2) <<
                                                            0xB));

            Xio.Position = TopTable.AddressInFile;

            var dataBlocksPerHashTreeLevel = new uint[] {1, 0xAA, 0x70E4};

            // load the information
            TopTable.EntryCount = Header.StfsVolumeDescriptor.allocatedBlockCount/
                                  dataBlocksPerHashTreeLevel[(int) TopLevel];
            if (Header.StfsVolumeDescriptor.allocatedBlockCount > 0x70E4 &&
                (Header.StfsVolumeDescriptor.allocatedBlockCount%0x70E4 != 0))
                TopTable.EntryCount++;
            else if (Header.StfsVolumeDescriptor.allocatedBlockCount > 0xAA &&
                     (Header.StfsVolumeDescriptor.allocatedBlockCount%0xAA != 0))
                TopTable.EntryCount++;
            TopTable.Entries = new HashEntry[TopTable.EntryCount];
            for (var i = 0; i < TopTable.EntryCount; i++)
            {
                var hash = Xio.ReadBytes(0x14);
                TopTable.Entries[i] = new HashEntry(Xio.ReadUInt32()) {BlockHash = hash};
            }


            // set default values for the root of the file listing
            var fe = new StfsEntry(this) {PathIndicator = 0xFFFF, Name = "Root", EntryIndex = 0xFFFF};
            Root = new StfsDirectory(fe);
            Root.ReadFileListing(this);
        }

        private Level CalcualateTopLevel()
        {
            if (Header.StfsVolumeDescriptor.allocatedBlockCount <= 0xAA)
                return Level.Zero;
            if (Header.StfsVolumeDescriptor.allocatedBlockCount <= 0x70E4)
                return Level.One;
            if (Header.StfsVolumeDescriptor.allocatedBlockCount <= 0x4AF768)
                return Level.Two;
            throw new Exception("STFS: Invalid number of allocated blocks.\n");
        }

        private uint ComputeBackingDataBlockNumber(uint blockNum)
        {
            var space = (byte) PackageSex;
            var toReturn = ((blockNum + 0xAA)/0xAA << space) + blockNum;
            if (blockNum < 0xAA)
                return toReturn;
            if (blockNum < 0x70E4)
                return toReturn + (((blockNum + 0x70E4)/0x70E4) << space);
            return (uint) ((1 << space) + (toReturn + (((blockNum + 0x70E4)/0x70E4) << space)));
        }

        internal uint BlockToAddress(uint blockNum)
        {
            // check for invalid block number
            if (blockNum >= 0xFFFFFF)
                throw new Exception("STFS: Block number must be less than 0xFFFFFF.\n");
            return (ComputeBackingDataBlockNumber(blockNum) << 0xC) + FirstHashTableAddress;
        }

        private uint ComputeLevelNBackingHashBlockNumber(uint blockNum, Level level)
        {
            if (blockNum >= 0x4AF768)
                return 0xFFFFFF;
            switch ((uint) level)
            {
                case 0:
                {
                    //if (blockNum < 0xAA)
                    //    return 0;

                    //uint num = (blockNum / 0xAA) * BlockStep[0];
                    //num += ((blockNum / 0x70E4) + 1) << ((byte)PackageSex);

                    //if (blockNum / 0x70E4 == 0)
                    //    return num;

                    //return (uint) (num + (1 << (byte)PackageSex));
                    var num = blockNum/0xAA*BlockStep[0];
                    // Adjusts the result for Level 1 table count
                    if (blockNum >= 0xAA)
                    {
                        num += (blockNum/0x70E4 + 1) << (byte) PackageSex;
                        // Adjusts for the Level 2 table
                        if (blockNum >= 0x70E4)
                            num += (uint) (1 << (byte) PackageSex);
                    }
                    return num;
                }
                case 1:
                {
                    if (blockNum < 0x70E4)
                        return BlockStep[0];
                    return (uint) (1 << (byte) PackageSex) + blockNum/0x70E4*BlockStep[1];
                    ;
                }

                // Only one Level 2 table
                case 2:
                    return BlockStep[1];
                default:
                    throw new Exception("STFS: Invalid level.\n");
            }
        }

        internal uint GetTableHashAddress(uint index, Level lvl)
        {
            if (lvl >= TopTable.Level || lvl < 0)
                throw new Exception("STFS: Level is invalid. No parent hash address accessible.\n");

            // compute base address of the hash table
            var baseHashAddress = GetBaseHashTableAddress(index/0xAA, lvl + 1);

            // add the hash offset to the base address so we use the correct table
            if (lvl + 1 == TopLevel)
                baseHashAddress += (uint) ((Header.StfsVolumeDescriptor.blockSeperation & 2) << 0xB);
            else
                baseHashAddress += (uint) (((byte) TopTable.Entries[index].Status & 0x40) << 6);

            return baseHashAddress + index*0x18;
        }

        internal HashEntry GetHashEntryOfBlock(uint blockNum)
        {
            if (blockNum >= Header.StfsVolumeDescriptor.allocatedBlockCount)
                throw new Exception("STFS: Reference to illegal block number.\n");
            //go to the position of the hash address
            Xio.Position = GetHashAddressOfBlock(blockNum);
            //// read the hash entry
            var hash = Xio.ReadBytes(0x14);
            var he = new HashEntry(Xio.ReadUInt32());
            he.BlockHash = hash;
            return he;
        }

        private uint GetHashAddressOfBlock(uint blockNum)
        {
            if (blockNum >= Header.StfsVolumeDescriptor.allocatedBlockCount)
                throw new Exception("STFS: Reference to illegal block number.\n");

            var hashAddr = (ComputeLevelNBackingHashBlockNumber(blockNum, Level.Zero) << 0xC) + FirstHashTableAddress;
            hashAddr += blockNum%0xAA*0x18;
            switch ((uint) TopLevel)
            {
                case 0:
                    hashAddr += (uint) ((Header.StfsVolumeDescriptor.blockSeperation & 2) << 0xB);
                    break;
                case 1:
                    hashAddr += (uint) (((byte) TopTable.Entries[blockNum/0xAA].Status & 0x40) << 6);
                    break;
                case 2:
                    var level1Off = (uint) (((byte) TopTable.Entries[blockNum/0x70E4].Status & 0x40) << 6);
                    var pos = ComputeLevelNBackingHashBlockNumber(blockNum << 0xC, Level.One) +
                              FirstHashTableAddress +
                              level1Off + blockNum%0xAA*0x18;
                    Xio.Position = pos + 0x14;
                    hashAddr += (uint) ((Xio.ReadUInt8() & 0x40) << 6);
                    break;
            }
            return hashAddr;
        }

        private uint GetHashTableSkipSize(uint tableAddress)
        {
            // convert the address to a true block number
            var trueBlockNumber = (tableAddress - FirstHashTableAddress) >> 0xC;

            // check if it's the first hash table
            if (trueBlockNumber == 0)
                return (uint) (0x1000 << (int) PackageSex);

            // check if it's the level 2 table, or above
            if (trueBlockNumber == BlockStep[1])
                return (uint) (0x3000 << (int) PackageSex);
            if (trueBlockNumber > BlockStep[1])
                return (uint) (trueBlockNumber - (BlockStep[1] + (1 << (int) PackageSex)));

            // check if it's at a level 1 table
            if (trueBlockNumber == BlockStep[0] || trueBlockNumber%BlockStep[1] == 0)
                return (uint) (0x2000 << (int) PackageSex);

            // otherwise, assume it's at a level 0 table
            return (uint) (0x1000 << (int) PackageSex);
        }

        private HashTable GetLevelNHashTable(uint index, Level lvl)
        {
            var toReturn = new HashTable
            {
                Level = lvl,
                TrueBlockNumber = ComputeLevelNBackingHashBlockNumber(index*
                                                                      DataBlocksPerHashTreeLevel[(int) lvl], lvl),
                EntryCount = GetHashTableEntryCount(index, lvl)
            };

            var baseHashAddress = GetHashTableAddress(index, lvl);
            ////switch ((uint)lvl)
            ////{
            ////    case 0:
            ////        baseHashAddress += (uint)((Header.StfsVolumeDescriptor.blockSeperation & 2) << 0xB);
            ////        break;
            ////    case 1:
            ////        baseHashAddress += (uint)(((byte)TopTable.Entries[toReturn.TrueBlockNumber / 0xAA].Status & 0x40) << 6);
            ////        break;
            ////    case 2:
            ////        var level1Off = (uint)(((byte)TopTable.Entries[toReturn.TrueBlockNumber / 0x70E4].Status & 0x40) << 6);
            ////        uint pos = ((ComputeLevelNBackingHashBlockNumber((toReturn.TrueBlockNumber << 0xC), Level.One) +
            ////                     FirstHashTableAddress +
            ////                     level1Off) + ((toReturn.TrueBlockNumber % 0xAA) * 0x18));
            ////        Xio.Position = (pos + 0x14);
            ////        baseHashAddress += (uint)((Xio.ReadUInt8() & 0x40) << 6);
            ////        break;
            ////}

            // compute base address of the hash table
            //uint baseHashAddress = ((toReturn.TrueBlockNumber << 0xC) + FirstHashTableAddress);
            //uint baseHashAddress = GetHashEntryOfBlock(toReturn.TrueBlockNumber, lvl);
            //// adjust the hash address
            //if (lvl < 0 || lvl > TopLevel)
            //    throw new Exception("STFS: Invalid level.\n");
            //if (lvl == TopLevel)
            //    return TopTable;
            //if (lvl + 1 == TopLevel)
            //{
            //    //add the hash offset to the base address so we use the correct table
            //    //baseHashAddress += ((uint) ((byte) TopTable.Entries[index].Status & 0x40) << 6);

            //    // calculate the number of entries in the requested table
            //    if (index + 1 == TablesPerLevel[(int)lvl])
            //        toReturn.EntryCount = (lvl == Level.Zero)
            //            ? Header.StfsVolumeDescriptor.allocatedBlockCount % 0xAA
            //            : TablesPerLevel[(int)lvl - 1] % 0xAA;
            //    else
            //        toReturn.EntryCount = 0xAA;
            //}
            //else
            //{
            //    //if (Cached.TrueBlockNumber != ComputeLevelNBackingHashBlockNumber(index * 0xAA, Level.One))
            //    //    Cached = GetLevelNHashTable(index % 0xAA, Level.One);
            //    //baseHashAddress += (uint)(((byte)Cached.Entries[index % 0xAA].Status & 0x40) << 6);

            //    // calculate the number of entries in the requested table
            //    if (index + 1 == TablesPerLevel[(int)lvl])
            //        toReturn.EntryCount = Header.StfsVolumeDescriptor.allocatedBlockCount % 0xAA;
            //    else
            //        toReturn.EntryCount = 0xAA;
            //}
            //seek to the hash table requested
            toReturn.AddressInFile = baseHashAddress;
            Xio.Position = baseHashAddress;
            toReturn.Entries = new HashEntry[toReturn.EntryCount];
            for (uint i = 0; i < toReturn.EntryCount; i++)
            {
                var hash = Xio.ReadBytes(0x14);
                toReturn.Entries[i] = new HashEntry(Xio.ReadUInt32()) {BlockHash = hash, CurrentBlock = i};
            }

            return toReturn;
        }

        private uint GetHashTableEntryCount(uint index, Level lvl)
        {
            if (lvl < 0 || lvl > TopLevel)
                throw new Exception("STFS: Invalid level.\n");

            if (lvl == TopLevel)
                return TopTable.EntryCount;
            if (lvl + 1 == TopLevel)
            {
                if (index + 1 == TablesPerLevel[(int) lvl])
                    return lvl == Level.Zero
                        ? Header.StfsVolumeDescriptor.allocatedBlockCount%0xAA
                        : TablesPerLevel[(int) lvl - 1]%0xAA;
                return 0xAA;
            }
            if (index + 1 == TablesPerLevel[(int) lvl])
                return Header.StfsVolumeDescriptor.allocatedBlockCount%0xAA;
            return 0xAA;
        }

        private byte[] ExtractBlock(uint blockNum, int length)
        {
            if (blockNum >= Header.StfsVolumeDescriptor.allocatedBlockCount)
                throw new Exception("STFS: Reference to illegal block number.\n");

            // check for an invalid block length
            if (length > 0x1000)
                throw new Exception("STFS: length cannot be greater 0x1000.\n");

            // go to the block's position
            Xio.Position = BlockToAddress(blockNum);

            // read the data, and return
            return Xio.ReadBytes(length, false);
        }

        private StfsEntry GetEntry(List<string> locationOfFile, StfsDirectory dir, bool checkFolders)
        {
            while (true)
            {
                if (!dir.ListingRead)
                    dir.ReadFileListing(this);
                // check to see if this is our last call
                if (locationOfFile.Count == 1)
                {
                    var tfile = dir.Files.FirstOrDefault(x => x.Name.ToLower() == locationOfFile[0].ToLower());
                    if (tfile == null && checkFolders)
                        return dir.Directories.FirstOrDefault(x => x.Name.ToLower() == locationOfFile[0].ToLower());
                    return tfile;
                }
                if (locationOfFile.Count > 1)
                {
                    // find the next folder
                    var root = dir.Directories.FirstOrDefault(x => x.Name.ToLower() == locationOfFile[0].ToLower());
                    if (root == null)
                        return null;
                    locationOfFile.RemoveAt(0);
                    dir = root;
                    continue;
                }
                return null;
            }
        }

        private void SwapTable(uint index, Level lvl)
        {
            // only one table per set in female packages
            if (PackageSex == Sex.StfsFemale)
                return;
            if (index >= TablesPerLevel[(int) lvl] || lvl > Level.Two)
                throw new Exception("STFS: Invaid parameters for swapping table.\n");

            // read in all the status's so that when we swap tables, the package isn't messed up
            var entryCount = GetHashTableEntryCount(index, lvl);
            var tableStatuses = new uint[entryCount];

            // set the io to the beginning of the table
            var tablePos = GetHashTableAddress(index, lvl) + 0x14;
            Xio.Position = tablePos;

            for (uint i = 0; i < entryCount; i++)
            {
                tableStatuses[i] = Xio.ReadUInt32();
                Xio.Position = tablePos + i*0x18;
            }

            // if the level requested to be swapped is the top level, we need to invert the '2' bit of the block seperation
            if (lvl == TopTable.Level)
            {
                Header.StfsVolumeDescriptor.blockSeperation ^= 2;
                Header.WriteVolumeDescriptor();
            }
            else
            {
                var statusPosition = GetTableHashAddress(index, lvl) + 0x14;

                // read the status of the requested hash table
                Xio.Position = statusPosition;
                var status = Xio.ReadUInt8();

                // invert the table used
                status ^= 0x40;

                // Write it back to the table
                Xio.Position = statusPosition;
                Xio.WriteUInt8(status);
            }

            // retrieve the table address again since we swapped it
            tablePos = GetHashTableAddress(index, lvl) + 0x14;
            Xio.Position = tablePos;

            // Write all the statuses to the other table
            for (uint i = 0; i < entryCount; i++)
            {
                Xio.WriteUInt32(tableStatuses[i]);
                Xio.Position = tablePos + i*0x18;
            }
        }

        private uint GetHashTableAddress(uint index, Level lvl)
        {
            // get the base address of the hash table requested
            var baseAddress = GetBaseHashTableAddress(index, lvl);

            // only one table per index in female packages
            if (PackageSex == Sex.StfsFemale)
                return baseAddress;

            // if the level requested is the top, then we need to reference the '2' bit of the block seperation
            if (lvl == TopTable.Level)
                return (uint) (baseAddress + ((Header.StfsVolumeDescriptor.blockSeperation & 2) << 0xB));
            Xio.Position = GetTableHashAddress(index, lvl) + 0x14;
            return (uint) (baseAddress + ((Xio.ReadUInt8() & 0x40) << 6));
        }

        private uint GetBaseHashTableAddress(uint index, Level lvl)
        {
            return (ComputeLevelNBackingHashBlockNumber(index*DataBlocksPerHashTreeLevel[(int) lvl],
                lvl) << 0xC) + FirstHashTableAddress;
        }

        private void SetBlockStatus(uint blockNum, BlockStatusLevelZero status)
        {
            if (blockNum >= Header.StfsVolumeDescriptor.allocatedBlockCount)
                throw new Exception("STFS: Reference to illegal block number.\n");

            var statusAddress = GetHashAddressOfBlock(blockNum) + 0x14;
            Xio.Position = statusAddress;
            Xio.WriteUInt8((byte) status);
        }

        private byte[] HashBlock(byte[] block)
        {
            return SHA1.Create().ComputeHash(block, 0, block.Length);
        }

        private byte[] BuildTableInMemory(HashTable table)
        {
            var data = new byte[0x1000];
            using (var io = new StreamIO(data, true))
            {
                for (uint i = 0; i < table.EntryCount; i++)
                {
                    // copy the hash over
                    if (table.Entries[i].Flags == 0x80000232)
                        Console.WriteLine("Here");
                    io.WriteBytes(table.Entries[i].BlockHash);
                    io.WriteUInt32(table.Entries[i].Flags);
                }
            }
            return data;
        }

        private void WriteFileListing()
        {
            var x1 = new List<StfsFile>();
            var x2 = new List<StfsDirectory>();
            WriteFileListing(false, ref x1, ref x2);
        }

        private void WriteFileListing(bool uselist, ref List<StfsFile> files,
            ref List<StfsDirectory> dirs)
        {
            // get the raw file listing

            if (!uselist)
            {
                files = new List<StfsFile>();
                dirs = new List<StfsDirectory>();
                GenerateRawFileListing(Root, ref files, ref dirs);
            }


            // initialize the folders map (used in new path indicators)
            var folders = new int[0xFFFF];
            folders[folders.Length - 1] = 0xFFFF;

            bool alwaysAllocate = false, firstCheck = true;

            // go to the block where the file listing begins (for overwriting)
            var block = (uint) Header.StfsVolumeDescriptor.fileTableBlockNum;
            Xio.Position = BlockToAddress(block);

            var outFileSize = files.Count;

            // add all folders to the folders map
            for (var i = 0; i < dirs.Count; i++)
                folders[dirs[i].EntryIndex] = i;

            // Write the folders to the listing
            for (var i = 0; i < dirs.Count; i++)
            {
                // check to see if we need to go to the next block
                if (firstCheck)
                    firstCheck = false;
                else if ((i + 1)%0x40 == 0)
                {
                    // check if we need to allocate a new block
                    int nextBlock;
                    if (alwaysAllocate)
                    {
                        nextBlock = AllocateBlock();

                        // if so, set the current block pointing to the next one
                        SetNextBlock(block, nextBlock);
                    }
                    else
                    {
                        // see if a block was already allocated with the previous table
                        nextBlock = (int) GetHashEntryOfBlock(block).NextBlock;

                        // if not, allocate one and make it so it always allocates
                        if (nextBlock == 0xFFFFFF)
                        {
                            nextBlock = AllocateBlock();
                            SetNextBlock(block, nextBlock);
                            alwaysAllocate = true;
                        }
                    }

                    // go to the next block position
                    block = (uint) nextBlock;
                    Xio.Position = BlockToAddress(block);
                }

                // set the correct path indicator
                dirs[i].PathIndicator = (ushort) folders[dirs[i].PathIndicator];

                // Write the file (folder) entry to file
                WriteEntry(dirs[i]);
            }

            // same as above
            var outFoldersAndFilesSize = outFileSize + dirs.Count;
            for (var i = outFileSize; i < outFoldersAndFilesSize; i++)
            {
                if (firstCheck)
                    firstCheck = false;
                else if (i%0x40 == 0)
                {
                    int nextBlock;
                    if (alwaysAllocate)
                    {
                        nextBlock = AllocateBlock();
                        SetNextBlock(block, nextBlock);
                    }
                    else
                    {
                        nextBlock = (int) GetHashEntryOfBlock(block).NextBlock;
                        if (nextBlock == 0xFFFFFF)
                        {
                            nextBlock = AllocateBlock();
                            SetNextBlock(block, nextBlock);
                            alwaysAllocate = true;
                        }
                    }

                    block = (uint) nextBlock;
                    Xio.Position = BlockToAddress(block);
                }

                files[i - outFileSize].PathIndicator = (ushort) dirs[files[i - outFileSize].PathIndicator].EntryIndex;
                WriteEntry(files[i - outFileSize]);
            }

            // Write remaining null bytes
            var remainingEntries = (uint) (outFoldersAndFilesSize%0x40);
            var remainer = 0;
            if (remainingEntries > 0)
                remainer = (int) ((0x40 - remainingEntries)*0x40);
            var nullBytes = new byte[remainer];
            Xio.WriteBytes(nullBytes);

            // update the file table block count and Write it to file
            Header.StfsVolumeDescriptor.fileTableBlockCount = (ushort) (outFoldersAndFilesSize/0x40 + 1);
            if (outFoldersAndFilesSize%0x40 == 0 && outFoldersAndFilesSize != 0)
                Header.StfsVolumeDescriptor.fileTableBlockCount--;
            Header.WriteVolumeDescriptor();
            Root.ReadFileListing(this);
        }

        private void SetNextBlock(uint blockNum, int nextBlockNum)
        {
            if (blockNum >= Header.StfsVolumeDescriptor.allocatedBlockCount)
                throw new Exception("STFS: Reference to illegal block number.\n");

            var hashLoc = GetHashAddressOfBlock(blockNum) + 0x14;
            Xio.Position = hashLoc;
            var ent = new HashEntry(Xio.ReadUInt32());
            ent.NextBlock = (uint) nextBlockNum;
            Xio.WriteUInt32(ent.Flags);

            if (TopLevel == Level.Zero)
                TopTable.Entries[blockNum].NextBlock = (uint) nextBlockNum;

            Xio.Flush();
        }

        private void WriteEntry(StfsEntry entry)
        {
            // update the name length so it matches the string
            entry.NameLen = (byte) entry.Name.Length;

            if (entry.NameLen > 0x28)
                throw new Exception("STFS: File entry name length cannot be greater than 40(0x28) characters.\n");

            // put the flags and name length into one byte
            var nameLengthAndFlags = (byte) (entry.NameLen | (entry.Flags << 6));

            // Write the entry
            var buffer = new byte[0x28];
            Array.Copy(Encoding.ASCII.GetBytes(entry.Name), 0, buffer, 0, entry.NameLen);
            Xio.Position = entry.FileEntryAddress;
            Xio.WriteBytes(buffer);
            Xio.WriteUInt8(nameLengthAndFlags);
            Xio.WriteInt24(entry.BlocksForFile, false);
            Xio.WriteInt24(entry.BlocksForFile, false);
            Xio.WriteInt24(entry.StartingBlockNum, false);
            Xio.WriteUInt16(entry.PathIndicator);
            Xio.WriteUInt32(entry.FileSize);
            Xio.WriteUInt32(entry.CreatedTimeStamp);
            Xio.WriteUInt32(entry.AccessTimeStamp);
        }

        private void RemoveFile(string pathInPackage)
        {
            var ent = GetEntry(pathInPackage, false);
            if (ent == null || ent.NameLen == 0 || ent.IsDirectory)
                throw new Exception("STFS : File does not exist at : " + pathInPackage + "\n");
            RemoveFile(new StfsFile(ent));
        }

        private int AllocateBlock()
        {
            // reset the cached table
            Cached.AddressInFile = 0;
            Cached.EntryCount = 0;
            Cached.Level = Level.Lt;
            Cached.TrueBlockNumber = 0xFFFFFFFF;

            uint lengthToWrite = 0xFFF;

            // update the allocated block count
            Header.StfsVolumeDescriptor.allocatedBlockCount++;

            // recalculate the hash table counts to see if we need to make any new tables
            var recalcTablesPerLevel = new uint[3];
            recalcTablesPerLevel[0] =
                (uint)
                    (Header.StfsVolumeDescriptor.allocatedBlockCount/0xAA +
                     (Header.StfsVolumeDescriptor.allocatedBlockCount%0xAA != 0
                         ? 1
                         : 0));
            recalcTablesPerLevel[1] = (uint) (recalcTablesPerLevel[0]/0xAA + (recalcTablesPerLevel[0]%0xAA != 0
                                                                              &&
                                                                              Header.StfsVolumeDescriptor
                                                                                  .allocatedBlockCount > 0xAA
                ? 1
                : 0));
            recalcTablesPerLevel[2] = (uint) (recalcTablesPerLevel[1]/0xAA + (recalcTablesPerLevel[1]%0xAA != 0
                                                                              &&
                                                                              Header.StfsVolumeDescriptor
                                                                                  .allocatedBlockCount > 0x70E4
                ? 1
                : 0));

            // allocate memory for hash tables if needed
            for (var i = 2; i >= 0; i--)
            {
                if (recalcTablesPerLevel[i] != TablesPerLevel[i])
                {
                    lengthToWrite += (uint) ((byte) PackageSex + 1)*0x1000;
                    TablesPerLevel[i] = recalcTablesPerLevel[i];

                    // update top level hash table if needed
                    if (i + 1 == (byte) TopLevel)
                    {
                        TopTable.EntryCount++;
                        TopTable.Entries[TopTable.EntryCount - 1].Status = 0;
                        TopTable.Entries[TopTable.EntryCount - 1].NextBlock = 0;

                        // Write it to the file
                        Xio.Position = TopTable.AddressInFile + (TablesPerLevel[i] - 1)*0x18 + 0x15;
                        Xio.WriteInt24(0xFFFFFF);
                    }
                }
            }

            // allocate the necessary memory
            Xio.Position = lengthToWrite;
            Xio.WriteUInt8(0);

            // if the top level changed, then we need to re-load the top table
            var newTop = CalcualateTopLevel();
            if (TopLevel != newTop)
            {
                TopLevel = newTop;
                TopTable.Level = TopLevel;

                var blockOffset = (uint) (Header.StfsVolumeDescriptor.blockSeperation & 2);
                Header.StfsVolumeDescriptor.blockSeperation &= 0xFD;
                TopTable.AddressInFile = GetHashTableAddress(0, TopLevel);
                TopTable.EntryCount = 2;
                TopTable.TrueBlockNumber = ComputeLevelNBackingHashBlockNumber(0, TopLevel);
                TopTable.Entries = new HashEntry[TopTable.EntryCount];
                TopTable.Entries[0].Status = (BlockStatusLevelZero) (blockOffset << 5);
                Xio.Position = TopTable.AddressInFile + 0x14;
                Xio.WriteUInt8((byte) TopTable.Entries[0].Status);

                // clear the top hash offset
                Header.StfsVolumeDescriptor.blockSeperation &= 0xFD;
            }

            // Write the block status
            Xio.Position = GetHashAddressOfBlock(Header.StfsVolumeDescriptor.allocatedBlockCount - 1) +
                           0x14;
            Xio.WriteUInt8((byte) BlockStatusLevelZero.Allocated);

            if (TopLevel == Level.Zero)
            {
                TopTable.EntryCount++;
                TopTable.Entries[Header.StfsVolumeDescriptor.allocatedBlockCount - 1].Status =
                    BlockStatusLevelZero.Allocated;
                TopTable.Entries[Header.StfsVolumeDescriptor.allocatedBlockCount - 1].NextBlock = 0xFFFFFF;
            }

            // terminate the chain
            Xio.WriteInt24(0xFFFFFF);

            Header.WriteVolumeDescriptor();
            return (int) (Header.StfsVolumeDescriptor.allocatedBlockCount - 1);
        }

        private uint GetBlocksUntilNextHashTable(uint currentBlock)
        {
            return ComputeLevelNBackingHashBlockNumber(currentBlock + BlockStep[0] - ((BlockToAddress(
                currentBlock) - FirstHashTableAddress) >> 0xC), Level.Zero);
        }

        private int AllocateBlocks(uint blockCount)
        {
            var returnValue = (int) Header.StfsVolumeDescriptor.allocatedBlockCount;

            // figure out how far away the next hash table set is
            var blocksUntilTable = GetBlocksUntilNextHashTable(
                Header.StfsVolumeDescriptor.allocatedBlockCount);

            // create a hash block of all statuses set to allocated, for fast writing
            var allocatedHashBlock = new byte[0x1000];
            for (uint i = 0; i < 0xAA; i++)
                allocatedHashBlock[0x14 + i*0x18] = 0x80;

            // allocate the amount before the hash table

            // allocate the memory in the file
            Xio.Position = ((blockCount <= blocksUntilTable ? blockCount : blocksUntilTable) << 0xC) - 1;
            Xio.WriteUInt8(0);

            // set blocks to allocated in hash table
            Xio.Position = BlockToAddress(Header.StfsVolumeDescriptor.allocatedBlockCount -
                                          (0xAA - blocksUntilTable)) - (0x1000 << (int) PackageSex) +
                           Header.StfsVolumeDescriptor.allocatedBlockCount*0x18;
            Xio.WriteBytes(allocatedHashBlock, 0, (int) (blocksUntilTable*0x18));

            // update the allocated block count
            Header.StfsVolumeDescriptor.allocatedBlockCount += blockCount <= blocksUntilTable
                ? blockCount
                : blocksUntilTable;

            // allocate memory the hash table
            Xio.Position = GetHashTableSkipSize(Header.StfsVolumeDescriptor.allocatedBlockCount) - 1;
            Xio.WriteUInt8(0);

            blockCount -= blocksUntilTable;

            // allocate all of the full sets
            while (blockCount >= 0xAA)
            {
                // allocate the memory in the file
                Xio.Position = 0xAA000 + GetHashTableSkipSize(Header.StfsVolumeDescriptor.allocatedBlockCount +
                                                              0xAA) - 1;
                Xio.WriteUInt8(0);

                // set all the blocks to allocated
                Xio.Position = BlockToAddress(Header.StfsVolumeDescriptor.allocatedBlockCount) -
                               (0x1000 << (int) PackageSex);
                Xio.WriteBytes(allocatedHashBlock, 0, 0x1000);

                // update the values
                Header.StfsVolumeDescriptor.allocatedBlockCount += 0xAA;
                blockCount -= 0xAA;
            }

            if (blockCount > 0)
            {
                // allocate the extra
                Xio.Position = GetHashTableSkipSize(Header.StfsVolumeDescriptor.allocatedBlockCount + 0xAA) +
                               (blockCount << 0xC) - 1;
                Xio.WriteUInt8(0);

                // set all the blocks to allocated
                Xio.Position = BlockToAddress(Header.StfsVolumeDescriptor.allocatedBlockCount) -
                               (0x1000 << (int) PackageSex);
                Xio.WriteBytes(allocatedHashBlock, 0, (int) (blockCount*0x18));
            }

            Header.WriteVolumeDescriptor();

            // if the top level changed, then we need to re-load the top table
            var newTop = CalcualateTopLevel();
            if (TopLevel != newTop)
            {
                TopLevel = newTop;
                TopTable.Level = TopLevel;

                var blockOffset = (uint) (Header.StfsVolumeDescriptor.blockSeperation & 2);
                Header.StfsVolumeDescriptor.blockSeperation &= 0xFD;
                TopTable.AddressInFile = GetHashTableAddress(0, TopLevel);
                TopTable.EntryCount = 2;

                // clear the top table
                //memset(TopTable.Entries, 0, sizeof(HashEntry) * 0xAA);
                TopTable.Entries = new HashEntry[0xAA];
                TopTable.Entries[0].Status = (BlockStatusLevelZero) (blockOffset << 5);
                Xio.Position = TopTable.AddressInFile + 0x14;
                Xio.WriteUInt8((byte) TopTable.Entries[0].Status);

                // clear the top hash offset
                Header.StfsVolumeDescriptor.blockSeperation &= 0xFD;
            }

            return returnValue;
        }

        private void UpdateEntry(string pathInPackage, StfsEntry entry)
        {
            var ent = GetEntry(pathInPackage.Split('\\').ToList(), Root, true);
            if (ent != null && ent.NameLen > 0)
            {
                entry.CopyTo(ent);
            }
        }

        private void GenerateRawFileListing(StfsDirectory dir, ref List<StfsFile> files,
            ref List<StfsDirectory> dirs)
        {
            if (!dir.ListingRead)
                dir.ReadFileListing(this);
            if (dir.Files.Length > 0)
                files.AddRange(dir.Files);
            if (dir.Directories.Length > 0)
            {
                dirs.AddRange(dir.Directories);
                foreach (var t in dir.Directories)
                    GenerateRawFileListing(t, ref files, ref dirs);
            }
        }

        #endregion

        #region Public Methods

        public StfsDirectory FindDirectory(List<string> locationOfDirectory, StfsDirectory start)
        {
            if (locationOfDirectory.Count == 0)
                return Root;

            var finalLoop = locationOfDirectory.Count == 1;
            foreach (var t in start.Directories)
            {
                if (t.Name == locationOfDirectory[0])
                {
                    locationOfDirectory.RemoveAt(0);
                    if (finalLoop)
                        return t;
                    return FindDirectory(locationOfDirectory, t);
                }
            }
            return null;
        }

        public StfsFile InjectFile(string filepath, string pathInPackage)
        {
            if (FileExists(pathInPackage))
                throw new Exception("STFS: File already exists in the package.\n");

            // split the string and open a io
            var split = pathInPackage.Split('\\').ToList();
            var xentry = GetEntry(split, Root, false);
            if (xentry == null || xentry.NameLen == 0)
                throw new Exception("STFS: " + pathInPackage + " already exists in package.\n");
            StfsDirectory direntry = null;
            var fileName = split[split.Count - 1];
            if (split.Count > 1)
            {
                split.RemoveAt(split.Count - 1);
                xentry = GetEntry(split, Root, true);
                if (xentry == null || xentry.NameLen == 0 || !xentry.IsDirectory)
                    throw new Exception("STFS: The given folder could not be found.\n");
                direntry = new StfsDirectory(xentry);
            }
            else direntry = Root;

            var fileIn = new StreamIO(filepath, true);

            // set up the entry
            var entry = new StfsEntry(this);
            entry.Name = fileName;

            if (fileName.Length > 0x28)
                throw new Exception("STFS: File entry name length cannot be greater than 40(0x28) characters.\n");

            entry.FileSize = (uint) fileIn.Length;
            entry.Flags = 1;
            entry.PathIndicator = (ushort) xentry.EntryIndex;
            entry.StartingBlockNum = 0xFFFFFF;
            entry.BlocksForFile = (int) (((fileIn.Length + 0xFFF) & 0xFFFFFFF000) >> 0xC);
            entry.CreatedTimeStamp = (uint) DateTime.Now.Millisecond;
            entry.AccessTimeStamp = entry.CreatedTimeStamp;
            var fileSize = (int) fileIn.Length;
            var block = 0;
            uint prevBlock = 0xFFFFFF;
            while (fileIn.Length >= 0x1000)
            {
                block = AllocateBlock();
                if (entry.StartingBlockNum == 0xFFFFFF)
                    entry.StartingBlockNum = block;

                if (prevBlock != 0xFFFFFF)
                    SetNextBlock(prevBlock, block);

                prevBlock = (uint) block;

                // read the data;
                var data = fileIn.ReadBytes(0x1000);

                Xio.Position = BlockToAddress((uint) block);
                Xio.WriteBytes(data, 0, 0x1000);

                fileSize -= 0x1000;
            }

            if (fileSize != 0)
            {
                block = AllocateBlock();

                if (entry.StartingBlockNum == 0xFFFFFF)
                    entry.StartingBlockNum = block;

                if (prevBlock != 0xFFFFFF)
                    SetNextBlock(prevBlock, block);

                var data = fileIn.ReadBytes(fileSize);
                Array.Resize(ref data, 0x1000);
                Xio.Position = BlockToAddress((uint) block);
                Xio.WriteBytes(data);
            }
            fileIn.Close();
            direntry.AddFileToList(new StfsFile(entry));
            SetNextBlock((uint) block, 0xFFFFFF);
            WriteFileListing();
            if (TopLevel == Level.Zero)
            {
                Xio.Position = TopTable.AddressInFile;

                TopTable.EntryCount = Header.StfsVolumeDescriptor.allocatedBlockCount;

                for (uint i = 0; i < TopTable.EntryCount; i++)
                {
                    var hash = Xio.ReadBytes(0x14);
                    TopTable.Entries[i] = new HashEntry(Xio.ReadUInt32());
                    TopTable.Entries[i].BlockHash = hash;
                }
            }
            return new StfsFile(entry);
        }

        public StfsFile InjectData(byte[] data, uint length, string pathInPackage)
        {
            if (FileExists(pathInPackage))
                throw new Exception("STFS: File already exists in the package.\n");

            // split the string and open a io
            var split = pathInPackage.Split('\\').ToList();
            StfsDirectory folder = null;

            var size = split.Count;
            string fileName;
            if (size > 1)
            {
                // get the name
                fileName = split[size - 1];
                split.RemoveAt(size - 1);

                // find the directory we'd like to inject to
                var ent = GetEntry(split, Root, true);
                if (ent == null || ent.NameLen == 0)
                    throw new Exception("STFS: The given folder could not be found.\n");
                folder = new StfsDirectory(ent);
            }
            else
            {
                fileName = pathInPackage;
                folder = Root;
            }

            var fileSize = length;

            // set up the entry
            var entry = new StfsEntry(this);
            entry.Name = fileName;
            entry.FileSize = fileSize;
            entry.Flags = 1;
            entry.PathIndicator = (ushort) folder.EntryIndex;
            entry.StartingBlockNum = 0xFFFFFF;
            entry.BlocksForFile = (int) (((fileSize + 0xFFF) & 0xFFFFFFF000) >> 0xC);

            var block = 0;
            var prevBlock = 0xFFFFFF;
            uint counter = 0;
            while (fileSize >= 0x1000)
            {
                block = AllocateBlock();

                if (entry.StartingBlockNum == 0xFFFFFF)
                    entry.StartingBlockNum = block;

                if (prevBlock != 0xFFFFFF)
                    SetNextBlock((uint) prevBlock, block);

                prevBlock = block;

                // read the data

                var dataBlock = new byte[0x1000];
                Array.Copy(data, counter++*0x1000, dataBlock, 0, 0x1000);
                Xio.Position = BlockToAddress((uint) block);
                Xio.WriteBytes(dataBlock);
                fileSize -= 0x1000;
            }

            if (fileSize != 0)
            {
                block = AllocateBlock();

                if (entry.StartingBlockNum == 0xFFFFFF)
                    entry.StartingBlockNum = block;

                if (prevBlock != 0xFFFFFF)
                    SetNextBlock((uint) prevBlock, block);

                var dataBlock = new byte[0x1000];
                Array.Copy(data, length - fileSize, dataBlock, 0, fileSize);
                Xio.Position = BlockToAddress((uint) block);
                Xio.WriteBytes(dataBlock);
            }

            SetNextBlock((uint) block, 0xFFFFFF);

            folder.AddFileToList(new StfsFile(entry));
            WriteFileListing();

            if (TopLevel == Level.Zero)
            {
                Xio.Position = TopTable.AddressInFile;

                TopTable.EntryCount = Header.StfsVolumeDescriptor.allocatedBlockCount;

                for (uint i = 0; i < TopTable.EntryCount; i++)
                {
                    var hash = Xio.ReadBytes(0x14);
                    TopTable.Entries[i] = new HashEntry(Xio.ReadUInt32());
                    TopTable.Entries[i].BlockHash = hash;
                }
            }

            return new StfsFile(entry);
        }

        public void ReplaceFile(Stream input, StfsEntry entry)
        {
            if (entry == null || entry.NameLen == 0)
                throw new Exception("STFS: File doesn't exists in the package.\n");

            var fileIn = new StreamIO(input, false);
            var fileSize = (uint) fileIn.Length;
            // set up the entry
            entry.FileSize = fileSize;
            entry.BlocksForFile = (int) (((fileSize + 0xFFF) & 0xFFFFFFF000) >> 0xC);

            var block = (uint) entry.StartingBlockNum;
            Xio.Position = BlockToAddress(block);

            var fullReads = fileSize/0x1000;
            bool first = true, alwaysAllocate = false;

            // Write the folders to the listing
            for (uint i = 0; i < fullReads; i++)
            {
                if (!first)
                {
                    // check if we need to allocate a new block
                    int nextBlock;
                    if (alwaysAllocate)
                    {
                        nextBlock = AllocateBlock();

                        // if so, set the current block pointing to the next one
                        SetNextBlock(block, nextBlock);
                    }
                    else
                    {
                        // see if a block was already allocated with the previous table
                        nextBlock = (int) GetHashEntryOfBlock(block).NextBlock;

                        // if not, allocate one and make it so it always allocates
                        if (nextBlock == 0xFFFFFF)
                        {
                            nextBlock = AllocateBlock();
                            SetNextBlock(block, nextBlock);
                            alwaysAllocate = true;
                        }
                    }

                    // go to the next block position
                    block = (uint) nextBlock;
                    Xio.Position = BlockToAddress(block);
                }
                else
                    first = false;

                var toWrite = fileIn.ReadBytes(0x1000);
                // Write the data
                Xio.WriteBytes(toWrite);
            }

            var remainder = fileSize%0x1000;
            if (remainder != 0)
            {
                if (!first)
                {
                    // check if we need to allocate a new block
                    int nextBlock;
                    if (alwaysAllocate)
                    {
                        nextBlock = AllocateBlock();

                        // if so, set the current block pointing to the next one
                        SetNextBlock(block, nextBlock);
                    }
                    else
                    {
                        // see if a block was already allocated with the previous table
                        nextBlock = (int) GetHashEntryOfBlock(block).NextBlock;

                        // if not, allocate one and make it so it always allocates
                        if (nextBlock == 0xFFFFFF)
                        {
                            nextBlock = AllocateBlock();
                            SetNextBlock(block, nextBlock);
                            alwaysAllocate = true;
                        }
                    }

                    block = (uint) nextBlock;
                }
                // go to the next block position
                Xio.Position = BlockToAddress(block);
                var toWrite = fileIn.ReadBytes((int) remainder);
                var buffer = new byte[0x1000];
                Array.Copy(toWrite, 0, buffer, 0, toWrite.Length);
                Xio.WriteBytes(buffer);
            }

            SetNextBlock(block, 0xFFFFFF);
            entry.Flags &= 0x2;
            WriteEntry(entry);

            if (TopLevel == Level.Zero)
            {
                Xio.Position = TopTable.AddressInFile;

                for (uint i = 0; i < TopTable.EntryCount; i++)
                {
                    var hash = Xio.ReadBytes(0x14);
                    TopTable.Entries[i] = new HashEntry(Xio.ReadUInt32());
                    TopTable.Entries[i].BlockHash = hash;
                }
            }
        }

        public void ReplaceFile(string path, string pathInPackage)
        {
            var ent = GetEntry(pathInPackage, false);
            if (ent == null || ent.NameLen == 0)
                throw new Exception("STFS: The given path could not be found.\n");
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
            {
                ReplaceFile(fs, new StfsFile(ent));
            }
        }

        public void ReplaceFile(Stream file, string pathInPackage)
        {
            var ent = GetEntry(pathInPackage, false);
            if (ent == null || ent.NameLen == 0)
                throw new Exception("STFS: The given path could not be found.\n");
            ReplaceFile(file, new StfsFile(ent));
        }

        public void ReplaceFile(byte[] file, string pathInPackage)
        {
            var ent = GetEntry(pathInPackage, false);
            if (ent == null || ent.NameLen == 0)
                throw new Exception("STFS: The given path could not be found.\n");
            using (var ms = new MemoryStream(file))
            {
                ReplaceFile(ms, new StfsFile(ent));
            }
        }

        public void RenameEntry(string newName, string pathInPackage)
        {
            var ent = GetEntry(pathInPackage, true);
            if (ent == null || ent.NameLen == 0)
                throw new Exception("STFS: The given path could not be found.\n");
            RenameEntry(ent, newName);
        }

        public void RenameEntry(StfsEntry ent, string newName)
        {
            ent.Name = newName;
            WriteEntry(ent);
        }

        public void CreateFolder(string pathInPackage)
        {
            // split the string and open a io
            var split = pathInPackage.Split('\\').ToList();

            var folder = FindDirectory(split, Root);
            if (folder != null)
                throw new Exception("STFS: Directory already exists in the package.\n");

            var size = split.Count;
            string fileName;
            if (size > 1)
            {
                // get the name
                fileName = split[size - 1];
                split.RemoveAt(size - 1);
                // find the directory we'd like to inject to
                folder = FindDirectory(split, Root);
                if (folder == null)
                    throw new Exception("STFS: The given folder could not be found.\n");
            }
            else
            {
                fileName = pathInPackage;
                folder = Root;
            }

            // set up the entry
            var entry = new StfsEntry(this);
            entry.Name = fileName;

            if (fileName.Length > 0x28)
                throw new Exception("STFS: File entry name length cannot be greater than 40(0x28) characters.\n");

            entry.NameLen = (byte) fileName.Length;
            entry.FileSize = 0;
            entry.Flags = 2;
            entry.PathIndicator = (ushort) folder.EntryIndex;
            entry.StartingBlockNum = 0;
            entry.BlocksForFile = 0;
            entry.CreatedTimeStamp = (uint) DateTime.Now.Ticks;
            entry.AccessTimeStamp = entry.CreatedTimeStamp;

            var newFolder = new StfsDirectory(entry);
            newFolder.ListingRead = true;
            // add the entry to the listing
            folder.AddDirectoryToList(newFolder);
            WriteFileListing();
        }

        public uint GetFileMagic(string pathInPackage)
        {
            var entry = GetEntry(pathInPackage, false);
            if (entry == null)
                return 0;
            return GetFileMagic(entry);
        }

        public uint GetFileMagic(StfsEntry entry)
        {
            if (entry.IsDirectory)
                return 0;
            // make sure the file is at least 4 bytes
            if (entry.FileSize < 4)
                return 0;

            // seek to the begining of the file in the package
            Xio.Position = BlockToAddress((uint) entry.StartingBlockNum);

            // read the magic
            return Xio.ReadUInt32();
        }

        public void ExtractFile(string pathInPackage, string outPath)
        {
            // get the given path's file entry
            var entry = GetEntry(pathInPackage, false);
            if (entry == null || entry.NameLen == 0)
                throw new FileNotFoundException(pathInPackage);
            // extract the file
            ExtractFile(new StfsFile(entry), outPath);
        }

        public byte[] ExtractFile(string pathInPackage)
        {
            // get the given path's file entry
            var entry = GetEntry(pathInPackage, false);
            if (entry == null || entry.NameLen == 0)
                throw new FileNotFoundException(pathInPackage);
            // extract the file
            return ExtractFile(new StfsFile(entry));
        }

        public byte[] ExtractFile(StfsFile entry)
        {
            byte[] xreturn = null;
            using (var ms = new MemoryStream())
            {
                ExtractFile(new StfsFile(entry), ms);
                xreturn = ms.ToArray();
            }
            return xreturn;
        }

        public void ExtractFile(StfsFile entry, Stream outFile)
        {
            try
            {
                var p = new ProgressChangedArg();
                p.Message = "Initializing " + entry.Name;
                if (entry.NameLen == 0)
                    throw new Exception("STFS: File '" + entry.Name + "' doesn't exist in the package.\n");
                // get the file size that we are extracting
                var fileSize = entry.FileSize;
                p.Total = fileSize;
                OnProgressChanged(entry, p);
                // make a special case for files of size 0
                if (fileSize == 0)
                {
                    OnProgressCompleted(entry,
                        new ProgressCompletedArg
                        {
                            Canceled = true,
                            Message = "FileSize can not be Zero!",
                            Result = null
                        });
                    return;
                }
                // check if all the blocks are consecutive
                if ((entry.Flags & 1) == 1 && AllowConsecutive)
                {
                    // allocate 0xAA blocks of memory, for maximum efficiency
                    byte[] buffer;

                    // seek to the begining of the file
                    var startAddress = BlockToAddress((uint) entry.StartingBlockNum);
                    Xio.Position = startAddress;

                    // calculate the number of blocks to read before we hit a table
                    var blockCount = ComputeLevelNBackingHashBlockNumber((uint) entry.StartingBlockNum, Level.Zero) +
                                     BlockStep[0]
                                     - ((startAddress - FirstHashTableAddress) >> 0xC);

                    // pick up the change at the begining, until we hit a hash table
                    if ((uint) entry.BlocksForFile <= blockCount)
                    {
                        p.Message = "Allocating " + entry.FileSize + " bytes of memory";
                        OnProgressChanged(entry, p);
                        buffer = Xio.ReadBytes((int) entry.FileSize);
                        p.Message = "Writing Allocated memory";
                        OnProgressChanged(entry, p);
                        outFile.Write(buffer, 0, buffer.Length);
                        // free the temp buffer
                        buffer = null;
                        return;
                    }
                    buffer = Xio.ReadBytes((int) (blockCount << 0xC));
                    outFile.Write(buffer, 0, buffer.Length);

                    // extract the blocks inbetween the tables
                    var tempSize = entry.FileSize - (blockCount << 0xC);
                    while (tempSize >= 0xAA000)
                    {
                        // skip past the hash table(s)
                        var currentPos = (uint) Xio.Position;
                        Xio.Position = currentPos + GetHashTableSkipSize(currentPos);

                        // read in the 0xAA blocks between the tables
                        buffer = Xio.ReadBytes(0xAA000);

                        // Write the bytes to the out file
                        outFile.Write(buffer, 0, 0xAA000);

                        tempSize -= 0xAA000;
                        blockCount += 0xAA;
                    }

                    // pick up the change at the end
                    if (tempSize != 0)
                    {
                        // skip past the hash table(s)
                        var currentPos = (uint) Xio.Position;
                        Xio.Position = currentPos + GetHashTableSkipSize(currentPos);

                        // read in the extra crap
                        buffer = Xio.ReadBytes((int) tempSize);

                        // Write it to the out file
                        outFile.Write(buffer, 0, (int) tempSize);
                    }

                    // free the temp buffer
                }
                else
                {
                    // generate the block chain which we have to extract
                    var fullReadCounts = fileSize/0x1000;

                    fileSize -= fullReadCounts*0x1000;

                    var block = (uint) entry.StartingBlockNum;

                    // allocate data for the blocks
                    byte[] data;

                    // read all the full blocks the file allocates
                    for (uint i = 0; i < fullReadCounts; i++)
                    {
                        data = ExtractBlock(block, 0x1000);
                        outFile.Write(data, 0, 0x1000);

                        block = GetHashEntryOfBlock(block).NextBlock;
                    }

                    // read the remaining data
                    if (fileSize != 0)
                    {
                        data = ExtractBlock(block, (int) fileSize);
                        outFile.Write(data, 0, (int) fileSize);
                    }
                }
            }
            catch
                (Exception ex)
            {
                OnProgressCompleted(entry,
                    new ProgressCompletedArg {Canceled = false, Error = ex, Message = ex.Message, Result = null});
                if (ProgressCompleted == null)
                    throw;
            }
        }

        public void ExtractFile(StfsFile entry, string filepath)
        {
            using (var x = new FileStream(filepath, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
            {
                ExtractFile(entry, x);
            }
        }

        public StfsEntry GetEntry(string pathInPackage, bool checkFolders)
        {
            var entry =
                GetEntry(pathInPackage.Split('\\').ToList(), Root,
                    checkFolders)
                ;

            if (entry == null || entry.NameLen == 0)
                throw new Exception("STFS: File entry '" + pathInPackage + "' cannot be found in the package.\n");

            return entry;
        }

        public bool FileExists(string pathInPackage)
        {
            var entry = GetEntry(pathInPackage.Split('\\').ToList(), Root, true);
            return entry != null && entry.NameLen != 0;
        }

        public void RemoveFile(StfsFile entry)
        {
            var found = false;

            var files = new List<StfsFile>();
            var folders = new List<StfsDirectory>();
            GenerateRawFileListing(Root, ref files, ref folders);

            // remove the file from the listing
            for (var i = 0; i < files.Count; i++)
                if (files[i].Name == entry.Name && files[i].PathIndicator == entry.PathIndicator)
                {
                    files.RemoveAt(i);
                    found = true;
                    break;
                }

            // make sure the file was found in the package
            if (!found)
                throw new Exception("STFS: File could not be deleted because it doesn't exist in the package.\n");

            // set the status of every allocated block to unallocated
            var blockToDeallocate = (uint) entry.StartingBlockNum;
            while (blockToDeallocate != 0xFFFFFF)
            {
                SetBlockStatus(blockToDeallocate, BlockStatusLevelZero.Unallocated);
                blockToDeallocate = GetHashEntryOfBlock(blockToDeallocate).NextBlock;
            }

            // update the file listing
            WriteFileListing(true, ref files, ref folders);
        }

        public void Rehash()
        {
            var topBuffer = new byte[0x1000];
            uint totalblocks = 0, alloccount = 0, unalloccount = 0;
            switch ((int) TopLevel)
            {
                case 0:
                    // set the position to the first data block in the file
                    Xio.Position = BlockToAddress(0);
                    // iterate through all of the data blocks
                    for (uint i = 0; i < TopTable.EntryCount; i++)
                    {
                        var buffer = new byte[0x1000];
                        if (Xio.Position + 0x1000 <= Xio.Length)
                            buffer = Xio.ReadBytes(0x1000);
                        // hash the block
                        if (TopTable.Entries[i].Status == BlockStatusLevelZero.Unallocated)
                            unalloccount++;
                        else
                            alloccount++;
                        totalblocks++;
                        Array.Copy(TopTable.Entries[i]._Flags, 0, topBuffer, i*0x18 + 0x14, 4);
                        Array.Copy(HashBlock(buffer), 0, topBuffer, i*0x18, 0x14);
                    }

                    break;

                case 1:
                    // loop through all of the level1 hash blocks
                    for (uint i = 0; i < TopTable.EntryCount; i++)
                    {
                        // get the current level0 hash table
                        var level0Table = GetLevelNHashTable(i, 0);

                        // set the position to the first data block in this table
                        //Xio.Position = (BlockToAddress(i * 0xAA));
                        var blockBuffer = new byte[0x1000];
                        // iterate through all of the data blocks this table hashes
                        for (uint x = 0; x < level0Table.EntryCount; x++)
                        {
                            Xio.Position = BlockToAddress(i*0xAA + x);
                            // read in the current data block
                            var buffer = new byte[0x1000];
                            if (Xio.Position + 0x1000 <= Xio.Length)
                                buffer = Xio.ReadBytes(0x1000);
                            // hash the block
                            if (TopTable.Entries[i].Status == BlockStatusLevelZero.Unallocated)
                                unalloccount++;
                            else
                                alloccount++;
                            totalblocks++;
                            Array.Copy(HashBlock(buffer), 0, blockBuffer, x*0x18, 0x14);
                            Array.Copy(level0Table.Entries[x]._Flags, 0, blockBuffer, x*0x18 + 0x14, 4);
                        }
                        // Write the hash table back to the file
                        Xio.Position = level0Table.AddressInFile;
                        Xio.WriteBytes(blockBuffer, 0, 0x1000);
                        if (TopTable.Entries[i].Status == BlockStatusLevelZero.Unallocated)
                            unalloccount++;
                        else
                            alloccount++;
                        totalblocks++;
                        Array.Copy(TopTable.Entries[i]._Flags, 0, topBuffer, i*0x18 + 0x14, 4);
                        Array.Copy(HashBlock(blockBuffer), 0, topBuffer, i*0x18, 0x14);
                    }
                    break;

                case 2:
                    // iterate through all of the level2 tables
                    for (uint i = 0; i < TopTable.EntryCount; i++)
                    {
                        // get the current level1 hash table
                        var level1Table = GetLevelNHashTable(i, Level.One);
                        var blockBuffer1 = new byte[0x1000];
                        // iterate through all of the level0 tables hashed in this table
                        for (uint x = 0; x < level1Table.EntryCount; x++)
                        {
                            // get the current level0 hash table
                            var level0Table = GetLevelNHashTable(i*0xAA + x, Level.Zero);

                            // set the position to the first data block in this table
                            Xio.Position = BlockToAddress(i*0x70E4 + x*0xAA);
                            var blockBuffer2 = new byte[0x1000];
                            // iterate through all of the data blocks hashed in this table
                            for (uint y = 0; y < level0Table.EntryCount; y++)
                            {
                                var buffer = new byte[0x1000];
                                if (Xio.Position + 0x1000 <= Xio.Length)
                                    buffer = Xio.ReadBytes(0x1000);
                                if (level0Table.Entries[y].Status == BlockStatusLevelZero.Unallocated)
                                    unalloccount++;
                                else
                                    alloccount++;
                                totalblocks++;
                                // hash the data block
                                Array.Copy(level0Table.Entries[y]._Flags, 0, blockBuffer2, y*0x18 + 0x14, 4);
                                Array.Copy(HashBlock(buffer), 0, blockBuffer2, y*0x18, 0x14);
                            }

                            // build the table for hashing and writing

                            // Write the hash table back to the file
                            Xio.Position = level0Table.AddressInFile;
                            Xio.WriteBytes(blockBuffer2, 0, 0x1000);
                            if (level1Table.Entries[x].Status == BlockStatusLevelZero.Unallocated)
                                unalloccount++;
                            else
                                alloccount++;
                            totalblocks++;
                            Array.Copy(level1Table.Entries[x]._Flags, 0, blockBuffer1, x*0x18 + 0x14, 4);
                            Array.Copy(HashBlock(blockBuffer2), 0, blockBuffer1, x*0x18, 0x14);
                        }


                        // Write the number of blocks hashed by this table at the bottom of the table, MS why?
                        uint blocksHashed;
                        if (i + 1 == TopTable.EntryCount)
                            blocksHashed = Header.StfsVolumeDescriptor.allocatedBlockCount%0x70E4 == 0
                                ? 0x70E4
                                : Header.StfsVolumeDescriptor.allocatedBlockCount%0x70E4;
                        else
                            blocksHashed = 0x70E4;
                        var temp = blocksHashed.ReverseGenericArray(1, 4);
                        Array.Copy(temp, 0, blockBuffer1, 0xFF0, temp.Length);
                        // Write the hash table back to the file
                        Xio.Position = level1Table.AddressInFile;
                        Xio.WriteBytes(blockBuffer1, 0, 0x1000);
                        if (TopTable.Entries[i].Status == BlockStatusLevelZero.Unallocated)
                            unalloccount++;
                        else
                            alloccount++;
                        totalblocks++;
                        // hash the table
                        Array.Copy(TopTable.Entries[i]._Flags, 0, topBuffer, i*0x18 + 0x14, 4);
                        Array.Copy(HashBlock(blockBuffer1), 0, topBuffer, i*0x18, 0x14);
                    }
                    break;
            }

            //Header.StfsVolumeDescriptor.allocatedBlockCount = (uint)totalblocks;
            //Header.StfsVolumeDescriptor.unallocatedBlockCount = (uint) unalloccount;

            // Write the number of blocks the table hashes at the bottom of the hash table, MS why?
            if (TopTable.Level >= Level.One)
            {
                //uint allocatedBlockCountSwapped = Header.StfsVolumeDescriptor.allocatedBlockCount;
                var temp = Header.StfsVolumeDescriptor.allocatedBlockCount.ReverseGenericArray(1, 4);
                Array.Copy(temp, 0, topBuffer, 0xFF0, temp.Length);
            }

            // hash the top table
            Header.StfsVolumeDescriptor.topHashTableHash = HashBlock(topBuffer);

            // Write new hash block to the package
            Xio.Position = TopTable.AddressInFile;
            Xio.WriteBytes(topBuffer, 0, 0x1000);
            Header.WriteMetaData();
            // set the headerStart
            var headerStart = (uint) (IsPec() ? 0x23C : 0x344);

            // calculate header size / first hash table address
            var calculated = (Header.HeaderSize + 0xFFF) & 0xF000;
            var headerSize = calculated - headerStart;

            // read the data to hash
            //byte[]buffer = new byte[headerSize];
            Xio.Position = headerStart;
            var header = Xio.ReadBytes((int) headerSize);
            Header.HeaderHash = HashBlock(header);
        }

        public void Resign()
        {
            Header.ResignHeader();
        }

        public void RehashAndResign()
        {
            Rehash();
            Resign();
        }

        public void RebuildNewStfs(string newpath)
        {
            var flag = File.Exists(newpath);
            using (var fs = new FileStream(newpath, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
            {
                var stfs = new StfsPackage(fs, 1);
                stfs.Header = Header;
                stfs.Root = Root;
                stfs.WriteFileListing();
            }
        }

        public bool IsPec()
        {
            return (Flags & (uint) StfsPackageFlags.StfsPackagePec) == 1;
        }

        public void Close()
        {
            if (Xio != null && !IoPassedIn)
                Xio.Close();
            if (Header != null)
                Header.CleanUp();
            Header = null;
            if (Root != null)
                Root.CleanUp();
            Root = null;
        }

        #endregion
    }
}