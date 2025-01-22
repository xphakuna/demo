using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

// https://www.linkedin.com/pulse/unity-async-vs-coroutine-jo%C3%A3o-borks

public partial class Test : MonoBehaviour
{
	public Button btnCoroutine;
	public Button btnUniTask;
	public Button btnNet;


	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		btnCoroutine.onClick.AddListener(() =>
		{
			StartCoroutine(CoTask());

		});
		btnUniTask.onClick.AddListener(() =>
		{
			FuncUniTask().Forget();

		});
		btnNet.onClick.AddListener(() =>
		{
			FuncUniNet().Forget();
		});

		btnStopTask1.onClick.AddListener(() =>
		{
			TestStopTask1();
		});

		btnCancelDisable.onClick.AddListener(() =>
		{
			TestCancelDisable();
		});

		btnTryFinalize.onClick.AddListener(() =>
		{
			DoTryFinalize();
		});
	}

	IEnumerator CoTask()
	{
		Debug.Log($"{nameof(CoTask)} 0");
		yield return new WaitForSeconds(1);
		try
		{
			FuncWithException();
		}
		catch (System.Exception e)
		{
			Debug.Log($"{nameof(CoTask)} exception {e.Message}");
		}
		yield return new WaitForSeconds(1);
		Debug.Log($"{nameof(CoTask)} 9");
	}

	async UniTask FuncUniTask()
	{
		try
		{
			Debug.Log($"{nameof(UniTask)} 0");
			await UniTask.Delay(1000);

			FuncWithException();

			await UniTask.Delay(1000);
			Debug.Log($"{nameof(UniTask)} 9");
		}
		catch (System.Exception e)
		{
			Debug.Log($"{nameof(UniTask)} exception {e.Message}");
		}
	}


	void FuncWithException()
	{
		throw new System.Exception("Exception");
	}



	async UniTask FuncUniNet()
	{
		var task = GameNet.I.SendAsync(new SomeReq { seq=11});
		var ack = await task;
		if (ack == null)
			return;

		Debug.Log($"ack {ack.seq}");
	}

	// Update is called once per frame
	void Update()
	{

	}
}

