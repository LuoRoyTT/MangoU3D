using System;
using System.Collections;
using System.Collections.Generic;
using Client.Core;
using Client.Data;
using Client.FuncAllocator;
using UnityEngine;

namespace Client.ResourceModule
{ 
	public class ResourceManager : MonoSingleton<ResourceManager> 
	{

		private Dictionary<string,IAssetRes> loadersMap;
		private List<IAssetRes> waitForReleaseLoaders;
		private float releaseInterval = 10f;
		private FuncRec releaseLoaderFuncRec;
		public AssetBundleManifest Manifest{get;private set;}
		private static string LOAD_TYPE_CLASS_KEY;
		protected override void Init()
		{
			LOAD_TYPE_CLASS_KEY = "AssetBundleRes";
		}

		public IAssetRes Get(string assetName)
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
					IAssetRes loader = waitForReleaseLoaders[index];
					waitForReleaseLoaders.Remove(loader);
					return loader;
				}
				else
				{
					return CreateLoader(assetName);
				}
			}
		}

		private IAssetRes CreateLoader(string assetName) 
		{
			IAssetRes loader = RecyclableObjectPool.Get(LOAD_TYPE_CLASS_KEY) as IAssetRes;
			loader.Init(assetName);
			return loader;
		}
		public bool Exist(string assetName)
		{
			return loadersMap.ContainsKey(assetName);
		}
	}
}

