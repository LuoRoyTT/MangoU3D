using System;
using System.Collections;
using System.Collections.Generic;
using Mango.Framework.Core;
using Mango.Framework.Task;
using UnityEngine;

namespace Mango.Framework.Task.Coroutine
{
    public static class CoroutineUtil
    {
        public static CoroutineTask StartCoroutine(this TaskModule module,IEnumerator it,Action onComplete = null)
        {
            CoroutineTask task = new CoroutineTask(it);
            module.AppendTask(task).OnComplete(onComplete);
            return task;
        }

        public static void StopCoroutine(this TaskModule module,CoroutineTask task)
        {
            module.RemoveTask(task);
        }
    }
}

