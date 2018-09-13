using System;
using System.Collections;
using System.Collections.Generic;
using Client.Data;
using UnityEngine;


namespace Client.ResourceModule
{
    public class AssetLoader : IRecyclableObject,IAssetLoader
    {
        public static string CLASS_KEY="AssetLoader";
        public static readonly int weight = 1;
        public string ClassKey {get{return CLASS_KEY;}}
        public string AssetName {get{return assetName;}}
        private string assetName;
        private UnityEngine.Object asset;
        public int RefCount{get{return refCount;}}
        private int refCount;

        public eLoadStatus Status{get{return status;}}
        private eLoadStatus status;

		private AssetBundleLoader bundleLoader;

        public void Init(string assetName)
        {
            status = eLoadStatus.idle;
            this.assetName = assetName;
        }

        public UnityEngine.Object Load()
        {
			status = eLoadStatus.Loading;
            refCount++;
			if(ResourceManager.Instance.Exist(assetName))
			{
				return asset;
			}
			if(bundleLoader==null)
			{
				string bundleName = ResourceSetting.GetBundleNameByAssetName(assetName);
				bundleLoader = ResourceManager.Instance.Get(bundleName) as AssetBundleLoader;
			}
			AssetBundle bundle = bundleLoader.Load() as AssetBundle;
			asset = bundle.LoadAsset(assetName);
			return asset;
        }

        public T Load<T>() where T : UnityEngine.Object
        {
			return Load() as T;
        }

        public void LoadAsyn<T>(Action<T> onCacheFinished) where T : UnityEngine.Object
        {
			status = eLoadStatus.Loading;
            refCount++;
			if(ResourceManager.Instance.Exist(assetName))
			{
				onCacheFinished(asset as T);
				return;
			}
			if(bundleLoader==null)
			{
				string bundleName = ResourceSetting.GetBundleNameByAssetName(assetName);
				bundleLoader = ResourceManager.Instance.Get(bundleName) as AssetBundleLoader;
			}
			bundleLoader.LoadAsyn<AssetBundle>((bundle)=>
			{
                ResourceManager.Instance.StartCoroutine(LoadAsset(assetName,bundle,onCacheFinished));
			});
        }

        public IAssetAsynRequest LoadAsyn<T>() where T:UnityEngine.Object
        {
			status = eLoadStatus.Loading;
            refCount++;
			IAssetAsynRequest asynRequest = new AssetAsynRequest();
			if(ResourceManager.Instance.Exist(assetName))
			{
				asynRequest.SetAsset(asset);
			}
			else
			{
				if(bundleLoader==null)
				{
					string bundleName = ResourceSetting.GetBundleNameByAssetName(assetName);
					bundleLoader = ResourceManager.Instance.Get(bundleName) as AssetBundleLoader;
				}
				bundleLoader.LoadAsyn<AssetBundle>((bundle)=>
				{
					ResourceManager.Instance.StartCoroutine(LoadAsset<T>(assetName,bundle,(asset)=>
					{
						asynRequest.SetAsset(asset);
					}));
				});
			}
            return asynRequest;
        }
		private IEnumerator LoadAsset<T>(string assetName,AssetBundle bundle,Action<T> onCacheFinished) where T:UnityEngine.Object
        {
            AssetBundleRequest request = bundle.LoadAssetAsync(assetName);
            //TODO 加载相应的资源
            yield return request;
            asset = request.asset;
            status = eLoadStatus.Loaded;
            if(onCacheFinished!=null)
            {
                onCacheFinished(asset as T);
            }
        }
        public void Recycle()
        {
            if(status!=eLoadStatus.Loaded)
            {
                Debug.LogError("asset尚未完成加载!");
                return;
            }
            refCount--;
            if (refCount==0)
            {
                ResourceManager.Instance.Recycle(this);
            }
            bundleLoader.Recycle();
        }
        public void OnUse()
        {

        }
        public void OnRelease()
        {
            status = eLoadStatus.Release;
            asset = null;
            refCount = 0;
        }

    }
}


