using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

internal class GameNet
{
	public static GameNet I = new GameNet();



	public async UniTask<SomeAck> SendAsync(SomeReq req, float timeout = 4)
	{
		// reconnect
		var cts = new System.Threading.CancellationTokenSource();
		cts.CancelAfterSlim(System.TimeSpan.FromSeconds(timeout)); // 超时
																   //
		try
		{
			var task = await sendAsync(req);
			return task;
		}
		catch (System.Exception e)
		{
			Debug.Log($"{nameof(UniTask)} exception {e.Message}");
			return null;
		}
	}

	async UniTask<SomeAck> sendAsync(SomeReq req)
	{
		Send(req);
		await UniTask.Delay(1000);
		return new SomeAck { seq = req.seq + 1 };
	}

	public void Send(SomeReq req)
	{

	}
}

public class SomeReq
{
	public int seq;
}

public class SomeAck
{
	public int seq;
}

