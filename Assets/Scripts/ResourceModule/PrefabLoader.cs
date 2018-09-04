using System.Collections;
using System.Collections.Generic;
using Client.Data;
using UnityEngine;


namespace Client.ResourceModule
{
    public class PrefabLoader : IRecyclableObject,IAssetLoader<GameObject>
    {
        public static string CLASS_KEY="PrefabLoader";
        public static readonly int weight = 1;
        public string ClassKey {get{return CLASS_KEY;}}
        public string AssetName {get{return assetName;}}
        private string assetName;
        public GameObject Asset {get{return asset as GameObject;}}
        UnityEngine.Object IAssetLoader.Asset {get{return asset;}}
        private UnityEngine.Object asset;

        public float Progress{get{return progress;}}
        private float progress;

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

        public void Load(CachedCallback onCacheFinished) 
        {
			status = eLoadStatus.Loading;
            refCount++;
			if(bundleLoader==null)
			{
				string bundleName = ResourceSetting.GetBundleNameByAssetName(assetName);
				bundleLoader = ResourceModule.Instance.Get<AssetBundleLoader>(bundleName);
			}
			bundleLoader.Load((bundle)=>
			{
                ResourceModule.Instance.StartCoroutine(LoadAsset(assetName,(AssetBundle)bundle,onCacheFinished));
			});
        }
        private IEnumerator LoadAsset(string assetName,AssetBundle bundle,CachedCallback onCacheFinished)
        {
            AssetBundleRequest request = bundle.LoadAssetAsync(assetName);
            //TODO 加载相应的资源
            yield return request;
            asset = request.asset;
            status = eLoadStatus.Loaded;
            if(onCacheFinished!=null)
            {
                onCacheFinished(asset);
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
                ResourceModule.Instance.Recycle(this);
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
            progress = 0f;
            refCount = 0;
        }
    }
}


