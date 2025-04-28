using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastfileToolkit.Bdiff;

public unsafe struct VcdState
{
    public uint next_slot;
    public ulong[] anear = new ulong[4];
    public ulong[] asame = new ulong[768];
    public byte* pAddr;

    public VcdState() { }
}
