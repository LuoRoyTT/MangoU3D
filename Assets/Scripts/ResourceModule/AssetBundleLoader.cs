using System.Collections;
using System.Collections.Generic;
using Client.Data;
using UnityEngine;

namespace Client.ResourceModule
{
    public class AssetBundleLoader : RecyclableObject,IAssetLoader<AssetBundle>
    {
        public static string CLASS_KEY="AssetBundleLoader";
        public override string ClassKey {get{return CLASS_KEY;}}
        public string AssetName {get{return assetName;}}
        private string assetName;
        public AssetBundle Asset {get{return asset as AssetBundle;}}
        Object IAssetLoader.Asset {get{return asset;}}
        private Object asset;

        public float Progress{get{return progress;}}
        public float progress;

        public bool Compeleted{get{return compeleted;}}

        public eLoadStatus Status{get{return status;}}
        private eLoadStatus status;

        private bool compeleted;

        public AssetBundleLoader()
        {

        }
        public void Load()
        {
            throw new System.NotImplementedException();
        }
        public override void OnUse()
        {
            
        }

        public void Recycle()
        {
            ResourceModule.Instance.Recycle(assetName);
        }
        public override void OnRelease()
        {
            ResourceModule.Instance.ReleaseLoader(assetName);
        }
    }
}

