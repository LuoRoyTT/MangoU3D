using System.Collections;
using System.Collections.Generic;
using Client.Core;
using UnityEngine;

namespace Client.Framework
{
	
	public class ObjectFatory : MonoSingleton<ObjectFatory>
	{
		public T CreateObject<T>(string name) where T:Object
		{
			return default(T);
		}	
	}
}
