using System;
using System.Collections;
using Mango.Task;
using Mango.Task.Coroutine;

namespace Mango
{
    public class MangoObject
    {
        protected TaskModule taskModule = Mango.GetModule<TaskModule>();
        public AbstractTask AppendTask(AbstractTask task)
        {
            task.SetTaskInitiator(this);
            return taskModule.AppendTask(task);
        }
        public void RemoveTask(AbstractTask task)
        {
            taskModule.RemoveTask(task);
        }
        public CoroutineTask StartCoroutine(IEnumerator it,Action onComplete = null)
        {
            CoroutineTask task = new CoroutineTask(it);
            return taskModule.AppendTask(task).OnComplete(onComplete) as CoroutineTask;
        }

        public void StopCoroutine(CoroutineTask task)
        {
            taskModule.RemoveTask(task);
        }
        public void StopAllCoroutine()
        {
            
        }
    }
}

