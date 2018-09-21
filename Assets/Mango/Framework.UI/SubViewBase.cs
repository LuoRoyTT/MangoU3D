using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Mango.Framework.Core;
using Mango.Framework.UI.Component;
using UnityEngine;

namespace Mango.Framework.UI
{
	public class SubViewBase : ViewBase 
	{
		private	Dictionary<string, List<MethodInfo>> methodInfoMap=new Dictionary<string, List<MethodInfo>>();
		private Dictionary<int,Action<UINotifiction>> UINotifictionMaps = new Dictionary<int,Action<UINotifiction>>();

		private ViewBase mainView;

        public override ViewModelBase Model 
		{
			get
			{
				return mainView.Model;
			}
		}


		protected override void OnCreate()
		{
			base.OnCreate();
			methodInfoMap.InitMethods(this.GetType());	
		}

		private void OnViewValueChanged(object oldModel,object newModel)
		{

		}
	}
}