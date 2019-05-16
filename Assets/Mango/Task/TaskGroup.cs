using System;
using System.Collections;
using System.Collections.Generic;
using Mango.Core;
using UnityEngine;

namespace Mango.Task
{
	public sealed class TaskGroup : AbstractTask 
	{	
        public TaskGroup()
        {
			status = eTaskStatus.WillDo;
        }

        private Queue<AbstractTask> subTasks = new Queue<AbstractTask>();
		private AbstractTask current;
        public int Count{get{return subTasks.Count;}}

        protected override void OnStart()
        {
            if (subTasks==null || subTasks.Count==0) 
			{
				SubTaskCompleteCallback();
				return;
			}
			status = eTaskStatus.Doing;
			StartSubTask();
        }
		private void StartSubTask()
		{
			current = subTasks.Dequeue();
			current.onComplete += SubTaskCompleteCallback;
			current.Start();
		}
		private void SubTaskCompleteCallback()
		{
			if (subTasks.Count==0) 
			{
				status = eTaskStatus.Done;
				onComplete();
			}
			else StartSubTask();
		}
        protected override void OnUpdate()
        {
            if (current!=null)
			{
				current.Update();
			}
        }

        public TaskGroup AddSubTask(AbstractTask subTask)
		{
			if (subTask!=null)
			{
				subTasks.Enqueue(subTask);
			}
			return this;
		}
    }
}

