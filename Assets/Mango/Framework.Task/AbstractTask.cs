using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mango.Framework.Task
{
	public abstract class AbstractTask
	{
		public eTaskStatus status;
		public virtual void Start(){}
		public virtual void Update(){}
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
