using System;

namespace CMKZ {
    public static partial class LocalStorage {
        public static string 桌面路径 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        /// <summary>
        /// [yyyy.MM.dd_HH:mm:ss]
        /// </summary>
        public static string NowTime => DateTime.Now.ToLocalTime().ToString("[yyyy.MM.dd_HH:mm:ss]");
        /// <summary>
        /// yyyy_MM_dd_HH_mm_ss
        /// </summary>
        public static string NowTimeWithUnderLine => DateTime.Now.ToLocalTime().ToString("yyyy_MM_dd_HH_mm_ss");
        /// <summary>
        /// HH:mm:ss
        /// </summary>
        public static string NowTimeWithoutDay => DateTime.Now.ToLocalTime().ToString("HH:mm:ss");
        public static string DirName(this string X) => X.IndexOf("/") == -1 ? "" : X[..X.LastIndexOf("/")];
        public static string FileName(this string X) => X.IndexOf("/") == -1 ? X : X[(X.LastIndexOf("/") + 1)..];
        public static string RootDirName(this string X) => X.IndexOf("/") == -1 ? X : X[..X.IndexOf("/")];
        public static string RemoveFirst(this string Y) => Y.Split("/").RemoveFirst().Join("/");
        public static string GetAtSecond(this string X) => X.Split("/")[1];
        public static string RePath(this string X) => X.Replace("\\", "/").Replace("//", "/").TrimEnd('/');
        public static string 绝对路径(this string X) {
            X = X.RePath();
            if (!X.Contains(":/")) {
                X = IVsOrUnity<IDebug>.Single.文件路径() + X;
            }
            return X;
        }
        public static double BToGB(long bytes) {
            return (double)bytes / (1024 * 1024 * 1024);
        }
    }
}