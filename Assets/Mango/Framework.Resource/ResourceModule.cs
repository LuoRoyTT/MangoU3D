using System;
using System.Collections;
using System.Collections.Generic;
using Mango.Framework.Core;
using UnityEngine;

namespace Mango.Framework.Resource
{ 
	public struct ResID
	{
		static Dictionary<int,ResID> cacheIDs = new Dictionary<int, ResID>();
		public static ResID New(int id)
		{
			ResID resID;
			if(!cacheIDs.TryGetValue(id,out resID))
			{
				resID = new ResID(id); 
			}
			return resID;
		}
		private int id;
		private string path;
        internal string assetName;

        public override string ToString()
		{
			if(string.IsNullOrEmpty(path))
			{

			}
			return path;
		}
		private ResID(int id)
		{
			this.id = id;
			path = null;
			assetName = null;
		}
	}
	public class ResourceModule : GameModule
	{
		private Dictionary<string,AssetBundleLoader> bundleLoadersMap;
		private Dictionary<int,AssetLoader> assetsMap;
		private List<IAssetLoader> waitForReleaseBundleLoaders;
		private float releaseInterval = 10f;
		public AssetBundleManifest Manifest{get;private set;}

		public AssetLoader GetAssetLoader(int id)
		{
			if(assetsMap.ContainsKey(id))
			{
				return assetsMap[id];
			}
			else
			{
				AssetLoader loader = new AssetLoader(id);
				assetsMap.Add(id,loader);
				return loader;
			}
		}

		public AssetBundleLoader GetBundleLoader(string bundleName)
		{
			
			if(bundleLoadersMap.ContainsKey(bundleName))
			{
				return bundleLoadersMap[bundleName];
			}
			else
			{
				AssetBundleLoader loader = new AssetBundleLoader(bundleName);
				bundleLoadersMap.Add(bundleName,loader);
				return loader;
			}
		}

		public void UnLoad(int id)
		{
			AssetLoader loader;
			if(!assetsMap.TryGetValue(id,out loader))
			{
				Debug.LogError("asset not exits!");
			}
			loader.UnLoad();
		}
		internal void AddReleaseBundleTask(string bundleName)
		{

		}
    }
}

