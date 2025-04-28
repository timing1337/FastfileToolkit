
using FastfileToolkit.Fastfiles;
using System.IO;
using System.Runtime.InteropServices;

namespace FastfileToolkit.Bdiff
{
    public struct BDiffState
    {
        public bool headerRead;
        public bool error;
        public bool eof;
        public byte padding;
        public uint error_code;
        public uint features;
        public uint checksum;
    };

    public unsafe class DBBinaryPatchStream
    {
        private BDiffState State;
        private uint[] archiveChecksum;
        private byte* sourceWindow;
        private ulong sourceWindowOffset;
        private ulong sourceWindowSize;
        private ulong sourceWindowAllocated;
        private byte* destWindow;
        private ulong destWindowOffset;
        private ulong destWindowSize;
        private ulong destWindowAllocated;
        private ulong destWindowReadOffset;
        private byte* patchWindow;
        private ulong patchWindowOffset;
        private ulong patchWindowSize;
        private ulong patchWindowAllocated;
        private ulong patchWindowOffsetLast;
        private ulong patchDataOffset;
        private ulong diffUncompSize;

        private BinaryReader BaseReader;
        private BinaryReader DiffReader;

        public DBBinaryPatchStream(FastPatch header, byte[] data, byte[] diff)
        {
            ulong destSize = (header.residentWindowSizes.destWindow + 4095) & 0xFFFFFFFFFFFFF000;
            if (destSize < 0x10000)
                destSize = 0x10000;
            ulong sourceSize = (header.residentWindowSizes.sourceWindow + 4095) & 0xFFFFFFFFFFFFF000;
            if (sourceSize < 0x10000)
                sourceSize = 0x10000;
            ulong diffSize = (header.residentWindowSizes.diffWindow + 4095) & 0xFFFFFFFFFFFFF000;
            if (diffSize < 0x10000)
                diffSize = 0x10000;

            sourceWindowAllocated = sourceSize;
            destWindowAllocated = destSize;
            patchWindowAllocated = diffSize;

            sourceWindow = (byte*)NativeMemory.AlignedAlloc((nuint)sourceWindowAllocated, 4096);
            destWindow = (byte*)NativeMemory.AlignedAlloc((nuint)destWindowAllocated, 4096);
            patchWindow = (byte*)NativeMemory.AlignedAlloc((nuint)patchWindowAllocated, 4096);

            destWindowReadOffset = 0;
            diffUncompSize = header.residentDiffUncompSize;

            BaseReader = new BinaryReader(new MemoryStream(data));
            DiffReader = new BinaryReader(new MemoryStream(diff));

            State = new BDiffState();   
        }

        private byte* LoadSourceData(ulong offset, ulong size)
        {
            var sourceWindowSize = this.sourceWindowSize;
            var sourceWindowOffset = this.sourceWindowOffset;
            if (offset + size <= sourceWindowOffset + sourceWindowSize)
            {
                return sourceWindow + offset - sourceWindowOffset;
            }

            ulong v10 = 0;
            if (offset != sourceWindowOffset)
            {
                if (offset >= sourceWindowSize + sourceWindowOffset)
                {
                    v10 = offset - sourceWindowSize - sourceWindowOffset;
                    sourceWindowSize = 0;
                }
                else
                {
                    sourceWindowSize -= offset - sourceWindowOffset;
                    for (ulong i = 0; i < sourceWindowSize; i++)
                    {
                        sourceWindow[i] = sourceWindow[offset - sourceWindowOffset + i];
                    }
                }
                sourceWindowOffset = offset;
            }

            if (v10 != 0) BaseReader.BaseStream.Position += (int)v10;

            sourceWindowSize = size;
            var v12 = size - sourceWindowSize;
            if (v12 > 0)
            {
                byte[] bytes = BaseReader.ReadBytes((int)v12);
                Marshal.Copy(bytes, 0, (nint)(sourceWindow + sourceWindowSize), (int)v12);
            }
            return sourceWindow;
        }

