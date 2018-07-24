using System;
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
		private Dictionary<int,Action<UINotifiction>> UINotifictionMaps = new Dictionary<int,Action<UINotifiction>>();
		private ModuleBase module;
		private BindableProperty bindableViewModel;

        public ViewModelBase Model 
		{
			get
			{
				return bindableViewModel.Value as ViewModelBase ;
			}
			set
			{
				bindableViewModel.AddListener(OnViewValueChanged);
				bindableViewModel.Value = value;
			}
		}


		protected override void OnCreate()
		{
			methodInfoMap.InitMethods(this.GetType());
			base.OnCreate();
			
		}

		private void OnViewValueChanged(object oldModel,object newModel)
		{
			//methodInfoMap.InvokeMethod(this,"OnBindValue");
			if(oldModel!=null)
			{
				(oldModel as ViewModelBase).RemoveView(this);
			}
			if(newModel!=null)
			{
				(newModel as ViewModelBase).AddView(this);
			}
			
		}
		protected void SendCommand(int commandID,UICommad command)
		{
			Model.ReceiveCommand(commandID,command);
		}

		public void OnNotifiction(int notifictionID,UINotifiction notifiction)
		{
            if(UINotifictionMaps.ContainsKey(notifictionID))
            {
                Action<UINotifiction> notifictions = UINotifictionMaps[notifictionID];
                if(notifictions!=null)
                {
					notifictions(notifiction);
                } 
            }
            else
            {
                Debug.LogError("");
            }
		}
	}
}