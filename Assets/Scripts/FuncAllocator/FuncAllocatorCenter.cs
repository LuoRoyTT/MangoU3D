using System.Collections;
using System.Collections.Generic;
using Client.Core;
using UnityEngine;

namespace Client.FuncAllocator
{
	public class FuncAllocatorCenter :  MonoSingleton<FuncAllocatorCenter>  
	{
		private static float frameFixedInterval = 0.02f;
		private float frameInterval = 0f;
		private List<IFuncRec> waitFuncRecCalls = new List<IFuncRec>();
		public void AddFuncRec(IFuncRec func)
		{
			int index = waitFuncRecCalls.FindIndex((a)=>{return a.Equals(func);});
			if(index!=-1)
			{
				waitFuncRecCalls.RemoveAt(index);
			}
			waitFuncRecCalls.Add(func);
		}
		public void Remove(IFuncRec func)
		{
			waitFuncRecCalls.Remove(func);
		}
		public void RemoveAll()
		{
			waitFuncRecCalls.Clear();
		}
		void Update()
		{
			if(waitFuncRecCalls==null || waitFuncRecCalls.Count==0)
			{
				return;
			}
			while (waitFuncRecCalls!=null && waitFuncRecCalls.Count!=0)
			{
				IFuncRec func = waitFuncRecCalls[0];
				waitFuncRecCalls.RemoveAt(0);
				if(!func.script||!func.script.enabled) continue;
				func.Dispose();
				frameInterval += func.Interval;
				if(func.Interval>frameFixedInterval) break;
			}
			frameInterval = 0f;
		}
	}
}

