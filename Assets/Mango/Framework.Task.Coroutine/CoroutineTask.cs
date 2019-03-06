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
			status = eTaskStatus.WillDo;
		}

        protected override void OnStart()
        {
			status = eTaskStatus.Doing;
			it.MoveNext();
			current = it.Current;
        }

        protected override void OnUpdate()
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
				switch (currentTask.status)
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
				status = eTaskStatus.Done;
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

