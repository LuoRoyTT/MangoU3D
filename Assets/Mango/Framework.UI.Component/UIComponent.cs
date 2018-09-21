using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mango.Framework.UI.Component
{
	public class UIComponent : MonoBehaviour
	{
		public string Name
		{
			get
			{
				return gameObject.name;
			}
		}
		protected UIContainer container;
		protected bool initialized;
		public void Initialize(UIContainer container)
		{
			if(initialized) return;
			initialized=true;
			this.container = container;
			OnCreate();
		}
		protected virtual void OnCreate(){}
		protected virtual void Appear(){}
		protected virtual void Hide(){}
		protected virtual void OnClose(){}
		public virtual void Reset(){}
	}
}
