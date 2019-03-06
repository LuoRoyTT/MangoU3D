using System;
using System.Collections;
using System.Collections.Generic;
using Mango.Framework.Core;
using Mango.Framework.Event;
using UnityEngine;

namespace Mango.Framework.UI
{
    public class UIModule : GameModule
    {
        private Dictionary<Type,IView> views;
		private IView current;
		public Transform Root
		{
			get
			{
				return root;
			}
		}
		private Transform root;
        private MangoEvents uievents;
		public void AddUIListener(ushort msgId,MangoDelegate callback)
		{
			uievents.AddListener(msgId,callback);
		}
		public void AddUIListener(ushort msgId,MangoDelegate<IMessage> callback1)
		{
			uievents.AddListener(msgId,callback1);
		}
		public void RemoveUIListener(ushort msgId,MangoDelegate callback)
		{
			uievents.RemoveListener(msgId,callback);
		}
		public void RemoveUIListener(ushort msgId,MangoDelegate<IMessage> callback1)
		{
			uievents.RemoveListener(msgId,callback1);
		}
		public void OpenView(Type type)
		{
			IView view;
			if(!views.TryGetValue(type,out view))
			{
				view = Activator.CreateInstance(type) as IView;
			}
			view.Enter();
		}
        public void OpenView<T>() where T:IView
		{
            OpenView(typeof(T));
		}

		public void ReturnBack()
		{

		}
    }
}

