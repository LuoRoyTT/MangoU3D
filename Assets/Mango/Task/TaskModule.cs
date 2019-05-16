using System;
using System.Collections;
using System.Collections.Generic;
using Mango.Core;
using Mango.Update;
using UnityEngine;

namespace Mango.Task
{
	public class TaskModule : GameModule
	{
		LinkedList<AbstractTask> execlusiveTaskList = new LinkedList<AbstractTask>();
        private DateTime lastFrameTime;
        private double standardDeltaTime;

        protected override void OnInit()
        {
            Mango.GetModule<UpdateModule>().onUpdate.AddListener(Update);
        }

        protected override void OnRelease()
        {
            Clear();
        }

        void Update()
		{
			this.lastFrameTime = DateTime.Now;
			while ((DateTime.Now - this.lastFrameTime).TotalMilliseconds < standardDeltaTime && this.execlusiveTaskList.Count > 0)
			{
				LinkedListNode<AbstractTask> last = execlusiveTaskList.Last;
                AbstractTask task = last.Value;
				task.onComplete += new Action(()=>{execlusiveTaskList.Remove(last);});
                switch (task.status)
                {
					case eTaskStatus.Invalid:
						execlusiveTaskList.Remove(last);
						break;
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
        public AbstractTask AppendTask(AbstractTask task)
        {
			if(task!=null) 
            {
                execlusiveTaskList.AddLast(task);
            }
			return task;
        }
        public void RemoveTask(AbstractTask task)
        {
			if(task==null) return;
			LinkedListNode<AbstractTask> tmp = execlusiveTaskList.Last;
			AbstractTask nodeValue = tmp.Value;
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

		public void Clear()
		{
			if(execlusiveTaskList.Count > 0)
				execlusiveTaskList.Clear();
		}
	}
}

