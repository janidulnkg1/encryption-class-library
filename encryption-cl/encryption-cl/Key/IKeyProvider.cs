﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace encryption_cl
{
    public interface IKeyProvider
    {
        byte[] GetKey();
        void SetKey(byte[] key);
    }
}