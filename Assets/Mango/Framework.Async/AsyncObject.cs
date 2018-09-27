using System;
using System.Collections;
using System.Collections.Generic;
using Client.Data;
using UnityEngine;

namespace Mango.Framework.Async
{
	public abstract class AsyncObject:IRecyclableObject 
	{
		private AsyncQueue queue = null;
		protected Action onStart;
		protected Action onComplete;
		public AsyncObject Next{get;protected set;}

        public abstract string ClassKey {get;}

		public void SetQueue(AsyncQueue queue)
		{
			this.queue = queue;
		}

        public AsyncObject SetNext(AsyncObject next)
		{
			if(next.Equals(this))
			{
				Debug.LogError("AsyncObject错误赋值，导致对象的next指向自身！");
			}
			this.Next = next;
			return this;
		}
		public AsyncObject SetOnStart(Action onStart)
		{
			this.onStart = onStart;
			return this;
		}
		public AsyncObject SetOnComplete(Action onComplete)
		{
			this.onComplete = onComplete;
			return this;
		}
		public void Start()
		{
			if(onStart!=null)
			{
				onStart();
			}
			AsyncCenter.Instance.StartCoroutine(WaitNext());
		}
		protected abstract IEnumerator WaitNext();
		protected void Complete()
		{
			if(onComplete!=null)
            {
                onComplete();
            }
            if(Next!=null)
            {
                Next.Start();
            }
			else
			{
				queue.Complete();
			}
		}

        public abstract void OnUse();

        public abstract void OnRelease();
    } 
}

