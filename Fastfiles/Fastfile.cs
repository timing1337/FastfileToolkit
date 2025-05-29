using FastfileToolkit.Compressors;
using Serilog;
using System.Drawing;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;

namespace FastfileToolkit.Fastfiles;

public unsafe class Fastfile {

    public string Name;
    public ulong Version;

    public XArchiveBlock* MemoryBlocks;
    public ulong* AssetList;

    public byte[] sourceBuffer;

    public BinaryReader Reader;

    public Fastfile(string path) {
        Name = Path.GetFileNameWithoutExtension(path);
        ReadFastfile(path);
    }

    private void ReadFastfile(string zone) {
        string path = Path.Join(Toolkit.Instance.GamePath, zone + ".ff");

        BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open));

        if (reader.ReadUInt64() != 0x3030316166665749) { //IWffa100
            throw new Exception("Invalid fast file header");
        }

        Version = reader.ReadUInt64();

        if (Version < 18) {
            throw new Exception($"Unsupported fast file version: {Version}");
        }
        //unknown 32 bytes
        reader.BaseStream.Seek(56, SeekOrigin.Begin);
        ulong fileSize = reader.ReadUInt64();

        reader.BaseStream.Seek(72, SeekOrigin.Begin);

        ulong[] bufferSizes = new ulong[17];
        for (int i = 0; i < 17; i++) {
            bufferSizes[i] = reader.ReadUInt64();
        }

        reader.BaseStream.Seek(224, SeekOrigin.Begin);

        if (Version >= 19) { //TAFF!
            uint taffMagic = reader.ReadUInt32();
            uint count = reader.ReadUInt32();
            for (ulong i = 0; i < count; i++) {
                uint key = reader.ReadUInt32();
                uint size = reader.ReadUInt32();
                reader.BaseStream.Seek(size, SeekOrigin.Current);
            }

            uint magic = reader.ReadUInt32();
            if (magic != 0x43574902) {
                //Another chunk of taff
                reader.BaseStream.Seek(288, SeekOrigin.Current);
                magic = reader.ReadUInt32();
            }
        }

        OodleDecompressor oodle = new OodleDecompressor();
        if (!oodle.Decompress(reader, fileSize, out byte[] sourceBuffer)) {
            throw new Exception("Failed to decompress fastfile data");
        }

        AllocateMemoryBlocks(bufferSizes);

        this.sourceBuffer = sourceBuffer;
        Patch(zone, sourceBuffer);
    }

    private unsafe void Patch(string zone, byte[] sourceBuffer) {
        string patchFile = Path.Join(Toolkit.Instance.GamePath, zone + ".fp");
        if (!File.Exists(patchFile)) {
            return;
        }
        BinaryReader reader = new BinaryReader(File.Open(patchFile, FileMode.Open));
        FastPatch patch = FastPatch.Read(reader);

        if (patch.residentDiffUncompSize == 0 || patch.residentDiffCompSize == 0) {
            return;
        }

        reader.BaseStream.Seek(224 * 2, SeekOrigin.Current);

        if (Version >= 19) { //TAFF!
            uint taffMagic = reader.ReadUInt32();
            uint count = reader.ReadUInt32();
            for (ulong i = 0; i < count; i++) {
                uint key = reader.ReadUInt32();
                uint size = reader.ReadUInt32();
                reader.BaseStream.Seek(size, SeekOrigin.Current);
            }

            uint magic = reader.ReadUInt32();
            if (magic != 0x43574902) {
                //Another chunk of taff
                reader.BaseStream.Seek(288, SeekOrigin.Current);
                magic = reader.ReadUInt32();
            }
        }

        OodleDecompressor oodle = new OodleDecompressor();
        if (!oodle.Decompress(reader, patch.residentDiffUncompSize, out byte[] patchBuffer)) {
            Log.Error("Failed to decompress patch");
            return;
        }

        var patchStream = new DBBinaryPatchStream(patch, sourceBuffer, patchBuffer);
        Reader = new BinaryReader(new MemoryStream(patchStream.Patch()));
    }


    private void AllocateMemoryBlocks(ulong[] bufferSizes) {
        AssetList = (ulong*)Marshal.AllocHGlobal(128 * sizeof(ulong));
        MemoryBlocks = (XArchiveBlock*)Marshal.AllocHGlobal(17 * sizeof(XArchiveBlock));
        for (int i = 0; i < 17; i++) {
            if (bufferSizes[i] == 0) continue;
            MemoryBlocks[i] = XArchiveBlock.Allocate((nuint)bufferSizes[i]);

            Log.Information("Allocated block {i} of size {size}", i, bufferSizes[i]);
        }
    }

    private void FreeMemoryBlocks() {
        for (int i = 0; i < 17; i++) {
            if (MemoryBlocks[i].memory == null) continue;
            MemoryBlocks[i].Free();
        }
        Marshal.FreeHGlobal((IntPtr)MemoryBlocks);
        Marshal.FreeHGlobal((IntPtr)AssetList);
    }
}
