using System;
using System.IO;
using System.Linq;

namespace CMKZ {
    public enum FileSortType{
        FileName,
        CreationTime,
        LastWriteTime,
        Size
    }
    public static partial class LocalStorage {
        public static long FileSize(string X) {
            var A = new FileInfo(X);
            return A.Exists ? A.Length : -1;
        }
        public static string[] SortFile(this List<string> fileNames) {
            return fileNames.Select(fileName => (fileName, File.GetCreationTime(fileName))).OrderBy(file => file.Item2).Select(file => file.Item1).ToArray();
        }
        public static FileSystemInfo[] SortByCreateTime(this FileSystemInfo[] X) {
            Array.Sort(X, (x, y) => y.CreationTime.CompareTo(x.CreationTime));
            return X;
        }
        public static List<string> 文件检索(string 文件夹名, string 内容) {
            var A = new List<string>();
            foreach (var i in GetFileList(文件夹名)) {
                foreach (var j in File.ReadAllLines(i)) {
                    if (j.Contains(内容)) {
                        A.Add(i + "：" + j);
                    }
                }
            }
            return A;
        }
        public static List<string> 获取文件列表(string 文件夹名) {
            文件夹名 = 文件夹名.绝对路径();
            var A = new List<string>();
            foreach (var i in Directory.GetFiles(文件夹名)) {
                A.Add(i.Replace(文件夹名 + "\\", ""));
            }
            return A;
        }
        public static string TryFileRead(string X, string Y) {
            if (!FileExists(X)) {
                FileWrite(X, Y);
            }
            return FileRead(X);
        }
        public static T TryFileRead<T>(string X, T Y, bool 加密 = false, bool 全保存 = true) {
            if (!FileExists(X)) {
                FileWrite(X, Y, 加密, 全保存);
            }
            return FileRead<T>(X, 加密, 全保存);
        }
        public static T TryFileRead<T>(string X, Func<T> Y, bool 加密 = false, bool 全保存 = true) {
            if (!FileExists(X)) {
                FileWrite(X, Y(), 加密, 全保存);
            }
            return FileRead<T>(X, 加密, 全保存);
        }
        public static string FileRead(string X) {
            X = X.绝对路径();
            if (File.Exists(X)) {
                return File.ReadAllText(X).Replace("\r", "");
            } else {
                PrintWarning("Read的文件不存在：" + X);
                return "";
            }
        }
        public static T FileRead<T>(string X, bool 加密 = false, bool 全保存 = true) {
            X = X.绝对路径();
            if (File.Exists(X)) {
                if (!加密) {
                    return File.ReadAllText(X).JsonDeserialize<T>(全保存);
                }
                return File.ReadAllBytes(X).Decrypt().BytesToString().JsonDeserialize<T>(全保存);
            } else {
                PrintWarning("Read的文件不存在：" + X);
                return default;
            }
        }
        public static bool FileExists(string X) {
            X = X.绝对路径();
            return File.Exists(X);
        }

        public static bool FolderExists(string 文件夹名) {
            文件夹名 = 文件夹名.绝对路径();
            return Directory.Exists(文件夹名);

        }
        public static void CreateFolder(string 文件夹名) {
            文件夹名 = 文件夹名.绝对路径();
            if (!Directory.Exists(文件夹名)) {
                Print($"正在创建：{文件夹名}");
                Directory.CreateDirectory(文件夹名);
            } else {
                PrintWarning("正在创建的文件夹已存在：" + 文件夹名);
            }
        }
        public static List<string> 获取或创建文件列表(string 文件夹名) {
            if (!FolderExists(文件夹名)) {
                CreateFolder(文件夹名);
            }
            文件夹名 = 文件夹名.绝对路径();
            var A = new List<string>();
            foreach (var i in Directory.GetFiles(文件夹名)) {
                A.Add(i.Replace(文件夹名 + "\\", ""));
            }
            return A;
        }
    }
}