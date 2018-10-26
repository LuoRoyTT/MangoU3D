using System;
using System.Collections;
using System.Collections.Generic;
using Mango.Framework.Core;
using UnityEngine;

namespace Mango.Framework.Resource
{ 
	public class ResourceModule : Singleton<ResourceModule>,IGameModule
	{

		private Dictionary<string,IAssetLoader> loadersMap;
		private List<IAssetLoader> waitForReleaseLoaders;
		private float releaseInterval = 10f;
		public AssetBundleManifest Manifest{get;private set;}

        public int Priority
		{
			get
			{
				return 1;
			}
		}

        private static string LOAD_TYPE_CLASS_KEY;
		public void Init()
		{
			LOAD_TYPE_CLASS_KEY = "AssetBundleRes";
		}

		public IAssetLoader Get(string assetName)
		{
			if(loadersMap.ContainsKey(assetName))
			{
				return loadersMap[assetName];
			}
			else
			{
				int index = waitForReleaseLoaders.FindIndex((a)=>{return a.AssetName.Equals(assetName);});
				if(index!=-1)
				{
					IAssetLoader loader = waitForReleaseLoaders[index];
					waitForReleaseLoaders.Remove(loader);
					loadersMap.Add(assetName,loader);
					return loader;
				}
				else
				{
					return CreateLoader(assetName);
				}
			}
		}

		private IAssetLoader CreateLoader(string assetName) 
		{
			IAssetLoader loader = RecyclableObjectPool.Get(LOAD_TYPE_CLASS_KEY) as IAssetLoader;
			loadersMap.Add(assetName,loader);
			loader.Init(assetName);
			return loader;
		}
		public bool Exist(string assetName)
		{
			return loadersMap.ContainsKey(assetName);
		}
		public void Recycle(IAssetLoader loader)
		{
			loadersMap.Remove(loader.AssetName);
			waitForReleaseLoaders.Add(loader);
		}
		public void ReleaseLoader()
		{
			if(waitForReleaseLoaders==null || waitForReleaseLoaders.Count==0) return;
			for (int index = 0; index < waitForReleaseLoaders.Count; index++)
			{
				RecyclableObjectPool.Release(waitForReleaseLoaders[index] as IRecyclableObject);
			}
		}

        public void Release()
        {
            throw new NotImplementedException();
        }
    }
}

