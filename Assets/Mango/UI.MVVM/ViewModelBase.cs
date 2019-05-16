using System.Collections.Generic;
using Mango.Core;
using Mango.Resload;
using Mango.Event;
using Mango.Framework;

namespace Mango.UI.MVVM
{
    public class ViewModelBase : MangoSingleton<ViewModelBase>
    {
        private MVVM_UIView view;
        private MangoEvents commands;
        public ViewModelBase()
        {
            commands = new MangoEvents();
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