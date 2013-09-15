using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xGTAV.Common.Helpers
{
    public static partial class BufferHelpers
    {
        public static void Decompress(this byte[] buffer, byte[] output) { Compression.xCompress.Decompress(buffer, output); }
        public static byte[] Compress(this byte[] buffer) { return Compression.xCompress.Compress(buffer); }
        public static byte[] Swap(this byte[] buffer)
        {
            Array.Reverse(buffer);
            return buffer;
        }
        public static byte[] Align(this byte[] buffer, int alignment)
        {
            int alignmentSize = buffer.Length.Align(alignment);
            byte[] newBuffer = new byte[alignmentSize];
            Array.Copy(buffer, newBuffer, buffer.Length);
            return newBuffer;
        }
        public static byte[] Trim(this byte[] buffer)
        {
            int byteCounter = buffer.Length - 1;
            while (buffer[byteCounter] == 0x00)
            {
                byteCounter--;
            }
            byte[] rv = new byte[(byteCounter + 1)];
            for (int byteCounter1 = 0; byteCounter1 < (byteCounter + 1); byteCounter1++)
            {
                rv[byteCounter1] = buffer[byteCounter1];
            }
            return rv;
        }
    }
}
