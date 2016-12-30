using System;
using System.Text;
using HavenInterface.IOPackage;

namespace Dark_Souls_II_Save_Editor.STFSPackage
{
    public class LicenseEntry
    {
        public uint bits;
        public ulong data;
        public uint flags;
        public LicenseType type;
    }

    public class StfsVolumeDescriptor
    {
        public uint allocatedBlockCount;
        public byte blockSeperation;
        public ushort fileTableBlockCount;
        public int fileTableBlockNum; //int24
        public byte reserved;
        public byte size;
        public byte[] topHashTableHash = new byte[0x14];
        public uint unallocatedBlockCount;
    }

    public class SvodVolumeDescriptor
    {
        public byte blockCacheElementCount;
        public int dataBlockCount; //int24;
        public int dataBlockOffset; //int24;
        public byte flags;
        public byte[] reserved = new byte[5];
        public byte[] rootHash = new byte[0x14];
        public byte size;
        public byte workerThreadPriority;
        public byte workerThreadProcessor;
    }


    public class Certificate
    {
        public byte[] certificateSignature = new byte[0x100];
        public ConsoleTypeFlags consoleTypeFlags;
        public string dateGeneration;
        public byte[] ownerConsoleID = new byte[5];
        public string ownerConsolePartNumber;
        public ConsoleType ownerConsoleType;
        public uint publicExponent;
        public ushort publicKeyCertificateSize;
        public byte[] publicModulus = new byte[0x80];
        public byte[] signature = new byte[0x80];
    }

    public class MSTime
    {
        public byte hours;
        public byte minutes;
        public byte month;
        public byte monthDay;
        public byte seconds;
        public ushort year;
    }

    public class TimeT
    {
        public uint dwHighDateTime;
        public uint dwLowDateTime;
    }

    public static class STFSDefinitions
    {
        public static StfsVolumeDescriptor ReadStfsVolumeDescriptorEx(StreamIO io, uint address)
        {
            var descriptor = new StfsVolumeDescriptor();
            // seek to the volume descriptor
            io.Position = address;

            // read the descriptor
            descriptor.size = io.ReadUInt8();
            descriptor.reserved = io.ReadUInt8();
            descriptor.blockSeperation = io.ReadUInt8();

            io.IsBigEndian = false;

            descriptor.fileTableBlockCount = io.ReadUInt16();
            descriptor.fileTableBlockNum = io.ReadInt24();
            descriptor.topHashTableHash = io.ReadBytes(0x14);

            io.IsBigEndian = true;

            descriptor.allocatedBlockCount = io.ReadUInt32();
            descriptor.unallocatedBlockCount = io.ReadUInt32();
            return descriptor;
        }

        public static SvodVolumeDescriptor ReadSvodVolumeDescriptorEx(StreamIO io)
        {
            var descriptor = new SvodVolumeDescriptor();
            // seek to the volume descriptor
            io.Position = 0x379;
            descriptor.size = io.ReadUInt8();
            descriptor.blockCacheElementCount = io.ReadUInt8();
            descriptor.workerThreadProcessor = io.ReadUInt8();
            descriptor.workerThreadPriority = io.ReadUInt8();

            descriptor.rootHash = io.ReadBytes(0x14);

            descriptor.flags = io.ReadUInt8();
            descriptor.dataBlockCount = io.ReadInt24(false);
            descriptor.dataBlockOffset = io.ReadInt24(false);

            descriptor.reserved = io.ReadBytes(0x05);
            return descriptor;
        }

        public static Certificate ReadCertificateEx(StreamIO io, uint address)
        {
            var cert = new Certificate();
            // seek to the address of the certificate
            io.Position = address;

            cert.publicKeyCertificateSize = io.ReadUInt16();
            cert.ownerConsoleID = io.ReadBytes(5);

            var tempPartNum = new byte[0x15];
            tempPartNum[0x14] = 0;
            var temp = io.ReadBytes(0x11);
            Array.Copy(temp, 0, tempPartNum, 0, 0x11);
            cert.ownerConsolePartNumber = Encoding.ASCII.GetString(tempPartNum);

            var x = io.ReadUInt32();
            cert.ownerConsoleType = (ConsoleType) (x & 3);
            cert.consoleTypeFlags = (ConsoleTypeFlags) (x & 0xFFFFFFFC);
            if (cert.ownerConsoleType != ConsoleType.DevKit && cert.ownerConsoleType != ConsoleType.Retail)
                throw new Exception("STFS: Invalid console type.\n");

            var tempGenDate = io.ReadBytes(8);
            Array.Resize(ref tempGenDate, 9);

            cert.dateGeneration = Encoding.ASCII.GetString(tempGenDate);

            cert.publicExponent = io.ReadUInt32();
            cert.publicModulus = io.ReadBytes(0x80);
            cert.certificateSignature = io.ReadBytes(0x100);
            cert.signature = io.ReadBytes(0x80);
            return cert;
        }

        public static void WriteCertificateEx(Certificate cert, StreamIO io, uint address, RsaParam param = null)
        {
            if (param == null)
                param = new RsaParam();
            // seek to the position of the certificate
            io.Position = address;
            //// Write the certificate
            //io.WriteUInt16(cert.publicKeyCertificateSize);
            //io.WriteBytes(cert.ownerConsoleID);
            //io.WriteString(cert.ownerConsolePartNumber, StringType.Ascii);
            //uint temp = (uint)cert.consoleTypeFlags | (uint)cert.ownerConsoleType;
            //io.WriteUInt32(temp);
            //io.WriteString(cert.dateGeneration, StringType.Ascii);
            //io.WriteUInt32(cert.publicExponent);
            //io.WriteBytes(cert.publicModulus);
            //io.WriteBytes(cert.certificateSignature);
            if (address == 4)
                io.WriteBytes(param.Certificate);
            io.WriteBytes(cert.signature);
        }

        public static void WriteSvodVolumeDescriptorEx(SvodVolumeDescriptor descriptor, StreamIO io)
        {
            // volume descriptor position
            io.Position = 0x379;
            io.IsBigEndian = false;

            io.WriteUInt8(descriptor.size);
            io.WriteUInt8(descriptor.blockCacheElementCount);
            io.WriteUInt8(descriptor.workerThreadProcessor);
            io.WriteUInt8(descriptor.workerThreadPriority);

            io.WriteBytes(descriptor.rootHash);

            io.WriteUInt8(descriptor.flags);
            io.WriteInt24(descriptor.dataBlockCount);
            io.WriteInt24(descriptor.dataBlockOffset);

            io.WriteBytes(descriptor.reserved);

            io.IsBigEndian = true;
        }

        public static void WriteStfsVolumeDescriptorEx(StfsVolumeDescriptor descriptor, StreamIO io, uint address)
        {
            // volume descriptor position
            io.Position = address;

            // Write size, padding and block seperation
            var start = 0x240000; //int24
            start |= descriptor.blockSeperation;
            io.WriteInt24(start);

            io.WriteUInt16(descriptor.fileTableBlockCount, false);

            io.WriteInt24(descriptor.fileTableBlockNum, false);
            io.WriteBytes(descriptor.topHashTableHash);
            io.WriteUInt32(descriptor.allocatedBlockCount);
            io.WriteUInt32(descriptor.unallocatedBlockCount);
        }
    }
}