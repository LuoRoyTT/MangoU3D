using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client.FuncAllocator
{
	public interface IFuncRec
	{
		float Interval{get;}
		MonoBehaviour script{get;}
		void Dispose();
	}

    public class FuncRec : IFuncRec
    {
        public float Interval { get; private set; }
		public MonoBehaviour script{get;private set;}
		Action action;

        public void Dispose()
        {
            float startTimeStamp = Time.realtimeSinceStartup;
			action();
			Interval = Time.realtimeSinceStartup - startTimeStamp;
        }
		public FuncRec(Action action)
		{
			this.action = action;
		}
    }

	public class FuncRec<T> : IFuncRec
    {
        public float Interval { get; private set; }
		public MonoBehaviour script{get;private set;}
		Action<T> action;
        T param1;
        public void Dispose()
        {
            float startTimeStamp = Time.realtimeSinceStartup;
			action(param1);
			Interval = Time.realtimeSinceStartup - startTimeStamp;
        }
		public FuncRec(Action<T> action,T param1)
		{
			this.action = action;
			this.param1 = param1;
		}
    }
	public class FuncRec<T1,T2> : IFuncRec
    {
        public float Interval { get; private set; }
		public MonoBehaviour script{get;private set;}
		Action<T1,T2> action;
        T1 param1;
		T2 param2;
        public void Dispose()
        {
            float startTimeStamp = Time.realtimeSinceStartup;
			action(param1,param2);
			Interval = Time.realtimeSinceStartup - startTimeStamp;
        }
		public FuncRec(Action<T1,T2> action,T1 param1,T2 param2)
		{
			this.action = action;
			this.param1 = param1;
			this.param2 = param2;
		}
    }
}

