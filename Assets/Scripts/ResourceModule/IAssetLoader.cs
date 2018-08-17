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
		Object m_asset{get;}
        eLoadError Error { get; }
		void Init();

	} 
	public interface IAssetLoader<T>:IAssetLoader 
	{
		T m_asset{get;}

	} 
}
