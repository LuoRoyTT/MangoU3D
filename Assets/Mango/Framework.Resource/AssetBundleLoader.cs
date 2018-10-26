using System;
using System.Collections;
using System.Collections.Generic;
using Mango.Coroutine;
using Mango.Framework.Core;
using Mango.Framework.Task;
using UnityEngine;

namespace Mango.Framework.Resource
{
    public class AssetBundleLoader : IRecyclableObject,IAssetLoader,IProcessTask
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
        private bool CheckLoadStatus()
        {
            if(status == eLoadStatus.Loading)
                return false;
			status = eLoadStatus.Loading;
            refCount++;
            return true;
        }

        public UnityEngine.Object Load()
        {
            if(!CheckLoadStatus()) return null;
            if(bundle)
            {
                return (UnityEngine.Object)bundle;
            }
            else
            {
                if(dependencies==null||dependencies.Length==0)
                {
                    dependencies = ResourceModule.instance.Manifest.GetAllDependencies(assetName);
                }
                if(dependencies!=null)
                {
                    for (int i = 0; i < dependencies.Length; i++)
                    {
                        IAssetLoader loader = ResourceModule.instance.Get(dependencies[i]);
                        loader.Load();
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
            if(!CheckLoadStatus()) return;
            if(bundle)
            {
                LoadedCallback(onCacheFinished);
            }
            else
            {
                this.StartCoroutineTask(LoadAssetBundle(onCacheFinished));
                // AppendCoroutine(LoadAssetBundle(onCacheFinished));
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
                this.StartCoroutineTask(LoadAssetBundle<AssetBundle>((bundle)=>
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
        private void LoadedCallback<T>(Action<T> onCacheFinished) where T : UnityEngine.Object
        {
            status = eLoadStatus.Loaded;
            if(onCacheFinished!=null)
            {
                onCacheFinished(bundle as T);
            }
        }

        private IEnumerator LoadAssetBundle<T>(Action<T> onCacheFinished) where T : UnityEngine.Object
        {
            if(dependencies==null||dependencies.Length==0)
            {
                dependencies = ResourceModule.instance.Manifest.GetAllDependencies(assetName);
            }
            if(dependencies!=null)
            {
                int dependenciesLoadedCount = 0;
                int dependenciesCount = dependencies.Length;
                for (int i = 0; i < dependenciesCount; i++)
                {
                    AssetBundleLoader loader = ResourceModule.instance.Get(dependencies[i]) as AssetBundleLoader;
                    loader.LoadAsyn<AssetBundle>((bundle)=>
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
        public void Recycle()
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
                ResourceModule.instance.Recycle(this);
            }
            for (int i = 0; i < dependencies.Length; i++)
            {
                ResourceModule.instance.Get(dependencies[i]).Recycle();
            }
        }
        public void OnUse()
        {

        }
        public void OnRelease()
        {
            status = eLoadStatus.Release;
            this.RemoveAllTasks();
            refCount = 0;
            bundle = null;
            dependencies = null;
            assetName = null;
            path = null;
        }

    }
}

