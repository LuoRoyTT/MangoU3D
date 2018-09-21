using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mango
{
	

	public class Mango 
	{

		private static Mango instance;

		public static Mango Instance
		{
			get
			{
				if (Mango.instance == null)
				{
					Mango.instance = new Mango();
				}
				return Mango.instance;
			}
		}
	}
}
