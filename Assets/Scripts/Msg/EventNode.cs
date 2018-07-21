using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Client.Data;

namespace Client.Event
{
    public class EventNode
    {
        public IProcessEvent script;
        public EventNode next;
        public EventNode(IProcessEvent script)
        {
            this.script = script;
            next = null;
        }
    }
}
