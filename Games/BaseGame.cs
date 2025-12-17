using FastfileToolkit.Assets;
using FastfileToolkit.Fastfiles;
using FastfileToolkit.Native;
using FastfileToolkit.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using Windows.Win32;
using Windows.Win32.System.Memory;

namespace FastfileToolkit.Games;
public abstract unsafe class BaseGame {
    public abstract string Name { get; }
    public abstract string ExecutableName { get; }
    public abstract Offset GameOffset { get; }

    public string GamePath { get; }
    public Module Module { get; set; }

    public string CurrrentLoadingZone = null;

    public Dictionary<ulong, XAsset> LoadedAssets = new();
    public Dictionary<string, Fastfile> LoadedFastfiles = new();
    public Dictionary<uint, string> AssetTypes = new();
    public Dictionary<uint, nint> LoadedStrings = new();
    public Dictionary<ulong, ulong> UnlinkedAssets = new();

    public Dictionary<ulong, string> PotentialHashes = new();

    public delegate void DB_InitStateFunc(void* loadState);
    public DB_InitStateFunc DB_InitState;

    public delegate void Load_ArchiveDataFunc(void* loadState, void* zoneMem, void* assetList, char* ffName, bool wasPaused);
    public Load_ArchiveDataFunc Load_ArchiveData;

    public delegate void DB_ReadXFile(void* loadState, byte* pos, ulong size);
    public NativeHook<DB_ReadXFile> DB_ReadXFileHook;

    public delegate void DB_InitStreamsFunc(void* loadState, void* blocks);
    public NativeHook<DB_InitStreamsFunc> DB_InitStreamsHook;

    public delegate void DB_InitLoadStreamsFunc(void* loadState, void* zoneMem);
    public DB_InitLoadStreamsFunc DB_InitLoadStreams;

    public delegate void DB_PatchMem_BeginLoadFunc(void* loadState);
    public DB_PatchMem_BeginLoadFunc DB_PatchMem_BeginLoad;

    public delegate char* SL_GetStringOfSize(nint result, char* ptr, uint user, ulong size, int type);
    public NativeHook<SL_GetStringOfSize> SL_GetStringOfSizeHook;

    public delegate char* DecryptStringFunc(void* container, uint size, char* str, void* unk);
    public DecryptStringFunc DecryptString;

    public delegate nint DB_AddXAssetFunc(nint stream, uint type, nint assetPtr);
    public NativeHook<DB_AddXAssetFunc> DB_AddXAssetHook;

    public delegate nint DB_GetXAssetFunc(uint type, ulong hash, nint assetNamePtr);
    public NativeHook<DB_GetXAssetFunc> DB_GetXAssetHook;

    public delegate char* GetXAssetTypeNameFunc(uint type);
    public GetXAssetTypeNameFunc GetXAssetTypeName;

    public delegate ulong j_CoD_XXH64Func(nint data, ulong size, ulong seed);
    public j_CoD_XXH64Func j_CoD_XXH64;

    public delegate ulong Hash_ScriptStringHashFunc(nint ptr, ulong size, long seed);
    public Hash_ScriptStringHashFunc Hash_ScriptStringHash;

    public delegate nint memsetFunc(nint dest, byte value, uint count);
    public NativeHook<memsetFunc> memsetHook;

    public delegate void ReadXAsset(nint loadState, uint a2, nint asset, nint assetTypePtr);
    public NativeHook<ReadXAsset> ReadXAssetHook;

    public delegate void ReadString(nint loadState, nint strPtr);
    public NativeHook<ReadString> ReadStringHook;

    public BaseGame(string path) {
        GamePath = path;
        LoadGame(path);
        ResolveOffset();
        ApplyPatches();
        AttachHooks();
        Initialize();
    }

    public void LoadGame(string path) {
        PInvoke.SetDllDirectory(path);
        string executable = Path.Join(Toolkit.DumpDirectory, ExecutableName);
        if (!File.Exists(executable)) {
            throw new FileNotFoundException($"Executable {ExecutableName} not found in {Toolkit.DumpDirectory}");
        }
        Module = Module.Load(executable, path);
    }

