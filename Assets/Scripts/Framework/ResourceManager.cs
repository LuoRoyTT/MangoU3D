using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Client.Core;

namespace Client.Framework
{
    public class ResourceManager : MonoSingleton<ResourceManager>
    {
        private Dictionary<string, Dictionary<string, UnityEngine.Object>> cache = new Dictionary<string, Dictionary<string, UnityEngine.Object>>();

        protected override void Awake() { }
        public void CacheAssets(Action<float> onCaching, Action onfinished, params string[] assetNames) { }
        public void CacheAssets(bool isPermanent, string groupName, Action<float> onCaching, Action onfinished, params string[] assetNames)
        {

        }
        public void Clear(bool forceGC, params string[] names) { }
        public void ClearGroup(string groupName, bool forceGC = false, bool withPermanent = false) { }
        public void ClearImpermanent(bool forceGC) { }
        public void ClearTimeOut() { }
        public T CreateObject<T>(string name, bool createActive = false) where T : UnityEngine.Object
        {
            
            return default(T);
        }
        public T GetComponent<T>(string name) where T : Component { return default(T); }
        public void Recycle<T>(T go) where T:UnityEngine.Object
        {

        }
        public T SharedObject<T>(string name) where T : UnityEngine.Object
        {
            return default(T);
        }
        public void WarmupGameObject(string name, int count) { }
    }
}
