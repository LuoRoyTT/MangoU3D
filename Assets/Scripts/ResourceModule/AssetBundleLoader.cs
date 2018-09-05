using System;
using System.Collections;
using System.Collections.Generic;
using Client.Data;
using UnityEngine;

namespace Client.ResourceModule
{
    public class AssetBundleLoader : IRecyclableObject,IAssetLoader
    {
        public static string CLASS_KEY="AssetBundleLoader";
        public static readonly int weight = 1;
        public string ClassKey {get{return CLASS_KEY;}}
        public string AssetName {get{return assetName;}}
        private string assetName;

        public int RefCount{get{return refCount;}}
        private int refCount;

        public eLoadStatus Status{get{return status;}}
        private eLoadStatus status;
        private AssetBundle bundle;
        private string path;
        private string[] dependencies;

        public void Init(string assetName)
        {
            status = eLoadStatus.idle;
            this.assetName = assetName;
            path = ResourceSetting.GetBundlePathByBundleName(assetName);
        }
        
        public UnityEngine.Object Load()
        {
            status = eLoadStatus.Loading;
            refCount++;
            if(ResourceModule.Instance.Exist(assetName))
            {
                return (UnityEngine.Object)bundle;
            }
            else
            {
                if(dependencies==null||dependencies.Length==0)
                {
                    dependencies = ResourceModule.Instance.Manifest.GetAllDependencies(assetName);
                }
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

                    }
                }
                AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(path);
                while (request.assetBundle==null)
                {

                }
                bundle = request.assetBundle;
                return bundle;
            }
        }

        public T Load<T>() where T : UnityEngine.Object
        {
            return Load() as T;
        }

        public void LoadAsyn<T>(Action<T> onCacheFinished) where T : UnityEngine.Object
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

        public IAssetAsynResquest LoadAsyn()
        {
            status = eLoadStatus.Loading;
            refCount++;
            IAssetAsynResquest asynRequest = null;
            if(ResourceModule.Instance.Exist(assetName))
            {
                asynRequest = new AssetBundleAsynResquest(bundle);
            }
            else
            {
                ResourceModule.Instance.StartCoroutine(LoadAssetBundle<AssetBundle>((bundle)=>
                {
                    asynRequest = new AssetBundleAsynResquest(bundle);
                }));
            }
            return asynRequest;
        }
        private void LoadedCallback<T>(Action<T> onCacheFinished) where T : UnityEngine.Object
        {
            status = eLoadStatus.Loaded;
            if(onCacheFinished!=null)
            {
                onCacheFinished(bundle as T);
            }
        }
        // public void Load(CachedCallback onCacheFinished) 
        // {
        //     status = eLoadStatus.Loading;
        //     refCount++;
        //     if(ResourceModule.Instance.Exist(assetName))
        //     {
        //         LoadedCallback(onCacheFinished);
        //     }
        //     else
        //     {
        //         ResourceModule.Instance.StartCoroutine(LoadAssetBundle(onCacheFinished));
        //     }
        // }
        private IEnumerator LoadAssetBundle<T>(Action<T> onCacheFinished) where T : UnityEngine.Object
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
            bundle = request.assetBundle;
            if(!bundle)
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
            bundle = null;
            status = eLoadStatus.Release;
            refCount = 0;
            dependencies = null;
            assetName = null;
            path = null;
        }

    }
}

