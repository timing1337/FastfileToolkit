using FastfileToolkit.Native;
using FastfileToolkit.Utils;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FastfileToolkit.Games;

class BlackOps6 : BaseGame {
    public override string Name => "Call of Duty: Black Ops 6";
    public override string ExecutableName => "cod.exe";

    public Dictionary<uint, uint> HashedAssetTypeMapping = new();

    public delegate uint DB_RemapAssetTypeFunc(uint type);
    public NativeHook<DB_RemapAssetTypeFunc> DB_RemapAssetTypeHook;

    public override Offset GameOffset => new Offset() {
        DB_InitStreams = 0x2C23FF0,
        DB_PatchMem_BeginLoad = 0x2C15D30,
        Load_ArchiveData = 0x2C62230,
        DB_ReadXFile = 0x2C10D50,
        SL_GetStringOfSize = 0x817CB30,
        DecryptString = 0x6D9A190,
        GetXAssetTypeName = 0x88C3AE0,
        DB_RemapAssetType = 0x88C38A0,
        j_CoD_XXH64 = 0x8B48670,

        Patches = [new Patch() {
            Name = "SL_GetStringOfSize patch bottom function",
            Offset = 0x2C62880,
            Replacement = new byte[]{ 0xC3 }
        }]
    };

    public BlackOps6(string path) : base(path) {}

    public override void AttachHooks() {
        base.AttachHooks();
        DB_RemapAssetTypeHook = new NativeHook<DB_RemapAssetTypeFunc>(Module.BaseAddress + GameOffset.DB_RemapAssetType, DB_RemapAssetTypeDetour);
    }

    public uint DB_RemapAssetTypeDetour(uint hashedType) {
        Log.Information("Remapping asset type {type:X}", hashedType);
        return HashedAssetTypeMapping[hashedType];
    }

    public unsafe override void Initialize() {
        uint index = 0;
        while (true) {
            string assetName = Marshal.PtrToStringUTF8((nint)GetXAssetTypeName(index++));
            HashedAssetTypeMapping[HashUtils.Fnv1a32(assetName)] = index;
            Log.Information("Asset type {name} has index {index}", assetName, index);
            if (assetName == "assetlist") break;
        }
    }
}
