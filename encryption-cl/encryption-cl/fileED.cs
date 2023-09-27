using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Security.Cryptography;

namespace encryption_cl
{
    public class FileED
    {
        private readonly IConfiguration _configuration;

        private FileED(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public byte[] Encrypt(byte[] inputData)
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
                        cs.Write(inputData, 0, inputData.Length);
                    }

                    return ms.ToArray();
                }
            }
        }

        public byte[] Decrypt(byte[] encryptedData)
        {
            using (Aes aes = Aes.Create())
            {
                aes.KeySize = 256;
                byte[] keyBytes = HexToBytes(_configuration["Key:DataEDKey"]);
                byte[] iv = new byte[aes.IV.Length];
                byte[] decryptedBytes;

                Array.Copy(encryptedData, iv, iv.Length);

                using (var decryptor = aes.CreateDecryptor(keyBytes, iv))
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write))
                    {
                        cs.Write(encryptedData, iv.Length, encryptedData.Length - iv.Length);
                    }

                    decryptedBytes = ms.ToArray();
                }

                return decryptedBytes;
            }
        }

        public class FileEDBuilder
        {
            private IConfiguration _configuration;

            public FileEDBuilder SetConfiguration(IConfiguration configuration)
            {
                _configuration = configuration;
                return this;
            }

            public FileED Build()
            {
                if (_configuration == null)
                {
                    throw new InvalidOperationException("Configuration is required.");
                }

                return new FileED(_configuration);
            }
        }

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
