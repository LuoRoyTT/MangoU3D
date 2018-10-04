using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mango.Framework.Coroutine
{
	public interface ICoroutine
	{
		MCoroutine AppendCoroutine(IEnumerator it);
		void RemoveCoroutine(IEnumerator it);
		void RemoveAllCoroutine();

	}
}

