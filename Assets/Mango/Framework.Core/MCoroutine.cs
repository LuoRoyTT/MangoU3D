using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Mango.Framework.Core
{
	public class MCoroutine  
	{
		public IEnumerator iter;

		public MCoroutine(IEnumerator it)
		{
			this.iter = it;
		}

	}
}

