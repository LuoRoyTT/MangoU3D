using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mango.Framework.Task
{
	public interface ITask
	{
		eTaskStatus Status{get;}
		IProcessTask Script{get;}
		void Start();
		void Update();
		event Action onComplete;
	}
}
