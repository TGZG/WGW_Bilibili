using System;//Action
using System.Text;
using UnityEngine;//Mono
using static CMKZ.LocalStorage;

namespace CMKZ {
    public class UnityDebug : IDebug {
        public string 堆栈信息() => StackTraceUtility.ExtractStackTrace();
        public void Log(string X) => Debug.Log(X);
        public void LogWarning(string X) => Debug.LogWarning(X);
        public void LogError(string X) => Debug.LogError(X);
        public void OnQuit(Action X) => OnAppQuit(X);
        public string 系统信息() {
            var A = new StringBuilder();
            A.AppendLine("CPU：" + SystemInfo.processorType + " " + SystemInfo.processorCount + "核");
            A.AppendLine("显卡：" + SystemInfo.graphicsDeviceName + " " + (SystemInfo.graphicsMemorySize / 1024 + 1) + "GB");
            A.AppendLine("内存：" + (SystemInfo.systemMemorySize / 1024 + 1) + "GB");
            A.AppendLine("操作系统：" + SystemInfo.operatingSystem);
            return A.ToString();
        }
        public string 文件路径() => Application.dataPath + "/Save/";
        [初始化函数(typeof(CMKZProject), 初始化优先级.引擎库)]
        public static void InitIDebug() {
            IVsOrUnity<IDebug>.Single = new UnityDebug();
            IVsOrUnity<ILife>.Single = new UnityLife();
        }
    }
}