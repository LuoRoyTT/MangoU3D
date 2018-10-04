using System;
using System.Collections;
using System.Collections.Generic;
using Mango.Framework.Core;
using UnityEngine;

namespace Mango.Framework.Frame
{
	public class FrameManager : MonoSingleton<FrameManager> 
	{
		private DateTime lastFrameTime;
		private long lastFrameStamp;
		private int frameCount;

		private Action logicUpdateHandle;

		void Update()
		{

		}
		
	}
}

