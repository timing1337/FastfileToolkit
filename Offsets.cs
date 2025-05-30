using FastfileToolkit.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastfileToolkit;

public struct Patch {
    public string Name;
    public nint Offset;
    public byte[] Replacement;
}

public struct Offset {
    public nint DB_PatchMem_BeginLoad;
    public nint Load_ArchiveData;
    public nint DB_ReadXFile;
    public nint j_CoD_XXH64;

    //Stream
    public nint DB_InitLoadStreams;
    public nint DB_InitStreams;

    //String
    public nint SL_GetStringOfSize;
    public nint DecryptString;

    //Asset
    public nint GetXAssetTypeName;
    public nint DB_AddXAsset;
    public nint DB_GetXAsset;

    // Black Ops 6 specific patches
    public nint DB_RemapAssetType;

    public Patch[] Patches;
}