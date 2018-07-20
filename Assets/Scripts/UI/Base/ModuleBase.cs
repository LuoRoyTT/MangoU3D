using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

namespace Client.UI
{
    public class ModuleBase : UIContainer
    {
        private Dictionary<string,ViewBase> viewDict = new Dictionary<string,ViewBase>();
        private Dictionary<string,ViewModelBase> modelDict=new Dictionary<string,ViewModelBase>();
        public void OpenView<K,V>() where K:ViewBase  where V:ViewModelBase  
        {
            string viewName =  typeof(K).Name;
            if(viewDict.ContainsKey(viewName))
            {
                ShowView(viewName);
            }
            else
            {
                //TODO 加载view的prefab
                GameObject viewGO=new GameObject();
                K view = viewGO.GetComponent<K>();
                viewDict.Add(viewName,view);
                string modelName = typeof(V).Name;
                if(modelDict.ContainsKey(modelName))
                {
                    V model = modelDict[modelName] as V;
                    view.Model = model;
                }
                else
                {
                    V model = new ViewModelBase(this) as V;
                    view.Model = model;
                }
                ShowView(viewName);
            } 
        }

        public void ShowView(string viewName)
        {

        }
        
        public bool IsModelLoaded(string modelName)
        {
            return modelDict.ContainsKey(modelName);
        }

        public ViewModelBase GetModelFromDic(string modelName)
        {
            return modelDict[modelName];
        }

        public void AddModelToDic(ViewModelBase model)
        {
            modelDict.Add(model.GetType().Name,model);

        }
    }
}
