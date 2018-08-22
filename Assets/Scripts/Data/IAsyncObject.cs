using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client.Data
{
	public interface IAsyncObject
	{
		IAsyncObject Next{get;set;}
		void Start();
		void Compelete();
	} 
}

