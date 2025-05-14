using ExcelDataReader.Log;
using System.IO;
using System;
using System.Security.Cryptography;

namespace CMKZ {
    public static partial class LocalStorage {
        public static void RequireDirNotExist(string X) {
            if (Directory.Exists(X)) {
                Directory.Delete(X, true);
            }
        }
        public static void RequireDirExist(string X) {
            if (!Directory.Exists(X)) {
                Directory.CreateDirectory(X);
            }
        }
        public static void RequireFileNotExist(string X) {
            if (File.Exists(X)) {
                File.Delete(X);
            }
        }
        public static void RequireFileExist(string X) {
            if (!File.Exists(X)) {
                FileWrite(X, "");
            }
        }
        public static void DirCopy(string 原位置, string 目标位置) {
            RequireDirExist(目标位置);
            foreach (string folder in Directory.GetDirectories(原位置)) {
                DirCopy(folder, Path.Combine(目标位置, Path.GetFileName(folder)));
            }
            foreach (string file in Directory.GetFiles(原位置)) {
                File.Copy(file, Path.Combine(目标位置, Path.GetFileName(file)));
            }
        }
        public static void DirRename(string 原本路径, string 新名称) {
            try {
                Directory.Move(原本路径, Path.Combine(Path.GetDirectoryName(原本路径), 新名称));
            } catch (Exception e) {
                Print("文件夹重命名错误：" + e.ToString());
            }
        }
    }
}