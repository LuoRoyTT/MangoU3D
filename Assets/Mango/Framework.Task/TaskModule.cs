using System;
using System.Collections;
using System.Collections.Generic;
using Mango.Framework.Core;
using Mango.Framework.Update;
using UnityEngine;

namespace Mango.Framework.Task
{
	public class TaskModule :Singleton<TaskModule>,IGameModule
	{
		LinkedList<ITask> execlusiveTaskList = new LinkedList<ITask>();
        private DateTime lastFrameTime;
        private double standardDeltaTime;

        public int Priority
		{
			get
			{
				return 1;
			}
		}

        public void Init()
        {
            UpdateModule.instance.onMainThreadUpdate += Update;
        }

        public void Release()
        {
            Clear();
        }

        void Update()
		{
			this.lastFrameTime = DateTime.Now;
			while ((DateTime.Now - this.lastFrameTime).TotalMilliseconds < standardDeltaTime && this.execlusiveTaskList.Count > 0)
			{
				LinkedListNode<ITask> last = execlusiveTaskList.Last;
                ITask task = last.Value;
				task.onComplete += new Action(()=>{execlusiveTaskList.Remove(last);});
                switch (task.Status)
                {
                    case eTaskStatus.WillDo:
                        task.Start();
                        break;
                    case eTaskStatus.Doing:
                        task.Update();
                        break;                   
                }
                this.lastFrameTime = DateTime.Now;
            }
		}
        public void AppendTask(ITask task)
        {
			if(task!=null) 
            {
                execlusiveTaskList.AddLast(task);
            }
        }
        public void RemoveTask(ITask task)
        {
			if(task==null) return;
			LinkedListNode<ITask> tmp = execlusiveTaskList.Last;
			ITask nodeValue = tmp.Value;
			bool flag = false;
			while (tmp!=null)
			{
				if(nodeValue == task)
				{
					flag = true;
					break;
				}
				tmp = tmp.Previous;
				nodeValue = tmp.Value;
			}
			if(flag)
			{
				execlusiveTaskList.Remove(nodeValue);
			}
        }
        public void RemoveAllTasks(IProcessTask script)
        {
			List<ITask> waitRemoveList = null;
			LinkedList<ITask>.Enumerator enumerator = execlusiveTaskList.GetEnumerator();
			while (enumerator.MoveNext())
			{
				ITask current = enumerator.Current;
				if (current.Script == script)
				{
					waitRemoveList.Add(current);
				}
			}
			if(waitRemoveList!=null)
			{
				for (int i = 0; i < waitRemoveList.Count; i++)
				{
					execlusiveTaskList.Remove(waitRemoveList[i]);
				}
			}
        }
		public void Clear()
		{
			if(execlusiveTaskList.Count > 0)
				execlusiveTaskList.Clear();
		}
	}
}

