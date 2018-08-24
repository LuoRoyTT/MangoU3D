using System;
using System.Collections;
using System.Collections.Generic;
using Client.Async;
using UnityEngine;

namespace Client.Data
{
	public abstract class AsyncObject:IRecyclableObject 
	{
		protected Action onStart;
		protected Action onCompelete;
		public AsyncObject Next{get;protected set;}

        public abstract string ClassKey {get;}

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
		public AsyncObject SetOnCompelete(Action onCompelete)
		{
			this.onCompelete = onCompelete;
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
		protected void Compelete()
		{
			if(onCompelete!=null)
            {
                onCompelete();
            }
            if(Next!=null)
            {
                Next.Start();
            }
		}

        public abstract void OnUse();

        public abstract void OnRelease();
    } 
}

