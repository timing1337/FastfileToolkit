
using FastfileToolkit.Fastfiles;
using FastfileToolkit.Utils;
using Serilog;
using System;
using System.Drawing;
using System.IO;
using System.IO.Hashing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Win32;

namespace FastfileToolkit.Fastfiles {
    public struct BDiffState {
        public bool headerRead;
        public bool error;
        public bool eof;
        public byte padding;
        public uint error_code;
        public uint features;
        public uint checksum;
    };

    public unsafe class DBBinaryPatchStream {

        private byte[] Source;
        private byte[] Diff;

        private long SourceOffset = 0;
        private long DiffOffset = 0;

        private ulong Checksum = 0;

        private ulong Size = 0;

        private List<byte[]> Chunks = new List<byte[]>();

        public DBBinaryPatchStream(FastPatch header, byte[] data, byte[] diff) {
            Source = data;
            Diff = diff;

            if (Diff[0] != 0xD6 || Diff[1] != 0xC3 || Diff[2] != 0xC4) {
                throw new Exception("Invalid VCD patch magic");
            }

            //probably? i don't fucking know lol
            if ((Diff[4] & 0xFFFFFFF3) != 0) {
                throw new Exception("Invalid VCD patch flags");
            }

            DiffOffset += 5;
        }

        public byte[] Patch() {
            while (DiffOffset < Diff.Length) {
                byte flags = Diff[DiffOffset++];

                if (flags <= 0 || (flags & 3) == 3 || (flags & 3) != 1) {
                    throw new Exception("Invalid VCD patch flags");
                }

                ulong sourceLength = BinaryUtils.ReadULEB128(Diff, ref DiffOffset);
                ulong sourceOffset = (ulong)SourceOffset + BinaryUtils.ReadULEB128(Diff, ref DiffOffset);

                if ((flags & 4) != 0) {
                    BinaryUtils.ReadULEB128(Diff, ref DiffOffset);
                }

                ulong patchLength = BinaryUtils.ReadULEB128(Diff, ref DiffOffset) + (((ulong)flags >> 1) & 4);

                ulong finalSize = BinaryUtils.ReadULEB128(Diff, ref DiffOffset);

                Size += finalSize;

                byte[] destChunk = new byte[finalSize];
                ulong destOffset = 0;

                if (Diff[DiffOffset++] != 0) {
                    throw new Exception("Unknown");
                }

                nint vcdState = (nint)NativeMemory.AllocZeroed(6184);

                long offset = (long)BinaryUtils.ReadULEB128(Diff, ref DiffOffset);
                long length = (long)BinaryUtils.ReadULEB128(Diff, ref DiffOffset);
                long v57 = (long)BinaryUtils.ReadULEB128(Diff, ref DiffOffset);

                long baseDiffOffset = DiffOffset;
                long pAddrOffset = DiffOffset + offset + length;
                long endOffset = pAddrOffset;
                long checksumOffset = DiffOffset + offset + length + v57;

                DiffOffset += offset;
                while (true) {
                    var index = 2 * Diff[DiffOffset++];

                    for (int i = 0; i < 2; i++) {
                        var instruction = Vcd.Instructions[index + i];
                        if (instruction.op == 0) {
                            continue;
                        }

                        ulong size = instruction.size;
                        if (size == 0) {
                            size = BinaryUtils.ReadULEB128(Diff, ref DiffOffset);
                        }

                        switch (instruction.op) {
                            case 1:
                                Buffer.BlockCopy(Diff, (int)baseDiffOffset, destChunk, (int)destOffset, (int)size);
                                baseDiffOffset += (int)size;
                                destOffset += size;
                                break;
                            case 2:
                                byte value = Diff[baseDiffOffset++];
                                fixed (byte* ptr = &destChunk[destOffset]) {
                                    NativeMemory.Fill(ptr, (nuint)size, value);
                                }
                                destOffset += size;
                                break;
                            default:
                                ulong here = destOffset + sourceLength;
                                uint mode = instruction.mode;
                                ulong result = 0;
                                if (mode != 0) {
                                    if (mode == 1) {
                                        ulong v8 = BinaryUtils.ReadULEB128(Diff, ref pAddrOffset);
                                        result = here - v8;
                                    } else if (mode >= 6u) {
                                        byte v10 = Diff[pAddrOffset++];
                                        result = *(ulong*)(vcdState + 8 * (v10 + ((mode - 6) << 8)) + 40);
                                    } else {
                                        result = *(ulong*)(vcdState + 8 * mode - 8) + BinaryUtils.ReadULEB128(Diff, ref pAddrOffset);
                                    }
                                } else {
                                    result = BinaryUtils.ReadULEB128(Diff, ref pAddrOffset);
                                }
                                *(ulong*)(vcdState + 8 * *(uint*)vcdState + 8) = result;
                                *(uint*)vcdState = (uint)(((byte)*(uint*)vcdState + 1) & 3);
                                *(ulong*)(vcdState + 8 * ((nint)result % 0x300) + 40) = result;

                                if (result < sourceLength) {
                                    Buffer.BlockCopy(Source, (int)(sourceOffset + result), destChunk, (int)destOffset, (int)size);
                                    destOffset += size;
                                } else {
                                    Buffer.BlockCopy(destChunk, (int)(result - sourceLength), destChunk, (int)destOffset, (int)size);
                                    destOffset += size;
                                }
                                break;
                        }
                    }
                    if (DiffOffset >= endOffset) break;
                }

                //Checksum check
                if ((flags & 8) != 0) {
                    ulong originalChecksum = BitConverter.ToUInt32(Diff, (int)(checksumOffset));
                    fixed (byte* ptr = destChunk) {
                        //this is baddd
                        ulong checksum = Toolkit.Instance.j_CoD_XXH64((nint)ptr, finalSize, Checksum);
                        if (checksum != originalChecksum) {
                            throw new Exception($"Checksum mismatch: expected {originalChecksum:X}, got {checksum:X}");
                        }
                        Checksum = checksum;
                    }
                }

                DiffOffset = checksumOffset + 4;
                Size += finalSize;
                Chunks.Add(destChunk);
            }

            byte[] merged = new byte[Size];
            int chunkOffset = 0;
            for (int i = 0; i < Chunks.Count; i++) {
                Buffer.BlockCopy(Chunks[i], 0, merged, chunkOffset, Chunks[i].Length);
                chunkOffset += Chunks[i].Length;
            }

            return merged;
        }
    };
}