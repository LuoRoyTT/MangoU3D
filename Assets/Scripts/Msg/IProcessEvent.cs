using System.Collections;
using System.Collections.Generic;
using Client.Data;
using UnityEngine;

namespace Client.Event
{
    public interface IProcessEvent
    {
        void ProcessEvent(enEventID eventID,DataBase data);
		void RegistMsg(enEventID eventID);
		void UnRegistMsg(enEventID eventID);
    }
}
