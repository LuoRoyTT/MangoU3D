using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client.ResourceModule
{
	public interface IAssetAsynLoader
	{
		AssetBundleRequest request{get;}
	}
	public interface IAssetRes
	{
		string AssetName{get;}
		void Init(string assetName);
		T Load<T>() where T:UnityEngine.Object; 
		void LoadAsyn<T>(Action<T> onCacheFinished) where T:UnityEngine.Object;
		IAssetAsynLoader LoadAsyn();
		void Recycle();
	} 
}
