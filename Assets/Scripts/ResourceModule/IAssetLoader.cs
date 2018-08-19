using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client.ResourceModule
{
	public enum eLoadError
	{
		None
	}
	public interface IAssetLoader
	{
		Object Asset{get;}
        eLoadError Error { get; }
		void Init();

	} 
	public interface IAssetLoader<T>:IAssetLoader 
	{
		T Asset{get;}

	} 
}
