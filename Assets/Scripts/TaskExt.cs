using System;
using System.Threading;
using System.Threading.Tasks;

namespace MyExt {
    public static class TaskExt
    {
        public static async Task Delay(float time) {
            await Task.Delay(TimeSpan.FromMilliseconds(1000 * time));
        }
        public static async Task Delay(float time, CancellationToken token) {
            await Task.Delay(TimeSpan.FromMilliseconds(1000 * time), token);
        }
    }
}