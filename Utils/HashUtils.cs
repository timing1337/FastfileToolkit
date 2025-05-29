using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastfileToolkit.Utils;

public static class HashUtils {
    public static uint Fnv1a32(string data) {
        uint hash = 0x811c9dc5;
        uint prime = 0x1000193;
        for (int i = 0; i < data.Length; ++i) {
            hash ^= (uint)data[i];
            hash *= prime;
        }
        return hash;
    }
}