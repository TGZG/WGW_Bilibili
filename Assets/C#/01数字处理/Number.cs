using System;
using System.Collections.Generic;
using static CMKZ.LocalStorage;
using System.Linq;
using Newtonsoft.Json;

namespace CMKZ {
    public interface INumber {
        public bool IsZero();
    }
    public static partial class LocalStorage {
        public static double Sum(this IEnumerable<Number> X) {
            var A = 0.0;
            foreach (var B in X) {
                A += B;
            }
            return A;
        }
        public static Number Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, Number> selector) {
            return source.Select(selector).Sum();
        }
        public static Number Average(this IEnumerable<Number> source) {
            return source.Sum() / source.Count();
        }
    }
    public struct Number : INumber {
        [JsonProperty]
        private double Data;
        //重载加减乘除 大于小于等于不等 隐式到int flota
        public static Number operator +(Number X, Number Y) => new() { Data = X.Data + Y.Data };
        public static Number operator -(Number X, Number Y) => new() { Data = X.Data - Y.Data };
        public static Number operator *(Number X, Number Y) => new() { Data = X.Data * Y.Data };
        public static Number operator /(Number X, Number Y) => new() { Data = X.Data / Y.Data };
        public static bool operator >(Number X, Number Y) => X.Data > Y.Data;
        public static bool operator <(Number X, Number Y) => X.Data < Y.Data;
        public static bool operator >=(Number X, Number Y) => X.Data >= Y.Data;
        public static bool operator <=(Number X, Number Y) => X.Data <= Y.Data;
        public static bool operator ==(Number X, Number Y) => X.Data == Y.Data;
        public static bool operator !=(Number X, Number Y) => X.Data != Y.Data;
        public static implicit operator int(Number X) => (int)X.Data;
        public static implicit operator float(Number X) => (float)X.Data;
        public static implicit operator double(Number X) => X.Data;
        public static implicit operator int(Number? X) => (int)X.Value.Data;
        public static implicit operator float(Number? X) => (float)X.Value.Data;
        public static implicit operator double(Number? X) => X.Value.Data;
        //public static implicit operator string(Number X) => X.ToString();
        public static implicit operator Number(int X) => new() { Data = X };
        public static implicit operator Number(int? X) => new() { Data = (int)X };
        public static implicit operator Number(float X) => new() { Data = X };
        public static implicit operator Number(float? X) => new() { Data = (float)X };
        public static implicit operator Number(double X) => new() { Data = X };
        public static implicit operator Number(double? X) => new() { Data = (double)X };
        //public static implicit operator Number(string X) => new() { Data = double.Parse(X) };
        public static implicit operator Number?(int X) => new() { Data = X };
        public static implicit operator Number?(int? X) => new() { Data = (int)X };
        public static implicit operator Number?(float X) => new() { Data = X };
        public static implicit operator Number?(float? X) => new() { Data = (float)X };
        public static implicit operator Number?(double X) => new() { Data = X };
        public static implicit operator Number?(double? X) => new() { Data = (double)X };
        //public static implicit operator Number?(string X) => new() { Data = double.Parse(X) };
        public override bool Equals(object obj) {
            return obj is Number number && Data == number.Data;
        }
        public override int GetHashCode() {
            return Data.GetHashCode();
        }
        public static bool 限一 = false;//最小为1
        public static NumberMode 模式;
        public override string ToString() {
            var A = Data;
            if (模式.HasFlag(NumberMode.最小为一) && Data < 1) A = 1;
            if (模式.HasFlag(NumberMode.移除超两位小数)) A = Math.Round(A, 2);
            if (模式.HasFlag(NumberMode.移除超二位有效)) A = X位有效(A);
            if (模式.HasFlag(NumberMode.移除超三位有效)) A = X位有效(A, 3);
            if (模式.HasFlag(NumberMode.移除超四位有效)) A = X位有效(A, 4);
            if (模式.HasFlag(NumberMode.万亿规范)) return 万亿(A);
            if (模式.HasFlag(NumberMode.千兆规范)) return 千兆(A);
            return 清零(A);
        }
        public bool IsZero() => Data == 0;
        public Number 乘方(double X) {
            return new() { Data = Math.Pow(Data, X) };
        }
        public Number 开根(double X = 2) {
            return new() { Data = Math.Pow(Data, 1 / X) };
        }

        public static string 清零(double X) {
            return X.ToString("0.00").TrimEnd('0').TrimEnd('.');
        }
        public static double X位有效(double X, int 位数 = 2) {
            if (X == 0) return 0;
            double scale = Math.Pow(10, Math.Floor(Math.Log10(Math.Abs(X))) - (位数 - 1));
            return Math.Round(X / scale) * scale;
        }
        public static string 万亿(double X) {
            string A = X < 0 ? "-" : "";
            X = Math.Abs(X);
            if (X is < 10000) {
                A += 清零(X);
            } else if (X is >= 10000 and < 10000_0000) {
                A += 清零(X / 10000) + "万";
            } else if (X is >= 10000_0000 and < 10000_0000_0000) {
                A += 清零(X / 10000_0000) + "亿";
            } else {
                A += 清零(X / 10000_0000_0000) + "万亿";
            }
            return A;
        }
        public static string 千兆(double X) {
            string A = X < 0 ? "-" : "";
            X = Math.Abs(X);
            if (X is < 1000) {
                A += X;
            } else if (X is >= 1000 and < 1000_000) {
                A += 清零(X / 1000) + "千";
            } else if (X is >= 1000_000 and < 1000_000_000) {
                A += 清零(X / 1000_000) + "百万";
            } else {
                A += 清零(X / 1000_000_000) + "十亿";
            }
            return A;
        }
    }
    [Flags]
    public enum NumberMode {
        移除超两位小数 = 0x1,
        移除超二位有效 = 0x2,
        移除超四位有效 = 0x4,
        万亿规范 = 0x8,
        最小为一 = 0x10,
        千兆规范 = 0x20,
        移除超三位有效 = 0x40,
    }
}