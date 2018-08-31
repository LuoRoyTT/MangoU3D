using System;
using System.Collections;
using System.Collections.Generic;
using Client.Data;
using UnityEngine;

namespace Client.ResourceModule
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
        UnityEngine.Object Asset {get;}
		float Progress{get;}
		int RefCount{get;}
		eLoadStatus Status{get;}
		void Init(string assetName);
		void Load(CachedCallback onCacheFinished);
		void Recycle();

	} 
	public interface IAssetLoader<T>:IAssetLoader 
	{
		T Asset{get;}

	} 
}
