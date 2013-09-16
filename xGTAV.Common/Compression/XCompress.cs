/*
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using xGTAV.Common.Helpers;
namespace xGTAV.Common.Compression
{
    public static class xCompress
    {
        public enum XMEMCODEC_TYPE
        {
            XMEMCODEC_DEFAULT = 0,
            XMEMCODEC_LZX = 1
        }
        public struct XMEMCODEC_PARAMETERS_LZX
        {
            public int Flags;
            public int WindowSize;
            public int CompressionPartitionSize;
        }
        public const int XMEMCOMPRESS_STREAM = 0x00000001;
        #region DLL Imports
        [DllImport("xcompress32.dll", EntryPoint = "XMemCreateDecompressionContext")]
        public static extern int XMemCreateDecompressionContext(
            XMEMCODEC_TYPE CodecType,
            int pCodecParams,
            int Flags, ref int pContext);

        [DllImport("xcompress32.dll", EntryPoint = "XMemDestroyDecompressionContext")]
        public static extern void XMemDestroyDecompressionContext(int Context);

        [DllImport("xcompress32.dll", EntryPoint = "XMemResetDecompressionContext")]
        public static extern int XMemResetDecompressionContext(int Context);

        [DllImport("xcompress32.dll", EntryPoint = "XMemDecompress")]
        public static extern int XMemDecompress(int Context,
            byte[] pDestination, ref int pDestSize,
            byte[] pSource, int pSrcSize);

        [DllImport("xcompress32.dll", EntryPoint = "XMemDecompressStream")]
        public static extern int XMemDecompressStream(int Context,
            byte[] pDestination, ref int pDestSize,
            byte[] pSource, ref int pSrcSize);

        [DllImport("xcompress32.dll", EntryPoint = "XMemCreateCompressionContext")]
        public static extern int XMemCreateCompressionContext(
            XMEMCODEC_TYPE CodecType, int pCodecParams,
            int Flags, ref int pContext);

        [DllImport("xcompress32.dll", EntryPoint = "XMemDestroyCompressionContext")]
        public static extern void XMemDestroyCompressionContext(int Context);

        [DllImport("xcompress32.dll", EntryPoint = "XMemResetCompressionContext")]
        public static extern int XMemResetCompressionContext(int Context);

        [DllImport("xcompress32.dll", EntryPoint = "XMemCompress")]
        public static extern int XMemCompress(int Context,
            byte[] pDestination, ref int pDestSize,
            byte[] pSource, int pSrcSize);

        [DllImport("xcompress32.dll", EntryPoint = "XMemCompressStream")]
        public static extern int XMemCompressStream(int Context,
            byte[] pDestination, ref int pDestSize,
            byte[] pSource, ref int pSrcSize);
        #endregion
        public static byte[] Decompress(byte[] compressedData, byte[] decompressedData)
        {
            // Setup our decompression context
            int DecompressionContext = 0;
            int hr = XMemCreateDecompressionContext(
                XMEMCODEC_TYPE.XMEMCODEC_LZX,
                0, 0, ref DecompressionContext);

            // Now lets decompress
            int compressedLen = compressedData.Length;
            int decompressedLen = decompressedData.Length;
            try
            {
                hr = XMemDecompress(DecompressionContext,
                    decompressedData, ref decompressedLen,
                    compressedData, compressedLen);
            }
            catch { }
            // Go ahead and destory our context
            XMemDestroyDecompressionContext(DecompressionContext);
            // Return our decompressed bytes
            return decompressedData.Trim();
        }
        public static byte[] Compress(byte[] decompressedData)
        {
            // Setup our compression context
            int compressionContext = 0;
            int hr = XMemCreateCompressionContext(
                XMEMCODEC_TYPE.XMEMCODEC_LZX,
                0, 0, ref compressionContext);

            // Now lets compress
            int compressedLen = decompressedData.Length * 2;
            byte[] compressed = new byte[compressedLen];
            int decompressedLen = decompressedData.Length;
            hr = XMemCompress(compressionContext,
                compressed, ref compressedLen,
                decompressedData, decompressedLen);
            // Go ahead and destory our context
            XMemDestroyCompressionContext(compressionContext);

            // Resize our compressed data
            Array.Resize<byte>(ref compressed, compressedLen);

            // Now lets return it
            return compressed;
        }
    }
}
