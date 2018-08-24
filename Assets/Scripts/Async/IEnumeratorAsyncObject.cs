using System.Collections;
using System.Collections.Generic;
using Client.Data;
using UnityEngine;

namespace Client.Async
{
    public class IEnumeratorAsyncObject : AsyncObject,IRecyclableObject
    {
        private IEnumerator enumerator;
        public static string CLASS_KEY = "CoroutineAsyncObject";
        public override string ClassKey{get{return CLASS_KEY;}}
        public override void OnUse()
        {

        }
        public IEnumeratorAsyncObject SetEnumerator(IEnumerator enumerator)
        {
            this.enumerator = enumerator;
            return this;
        }
        public override void OnRelease()
        {
            enumerator = null;
            Next = null;
            onCompelete = null;
        }

        protected override IEnumerator WaitNext()
        {
            yield return AsyncCenter.Instance.StartCoroutine(enumerator);
            Compelete();
        }
    }
}
