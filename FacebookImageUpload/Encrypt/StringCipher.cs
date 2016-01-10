using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Linq;
using FacebookImageUpload.FB_Images;

namespace FacebookImageUpload
{
    public class SimplerAES
    {
        private static int KeyLen = 32;
        private static byte[] key = { 123, 217, 19, 11, 24, 26, 85, 45, 114, 184, 27, 162, 37, 112, 222, 209, 241, 24, 175, 144, 173, 53, 196, 29, 24, 26, 17, 218, 131, 236, 53, 209 };
        private static byte[] vector = { 146, 64, 191, 111, 23, 3, 113, 119, 231, 121, 221, 112, 79, 32, 114, 156 };
        private ICryptoTransform encryptor, decryptor;
        private UTF8Encoding encoder;

        public SimplerAES()
        {
            RijndaelManaged rm = new RijndaelManaged();
            encryptor = rm.CreateEncryptor(key, vector);
            decryptor = rm.CreateDecryptor(key, vector);
            encoder = new UTF8Encoding();
        }

        public SimplerAES(string pkey)
        {
           
            key = ExpandUserKey(pkey);
            RijndaelManaged rm = new RijndaelManaged();
            encryptor = rm.CreateEncryptor(key, vector);
            decryptor = rm.CreateDecryptor(key, vector);
            encoder = new UTF8Encoding();
        }


        private byte[] ExpandUserKey(string key)
        {
            byte[] temp = Encoding.UTF8.GetBytes(key);
            int len = temp.Length;
            int n = KeyLen / len;
            byte[] myKey = new byte[KeyLen];
            //Array.Clear(myKey, 0, KeyLen);
            int i, j;
            if (n > 1)
            {
                for (i = 0,j=0 ; i <KeyLen ; i++, j=(j+1)%len)
                {
                    myKey[i] = temp[j];
                }
            }
            else
            {
                Array.Copy(temp, myKey, 256);
            }
            return myKey;
        }
                

        public string Encrypt(string unencrypted)
        {
            return Convert.ToBase64String(Encrypt(encoder.GetBytes(unencrypted)));
        }

        public string Decrypt(string encrypted)
        {
            return encoder.GetString(Decrypt(Convert.FromBase64String(encrypted)));
        }

        public string EncryptFile(string fullPath)
        {
            try
            {
                string plainText = File.ReadAllText(fullPath);
                string cipherText = this.Encrypt(plainText);
                string newPath = MyHelper.AppendFileName(fullPath, "_encrypt");
                File.WriteAllText(newPath, cipherText);
                return newPath;
            }
            catch (Exception e)
            {
                Form1.Log(e);
                return null;
            }
            
        }
        public string DecryptFile(string fullPath)
        {
            try
            {
                string cipherText = File.ReadAllText(fullPath);
                string plaintText = this.Encrypt(cipherText);
                string newPath = MyHelper.AppendFileName(fullPath, "_decrypt");
                File.WriteAllText(newPath, plaintText);
                return newPath;
            }
            catch (Exception e)
            {
                Form1.Log(e);
                return null;
            }

        }

        public byte[] Encrypt(byte[] buffer)
        {
            return Transform(buffer, encryptor);
        }

        public byte[] Decrypt(byte[] buffer)
        {
            return Transform(buffer, decryptor);
        }

        protected byte[] Transform(byte[] buffer, ICryptoTransform transform)
        {
            MemoryStream stream = new MemoryStream();
            using (CryptoStream cs = new CryptoStream(stream, transform, CryptoStreamMode.Write))
            {
                cs.Write(buffer, 0, buffer.Length);
            }
            return stream.ToArray();
        }
    }
}
