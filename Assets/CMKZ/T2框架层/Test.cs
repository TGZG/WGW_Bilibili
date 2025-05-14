using System;//Action
using System.Diagnostics;
using System.Linq;
using UnityEngine;//Mono
using UnityEngine.UI;
using Debug=UnityEngine.Debug;

namespace CMKZ {
    public class WaitForCondition : CustomYieldInstruction {
        private Func<bool> _condition;
        public WaitForCondition(Func<bool> condition) {
            _condition = condition;
        }
        // 当keepWaiting返回false时，等待结束
        public override bool keepWaiting {
            get {
                return !_condition();
            }
        }
    }
    //旧的
    public static partial class LocalStorage {
        public static void VSLog(string Path) {
            AppDomain.CurrentDomain.UnhandledException += (object sender, UnhandledExceptionEventArgs e) => {
                Exception ex = (Exception)e.ExceptionObject;
                FileWrite(Path, "[" + DateTime.Now.ToString() + "] " + ex.Message + "\n" + ex.StackTrace);
            };
        }
        /// <summary>
        /// 输入路径，设置Unity的日志信息储存位置。
        /// </summary>
        public static void UnityLog(string Path) {
            Application.logMessageReceived += (string logString, string stackTrace, LogType type) => {
                if (type is LogType.Error or LogType.Exception) {
                    FileWrite(Path, "[" + DateTime.Now.ToString() + "][" + type + "] " + logString + "\n" + stackTrace);
                }
            };
        }
        public static void TryDo(Action X) {
            try {
                X();
            } catch (Exception e) {
                Print(e);
            }
        }
        public static void Test(Action X, int Y = 1000, string Z = "MyTest") {
            var A = new Stopwatch();
            A.Start();
            执行X次(Y, X);
            A.Stop();
            Print($"Test {Z} {A.ElapsedMilliseconds}ms");
        }
        public static void TestTest() {
            var i = 0;
            var A = new TestClass();
            var B = Range(0, 100);
            Test(() => i++, 10000000, "i++");//30
            Test(() => PlayerPrefs.SetInt("Test", 1), 10000, "PlayerPrefs");//62
            Test(() => Print("Test"), 1000, "Print");//85
            Test(() => UnityEngine.Debug.Log("Test"), 1000, "Debug.Log");//43
            Test(() => FileWrite("C:/Test", "Test"), 1000, "FileWrite");//169
            Test(() => A.SetFieldValue("TestField", 1), 100000, "SetFieldValue");//77
            Test(() => A.GetFieldValue("TestField"), 100000, "GetFieldValue");//63
            Test(() => B.Where(t => t > 50).ToList(), 10000, "Where");//27
            Test(() => new GameObject().AddComponent<Image>().gameObject.Destroy(true), 1000, "GameObject");//95
        }
        public static void ErrorLog(Exception e, string X) => TextAppend(X, NowTime + e.Message + "\n" + e.StackTrace);
    }
    public class TestClass {
        public int TestField;
    }
}