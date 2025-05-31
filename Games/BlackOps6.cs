
using FastfileToolkit.Native;
using FastfileToolkit.Utils;
using Serilog;
using System.Runtime.InteropServices;


namespace FastfileToolkit.Games;

unsafe class BlackOps6 : BaseGame {
    public override string Name => "Call of Duty: Black Ops 6";
    public override string ExecutableName => "cod_dump.exe";

    public Dictionary<uint, uint> HashedAssetTypeMapping = new();
    public Dictionary<ulong, nint> StreamPositions = new();

    public delegate uint DB_RemapAssetTypeFunc(uint type);
    public NativeHook<DB_RemapAssetTypeFunc> DB_RemapAssetTypeHook;

    public delegate void SaveStreamPostionFunc(void* loadState, ulong streamId, nint pos);
    public NativeHook<SaveStreamPostionFunc> SaveStreamPostionHook;

    public delegate void LoadStreamPostionFunc(void* loadState, ulong streamId, nint pos);
    public NativeHook<LoadStreamPostionFunc> LoadStreamPostionHook;

    public delegate nint DB_LoadStoreScriptStringFunc(void* loadState, nint pos);
    public NativeHook<DB_LoadStoreScriptStringFunc> DB_LoadStoreScriptStringHook;

    public override Offset GameOffset => new Offset() {
        DB_PatchMem_BeginLoad = 0x2DD53A0,
        Load_ArchiveData = 0x2E25580,
        DB_ReadXFile = 0x2DCF500,

        //Hash
        Hash_ScriptStringHash = 0x8DE6F00,
        j_CoD_XXH64 = 0x8DD23B0,

        //Stream
        DB_InitState = 0x2E25030,
        DB_InitLoadStreams = 0x2E25060,
        DB_InitStreams = 0x2DE3D00,

        //String
        SL_GetStringOfSize = 0x84137E0,
        DecryptString = 0x701DE80,

        //Asset
        GetXAssetTypeName = 0x8B5E020,
        DB_AddXAsset = 0x2DD9DD0,
        DB_GetXAsset = 0x2DDC2E0,

        // Black Ops 6 specific patches
        LoadStreamPostion = 0x2E25100,
        SaveStreamPostion = 0x2E24F20,
        DB_RemapAssetType = 0x8B5DDF0,
        DB_LoadStoreScriptString = 0x2E261A0,

        Patches = [
            new Patch() {
                Name = "Disable memset",
                Offset = 0x9696AC8,
                Replacement = new byte[] { 0xC3 },
            },
            //48 85 D2 0F 84 ? ? ? ? 48 89 5C 24 ? 48 89 74 24 ? 57 48 83 EC ? 48 8B 01
            new Patch() {
                Name = "Disable DB_StoreScriptStringEntry",
                Offset = 0x2E25E40,
                Replacement = new byte[] { 0xC3 },
            },
            //E8 ? ? ? ? 48 C7 43 ? ? ? ? ? 48 83 C4 ? 5B C3 CC
            new Patch(){
                Name = "Disable loading external image data",
                Offset = 0x6C3B8D0,
                Replacement = new byte[] { 0xC3 },
            },
            //0F B7 80 ? ? ? ? C3 90 40 53
            new Patch(){
                Name = "Disable soundbank transient loading flag #1",
                Offset = 0x2E284B0,
                Replacement = new byte[] { 0xC3 },
            },
            //E8 ? ? ? ? 48 8B 5C 24 ? 48 83 C4 ? 5F C3 4C 8D 44 24 ? BA ? ? ? ? 48 8B CB E8 ? ? ? ? 48 8B 5C 24 ? 48 89 07 48 83 C4 ? 5F C3 B8
            new Patch(){
                Name = "Disable soundbank transient loading flag #2",
                Offset = 0x8582B10,
                Replacement = new byte[] { 0xC3 },
            },
            //E8 ? ? ? ? E8 ? ? ? ? 48 8B 0B 8B D0 E8 ? ? ? ? 48 83 C4 ? 5B C3 90
            new Patch(){
                Name = "Compute shader null patch",
                Offset = 0x2DC1A50,
                Replacement = new byte[] { 0xC3 },
            },
            //E8 ? ? ? ? E8 ? ? ? ? 48 8B 0B 8B D0 E8 ? ? ? ? 48 83 C4 ? 5B C3 90
            new Patch(){
                Name = "Compute shader null patch #2",
                Offset = 0x6A7D0A0,
                Replacement = new byte[] { 0xC3 },
            },
            new Patch(){
                Name = "Libshader null patch",
                Offset = 0x6A7D100,
                Replacement = new byte[] { 0xC3 },
            },
            new Patch(){
                Name = "Dlogschema null patch",
                Offset = 0x8DF6780,
                Replacement = new byte[] { 0xC3 },
            },
            new Patch(){
                Name = "Disable post loading StreamingInfo",
                Offset = 0x2E24C50,
                Replacement = new byte[] { 0xC3 },
            },
            new Patch(){
                Name = "TransientZone null patch",
                Offset = 0x69C8160,
                Replacement = new byte[] { 0xC3 },
            },
            new Patch(){
                Name = "TransientZone null patch #2",
                Offset = 0x8631690,
                Replacement = new byte[] { 0xC3 },
            },
            new Patch(){
                Name = "TransientZone null patch #2",
                Offset = 0x6AF3D10,
                Replacement = new byte[] { 0xC3 },
            },
        ]
    };

    public BlackOps6(string path) : base(path) { }

    public override void AttachHooks() {
        base.AttachHooks();

        DB_RemapAssetTypeHook = new NativeHook<DB_RemapAssetTypeFunc>(Module.BaseAddress + GameOffset.DB_RemapAssetType, DB_RemapAssetTypeDetour);
        SaveStreamPostionHook = new NativeHook<SaveStreamPostionFunc>(Module.BaseAddress + GameOffset.SaveStreamPostion, SaveStreamPostionDetour);
        LoadStreamPostionHook = new NativeHook<LoadStreamPostionFunc>(Module.BaseAddress + GameOffset.LoadStreamPostion, LoadStreamPostionDetour);
        DB_LoadStoreScriptStringHook = new NativeHook<DB_LoadStoreScriptStringFunc>(Module.BaseAddress + GameOffset.DB_LoadStoreScriptString, DB_LoadStoreScriptStringDetour);
    }

    public unsafe void SaveStreamPostionDetour(void* loadState, ulong streamId, nint pos) {
        streamId = streamId & 0x1FFFFFFFFFFFFFFF;
        StreamPositions[streamId] = pos;
    }


    public unsafe void LoadStreamPostionDetour(void* loadState, ulong streamId, nint pos) {
        streamId = streamId & 0x1FFFFFFFFFFFFFFF;
        if (StreamPositions.ContainsKey(streamId)) {
            *(nint*)pos = StreamPositions[streamId];
        }
    }

    public uint DB_RemapAssetTypeDetour(uint hashedType) {
        return HashedAssetTypeMapping[hashedType];
    }

    public unsafe nint DB_LoadStoreScriptStringDetour(void* loadState, nint pos) {
        LoadedFastfiles[CurrrentLoadingZone].Offset += 4;
        return pos;
    }

    public unsafe override void Initialize() {
        base.Initialize();
        foreach (var assetType in AssetTypes) {
            HashedAssetTypeMapping[HashUtils.Fnv1a32(assetType.Value)] = assetType.Key;
        }
    }
}
