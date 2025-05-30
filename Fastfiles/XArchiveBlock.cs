using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FastfileToolkit.Fastfiles;
public unsafe struct XArchiveBlock {
    public byte* Pointer;
    public ulong Size;

    public XArchiveBlock(ulong size, bool allocate = false) {
        Size = size;
        if (allocate) {
            Pointer = (byte*)NativeMemory.AlignedAlloc((nuint)size, 4096);
        } else {
            Pointer = null;
        }
    }

    public void Allocate() {
        if (Pointer != null || Size == 0)
            return;

        Pointer = (byte*)NativeMemory.AlignedAlloc((nuint)Size, 4096);
    }

    public void Free() {
        if (Pointer == null)
            return;
        NativeMemory.AlignedFree((void*)Pointer);
        Pointer = null;
        Size = 0;
    }
}