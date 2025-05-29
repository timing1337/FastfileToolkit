using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastfileToolkit.Utils;

public static class BinaryUtils {
    public static ulong ReadULEB128(byte[] buffer, ref long offset) {
        ulong result = 0;
        sbyte b;

        do {
            b = (sbyte)buffer[offset++];
            result = (result << 7) | (byte)(b & 0x7F);
        } while (b < 0);

        return result;
    }

    public static unsafe ulong ReadULEB128(nint ptr) {
        ulong result = 0;
        sbyte b;
        do {
            b = *(*(sbyte**)ptr)++;
            result = (result << 7) | (byte)(b & 0x7F);
        }
        while (b < 0);
        return result;
    }
}