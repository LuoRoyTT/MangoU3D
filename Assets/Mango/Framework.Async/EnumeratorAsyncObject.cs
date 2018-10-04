using System.Collections;
using System.Collections.Generic;
using Mango.Framework.Core;
using UnityEngine;

namespace Mango.Framework.Async
{
    public class EnumeratorAsyncObject : AsyncObject,IRecyclableObject
    {
        private IEnumerator enumerator;
        public static string CLASS_KEY = "EnumeratorAsyncObject";
        public override string ClassKey{get{return CLASS_KEY;}}
        public override void OnUse()
        {

        }
        public EnumeratorAsyncObject SetEnumerator(IEnumerator enumerator)
        {
            this.enumerator = enumerator;
            return this;
        }
        public override void OnRelease()
        {
            enumerator = null;
            Next = null;
            onComplete = null;
        }

        protected override IEnumerator WaitNext()
        {
            yield return AsyncCenter.Instance.StartCoroutine(enumerator);
            Complete();
        }
    }
}
