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
		private IAssetLoader LoadAssetSyn(string assetName)
		{
			return null;
		}
        private IAssetLoader LoadAssetAsyn(string assetName)
		{
			return null;
		}
		public T CreateLoader<T>(string assetName) where T:IAssetLoader,new()
		{
			IAssetLoader loader;
			if(loadersMap.TryGetValue(assetName,out loader))
			{
				return (T)loader;
			}
			else
			{
				loader = new T();
				loader.Init();
				return (T)loader;
			}
		}
		public void Recycle(string assetName)
		{

		}
		public void ReleaseLoader()
		{

		}
	}
}

