using System;

namespace CMKZ {
    public static partial class LocalStorage {
        public static TimeSpan ToTimeSpan(this string X) {
            return TimeSpan.Parse(X);
        }
        public static string ToNullIfEmpty(this string X) {
            if (X == "") {
                return null;
            }
            return X;
        }
    }
}