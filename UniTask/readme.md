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