using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using System.Threading;


public partial class Test
{
	public Button btnTryFinalize;

	public Button btnTryFinalize2;

	void DoTryFinalize()
	{
		btnTryFinalize.enabled = false;
		void finalize()
		{
			btnTryFinalize.enabled = true;
		}
		TaskTryFinalize().ContinueWith(finalize).Forget((ex) =>
		{
			Debug.Log($"DoTryFinalize exception {ex}");
			finalize();
		});
	}



	

	async UniTask TaskTryFinalize()
	{
		int cnt = 0;
		while (true)
		{
			Debug.Log($"{nameof(TestStopTask1)} {cnt++}");

			await UniTask.Delay(1000);

			if (cnt >= 5)
			{
				throw new System.Exception("Test exception");
			}
		}
	}

	void DoTryFinalize2()
	{
		btnTryFinalize2.enabled = false;
		TaskTryFinalize2().Forget();
	}





	async UniTask TaskTryFinalize2()
	{
		try
		{
			int cnt = 0;
			while (true)
			{
				Debug.Log($"{nameof(TestStopTask1)} {cnt++}");

				await UniTask.Delay(1000);

				if (cnt >= 5)
				{
					throw new System.Exception("Test exception");
				}
			}
		}
		catch (System.Exception e)
		{
			Debug.Log($"TaskTryFinalize2 exception {e}");
		}
		finally
		{
			Debug.Log($"TaskTryFinalize2 finally");
			btnTryFinalize2.enabled = true;
		}
		
	}
}

