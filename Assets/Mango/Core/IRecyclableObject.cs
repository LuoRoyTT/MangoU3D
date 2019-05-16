using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mango.Core
{
	public  interface IRecyclableObject
	{ 
		string ClassKey{get;}
		void OnUse();

		void OnRelease();

	}
}