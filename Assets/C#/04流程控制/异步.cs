using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System;
using System.Threading.Tasks;

namespace CMKZ {
    public static partial class LocalStorage {
        /// <![CDATA[
        /// await 5.5; //等待5.5秒
        /// ]]>
        public static TaskAwaiter GetAwaiter(this double X) {
            return Task.Delay((int)(X * 1000)).GetAwaiter();
        }
        /// <![CDATA[
        /// foreach (int i in 1..10) {
        ///     Print(i);
        /// }
        /// ]]>
        public static IEnumerator<int> GetEnumerator(this Range X) {
            if (X.End.IsFromEnd || X.Start.IsFromEnd)
                throw new NotSupportedException("Range with End or Start from end is not supported");
            for (int i = X.Start.Value; i < X.End.Value; i++)
                yield return i;
        }
        public static IEnumerator<int> GetEnumerator(this IEnumerable<int> enumerable) {
            foreach (int i in enumerable)
                yield return i;
        }
        /// <![CDATA[
        /// (1..10).ForEach(i => Print(i));
        /// ]]>
        public static void ForEach(this Range range, Action<int> action) {
            foreach (int i in range) {
                action(i);
            }
        }
        public static void ForEach(this IEnumerable<int> enumerable, Action<int> action) {
            foreach (var item in enumerable) {
                action(item);
            }
        }
        public static List<int> ToList(this Range range) {
            var list = new List<int>();
            range.ForEach(i => list.Add(i));
            return list;
        }
        //自由转化器
        public static T2 To<T1, T2>(this T1 obj, Func<T1, T2> func) {
            return func(obj);
        }
    }
}