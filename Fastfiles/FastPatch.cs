using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastfileToolkit.Fastfiles;

public class FastPatch {
    public ulong magic;
    public uint version;
    public uint diffVersion;
    public BDiffWindowSizes residentWindowSizes;
    public ulong residentDiffCompSize;
    public ulong residentDiffUncompSize;

    public static FastPatch Read(BinaryReader reader) {
        FastPatch header = new FastPatch();
        header.magic = reader.ReadUInt64();
        header.version = reader.ReadUInt32();
        header.diffVersion = reader.ReadUInt32();
        header.residentWindowSizes = BDiffWindowSizes.Read(reader);
        header.residentDiffCompSize = reader.ReadUInt64();
        header.residentDiffUncompSize = reader.ReadUInt64();
        return header;
    }
}