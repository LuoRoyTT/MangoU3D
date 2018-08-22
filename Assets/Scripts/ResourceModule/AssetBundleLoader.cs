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
        public string ClassKey {get{return CLASS_KEY;}}
        public string AssetName {get{return assetName;}}
        private string assetName;
        public AssetBundle Asset {get{return asset as AssetBundle;}}
        UnityEngine.Object IAssetLoader.Asset {get{return asset;}}
        private UnityEngine.Object asset;

        public float Progress{get{return progress;}}
        public float progress;

        public bool Compeleted{get{return compeleted;}}

        public eLoadStatus Status{get{return status;}}
        private eLoadStatus status;

        private bool compeleted;

        public AssetBundleLoader()
        {

        }
        public void OnUse()
        {
            status = eLoadStatus.idle;
        }

        public void Recycle()
        {
            ResourceModule.Instance.Recycle(assetName);
        }
        public void OnRelease()
        {
            status = eLoadStatus.Release;
            ResourceModule.Instance.ReleaseLoader(assetName);
        }

        public void Load<T>(string assetName,Action<T> onFinished) where T:IRecyclableObject,IAssetLoader
        {
            this.assetName = assetName;
            string path = ResourceSetting.GetAssetPathByAssetName(assetName);
            throw new NotImplementedException();
        }
    }
}

