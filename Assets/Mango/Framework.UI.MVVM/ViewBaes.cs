using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Mango.Framework.Core;
using Mango.Framework.Coroutine;
using Mango.Framework.Resource;
using Mango.Framework.UI.Component;
using UnityEngine;

namespace Mango.Framework.UI.MVVM
{
	public class ViewBase : UIContainer,ICoroutine 
	{
		// private	Dictionary<string, List<MethodInfo>> methodInfoMap;
		// private List<SubViewBase> subViews;
		private Dictionary<int,Action<UINotifiction>> UINotifictionMaps;
		public ViewModelBase Model{get;set;}

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
		public override void Appear()
		{
			base.Appear();
			OnAppear();
		}

		public override void Hide()
		{
			base.Hide();
			OnHide();
			RemoveAllCoroutine();
		}

        public MCoroutine AppendCoroutine(IEnumerator it)
        {
            return MCoroutinManager.Instance.AppendCoroutine(GetHashCode(),it);
        }

        public void RemoveCoroutine(IEnumerator it)
        {
			MCoroutinManager.Instance.AppendCoroutine(GetHashCode(),it);
        }

        public void RemoveAllCoroutine()
        {
			MCoroutinManager.Instance.RemoveAllCoroutine(GetHashCode());
        }
    }
}