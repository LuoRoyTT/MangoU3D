using UnityEngine;
using System.Collections;

namespace Client.Core
{
	public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
	{
		private static T instance = null;
		public static T Instance {
			get {
				if (instance == null)
				{
					instance = FindObjectOfType(typeof(T)) as T;
					if (instance == null)
					{
						instance = new GameObject("_" + typeof(T).Name).AddComponent<T>();
						DontDestroyOnLoad(instance);
                        instance.Init();

                    }
					if (instance == null)
						Debug.LogError("Failed to create instance of " + typeof(T).FullName + ".");
				}
				return instance;
			}
		}
		
		void OnApplicationQuit () { if (instance != null) instance = null; }
		
		public  T CreateInstance ()
		{
			if (Instance != null) Instance.Init();
			return Instance;
		}
		
		protected virtual void Init ()
		{
			
		}

        protected virtual void Awake()
        {
            CreateInstance();
        }
	}
}