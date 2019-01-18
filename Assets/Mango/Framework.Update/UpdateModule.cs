using System;
using System.Collections;
using System.Collections.Generic;
using Mango.Framework.Core;
using UnityEngine;


namespace Mango.Framework.Update
{
    public class UpdateModule : GameModule
    {
        public override int Priority
		{
			get
			{
				return (int)eModulePriority.First;
			}
		}
        
        public event Action onMainThreadUpdate;
        public event Action onNetThreadUpdate;

    }
}

