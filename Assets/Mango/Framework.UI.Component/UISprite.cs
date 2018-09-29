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
			IAssetLoader loader = ResourceModule.Instance.Get(spriteName);
			IAssetAsynRequest request = loader.LoadAsyn();
			yield return request;
			if(onFinished==null)
			{
				onFinished();
			}
		}
		
		public void SetSpriteByName(string spriteName)
		{
			
		}
	}
}

