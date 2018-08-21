using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client.Async
{
	public delegate void Callback();
	public class AsyncCallback 
	{
		private Callback callback;
		private float interval;
		public AsyncCallback(float interval,Callback ation)
		{
			this.interval = interval;
			this.callback = ation;
		}
	
		public void AddListener(Callback action)
		{
			callback+=action;
		}
		public void RemoveListener(Callback action)
		{
			callback-=action;
		}
		public void RemoveAllListeners()
		{
			callback=null;
		}
	}
}

