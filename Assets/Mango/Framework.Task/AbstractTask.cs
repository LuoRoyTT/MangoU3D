using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mango.Framework.Task
{
	public abstract class AbstractTask
	{
		protected MangoObject mango; 
		public eTaskStatus status;
		public void SetTaskInitiator(MangoObject mango)
		{
			this.mango = mango;
		}
		public void Start()
		{
			if(mango == null)
			{
				status = eTaskStatus.Invalid;
				Debug.LogError("Mango不能为空！！！");
			}
			if(status != eTaskStatus.WillDo) 
			{
				return;
			}

			OnStart();
		}
		public void Update()
		{
			OnUpdate();
		}
		protected virtual void OnStart(){}
		protected virtual void OnUpdate(){}
		public AbstractTask OnComplete(Action action)
		{
			if(action!=null)
			{
				onComplete += action;
			}
			return this;
		}
		public Action onComplete;
	}
}
