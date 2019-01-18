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
			Status = eTaskStatus.WillDo;
            this.action = action;
        }
        private Action action;

        public eTaskStatus Status{get;private set;}

        public override void Start()
        {
            if(action != null)
            {
                action();
            }
            Status = eTaskStatus.Done;
            onComplete();
        }
    }

    public sealed class ActionTask<T> : AbstractTask 
	{	
        public ActionTask(Action<T> action,T param)
        {
			Status = eTaskStatus.WillDo;
            this.action = action;
            this.param = param;
        }
        private Action<T> action;
        private T param;

        public eTaskStatus Status{get;private set;}

        public override void Start()
        {
            if(action != null)
            {
                action(param);
            }
            Status = eTaskStatus.Done;
            onComplete();
        }

    }
    public sealed class ActionTask<T1,T2> : AbstractTask 
	{	
        public ActionTask(Action<T1,T2> action,T1 param1,T2 param2)
        {
			Status = eTaskStatus.WillDo;
            this.action = action;
            this.param1 = param1;
            this.param2 = param2;
        }
        private Action<T1,T2> action;
        private T1 param1;
        private T2 param2;

        public eTaskStatus Status{get;private set;}

        public override void Start()
        {
            if(action != null)
            {
                action(param1,param2);
            }
            Status = eTaskStatus.Done;
            onComplete();
        }
    }
}