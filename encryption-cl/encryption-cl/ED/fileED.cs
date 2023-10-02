using encryption_cl.Key;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class FileED
{
    private readonly IConfiguration _configuration;
    private readonly IKeyProvider _keyProvider;

    public FileED(IConfiguration configuration, IKeyProvider keyProvider)
    {
        _configuration = configuration;
        _keyProvider = keyProvider;
    }

    public byte[] Encrypt(byte[] inputData)
    {
        using (Aes aes = Aes.Create())
        {
            aes.KeySize = 256;
            string key = _keyProvider.GetKey(); // Getting encryption key as a string
            byte[] keyBytes = Encoding.UTF8.GetBytes(key); // Converting the string key to bytes
            byte[] iv = aes.IV;
            byte[] encryptedBytes;

            using (var encryptor = aes.CreateEncryptor(keyBytes, iv))
            using (var ms = new MemoryStream())
            {
                ms.Write(iv, 0, iv.Length);
                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    cs.Write(inputData, 0, inputData.Length);
                    cs.FlushFinalBlock(); // Flushing the final block
                }

                encryptedBytes =  ms.ToArray();
            }
            return encryptedBytes;
        }
    }

    public byte[] Decrypt(byte[] encryptedData)
    {
        using (Aes aes = Aes.Create())
        {
            aes.KeySize = 256;
            string key = _keyProvider.GetKey(); // Getting the key as a string
            byte[] keyBytes = Encoding.UTF8.GetBytes(key); // Converting the string key to bytes
            byte[] iv = new byte[aes.IV.Length];
            byte[] decryptedBytes;

            Array.Copy(encryptedData, iv, iv.Length);

            using (var decryptor = aes.CreateDecryptor(keyBytes, iv))
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write))
                {
                    cs.Write(encryptedData, iv.Length, encryptedData.Length - iv.Length);
                    cs.FlushFinalBlock(); // Flushing the final block
                }

                decryptedBytes = ms.ToArray();
            }

            return decryptedBytes;
        }
    }

    public class FileEDBuilder
    {
        private IConfiguration _configuration;
        private IKeyProvider _keyProvider;

        public FileEDBuilder SetConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
            return this;
        }

        public FileEDBuilder SetKeyProvider(IKeyProvider keyProvider)
        {
            _keyProvider = keyProvider;
            return this;
        }

        public FileED Build()
        {
            if (_configuration == null || _keyProvider == null)
            {
                throw new InvalidOperationException("Configuration and KeyProvider must be set.");
            }

            return new FileED(_configuration, _keyProvider);
        }
    }

}



