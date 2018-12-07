using System;
using System.Collections;
using System.Collections.Generic;
using Mango.Framework.Core;
using UnityEngine;


namespace Mango.Framework.Update
{
    public class UpdateModule : Singleton<UpdateModule>, IGameModule
    {
        public int Priority
		{
			get
			{
				return (int)eModulePriority.First;
			}
		}
        
        public event Action onMainThreadUpdate;
        public event Action onNetThreadUpdate;

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

