using UnityEngine;
using System.Collections;
using System;

namespace Mango.Core
{
	public abstract class MangoSingleton<T> : MangoObject where T : MangoSingleton<T>
	{
		private static T instance = null;
		public static T Instance {
			get {
				if (instance == null)
				{
					instance = CreateInstance();
				}
				return instance;
			}
		}
		
		private static T CreateInstance ()
		{
			if (instance == null)
            {
                instance = Activator.CreateInstance<T>();
            } 
            Instance.Init();
			return Instance;
		}
		protected virtual void Init()
		{
			
		}

	}
}