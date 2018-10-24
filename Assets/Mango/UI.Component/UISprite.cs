using System;
using System.Collections;
using System.Collections.Generic;
using Mango.Framework.Resource;
using UnityEngine;
using UnityEngine.UI;

namespace Mango.Framework.UI.Component
{
	[RequireComponent(typeof(Image))]
	public class UISprite : UIComponent 
	{
		private Image img;
		public Image Img
		{
			get
			{
				if(img)
				{
					img = GetComponent<Image>();
				}
				return img;
			}
		}
		[SerializeField]
		private string spriteName;
		private IAssetLoader loader;
		protected override void Prepare(Action onFinished)
		{
			if(!Img.sprite || spriteName.Equals(Img.sprite.name))
			{
				StartCoroutine(LoadSprite(onFinished));
			}
			else
			{
				if(onFinished==null)
				{
					onFinished();
				}
			}

		}

		private IEnumerator LoadSprite(Action onFinished)
		{
			IAssetLoader loaderTmp = ResourceModule.instance.Get(spriteName);
			IAssetAsynRequest request = loaderTmp.LoadAsyn();
			yield return request;
			Sprite sprite = request.GetAsset<Sprite>();
			img.sprite = sprite;
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
		
		public void SetSpriteByName(string spriteName)
		{
			if(!Img.sprite || spriteName.Equals(Img.sprite.name))
			{
				this.spriteName = spriteName;
				StartCoroutine(LoadSprite(null));
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

