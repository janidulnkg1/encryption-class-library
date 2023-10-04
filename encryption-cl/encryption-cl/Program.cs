using encryption_cl.Key;
using Microsoft.Extensions.Configuration;
using System.IO;
using encryption_cl.ED;


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

            KeyProvider keyProvider = new KeyProvider();
            keyProvider.SetKey(encryptionKey);

            CryptoEDBuilder cryptoEDBuilder = new CryptoEDBuilder();
            cryptoEDBuilder.SetKeyProvider(keyProvider);

            CryptoED cryptoED = cryptoEDBuilder.Build();

        }
    }
}
