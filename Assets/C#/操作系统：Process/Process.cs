using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace CMKZ {
    public static partial class LocalStorage {
        public static Process SelfProcess;
        [初始化函数(typeof(CMKZProject))]
        public static void GetSelfProcess() {
            SelfProcess = Process.GetCurrentProcess();
        }
        /// <![CDATA[
        /// StartProcess("C:/XX.exe");
        /// ]]>
        public static void StartProcess(string X, bool 管理员权限 = false) {
            Process.Start(new ProcessStartInfo {
                FileName = X,
                UseShellExecute = 管理员权限,
            });
        }
        public static void StopSelf() {
            Environment.Exit(0);
        }
        public static void StopProcess(string X) {
            foreach (var i in Process.GetProcessesByName(X)) {
                i.Kill();
            }
        }
        public static long GetProcessMemory() {
            return SelfProcess.WorkingSet64 / 1024 / 1024;
        }
        public static float GetProcessCPU() {
            return new PerformanceCounter("Process", "% Processor Time", SelfProcess.ProcessName).NextValue();
        }
        public static void LogSystemUsage() {
            Print($"Memory：{GetProcessMemory()}MB，CPU：{GetProcessCPU()}%");
        }
    }
}