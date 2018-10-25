using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mango.Framework.Task
{
	public interface ITaskGroup : ITask 
	{
		ITaskGroup AddSubTask(ITask subTask);

	}
}

