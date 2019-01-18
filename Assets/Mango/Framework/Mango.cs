using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mango.Framework
{
	public static class Mango 
	{
		private readonly static LinkedList<GameModule> gameModules = new LinkedList<GameModule>();
		public static void GameMain()
		{
			Type typeFromHandle = typeof(GameModule);
			Type[] types = typeFromHandle.Assembly.GetTypes();
			for (int i = 0; i < types.Length; i++)
			{
				Type type = types[i];
				if (!type.IsAbstract && type.IsSealed && type.IsSubclassOf(typeFromHandle))
				{
					GameModule module = (GameModule)Activator.CreateInstance(type);
					if (module == null)
					{
						throw new System.NotImplementedException();
					}

					LinkedListNode<GameModule> current = gameModules.First;
					while (current != null)
					{
						if (module.Priority > current.Value.Priority)
						{
							break;
						}

						current = current.Next;
					}

					if (current != null)
					{
						gameModules.AddBefore(current, module);
					}
					else
					{
						gameModules.AddLast(module);
					}
				}
			}
		}

		public static T GetModule<T>() where T : GameModule
		{
			Type moduleType = typeof(T);
			return (T)GetModule(moduleType);
		}

		private static GameModule GetModule(Type moduleType)
		{
			foreach (var module in gameModules)
			{
				if (module.GetType()==moduleType)
				{
					return module;
				}
			}
			return null;
		}
	}
}
