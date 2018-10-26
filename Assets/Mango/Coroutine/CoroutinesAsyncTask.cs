using System;
using System.Collections;
using System.Collections.Generic;
using Mango.Framework.Core;
using Mango.Framework.Task;
using UnityEngine;

namespace Mango.Coroutine
{
	
	public class CoroutinesAsyncTask : ITask, IRecyclableObject
	{
 		public static string CLASS_KEY = "CoroutinesAsyncTask" ;
        public string ClassKey {get{return CLASS_KEY;}}
		public static CoroutinesAsyncTask Create(IProcessTask script,params IEnumerator[] iters)
		{
			CoroutinesAsyncTask asyncTask = RecyclableObjectPool.Get<CoroutinesAsyncTask>();
			List<CoroutineTask> coroutines = new List<CoroutineTask>();
			for (int i = 0; i < iters.Length; i++)
			{
				CoroutineTask co = CoroutineTask.CreateTask(script,iters[i]);
				coroutines.Add(co);
			}
			asyncTask.Init(script,coroutines);
			return asyncTask;
		}
		public static CoroutinesAsyncTask Create(IProcessTask script,params CoroutineTask[] coroutineArray)
		{
			CoroutinesAsyncTask asyncTask = RecyclableObjectPool.Get<CoroutinesAsyncTask>();
			List<CoroutineTask> coroutines = new List<CoroutineTask>();
			coroutines.AddRange(coroutineArray);
			asyncTask.Init(script,coroutines);
			return asyncTask;
		}
        private void Init(IProcessTask script,List<CoroutineTask> coroutines)
        {
            this.Script = script; 
			this.coroutines = coroutines;
        }

        private List<CoroutineTask> coroutines = new List<CoroutineTask>();

        public event Action onComplete;

        public int Count{get{return coroutines.Count;}}
		private int doneCount;
		public int DoneCount
		{
			get
			{
				return doneCount;
			}
		}
        public eTaskStatus Status{get;private set;}

        public IProcessTask Script { get; private set; }

        public void Start()
        {
            if (coroutines==null || coroutines.Count==0) return;
			foreach (var coroutine in coroutines)
			{
				coroutine.onComplete += CoroutineCallback;
				coroutine.Start();
			} 
        }
		private void CoroutineCallback()
		{
			doneCount++;
			if(doneCount>=Count)
			{
				onComplete();
				this.RecycleTask();
			}
		}

        public void Update()
        {
			for (int i = 0; i < coroutines.Count; i++)
			{
				CoroutineTask co = coroutines[i];
				if (co.Status == eTaskStatus.Doing)
				{
					co.Update();
				}
			}
        }

		public CoroutinesAsyncTask OnComplete(Action action)
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
			for (int i = 0; i < coroutines.Count; i++)
			{
				coroutines[i].RecycleTask();
			}
			doneCount = 0;
            coroutines.Clear();
			Script = null;
			onComplete = null;
			Status = eTaskStatus.Release;
        }
    }
}
