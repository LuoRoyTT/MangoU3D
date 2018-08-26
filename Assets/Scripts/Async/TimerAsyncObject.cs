using System.Collections;
using System.Collections.Generic;
using Client.Data;
using UnityEngine;

namespace Client.Async
{
    public class TimerAsyncObject : AsyncObject,IRecyclableObject
    {
        private float interval = 0f;
        public static string CLASS_KEY = "TimerAsyncObject";
        public override string ClassKey{get{return CLASS_KEY;}}
        public override void OnUse()
        {

        }
        public TimerAsyncObject SetInterval(float interval)
        {
            this.interval = interval;
            return this;
        }
        public override void OnRelease()
        {
            interval = 0f;
            Next = null;
            onComplete = null;
        }
        protected override IEnumerator WaitNext()
        {
            yield return AsyncCenter.Instance.StartCoroutine(Timer());
            Complete();
        }
        private IEnumerator Timer()
        {
            yield return new WaitForSeconds(interval);
        }
    }
}
