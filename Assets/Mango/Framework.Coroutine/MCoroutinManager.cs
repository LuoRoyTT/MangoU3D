using System;
using System.Collections;
using System.Collections.Generic;
using Mango.Framework.Core;
using Mango.Framework.Frame;
using UnityEngine;


namespace Mango.Framework.Coroutine
{
    public class MCoroutinManager : MonoSingleton<MCoroutinManager>,ILogicUpdate
    {
		private DateTime lastFrameTime;

		private DateTime waitTillTime;

		private LinkedList<KeyValuePair<int,IEnumerator>> execlusiveCoroutineList = new LinkedList<KeyValuePair<int,IEnumerator>>();
		private const double standardDeltaTime = 33.333332061767578;
		protected override void Init()
		{
			this.lastFrameTime = DateTime.Now;
			this.waitTillTime = DateTime.Now;
		}
		public void LogicUpdate()
        {
			if (DateTime.Now <= this.waitTillTime)
			{
				return;
			}
			this.lastFrameTime = DateTime.Now;
			while ((DateTime.Now - this.lastFrameTime).TotalMilliseconds < standardDeltaTime && this.execlusiveCoroutineList.Count > 0)
			{
				LinkedListNode<KeyValuePair<int,IEnumerator>> last = execlusiveCoroutineList.Last;
				IEnumerator iter = last.Value.Value;
				if (!iter.MoveNext())
				{
					this.execlusiveCoroutineList.Remove(last);
				}
				else
				{
					object current = iter.Current;
					if(!(current is MCoroutine))
					{
						if(current is MWaitForSeconds)
						{
							ProcessWaitForSeconds(current as MWaitForSeconds);
						}
						break;
					}
				}
			}
			this.lastFrameTime = DateTime.Now;
        }

        private void ProcessWaitForSeconds(MWaitForSeconds handle)
        {
			if (handle.interval > 0f)
			{
				this.waitTillTime = DateTime.Now.AddSeconds((double)handle.interval);
			}
        }

        public MCoroutine AppendCoroutine(int hashId,IEnumerator it)
        {
			if(it==null) return null;
            MCoroutine coroutine = new MCoroutine(it);
			KeyValuePair<int,IEnumerator> kv = new KeyValuePair<int, IEnumerator>(hashId,it);
			execlusiveCoroutineList.AddLast(kv);
			return coroutine;
        }
        public void RemoveCoroutine(int hashId,IEnumerator it)
        {
			if(it==null) return;
			LinkedListNode<KeyValuePair<int,IEnumerator>> tmp = execlusiveCoroutineList.Last;
			KeyValuePair<int,IEnumerator> nodeValue = tmp.Value;
			bool flag = false;
			while (tmp!=null)
			{
				if(nodeValue.Key == hashId && nodeValue.Value == it)
				{
					flag = true;
					break;
				}
				tmp = tmp.Previous;
				nodeValue = tmp.Value;
			}
			if(flag)
			{
				execlusiveCoroutineList.Remove(nodeValue);
			}
        }
        public void RemoveAllCoroutine(int hashId)
        {
			List<KeyValuePair<int,IEnumerator>> waitRemoveList = null;
			LinkedList<KeyValuePair<int,IEnumerator>>.Enumerator enumerator = execlusiveCoroutineList.GetEnumerator();
			while (enumerator.MoveNext())
			{
				KeyValuePair<int,IEnumerator> current = enumerator.Current;
				if (current.Key == hashId)
				{
					waitRemoveList.Add(current);
				}
			}
			if(waitRemoveList!=null)
			{
				for (int i = 0; i < waitRemoveList.Count; i++)
				{
					execlusiveCoroutineList.Remove(waitRemoveList[i]);
				}
			}
        }
		public void Clear()
		{
			if(execlusiveCoroutineList.Count > 0)
				execlusiveCoroutineList.Clear();
		}
		public void AddUpdateHandle()
        {
            throw new System.NotImplementedException();
        }
        public void RemoveUpdateHandle()
        {
            throw new NotImplementedException();
        }


    }
}

