﻿using System.Collections;
using System.Collections.Generic;
using Client.Data;
using UnityEngine;

namespace Client.Event
{
	public interface IBroadcast 
	{
		void Broadcast(enEventID eventID,DataBase data);
	}
}
