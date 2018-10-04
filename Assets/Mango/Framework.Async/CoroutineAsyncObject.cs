using System;
using System.Collections;
using System.Collections.Generic;
using Mango.Framework.Core;
using UnityEngine;

namespace Mango.Framework.Async
{
    public class CoroutineAsyncObject : AsyncObject,IRecyclableObject
    {
        private UnityEngine.Coroutine co;
        public static string CLASS_KEY = "CoroutineAsyncObject";
        public override string ClassKey{get{return CLASS_KEY;}}
        public override void OnUse()
        {
            throw new NotImplementedException();
        }
        public CoroutineAsyncObject SetCoroutine(UnityEngine.Coroutine co)
        {
            this.co = co;
            return this;
        }
        public override void OnRelease()
        {
            co = null;
            Next = null;
            onComplete = null;
        }
        protected override IEnumerator WaitNext()
        {
            yield return co;
            Complete();
        }
    }
}

