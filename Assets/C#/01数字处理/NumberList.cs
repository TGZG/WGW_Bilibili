using System;

namespace CMKZ {
    public static partial class LocalStorage {
        public static int[] Add(this int[] X, int[] Y) {
            for (var i = 0; i < X.Length; i++) {
                X[i] += Y[i];
            }
            return X;
        }
        public static int[] Multiply(this int[] X, int[] Y) {
            for (var i = 0; i < X.Length; i++) {
                X[i] *= Y[i];
            }
            return X;
        }
        public static T[] AddToNew<T>(this T[] X, T[] Y) where T : struct {
            if (X.Length > Y.Length) {
                throw new Exception("数组长度不对");
            }
            if (!IsNumericType(typeof(T))) {
                throw new InvalidOperationException("类型必须是数值");
            }
            var A = new T[X.Length];
            for (var i = 0; i < X.Length; i++) {
                A[i] = (dynamic)X[i] + (dynamic)Y[i];
            }
            return A;
        }
        public static bool BiggerThan(this int[] X, int[] Y) {
            for (var i = 0; i < X.Length; i++) {
                if (X[i] < Y[i]) {
                    return false;
                }
            }
            return true;
        }
        public static bool NotBiggerThan(this int[] X, int[] Y) {
            for (var i = 0; i < X.Length; i++) {
                if (X[i] > Y[i]) {
                    return false;
                }
            }
            return true;
        }
        public static bool OneBiggerThan(this int[] X, int[] Y) {
            for (var i = 0; i < X.Length; i++) {
                if (X[i] > Y[i]) {
                    return true;
                }
            }
            return false;
        }
        public static bool OneBiggerThan(this double[] X, double[] Y) {
            for (var i = 0; i < X.Length; i++) {
                if (X[i] > Y[i]) {
                    return true;
                }
            }
            return false;
        }
        public static bool IsNumericType(Type type) {
            return type == typeof(byte) || type == typeof(sbyte) ||
                   type == typeof(short) || type == typeof(ushort) ||
                   type == typeof(int) || type == typeof(uint) ||
                   type == typeof(long) || type == typeof(ulong) ||
                   type == typeof(float) || type == typeof(double) ||
                   type == typeof(decimal) || type == typeof(Number);
        }
    }
}