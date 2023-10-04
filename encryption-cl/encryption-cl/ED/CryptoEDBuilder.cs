using encryption_cl.Key;

namespace encryption_cl.ED
{
    public class CryptoEDBuilder
    {
        private IKeyProvider _keyProvider;
      
        public CryptoEDBuilder SetKeyProvider(IKeyProvider keyProvider)
        {
            _keyProvider = keyProvider;
            return this;
        }

        public CryptoED Build()
        {
            if ( _keyProvider == null)
            {
                throw new InvalidOperationException("KeyProvider must be set.");
            }

            return new CryptoED(_keyProvider);
        }

        internal void SetKeyProvider(KeyProvider keyProvider)
        {
            throw new NotImplementedException();
        }
    }
}
