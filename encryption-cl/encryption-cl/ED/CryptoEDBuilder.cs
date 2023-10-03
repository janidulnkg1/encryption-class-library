using encryption_cl.Key;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CryptoED;

namespace encryption_cl.ED
{
    public class CryptoEDBuilder
    {
        private IConfiguration _configuration;
        private IKeyProvider _keyProvider;

        public CryptoEDBuilder SetConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
            return this;
        }

        public CryptoEDBuilder SetKeyProvider(IKeyProvider keyProvider)
        {
            _keyProvider = keyProvider;
            return this;
        }

        public CryptoED Build()
        {
            if (_configuration == null || _keyProvider == null)
            {
                throw new InvalidOperationException("Configuration and KeyProvider must be set.");
            }

            return new CryptoED(_configuration, _keyProvider);
        }
    }
}
