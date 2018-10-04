using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mango.Framework.Frame
{
	public interface ILogicUpdate
	{
		void AddUpdateHandle();
		void RemoveUpdateHandle();
		void LogicUpdate();

	}
}

