using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace xGTAV.Common.Encryption
{
    public class GTAVAES
    {
        private static RijndaelManaged aes = null;
        private static ICryptoTransform decryptor;

        //GTA V key
        public static byte[] gtaKey;

        public static byte[] Decrypt(byte[] data)
        {
            if (aes == null)
            {
                aes = new RijndaelManaged();
                aes.KeySize = 256;
                aes.Key = gtaKey;
                aes.Padding = PaddingMode.Zeros;
                decryptor = aes.CreateDecryptor();
            }
            decryptor.TransformBlock(data, 0, (data.Length / 0x10) * 0x10, data, 0);
            return data;
        }
    }
}
