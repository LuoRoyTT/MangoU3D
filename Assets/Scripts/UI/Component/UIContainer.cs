using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client.UI
{
	public class UIContainer : UIComponent 
	{
		private Dictionary<string,UIComponent> components = new Dictionary<string,UIComponent>();
	}
}
