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

        public AssetBundle m_asset {get{return m_asset as AssetBundle;}}

        Object IAssetLoader.m_asset {get{return m_asset ;}}
		
        public eLoadError Error {get{return error;}}
        private eLoadError error = eLoadError.None;

        public void Init()
        {
            throw new System.NotImplementedException();
        }
    }
}

