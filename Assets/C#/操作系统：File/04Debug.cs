using System;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Text;

namespace CMKZ {
    public interface IDebug : IVsOrUnity<IDebug> { 
        public string 堆栈信息();
        public void Log(string X);
        public void LogWarning(string X);
        public void LogError(string X);
        public void OnQuit(Action X);
        public string 系统信息();
        public string 文件路径();
    }
    public class VsDebug : IDebug {
        public string 堆栈信息() => new StackTrace().ToString();
        public void Log(string X) => System.Console.WriteLine(X);
        public void LogWarning(string X) => System.Console.WriteLine("Warnning:" + X);
        public void LogError(string X) => System.Console.WriteLine("Error:" + X);
        public void OnQuit(Action X) => AppDomain.CurrentDomain.ProcessExit += (s, e) => X();
        public string 系统信息() {
            var A = new StringBuilder();
            A.AppendLine("操作系统：" + Environment.OSVersion);
            A.AppendLine("处理器：" + Environment.ProcessorCount);
            A.AppendLine("内存：" + Environment.WorkingSet);
            A.AppendLine("启动时间：" + Process.GetCurrentProcess().StartTime);
            A.AppendLine("运行时间：" + Process.GetCurrentProcess().TotalProcessorTime);
            A.AppendLine("线程：" + Process.GetCurrentProcess().Threads.Count);
            A.AppendLine("堆栈：" + Process.GetCurrentProcess().PagedMemorySize64);
            A.AppendLine("路径：" + Process.GetCurrentProcess().MainModule.FileName);
            A.AppendLine("版本：" + Process.GetCurrentProcess().MainModule.FileVersionInfo);
            return A.ToString();
        }
        public string 文件路径() => Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData).RePath() + "/CMKZ/";

    }
    /// <summary>
    /// Tobo：处理Throw
    /// </summary>
    public static partial class LocalStorage {
        public static object DefaultSender = "Debug";
        public static List<LogData> LogList = new();
        public static StreamWriter LogFileWriter;
        public static bool IsLogQuit = false;
        public static int MaxLogFileCount = 1000;
        public static int MinLogFileCount = 100;
        public static LogLevel LogLevel = LogLevel.Debug;
        public static object System = nameof(System);
        public static event Action<LogData> OnPrint = t => {
            if (t.Level == LogLevel.Debug) {
                IVsOrUnity<IDebug>.Single.Log(t.Message);
            } else if (t.Level == LogLevel.Warning) {
                IVsOrUnity<IDebug>.Single.LogWarning(t.Message);
            } else if (t.Level == LogLevel.Error) {
                IVsOrUnity<IDebug>.Single.LogError(t.Message);
            }
            LogList.Add(t);
            LogToFile(t);
        };
        public static void Print(object X, object Sender = null) {
            if (LogLevel > LogLevel.Debug) return;
            OnPrint(new LogData {
                Level = LogLevel.Debug,
                Message = X.ToString(),
                Time = DateTime.Now,
                Sender = Sender ?? DefaultSender,
                Stack = IVsOrUnity<IDebug>.Single.堆栈信息(),
            });
        }
        public static void PrintWarning(string X, object Sender = null) {
            if (LogLevel > LogLevel.Warning) return;
            OnPrint(new LogData {
                Level = LogLevel.Warning,
                Message = X,
                Time = DateTime.Now,
                Sender = Sender ?? DefaultSender,
                Stack = IVsOrUnity<IDebug>.Single.堆栈信息(),
            });
        }
        public static void PrintError(string X, object Sender = null) {
            if (LogLevel > LogLevel.Error) return;
            OnPrint(new LogData {
                Level = LogLevel.Error,
                Message = X,
                Time = DateTime.Now,
                Sender = Sender ?? DefaultSender,
                Stack = IVsOrUnity<IDebug>.Single.堆栈信息(),
            });
        }
        public static void PrintSystem(string X) {
            Print(X, System);
        }
        public static void LogToFile(LogData X) {
            if (IsLogQuit) {
                IVsOrUnity<IDebug>.Single.Log("程序已退出，Print无效");
                return;
            }
            if (LogFileWriter == null) {
                var LogFiles = GetFiles("Log".绝对路径());
                if (LogFiles.Length > MaxLogFileCount) {
                    var A = LogFiles.Select(t => new FileInfo(t)).OrderBy(t => t.CreationTime).ToList();
                    for (int i = 0; i < A.Count - MinLogFileCount; i++) {
                        A[i].Delete();
                    }
                }
                LogFileWriter = GetWriter("Log/" + NowTimeWithUnderLine + ".log");
                PhoneSystemInfo(LogFileWriter);
                IVsOrUnity<IDebug>.Single.OnQuit(() => {
                    IsLogQuit = true;
                    LogFileWriter.Flush();
                    LogFileWriter.Close();
                    LogFileWriter.Dispose();
                });
            }
            LogFileWriter.WriteLine(X.Level.ToString() + ":" + X.Message + "\n" + X.Stack);
        }
        public static void PhoneSystemInfo(StreamWriter X) {
            X.WriteLine("********************************");
            X.WriteLine(NowTime);
            X.Write(IVsOrUnity<IDebug>.Single.系统信息());
            foreach (var i in DriveInfo.GetDrives()) {
                if (i.IsReady && i.DriveType == DriveType.Fixed) {
                    var A = BToGB(i.TotalSize);
                    var B = BToGB(i.TotalFreeSpace);
                    var C = (1 - B / A) * 100;
                    X.WriteLine($"硬盘 {i.Name} {A:F0}GB 已用{C:F0}%");
                }
            }
            X.WriteLine("********************************");
            X.WriteLine();
        }
    }
    public enum LogLevel {
        Debug,
        Warning,
        Error,
    }
    public class LogData {
        public LogLevel Level;
        public object Sender;
        public string Message;
        public string Stack;
        public DateTime Time;
    }
}