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
		private Dictionary<string,IAssetLoader> loadersMap;
		private List<IAssetLoader> waitForReleaseLoaders;
		private IAssetLoader LoadAssetSyn(string assetName)
		{
			return null;
		}
        private IAssetLoader LoadAssetAsyn(string assetName)
		{
			return null;
		}

		public T Get<T>(string assetName) where T:IRecyclableObject,IAssetLoader
		{
			if(loadersMap.ContainsKey(assetName))
			{
				return (T)loadersMap[assetName];
			}
			else
			{
				int index = waitForReleaseLoaders.FindIndex((a)=>{return a.AssetName.Equals(assetName);});
				if(index!=-1)
				{
					IAssetLoader loader = waitForReleaseLoaders[index];
					waitForReleaseLoaders.Remove(loader);
					return (T)loader;
				}
				else
				{
					return CreateLoader<T>(assetName);
				}
			}
		}
		private T CreateLoader<T>(string assetName) where T:IRecyclableObject,IAssetLoader
		{
			T loader = RecyclableObjectPool.Get<T>();
			loader.Init(assetName);
			return loader;
		}
        public void Load<T>(string assetName,Action<T> onFinished) where T:IRecyclableObject,IAssetLoader
		{
			T loader = Get<T>(assetName);
			loader.Load(assetName,onFinished);
		}
		public void Recycle(IAssetLoader loader)
		{
			loadersMap.Remove(loader.AssetName);
			waitForReleaseLoaders.Add(loader);
		}
		private void ReleaseLoader()
		{

		}
	}
}

