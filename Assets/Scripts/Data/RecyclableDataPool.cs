using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Client.Data
{

    public class RecyclableDataPool
    {
		public Type type;
		public string classKey;
		public Stack<RecyclableData> unusedobjs = new Stack<RecyclableData>();
		private static Dictionary<string,RecyclableDataPool> poolMaps = new Dictionary<string, RecyclableDataPool>();
		public RecyclableDataPool()
		{
			Init();
		}
		public RecyclableDataPool(Type type,string classKey)
		{
			this.type=type;
			this.classKey=classKey;
		}
		public static void Init()
		{
			if (RecyclableDataPool.poolMaps.Count > 0)
			{
				return;
			}
			Type typeFromHandle = typeof(RecyclableDataPool);
			Type[] types = typeFromHandle.Assembly.GetTypes();
			for (int i = 0; i < types.Length; i++)
			{
				Type type = types[i];
				if (!type.IsAbstract && type.IsSubclassOf(typeFromHandle))
				{
					FieldInfo field = type.GetField("CLASS_KEY", BindingFlags.Static|BindingFlags.Public);
					string key = (string)field.GetValue(null);
					RecyclableDataPool pool = new RecyclableDataPool(type,key);
					if(!RecyclableDataPool.poolMaps.ContainsKey(pool.classKey))
					{
						RecyclableDataPool.poolMaps.Add(pool.classKey,pool);
					}
				}
			}
		}

		public static RecyclableData Get(string classKey)
		{
			RecyclableDataPool pool;
			if(poolMaps.TryGetValue(classKey,out pool))
			{
				if(pool.unusedobjs.Count>0)
				{
					return pool.unusedobjs.Pop();
				}
				else
				{
					return (RecyclableData)Activator.CreateInstance(pool.type);
				}
			}
			else
			{
				Debug.LogError("不存在CLASS_KEY:"+classKey);
				return null;
			}

		}
		public static RecyclableDataPool GetPool(string classKey)
		{
			return poolMaps[classKey];
		}
	    public static void Release(RecyclableData recyclableData)
        {
            
        }
		public static void Clear(RecyclableDataPool pool)
		{
			
		}
    }
}
