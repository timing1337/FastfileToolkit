
using FastfileToolkit.Fastfiles;
using FastfileToolkit.Native;
using FastfileToolkit.Utils;
using Serilog;
using System.Runtime.InteropServices;


namespace FastfileToolkit.Games;

unsafe class BlackOps7 : BaseGame {
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
        DB_PatchMem_BeginLoad = 0x308E3E0,
        Load_ArchiveData = 0x30EE020,
        DB_ReadXFile = 0x30831E0,

        //Hash
        Hash_ScriptStringHash = 0x9D1C7B0,
        j_CoD_XXH64 = 0x9D150F0,

        //Stream
        DB_InitState = 0x30EDA60,
        DB_InitLoadStreams = 0x30EDAB0,
        DB_InitStreams = 0x309FC40,

        //String
        SL_GetStringOfSize = 0x93B2B60,
        DecryptString = 0x7B56E30,

        //Asset
        GetXAssetTypeName = 0x9C649A0,
        DB_AddXAsset = 0x30955E0,
        DB_GetXAsset = 0x30970F0,

        // Black Ops 6 specific patches
        LoadStreamPostion = 0x30EDB50,
        SaveStreamPostion = 0x30ED950,
        DB_RemapAssetType = 0x9C647B0,
        DB_LoadStoreScriptString = 0x30EECB0,

        memcmp = 0xA567F0C,

        Patches = [
            new Patch() {
                Name = "Disable DB_StoreScriptStringEntry",
                Offset = 0x30EE950,
                Replacement = new byte[] { 0xC3 },
            },

            new Patch(){
                Name = "Computeshader Patch #1",
                Offset = 0x3074610,
                Replacement = new byte[] { 0xC3 },
            },

            new Patch(){
                Name = "Libshader Patch #2",
                Offset = 0x30746E0,
                Replacement = new byte[] { 0xC3 },
            },

            new Patch(){ 
                Name = "External image data patch",
                Offset = 0x30746B0,
                Replacement = new byte[] { 0xC3 },
            },

            new Patch(){
                Name = "Disable Flag #1",
                Offset = 0x30F2210,
                Replacement = new byte[] { 0xC3 }
            },

            new Patch(){
                Name = "Disable soundblank #1",
                Offset = 0x9566480,
                Replacement = new byte[] { 0xC3 }
            },

            new Patch(){ 
                Name = "Disable dlogschema",
                Offset = 0x9D2CE80,
                Replacement = new byte[] { 0xC3 }
            },

            new Patch(){
                Name = "Disable StreamingInfo",
                Offset = 0x30ED620,
                Replacement = new byte[] { 0xC3 }
            },

            new Patch(){
                Name = "Disable TrZone",
                Offset = 0x7440110,
                Replacement = new byte[] { 0xC3 }
            },

            new Patch(){
                Name = "??",
                Offset = 0x9674B50,
                Replacement = new byte[] { 0xC3 }
            },

            new Patch(){
                Name = "??",
                Offset = 0x75EADF0,
                Replacement = new byte[] { 0xC3 }
            }
        ]
    };

    public BlackOps7(string path) : base(path) { }

    public override void AttachHooks()
    {
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
        var currentFastfile = LoadedFastfiles[CurrrentLoadingZone];
        currentFastfile.Offset += 4;
        return pos;
    }

    public unsafe override void Initialize() {
        base.Initialize();
        foreach (var assetType in AssetTypes) {
            HashedAssetTypeMapping[HashUtils.Fnv1a32(assetType.Value)] = assetType.Key;
        }
    }
}
