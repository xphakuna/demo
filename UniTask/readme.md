# coroutine vs UniTask vs system的async
- https://www.linkedin.com/pulse/unity-async-vs-coroutine-jo%C3%A3o-borks 有更详细的对比
- coroutine：unity自带的携程，最简单，但是不支持的功能比较多
- UniTask: 需要单独安装库，0GC，支持的功能比较多，跟system的async比，提供了很多Unity友好的扩展跟接口
- system的async：不需要安装，支持的功能比较多
# UniTask支持的功能比较多，哪些是实用的功能？
- try catch，coroutine不能在try的block里面yield，例如下面会编译报错try { yield return null; }
- UniTask可以方便的WaitAll跟WaitAny
- UniTask可以方便的cancel
- UniTask可以在Editor模式使用
- UniTask的启动不用依赖monobehaviour，直接启动
- # UniTask 取消比较麻烦
  coroutine比较简单
  ```
  var coutine = StartCoroutine(somefunc());
  // want to stop
  StopCoutine(coutine);
  ```
  Task需要用CancellationTokenSource。在task里需要判断，如果取消就不继续执行。
    ```
  var coutine = StartCoroutine(somefunc());
  // want to stop
    async UniTask TestStopTask1(CancellationToken token)
    {
        await someTask(token);
        if (token.IsCancellationRequested)
        {
            return;
        }
        await someTask2(token);
        if (token.IsCancellationRequested)
        {
            return;
        }
    }
  ```
  上面的例子可以看出需要加很多的判断，代码很乱，暂时没想到好的方法。
- # UniTask模式1，同一个任务执行多次，不取消上次的任务。unity中在disable的时候要取消
```
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
	// some func
}
```
- # UniTask模式2，同一个任务执行多次，取消上次的任务
```
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
	// some func
}
```