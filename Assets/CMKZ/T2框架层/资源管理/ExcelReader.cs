using ExcelDataReader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using UnityEngine;

namespace CMKZ {
    public static partial class LocalStorage {
        /// <summary>
        /// 1.从StreamingAssets读取
        /// 2.表格第一行是表头
        /// 3.表格以.xlsx结尾。X参数不带后缀
        /// </summary>
        public static List<T> ReadExcel<T>(string X, Func<string[], T> Y) {
            if (!X.Contains(":")) X = Path.Combine(Application.streamingAssetsPath, "Excel", X + ".xlsx");
            var A = new List<T>();
            using (var stream = File.Open(X, FileMode.Open, FileAccess.Read)) {
                using var reader = ExcelReaderFactory.CreateReader(stream);
                foreach (DataRow row in reader.AsDataSet().Tables[0].Rows.RemoveFirst()) {
                    A.Add(Y(row.ItemArray.Select(C => C.ToString()).ToArray()));
                }
            }
            return A;
        }
    }
}