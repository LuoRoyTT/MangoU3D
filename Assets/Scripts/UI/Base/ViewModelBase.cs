using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using Client.Core;
using System;

namespace Client.UI
{
    public class BindableProperty<T>
    {
        private UnityEvent<T,T> OnValueChanged;
        public BindableProperty(UnityAction<T,T> onValueChanged)
        {
            OnValueChanged.AddListener(onValueChanged);
        }
        public BindableProperty(T value,UnityAction<T,T> onValueChanged)
		{
			_value = value;
            OnValueChanged.AddListener(onValueChanged);
		}
        private T _value = default(T);
        public T Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (!Equals(_value, value))
                {
                    T oldValue = _value;
                    _value = value;
                    if (OnValueChanged != null)
                        OnValueChanged.Invoke(oldValue,value);
                }
            }
        }
 
        public void AddListener(UnityAction<T,T> onValueChanged)
        {
            this.OnValueChanged.AddListener(onValueChanged);
        }
 
        public void RemoveListener(UnityAction<T,T> onValueChanged)
        {
            this.OnValueChanged.RemoveListener(onValueChanged);
        }
 
        public void RemoveAllListeners()
        {
            this.OnValueChanged.RemoveAllListeners();
        }

        public override string ToString()
        {
            return (Value != null ? Value.ToString() : "null");
        }
 
        public void Clear()
        {
            _value = default(T);
        }
 
	}

	public class ViewModelBase
	{
        private	Dictionary<string, List<MethodInfo>> methodInfoMap=new Dictionary<string, List<MethodInfo>>();
        private ModuleBase module;
        private List<ViewBase> views=new List<ViewBase>();

        private Dictionary<int,List<Action<UIEvent>>> UIEventMaps = new Dictionary<int,List<Action<UIEvent>>>();

        public ViewModelBase(ModuleBase module)
        {
            this.module = module;
            methodInfoMap.InitMethods(this.GetType());
			methodInfoMap.InvokeMethod(this,"initialize");
            OnCreate();
        }
		protected virtual void OnCreate()
		{

		}
        public void AddView(ViewBase view)
        {
            if(views.Contains(view)) return;
            views.Add(view);
        }

        public void RemoveView(ViewBase view)
        {
            if(views.Contains(view))
            {
                views.Remove(view);
            }
        }
        public void ReceiveCommand(int command,UIEvent uiMsg)
        {
            if(UIEventMaps.ContainsKey(command))
            {
                List<Action<UIEvent>> actions = UIEventMaps[command];
                if(actions!=null && actions.Count!=0)
                {
                    for (int i = 0; i < actions.Count; i++)
                    {
                        actions[i](uiMsg);
                    }
                } 
            }
            else
            {
                Debug.LogError("");
            }
        }
        protected void AddUIEventListener(int command,Action<UIEvent> action)
        {
            if(UIEventMaps.ContainsKey(command))
            {
                List<Action<UIEvent>> actions = UIEventMaps[command];
                actions.Add(action);
            }
            else
            {
                UIEventMaps.Add(command,new List<Action<UIEvent>>(){action});
            }
        }
        protected void RemoveUIEventListeners(int command,Action<UIEvent> action)
        {
            if(UIEventMaps.ContainsKey(command))
            {
                List<Action<UIEvent>> actions = UIEventMaps[command];
                actions.Remove(action);
                if(actions.Count==0)
                {
                    UIEventMaps.Remove(command);
                }
            }
            else
            {
                Debug.LogError("");
            }
        }
        protected void RemoveAllUIEvnetListeners()
        {
            UIEventMaps.Clear();
        }
	}
}