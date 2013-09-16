using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Windows.Forms;
using xGTAV.Common.Helpers;
namespace xGTAV.Common.Encryption
{
    public class GTAVAES
    {
        private static RijndaelManaged aes = null;
        private static ICryptoTransform decryptor;

        //GTA V key
        public static byte[] gtaKey;

        public static byte[] Decrypt(byte[] data, int numRounds)
        {
            if (gtaKey == null)
                GetKey();
            if (aes == null)
            {
                aes = new RijndaelManaged();
                aes.KeySize = 256;
                aes.Key = gtaKey;
                aes.Padding = PaddingMode.Zeros;
                decryptor = aes.CreateDecryptor();
            }
            decryptor.TransformBlock(data, 0, numRounds , data, 0);
            return data;
        }

        /// <summary>
        /// Gets the rpf key from key.dat or scans for a xex base file to get it
        /// </summary>
        public static void GetKey()
        {
            MD5 md = MD5.Create();
            if (File.Exists("key.dat"))
                gtaKey = File.ReadAllBytes("key.dat");
            else
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Title = "Open GTA V base executable...";
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        MemoryStream xex = new MemoryStream(File.ReadAllBytes(ofd.FileName));

                        ///Offsets TU0
                        int[] offsets = new int[16] { 0x16F2956, 0x16E8F3E, 0x16E9A2E, 0x16DF5F6, 0x16EA4AE, 0x16EA766, 0x16EA90E, 0x16ED3A6, 
                                                        0x16EF0D6,  0x16EDAAE, 0x16F5426, 0x16F4146, 0x1706A0E, 0x170693E, 0x1705D5E, 0x17081EE };
                        List<byte> key = new List<byte>();
                        for (int i = 0; i < offsets.Length; i++)
                        {
                            xex.Position = offsets[i];
                            byte[] hex = xex.ReadBytes(2);
                            key.Add(hex[0]);
                            key.Add(hex[1]);
                        }
                        gtaKey = key.ToArray();
                    }
                }
            }
            byte[] mdHash = md.ComputeHash(gtaKey);
            string strHash = BitConverter.ToString(mdHash).Replace("-", "").ToLower();
            if (strHash != "EAD1EA1A3870557B424BC8CF73F51018")
                throw new Exception("Invalid RPF key!");
            else 
                return;
        }
    }
}
