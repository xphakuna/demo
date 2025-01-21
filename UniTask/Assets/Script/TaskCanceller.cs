using System.Threading;
using UnityEngine;

public class TaskCanceller : MonoBehaviour
{
	public CancellationTokenSource DisableCanceller = new CancellationTokenSource();
	void OnEnable()
    {
		if (DisableCanceller != null)
		{
			DisableCanceller.Dispose();
		}

		DisableCanceller = new CancellationTokenSource();
	}

    void OnDisable()
    {
		if (DisableCanceller != null)
		{
			DisableCanceller.Cancel();
			DisableCanceller.Dispose();
			DisableCanceller = null;
		}
	}

	private void OnDestroy()
	{
		if (DisableCanceller != null)
		{
			DisableCanceller.Cancel();
			DisableCanceller.Dispose();
			DisableCanceller = null;
		}
	}
}
