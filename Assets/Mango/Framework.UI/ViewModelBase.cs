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

namespace Mango.Framework.UI
{
	public class ViewModelBase:IProcessEvent
	{
        private	Dictionary<string, List<MethodInfo>> methodInfoMap=new Dictionary<string, List<MethodInfo>>();
        private Dictionary<int,Action<UICommad>> UICommandMaps = new Dictionary<int,Action<UICommad>>();
        private ViewBase mainView;

        public ViewModelBase()
        {
            methodInfoMap.InitMethods(this.GetType());
			methodInfoMap.InvokeMethod(this,"initialize");
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
            mainView.OnNotifiction(notifictionID,notifiction);
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