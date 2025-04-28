using FastfileToolkit.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastfileToolkit;

public struct Patch
{
    public string Name;
    public nint Offset;
    public byte[] Replacement;
}

public struct Offset
{
    public nint DB_InitStreams;
    public nint DB_PatchMem_BeginLoad;
    public nint Load_ArchiveData;
    public nint DB_ReadXFile;
    public nint LoadStream;
    public nint GetXAssetTypeName;
    public nint SL_GetStringOfSize;
    public nint DecryptString;
    public nint DB_AddXAsset;

    public Patch[] Patches;
}