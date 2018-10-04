using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using Mango.Framework.Core;
using System;
using Mango.Framework.UI.Component;
using Mango.Framework.Resource;
using Mango.Framework.Event;

namespace Mango.Framework.UI.MVVM
{
	public class ViewModelBase: Singleton<ViewModelBase>,IProcessEvent
	{
        // private	Dictionary<string, List<MethodInfo>> methodInfoMap=new Dictionary<string, List<MethodInfo>>();
        private Dictionary<int,Action<UICommad>> UICommandMaps = new Dictionary<int,Action<UICommad>>();
        public string ModelName
        {
            get
            {
                return GetType().Name;
            }
        }
        public virtual string MainView {get;}
        private List<ViewBase> views;
        protected virtual string[] Assets{get;}

        protected void DynamicAddAssets(Action onFinished,params string[] assets)
        {
            GameObjectPool.Instance.PreloadAsset(this.ModelName,null,onFinished,assets);
        }
        public ViewModelBase()
        {
            // methodInfoMap.InitMethods(this.GetType());
			// methodInfoMap.InvokeMethod(this,"initialize");
            OnCreate();
        }
        

        public void Enter(Action callback)
        {
            GameObjectPool.Instance.PreloadAsset(ModelName,null,()=>
            {
                OnEnter();
                if(String.IsNullOrEmpty(MainView))
                {
                    OpenView(MainView);
                }
                if(callback!=null)
                {
                    callback();
                }
            },Assets);
        }
        public void Exit()
        {
            OnExit();
            for (int i = 0; i < views.Count; i++)
            {
                GameObjectPool.Instance.Destroy(views[i].gameObject);
            }
            GameObjectPool.Instance.ClearGoup(ModelName);
        }
        protected virtual void OnCreate()
		{

		}
        protected virtual void OnEnter()
        {

        }

        protected virtual void OnExit()
        {

        }
        public void ReceiveCommand(int commandID,UICommad command)
        {
            if(UICommandMaps.ContainsKey(commandID))
            {
                Action<UICommad> commands = UICommandMaps[commandID];
                if(commands!=null)
                {
                    commands(command);
                } 
            }
            else
            {
                Debug.LogError("");
            }
        }
        public void SendNotifiction(int notifictionID,UINotifiction notifiction)
        {
            // mainView.OnNotifiction(notifictionID,notifiction);
        }
        protected void AddUIEventListener(int commandID,Action<UICommad> command)
        {
            if(UICommandMaps.ContainsKey(commandID))
            {
                Action<UICommad> commands = UICommandMaps[commandID];
                commands +=command;
            }
            else
            {
                Action<UICommad> commands = null; 
                commands+=command;
                UICommandMaps.Add(commandID,commands);
            }
        }
        protected void RemoveUIEventListeners(int commandID,Action<UICommad> command)
        {
            if(UICommandMaps.ContainsKey(commandID))
            {
                Action<UICommad> commands = UICommandMaps[commandID];
                commands-=command;
                if(commands==null)
                {
                    UICommandMaps.Remove(commandID);
                }
            }
            else
            {
                Debug.LogError("");
            }
        }
        protected void RemoveAllUIEvnetListeners()
        {
            UICommandMaps.Clear();
        }

        public void ProcessEvent(enEventID eventID, IMessage data)
        {
            throw new NotImplementedException();
        }

        public void RegistMsg(enEventID eventID)
        {
            MsgCenter.Instance.RegistMsg(this,eventID);
        }

        public void UnRegistMsg(enEventID eventID)
        {
            MsgCenter.Instance.UnRegistMsg(this,eventID);
        }

        public void OpenView(string viewName)
        {
            ViewBase view = GetView(viewName);
            if(view)
            {
                view.Appear();
            }
            else
            {
                GameObject viewGO = GameObjectPool.Instance.CreateGO(viewName,null,false);
                view = viewGO.GetComponent<ViewBase>();
                views.Add(view);
            }
        }
        
        private ViewBase GetView(string viewName)
        {
            for (int i = 0; i < views.Count; i++)
            {
                if(views[i].gameObject.name.Equals(viewName))
                {
                    return views[i];
                }
            }
            return null;
        }
        private ViewBase GetVisiableView(string viewName)
        {
            for (int i = 0; i < views.Count; i++)
            {
                if(views[i].gameObject.name.Equals(viewName) && views[i].Visiable)
                {
                    return views[i];
                }
            }
            return null;
        }

        private ViewBase[] GetAllViews()
        {
            return views.ToArray();
        }

        private ViewBase[] GetVisiableViews()
        {
            return views.FindAll(a=> a.Visiable).ToArray();
        }

        public void HideView(string viewName)
        {
            ViewBase view = GetVisiableView(viewName);
            if(view)
            {
                view.Hide();
            }
        }

        public void HideAllView()
        {
            ViewBase[] visiableViews = GetVisiableViews();
            for (int i = 0; i < visiableViews.Length; i++)
            {
                visiableViews[i].Hide();
            }

        }
        public void ChangeModel(string modelName)
        {
            ModelManager.Instance.ChangeModel(modelName);
        }
    }
}