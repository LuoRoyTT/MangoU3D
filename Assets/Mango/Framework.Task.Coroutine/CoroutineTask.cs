using System;
using System.Collections;
using System.Collections.Generic;
using Mango.Framework.Core;
using Mango.Framework.Task;
using UnityEngine;

namespace Mango.Framework.Task.Coroutine
{
    public class CoroutineTask : AbstractTask
    {
		private IEnumerator it;
		private object current;
        private DateTime waitTillTime;

		public CoroutineTask(IEnumerator it)
		{
			this.it = it;
			Status = eTaskStatus.WillDo;
		}
        public eTaskStatus Status{ get; private set;}

        public override void Start()
        {
			Status = eTaskStatus.Doing;
			it.MoveNext();
			current = it.Current;
        }

        public override void Update()
        {
			if (DateTime.Now <= this.waitTillTime)
			{
				return;
			}
			if(current is MWaitForSeconds)
			{
				ProcessWaitForSeconds(current as MWaitForSeconds);
				return;
			}
			if(current is CoroutineTask)
			{
				CoroutineTask currentTask = current as CoroutineTask;
				switch (currentTask.Status)
                {
                    case eTaskStatus.WillDo:
                        currentTask.Start();
                        return;
                    case eTaskStatus.Doing:
                        currentTask.Update();
                        return;                   
                }
			}
            if (!it.MoveNext())
			{
				Status = eTaskStatus.Done;
				onComplete();
			}
			else
			{
				current = it.Current;
			}
        }
		private void ProcessWaitForSeconds(MWaitForSeconds handle)
        {
			if (handle.interval > 0f)
			{
				waitTillTime = DateTime.Now.AddSeconds((double)handle.interval);
			}
        }
    }
}

