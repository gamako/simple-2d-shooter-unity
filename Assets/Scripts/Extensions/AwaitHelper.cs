using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections;
using UniRx;

namespace MyExt {
    public static class AwaitHelper
    {
        // await Task.Delay()の代わりに使う
        // WebGLだとThread.Createのエラーが出てしまうため
        public static async Task Delay(float time) {
            await Observable.Timer(TimeSpan.FromSeconds(time));
        }
        public static async Task Delay(float time, CancellationToken token) {
            await Observable.Timer(TimeSpan.FromSeconds(time));
                // .FirstOrDefault()
                // .ToAwaitableEnumerator(token);
            
            if (token != null && token.IsCancellationRequested) {
                throw new TaskCanceledException();
            }
        }
    }
}