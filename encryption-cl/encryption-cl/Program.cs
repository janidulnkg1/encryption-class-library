using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using encryption_cl.Key;

namespace encryption_cl
{
    class Program
    {
        static void Main(string[] args)
        {
            // Creating Key instance and set key value
            var key = new KeyProvider();
            key.SetKey("");

            // Creating FileED instance with the IConfiguration 
            var builder = new FileED.FileEDBuilder();
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json")
                .Build();
            var fileED = builder.SetConfiguration(configuration).SetKeyProvider(key).Build();

          
            byte[] inputData  /* input data as byte[] */;

            // Encrypt data
            byte[] encryptedData = fileED.Encrypt(inputData);

            // Decrypt data
            byte[] decryptedData = fileED.Decrypt(encryptedData);

            
        }
    }
}
