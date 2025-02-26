using System;
using System.Collections;
using UnityEngine;


public static class SafeCoroutine
{
	private class Context
	{
		/// <summary>
		/// 参数：是否顺利无异常
		/// </summary>
		public Action<bool> onFinish;
		public Action<Exception> onException;
		public bool hasException;
	}

	public static Coroutine StartCoroutineSafely(
		this MonoBehaviour mono, IEnumerator enu,
		Action<bool> onFinish = null,
		Action<Exception> onException = null)
	{
		if (mono == null) throw new ArgumentNullException(nameof(mono), "MonoBehaviour为空");
		var ctx = new Context
		{
			onFinish = onFinish,
			onException = onException,
			hasException = false
		};
		return mono.StartCoroutine(DoCoroutine(enu, ctx));
	}

	private static IEnumerator DoCoroutine(IEnumerator enu, Context ctx)
	{
		yield return SafeIterate(enu, ctx);
		ctx.onFinish?.Invoke(!ctx.hasException);
	}

	private static IEnumerator SafeIterate(IEnumerator enu, Context ctx)
	{
		while (true)
		{
			object current;
			try
			{
				var done = !enu.MoveNext();
				if (done) break;
				current = enu.Current;
			}
			catch (Exception exc)
			{
				Debug.LogError("[SafeCoroutine]枚举器内部异常：" + exc);
				Debug.LogException(exc);
				ctx.onException?.Invoke(exc);
				ctx.hasException = true;
				yield break;
			}

			if (current is IEnumerator subEnu)
			{
				yield return SafeIterate(subEnu, ctx);
				if (ctx.hasException)
				{
					yield break;
				}
			}
			else
			{
				yield return current;
			}
		}
	}
}
