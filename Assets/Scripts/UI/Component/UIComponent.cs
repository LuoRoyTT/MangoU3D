using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client.UI
{
	public class UIComponent : MonoBehaviour
	{
		public virtual void Initialize(){}
		protected virtual void OnCreate(){}
		protected virtual void Appear(){}
		protected virtual void Hide(){}
		protected virtual void OnClose(){}
	}
}
