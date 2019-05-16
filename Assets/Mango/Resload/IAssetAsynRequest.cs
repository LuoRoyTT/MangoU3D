using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mango.Resload
{
	public interface IAssetAsynRequest:IEnumerator
	{
		void SetAsset(UnityEngine.Object asset);
		T GetAsset<T>() where T:UnityEngine.Object; 
	}
}
