using FastfileToolkit.Compressors;
using Serilog;
using System.Drawing;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;

namespace FastfileToolkit.Fastfiles;

public unsafe class Fastfile {

    public string GameDirectory;
    public string Name;

    public byte[] FastfileHeader;

    public XArchiveBlock* MemoryBlocks;
    public ulong* AssetList;

    public byte[] Data;
    public ulong Offset = 0;

    public ulong Magic {
        get {
            return BitConverter.ToUInt64(FastfileHeader, 0);
        }
    }

    public ulong Version {
        get {
            return BitConverter.ToUInt64(FastfileHeader, 8);
        }
    }
    public ulong Filesize {
        get {
            return BitConverter.ToUInt64(FastfileHeader, 56);
        }
    }

    public ulong[] BufferSizes {
        get {
            ulong[] bufferSizes = new ulong[17];
            for (int i = 0; i < 17; i++) {
                bufferSizes[i] = BitConverter.ToUInt64(FastfileHeader, 72 + (i * 8));
            }
            return bufferSizes;
        }
    }

    public Fastfile(string gameDirectory, string zoneName) {
        GameDirectory = gameDirectory;
        Name = zoneName;

        Patch(Read());
        AllocateMemoryBlocks();
    }

    private byte[] Read() {
        string path = Path.Join(GameDirectory, Name + ".ff");
        Log.Information("Reading fastfile {name} from {path}", Name, path);
        if (!File.Exists(path)) {
            throw new FileNotFoundException($"Fastfile {Name} not found in {GameDirectory}");
        }

        BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open));

        FastfileHeader = reader.ReadBytes(224);

        if (Magic != 0x3030316166665749) {
            throw new Exception("Invalid fast file header");
        }

        if (Version < 18) {
            throw new Exception($"Unsupported fast file version: {Version}");
        }

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
        if (!oodle.Decompress(reader, Filesize, out byte[] sourceBuffer)) {
            throw new Exception("Failed to decompress fastfile data");
        }

        return sourceBuffer;
    }

    private unsafe void Patch(byte[] sourceBuffer) {
        string patchFile = Path.Join(GameDirectory, Name + ".fp");
        if (!File.Exists(patchFile)) {
            Data = sourceBuffer;
            return;
        }

        BinaryReader reader = new BinaryReader(File.Open(patchFile, FileMode.Open));
        FastPatch patch = FastPatch.Read(reader);

        if (patch.residentDiffUncompSize == 0 || patch.residentDiffCompSize == 0) {
            Data = sourceBuffer;
            return;
        }

        reader.BaseStream.Seek(224, SeekOrigin.Current);
        FastfileHeader = reader.ReadBytes(224);

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
        Data = patchStream.Patch();
    }

    private void AllocateMemoryBlocks() {
        AssetList = (ulong*)NativeMemory.AllocZeroed(65536);
        MemoryBlocks = (XArchiveBlock*)NativeMemory.AllocZeroed((nuint)(17 * sizeof(XArchiveBlock)));

        var bufferSizes = BufferSizes;
        for (int i = 0; i < 17; i++) {
            if (bufferSizes[i] == 0) continue;
            MemoryBlocks[i] = new XArchiveBlock((nuint)bufferSizes[i]);
            MemoryBlocks[i].Allocate();
        }
    }

    private void FreeMemoryBlocks() {
        for (int i = 0; i < 17; i++) {
            if (MemoryBlocks[i].Pointer == null) continue;
            MemoryBlocks[i].Free();
        }
        NativeMemory.Free(MemoryBlocks);
        NativeMemory.Free(AssetList);
    }
}
