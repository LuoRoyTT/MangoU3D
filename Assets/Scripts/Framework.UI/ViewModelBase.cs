using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using Client.Core;
using System;
using Client.Event;
using Client.Data;
using Client.Framework;

namespace Client.UI
{
	public class ViewModelBase:IProcessEvent
	{
        private	Dictionary<string, List<MethodInfo>> methodInfoMap=new Dictionary<string, List<MethodInfo>>();
        private ModuleBase module;
        private List<ViewBase> views=new List<ViewBase>();

        private Dictionary<int,Action<UICommad>> UICommandMaps = new Dictionary<int,Action<UICommad>>();

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
            for (int i = 0; i < views.Count; i++)
            {
                views[i].OnNotifiction(notifictionID,notifiction);
            }
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
    }
}