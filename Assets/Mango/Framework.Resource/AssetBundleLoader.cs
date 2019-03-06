using System;
using System.Collections;
using System.Collections.Generic;
using Mango.Framework.Core;
using Mango.Framework.Task;
using Mango.Framework.Task.Coroutine;
using UnityEngine;

namespace Mango.Framework.Resource
{
    public class AssetBundleLoader : MangoObject 
    {
        public static readonly int weight = 1;
        public string BundleName
        {
            get
            {
                return bundleName;
            }
        }
        private string bundleName;
        private int refCount;
        public eLoadStatus Status{get{return status;}}
        private eLoadStatus status;
        private AssetBundle bundle;
        private string path;
        private string[] dependencies;
        private ResourceModule resourceModule;
        private TaskModule taskModule;

        public AssetBundleLoader(string bundleName)
        {
            status = eLoadStatus.idle;
            this.bundleName = bundleName;
            path = ResourceSetting.GetBundlePathByBundleName(bundleName);
            resourceModule = Mango.GetModule<ResourceModule>();
            taskModule = Mango.GetModule<TaskModule>();
        }

        private bool CheckLoadStatus()
        {
            if(status == eLoadStatus.Loading)
                return false;
			status = eLoadStatus.Loading;
            refCount++;
            return true;
        }

        public AssetBundle Load()
        {
            if(!CheckLoadStatus()) return null;
            if(bundle)
            {
                return bundle;
            }
            else
            {
                if(dependencies==null||dependencies.Length==0)
                {
                    dependencies = resourceModule.Manifest.GetAllDependencies(bundleName);
                }
                if(dependencies!=null)
                {
                    for (int i = 0; i < dependencies.Length; i++)
                    {
                        AssetBundleLoader loader = resourceModule.GetBundleLoader(dependencies[i]);
                        loader.Load();
                    }
                }
                var bundle = AssetBundle.LoadFromFile(path);
                return bundle;
            }
        }

        public void LoadAsyn(Action<AssetBundle> onCacheFinished)
        {
            if(!CheckLoadStatus()) return;
            if(bundle)
            {
                LoadedCallback(onCacheFinished);
            }
            else
            {
                StartCoroutine(LoadAssetBundle(onCacheFinished));
            }
        }

        public IAssetAsynRequest LoadAsyn()
        {
            if(!CheckLoadStatus()) return null;
            IAssetAsynRequest asynRequest = new AssetBundleAsynRequest();
            if(bundle)
            {
                asynRequest.SetAsset(bundle);
            }
            else
            {
                StartCoroutine(LoadAssetBundle((bundle)=>
                {
                    asynRequest.SetAsset(bundle);
                }));
                // AppendCoroutine(LoadAssetBundle<AssetBundle>((bundle)=>
                // {
                //     asynRequest.SetAsset(bundle);
                // }));
            }
            return asynRequest;
        }
        private void LoadedCallback(Action<AssetBundle> onCacheFinished)
        {
            status = eLoadStatus.Loaded;
            if(onCacheFinished!=null)
            {
                onCacheFinished(bundle);
            }
        }

        private IEnumerator LoadAssetBundle(Action<AssetBundle> onCacheFinished)
        {
            if(dependencies==null||dependencies.Length==0)
            {
                dependencies = resourceModule.Manifest.GetAllDependencies(bundleName);
            }
            if(dependencies!=null)
            {
                int dependenciesLoadedCount = 0;
                int dependenciesCount = dependencies.Length;
                for (int i = 0; i < dependenciesCount; i++)
                {
                    AssetBundleLoader loader = resourceModule.GetBundleLoader(dependencies[i]);
                    loader.LoadAsyn((bundle)=>
                    {
                        dependenciesLoadedCount++;
                    });
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
        public void UnLoad()
        {
            if(status!=eLoadStatus.Loaded)
            {
                Debug.LogError("asset尚未完成加载!");
                return;
            }
            bundle.GetAllAssetNames();
            refCount--;
            if (refCount==0)
            {
                resourceModule.AddReleaseBundleTask(bundleName);
            }
            for (int i = 0; i < dependencies.Length; i++)
            {
                resourceModule.GetBundleLoader(dependencies[i]).UnLoad();
            }
        }
    }
}

