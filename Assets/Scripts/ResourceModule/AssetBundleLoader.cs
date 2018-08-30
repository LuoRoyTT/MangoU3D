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
        private bool compeleted;

        public eLoadStatus Status{get{return status;}}
        private eLoadStatus status;

        public void Init(string assetName)
        {
            this.assetName = assetName;
            progress = 0f;
        }
        public void OnUse()
        {
            status = eLoadStatus.idle;
        }

        public void Recycle()
        {
            status = eLoadStatus.Recycle;
            ResourceModule.Instance.Recycle(this);
        }
        public void OnRelease()
        {
            asset = null;
            status = eLoadStatus.Release;
        }

        public void Load<T>(string assetName,Action<T> onFinished) where T:IRecyclableObject,IAssetLoader
        {
            this.assetName = assetName;
            string path = ResourceSetting.GetAssetPathByAssetName(assetName);
            throw new NotImplementedException();
        }
    }
}

