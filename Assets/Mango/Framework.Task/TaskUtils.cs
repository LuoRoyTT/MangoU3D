using System.Collections;
using System.Collections.Generic;
using Mango.Coroutine;
using Mango.Framework.Core;
using UnityEngine;

namespace Mango.Framework.Task
{
	public static class TaskUtils 
	{
		public static void AppendTask(this IProcessTask script, ITask task)
		{
			TaskModule.instance.AppendTask(task);
		}

		public static void RemoveTask(this IProcessTask script, ITask task)
		{
			TaskModule.instance.RemoveTask(task);
		}

		public static void RemoveAllTasks(this IProcessTask script)
		{
			TaskModule.instance.RemoveAllTasks(script);
		}
		public static void RecycleTask(this ITask task)
		{
			RecyclableObjectPool.Release(task as IRecyclableObject);
		}
		public static CoroutineTask StartCoroutineTask(this IProcessTask script,IEnumerator it)
		{
			CoroutineTask co = CoroutineTask.CreateTask(script,it);
			script.AppendTask(co);
			return co;
		}
		public static void StopCoroutineTask(this IProcessTask script,CoroutineTask co)
		{
			script.RemoveTask(co);
		}
	}
}

