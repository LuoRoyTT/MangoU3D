using System;
using System.Collections;
using System.Collections.Generic;
using Mango.Framework.Core;
using Mango.Framework.Task;
using UnityEngine;

namespace Mango.Coroutine
{
    public class CoroutineTask : ITask, IRecyclableObject
    {
		public static string CLASS_KEY="CoroutineTask";
        public string ClassKey {get{return CLASS_KEY;}}
		private IEnumerator it;
		private object current;
        private DateTime waitTillTime;

        public static CoroutineTask CreateTask(IProcessTask script,IEnumerator it)
		{
			CoroutineTask task = RecyclableObjectPool.Get<CoroutineTask>();
			task.Init(script,it);
			return task;
		}
		private void Init(IProcessTask script, IEnumerator it)
		{
			this.Script = script;
			this.it = it;
		}
        public eTaskStatus Status{ get; private set;}

        public IProcessTask Script { get; private set; }

        public event Action onComplete;

        public void OnUse()
        {
            Status = eTaskStatus.WillDo;
        }

        public void Start()
        {
			Status = eTaskStatus.Doing;
			it.MoveNext();
			current = it.Current;
        }

        public void Update()
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
				this.RecycleTask();
			}
			else
			{
				current = it.Current;
			}
        }

        public void OnRelease()
        {
            it = null;
			current = null;
			Script = null;
			onComplete = null;
			Status = eTaskStatus.Release;
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

