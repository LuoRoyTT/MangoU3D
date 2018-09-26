using System.Collections;
using System.Collections.Generic;
using Mango.Framework.Core;
using Mango.Framework.Resource;
using UnityEngine;

namespace Mango.Framework.UI
{
	public class UIManager : MonoSingleton<UIManager> 
	{
		
		private List<KeyValuePair<string,string>> kv;

		public ViewBase CurrentView{private set; get;}
		public void OpenSubView(string viewName)
		{

		}

		public void HideView(string viewName)
		{

		}
		public void ChangeMainView(string viewName)
		{

		}

		private IEnumerator LoadView(string viewName)
		{
			IAssetLoader loader = ResourceModule.Instance.Get(viewName);
			IAssetAsynRequest request = loader.LoadAsyn();
			yield return request;
			GameObject viewGO = request.GetAsset<GameObject>();
		}
	}
}

