namespace Dark_Souls_II_Save_Editor.STFSPackage
{
    public enum LanguagesType : byte
    {
        /// <summary>English Langauage</summary>
        English = 0,

        /// <summary>Japanese Langauage</summary>
        Japanese = 1,

        /// <summary>German Langauage</summary>
        German = 2,

        /// <summary>French Langauage</summary>
        French = 3,

        /// <summary>Spanish Langauage</summary>
        Spanish = 4,

        /// <summary>Italian Langauage</summary>
        Italian = 5,

        /// <summary>Korean Langauage</summary>
        Korean = 6,

        /// <summary>Chinese Langauage</summary>
        Chinese = 7,

        /// <summary>Portugese Langauage</summary>
        Portuguese = 8
    }


    public enum Sex
    {
        StfsFemale = 0,
        StfsMale
    }

    public enum TransferLock : byte
    {
        AllowTransfer = 3,
        DeviceAllowOnly = 2,
        NoTransfer = 0,
        ProfileAllowOnly = 1
    }

    public enum Level
    {
        Lt = -1,
        Zero = 0,
        One = 1,
        Two = 2
    }

    public enum ConsoleType
    {
        DevKit = 1,
        Retail = 2
    }

    public enum ConsoleTypeFlags : uint
    {
        TestKit = 0x40000000,
        RecoveryGenerated = 0x80000000
    }

    public enum Magic
    {
        CON = 0x434F4E20,
        LIVE = 0x4C495645,
        PIRS = 0x50495253
    }

    public enum FileEntryFlags
    {
        ConsecutiveBlocks = 1,
        Folder = 2
    }

    public enum InstallerType
    {
        None = 0,
        SystemUpdate = 0x53555044,
        TitleUpdate = 0x54555044,
        SystemUpdateProgressCache = 0x50245355,
        TitleUpdateProgressCache = 0x50245455,
        TitleContentProgressCache = 0x50245443
    }

    public enum LicenseType
    {
        Unused = 0x0000,
        Unrestricted = 0xFFFF,
        ConsoleProfileLicense = 0x0009,
        WindowsProfileLicense = 0x0003,
        ConsoleLicense = 0xF000,
        MediaFlags = 0xE000,
        KeyVaultPrivileges = 0xD000,
        HyperVisorFlags = 0xC000,
        UserPrivileges = 0xB000
    }

    public enum StfsPackageFlags : uint
    {
        StfsPackagePec = 1,
        StfsPackageCreate = 2,
        StfsPackageFemale = 4 // only used when creating a packge
    }

    public enum ContentType
    {
        ArcadeGame = 0xD0000,
        AvatarAssetPack = 0x8000,
        AvatarItem = 0x9000,
        CacheFile = 0x40000,
        CommunityGame = 0x2000000,
        GameDemo = 0x80000,
        GameOnDemand = 0x7000,
        GamerPicture = 0x20000,
        GamerTitle = 0xA0000,
        GameTrailer = 0xC0000,
        GameVideo = 0x400000,
        InstalledGame = 0x4000,
        Installer = 0xB0000,
        IPTVPauseBuffer = 0x2000,
        LicenseStore = 0xF0000,
        MarketPlaceContent = 2,
        Movie = 0x100000,
        MusicVideo = 0x300000,
        PodcastVideo = 0x500000,
        Profile = 0x10000,
        Publisher = 3,
        SavedGame = 1,
        StorageDownload = 0x50000,
        Theme = 0x30000,
        Video = 0x200000,
        ViralVideo = 0x600000,
        XboxDownload = 0x70000,
        XboxOriginalGame = 0x5000,
        XboxSavedGame = 0x60000,
        Xbox360Title = 0x1000,
        XNA = 0xE0000
    }

    public enum BlockStatusLevelZero
    {
        Unallocated = 0,
        PreviouslyAllocated = 0x40,
        Allocated = 0x80,
        NewlyAllocated = 0xC0
    }

    public enum ContentFlags
    {
        MetadataIsPec = 1,
        MetadataSkipRead = 2,
        MetadataDontFreeThumbnails = 4
    }

    public enum OnlineContentResumeState
    {
        FileHeadersNotReady = 0x46494C48,
        NewFolder = 0x666F6C64,
        NewFolderResumAttempt1 = 0x666F6C31,
        NewFolderResumeAttempt2 = 0x666F6C32,
        NewFolderResumeAttemptUnknown = 0x666F6C3F,
        NewFolderResumeAttemptSpecific = 0x666F6C40
    }

    public enum FileSystem
    {
        FileSystemStfs = 0,
        FileSystemSvod,
        FileSystemFatx
    }

    public enum HashFlag : byte
    {
        Unallocated = 0,
        AllocatedFree,
        AllocatedInUseOld,
        AllocatedInUseCurrent
    }

    // this is from eaton
    public enum SVODFeatures
    {
        EnhancedGDFLayout = 0x40,
        houldBeZeroForDownLevelClients = 0x80
    }

    public class Version
    {
        public ushort major, minor, build, revision;
    }
}