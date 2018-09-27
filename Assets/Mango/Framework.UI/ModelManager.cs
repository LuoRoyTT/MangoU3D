using System.Collections;
using System.Collections.Generic;
using Mango.Framework.Core;
using Mango.Framework.Resource;
using UnityEngine;

namespace Mango.Framework.UI
{
	public class ModelManager : MonoSingleton<ModelManager> 
	{
		private Dictionary<string,ViewModelBase> modelMap;
		private ViewModelBase currentModel;
		public ViewModelBase CurrentModel
		{
			get
			{
				return currentModel;
			}
		}
		public void ChangeModel(string modelName)
		{
			ViewModelBase model = GetModel(modelName);
			model.OpenView(model.MainView);
			currentModel.HideAllView();
			currentModel = model;
		}
		private ViewModelBase GetModel(string modelName)
		{
			return new ViewModelBase();
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

