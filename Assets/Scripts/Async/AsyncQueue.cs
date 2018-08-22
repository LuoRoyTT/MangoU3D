using System;
using System.Collections;
using System.Collections.Generic;
using Client.Data;
using UnityEngine;


namespace Client.Async
{
    public class AsyncQueue : IRecyclableObject
    {
		public static string CLASS_KEY = "AsyncQueue";
        private static int APPEND_COUNT = 20;
        public string ClassKey{get{return CLASS_KEY;}}
		private IAsyncObject header;
        private AsyncCallback appendCallback;
        public AsyncQueue()
        {
            
        }
        public AsyncQueue Append(IAsyncObject async)
        {
            if(header==null)
            {
                header = async;
            }
            else
            {
                header.Next = async;
            }
            return this;
        }
        public AsyncQueue AppendCallback(Callback action)
        {
            appendCallback.AddListener(action);
            return this;
        }
        public AsyncQueue Prepend(IAsyncObject async)
        {
            return this;
        }
        public AsyncQueue PrependCallback(Action callback)
        {
            return this;
        }
        public AsyncQueue PrependInterval(float interval)
        {
            return this;
        }

        public void OnUse()
        {
            throw new NotImplementedException();
        }

        public void OnRelease()
        {
            throw new NotImplementedException();
        }
    }
}

