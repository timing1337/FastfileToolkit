using FastfileToolkit.Bdiff;
using FastfileToolkit.Compressors;
using FastfileToolkit.Native;
using Newtonsoft.Json;
using Serilog;
using System.Runtime.InteropServices;

namespace FastfileToolkit.Fastfiles;

public abstract unsafe class Fastfile
{
    public byte[] Header;

    public XArchiveBlock* MemoryBlocks;
    public ulong* AssetList;

    public Fastfile(string path)
    {
        ReadFastfile(path);
        AllocateMemoryBlocks();
    }

    private void ReadFastfile(string zone)
    {
        string path = Path.Join(Toolkit.Instance.GamePath, zone + ".ff");
        BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open));
        Header = ReadHeader(reader);

        uint magic = reader.ReadUInt32();

        if (magic != 0x43574902)
        {
            Log.Error("Invalid magic number in fastfile header");
            reader.Close();
            return;
        }

        ulong size = ReadFastfileSize();

        OodleDecompressor oodle = new OodleDecompressor();
        if (!oodle.Decompress(reader, size, out byte[] sourceBuffer))
        {
            Log.Error("Failed to decompress fastfile");
            return;
        }

        Patch(zone, sourceBuffer);
    }

    private unsafe void Patch(string zone, byte[] sourceBuffer)
    {
        string patchFile = Path.Join(Toolkit.Instance.GamePath, zone + ".fp");
        if (!File.Exists(patchFile))
        {
            return;
        }
        BinaryReader patchReader = new BinaryReader(File.Open(patchFile, FileMode.Open));
        FastPatch patch = FastPatch.Read(patchReader);

        if (patch.residentDiffUncompSize == 0 || patch.residentDiffCompSize == 0)
        {
            return;
        }

        ReadHeader(patchReader);

        Header = ReadHeader(patchReader);

        uint magic = patchReader.ReadUInt32();

        if (magic != 0x43574902)
        {
            Log.Error("Invalid magic number in fastpatch header");
            patchReader.Close();
            return;
        }

        OodleDecompressor oodle = new OodleDecompressor();
        if (!oodle.Decompress(patchReader, patch.residentDiffUncompSize, out byte[] patchBuffer))
        {
            Log.Error("Failed to decompress patch");
            return;
        }
    }


    private void AllocateMemoryBlocks()
    {
        AssetList = (ulong*)Marshal.AllocHGlobal(128 * sizeof(ulong));
        MemoryBlocks = (XArchiveBlock*)Marshal.AllocHGlobal(17 * sizeof(XArchiveBlock));
        ulong[] bufferSizes = ReadXArchiveBlockSizes();
        for (int i = 0; i < 17; i++)
        {
            if (bufferSizes[i] == 0) continue;
            MemoryBlocks[i] = XArchiveBlock.Allocate((nuint)bufferSizes[i]);

            Log.Information("Allocated block {i} of size {size}", i, bufferSizes[i]);
        }
    }

    private void FreeMemoryBlocks()
    {
        for (int i = 0; i < 17; i++)
        {
            if (MemoryBlocks[i].memory == null) continue;
            MemoryBlocks[i].Free();
        }
        Marshal.FreeHGlobal((IntPtr)MemoryBlocks);
        Marshal.FreeHGlobal((IntPtr)AssetList);
    }

    public abstract byte[] ReadHeader(BinaryReader reader);
    public abstract byte[] ReadPatchHeader(BinaryReader reader);
    public abstract ulong ReadFastfileSize();
    public abstract ulong[] ReadXArchiveBlockSizes();
}
