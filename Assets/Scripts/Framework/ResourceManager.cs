using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Client.Core;

namespace Client.Framework
{
    public class ResourceManager : MonoSingleton<ResourceManager>
    {

        private Dictionary<string,AssetBundle> bundleMap = new Dictionary<string, AssetBundle>();
        public AssetBundle LoadAssetBundle(string bundleName)
        {
            return null;
        }

        public void AsyncLoadAssetBundle(string bundleName,Action<AssetBundle> callBack)
        {

        }

        public void UnLoadAssetBundle(string bundleName)
        {

        }
        public void UnLoadAssetBundle(AssetBundle bundle)
        {

        }
        public void Clear()
        {

        }
    }
}
