using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastfileToolkit.Fastfiles;
public class BDiffWindowSizes {

    public ulong destWindow;
    public ulong sourceWindow;
    public ulong diffWindow;

    public static BDiffWindowSizes Read(BinaryReader reader) {
        BDiffWindowSizes windowSizes = new BDiffWindowSizes();
        windowSizes.destWindow = reader.ReadUInt64();
        windowSizes.sourceWindow = reader.ReadUInt64();
        windowSizes.diffWindow = reader.ReadUInt64();
        return windowSizes; 
    }
}