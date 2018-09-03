using System.Collections;
using System.Collections.Generic;
using Client.Data;
using UnityEngine;


namespace Client.ResourceModule
{
    public class SpriteLoader : IRecyclableObject,IAssetLoader<Sprite>
    {
        public static string CLASS_KEY="SpriteLoader";
        public static readonly int weight = 1;
        public string ClassKey {get{return CLASS_KEY;}}
        public string AssetName {get{return assetName;}}
        private string assetName;
        public Sprite Asset {get{return asset as Sprite;}}
        UnityEngine.Object IAssetLoader.Asset {get{return asset;}}
        private UnityEngine.Object asset;

        public float Progress{get{return progress;}}
        public float progress;

        public int RefCount{get{return refCount;}}
        private int refCount;

        public eLoadStatus Status{get{return status;}}
        private eLoadStatus status;

		private AssetBundleLoader bundleLoader;

        public void Init(string assetName)
        {
            this.assetName = assetName;
        }

        public void Load(CachedCallback onCacheFinished) 
        {
			status = eLoadStatus.Loading;
			if(bundleLoader==null)
			{
				string bundleName = ResourceSetting.GetBundleNameByAssetName(assetName);
				bundleLoader = ResourceModule.Instance.Get<AssetBundleLoader>(bundleName);
			}
			bundleLoader.Load((bundle)=>
			{
				Load(onCacheFinished);
			});
            ResourceModule.Instance.StartCoroutine(LoadAsset(assetName,onCacheFinished));
        }
        private IEnumerator LoadAsset(string assetName,CachedCallback onCacheFinished)
        {

			string path = ResourceSetting.GetAssetPathByAssetName(assetName);
            //TODO 加载相应的bundle和依赖的bundle
            yield return null;
            refCount++;
            status = eLoadStatus.Loaded;
            if(onCacheFinished!=null)
            {
                onCacheFinished(asset);
            }
        }
        public void Recycle()
        {
            refCount--;
            if (refCount==0)
            {
                ResourceModule.Instance.Recycle(this);
            }
        }
        public void OnUse()
        {
            status = eLoadStatus.idle;
        }
        public void OnRelease()
        {
            asset = null;
            status = eLoadStatus.Release;
            progress = 0f;
            refCount = 0;
        }
    }
}


