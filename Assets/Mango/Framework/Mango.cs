using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mango.Framework
{
	public static class Mango 
	{
		private readonly static LinkedList<IGameModule> gameModules = new LinkedList<IGameModule>();
		public static void GameMain()
		{
			Type typeFromHandle = typeof(IGameModule);
			Type[] types = typeFromHandle.Assembly.GetTypes();
			for (int i = 0; i < types.Length; i++)
			{
				Type type = types[i];
				if (!type.IsAbstract && type.IsSealed && type.IsSubclassOf(typeFromHandle))
				{
					IGameModule module = (IGameModule)Activator.CreateInstance(type);
					if (module == null)
					{
						throw new System.NotImplementedException();
					}

					LinkedListNode<IGameModule> current = gameModules.First;
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

		public static T GetModule<T>() where T : IGameModule
		{
			Type moduleType = typeof(T);
			return (T)GetModule(moduleType);
		}

		private static IGameModule GetModule(Type moduleType)
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
