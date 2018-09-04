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
        private float progress;

        public int RefCount{get{return refCount;}}
        private int refCount;

        public eLoadStatus Status{get{return status;}}
        private eLoadStatus status;
        private string path;
        private string[] dependencies;

        public void Init(string assetName)
        {
            status = eLoadStatus.idle;
            this.assetName = assetName;
            path = ResourceSetting.GetBundlePathByBundleName(assetName);
        }
        private void LoadedCallback(CachedCallback onCacheFinished)
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
            refCount++;
            if(ResourceModule.Instance.Exist(assetName))
            {
                LoadedCallback(onCacheFinished);
            }
            else
            {
                ResourceModule.Instance.StartCoroutine(LoadAssetBundle(onCacheFinished));
            }
        }
        private IEnumerator LoadAssetBundle(CachedCallback onCacheFinished)
        {
            //TODO 加载相应的bundle和依赖的bundle
            dependencies = ResourceModule.Instance.Manifest.GetAllDependencies(assetName);
            if(dependencies!=null)
            {
                int dependenciesLoadedCount = 0;
                int dependenciesCount = dependencies.Length;
                for (int i = 0; i < dependenciesCount; i++)
                {
                    ResourceModule.Instance.Load<AssetBundleLoader>(dependencies[i],(asset)=>
                    {
                        dependenciesLoadedCount++;
                    });
                    // AssetBundleLoader loader = ResourceModule.Instance.Get<AssetBundleLoader>(dependencies[i]);
                    // loader.Load((asset)=>
                    // {
                    //     dependenciesLoadedCount++;
                    // });
                }
                while (dependenciesLoadedCount<dependenciesCount)
                {
                    yield return null;
                }
            }
            AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(path);
            yield return request;
            asset = request.assetBundle;
            if(!asset)
            {
                Debug.LogError("Bundle加载失败!");
            }
            LoadedCallback(onCacheFinished);
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
            for (int i = 0; i < dependencies.Length; i++)
            {
                ResourceModule.Instance.Get<AssetBundleLoader>(dependencies[i]).Recycle();
            }
        }
        public void OnUse()
        {

        }
        public void OnRelease()
        {
            asset = null;
            status = eLoadStatus.Release;
            progress = 0f;
            refCount = 0;
            dependencies = null;
            assetName = null;
            path = null;
        }
    }
}

