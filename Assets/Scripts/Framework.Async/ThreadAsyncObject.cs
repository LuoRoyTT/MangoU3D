using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Client.Data;
using UnityEngine;

namespace Client.Async
{
    public delegate void AsyncThreadDelegateFull(object param, Action next);
    public class ThreadAsyncObject : AsyncObject,IRecyclableObject
    {
        public static string CLASS_KEY = "ThreadAsyncObject";
        public override string ClassKey{get{return CLASS_KEY;}}
        private AsyncThreadDelegateFull threadAction;
        private object param;

        public override void OnUse()
        {

        }

        public override void OnRelease()
        {
            threadAction = null;
            param = null;
            Next = null;
            onComplete = null;
        }

        protected override IEnumerator WaitNext()
        {
            yield return AsyncCenter.Instance.StartCoroutine(_Thread(threadAction,param));
            Complete();
        }
        private IEnumerator _Thread(AsyncThreadDelegateFull threadCalAction, object param = null)
        {
            bool waitThreadFinish = false;

            var thread = new Thread(() =>
            {
                Action customNext = () => { waitThreadFinish = true; };
                threadCalAction(param, customNext);
            });

            thread.Start();

            while (!waitThreadFinish)
                yield return null;
        }
    }
}
