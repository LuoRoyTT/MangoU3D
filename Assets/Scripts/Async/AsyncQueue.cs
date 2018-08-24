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
        public string ClassKey{get{return CLASS_KEY;}}
		private AsyncObject header;
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

        public void OnUse()
        {
            throw new NotImplementedException();
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
        }
    }
}

