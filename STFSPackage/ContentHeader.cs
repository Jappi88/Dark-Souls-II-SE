using System;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using HavenInterface.Encryptions;
using HavenInterface.IOPackage;

namespace Dark_Souls_II_Save_Editor.STFSPackage
{
    public class ContentHeader
    {
        public ContentHeader(StreamIO io, uint flag)
        {
            Xio = io;
            Xflags = flag;
            InstallerType = 0;
            if ((Xflags & (uint) ContentFlags.MetadataSkipRead) == 0)
                Readmetadata();
        }

        public void CleanUp()
        {
            SeasonId = null;
            CabResumeData = null;
            SeriesId = null;
            Guid = null;
            DeviceId = null;
            ConsoleId = null;
            ProfileId = null;
            HeaderHash = null;
            LicenseData = null;
        }

        #region internal variables

        // only console signed
        private readonly string[] _displayDescription = new string[12];
        private readonly string[] _displayName = new string[12];

        // only strong signed

        private byte[] _thumbnailImage;
        private byte[] _titleThumbnailImage;

        internal uint Xflags;
        internal StreamIO Xio;

        #endregion

        #region private methods

        private void Readmetadata()
        {
            Xio.Position = 0;
            if ((Xflags & (uint) ContentFlags.MetadataIsPec) == 0)
            {
                Magic = (Magic) Xio.ReadUInt32();

                if (Magic == Magic.CON)
                    Certificate = STFSDefinitions.ReadCertificateEx(Xio, 4);
                else if (Magic == Magic.LIVE || Magic == Magic.PIRS)
                    PackageSignature = Xio.ReadBytes(0x100);
                else
                {
                    var x =
                        new Exception("ContentHeader: Content signature type 0x" + ((uint) Magic).ToString("X2") +
                                      " is not a valid STFS file.\n");
                    throw x;
                }

                Xio.Position = 0x22C;
                LicenseData = new LicenseEntry[0x10];
                // read licensing data
                for (var i = 0; i < 0x10; i++)
                {
                    var tempYo = Xio.ReadUInt64();
                    LicenseData[i] = new LicenseEntry();
                    LicenseData[i].type = (LicenseType) (tempYo >> 48);
                    LicenseData[i].data = tempYo & 0xFFFFFFFFFFFF;
                    LicenseData[i].bits = Xio.ReadUInt32();
                    LicenseData[i].flags = Xio.ReadUInt32();

                    switch (LicenseData[i].type)
                    {
                        case LicenseType.Unused:
                        case LicenseType.Unrestricted:
                        case LicenseType.ConsoleProfileLicense:
                        case LicenseType.WindowsProfileLicense:
                        case LicenseType.ConsoleLicense:
                        case LicenseType.MediaFlags:
                        case LicenseType.KeyVaultPrivileges:
                        case LicenseType.HyperVisorFlags:
                        case LicenseType.UserPrivileges:
                            break;
                        default:
                            throw new Exception("ContentHeader: Invalid license type at index " + i + ".\n");
                    }
                }

                // header hash / content id
                HeaderHash = Xio.ReadBytes(0x14);

                HeaderSize = Xio.ReadUInt32();
                // to do: make sure it's a valid type
                ContentType = (ContentType) Xio.ReadUInt32();

                // read metadata information
                MetaDataVersion = Xio.ReadUInt32();
                ContentSize = Xio.ReadUInt64();
                MediaId = Xio.ReadUInt32();
                Version = Xio.ReadUInt32();
                BaseVersion = Xio.ReadUInt32();
                TitleId = Xio.ReadUInt32();
                Platform = Xio.ReadUInt8();
                ExecutableType = Xio.ReadUInt8();
                DiscNumber = Xio.ReadUInt8();
                DiscInSet = Xio.ReadUInt8();
                SavegameId = Xio.ReadUInt32();
                ConsoleId = Xio.ReadBytes(5);
                ProfileId = Xio.ReadBytes(8);

                // read the file system type
                Xio.Position = 0x3A9;
                FileSystem = (FileSystem) Xio.ReadUInt32();
                if ((uint) FileSystem > 1)
                    throw new Exception("ContentHeader: Invalid file system. Only STFS and SVOD are supported.\n");

                // read volume descriptor
                if (FileSystem == FileSystem.FileSystemStfs)
                    StfsVolumeDescriptor = STFSDefinitions.ReadStfsVolumeDescriptorEx(Xio, 0x379);
                else if (FileSystem == FileSystem.FileSystemSvod)
                    SvodVolumeDescriptor = STFSDefinitions.ReadSvodVolumeDescriptorEx(Xio);

                DataFileCount = Xio.ReadUInt32();
                DataFileCombinedSize = Xio.ReadUInt64();

                // read the avatar metadata if needed
                if (ContentType == ContentType.AvatarItem)
                {
                    Xio.Position = 0x3D9;
                    Xio.IsBigEndian = false;

                    SubCategory = (AssetSubcategory) Xio.ReadUInt32();
                    Colorizable = Xio.ReadUInt16();

                    Xio.IsBigEndian = true;

                    Guid = Xio.ReadBytes(0x10);
                    SkeletonVersion = (SkeletonVersion) Xio.ReadUInt8();

                    if ((byte) SkeletonVersion < 1 || (byte) SkeletonVersion > 3)
                        throw new Exception("ContentHeader: Invalid skeleton version.");
                }
                else if (ContentType == ContentType.Video) // there may be other content types with this metadata
                {
                    Xio.Position = 0x3D9;

                    SeriesId = Xio.ReadBytes(0x10);
                    SeasonId = Xio.ReadBytes(0x10);

                    SeasonNumber = Xio.ReadUInt16();
                    EpisodeNumber = Xio.ReadUInt16();
                }

                // skip padding
                Xio.Position = 0x3FD;
                DeviceId = Xio.ReadBytes(0x14);
                var name = "";
                for (byte i = 0; i < 12; i++)
                {
                    name = Xio.ReadString(StringType.Unicode, 0x80, true).Replace("\0", "");
                    if (!string.IsNullOrWhiteSpace(name))
                        RegionId = i;
                    _displayName[i] = name;
                }
                //Xio.Position = 0xD11;
                for (byte i = 0; i < 12; i++)
                {
                    name = Xio.ReadString(StringType.Unicode, 0x100, true).Replace("\0", "");
                    if (!string.IsNullOrWhiteSpace(name))
                        RegionId = i;
                    _displayDescription[i] = name;
                }
                Xio.Position = 0x1611;
                PublisherName = Xio.ReadString(StringType.Unicode, 0x80, true).Replace("\0", "");
                TitleName = Xio.ReadString(StringType.Unicode, 0x80, true).Replace("\0", "");
                Xio.Position = 0x1711;
                TransferFlags = Xio.ReadUInt8();

                // read image sizes
                ThumbnailImageSize = Xio.ReadUInt32();
                TitleThumbnailImageSize = Xio.ReadUInt32();

                // read images
                Xio.Position = 0x171A;
                _thumbnailImage = Xio.ReadBytes((int) (ThumbnailImageSize < 0x4000 ? ThumbnailImageSize : 0x4000),
                    false);

                Xio.Position = 0x571A;
                _titleThumbnailImage =
                    Xio.ReadBytes((int) (TitleThumbnailImageSize < 0x4000 ? TitleThumbnailImageSize : 0x4000), false);
                Xio.Position = 0x971A;

                if (((HeaderSize + 0xFFF) & 0xFFFFF000) - 0x971A < 0x15F4)
                    return;

                InstallerType = (InstallerType) Xio.ReadUInt32();
                switch (InstallerType)
                {
                    case InstallerType.SystemUpdate:
                    case InstallerType.TitleUpdate:
                    {
                        var tempbv = Xio.ReadUInt32();
                        InstallerBaseVersion = new Version();
                        InstallerBaseVersion.major = (ushort) (tempbv >> 28);
                        InstallerBaseVersion.minor = (ushort) ((tempbv >> 24) & 0xF);
                        InstallerBaseVersion.build = (ushort) ((tempbv >> 8) & 0xFFFF);
                        InstallerBaseVersion.revision = (ushort) (tempbv & 0xFF);

                        tempbv = Xio.ReadUInt32();
                        InstallerVersion = new Version();
                        InstallerVersion.major = (ushort) (tempbv >> 28);
                        InstallerVersion.minor = (ushort) ((tempbv >> 24) & 0xF);
                        InstallerVersion.build = (ushort) ((tempbv >> 8) & 0xFFFF);
                        InstallerVersion.revision = (ushort) (tempbv & 0xFF);

                        break;
                    }
                    case InstallerType.SystemUpdateProgressCache:
                    case InstallerType.TitleUpdateProgressCache:
                    case InstallerType.TitleContentProgressCache:
                    {
                        ResumeState = (OnlineContentResumeState) Xio.ReadUInt32();
                        CurrentFileIndex = Xio.ReadUInt32();
                        CurrentFileOffset = Xio.ReadUInt64();
                        BytesProcessed = Xio.ReadUInt64();

                        var dwHighDateTime = Xio.ReadUInt32();
                        var dwLowDateTime = Xio.ReadUInt32();
                        LastModified = VariosStuff.FiletimEtoTimeT(dwHighDateTime, dwLowDateTime);

                        CabResumeData = Xio.ReadBytes(0x15D0);
                        break;
                    }
                    case InstallerType.None:
                        break;
                    default:
                        throw new Exception("ContentHeader: Invalid Installer Type value.");
                }

#if Debug
                if (metaDataVersion != 2)
                    throw new Exception("ContentHeader: Metadata version is not 2.\n");
#endif
            }
            else
            {
                Certificate = STFSDefinitions.ReadCertificateEx(Xio, 0);
                HeaderHash = Xio.ReadBytes(0x14);

                StfsVolumeDescriptor = STFSDefinitions.ReadStfsVolumeDescriptorEx(Xio, 0x244);

                // *skip missing int*
                Xio.Position = 0x26C;
                ProfileId = Xio.ReadBytes(8);
                Enabled = Xio.ReadUInt8() == 1;
                ConsoleId = Xio.ReadBytes(5);

                // anything between 1 and 0x1000 works, inclusive
                HeaderSize = 0x1000;
            }
        }

