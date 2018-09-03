using System;
using System.Collections;
using System.Collections.Generic;
using Client.Core;
using Client.Data;
using Client.FuncAllocator;
using UnityEngine;

namespace Client.ResourceModule
{ 
	public delegate void CachedCallback(UnityEngine.Object asset);
	public class ResourceModule : MonoSingleton<ResourceModule> 
	{

		private Dictionary<string,IAssetLoader> loadersMap;
		private List<IAssetLoader> waitForReleaseLoaders;
		private float releaseInterval = 10f;
		private FuncRec releaseLoaderFuncRec;
		protected override void Init()
		{
			loadersMap = new Dictionary<string, IAssetLoader>();
			waitForReleaseLoaders = new List<IAssetLoader>();
			releaseLoaderFuncRec = new FuncRec(this,ReleaseLoader,releaseInterval);
			FuncAllocatorCenter.Instance.AddFuncRec(releaseLoaderFuncRec);
		}

		public IAssetLoader Get(string assetName,string classKey)
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
					return loader;
				}
				else
				{
					return CreateLoader(assetName,classKey);
				}
			}
		}
		public T Get<T>(string assetName) where T:IRecyclableObject,IAssetLoader
		{
			T loader = default(T);
			loader = (T)Get(assetName,loader.ClassKey);
			return loader;
		}
		private IAssetLoader CreateLoader(string assetName,string classKey) 
		{
			IAssetLoader loader = RecyclableObjectPool.Get(classKey) as IAssetLoader;
			loader.Init(assetName);
			return loader;
		}
		public bool Exist(string assetName)
		{
			return loadersMap.ContainsKey(assetName);
		}
		public IAssetLoader Load(string assetName,string classKey,CachedCallback onCacheFinished)
		{
			IAssetLoader loader = Get(assetName,classKey);
			loader.Load(onCacheFinished);
			return loader;
		}
        public T Load<T>(string assetName,CachedCallback onCacheFinished) where T:IRecyclableObject,IAssetLoader
		{
			T loader = Get<T>(assetName);
			loader.Load(onCacheFinished);
			return loader;
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
	}
}

