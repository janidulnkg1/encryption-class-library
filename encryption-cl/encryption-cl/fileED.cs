using encryption_cl.Key;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;

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
            byte[] keyBytes = _keyProvider.GetKey();
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
            byte[] keyBytes = _keyProvider.GetKey();
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


}