        private void WriteCertificate(RsaParam param = null)
        {
            if (Magic != Magic.CON && (Xflags & (int) ContentFlags.MetadataIsPec) == 0)
                throw new Exception(
                    "XContentHeader: Error writing certificate. Package is strong signed and therefore doesn't have a certificate.\n");

            STFSDefinitions.WriteCertificateEx(Certificate, Xio,
                (uint) ((Xflags & (int) ContentFlags.MetadataIsPec) == 0 ? 4 : 0), param);
        }

        private void FixHeaderHash()
        {
            var headerStart = (uint) ((Xflags & (int) ContentFlags.MetadataIsPec) == 0 ? 0x344 : 0x23C);

            // calculate header size / first hash table address
            var calculated = (HeaderSize + 0xFFF) & 0xFFFFF000;
            Xio.Position = 0;
            //calculated = (xio.Position < calculated) ? (uint) xio.Position : calculated;
            var realHeaderSize = calculated - headerStart;

            // seek to the position
            Xio.Position = headerStart;
            var data = Xio.ReadBytes((int) realHeaderSize);
            HeaderHash = SHA1.Create().ComputeHash(data);

            // Write the new hash to the file
            if ((Xflags & (int) ContentFlags.MetadataIsPec) == 0)
                Xio.Position = 0x32c;
            else
                Xio.Position = 0x228;
            Xio.WriteBytes(HeaderHash);
            Xio.Flush();
        }

