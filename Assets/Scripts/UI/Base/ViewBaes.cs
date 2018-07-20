using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Client.Core;
using UnityEngine;

namespace Client.UI
{
	public class ViewBase : UIContainer 
	{
		private	Dictionary<string, List<MethodInfo>> methodInfoMap=new Dictionary<string, List<MethodInfo>>();
		private ModuleBase module;
		private BindableProperty<ViewModelBase> BindableProperty;

        public ViewModelBase Model 
		{
			get
			{
				return BindableProperty.Value ;
			}
			set
			{
				BindableProperty.AddListener(OnViewValueChanged);
				BindableProperty.Value = value;
			}
		}

        public override void Initialize()
		{
			base.Initialize();
			methodInfoMap.InitMethods(this.GetType());
			methodInfoMap.InvokeMethod(this,"initialize");

		}
		private void OnViewValueChanged(ViewModelBase oldModel,ViewModelBase newModel)
		{
			methodInfoMap.InvokeMethod(this,"OnBindValue");
			if(oldModel!=null)
			{
				oldModel.RemoveView(this);
			}
			if(newModel!=null)
			{
				newModel.AddView(this);
			}
			
		}
		private void SendCommand(int command,params object[] objs)
		{
			UIEvent uiMsg=new UIEvent();
			Model.ReceiveCommand(command,uiMsg);
		}
	}
}