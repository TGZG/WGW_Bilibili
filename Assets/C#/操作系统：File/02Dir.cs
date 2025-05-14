using System;
using System.IO;
using System.Linq;

namespace CMKZ {
    public static partial class LocalStorage {
        ///<summary>单层目录</summary>
        public static List<string> ReadFileList(string Path, FileSortType 参数 = FileSortType.FileName) {
            var 文件名列表 = new List<string>();
            var 文件列表 = new DirectoryInfo(Path).GetFiles();
            if (参数 == FileSortType.FileName) {
                Array.Sort(文件列表, (x, y) => x.Name.CompareTo(y.Name));
            } else if (参数 == FileSortType.CreationTime) {
                Array.Sort(文件列表, (x, y) => x.CreationTime.CompareTo(y.CreationTime));
            } else if (参数 == FileSortType.LastWriteTime) {
                Array.Sort(文件列表, (x, y) => x.LastWriteTime.CompareTo(y.LastWriteTime));
            } else if (参数 == FileSortType.Size) {
                Array.Sort(文件列表, (x, y) => x.Length.CompareTo(y.Length));
            }
            return 文件列表.Select(t => t.Name).ToList();
        }
        ///<summary>多层目录</summary>
        public static List<string> GetFileList(string X) {
            var A = new List<string>();
            GetFilesRecursive(X, A);
            var B = A.SortFile();
            for (int i = 0; i < B.Length; i++) {
                A[i] = B[i].Replace(X, X.FileName());
            }
            return A;
        }
        private static void GetFilesRecursive(string X, List<string> Y) {
            foreach (var i in Directory.GetFiles(X)) {
                Y.Add(i.Replace("\\", "/"));
            }
            foreach (var i in Directory.GetDirectories(X)) {
                GetFilesRecursive(i, Y);
            }
        }
    }
}