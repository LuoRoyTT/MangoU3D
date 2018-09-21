using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Mango.Framework.Core;
using Client.Data;
using UnityEngine;

namespace Client.Async
{
	public class AsyncCenter : MonoSingleton<AsyncCenter>  
	{
		public static bool autoStart;
		public static bool autoRelease;
		private static List<AsyncQueue> queueCollection;
		protected override void Init()
		{

		} 
		public static AsyncQueue CreateAsyncQueue()
		{
			AsyncQueue queue = RecyclableObjectPool.Get<AsyncQueue>();
			queueCollection.Add(queue);
			return queue;
		}
		public static void ReleaseAsyncQueue(AsyncQueue queue)
		{
			RecyclableObjectPool.Release(queue);
			queueCollection.Remove(queue);
		}
		void Update()
		{
			if(queueCollection==null||queueCollection.Count==0) return;
			if(!autoStart && !autoRelease) return;
			for (int i = 0; i < queueCollection.Count; i++)
			{
				switch (queueCollection[i].Status)
				{
					case eAsyncQueueState.Idle:
					if(autoStart)
					{
						queueCollection[i].Start();
					}
					break;
					case eAsyncQueueState.Fininshed:
					if(autoRelease)
					{
						ReleaseAsyncQueue(queueCollection[i]);	
					}
					break;
					default:
					break;
				}
			}

			
		}
	}
}
