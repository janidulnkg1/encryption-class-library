uusing Microsoft.Extensions.Configuration;
using System;
using System.IO;
using encryption_cl.Key;

namespace encryption_cl
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get the encryption key from the configuration
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json")
                .Build();

            string encryptionKey = configuration["EncryptionKey"];

            // Create a KeyProvider and set the encryption key
            IKeyProvider keyProvider = new MyKeyProvider(encryptionKey);

            // Create a CryptoEDBuilder and configure it with the KeyProvider
            CryptoEDBuilder cryptoEDBuilder = new CryptoEDBuilder();
            cryptoEDBuilder.SetKeyProvider(keyProvider);

            // Build the CryptoED object
            CryptoED cryptoED = cryptoEDBuilder.Build();

            // Now you can use the cryptoED object for encryption and decryption operations
            // Example: byte[] encryptedData = cryptoED.Encrypt(Encoding.UTF8.GetBytes("your data"));

            Console.WriteLine("Encryption and decryption setup complete.");
        }
    }
}

