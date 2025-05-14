using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using static CMKZ.LocalStorage;

namespace CMKZ {
    public class UnityLife : ILife {
        public void OnAppError(Action<string> X) {
            Application.logMessageReceived += (string logString, string stackTrace, LogType type) => {
                if (type == LogType.Exception) {
                    X($"{logString}\n{stackTrace}");
                }
            };
            TaskScheduler.UnobservedTaskException += (sender, args) => {
                X(args.Exception.Message);
                args.SetObserved(); // 防止应用程序崩溃
            };
        }
        public void OnAppFocus(Action X) {
            MainThread.OnFoucus += X;
        }
        public void OnAppFocusOut(Action X) {
            MainThread.OnFoucusOut += X;
        }
        public void OnAppQuit(Action X) {
            MainThread.OnQuit += X;
        }
        public void OnAppUpdate(Action X) {
            if (MainThread == null) throw new Exception("请先初始化主线程");
            MainThread.OnUpdate += X;
        }
        public void OnAppUpdate(string X, Action<float> Y) {
            if (MainThread.onAppKeyUpdate.ContainsKey(X)) throw new Exception("已存在的事件：" + X);
            MainThread.onAppKeyUpdate[X] = Y;
        }
        public void RemoveAppUpdate(string X) {
            if (!MainThread.onAppKeyUpdate.ContainsKey(X)) throw new Exception("已存在的事件：" + X);
            MainThread.需要删除的事件.Enqueue(X);
        }
    }
    public static partial class LocalStorage {
        public static AppThread MainThread;
        [初始化函数(typeof(CMKZProject), 初始化优先级.引擎库)]
        public static void InitMainThread() => MainThread = MainPanel.AddComponent<AppThread>();
    }
    public class AppThread : MonoBehaviour {
        public Queue<Action> 执行一次 = new();
        public Action OnQuit;
        public Action OnFoucus;
        public Action OnFoucusOut;
        public Action OnUpdate;
        public Dictionary<string, Action<float>> onAppKeyUpdate = new();
        public Queue<string> 需要删除的事件 = new();
        public void Update() {
            lock (执行一次) {
                while (执行一次.Count > 0) {
                    执行一次.Dequeue().Invoke();
                }
                OnUpdate?.Invoke();
                foreach (var i in onAppKeyUpdate.Keys) onAppKeyUpdate[i]?.Invoke(1000 / 16);
                while (需要删除的事件.Count > 0) onAppKeyUpdate.RemoveKey(需要删除的事件.Dequeue());
            }
        }
        public void OnApplicationQuit() {
            OnQuit?.Invoke();
        }
        public void OnApplicationFocus(bool X) {
            if (X) {
                OnFoucus?.Invoke();
            } else {
                OnFoucusOut?.Invoke();
            }
        }
        public void Exec(Action action) {
            lock (执行一次) {
                执行一次.Enqueue(action);
            }
        }
    }
}