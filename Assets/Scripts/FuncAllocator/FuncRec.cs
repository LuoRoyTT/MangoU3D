using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client.FuncAllocator
{
	public interface IFuncRec
	{
		float ElapsedTime{get;}
		float CountDown{get;set;}
		MonoBehaviour Script{get;}
		void Dispose();
	}

    public struct FuncRec : IFuncRec
    {
		public MonoBehaviour Script{get;private set;}
        public float ElapsedTime { get; private set;}
		public float CountDown{get; set;}
		private float interval;
		Action action;

        public void Dispose()
        {
            float startTimeStamp = Time.realtimeSinceStartup;
			action();
			ElapsedTime = Time.realtimeSinceStartup - startTimeStamp;
        }
		public FuncRec(MonoBehaviour script,Action action,float interval=0f)
		{
			this.Script = script;
			this.action = action;
			this.ElapsedTime = 0f;
			this.CountDown = interval;
			this.interval = interval;

		}
    }

	public struct FuncRec<T> : IFuncRec
    {
		public MonoBehaviour Script{get;private set;}
        public float ElapsedTime { get; private set;}
		public float CountDown{get; set;}
		private float interval;
		Action<T> action;
        T param1;
        public void Dispose()
        {
            float startTimeStamp = Time.realtimeSinceStartup;
			action(param1);
			ElapsedTime = Time.realtimeSinceStartup - startTimeStamp;
        }
		public FuncRec(MonoBehaviour script,Action<T> action,T param1,float interval=0f)
		{
			this.Script = script;
			this.action = action;
			this.param1 = param1;
			this.ElapsedTime = 0f;
			this.CountDown = interval;
			this.interval = interval;
		}
    }
	public struct FuncRec<T1,T2> : IFuncRec
    {
		public MonoBehaviour Script{get;private set;}
        public float ElapsedTime { get; private set; }
		public float CountDown{get; set;}
		private float interval;
		Action<T1,T2> action;
        T1 param1;
		T2 param2;
        public void Dispose()
        {
            float startTimeStamp = Time.realtimeSinceStartup;
			action(param1,param2);
			ElapsedTime = Time.realtimeSinceStartup - startTimeStamp;
        }
		public FuncRec(MonoBehaviour script,Action<T1,T2> action,T1 param1,T2 param2,float interval=0f)
		{
			this.action = action;

			this.Script = script;
			this.param1 = param1;
			this.param2 = param2;
			this.ElapsedTime = 0f;
			this.CountDown = interval;
			this.interval = interval;
		}
    }
}

