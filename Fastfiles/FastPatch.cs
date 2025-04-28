using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastfileToolkit.Fastfiles;

public class BDiffWindowSizes
{
    public ulong destWindow;
    public ulong sourceWindow;
    public ulong diffWindow;

    public static BDiffWindowSizes Read(BinaryReader reader)
    {
        BDiffWindowSizes windowSizes = new BDiffWindowSizes();
        windowSizes.destWindow = reader.ReadUInt64();
        windowSizes.sourceWindow = reader.ReadUInt64();
        windowSizes.diffWindow = reader.ReadUInt64();
        return windowSizes;
    }
}
public class FastPatch
{
    public ulong magic;
    public uint version;
    public uint diffVersion;
    public BDiffWindowSizes residentWindowSizes;
    public ulong residentDiffCompSize;
    public ulong residentDiffUncompSize;

    public static FastPatch Read(BinaryReader reader)
    {
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