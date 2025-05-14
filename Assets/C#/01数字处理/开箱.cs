using Microsoft.CodeAnalysis;
using System;//Action
using System.Collections.Generic;//List
using System.Linq;//from XX select XX
using static CMKZ.LocalStorage;

namespace CMKZ {
    public static partial class LocalStorage {
        /// <summary>
        /// 输入的字典的值表示每种选项的价格
        /// 输出的字典的值为每种选项的期望生成数量
        /// </summary>
        public static Dictionary<T, double> 期望生成概率<T>(this Dictionary<T, double> X, double 期望) {
            var 单位期望 = 期望 / X.Count;
            return X.ToDictionary(t => t.Key, t => 单位期望 / t.Value);
        }
        public static Dictionary<T, double> 期望生成概率<T>(this IEnumerable<(T, double)> X, double 期望) {
            var 单位期望 = 期望 / X.Count();
            var A = new Dictionary<T, double>();
            foreach (var i in X) {
                A[i.Item1] = 单位期望 / i.Item2;
            }
            return A;
        }
        /// <summary>
        /// 输入概率（由期望生成概率）
        /// 输出每个选项生成的数量。只记录数量大于0的。
        /// 特殊规则：如果某个选项的概率大于0.5，那么有50%概率输出X个这个选项
        /// </summary>
        public static Dictionary<T, double> 概率生成<T>(this Dictionary<T, double> X, bool 每种最多一个 = false) {
            X = X.ToDictionary(t => t.Key, t => t.Value > 0.5 ? 0.5 : t.Value);
            var A = new Dictionary<T, double>();
            foreach (var i in X) {
                if (Random(i.Value)) {
                    A[i.Key] = 每种最多一个 ? 1 : i.Value.取整();
                }
            }
            return A;
        }
        public static T 权重开箱<T>(this Dictionary<T, double> X) {
            return new 权重箱子<T>(X).开箱();
        }
        public static T 权重开箱<T>(this System.Collections.Generic.Dictionary<T, double> X) {
            return new 权重箱子<T>(X).开箱();
        }
        public static T 权重开箱<T>(this List<T> X) where T : I权重 {
            return new 权重箱子<T>(X.ToDictionary(i => i, i => i.权重)).开箱();
        }
    }
    public interface I权重 {
        public double 权重 { get; }
    }
    /// <summary>
    /// 权重和开出来的概率正相关。
    /// </summary>
    public class 权重箱子 : Dictionary<string, double> {
        public 权重箱子() { }
        public 权重箱子(Dictionary<string, double> X) {
            foreach (var i in X) {
                this[i.Key] = i.Value;
            }
        }
        //public void Add(I道具设定 X) {
        //    this[X.名称] = X.价格;
        //}
        public string 开箱() {
            if (Count == 0) throw new Exception("开箱错误");
            double A = Random(0, 1f) * Values.Sum();
            double B = 0f;
            foreach (var i in this) {
                B += i.Value;
                if (A < B) {
                    return i.Key;
                }
            }
            throw new Exception("开箱错误");
        }
    }
    public class 权重箱子<T> : Dictionary<T, double> {
        public 权重箱子() { }
        public 权重箱子(Dictionary<T, double> X) {
            foreach (var i in X) {
                this[i.Key] = i.Value;
            }
        }
        public T 开箱() {
            if (Count == 0) throw new Exception("开箱错误");
            double A = Random(0, 1f) * Values.Sum();
            double B = 0f;
            foreach (var i in this) {
                B += i.Value;
                if (A < B) {
                    return i.Key;
                }
            }
            throw new Exception("开箱错误");
        }
    }
    /// <summary>
    /// 有更大可能开出期望价格的物品。
    /// </summary>
    public class 期望箱子 : Dictionary<string, double> {
        public double 期望;
        public string 开箱() {
            if (Count == 0) throw new Exception("开箱错误");
            double A = Values.Sum();
            var B = this.ToDictionary(kvp => kvp.Key, kvp => kvp.Value / A);
            double C = Random(0, 1f);
            double D = 0f;
            foreach (var i in B) {
                D += i.Value;
                if (C <= D) {
                    return i.Key;
                }
            }
            throw new Exception("开箱错误");
        }
    }
}