        internal void WriteVolumeDescriptor()
        {
            if (FileSystem == FileSystem.FileSystemStfs)
                STFSDefinitions.WriteStfsVolumeDescriptorEx(StfsVolumeDescriptor, Xio,
                    (uint) ((Xflags & (int) ContentFlags.MetadataIsPec) == 0 ? 0x379 : 0x244));
            else if (FileSystem == FileSystem.FileSystemSvod)
                STFSDefinitions.WriteSvodVolumeDescriptorEx(SvodVolumeDescriptor, Xio);
        }

        internal void WriteMetaData()
        {
            // seek to the begining of the file
            Xio.Position = 0;

            if ((Xflags & (int) ContentFlags.MetadataIsPec) == 0)
            {
                Xio.WriteUInt32((uint) Magic);

                if (Magic == Magic.CON)
                    WriteCertificate();
                else if (Magic == Magic.PIRS || Magic == Magic.LIVE)
                    Xio.WriteBytes(PackageSignature);
                else
                    throw new Exception("XContentHeader: Content signature type 0x" + Magic.ToString("X2") +
                                        " is invalid.\n");

                // Write the licensing data
                Xio.Position = 0x22C;
                for (uint i = 0; i < 0x10; i++)
                {
                    Xio.WriteUInt64(((ulong) LicenseData[i].type << 48) | LicenseData[i].data);
                    Xio.WriteUInt32(LicenseData[i].bits);
                    Xio.WriteUInt32(LicenseData[i].flags);
                }

                Xio.WriteBytes(HeaderHash);
                Xio.WriteUInt32(HeaderSize);
                Xio.WriteUInt32((uint) ContentType);
                Xio.WriteUInt32(MetaDataVersion);
                Xio.WriteUInt64(ContentSize);
                Xio.WriteUInt32(MediaId);
                Xio.WriteUInt32(Version);
                Xio.WriteUInt32(BaseVersion);
                Xio.WriteUInt32(TitleId);
                Xio.WriteUInt8(Platform);
                Xio.WriteUInt8(ExecutableType);
                Xio.WriteUInt8(DiscNumber);
                Xio.WriteUInt8(DiscInSet);
                Xio.WriteUInt32(SavegameId);
                Xio.WriteBytes(ConsoleId);
                Xio.WriteBytes(ProfileId);

                WriteVolumeDescriptor();

                Xio.WriteUInt32(DataFileCount);
                Xio.WriteUInt64(DataFileCombinedSize);

                // Write the avatar asset metadata if needed
                if (ContentType == ContentType.AvatarItem)
                {
                    Xio.Position = 0x3D9;
                    Xio.SwapEndian();

                    Xio.WriteUInt32((uint) SubCategory);
                    Xio.WriteUInt32(Colorizable);

                    Xio.SwapEndian();

                    Xio.WriteBytes(Guid);
                    Xio.WriteUInt8((byte) SkeletonVersion);
                }
                else if (ContentType == ContentType.Video)
                {
                    Xio.Position = 0x3D9;

                    Xio.WriteBytes(SeriesId);
                    Xio.WriteBytes(SeasonId);

                    Xio.WriteUInt16(SeasonNumber);
                    Xio.WriteUInt16(EpisodeNumber);
                }

                // skip padding
                Xio.Position = 0x3FD;
                Xio.WriteBytes(DeviceId);
                for (var i = 0; i < 12; i++)
                {
                    Xio.WriteString(_displayName[i].PadRight(0x80, '\0'), StringType.Unicode);
                }
                Xio.Position = 0xD11;
                for (var i = 0; i < 12; i++)
                {
                    Xio.WriteString(_displayDescription[i].PadRight(0x100, '\0'), StringType.Unicode);
                }

                Xio.Position = 0x1611;
                Xio.WriteString(PublisherName.PadRight(0x80, '\0'), StringType.Unicode);
                Xio.WriteString(TitleName.PadRight(0x80, '\0'), StringType.Unicode);

                Xio.Position = 0x1711;
                Xio.WriteUInt8(TransferFlags);

                Xio.WriteUInt32((uint) _thumbnailImage.Length);
                Xio.Position += 4;
                Xio.WriteBytes(_thumbnailImage);
                Xio.Position = 0x1716;
                Xio.WriteUInt32((uint) _titleThumbnailImage.Length);
                Xio.Position = 0x571A;
                Xio.WriteBytes(_titleThumbnailImage);

                if (((HeaderSize + 0xFFF) & 0xFFFFF000) - 0x971A < 0x15F4)
                    return;

                Xio.WriteUInt32((uint) InstallerType, false);
                switch (InstallerType)
                {
                    case InstallerType.SystemUpdate:
                    case InstallerType.TitleUpdate:
                    {
                        var tempbv = 0;
                        tempbv |= (InstallerBaseVersion.major & 0xF) << 28;
                        tempbv |= (InstallerBaseVersion.minor & 0xF) << 24;
                        tempbv |= (InstallerBaseVersion.build & 0xFFFF) << 8;
                        tempbv |= InstallerBaseVersion.revision & 0xFF;
                        Xio.WriteInt32(tempbv);

                        var tempv = 0;
                        tempv |= (InstallerVersion.major & 0xF) << 28;
                        tempv |= (InstallerVersion.minor & 0xF) << 24;
                        tempv |= (InstallerVersion.build & 0xFFFF) << 8;
                        tempv |= InstallerVersion.revision & 0xFF;
                        Xio.WriteInt32(tempv);

                        break;
                    }

                    case InstallerType.SystemUpdateProgressCache:
                    case InstallerType.TitleUpdateProgressCache:
                    case InstallerType.TitleContentProgressCache:
                    {
                        Xio.WriteUInt32((uint) ResumeState);
                        Xio.WriteUInt32(CurrentFileIndex);
                        Xio.WriteUInt64(CurrentFileOffset);
                        Xio.WriteUInt64(BytesProcessed);
                        var time = VariosStuff.TimeTtoFiletime(LastModified);
                        Xio.WriteUInt32(time.dwHighDateTime);
                        Xio.WriteUInt32(time.dwLowDateTime);
                        Xio.WriteBytes(CabResumeData);
                        break;
                    }

                    case InstallerType.None:
                        break;
                }
            }
            else
            {
                Array.Copy(Certificate.ownerConsoleID, 0, ConsoleId, 0, 5);

                STFSDefinitions.WriteCertificateEx(Certificate, Xio, 0);
                Xio.WriteBytes(HeaderHash);

                STFSDefinitions.WriteStfsVolumeDescriptorEx(StfsVolumeDescriptor, Xio, 0x244);

                // *skip missing int*
                Xio.Position = 0x26C;
                Xio.WriteBytes(ProfileId);
                Xio.WriteUInt8((byte) (Enabled ? 1 : 0));
                Xio.WriteBytes(ConsoleId);
            }
            Xio.Flush();
        }

