using System;//Action

namespace CMKZ {
    public static partial class LocalStorage {
        public static T Set<T>(this T X, Action<T> Y) {
            Y(X);
            return X;
        }
    }
}