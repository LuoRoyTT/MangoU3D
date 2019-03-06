using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mango.Framework.Event
{
    public class EventNode
    {
        private MangoDelegate callback;
        private MangoDelegate<IMessage> callback1;
        private bool empty;
        public bool Empty
        {
            get
            {
                return empty;
            }
        }
        public EventNode next;
        public EventNode(MangoDelegate callback)
        {
            this.callback = callback;
            next = null;
        }
        public EventNode(MangoDelegate<IMessage> callback1)
        {
            this.callback1 = callback1;
            next = null;
        }
        public bool SameAs(MangoDelegate callback)
        {
            return this.callback == callback ;
        }
        public bool SameAs(MangoDelegate<IMessage> callback1)
        {
            return this.callback1 == callback1 ;
        }
        public void Call()
        {
            if(callback != null)
            {
                callback();
            }
        }
        public void Call(IMessage message)
        {
            if(callback1 != null)
            {
                callback1(message);
            }
        }
    }
}
