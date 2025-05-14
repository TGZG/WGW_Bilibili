using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using static CMKZ.LocalStorage;

namespace CMKZ {
    public static partial class LocalStorage {
        public static void StartCoroutines(IEnumerator X) {
            if (MainThread == null) throw new Exception("请先初始化主线程");
            MainThread.Exec(() => MainThread.StartCoroutine(X));
        }
        public static void 多线程(WaitCallback A) {
            ThreadPool.QueueUserWorkItem(A);
        }
        public static void 主线程单次(Action A) {
            MainThread.Exec(A);
        }
    }
}