using System;
using System.Collections;
using System.Collections.Generic;
using Client.Data;
using UnityEngine;

namespace Client.ResourceModule
{
	public enum eLoadStatus
	{
		Release,
		idle,
		Loading,
		Loaded,
		Recycle
	}
	public interface IAssetLoader
	{
		string AssetName{get;}
        UnityEngine.Object Asset {get;}
		float Progress{get;}
		bool Compeleted{get;}
		eLoadStatus Status{get;}
		void Init(string assetName);
		void Load<T>(string assetName,Action<T> onFinished) where T:IRecyclableObject,IAssetLoader;
		void Recycle();

	} 
	public interface IAssetLoader<T>:IAssetLoader 
	{
		T Asset{get;}

	} 
}
