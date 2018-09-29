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
			model.Enter(()=>
			{
				currentModel.Exit();
				currentModel = model;
			});

		}
		private ViewModelBase GetModel(string modelName)
		{
			return new ViewModelBase();
		}
	}
}

