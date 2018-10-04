using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mango.Framework.Resource
{
    public class AssetBundleAsynRequest : IAssetAsynRequest
    {
		private UnityEngine.Object bundle;

        public object Current 
		{
			get
			{
				return null;
			}
		}
        public void SetAsset(UnityEngine.Object asset)
        {
            this.bundle = asset;
        }

        public T GetAsset<T>() where T : UnityEngine.Object
        {
            return bundle as T;
        }

        public bool MoveNext()
        {
            return bundle == null;
        }

        public void Reset()
        {

        }
    }
}