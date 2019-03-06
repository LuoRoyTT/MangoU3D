using System;
using System.Collections;
using System.Collections.Generic;
using Mango.Framework.Core;
using UnityEngine;

namespace Mango.Framework.Task
{
	public sealed class ActionTask : AbstractTask 
	{	
        public ActionTask(Action action)
        {
			status = eTaskStatus.WillDo;
            this.action = action;
        }
        private Action action;

        protected override void OnStart()
        {
            if(action != null)
            {
                action();
            }
            status = eTaskStatus.Done;
            onComplete();
        }
    }

    public sealed class ActionTask<T> : AbstractTask 
	{	
        public ActionTask(Action<T> action,T param)
        {
			status = eTaskStatus.WillDo;
            this.action = action;
            this.param = param;
        }
        private Action<T> action;
        private T param;

        protected override void OnStart()
        {
            if(action != null)
            {
                action(param);
            }
            status = eTaskStatus.Done;
            onComplete();
        }

    }
    public sealed class ActionTask<T1,T2> : AbstractTask 
	{	
        public ActionTask(Action<T1,T2> action,T1 param1,T2 param2)
        {
			status = eTaskStatus.WillDo;
            this.action = action;
            this.param1 = param1;
            this.param2 = param2;
        }
        private Action<T1,T2> action;
        private T1 param1;
        private T2 param2;

        protected override void OnStart()
        {
            if(action != null)
            {
                action(param1,param2);
            }
            status = eTaskStatus.Done;
            onComplete();
        }
    }
}