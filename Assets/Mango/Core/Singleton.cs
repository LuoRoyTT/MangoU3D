﻿using System;

namespace Mango.Framework.Core
{
	public class Singleton<T> where T : class, new()
	{
		private static T s_instance;

		public static T instance
		{
			get
			{
				if (Singleton<T>.s_instance == null)
				{
					Singleton<T>.CreateInstance();
				}
				return Singleton<T>.s_instance;
			}
		}

		protected Singleton()
		{
		}

		public static void CreateInstance()
		{
			if (Singleton<T>.s_instance == null)
			{
				Singleton<T>.s_instance = Activator.CreateInstance<T>();
			}
		}

		public static void DestroyInstance()
		{
			if (Singleton<T>.s_instance != null)
			{
				Singleton<T>.s_instance = (T)((object)null);
			}
		}

		public static T GetInstance()
		{
			if (Singleton<T>.s_instance == null)
			{
				Singleton<T>.CreateInstance();
			}
			return Singleton<T>.s_instance;
		}

		public static bool HasInstance()
		{
			return Singleton<T>.s_instance != null;
		}
	}
}

