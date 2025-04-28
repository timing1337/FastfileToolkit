using FastfileToolkit.Fastfiles;
using FastfileToolkit.Native;
using Serilog;
using System.Runtime.InteropServices;
using Windows.Win32;
using Windows.Win32.System.Memory;

namespace FastfileToolkit.Games;
public abstract unsafe class BaseGame
{
    public abstract string Name { get; }
    public abstract string ExecutableName { get; }
    public abstract string OodleLibraryName { get; }
    public abstract Offset GameOffset { get; }

    public string GamePath { get; }

    private Module Module { get; set; }
    private Fastfile CurrentFile;

    private byte* AlignmentBuffer;

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

    public BaseGame(string path)
    {
        GamePath = path;
        LoadGame(path);
        ResolveOffset();
        AttachHooks();
    }

    public void LoadGame(string path)
    {
        PInvoke.SetDllDirectory(path);
        string executable = Path.Join(Toolkit.DumpDirectory, ExecutableName);
        Module = Module.Load(executable, path);
        File.WriteAllText($"D:/{Module.BaseAddress:X}.txt", "");
    }

    public void ResolveOffset()
    {
        Load_ArchiveData = Marshal.GetDelegateForFunctionPointer<Load_ArchiveDataFunc>(Module.BaseAddress + GameOffset.Load_ArchiveData);
        DB_InitStreams = Marshal.GetDelegateForFunctionPointer<DB_InitStreamsFunc>(Module.BaseAddress + GameOffset.DB_InitStreams);
        DB_PatchMem_BeginLoad = Marshal.GetDelegateForFunctionPointer<DB_PatchMem_BeginLoadFunc>(Module.BaseAddress + GameOffset.DB_PatchMem_BeginLoad);
        LoadStream = Marshal.GetDelegateForFunctionPointer<LoadStreamFunc>(Module.BaseAddress + GameOffset.LoadStream);
        DecryptString = Marshal.GetDelegateForFunctionPointer<DecryptStringFunc>(Module.BaseAddress + GameOffset.DecryptString);

        foreach(var patch in GameOffset.Patches)
        {
            Log.Information("Applying {name} patch @ {offset:X}", patch.Name, patch.Offset);

            if(!PInvoke.VirtualProtect((void*)(Module.BaseAddress + patch.Offset), (nuint)patch.Replacement.Length, PAGE_PROTECTION_FLAGS.PAGE_EXECUTE_READWRITE, out PAGE_PROTECTION_FLAGS oldProtect))
            {
                Log.Error("Failed to change memory protection for patch");
                continue;
            }

            for (int i = 0; i < patch.Replacement.Length; i++)
            {
                *(byte*)(Module.BaseAddress + patch.Offset + i) = patch.Replacement[i];
            }
        }
    }

    public void AttachHooks()
    {
        DB_ReadXFileHook = new NativeHook<DB_ReadXFile>(Module.BaseAddress + GameOffset.DB_ReadXFile, DB_ReadXFileDetour);
        SL_GetStringOfSizeHook = new NativeHook<SL_GetStringOfSize>(Module.BaseAddress + GameOffset.SL_GetStringOfSize, SL_GetStringOfSizeDetour);
        DB_AddXAssetHook = new NativeHook<DB_AddXAssetFunc>(Module.BaseAddress + GameOffset.DB_AddXAsset, DB_AddXAssetDetour);
    }

    public void DB_ReadXFileDetour(byte* pos, ulong size)
    {
        if (size > 2)
        {
            Log.Information("DB_ReadXFile size: {size}", size);
        }
        byte[] data = CurrentFile.Reader.ReadBytes((int)size);
        for (ulong i = 0; i < size; i++)
        {
            pos[i] = data[i];
        }
    }

    public char* SL_GetStringOfSizeDetour(nint result, char* ptr, uint user, ulong size, int type)
    {
        if ((*ptr & 0xC0) == 0x80)
        {
            nint container = Marshal.AllocHGlobal(4096);
            char* decrypted = DecryptString(container, 4096, ptr, (void*)0);
            Marshal.FreeHGlobal(container);
            ptr = decrypted;
        }
        return ptr;
    }

    public void DB_AddXAssetDetour(uint type, nint assetPtr)
    {
        nint asset = *(nint*)assetPtr;
        ulong hash = *(ulong*)asset;
    }

    public void LoadZone(string zone)
    {
        CurrentFile = new FastfileV1(zone);
        DB_InitStreams(CurrentFile.MemoryBlocks);
        DB_PatchMem_BeginLoad();
        Load_ArchiveData((void*)null, CurrentFile.AssetList, (char*)Marshal.StringToHGlobalAnsi(zone), false);
    }
}