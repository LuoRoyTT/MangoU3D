using System;
using System.Collections;
using System.Collections.Generic;
using Client.Core;
using UnityEngine;

namespace Client.ResourceModule
{ 
	public class ResourceModule : MonoSingleton<ResourceModule> 
	{
		private Dictionary<string,IAssetLoader> loadersMap = new Dictionary<string,IAssetLoader>();
		public T Create<T>(string assetname) where T:IAssetLoader
        {
			if(loadersMap.ContainsKey(assetname))
			{
				return (T)loadersMap[assetname];
			}
			else
			{
				return (T)Load(assetname) 
			}
		}

	}
}

