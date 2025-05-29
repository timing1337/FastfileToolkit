using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FastfileToolkit.Fastfiles;

[StructLayout(LayoutKind.Sequential, Size = 8)]
public struct Instruction {
    public ushort op;
    public ushort mode;
    public uint size;
}

public class Vcd {
    public static Instruction[] Instructions = new Instruction[512] {
        new Instruction {
            op = 2,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 1
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 2
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 3
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 4
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 5
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 6
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 7
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 8
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 9
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 10
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 11
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 12
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 13
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 14
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 15
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 16
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 17
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 0,
            size = 4
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 0,
            size = 5
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 0,
            size = 6
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 0,
            size = 7
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 0,
            size = 8
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 0,
            size = 9
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 0,
            size = 10
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 0,
            size = 11
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 0,
            size = 12
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 0,
            size = 13
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 0,
            size = 14
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 0,
            size = 15
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 0,
            size = 16
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 0,
            size = 17
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 0,
            size = 18
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 1,
            size = 0
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 1,
            size = 4
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 1,
            size = 5
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 1,
            size = 6
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 1,
            size = 7
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 1,
            size = 8
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 1,
            size = 9
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 1,
            size = 10
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 1,
            size = 11
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 1,
            size = 12
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 1,
            size = 13
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 1,
            size = 14
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 1,
            size = 15
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 1,
            size = 16
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 1,
            size = 17
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 1,
            size = 18
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 2,
            size = 0
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 2,
            size = 4
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 2,
            size = 5
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 2,
            size = 6
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 2,
            size = 7
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 2,
            size = 8
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 2,
            size = 9
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 2,
            size = 10
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 2,
            size = 11
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 2,
            size = 12
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 2,
            size = 13
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 2,
            size = 14
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 2,
            size = 15
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 2,
            size = 16
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 2,
            size = 17
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 2,
            size = 18
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 3,
            size = 0
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 3,
            size = 4
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 3,
            size = 5
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 3,
            size = 6
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 3,
            size = 7
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 3,
            size = 8
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 3,
            size = 9
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 3,
            size = 10
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 3,
            size = 11
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 3,
            size = 12
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 3,
            size = 13
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 3,
            size = 14
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 3,
            size = 15
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 3,
            size = 16
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 3,
            size = 17
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 3,
            size = 18
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 4,
            size = 0
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 4,
            size = 4
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 4,
            size = 5
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 4,
            size = 6
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 4,
            size = 7
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 4,
            size = 8
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 4,
            size = 9
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 4,
            size = 10
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 4,
            size = 11
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 4,
            size = 12
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 4,
            size = 13
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 4,
            size = 14
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 4,
            size = 15
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 4,
            size = 16
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 4,
            size = 17
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 4,
            size = 18
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 5,
            size = 0
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 5,
            size = 4
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 5,
            size = 5
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 5,
            size = 6
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 5,
            size = 7
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 5,
            size = 8
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 5,
            size = 9
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 5,
            size = 10
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 5,
            size = 11
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 5,
            size = 12
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 5,
            size = 13
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 5,
            size = 14
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 5,
            size = 15
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 5,
            size = 16
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 5,
            size = 17
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 5,
            size = 18
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 6,
            size = 0
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 6,
            size = 4
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 6,
            size = 5
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 6,
            size = 6
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 6,
            size = 7
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 6,
            size = 8
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 6,
            size = 9
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 6,
            size = 10
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 6,
            size = 11
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 6,
            size = 12
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 6,
            size = 13
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 6,
            size = 14
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 6,
            size = 15
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 6,
            size = 16
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 6,
            size = 17
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 6,
            size = 18
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 7,
            size = 0
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 7,
            size = 4
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 7,
            size = 5
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 7,
            size = 6
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 7,
            size = 7
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 7,
            size = 8
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 7,
            size = 9
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 7,
            size = 10
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 7,
            size = 11
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 7,
            size = 12
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 7,
            size = 13
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 7,
            size = 14
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 7,
            size = 15
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 7,
            size = 16
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 7,
            size = 17
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 7,
            size = 18
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 8,
            size = 0
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 8,
            size = 4
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 8,
            size = 5
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 8,
            size = 6
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 8,
            size = 7
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 8,
            size = 8
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 8,
            size = 9
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 8,
            size = 10
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 8,
            size = 11
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 8,
            size = 12
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 8,
            size = 13
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 8,
            size = 14
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 8,
            size = 15
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 8,
            size = 16
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 8,
            size = 17
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 3,
            mode = 8,
            size = 18
        },
        new Instruction {
            op = 0,
            mode = 0,
            size = 0
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 1
        },
        new Instruction {
            op = 3,
            mode = 0,
            size = 4
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 1
        },
        new Instruction {
            op = 3,
            mode = 0,
            size = 5
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 1
        },
        new Instruction {
            op = 3,
            mode = 0,
            size = 6
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 2
        },
        new Instruction {
            op = 3,
            mode = 0,
            size = 4
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 2
        },
        new Instruction {
            op = 3,
            mode = 0,
            size = 5
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 2
        },
        new Instruction {
            op = 3,
            mode = 0,
            size = 6
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 3
        },
        new Instruction {
            op = 3,
            mode = 0,
            size = 4
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 3
        },
        new Instruction {
            op = 3,
            mode = 0,
            size = 5
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 3
        },
        new Instruction {
            op = 3,
            mode = 0,
            size = 6
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 4
        },
        new Instruction {
            op = 3,
            mode = 0,
            size = 4
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 4
        },
        new Instruction {
            op = 3,
            mode = 0,
            size = 5
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 4
        },
        new Instruction {
            op = 3,
            mode = 0,
            size = 6
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 1
        },
        new Instruction {
            op = 3,
            mode = 1,
            size = 4
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 1
        },
        new Instruction {
            op = 3,
            mode = 1,
            size = 5
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 1
        },
        new Instruction {
            op = 3,
            mode = 1,
            size = 6
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 2
        },
        new Instruction {
            op = 3,
            mode = 1,
            size = 4
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 2
        },
        new Instruction {
            op = 3,
            mode = 1,
            size = 5
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 2
        },
        new Instruction {
            op = 3,
            mode = 1,
            size = 6
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 3
        },
        new Instruction {
            op = 3,
            mode = 1,
            size = 4
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 3
        },
        new Instruction {
            op = 3,
            mode = 1,
            size = 5
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 3
        },
        new Instruction {
            op = 3,
            mode = 1,
            size = 6
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 4
        },
        new Instruction {
            op = 3,
            mode = 1,
            size = 4
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 4
        },
        new Instruction {
            op = 3,
            mode = 1,
            size = 5
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 4
        },
        new Instruction {
            op = 3,
            mode = 1,
            size = 6
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 1
        },
        new Instruction {
            op = 3,
            mode = 2,
            size = 4
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 1
        },
        new Instruction {
            op = 3,
            mode = 2,
            size = 5
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 1
        },
        new Instruction {
            op = 3,
            mode = 2,
            size = 6
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 2
        },
        new Instruction {
            op = 3,
            mode = 2,
            size = 4
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 2
        },
        new Instruction {
            op = 3,
            mode = 2,
            size = 5
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 2
        },
        new Instruction {
            op = 3,
            mode = 2,
            size = 6
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 3
        },
        new Instruction {
            op = 3,
            mode = 2,
            size = 4
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 3
        },
        new Instruction {
            op = 3,
            mode = 2,
            size = 5
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 3
        },
        new Instruction {
            op = 3,
            mode = 2,
            size = 6
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 4
        },
        new Instruction {
            op = 3,
            mode = 2,
            size = 4
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 4
        },
        new Instruction {
            op = 3,
            mode = 2,
            size = 5
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 4
        },
        new Instruction {
            op = 3,
            mode = 2,
            size = 6
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 1
        },
        new Instruction {
            op = 3,
            mode = 3,
            size = 4
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 1
        },
        new Instruction {
            op = 3,
            mode = 3,
            size = 5
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 1
        },
        new Instruction {
            op = 3,
            mode = 3,
            size = 6
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 2
        },
        new Instruction {
            op = 3,
            mode = 3,
            size = 4
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 2
        },
        new Instruction {
            op = 3,
            mode = 3,
            size = 5
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 2
        },
        new Instruction {
            op = 3,
            mode = 3,
            size = 6
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 3
        },
        new Instruction {
            op = 3,
            mode = 3,
            size = 4
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 3
        },
        new Instruction {
            op = 3,
            mode = 3,
            size = 5
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 3
        },
        new Instruction {
            op = 3,
            mode = 3,
            size = 6
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 4
        },
        new Instruction {
            op = 3,
            mode = 3,
            size = 4
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 4
        },
        new Instruction {
            op = 3,
            mode = 3,
            size = 5
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 4
        },
        new Instruction {
            op = 3,
            mode = 3,
            size = 6
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 1
        },
        new Instruction {
            op = 3,
            mode = 4,
            size = 4
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 1
        },
        new Instruction {
            op = 3,
            mode = 4,
            size = 5
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 1
        },
        new Instruction {
            op = 3,
            mode = 4,
            size = 6
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 2
        },
        new Instruction {
            op = 3,
            mode = 4,
            size = 4
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 2
        },
        new Instruction {
            op = 3,
            mode = 4,
            size = 5
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 2
        },
        new Instruction {
            op = 3,
            mode = 4,
            size = 6
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 3
        },
        new Instruction {
            op = 3,
            mode = 4,
            size = 4
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 3
        },
        new Instruction {
            op = 3,
            mode = 4,
            size = 5
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 3
        },
        new Instruction {
            op = 3,
            mode = 4,
            size = 6
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 4
        },
        new Instruction {
            op = 3,
            mode = 4,
            size = 4
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 4
        },
        new Instruction {
            op = 3,
            mode = 4,
            size = 5
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 4
        },
        new Instruction {
            op = 3,
            mode = 4,
            size = 6
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 1
        },
        new Instruction {
            op = 3,
            mode = 5,
            size = 4
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 1
        },
        new Instruction {
            op = 3,
            mode = 5,
            size = 5
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 1
        },
        new Instruction {
            op = 3,
            mode = 5,
            size = 6
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 2
        },
        new Instruction {
            op = 3,
            mode = 5,
            size = 4
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 2
        },
        new Instruction {
            op = 3,
            mode = 5,
            size = 5
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 2
        },
        new Instruction {
            op = 3,
            mode = 5,
            size = 6
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 3
        },
        new Instruction {
            op = 3,
            mode = 5,
            size = 4
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 3
        },
        new Instruction {
            op = 3,
            mode = 5,
            size = 5
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 3
        },
        new Instruction {
            op = 3,
            mode = 5,
            size = 6
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 4
        },
        new Instruction {
            op = 3,
            mode = 5,
            size = 4
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 4
        },
        new Instruction {
            op = 3,
            mode = 5,
            size = 5
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 4
        },
        new Instruction {
            op = 3,
            mode = 5,
            size = 6
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 1
        },
        new Instruction {
            op = 3,
            mode = 6,
            size = 4
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 2
        },
        new Instruction {
            op = 3,
            mode = 6,
            size = 4
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 3
        },
        new Instruction {
            op = 3,
            mode = 6,
            size = 4
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 4
        },
        new Instruction {
            op = 3,
            mode = 6,
            size = 4
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 1
        },
        new Instruction {
            op = 3,
            mode = 7,
            size = 4
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 2
        },
        new Instruction {
            op = 3,
            mode = 7,
            size = 4
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 3
        },
        new Instruction {
            op = 3,
            mode = 7,
            size = 4
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 4
        },
        new Instruction {
            op = 3,
            mode = 7,
            size = 4
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 1
        },
        new Instruction {
            op = 3,
            mode = 8,
            size = 4
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 2
        },
        new Instruction {
            op = 3,
            mode = 8,
            size = 4
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 3
        },
        new Instruction {
            op = 3,
            mode = 8,
            size = 4
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 4
        },
        new Instruction {
            op = 3,
            mode = 8,
            size = 4
        },
        new Instruction {
            op = 3,
            mode = 0,
            size = 4
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 1
        },
        new Instruction {
            op = 3,
            mode = 1,
            size = 4
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 1
        },
        new Instruction {
            op = 3,
            mode = 2,
            size = 4
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 1
        },
        new Instruction {
            op = 3,
            mode = 3,
            size = 4
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 1
        },
        new Instruction {
            op = 3,
            mode = 4,
            size = 4
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 1
        },
        new Instruction {
            op = 3,
            mode = 5,
            size = 4
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 1
        },
        new Instruction {
            op = 3,
            mode = 6,
            size = 4
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 1
        },
        new Instruction {
            op = 3,
            mode = 7,
            size = 4
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 1
        },
        new Instruction {
            op = 3,
            mode = 8,
            size = 4
        },
        new Instruction {
            op = 1,
            mode = 0,
            size = 1
        },
    };
}