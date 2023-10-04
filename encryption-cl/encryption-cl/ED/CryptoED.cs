using encryption_cl.Key;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class CryptoED
{
    private readonly IKeyProvider _keyProvider;
    private readonly Guid _fallbackIV;

    public IKeyProvider KeyProvider
    {
        get { return _keyProvider; }
    }

    public CryptoED(IConfiguration _configuration, IKeyProvider keyProvider)
    {
        _keyProvider = keyProvider ?? throw new ArgumentNullException(nameof(keyProvider));
        _fallbackIV = Guid.NewGuid();
    }

    public CryptoED(IKeyProvider keyProvider)
    {
        _keyProvider = keyProvider ?? throw new ArgumentNullException(nameof(keyProvider));
        _fallbackIV = Guid.NewGuid();
    }

    public byte[] Encrypt(byte[] inputData, Guid? customIV = null)
    {
        try
        {
            using (Aes aes = Aes.Create())
            {
                aes.KeySize = 256;
                string key = _keyProvider.GetKey(); // Getting encryption key as a string
                byte[] keyBytes = Encoding.UTF8.GetBytes(key); // Converting the string key to bytes

                byte[] iv = customIV.HasValue ? customIV.Value.ToByteArray() : _fallbackIV.ToByteArray();
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

                    encryptedBytes = ms.ToArray();
                }
                return encryptedBytes;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Encryption failed: " + ex.Message);
            throw;
        }
    }

    public byte[] Decrypt(byte[] encryptedData)
    {
        try
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
        catch (Exception ex)
        {
            Console.WriteLine("Decryption failed: " + ex.Message);
            throw;
        }
    }
}
