using encryption_cl.Key;
using Microsoft.Extensions.Configuration;
using System.IO;


namespace encryption_cl
{
    class Program
    {
        static void Main(string[] args)
        {
        
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json")
                .Build();

            // Get the encryption key from the configuration
            string encryptionKey = configuration["EncryptionKey"];

      
            KeyProvider keyProvider = new KeyProvider();
            keyProvider.SetKey(encryptionKey);


            CryptoED fileED = new CryptoED.CryptoEDBuilder()
                .SetConfiguration(configuration)
                .SetKeyProvider((IKeyProvider)keyProvider)
                .Build();

           
        }
    }
}
