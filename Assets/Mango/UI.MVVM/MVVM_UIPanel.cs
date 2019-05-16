using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Mango.Core;
using Mango.Resload;
using Mango.UI.Component;
using UnityEngine;

namespace Mango.UI.MVVM
{
	public class MVVM_UIPanel  
	{
		protected UIWindow window;
		public virtual bool Vaild
        {
            get
            {
                return window != null;
            }
        }

		protected void InitWindow()
		{
			
		}
		public void Init()
		{
			OnInit();
		}
		public void Appear()
		{
			OnAppear();
		}
		public void Hide()
		{
			OnHide();
		}
		public void Close()
		{
			window.Dispose();
			OnClose();
		}
		protected virtual void OnInit(){}
		protected virtual void OnAppear(){}
		protected virtual void OnHide(){}
		protected virtual void OnClose(){}
    }
}