        internal void ResignHeader()
        {
            var param = new RsaParam();
            uint size, toSignLoc, consoleIdLoc;

            // set the headerStart
            if ((Xflags & (uint) ContentFlags.MetadataIsPec) == 1)
            {
                size = 0xDC4;
                toSignLoc = 0x23C;
                consoleIdLoc = 0x275;
            }
            else
            {
                size = 0x118;
                toSignLoc = 0x22C;
                consoleIdLoc = 0x36C;
            }
            // Write the console id
            Xio.Position = consoleIdLoc;
            Xio.WriteBytes(Certificate.ownerConsoleID);
            // Fix the header hash
            FixHeaderHash();
            Xio.Position = toSignLoc;
            var dataToSign = Xio.ReadBytes((int) size);
            dataToSign = SHA1.Create().ComputeHash(dataToSign);
            Certificate.signature = Rsa.GeneratePks1Signature(param.ParametersRsaKeys, dataToSign).SimpleScramble(true);
            WriteCertificate(param);
            Xio.Flush();
        }

        #endregion

        #region Public Properties

        public TransferLock TransferFlag
        {
            get { return (TransferLock) (TransferFlags >> 6); }
            set { TransferFlags = (byte) ((byte) ((int) value & 3) << 6); }
        }

        public uint BaseVersion { get; set; }

