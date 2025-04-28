using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastfileToolkit.Compressors;

public abstract class BaseCompressor
{
    public abstract bool Decompress(BinaryReader reader, ulong size, out byte[] decompressed);
}