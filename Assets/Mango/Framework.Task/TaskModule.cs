using System;
using System.Collections;
using System.Collections.Generic;
using Mango.Framework.Core;
using UnityEngine;

namespace Mango.Framework.Task
{
	public class TaskModule :Singleton<TaskModule>,IGameModule
	{
		LinkedList<ITask> execlusiveTaskList = new LinkedList<ITask>();
        private DateTime lastFrameTime;
        private double standardDeltaTime;

        public int Priority => throw new System.NotImplementedException();

        public void Init()
        {
            throw new System.NotImplementedException();
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
                switch (task.status)
                {
                    case eTaskStatus.WillDo:
                        task.Start();
                        break;
                    case eTaskStatus.Doing:
                        task.Update();
                        break;
                    case eTaskStatus.Done:
                        task.Complete();
                        execlusiveTaskList.Remove(last);
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
        public void RemoveTask(int hashId,ITask task)
        {
			if(task==null) return;
			LinkedListNode<ITask> tmp = execlusiveTaskList.Last;
			ITask nodeValue = tmp.Value;
			bool flag = false;
			while (tmp!=null)
			{
				if(nodeValue.ScriptId == hashId && nodeValue == task)
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
        public void RemoveAllTasks(int hashId)
        {
			List<ITask> waitRemoveList = null;
			LinkedList<ITask>.Enumerator enumerator = execlusiveTaskList.GetEnumerator();
			while (enumerator.MoveNext())
			{
				ITask current = enumerator.Current;
				if (current.ScriptId == hashId)
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

