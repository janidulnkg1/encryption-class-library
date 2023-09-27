using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace encryption_cl
{
    public class KeyProvider
    {
        private byte[] key;

        public byte[] GetKey()
        {
            return key;
        }

        public void SetKey(byte[] key)
        {
            this.key = key;
        }
    }
}
