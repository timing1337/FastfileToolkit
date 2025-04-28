using FastfileToolkit.Native;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FastfileToolkit.Compressors;

public unsafe class OodleDecompressor : BaseCompressor
{

    public uint BlockIndex = 0;
    public uint NextBlockHashIndex = 511;

    [DllImport("oo2core_8_win64.dll")]
    public static extern unsafe long OodleLZ_Decompress(
           byte* compBuf, uint compSize, byte* rawBuf, uint rawSize,
           int fuzzSafe,
           int checkCrc,
           int verbosity,
           byte* dictBuff,
           ulong dictSize,
           ulong unk,
           void* unkCallback,
           byte* scratchBuff,
           ulong scratchSize,
           int threadPhase);

    public override bool Decompress(BinaryReader reader, ulong size, out byte[] decompressed)
    {
        decompressed = new byte[size];
        ulong decompressedOffset = 0;

        while (reader.BaseStream.Position < reader.BaseStream.Length)
        {
            if (BlockIndex == 0)
            {
                reader.BaseStream.Seek(0x8000, SeekOrigin.Current);
                reader.BaseStream.Seek(8, SeekOrigin.Current);
            }

            uint compressedBufferSize = reader.ReadUInt32();
            uint decompressedBufferSize = reader.ReadUInt32();
            uint flags = reader.ReadUInt32();

            ulong alignedSize = ((ulong)compressedBufferSize + 0x3) & 0xFFFFFFFFFFFFFFFC;
            byte[] compressedBuffer = reader.ReadBytes((int)alignedSize);

            fixed (byte* compressedBufferPtr = compressedBuffer)
            {
                fixed (byte* decompressedBufferPtr = decompressed)
                {
                    ulong result = (ulong)OodleLZ_Decompress(
                        compressedBufferPtr,
                        (uint)compressedBufferSize,
                        decompressedBufferPtr + decompressedOffset,
                        (uint)decompressedBufferSize,
                        0,
                        0,
                        0,
                        null,
                        0,
                        0,
                        null,
                        null,
                        0,
                        3);

                    if (result != decompressedBufferSize)
                    {
                        throw new Exception($"Failed to decompress Oodle buffer, expected {decompressedBufferSize} got {result}");
                    }

                    decompressedOffset += decompressedBufferSize;

                    BlockIndex++;

                    if (BlockIndex == NextBlockHashIndex)
                    {
                        reader.BaseStream.Seek(16384, SeekOrigin.Current);
                        NextBlockHashIndex += 512;
                    }
                }
            }
        }

        if (size != decompressedOffset)
        {
            return false;
        }

        return true;
    }
}