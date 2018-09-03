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
		private List<IFuncRec> updateFuncRecCalls = new List<IFuncRec>();
		private int index;
		public void AddFuncRec(IFuncRec func)
		{
			int findIndex = waitFuncRecCalls.FindIndex((a)=>{return a.Equals(func);});
			if(findIndex!=-1)
			{
				waitFuncRecCalls.RemoveAt(findIndex);
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
		public void AddUpdateFuncRec(IFuncRec func)
		{
			int findIndex = updateFuncRecCalls.FindIndex((a)=>{return a.Equals(func);});
			if(findIndex!=-1)
			{
				updateFuncRecCalls.RemoveAt(findIndex);
			}
			updateFuncRecCalls.Add(func);
		}

		public void RemoveUpdateCall(IFuncRec func)
		{
			updateFuncRecCalls.Remove(func);
		}
		public void RemoveAllUpdateCall()
		{
			updateFuncRecCalls.Clear();
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

