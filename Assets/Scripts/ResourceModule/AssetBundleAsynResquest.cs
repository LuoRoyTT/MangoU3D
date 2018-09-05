using System;
using System.Collections;
using System.Collections.Generic;
using Client.Data;
using UnityEngine;

namespace Client.ResourceModule
{
    public class AssetBundleAsynResquest : IAssetAsynResquest
    {
		private UnityEngine.Object bundle;
		public AssetBundleAsynResquest(UnityEngine.Object bundle)
		{
			this.bundle = bundle;
		}
        public T GetAsset<T>() where T : UnityEngine.Object
        {
            return bundle as T;
        }
    }
}