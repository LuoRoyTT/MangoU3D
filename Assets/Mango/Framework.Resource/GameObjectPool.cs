using System.Collections;
using System.Collections.Generic;
using Mango.Framework.Core;
using UnityEngine;

namespace Mango.Framework.Resource
{
	public class GameObjectPool : MonoSingleton<GameObjectPool> 
	{
		Dictionary<string,List<GameObject>> pool;
		Dictionary<string,GameObject> staticMap;
		public GameObject CreateGO(string name)
		{
			return null;
		}
		public GameObject CreateStaticGO(string name)
		{
			return null;
		}
		public void Recycle(GameObject go)
		{

		}
		public void Destroy(GameObject go)
		{
			if(pool.ContainsKey(go.name))
			{
				pool[go.name].Remove(go);
			}
			else if(staticMap.ContainsKey(go.name))
			{
				staticMap.Remove(go.name);
			}
			GameObject.DestroyImmediate(go);
		}
	}
}
