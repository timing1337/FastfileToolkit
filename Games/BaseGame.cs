using FastfileToolkit.Fastfiles;
using FastfileToolkit.Native;
using Newtonsoft.Json;
using Serilog;
using System.Runtime.InteropServices;
using System.Text;
using Windows.Win32;
using Windows.Win32.System.Memory;

namespace FastfileToolkit.Games;
public abstract unsafe class BaseGame {
    public abstract string Name { get; }
    public abstract string ExecutableName { get; }
    public abstract Offset GameOffset { get; }

    public string GamePath { get; }
    public Module Module { get; set; }

    public Fastfile CurrentFile { get; set; }

    public Dictionary<ulong, nint> StringTable = new();

    public delegate void Load_ArchiveDataFunc(void* zoneMem, void* assetList, char* ffName, bool wasPaused);
    public Load_ArchiveDataFunc Load_ArchiveData;

    public delegate void DB_ReadXFile(byte* pos, ulong size);
    public NativeHook<DB_ReadXFile> DB_ReadXFileHook;

    public delegate void DB_InitStreamsFunc(void* blocks);
    public DB_InitStreamsFunc DB_InitStreams;

    public delegate void DB_PatchMem_BeginLoadFunc();
    public DB_PatchMem_BeginLoadFunc DB_PatchMem_BeginLoad;

    public delegate bool LoadStreamFunc(int streamStart, void* a2, ulong size);
    public LoadStreamFunc LoadStream;

    public delegate char* SL_GetStringOfSize(nint result, char* ptr, uint user, ulong size, int type);
    public NativeHook<SL_GetStringOfSize> SL_GetStringOfSizeHook;

    public delegate char* DecryptStringFunc(nint container, uint size, char* str, void* unk);
    public DecryptStringFunc DecryptString;

    public delegate void DB_AddXAssetFunc(uint type, nint assetPtr);
    public NativeHook<DB_AddXAssetFunc> DB_AddXAssetHook;

    public delegate char* GetXAssetTypeNameFunc(uint type);
    public GetXAssetTypeNameFunc GetXAssetTypeName;

    public delegate ulong j_CoD_XXH64Func(nint data, ulong size, ulong seed);
    public j_CoD_XXH64Func j_CoD_XXH64;

    public BaseGame(string path) {
        GamePath = path;
        LoadGame(path);
        ResolveOffset();
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
        CurrentFile = new Fastfile(zone);
        DB_InitStreams(CurrentFile.MemoryBlocks);
        DB_PatchMem_BeginLoad();
        void* zoneMem = NativeMemory.AllocZeroed(1024);
        Load_ArchiveData(zoneMem, CurrentFile.AssetList, (char*)Marshal.StringToHGlobalAnsi(zone), false);
    }

    public virtual void ResolveOffset() {
        Load_ArchiveData = Marshal.GetDelegateForFunctionPointer<Load_ArchiveDataFunc>(Module.BaseAddress + GameOffset.Load_ArchiveData);
        DB_InitStreams = Marshal.GetDelegateForFunctionPointer<DB_InitStreamsFunc>(Module.BaseAddress + GameOffset.DB_InitStreams);
        DB_PatchMem_BeginLoad = Marshal.GetDelegateForFunctionPointer<DB_PatchMem_BeginLoadFunc>(Module.BaseAddress + GameOffset.DB_PatchMem_BeginLoad);
        LoadStream = Marshal.GetDelegateForFunctionPointer<LoadStreamFunc>(Module.BaseAddress + GameOffset.LoadStream);
        DecryptString = Marshal.GetDelegateForFunctionPointer<DecryptStringFunc>(Module.BaseAddress + GameOffset.DecryptString);
        j_CoD_XXH64 = Marshal.GetDelegateForFunctionPointer<j_CoD_XXH64Func>(Module.BaseAddress + GameOffset.j_CoD_XXH64);
        SL_GetStringOfSizeHook = new NativeHook<SL_GetStringOfSize>(Module.BaseAddress + GameOffset.SL_GetStringOfSize, SL_GetStringOfSizeDetour);
        GetXAssetTypeName = Marshal.GetDelegateForFunctionPointer<GetXAssetTypeNameFunc>(Module.BaseAddress + GameOffset.GetXAssetTypeName);

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

    public virtual void AttachHooks() {
        DB_ReadXFileHook = new NativeHook<DB_ReadXFile>(Module.BaseAddress + GameOffset.DB_ReadXFile, DB_ReadXFileDetour);
    }

    public void DB_ReadXFileDetour(byte* pos, ulong size) {
        byte[] data = CurrentFile.Reader.ReadBytes((int)size);
        for (ulong i = 0; i < size; i++) {
            pos[i] = data[i];
        }
    }
    public char* SL_GetStringOfSizeDetour(nint result, char* ptr, uint user, ulong size, int type) {
        if ((*ptr & 0xC0) == 0x80) {
            nint container = Marshal.AllocHGlobal(4096);
            char* decrypted = DecryptString(container, 4096, ptr, (void*)0);
            Marshal.FreeHGlobal(container);
            ptr = decrypted;
        }
        return ptr;
    }

    public abstract void Initialize();
}