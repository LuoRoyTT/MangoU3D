using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Mango.Framework.Core
{

    public class RecyclableObjectPool
    {
		public Type type;
		public string classKey;
		public Stack<IRecyclableObject> unusedobjs = new Stack<IRecyclableObject>();
		private static Dictionary<string,RecyclableObjectPool> poolMaps = new Dictionary<string, RecyclableObjectPool>();
		public RecyclableObjectPool()
		{
			Init();
		}
		public RecyclableObjectPool(Type type,string classKey)
		{
			this.type=type;
			this.classKey=classKey;
		}
		public static void Init()
		{
			if (RecyclableObjectPool.poolMaps.Count > 0)
			{
				return;
			}
			Type typeFromHandle = typeof(RecyclableObjectPool);
			Type[] types = typeFromHandle.Assembly.GetTypes();
			for (int i = 0; i < types.Length; i++)
			{
				Type type = types[i];
				if (!type.IsAbstract && type.IsSubclassOf(typeFromHandle))
				{
					FieldInfo field = type.GetField("CLASS_KEY", BindingFlags.Static|BindingFlags.Public);
					string key = (string)field.GetValue(null);
					RecyclableObjectPool pool = new RecyclableObjectPool(type,key);
					if(!RecyclableObjectPool.poolMaps.ContainsKey(pool.classKey))
					{
						RecyclableObjectPool.poolMaps.Add(pool.classKey,pool);
					}
				}
			}
		}

		public static IRecyclableObject Get(string classKey)
		{
			RecyclableObjectPool pool;
			if(poolMaps.TryGetValue(classKey,out pool))
			{
				if(pool.unusedobjs.Count>0)
				{
					return pool.unusedobjs.Pop();
				}
				else
				{
					return (IRecyclableObject)Activator.CreateInstance(pool.type);
				}
			}
			else
			{
				Debug.LogError("不存在CLASS_KEY:"+classKey);
				return null;
			}
		}
		public static T Get<T>() where T : IRecyclableObject
		{
			RecyclableObjectPool pool;
			T obj = default(T);
			if(poolMaps.TryGetValue(obj.ClassKey,out pool))
			{
				if(pool.unusedobjs.Count>0)
				{
					obj = (T)pool.unusedobjs.Pop();
					return obj;
				}
				else
				{
					return (T)Activator.CreateInstance(pool.type);
				}
			}
			else
			{
				Debug.LogError("不存在CLASS_KEY:"+obj.ClassKey);
				return obj;
			}
		}
		public static RecyclableObjectPool GetPool(string classKey)
		{
			return poolMaps[classKey];
		}
	    public static void Release(IRecyclableObject recyclableData)
        {
            RecyclableObjectPool pool;
			if(poolMaps.TryGetValue(recyclableData.ClassKey,out pool))
			{
				pool.unusedobjs.Push(recyclableData);
				recyclableData.OnRelease();
			}
        }
		public void Clear()
		{
			unusedobjs.Clear();
		}
		public static void ClearAll()
		{
			foreach (var pool in poolMaps.Values)
			{
				pool.unusedobjs.Clear();
			}
		}
    }
}
