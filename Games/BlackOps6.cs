
using FastfileToolkit.Native;
using FastfileToolkit.Utils;
using Serilog;
using System.Runtime.InteropServices;


namespace FastfileToolkit.Games;

class BlackOps6 : BaseGame {
    public override string Name => "Call of Duty: Black Ops 6";
    public override string ExecutableName => "cod_dump.exe";

    public Dictionary<uint, uint> HashedAssetTypeMapping = new();

    public delegate uint DB_RemapAssetTypeFunc(uint type);
    public NativeHook<DB_RemapAssetTypeFunc> DB_RemapAssetTypeHook;

    public delegate bool GetLoadStreamFunctionFunc(nint streamFuncPrs, nint a1, nint a2, nint a3, nint a4);
    public NativeHook<GetLoadStreamFunctionFunc> GetLoadStreamFunctionHook;

    public delegate bool LoadStreamFunc(nint a1, bool a2, nint a3, nint a4);
    public LoadStreamFunc LoadStream;

    public override Offset GameOffset => new Offset() {


        DB_PatchMem_BeginLoad = 0x2C15D30,
        Load_ArchiveData = 0x2C62230,
        DB_ReadXFile = 0x2C10D50,
        j_CoD_XXH64 = 0x8B48670,

        //Stream
        DB_InitLoadStreams = 0x2C61DD0,
        DB_InitStreams = 0x2C23FF0,

        //String
        SL_GetStringOfSize = 0x817CB30,
        DecryptString = 0x6D9A190,

        //Asset

        GetXAssetTypeName = 0x88C3AE0,
        DB_AddXAsset = 0x2C1A1C0,
        DB_GetXAsset = 0x2C1C660,

        // Black Ops 6 specific patches
        DB_RemapAssetType = 0x88C38A0,

        Patches = [new Patch() {
            Name = "SL_GetStringOfSize patch bottom function",
            Offset = 0x2C62880,
            Replacement = new byte[]{ 0xC3 }
        }]
    };

    public BlackOps6(string path) : base(path) { }

    public override void AttachHooks() {
        base.AttachHooks();

        DB_RemapAssetTypeHook = new NativeHook<DB_RemapAssetTypeFunc>(Module.BaseAddress + GameOffset.DB_RemapAssetType, DB_RemapAssetTypeDetour);
    }

    public uint DB_RemapAssetTypeDetour(uint hashedType) {
        uint type = HashedAssetTypeMapping[hashedType];
        Log.Information("DB_RemapAssetType called with type {0}({1})", hashedType, type);
        return type;
    }

    public unsafe override void Initialize() {
        base.Initialize();
        foreach (var assetType in AssetTypes) {
            HashedAssetTypeMapping[HashUtils.Fnv1a32(assetType.Value)] = assetType.Key;
        }
    }
}
