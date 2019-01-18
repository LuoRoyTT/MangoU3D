using System;
using System.Collections;
using System.Collections.Generic;
using Mango.Framework.Core;
using UnityEngine;

namespace Mango.Framework.Task
{
	public sealed class TaskGroup : AbstractTask 
	{	
        public TaskGroup()
        {
			Status = eTaskStatus.WillDo;
        }

        private Queue<AbstractTask> subTasks = new Queue<AbstractTask>();
		private AbstractTask current;
        public int Count{get{return subTasks.Count;}}
        public eTaskStatus Status{get;private set;}

        public override void Start()
        {
            if (subTasks==null || subTasks.Count==0) 
			{
				SubTaskCompleteCallback();
				return;
			}
			Status = eTaskStatus.Doing;
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
				Status = eTaskStatus.Done;
				onComplete();
			}
			else StartSubTask();
		}
        public override void Update()
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

