using System.Collections;
using System.Collections.Generic;
using Client.Data;
using UnityEngine;

namespace Client.Async
{
    public class FrameAsyncObject : AsyncObject,IRecyclableObject
    {
        private int frameCount = 0;
        public static string CLASS_KEY = "CoroutineAsyncObject";
        public override string ClassKey{get{return CLASS_KEY;}}
        public override void OnUse()
        {

        }
        public FrameAsyncObject SetInterval(int frameCount)
        {
            this.frameCount = frameCount;
            return this;
        }
        public override void OnRelease()
        {
            frameCount = 0;
            Next = null;
            onCompelete = null;
        }
        protected override IEnumerator WaitNext()
        {
            yield return AsyncCenter.Instance.StartCoroutine(Timer());
            Compelete();
        }
        private IEnumerator Timer()
        {
            int frameStamp = 0;
            while (frameStamp<frameCount)
            {
                frameCount++;
                yield return null;
            }
            
        }
    }
}
