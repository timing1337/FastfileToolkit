using FastfileToolkit.Fastfiles;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastfileToolkit.Games;

public class ModernWarfare4 : BaseGame
{
    public override string Name => "Call of Duty: Modern Warfare II (2022)";
    public override string ExecutableName => "cod22-cod.exe";
    public override string OodleLibraryName => "oo2core_8_win64.dll";

    public override Offset GameOffset => new Offset() {
        DB_InitStreams = 0x31D68A0,
        DB_PatchMem_BeginLoad = 0x2853E20,
        Load_ArchiveData = 0x31D6E20,
        DB_ReadXFile = 0x31CC720,
        LoadStream = 0x31D6F60,
        GetXAssetTypeName = 0x31CAE70,
        SL_GetStringOfSize = 0x334D1B0,
        DecryptString = 0x3C65850,
        DB_AddXAsset = 0x31CE420,

        Patches = new Patch[]
        {
            new Patch(){
                Name = "g_dbPreloading",
                Offset = 0x31D01B0,
                Replacement = new byte[]
                {
                    0xC3
                }
            },
            new Patch(){
                Name = "Asset type 87",
                Offset = 0x2856900,
                Replacement = new byte[]
                {
                    0xC3
                }
            },
            new Patch(){
                Name = "Asset type 19",
                Offset = 0x2F70CE0,
                Replacement = new byte[]
                {
                    0xC3
                }
            },
        }
    };

    public ModernWarfare4(string path) : base(path)
    {
    }
}