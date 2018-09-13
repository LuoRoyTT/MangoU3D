using System.Collections;
using System.Collections.Generic;
using Client.Core;
using UnityEngine;

namespace Client.Framework
{
	public class GameObjectPool : MonoSingleton<GameObjectPool> 
	{
		Dictionary<string,List<GameObject>> pool;
		public GameObject Create(string name)
		{
			return null;
		}
		public void Recycle(GameObject go)
		{

		}
	}
}
