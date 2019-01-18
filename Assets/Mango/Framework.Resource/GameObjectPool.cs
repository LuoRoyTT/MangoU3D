using System;
using System.Collections;
using System.Collections.Generic;
using Mango.Framework.Core;
using Mango.Framework.Task;
using Mango.Framework.Task.Coroutine;
using UnityEngine;

namespace Mango.Framework.Resource
{
	public class GameObjectPool : MonoSingleton<GameObjectPool>
	{
		Dictionary<ResID,List<string>> assetsMap;
		Dictionary<string,List<IAssetLoader>> groupMap;
		Dictionary<string,List<GameObject>> pool;
		private bool loading;
		private ResourceModule resourceModule;
		private TaskModule taskModule;
		protected override void Init()
		{
			resourceModule = Mango.GetModule<ResourceModule>();
			taskModule = Mango.GetModule<TaskModule>();
		}
		public void PreloadAsset(string groupName,Action<float> update,Action onFinished,params int[] ids)
		{
			if(loading) return;
			if(String.IsNullOrEmpty(groupName)) return;
			if(ids==null) return;
			loading = true;
			HashSet<int> assetsHashSet = null;
			List<IAssetLoader> loaders = null;
			for (int i = 0; i < ids.Length; i++)
			{
				assetsHashSet.Add(ids[i]);
			}
			foreach (int id in assetsHashSet)
			{
				loaders.Add(resourceModule.GetAssetLoader(id));
			}
			int index = 0;
			taskModule.StartCoroutine(DoPreloadAsset(groupName,loaders,onFinished,index));
		}
		private IEnumerator DoPreloadAsset(string groupName,List<IAssetLoader> loaders,Action onFinished,int index)
		{
			IAssetLoader loader = loaders[index];
			IAssetAsynRequest request = loader.LoadAsyn();
			yield return request;
			index++;
			if(index>=loaders.Count)
			{
				groupMap.Add(groupName,loaders);
				for (int i = 0; i < loaders.Count; i++)
				{
					ResID resID = loaders[i].ResID;
					if(assetsMap.ContainsKey(resID))
					{
						assetsMap[resID].Add(groupName);
					}
					else
					{
						assetsMap.Add(resID,new List<string>{groupName});
					}
				}
				loading = false;
				if(onFinished!=null) onFinished();
			}
			else
			{
				taskModule.StartCoroutine(DoPreloadAsset(groupName,loaders,onFinished,index));
			}
		}
		public void ClearGoup(string groupName)
		{
			List<IAssetLoader> loaders;
			if (groupMap.TryGetValue(groupName,out loaders))
			{
				if(loaders!=null && loaders.Count>0)
				{
					for (int i = 0; i < loaders.Count; i++)
					{
						if(assetsMap.ContainsKey(loaders[i].ResID))
						{
							List<string> groups = assetsMap[loaders[i].ResID];
							if(groups!=null && groups.Count>0)
							{
								groups.RemoveAt(groups.Count-1);
							}
							if(groups==null || groups.Count==0)
							{
								assetsMap.Remove(loaders[i].ResID);
								pool.Remove(loaders[i].ResID.assetName);
							}
						}
						loaders[i].UnLoad();
					}
				}
				groupMap.Remove(groupName);
			}
		}
		public GameObject CreateGO(int id,Transform parent,bool active)
		{
			ResID resID = ResID.New(id);
			string assetName = resID.assetName;
			if(pool.ContainsKey(assetName))
			{
				List<GameObject> goes = pool[assetName];
				if(goes!=null && goes.Count > 0)
				{
					GameObject go = goes[0];
					go.transform.SetParent(parent);
					go.SetActive(active);
					goes.Remove(go);
					if(goes==null || goes.Count == 0)
					{
						pool.Remove(assetName);
					}
					return go;
				}
			}
			List<string> groups;
			if (assetsMap.TryGetValue(resID,out groups))
			{
				if(groups.Count>0)
				{
					string groupName = groups[0];
					if(groupMap.ContainsKey(groupName))
					{
						List<IAssetLoader> loaders = groupMap[groupName];
						if(loaders.Count>0)
						{
							IAssetLoader loader = groupMap[name].Find(a=>a.ResID.Equals(name));
							GameObject prefab = loader.Load<GameObject>();
							GameObject go = GameObject.Instantiate(prefab);
							go.transform.SetParent(parent);
							go.SetActive(active);
							return go;
						} 
						
					}
				}
			}
			return null;
		}

		public void Recycle(GameObject go)
		{
			if(pool.ContainsKey(go.name))
			{
				List<GameObject> goes = pool[name];
				if(goes!=null)
				{
					goes.Add(go);
				}
				else
				{
					goes = new List<GameObject>(){go};
				}
			}
			else
			{
				pool.Add(go.name,new List<GameObject>(){go});
			}
		}
		public void Destroy(GameObject go)
		{
			GameObject.DestroyImmediate(go);
		}

    }
}