        private byte* LoadPatchData(ulong offset, ulong size, ulong* pOffset)
        {
            if (offset != 0)
                patchWindowOffsetLast = offset;
            else
                offset = patchWindowOffsetLast;
            if ((nint)pOffset != 0)
                *pOffset = offset;
            var patchWindowSize = this.patchWindowSize;
            var patchWindowOffset = this.patchWindowOffset;
            if (offset + size <= patchWindowOffset + patchWindowSize)
                return patchWindow + offset - patchWindowOffset;

            ulong v12 = 0;
            if (offset != patchWindowOffset)
            {
                if (offset >= patchWindowSize + patchWindowOffset)
                {
                    v12 = offset - patchWindowSize - patchWindowOffset;
                    patchWindowSize = 0;
                }
                else
                {
                    patchWindowSize -= offset - patchWindowOffset;

                    for (ulong i = 0; i < patchWindowSize; i++)
                    {
                        patchWindow[i] = patchWindow[offset - patchWindowOffset + i];
                    }
                }
                this.patchWindowOffset = offset;
            }

            if (size > 0 && patchWindowSize < size)
            {
                ulong v9 = diffUncompSize - patchWindowSize - offset;
                if (v9 > 0)
                {
                    if (size - patchWindowSize <= v9)
                        v9 = size - patchWindowSize;
                    byte[] diff = DiffReader.ReadBytes((int)v9);
                    Marshal.Copy(diff, 0, (nint)(patchWindow + patchWindowSize), (int)v9);
                    patchWindowSize += v9;
                    patchDataOffset += v9;
                }
                this.patchWindowSize = patchWindowSize;
                if (patchWindowSize == 0)
                    return (byte*)0;
            }
            else
            {
                this.patchWindowSize = patchWindowSize;
            }

            return this.patchWindow;
        }

        private byte* SetupDestData(ulong size)
        {
            destWindowSize = size;
            return destWindow;
        }

        private ulong ReadULEB128(ref byte* ptr)
        {
            ulong v36 = 0;
            char v37;
            do
            {
                v37 = *(char*)ptr;
                uint v38 = (uint)(*ptr++ & 0x7F);
                v36 = ((v36 << 7) | v38);
            }
            while (v37 < 0);
            return v36;
        }

        private bool Patch()
        {
            if (State.headerRead)
            {
                byte* header = LoadPatchData(0, 1029, (ulong*)0);
                if (header[0] != 0xD6 || header[1] != 0xC3 || header[2] != 0xC4) 
                {
                    throw new Exception("Invalid VCD patch magic");
                }

                byte diffStateFeature = header[4];
                if ((diffStateFeature & 0xFFFFFFF3) != 0)
                {
                    throw new Exception("Invalid diff state feature");
                }

                State.features = diffStateFeature;

                LoadPatchData(5, 0, (ulong*)0);
                State.headerRead = true;
            }

            ulong pOffset = 0;
            byte* v15 = LoadPatchData(0, 1024, &pOffset);

            byte flags = *v15;
            int hasFlag3 = flags & 3;
            if(hasFlag3 == 3)
            {
                return false;
            }

            byte* v21 = v15 + 1;

            ulong v23;
            byte* v82;
            if(hasFlag3 != 0)
            {
                v23 = ReadULEB128(ref v21);
                ulong v26 = ReadULEB128(ref v21);

                if(hasFlag3 != 1)
                {
                    return false;
                }

                ulong v29;
                if((flags & 4) != 0)
                {
                    v29 = ReadULEB128(ref v21);
                }
                else
                {
                    v29 = v26;
                }

                ulong v32 = v26 - v29;
                v82 = LoadSourceData(v29, v23 + v32) + v32;
            }

            ulong v34 = ReadULEB128(ref v21);
            byte* v37 = LoadPatchData(pOffset + 1, v34 + (((ulong)flags >> 1) & 4), &pOffset);
            return true;
        }

        public void Read(byte* pos, ulong size)
        {
            var remainingSize = size;
            while (remainingSize > 0)
            {
                if (destWindowSize == destWindowReadOffset)
                {
                    destWindowReadOffset = 0;

                    if (!Patch())
                    {
                        throw new Exception("Failed to patch");
                    }
                }

                var v6 = destWindowSize - destWindowReadOffset;

                if (remainingSize < v6)
                    v6 = remainingSize;

                for (ulong i = 0; i < v6; i++)
                {
                    pos[i] = destWindow[destWindowReadOffset + i];
                }

                destWindowReadOffset += v6;
                pos += v6;
                remainingSize -= v6;
            }
        }
    };
}