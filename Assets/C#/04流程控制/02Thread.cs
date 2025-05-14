using System;
using System.Diagnostics;
using System.Threading;

namespace CMKZ {
    public static partial class LocalStorage {
        public static void StartThread(ThreadStart X) {
            new Thread(X) { IsBackground = true }.Start();
        }
        public static void SleepFor(int 毫秒, Action X) {
            Stopwatch stopwatch = Stopwatch.StartNew();
            while (stopwatch.Elapsed.TotalSeconds < 3) {
                Thread.Sleep(100);
            }
            stopwatch.Stop();
            X();
        }
    }
}