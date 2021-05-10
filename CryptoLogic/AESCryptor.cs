using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SoSicencneSSHAgent.CryptoLogic
{
    public class AESCryptor
    {
        private readonly Aes aes;

        public AESCryptor()
        {
            aes = new AesManaged();
            byte[] a = new byte[256 / 8];
            byte[] iv = new byte[256 / 16];
            Random random = new Random();
            random.NextBytes(a);
            random.NextBytes(iv);
            aes.Key = a;
            aes.IV = iv;
        }
        public AESCryptor(int key = 256)
        {
            aes = new AesManaged();
            byte[] a = new byte[key / 8];
            byte[] iv = new byte[256 / 16];
            Random random = new Random();
            random.NextBytes(a);
            random.NextBytes(iv);
            aes.Key = a;
            aes.IV = iv;
        }
        public AESCryptor(byte[] key)
        {
            aes = new AesManaged
            {
                Key = key,
                IV = null
            };
        }
        public AESCryptor(byte[] key, byte[] iv)
        {
            aes = new AesManaged
            {
                Key = key,
                IV = iv
            };
        }
        public CryptoKey Key { get { return new CryptoKey(aes.IV, aes.Key); } }

        public string Decrypt(string encryptedData)
        {
            return Encoding.UTF8.GetString(Decrypt(Convert.FromBase64String(encryptedData)));
        }

        public byte[] Decrypt(byte[] encryptedData)
        {
            byte[] rawData = new byte[encryptedData.Length];
            // Create a decryptor to perform the stream transform.
            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            // Create the streams used for decryption.
            using (MemoryStream msDecrypt = new MemoryStream(encryptedData))
            {
                using CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
                using BinaryReader br = new BinaryReader(csDecrypt);
                int lenght = br.Read(rawData);
                Array.Resize(ref rawData, lenght);
            }
            return rawData;
        }

        public string Encrypt(string plainText)
        {
            return Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes(plainText)));
        }

        public byte[] Encrypt(byte[] rawData)
        {
            // Create an encryptor to perform the stream transform.
            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            // Create the streams used for EncryptionCore.
            using MemoryStream msEncrypt = new MemoryStream();
            using CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
            using (BinaryWriter swEncrypt = new BinaryWriter(csEncrypt))
            {
                //Write all data to the stream.
                swEncrypt.Write(rawData);
            }
            return msEncrypt.ToArray();
            // Return encrypted data    
        }
    }

    public class CryptoKey
    {
        public byte[] Iv { get; private set; }

        public byte[] Key { get; private set; }
        public CryptoKey(byte[] iv, byte[] key)
        {
            Iv = iv;
            Key = key;
        }
    }
}
