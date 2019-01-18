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
		ResID ResID{get;}
		eLoadStatus Status{get;}
		UnityEngine.Object Load();
		T Load<T>() where T:UnityEngine.Object; 
		void LoadAsyn<T>(Action<T> onCacheFinished) where T:UnityEngine.Object;
		IAssetAsynRequest LoadAsyn();
		void UnLoad();
	} 
}
