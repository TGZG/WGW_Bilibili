using System;//Action
using System.Collections;
using System.Collections.Generic;
using System.Linq;//List

namespace CMKZ {
    public static partial class LocalStorage {
        public static Dictionary<T, T2> ToDictionary<T, T2>(this string[][] X, Func<string[], KeyValue<T, T2>> Y) {
            //将X的每一项解析为一个键值对，作为字典的一项
            return X.Select(Y).ToDictionary(t => t.Key, t => t.Value);
        }
        public static IEnumerable<T> Where<T>(this IEnumerable X) {
            foreach (var i in X) {
                if (i is T A) {
                    yield return A;
                }
            }
        }
        public static void ForFirst<T>(this IEnumerable<T> X, Action<T> Y) {
            foreach (var i in X) {
                Y(i);
                break;
            }
        }
        public static IEnumerable<T> Where<T>(this IEnumerable<T> X, Func<T, int, bool> Y) {
            var A = 0;
            foreach (var i in X) {
                if (Y(i, A)) {
                    yield return i;
                }
                A++;
            }
        }
        public static T Choice<T>(params T[] X) {
            return X.Choice();
        }
        public static void InterleaveListsDemo() {
            List<int> list1 = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
                                          11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
                                          21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
                                          31, 32, 33, 34, 35, 36, 37, 38, 39, 40,
                                          41, 42, 43, 44, 45, 46, 47, 48, 49, 50,
                                          51, 52, 53, 54, 55, 56, 57, 58, 59, 60,
                                          61, 62, 63, 64, 65, 66, 67, 68, 69, 70,
                                          71, 72, 73, 74, 75, 76, 77, 78, 79, 80,
                                          81, 82, 83, 84, 85, 86, 87, 88, 89, 90,
                                          91, 92, 93, 94, 95, 96, 97, 98, 99, 100 };
            List<int> list2 = new List<int> { 10, 20, 30 };

            List<int> result = InterleaveLists(list1, list2);
            Print(string.Join(", ", result));
        }
        public static List<T> InterleaveLists<T>(this List<T> list1, List<T> list2) {
            List<T> result = list1.Clone();
            int longListCount = list1.Count;
            int shortListCount = list2.Count;

            if (shortListCount == 0) {
                return result; // 如果短列表为空，则直接返回长列表
            }

            // 计算插入间隔
            double interval = (double)longListCount / (shortListCount + 1);

            for (int i = 0; i < shortListCount; i++) {
                int insertIndex = (int)Math.Round(interval * (i + 1));
                if (insertIndex > result.Count) {
                    insertIndex = result.Count;
                }

                result.Insert(insertIndex, list2[i]);
            }

            return result;
        }
        public static string FirstOr(this string[] X, string Y) {
            return X.Length > 0 ? X[0] : Y;
        }
    }
}