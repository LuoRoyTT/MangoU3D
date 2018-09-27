using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using Mango.Framework.Core;
using System;
using Client.Event;
using Client.Data;
using Client.Framework;
using Mango.Framework.UI.Component;
using Mango.Framework.Resource;

namespace Mango.Framework.UI
{
	public class ViewModelBase:IProcessEvent
	{
        // private	Dictionary<string, List<MethodInfo>> methodInfoMap=new Dictionary<string, List<MethodInfo>>();
        private Dictionary<int,Action<UICommad>> UICommandMaps = new Dictionary<int,Action<UICommad>>();
        // private ViewBase mainView;
        public virtual string MainView{get;}
        private List<ViewBase> views;

        public ViewModelBase()
        {
            // methodInfoMap.InitMethods(this.GetType());
			// methodInfoMap.InvokeMethod(this,"initialize");
            OnCreate();
        }
		protected virtual void OnCreate()
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

        public void ProcessEvent(enEventID eventID, DataBase data)
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
                ModelManager.Instance.StartCoroutine(LoadView(viewName));
            }

        }

        private IEnumerator LoadView(string viewName)
        {
            IAssetLoader loader = ResourceModule.Instance.Get(viewName);
            IAssetAsynRequest request = loader.LoadAsyn();
            yield return request;
            GameObject viewGO = request.GetAsset<GameObject>();
            ViewBase view = viewGO.GetComponent<ViewBase>();
            views.Add(view);
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
            return views.FindAll((a)=>{return a.Visiable;}).ToArray();
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