using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client.ResourceModule
{
	public enum eLoadStatus
	{

	}
	public interface IAssetLoader
	{
		string AssetName{get;}
		Object Asset{get;}
		float Progress{get;}
		bool Compeleted{get;}
		eLoadStatus Status{get;}
		void Load();
		void Recycle();

	} 
	public interface IAssetLoader<T>:IAssetLoader 
	{
		T Asset{get;}

	} 
}
