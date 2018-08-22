using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client.Data
{
	public  interface IRecyclableObject
	{ 
		string ClassKey{get;}
		void OnUse();

		void OnRelease();

	}
}