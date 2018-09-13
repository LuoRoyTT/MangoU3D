using System;
using System.Collections;
using System.Collections.Generic;
using Client.Data;
using UnityEngine;


namespace Client.Async
{
    public enum eAsyncQueueState
    {
        Null,
        Idle,
        Doing,
        Fininshed

    }
    public class AsyncQueue : IRecyclableObject
    {
		public static string CLASS_KEY = "AsyncQueue";
        public string ClassKey{get{return CLASS_KEY;}}
        public eAsyncQueueState Status{get;private set;}
		private AsyncObject header;
        private Action asyncOnStart;
        private Action asyncOnComplete;
        public AsyncQueue Append(AsyncObject async)
        {
            if(header==null)
            {
                header = async;
            }
            else
            {
                header.SetNext(async);
            }
            return this;
        }
        public void AppendCallback(Action onComplete)
        {
            asyncOnComplete += onComplete;
        }
        public AsyncQueue Prepend(AsyncObject async)
        {
            if(header==null)
            {
                header = async;
            }
            else
            {
                async.SetNext(header);
            }
            return this;
        }
        public void PrependCallback(Action onStart)
        {
            asyncOnStart += onStart;
        }
        public void Start()
        {
            if (asyncOnStart != null)
            {
                asyncOnStart();
            }
            Status = eAsyncQueueState.Doing;
            header.Start();
        }
        public void Complete()
        {
            if(asyncOnComplete != null)
            {
                asyncOnComplete();
            }
            Status = eAsyncQueueState.Fininshed;
        }
        public void OnUse()
        {
            header = null;
            asyncOnStart = null;
            asyncOnComplete = null;
            Status = eAsyncQueueState.Idle;
        }

        public void OnRelease()
        {
            AsyncObject current = header;
            while (current!=null)
            {
                AsyncObject tmp = current;
                current = current.Next;
                RecyclableObjectPool.Release(tmp);
            }
            header = null;
            asyncOnStart = null;
            asyncOnComplete = null;
            Status = eAsyncQueueState .Null;
        }
    }
}

