using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Mango.Framework.Coroutine
{
	public class MWaitForSeconds : IYieldBase
	{
		public float interval;

		public MWaitForSeconds(float interval)
		{
			this.interval = interval;
		}

	}
}

