using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastfileToolkit.Assets;

public struct XAsset {
    public ulong Hash;
    public uint Type;
    public nint Asset;
    public string Zone;
}