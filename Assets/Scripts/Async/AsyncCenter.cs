using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Client.Core;
using UnityEngine;

namespace Client.Async
{
	public class AsyncCenter : MonoSingleton<AsyncCenter>  
	{
		public void GotoNext(IEnumerator ie,Action onCompelete)
		{
			StartCoroutine(WaitCompelete(ie,onCompelete));
		}
		public void GotoNext(AsyncThreadDelegateFull threadAction,Action onCompelete,object param=null)
		{
			StartCoroutine(WaitCompelete(threadAction,onCompelete,param));
		}
		private IEnumerator WaitCompelete(IEnumerator ie,Action onCompelete)
		{
			yield return StartCoroutine(ie);
			if(onCompelete!=null)
			{
				onCompelete();
			}
		}
		private IEnumerator WaitCompelete(AsyncThreadDelegateFull threadAction,Action onCompelete,object param=null)
		{
            bool waitThreadFinish = false;

            var thread = new Thread(() =>
            {
                Action customNext = () => { waitThreadFinish = true; };
                threadAction(param,customNext);
            });

            thread.Start();
            while (!waitThreadFinish)
                yield return null;
			if(onCompelete!=null)
			{
				onCompelete();
			}
		}
	}
}