        public ulong BytesProcessed { get; set; }

        public byte[] CabResumeData { get; set; } = new byte[5584];

        public Certificate Certificate { get; set; }

        public ushort Colorizable { get; set; }

        public byte[] ConsoleId { get; set; } = new byte[5];

        public ulong ContentSize { get; set; }

        public ContentType ContentType { get; set; }

        public uint CurrentFileIndex { get; set; }

        public ulong CurrentFileOffset { get; set; }

        public ulong DataFileCombinedSize { get; set; }

        public uint DataFileCount { get; set; }

        public byte[] DeviceId { get; set; } = new byte[0x14];

        public byte DiscInSet { get; set; }

        public byte DiscNumber { get; set; }

        public string DisplayDescription
        {
            get { return _displayDescription[RegionId]; }
            set { _displayDescription[RegionId] = value; }
        }

        public string DisplayName
        {
            get { return _displayName[RegionId]; }
            set { _displayName[RegionId] = value; }
        }

        public bool Enabled { get; set; }

        public ushort EpisodeNumber { get; set; }

        public byte ExecutableType { get; set; }

        public FileSystem FileSystem { get; set; }

        public byte[] Guid { get; set; } = new byte[0x10];

        public byte[] HeaderHash { get; set; } = new byte[0x14];

        public uint HeaderSize { get; set; }

