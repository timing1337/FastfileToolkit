using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastfileToolkit.Fastfiles;

public class FastfileV1 : Fastfile
{

    public FastfileV1(string path) : base(path)
    {
    }

    public override ulong[] ReadXArchiveBlockSizes()
    {
        ulong[] bufferSizes = new ulong[17];
        for (int i = 0; i < 17; i++)
        {
            bufferSizes[i] = BitConverter.ToUInt64(Header, 64 + (i * 8));
        }
        return bufferSizes;
    }

    public override ulong ReadFastfileSize()
    {
        return BitConverter.ToUInt64(Header, 48);
    }

    public override byte[] ReadHeader(BinaryReader reader)
    {
        return reader.ReadBytes(216);
    }

    public override byte[] ReadPatchHeader(BinaryReader reader)
    {
        return reader.ReadBytes(56);
    }
}