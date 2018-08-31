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
		private int index;
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
			index = 0;
			while (waitFuncRecCalls!=null && waitFuncRecCalls.Count!=0)
			{
				IFuncRec func = waitFuncRecCalls[index];
				if(func.CountDown>0) 
				{
					func.CountDown-=Time.deltaTime;
					index++;
					continue;
				}
				waitFuncRecCalls.RemoveAt(index);
				if(!func.Script||!func.Script.enabled) continue;
				func.Dispose();
				frameInterval += func.ElapsedTime;
				if(func.ElapsedTime>frameFixedInterval) break;
			}
			frameInterval = 0f;
		}
	}
}

