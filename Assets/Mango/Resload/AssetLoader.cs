using System;
using System.Collections;
using System.Collections.Generic;
using Mango.Task;
using Mango.Task.Coroutine;
using UnityEngine;


namespace Mango.Resload
{
    public class AssetLoader : MangoObject, IAssetLoader
    {
        public static readonly int weight = 1;
        public ResID ResID 
        {
            get
            {
                return resID;
            }
        }
        private ResID resID;
        private UnityEngine.Object asset;

        public eLoadStatus Status{get{return status;}}
        private eLoadStatus status;

		private AssetBundleLoader bundleLoader;
        private ResourceModule resourceModule;
        public AssetLoader(int id)
        {
            status = eLoadStatus.idle;
            resID = ResID.New(id);
            resourceModule = Mango.GetModule<ResourceModule>();
        }

        private bool CheckLoadStatus()
        {
            if(status == eLoadStatus.Loading)
                return false;
			status = eLoadStatus.Loading;
            return true;
        }

        public UnityEngine.Object Load()
        {
            if(!CheckLoadStatus()) return null;
			if(asset)
			{
				return asset;
			}
			if(bundleLoader==null)
			{
				string bundleName = ResourceSetting.GetBundleName(resID);
				bundleLoader = resourceModule.GetBundleLoader(bundleName);
			}
			AssetBundle bundle = bundleLoader.Load();
			asset = bundle.LoadAsset(resID.assetName);
            status = eLoadStatus.Loaded;
			return asset;
        }

        public T Load<T>() where T : UnityEngine.Object
        {
			return Load() as T;
        }
        public IAssetAsynRequest LoadAsyn()
        {
            if(!CheckLoadStatus()) return null;
			IAssetAsynRequest asynRequest = new AssetAsynRequest();
			if(asset)
			{
				asynRequest.SetAsset(asset);
			}
			else
			{
				if(bundleLoader==null)
				{
					string bundleName = ResourceSetting.GetBundleName(resID);
					bundleLoader = resourceModule.GetBundleLoader(bundleName);
				}
				bundleLoader.LoadAsyn((bundle)=>
				{
                    StartCoroutine(LoadAsset((asset)=>
					{
						asynRequest.SetAsset(asset);
					}));
					// AppendCoroutine(LoadAsset(assetName,bundle,(asset)=>
					// {
					// 	asynRequest.SetAsset(asset);
					// }));
				});
			}
            return asynRequest;
        }
		private IEnumerator LoadAsset(Action<UnityEngine.Object> onCacheFinished)
        {
            AssetBundle bundle = bundleLoader.Load();
            AssetBundleRequest request = bundle.LoadAssetAsync(resID.assetName);
            //TODO 加载相应的资源
            yield return request;
            asset = request.asset;
            status = eLoadStatus.Loaded;
            if(onCacheFinished!=null)
            {
                onCacheFinished(asset);
            }
        }
        public void LoadAsyn<T>(Action<T> onCacheFinished) where T : UnityEngine.Object
        {
            StartCoroutine(LoadAsset<T>(onCacheFinished));
        }
        private IEnumerator LoadAsset<T>(Action<T> onCacheFinished) where T : UnityEngine.Object
        {
            IAssetAsynRequest request = LoadAsyn();
            yield return request;
            onCacheFinished(request.GetAsset<T>());
        }

        public void UnLoad()
        {
            if(status!=eLoadStatus.Loaded)
            {
                Debug.LogError("asset尚未完成加载!");
                return;
            }

            bundleLoader.UnLoad();
            status = eLoadStatus.Release;
            asset = null;
        }
    }
}


