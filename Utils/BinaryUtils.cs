using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastfileToolkit.Utils;

public static class BinaryUtils {
    public static ulong ReadULEB128(byte[] buffer, ref long offset) {
        sbyte b = (sbyte)buffer[offset++];
        ulong result = (ulong)(b & 0x7F);
        if(b < 0) {
            do {
                b = (sbyte)buffer[offset++];
                result = (result << 7) | (byte)(b & 0x7F);
            } while (b < 0);
        }
        return result;
    }
}