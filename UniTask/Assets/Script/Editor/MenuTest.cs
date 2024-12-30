using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class MenuTest
{
	[MenuItem("Test/Test Unitask in Edit Mode", false, 0)]
	public static void Unitask_in_Edit_Mode()
	{
		FuncUniTask().Forget();
	}

	static async UniTask FuncUniTask()
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


	static void FuncWithException()
	{
		throw new System.Exception("Exception");
	}
}
