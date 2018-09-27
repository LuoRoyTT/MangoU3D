using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mango.Framework.UI.Component
{
	public class UIContainer : UIComponent 
	{
		private Dictionary<string,UIComponent> componentDic = new Dictionary<string,UIComponent>();
		private int childCount; 
		private int initializedCount;
		void Awake()
		{
			UIComponent[] components = GetComponentsInChildren<UIComponent>(true);
			childCount = 0;
			initializedCount = 0;
			if(components!=null)
			{
				for (int i = 0; i < components.Length; i++)
				{
					var cmp = components[i];
					cmp.Initialize(this);
					if(!cmp.Equals(this))
					{
						componentDic.Add(cmp.Name,cmp);
						childCount++;
					}
				}
			}
			OnCreate();
		}

		void Start()
		{
			foreach (var cmp in componentDic.Values)
			{
				cmp.Initialize(this);
			}
		}
		protected bool visiable;
		public bool Visiable
		{
			get
			{
				return visiable;
			}
		}

        public void ReceiveInitializedMsg()
        {
            initializedCount++;
			if(initializedCount>=childCount)
			{
				Initialize(this);
				Appear();
			}
        }
		public override void Appear()
		{
			foreach (var cmp in componentDic.Values)
			{
				cmp.Appear();
			}
			visiable = true;
			OnAppear();
		}

		public override void Hide()
		{
			foreach (var cmp in componentDic.Values)
			{
				cmp.Hide();
			}
			visiable = false;
			OnHide();
		}
		protected override void Prepare(Action onFinished)
		{
			if(onFinished!=null)
			{
				onFinished();
			}
		}

		protected virtual void OnCreate()
		{

		}
		protected virtual void OnAppear()
		{

		}
		protected virtual void OnHide()
		{
			
		}
		protected virtual void OnClose()
		{

		}
    }
}
