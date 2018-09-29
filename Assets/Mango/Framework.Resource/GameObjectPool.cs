using System;
using System.Collections;
using System.Collections.Generic;
using Mango.Framework.Core;
using UnityEngine;

namespace Mango.Framework.Resource
{
	public class GameObjectPool : MonoSingleton<GameObjectPool> 
	{
		Dictionary<string,List<string>> assetsMap;
		Dictionary<string,List<IAssetLoader>> groupMap;
		Dictionary<string,List<GameObject>> pool;
		private bool loading;

		public void PreloadAsset(string groupName,Action<float> update,Action onFinished,params string[] assets)
		{
			if(loading) return;
			if(String.IsNullOrEmpty(groupName)) return;
			if(assets==null) return;
			loading = true;
			HashSet<string> assetsHashSet = null;
			List<IAssetLoader> loaders = null;
			for (int i = 0; i < assets.Length; i++)
			{
				assetsHashSet.Add(assets[i]);
			}
			foreach (string asset in assetsHashSet)
			{
				loaders.Add(ResourceModule.Instance.Get(asset));
			}
			int index = 0;
			StartCoroutine(DoPreloadAsset(groupName,loaders,onFinished,index));
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
					string asset = loaders[i].AssetName;
					if(assetsMap.ContainsKey(asset))
					{
						assetsMap[asset].Add(groupName);
					}
					else
					{
						assetsMap.Add(asset,new List<string>{groupName});
					}
				}
				loading = false;
				if(onFinished!=null) onFinished();
			}
			else
			{
				StartCoroutine(DoPreloadAsset(groupName,loaders,onFinished,index));
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
						if(assetsMap.ContainsKey(loaders[i].AssetName))
						{
							List<string> groups = assetsMap[loaders[i].AssetName];
							if(groups!=null && groups.Count>0)
							{
								groups.RemoveAt(groups.Count-1);
							}
							if(groups==null || groups.Count==0)
							{
								assetsMap.Remove(loaders[i].AssetName);
								pool.Remove(loaders[i].AssetName);
							}
						}
						loaders[i].Recycle();
					}
				}
				groupMap.Remove(groupName);
			}
		}
		public GameObject CreateGO(string name,Transform parent,bool active)
		{
			if(pool.ContainsKey(name))
			{
				List<GameObject> goes = pool[name];
				if(goes!=null && goes.Count > 0)
				{
					GameObject go = goes[0];
					go.transform.SetParent(parent);
					go.SetActive(active);
					goes.Remove(go);
					if(goes==null || goes.Count == 0)
					{
						pool.Remove(name);
					}
					return go;
				}
			}
			List<string> groups;
			if (assetsMap.TryGetValue(name,out groups))
			{
				if(groups.Count>0)
				{
					string groupName = groups[0];
					if(groupMap.ContainsKey(groupName))
					{
						List<IAssetLoader> loaders = groupMap[groupName];
						if(loaders.Count>0)
						{
							IAssetLoader loader = groupMap[name].Find(a=>a.AssetName.Equals(name));
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
			if(pool.ContainsKey(name))
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
