using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client.UI
{
	public class UIComponent : MonoBehaviour
	{
		private UIContainer containercontainer;
		public virtual void Initialize(UIContainer container)
		{
			containercontainer = container;
		}
		protected virtual void OnCreate(){}
		protected virtual void Appear(){}
		protected virtual void Hide(){}
		protected virtual void OnClose(){}
		public virtual void Reset(){}
	}
}
