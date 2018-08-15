using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client.Data
{
	public abstract class RecyclableObject 
	{ 
		public abstract string ClassKey{get;}
		public void Release()
		{
			OnRelease();
			RecyclableObjectPool.Release(this);
		}
		public virtual void OnUse()
		{
		}

		public virtual void OnRelease()
		{
		}

	}
}