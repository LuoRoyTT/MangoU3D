using System;
using System.Collections;
using System.Collections.Generic;
using Client.Data;
using UnityEngine;


namespace Client.Async
{
    public class AsyncQueue : RecyclableObject
    {
		public static string CLASS_KEY = "AsyncQueue";
        private static int APPEND_COUNT = 20;
        private static int INSERT_COUNT = 20;
        private static int INSERTCALLBACK_COUNT = 20;
        public override string ClassKey{get{return CLASS_KEY;}}
		private Queue<IAsyncObject> appendCalls;
        private AsyncCallback appendCallback;
        private List<IAsyncObject> insertCalls;
        private List<AsyncCallback> insertCallback;
        public AsyncQueue()
        {
            appendCalls = new Queue<IAsyncObject>(APPEND_COUNT);
            insertCalls = new List<IAsyncObject>(INSERT_COUNT);
            insertCallback = new List<AsyncCallback>(INSERTCALLBACK_COUNT);
        }
        public AsyncQueue Append(IAsyncObject async)
        {
            return this;
        }
        public AsyncQueue AppendCallback(Callback action)
        {
            appendCallback.AddListener(action);
            return this;
        }
        public AsyncQueue Insert(float timePosition,IAsyncObject async)
        {
            return this;
        }
        public AsyncQueue InsertCallback(float timePosition, Callback action)
        {
            insertCallback.Add(new AsyncCallback(timePosition,action));
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
    }
}

