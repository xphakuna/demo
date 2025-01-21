using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using System.Threading;


public partial class Test
{
	public Button btnCancelDisable;

	void TestCancelDisable()
	{
		var mono = GetComponent<TaskCanceller>();
		if (mono == null)
		{
			mono = gameObject.AddComponent<TaskCanceller>();
		}
		TesCancelDisable(mono.DisableCanceller.Token).Forget();
	}

	async UniTask TesCancelDisable(CancellationToken token)
	{
		int cnt = 0;
		while (true)
		{
			await UniTask.Delay(1000, cancellationToken: token, cancelImmediately: false);
			if (token.IsCancellationRequested)
			{
				Debug.Log($"{nameof(TestStopTask1)} cancelled frame: {Time.frameCount}");
				return;
			}
			Debug.Log($"{nameof(TestStopTask1)} {cnt++}");

			if (cnt >= 5)
			{
				break;
			}
		}
	}
}

