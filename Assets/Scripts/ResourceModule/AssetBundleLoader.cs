﻿using System;
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
        }
        private void LoadedCallbac(CachedCallback onCacheFinished)
        {
            status = eLoadStatus.Loaded;
            if(onCacheFinished!=null)
            {
                onCacheFinished(asset);
            }
        }
        public void Load(CachedCallback onCacheFinished) 
        {
            status = eLoadStatus.Loading;
            if(ResourceModule.Instance.Exist(assetName))
            {
                LoadedCallbac(onCacheFinished);
            }
            else
            {
                ResourceModule.Instance.StartCoroutine(LoadAssetBundle(assetName,onCacheFinished));
            }
        }
        private IEnumerator LoadAssetBundle(string path,CachedCallback onCacheFinished)
        {
            //TODO 加载相应的bundle和依赖的bundle
            yield return null;
            refCount++;
            LoadedCallbac(onCacheFinished);
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

