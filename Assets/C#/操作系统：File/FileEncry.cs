using ExcelDataReader.Log;
using System.IO;
using System;
using System.Security.Cryptography;

namespace CMKZ {
    public static partial class LocalStorage {
        public static string GetFileMd5(string X) {
            if (!File.Exists(X)) {
                throw new FileNotFoundException($"文件不存在: {X}");
            }
            try {
                using var md5 = MD5.Create();
                using var stream = File.OpenRead(X);
                return BitConverter.ToString(md5.ComputeHash(stream)).Remove("-").ToLowerInvariant();
            } catch (Exception ex) {
                throw new Exception($"计算 {X} MD5失败：{ex.Message}");
            }
        }
    }
}