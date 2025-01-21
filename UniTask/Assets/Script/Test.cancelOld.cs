using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using System.Threading;


public partial class Test
{
	public Button btnStopTask1;

	CancellationTokenSource cts_stopTask = null;
	CancellationTokenSource cts_linkScource = null;

	// safe code
	void TestStopTask1_Dispose()
	{
		if (cts_stopTask != null)
		{
			Debug.Log($"{nameof(TestStopTask1)} call Dispose frame: {Time.frameCount}");
			cts_stopTask.Cancel();
			cts_stopTask.Dispose();
			cts_stopTask = null;
		}

		if (cts_linkScource != null)
		{
			cts_linkScource.Cancel();
			cts_linkScource.Dispose();
			cts_linkScource = null;
		}
	}

	void TestStopTask1()
	{

		TestStopTask1_Dispose();

		Debug.Log($"{nameof(TestStopTask1)} call start frame: {Time.frameCount}");
		cts_stopTask = new CancellationTokenSource();
		var mono = GetComponent<TaskCanceller>();
		if (mono == null)
		{
			mono = gameObject.AddComponent<TaskCanceller>();
		}
		cts_linkScource = CancellationTokenSource.CreateLinkedTokenSource(cts_stopTask.Token, mono.DisableCanceller.Token);
		//
		TestStopTask1(cts_linkScource.Token).Forget();
	}



	

	async UniTask TestStopTask1(CancellationToken token)
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

	private void OnDestroy()
	{
		// safe code
		TestStopTask1_Dispose();
	}
}

