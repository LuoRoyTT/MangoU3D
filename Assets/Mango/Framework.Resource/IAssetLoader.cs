using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mango.Framework.Resource
{
	public enum eLoadStatus
	{
		idle,
		Loading,
		Loaded,
		Release
	}

	public interface IAssetLoader
	{
		string AssetName{get;}
		eLoadStatus Status{get;}
		int RefCount{get;}
		void Init(string assetName);
		UnityEngine.Object Load();
		T Load<T>() where T:UnityEngine.Object; 
		void LoadAsyn<T>(Action<T> onCacheFinished) where T:UnityEngine.Object;
		IAssetAsynRequest LoadAsyn();
		void Recycle();
	} 
}
