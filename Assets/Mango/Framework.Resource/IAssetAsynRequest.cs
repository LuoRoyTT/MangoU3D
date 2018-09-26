using System;
using System.Collections;
using System.Collections.Generic;
using Client.Data;
using UnityEngine;

namespace Mango.Framework.Resource
{
	public interface IAssetAsynRequest:IEnumerator
	{
		void SetAsset(UnityEngine.Object asset);
		T GetAsset<T>() where T:UnityEngine.Object; 
	}
}
