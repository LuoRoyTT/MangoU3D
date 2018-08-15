using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client.Data
{
	public abstract class RecyclableData 
	{ 
		public abstract string ClassKey{get;}
		public void Release()
		{
			OnRelease();
			RecyclableDataPool.Release(this);
		}
		public virtual void OnUse()
		{
		}

		public virtual void OnRelease()
		{
		}

	}
}