    public void LoadZone(string zone) {
        if (LoadedFastfiles.ContainsKey(zone)) {
            throw new Exception($"Zone {zone} is already loaded");
        }

        CurrrentLoadingZone = zone;
        var fastfile = new Fastfile(Path.Join(GamePath, "cod25"), zone);
        LoadedFastfiles[zone] = fastfile;

        void* loadState = NativeMemory.AllocZeroed(65535);
        void* tempData = NativeMemory.AllocZeroed(65535 * 32);
        void* zoneMem = NativeMemory.AllocZeroed(65536);
        void* additionalBuffer = NativeMemory.AllocZeroed(65535 * 32);

        try
        {
            *(nint*)((nint)zoneMem + 120) = (nint)tempData;
            DB_InitState(loadState);
            DB_InitLoadStreams(loadState, zoneMem);

            *(nint*)((nint)loadState + 1552) = (nint)additionalBuffer;
            DB_PatchMem_BeginLoad((void*)((nint)loadState + 1552));

            var zoneName = Marshal.StringToHGlobalAnsi(zone);
            Load_ArchiveData(loadState, zoneMem, fastfile.AssetList, (char*)zoneName, false);
            Marshal.FreeHGlobal(zoneName);
        }
        catch(Exception ex)
        {
            Log.Error("Error loading zone {zone}: {ex}", zone, ex);
        }
        finally
        {
            NativeMemory.Free(zoneMem);
            NativeMemory.Free(loadState);
            NativeMemory.Free(tempData);
            NativeMemory.Free(additionalBuffer);
            CurrrentLoadingZone = null;
        }

        //Free
    }

    public virtual void ResolveOffset() {
        Load_ArchiveData = Marshal.GetDelegateForFunctionPointer<Load_ArchiveDataFunc>(Module.BaseAddress + GameOffset.Load_ArchiveData);
        DB_InitLoadStreams = Marshal.GetDelegateForFunctionPointer<DB_InitLoadStreamsFunc>(Module.BaseAddress + GameOffset.DB_InitLoadStreams);
        DB_PatchMem_BeginLoad = Marshal.GetDelegateForFunctionPointer<DB_PatchMem_BeginLoadFunc>(Module.BaseAddress + GameOffset.DB_PatchMem_BeginLoad);
        DecryptString = Marshal.GetDelegateForFunctionPointer<DecryptStringFunc>(Module.BaseAddress + GameOffset.DecryptString);
        j_CoD_XXH64 = Marshal.GetDelegateForFunctionPointer<j_CoD_XXH64Func>(Module.BaseAddress + GameOffset.j_CoD_XXH64);
        GetXAssetTypeName = Marshal.GetDelegateForFunctionPointer<GetXAssetTypeNameFunc>(Module.BaseAddress + GameOffset.GetXAssetTypeName);
        DB_InitState = Marshal.GetDelegateForFunctionPointer<DB_InitStateFunc>(Module.BaseAddress + GameOffset.DB_InitState);
        Hash_ScriptStringHash = Marshal.GetDelegateForFunctionPointer<Hash_ScriptStringHashFunc>(Module.BaseAddress + GameOffset.Hash_ScriptStringHash);
    }

    public static ulong HashAsset(string data)
    {
        data = data.ToLower();
        ulong result = 0x47F5817A5EF961BA;
        for (int i = 0; i < data.Length; i++)
        {
            ulong value = data[i];
            if (value == '\\')
                value = '/';
            result = 0x100000001B3 * (value ^ result);
        }
        return result & 0x7FFFFFFFFFFFFFFF;
    }

    public virtual void AttachHooks() {
        memsetHook = new NativeHook<memsetFunc>(Module.BaseAddress + GameOffset.memcmp, (nint dest, byte value, uint count) => {
            for (uint i = 0; i < count; i++) {
                *((byte*)(dest + i)) = value;
            }
            return dest;
        });

        ReadStringHook = new NativeHook<ReadString>(Module.BaseAddress + 0x30EE650, (nint loadState, nint strPtr) => {
            ReadStringHook.Trampoline(loadState, strPtr);
            var @str = *(nint*)strPtr;
            var toStr = Marshal.PtrToStringAnsi(@str);

            var hash = HashAsset(toStr);
            if (!PotentialHashes.ContainsKey(hash))
            {
                PotentialHashes[hash] =toStr;
            }
        });

        DB_ReadXFileHook = new NativeHook<DB_ReadXFile>(Module.BaseAddress + GameOffset.DB_ReadXFile, DB_ReadXFileDetour);
        DB_AddXAssetHook = new NativeHook<DB_AddXAssetFunc>(Module.BaseAddress + GameOffset.DB_AddXAsset, DB_AddXAssetDetour);
        DB_GetXAssetHook = new NativeHook<DB_GetXAssetFunc>(Module.BaseAddress + GameOffset.DB_GetXAsset, DB_GetXAssetDetour);

        DB_InitStreamsHook = new NativeHook<DB_InitStreamsFunc>(Module.BaseAddress + GameOffset.DB_InitStreams, DB_InitStreamsDetour);
        SL_GetStringOfSizeHook = new NativeHook<SL_GetStringOfSize>(Module.BaseAddress + GameOffset.SL_GetStringOfSize, SL_GetStringOfSizeDetour);
    }

