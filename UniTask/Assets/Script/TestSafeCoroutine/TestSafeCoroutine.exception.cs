using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using UnityEngine.SceneManagement;
using System.Threading;


public partial class TestSafeCoroutine
{
	public Button btnTryFinalize;

	void DoTryFinalize()
	{
		btnTryFinalize.enabled = false;
		void finalize()
		{
			btnTryFinalize.enabled = true;
		}
		this.StartCoroutineSafely(TaskTryFinalize(), onFinish: b =>
		{
			Debug.Log($"DoTryFinalize onFinish {b}");
			finalize();
		}, onException: ex =>
		{
			Debug.Log($"DoTryFinalize exception {ex}");
			finalize();
		});
	}



	

	IEnumerator TaskTryFinalize()
	{
		int cnt = 0;
		while (true)
		{
			Debug.Log($"{nameof(TaskTryFinalize)} {cnt++}");

			yield return new WaitForSeconds(1);

			if (cnt >= 5)
			{
				throw new System.Exception("Test exception");
			}
		}
	}







	
}

