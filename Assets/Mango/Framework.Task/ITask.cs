using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mango.Framework.Task
{
	public interface ITask
	{
		eTaskStatus status{get;set;}
		void Start();
		void Update();
		void Complete();

	}
}
