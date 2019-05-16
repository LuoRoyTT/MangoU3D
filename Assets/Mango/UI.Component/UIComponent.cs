using System;
using System.Collections;
using System.Collections.Generic;
using Mango.Task;
using UnityEngine;

namespace Mango.UI.Component
{
	public class UIComponent : MonoBehaviour
	{
		private Transform cachedTransform;
		public Transform CachedTransform
		{
			get
			{
				if(cachedTransform==null)
				{
					cachedTransform = transform;
				}
				return cachedTransform;
			}
		}

		private GameObject cachedGameObject;
		public GameObject CachedGameObject
		{
			get
			{
				if(cachedGameObject==null)
				{
					cachedGameObject = gameObject;
				}
				return cachedGameObject;
			}
		}
		public string Name
		{
			get
			{
				return CachedGameObject.name;
			}
		}
		protected UIContainer belongedContainer;
		protected bool initialized;

		public void Initialize(UIContainer container)
		{
			if(initialized) return;
			this.belongedContainer = container;
			Prepare(OnInitCommponentFinished);
		}
		protected virtual void Prepare(Action onFinished)
		{

		}
		private void OnInitCommponentFinished()
		{
			initialized = true;
			belongedContainer.ReceiveInitializedMsg();
		}
		public virtual void Init(){}
		public virtual void Appear(){}
		public virtual void Hide(){}
		public virtual void Dispose(){}
		public virtual void Reset(){}
	}
}
