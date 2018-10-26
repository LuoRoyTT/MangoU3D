using System;
using System.Collections;
using System.Collections.Generic;
using Mango.Framework.Core;
using UnityEngine;

namespace Mango.Framework.Task
{
	public sealed class TaskGroup : ITask ,IRecyclableObject
	{
		public static string CLASS_KEY="TaskGroup";
        public string ClassKey {get{return CLASS_KEY;}}
		public static TaskGroup CreateTaskGroup(IProcessTask Script)
		{
			TaskGroup taskGroup = RecyclableObjectPool.Get<TaskGroup>();
			taskGroup.Init(Script);
			return taskGroup;
		}

        private void Init(IProcessTask Script)
        {
            this.Script = Script; 
        }

        private Queue<ITask> subTasks = new Queue<ITask>();
		private ITask current;

        public event Action onComplete;

        public int Count{get{return subTasks.Count;}}
        public eTaskStatus Status{get;private set;}

        public IProcessTask Script { get; private set; }

        public void Start()
        {
            if (subTasks==null || subTasks.Count==0) return;
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
        public void Update()
        {
            if (current!=null)
			{
				current.Update();
			}
        }

        public TaskGroup AddSubTask(ITask subTask)
		{
			if (subTask!=null)
			{
				subTasks.Enqueue(subTask);
			}
			return this;
		}
		public TaskGroup OnComplete(Action action)
		{
			onComplete += action;
			return this;
		}

        public void OnUse()
        {
            Status = eTaskStatus.WillDo;
        }

        public void OnRelease()
        {
			current = null;
            subTasks.Clear();
			Script = null;
			onComplete = null;
			Status = eTaskStatus.Release;
        }
    }
}

