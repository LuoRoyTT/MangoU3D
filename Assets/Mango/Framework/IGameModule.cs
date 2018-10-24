using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mango.Framework
{
	public interface IGameModule 
	{
		int Priority{get;}
		void Init();
		void Release();

    }
}