    public void ApplyPatches() {
        foreach (var patch in GameOffset.Patches) {
            Log.Information("Applying {name} patch @ {offset:X}", patch.Name, patch.Offset);

            if (!PInvoke.VirtualProtect((void*)(Module.BaseAddress + patch.Offset), (nuint)patch.Replacement.Length, PAGE_PROTECTION_FLAGS.PAGE_EXECUTE_READWRITE, out PAGE_PROTECTION_FLAGS oldProtect)) {
                Log.Error("Failed to change memory protection for patch");
                continue;
            }

            for (int i = 0; i < patch.Replacement.Length; i++) {
                *(byte*)(Module.BaseAddress + patch.Offset + i) = patch.Replacement[i];
            }
        }
    }

    public void DB_ReadXFileDetour(void* a1, byte* pos, ulong size) {
        Fastfile fastfile = LoadedFastfiles[CurrrentLoadingZone];

        fixed (byte* data = &fastfile.Data[fastfile.Offset]) {
            NativeMemory.Copy(data, pos, (nuint)size);
            fastfile.Offset += size;
        }
    }

    public nint DB_AddXAssetDetour(nint stream, uint type, nint assetPtr) {
        var pointer = *(nint*)assetPtr;
        var hash = *(ulong*)pointer;

        hash = hash & 0x7FFFFFFFFFFFFFFF;

        LoadedAssets[hash] = new XAsset {
            Asset = pointer,
            Type = type,
            Hash = hash,
            Zone = CurrrentLoadingZone
        };

        return pointer;
    }

    public nint DB_GetXAssetDetour(uint type, ulong hash, nint assetNamePtr) {
        hash = hash & 0x7FFFFFFFFFFFFFFF;
        if (!LoadedAssets.ContainsKey(hash)) {
            //Okay this is a MASSIVE issue,
            //It's trying to find something that isn't loaded yet..which is a bad idea
            //TODO: Fix this
            //We can hook LoadStream and save the pointer of the unloaded asset, put it to UnloadedAssets and wait until the proper asset is loaded\
            return 0;
        }
        return LoadedAssets[hash].Asset;
    }

    public void DB_InitStreamsDetour(void* loadState, void* blocks) {
        DB_InitStreamsHook.Trampoline(loadState, LoadedFastfiles[CurrrentLoadingZone].MemoryBlocks);
    }

    public char* SL_GetStringOfSizeDetour(nint result, char* ptr, uint user, ulong size, int type) {
        if ((*ptr & 0xC0) == 0x80)
        {
            void* container = NativeMemory.AllocZeroed(8192);

            char* decrypted = DecryptString(container, 8192, ptr, (void*)0);
            string decryptedString = Marshal.PtrToStringUTF8((nint)decrypted);

            var hash = HashAsset(decryptedString);
            if (!PotentialHashes.ContainsKey(hash))
            {
                PotentialHashes[hash] = decryptedString;
            }
            nint stringPtr = Marshal.StringToHGlobalAnsi(decryptedString);

            NativeMemory.Free(container);
            
            ptr = (char*)stringPtr;
        }
        return ptr;
    }

    public virtual void Initialize() {
        uint index = 0;
        while (true) {
            string assetName = Marshal.PtrToStringUTF8((nint)GetXAssetTypeName(index));
            AssetTypes[index] = assetName;
            Log.Information("Asset Type {index}: {assetName}", index, assetName);
            index++;
            if (assetName == "assetlist") break;
        }
    }
}