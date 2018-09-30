using System;
using System.Collections;
using System.Collections.Generic;
using Mango.Framework.Resource;
using UnityEngine;
using UnityEngine.UI;

namespace Mango.Framework.UI.Component
{
	[RequireComponent(typeof(RawImage))]
	public class UITexture : UIComponent 
	{
		private RawImage rwImg;
		public RawImage RwImg
		{
			get
			{
				if(rwImg)
				{
					rwImg = GetComponent<RawImage>();
				}
				return rwImg;
			}
		}
		[SerializeField]
		private string texName;
		private IAssetLoader loader;
		protected override void Prepare(Action onFinished)
		{
			if(!rwImg.texture || texName.Equals(rwImg.texture.name))
			{
				StartCoroutine(LoadTexture(onFinished));
			}
			else
			{
				if(onFinished==null)
				{
					onFinished();
				}
			}

		}

		private IEnumerator LoadTexture(Action onFinished)
		{
			IAssetLoader loaderTmp = ResourceModule.Instance.Get(texName);
			IAssetAsynRequest request = loaderTmp.LoadAsyn();
			yield return request;
			Texture tex = request.GetAsset<Texture>();
			rwImg.texture = tex;
			if(loader!=null)
			{
				this.loader.Recycle();
			}
			this.loader = loaderTmp;
			if(onFinished==null)
			{
				onFinished();
			}
		}
		
		public void SetSpriteByName(string texName)
		{
			if(!rwImg.texture || texName.Equals(rwImg.texture.name))
			{
				this.texName = texName;
				StartCoroutine(LoadTexture(null));
			}
		}

		void OnDestroy()
		{
			if(loader!=null)
			{
				this.loader.Recycle();
			}
		}
	}
}

