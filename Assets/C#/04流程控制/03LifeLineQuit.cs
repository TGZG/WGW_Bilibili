using System;//Action
using System.Collections.Generic;
using System.Linq;//from XX select XX
using System.Reflection;
using static CMKZ.LocalStorage;
using System.Windows;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace CMKZ {
    public interface ILife : IVsOrUnity<ILife> {
        public void OnAppUpdate(Action X);
        public void OnAppUpdate(string X, Action<float> Y);
        public void RemoveAppUpdate(string X);
        public void OnAppQuit(Action X);
        public void OnAppFocus(Action X);
        public void OnAppFocusOut(Action X);
        public void OnAppError(Action<string> X);
    }
    public static partial class LocalStorage {
        public static void OnAppUpdate(Action X) { //每帧执行
            IVsOrUnity<ILife>.Single.OnAppUpdate(X);
        }
        public static void OnAppUpdate(string X, Action<float> Y) { //每帧执行
            IVsOrUnity<ILife>.Single.OnAppUpdate(X, Y);
        }
        public static void RemoveAppUpdate(string X) {
            IVsOrUnity<ILife>.Single.RemoveAppUpdate(X);
        }
        public static void OnAppQuit(Action X) {
            IVsOrUnity<ILife>.Single.OnAppQuit(X);
        }
        public static void OnAppFocus(Action X) {
            IVsOrUnity<ILife>.Single.OnAppFocus(X);
        }
        public static void OnAppFocusOut(Action X) {
            IVsOrUnity<ILife>.Single.OnAppFocusOut(X);
        }
        public static void OnAppChange(Func<bool> 条件, Action 进入时, Action 离开时) {
            bool 上次 = false;
            OnAppUpdate(() => {
                bool 这次 = 条件();
                if (上次 != 这次) {
                    if (这次) {
                        进入时?.Invoke();
                    } else {
                        离开时?.Invoke();
                    }
                }
                上次 = 这次;
            });
        }
    }
    public class VsConsoleLife : ILife {
        private Action onAppUpdate;
        private Action onAppFocus;
        private Action onAppFocusOut;
        private Dictionary<string, Action<float>> onAppKeyUpdate = new();
        private Queue<string> 需要删除的事件 = new();
        private bool IsInit;
        public VsConsoleLife() {
            //Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            StartThread(() => {
                while (true) {
                    if (!IsInit) {
                        IsInit = true;
                        var A = GetForegroundWindow();//不使用 GetConsoleWindow();
                        OnAppChange(() => GetForegroundWindow() == A, onAppFocus, onAppFocusOut);
                    }
                    onAppUpdate?.Invoke();
                    foreach (var i in onAppKeyUpdate.Keys) onAppKeyUpdate[i]?.Invoke(1000 / 16);
                    while (需要删除的事件.Count > 0) onAppKeyUpdate.RemoveKey(需要删除的事件.Dequeue());
                    Thread.Sleep(16);//60帧
                }
            });
        }
        public void OnAppUpdate(Action X) {
            onAppUpdate += X;
        }
        public void OnAppUpdate(string X, Action<float> Y) {
            if (onAppKeyUpdate.ContainsKey(X)) throw new Exception("已存在的事件：" + X);
            onAppKeyUpdate[X] = Y;
        }
        public void RemoveAppUpdate(string X) {
            if (!onAppKeyUpdate.ContainsKey(X)) throw new Exception("已存在的事件：" + X);
            需要删除的事件.Enqueue(X);
        }
        public void OnAppQuit(Action X) {
            AppDomain.CurrentDomain.ProcessExit += (sender, args) => X?.Invoke();
        }
        public void OnAppError(Action<string> X) {
            //Application.ThreadException += (sender, args) => X(args.Exception.Message);
            AppDomain.CurrentDomain.UnhandledException += (sender, args) => X((args.ExceptionObject as Exception)?.Message);
            TaskScheduler.UnobservedTaskException += (sender, args) => {
                X(args.Exception.Message);
                args.SetObserved();//防止程序崩溃
            };
        }
        public void OnAppFocus(Action X) {
            onAppFocus += X;
        }
        public void OnAppFocusOut(Action X) {
            onAppFocusOut += X;
        }
    }
}