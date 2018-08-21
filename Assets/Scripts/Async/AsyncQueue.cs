using System.Collections;
using System.Collections.Generic;
using Client.Data;
using UnityEngine;


namespace Async
{
    public class AsyncQueue : RecyclableObject
    {
		public static string CLASS_KEY = "AsyncQueue";
        public override string ClassKey{get{return CLASS_KEY;}}
		private Queue<IAsyncObject> cacheCallback=new Queue<IAsyncObject>();
    }
}

