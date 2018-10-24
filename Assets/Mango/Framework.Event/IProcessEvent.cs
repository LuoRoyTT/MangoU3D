using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mango.Framework.Event
{
    public interface IProcessEvent
    {
        void ProcessEvent(enEventID eventID,IMessage msg);
		void RegistMsg(enEventID eventID);
		void UnRegistMsg(enEventID eventID);
    }
}
