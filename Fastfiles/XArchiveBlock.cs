using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FastfileToolkit.Fastfiles;
public unsafe struct XArchiveBlock {
    public byte* memory;
    public ulong size;

    public static XArchiveBlock Allocate(ulong size) {
        XArchiveBlock block = new XArchiveBlock();
        block.memory = (byte*)NativeMemory.AlignedAlloc((nuint)size, 4096);
        block.size = size;
        return block;
    }

    public void Free() {
        if (memory == null)
            return;
        NativeMemory.AlignedFree((void*)memory);
        memory = null;
        size = 0;
    }
}