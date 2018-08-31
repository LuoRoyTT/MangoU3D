using System;
using System.Collections;
using System.Collections.Generic;
using Client.Data;
using UnityEngine;

namespace Client.ResourceModule
{
    public class AssetBundleLoader : IRecyclableObject,IAssetLoader<AssetBundle>
    {
        public static string CLASS_KEY="AssetBundleLoader";
        public static readonly int weight = 1;
        public string ClassKey {get{return CLASS_KEY;}}
        public string AssetName {get{return assetName;}}
        private string assetName;
        public AssetBundle Asset {get{return asset as AssetBundle;}}
        UnityEngine.Object IAssetLoader.Asset {get{return asset;}}
        private UnityEngine.Object asset;

        public float Progress{get{return progress;}}
        public float progress;

        public int RefCount{get{return refCount;}}
        private int refCount;

        public eLoadStatus Status{get{return status;}}
        private eLoadStatus status;

        public void Init(string assetName)
        {
            this.assetName = assetName;
            progress = 0f;
            refCount = 0;
        }

        public void Load(CachedCallback onCacheFinished) 
        {
            string path = ResourceSetting.GetAssetPathByAssetName(assetName);
            ResourceModule.Instance.StartCoroutine(LoadAssetBundle(path,onCacheFinished));
        }
        private IEnumerator LoadAssetBundle(string path,CachedCallback onCacheFinished)
        {
            yield return null;
            refCount++;
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
        }
    }
}

