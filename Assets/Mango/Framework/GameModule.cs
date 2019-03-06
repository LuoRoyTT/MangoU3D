using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mango.Framework
{
		public class GameModule 
		{
				public virtual eModulePriority Priority
				{
						get
						{
							return eModulePriority.Default;
						}
				}
				public void Init()
				{
					OnInit();
				}
				public void Release()
				{
					OnRelease();
				}
				protected virtual void OnInit(){}
				protected virtual void OnRelease(){}
		}
}

