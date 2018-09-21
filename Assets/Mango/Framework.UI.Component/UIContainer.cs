using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mango.Framework.UI.Component
{
	public class UIContainer : UIComponent 
	{
		private Dictionary<string,UIComponent> componentDic = new Dictionary<string,UIComponent>();
		protected override void OnCreate()
		{
			// methodInfoMap.InitMethods(this.GetType());
			UIComponent[] components = GetComponentsInChildren<UIComponent>(true);
			if(components!=null)
			{
				for (int i = 0; i < components.Length; i++)
				{
					AddUIComponentToDic(components[i]);
				}
			}
		}

		public void AddUIComponentToDic(UIComponent component)
		{
			if(!componentDic.ContainsKey(component.Name))
			{
				componentDic.Add(component.Name,component);
			}
			else
			{
				Debug.LogError("存在同名的Component:"+component.Name);
			}
		}

		public void RemoveUIComponentFromDic(string cmpName)
		{
			if(componentDic.ContainsKey(cmpName))
			{
				componentDic.Remove(cmpName);
			}
			else
			{
				Debug.LogError("不存在Component:"+cmpName);
			}
		}

		public void RemoveUIComponentFromDic(UIComponent cmp)
		{
			string cmpName=cmp.Name;
			RemoveUIComponentFromDic(cmpName);
		}
	}
}
