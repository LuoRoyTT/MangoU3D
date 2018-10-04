using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mango.Framework.Event
{
	public interface IBroadcast 
	{
		void Broadcast(enEventID eventID,IMessage data);
	}
}
