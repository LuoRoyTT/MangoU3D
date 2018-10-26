using System.Collections;
using System.Collections.Generic;
using Mango.Framework.Core;
using UnityEngine;

namespace Mango.Framework.Network
{
    public class NetworkModule : Singleton<NetworkModule>,IGameModule
    {
        public int Priority 
        {
            get
            {
                return 1;
            }
        }

        public void Init()
        {
            throw new System.NotImplementedException();
        }

        public void Release()
        {
            throw new System.NotImplementedException();
        }
    }
}

