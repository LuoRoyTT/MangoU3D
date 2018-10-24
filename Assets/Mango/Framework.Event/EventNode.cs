using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mango.Framework.Event
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
