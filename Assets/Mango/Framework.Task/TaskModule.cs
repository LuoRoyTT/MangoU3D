using System.Collections;
using System.Collections.Generic;
using Mango.Framework.Core;
using UnityEngine;

namespace Mango.Framework.Task
{
	public class TaskModule :Singleton<TaskModule>,IGameModule
	{
		LinkedList<KeyValuePair<int,ITask>> execlusiveTaskList = new LinkedList<KeyValuePair<int, ITask>>();

        public int Priority => throw new System.NotImplementedException();

        public void Init()
        {
            throw new System.NotImplementedException();
        }

        public void Release()
        {
            throw new System.NotImplementedException();
        }

        void Update()
		{

		}
	}
}

