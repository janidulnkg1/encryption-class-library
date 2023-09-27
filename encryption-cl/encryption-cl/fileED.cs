using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;


namespace encryption_cl
{
    public class FileED
    {
        private readonly IConfiguration _configuration;

        public FileED()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json"); 

            _configuration = builder.Build();
        }

        public string Encrypt(string inputFile)
        {
            using (Aes aes = Aes.Create())
            {
                aes.KeySize = 256;
                byte[] keyBytes = HexToBytes(_configuration["Key:DataEDKey"]);
                byte[] iv = aes.IV;

                using (var encryptor = aes.CreateEncryptor(keyBytes, iv))
                using (var ms = new MemoryStream())
                {
                    ms.Write(iv, 0, iv.Length);
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        byte[] dataBytes = Encoding.UTF8.GetBytes(inputFile);
                        cs.Write(dataBytes, 0, dataBytes.Length);
                    }

                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        public string Decrypt(string inputFile)
        {
            byte[] encryptedBytes = Convert.FromBase64String(inputFile);

            using (Aes aes = Aes.Create())
            {
                aes.KeySize = 256;
                byte[] keyBytes = HexToBytes(_configuration["Key:DataEDKey"]);
                byte[] iv = new byte[aes.IV.Length];
                byte[] decryptedBytes;

                Array.Copy(encryptedBytes, iv, iv.Length);

                using (var decryptor = aes.CreateDecryptor(keyBytes, iv))
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write))
                    {
                        cs.Write(encryptedBytes, iv.Length, encryptedBytes.Length - iv.Length);
                    }

                    decryptedBytes = ms.ToArray();
                }

                return Encoding.UTF8.GetString(decryptedBytes);
            }
        }

        // Helper method to convert a hex string to byte array
        private byte[] HexToBytes(string hex)
        {
            int numberChars = hex.Length;
            byte[] bytes = new byte[numberChars / 2];
            for (int i = 0; i < numberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return bytes;
        }
    }
}
