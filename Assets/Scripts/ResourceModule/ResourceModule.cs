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
		public T Get<T>(string assetName) where T:IRecyclableObject,IAssetLoader
		{
			if(loadersMap.ContainsKey(assetName))
			{
				return (T)loadersMap[assetName];
			}
			else
			{
				return default(T);
			}
			
		}
		public void Load<T>(string assetName,Action<T> onFinished) where T:IRecyclableObject,IAssetLoader
		{
			T loader = default(T);
			if(loadersMap.ContainsKey(assetName))
			{
				loader=(T)loadersMap[assetName];
			}
			else
			{
				loader = (T)RecyclableObjectPool.Get(loader.ClassKey);
			}
			string bundleName = ResourceSetting.GetBundleNameByAssetName(assetName);
			AssetBundleLoader bundleLoader = Get<AssetBundleLoader>(bundleName);
			if(bundleLoader==null) 
			{
				bundleLoader.Load(bundleName,onFinished);
			}
			else
			{
				loader.Load(assetName,onFinished);
			}

		}
		public void Recycle(string assetName)
		{

		}
		public void ReleaseLoader(string assetName)
		{
            
		}
	}
}

