using System;
using System.Collections;
using System.Collections.Generic;
using Client.Core;
using Client.Data;
using UnityEngine;

namespace Client.ResourceModule
{ 
	public class ResourceModule : MonoSingleton<ResourceModule> 
	{
		private Dictionary<string,IAssetLoader> loadersMap = new Dictionary<string,IAssetLoader>();
		private List<IAssetLoader> waitForReleaseLoaders = new List<IAssetLoader>();
		private IAssetLoader LoadAssetSyn(string assetName)
		{
			return null;
		}
        private IAssetLoader LoadAssetAsyn(string assetName)
		{
			return null;
		}
		public T Get<T>(string assetName) where T:RecyclableObject,IAssetLoader
		{
			T loader = null;
			if(loadersMap.ContainsKey(assetName))
			{
				loader=(T)loadersMap[assetName];
			}
			else
			{
				Debug.LogError("ResourceModule中不存在"+assetName+"的loader");
			}
			return loader;
		}
		public void Load<T>(string assetName,Action<T> onFinished) where T:RecyclableObject,IAssetLoader
		{
			T loader = null;
			if(loadersMap.ContainsKey(assetName))
			{
				loader=(T)loadersMap[assetName];
			}
			else
			{
				loader = (T)RecyclableObjectPool.Get(loader.ClassKey);
			}
			loader.Load();
		}
		public void Recycle(string assetName)
		{

		}
		public void ReleaseLoader(string assetName)
		{
            
		}
	}
}

