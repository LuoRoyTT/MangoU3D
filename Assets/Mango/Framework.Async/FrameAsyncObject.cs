using System.Collections;
using System.Collections.Generic;
using Client.Data;
using UnityEngine;

namespace Mango.Framework.Async
{
    public class FrameAsyncObject : AsyncObject,IRecyclableObject
    {
        private int frameCount = 0;
        public static string CLASS_KEY = "FrameAsyncObject";
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
            onComplete = null;
        }
        protected override IEnumerator WaitNext()
        {
            yield return AsyncCenter.Instance.StartCoroutine(Timer());
            Complete();
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
