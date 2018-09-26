using System;
using System.Collections;
using System.Collections.Generic;
using Client.Data;
using UnityEngine;

namespace Mango.Framework.Resource
{
    public class AssetAsynRequest : IAssetAsynRequest
    {
		private UnityEngine.Object asset;

        public object Current 
		{
			get
			{
				return null;
			}
		}
        public void SetAsset(UnityEngine.Object asset)
        {
            this.asset = asset;
        }
        public T GetAsset<T>() where T : UnityEngine.Object
        {
            return asset as T;
        }

        public bool MoveNext()
        {
            return asset == null;
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}