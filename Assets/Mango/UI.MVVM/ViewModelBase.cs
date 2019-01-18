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
    public class ViewModelBase : Singleton<ViewModelBase>
    {
        private MVVM_UIView view;
        private Events commands;
        public ViewModelBase()
        {
            commands = new Events();
            OnCreate();
        }

        protected virtual void OnCreate()
        {

        }


        public void ReceiveCommand(ushort commandID, IMessage message)
        {
            commands.Call(commandID,message);
        }
        public void SendNotifiction(ushort notifictionID, IMessage message)
        {
            view.OnNotification(notifictionID,message);
        }
    }
}