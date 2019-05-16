using System;
using System.Collections;
using System.Collections.Generic;
using Mango.Event;
using Mango.Resload;
using Mango.UI;
using UnityEngine;

namespace Mango.UI.MVVM
{
    public class MVVM_UIView :MVVM_UIPanel,IView
    {
		private Dictionary<Type,MVVM_UIPanel> panels;
		private ViewModelBase model;
		private string groupName;
		private MangoEvents notifications;
		private readonly int[] _assets;
        protected virtual int[] Assets
        {
            get
            {
                return _assets;
            }
        }
		public MVVM_UIView()
		{
			notifications = new MangoEvents();
			groupName = this.GetType().Name;
		}

        public sealed override bool Vaild
        {
            get
            {
                return window != null && model != null;
            }
        }
		protected void SendCommand(ushort commandID,IMessage message)
		{
			model.ReceiveCommand(commandID,message);
		}

		public void OnNotification(ushort notificationID,IMessage message)
		{
			notifications.Call(notificationID,message);
		}
		public MVVM_UIPanel OpenPanel(Type type)
		{
			MVVM_UIPanel panel;
			if(!panels.TryGetValue(type,out panel))
			{
				panel = Activator.CreateInstance(type) as MVVM_UIPanel;
			}
			if(!panel.Vaild)
			{
				panel.Init();
			}
			panel.Appear();
			return panel;
		}
		public T OpenPanel<T>() where T:MVVM_UIPanel
		{
			return OpenPanel(typeof(T)) as T;
		}
		public void HidePanel(Type type)
		{
			MVVM_UIPanel panel;
			if(!panels.TryGetValue(type,out panel))
			{
				Debug.LogError("不存在Type:" + type.Name);
				return;
			}
			panel.Hide();
		}
		public void HidePanel<T>() where T:MVVM_UIPanel
		{
			HidePanel(typeof(T));
		}

		protected void DynamicAddAssets(Action onFinished, params int[] assets)
        {
            GameObjectPool.Instance.PreloadAsset(groupName, null, onFinished, assets);
        }

        public void Enter()
        {
			if(Vaild)
			{
				Appear();
				return;
			}
            GameObjectPool.Instance.PreloadAsset(groupName, null, () =>
            {
                OnEnter();
				Init();
				Appear();
            }, Assets);
        }
		public new void Close()
		{
			base.Close();
            GameObjectPool.Instance.ClearGoup(groupName);
		}
        public void Exit()
        {
            OnExit();
        }

		protected virtual void OnEnter(){}
        protected virtual void OnExit(){}
    }
}
