using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client.ResourceModule
{
	public enum eLoadError
	{
		
	}
	public interface IAssetLoader<T>
	{
		T m_asset{get;}
        bool m_completed { get; }
        bool m_success { get; }
        eLoadError error { get; }

	} 
}