        public Version InstallerBaseVersion { get; set; }

        public InstallerType InstallerType { get; set; }

        public Version InstallerVersion { get; set; }

        public DateTime LastModified { get; set; }

        public LicenseEntry[] LicenseData { get; set; } = new LicenseEntry[0x10];

        public Magic Magic { get; set; }

        public uint MediaId { get; set; }

        public uint MetaDataVersion { get; set; }

        public byte[] PackageSignature { get; set; } = new byte[0x100];

        public byte Platform { get; set; }

        public byte[] ProfileId { get; set; } = new byte[8];

        public string PublisherName { get; set; }

        public OnlineContentResumeState ResumeState { get; set; }

        public uint SavegameId { get; set; }

        public byte[] SeasonId { get; set; } = new byte[0x10];

        public ushort SeasonNumber { get; set; }

        public byte[] SeriesId { get; set; } = new byte[0x10];

        public SkeletonVersion SkeletonVersion { get; set; }

        public StfsVolumeDescriptor StfsVolumeDescriptor { get; set; }

        public AssetSubcategory SubCategory { get; set; }

        public SvodVolumeDescriptor SvodVolumeDescriptor { get; set; }

        public Image ThumbnailImage
        {
            get { return Image.FromStream(new MemoryStream(_thumbnailImage)); }
            set { _thumbnailImage = value.ToByteArray(); }
        }

        public uint ThumbnailImageSize { get; set; }

        public uint TitleId { get; set; }

        public string TitleName { get; set; }

        public Image TitleThumbnailImage
        {
            get { return Image.FromStream(new MemoryStream(_titleThumbnailImage)); }
            set { _titleThumbnailImage = value.ToByteArray(); }
        }

        public uint TitleThumbnailImageSize { get; set; }

        public byte TransferFlags { get; set; }

        public uint Version { get; set; }

        public byte RegionId { get; private set; }

        #endregion
    }
}