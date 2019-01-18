using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mango.Framework.Event
{
    public delegate void EventCallback();
    public delegate void EventCallback1(IMessage message);
    public class EventNode
    {
        private EventCallback callback;
        private EventCallback1 callback1;
        private bool empty;
        public bool Empty
        {
            get
            {
                return empty;
            }
        }
        public EventNode next;
        public EventNode(EventCallback callback)
        {
            this.callback = callback;
            next = null;
        }
        public EventNode(EventCallback1 callback1)
        {
            this.callback1 = callback1;
            next = null;
        }
        public bool SameAs(EventCallback callback)
        {
            return this.callback == callback ;
        }
        public bool SameAs(EventCallback1 callback1)
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
