using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static CMKZ.LocalStorage;
using Newtonsoft.Json;

namespace CMKZ {
    public static partial class LocalStorage {
        public static Dictionary<Func<Type, bool>, Func<Type, object>> DefaultDictionary=new();
        public static void AddRange<T>(this Dictionary<T, double> X, Dictionary<T, double> Y) {
            foreach (var i in Y) {
                X[i.Key] += i.Value;
            }
        }
        public static void AddRange<T>(this Dictionary<T, int> X, Dictionary<T, int> Y) {
            foreach (var i in Y) {
                X[i.Key] += i.Value;
            }
        }
        [初始化函数(typeof(CMKZProject), 初始化优先级.引擎库)]
        public static void InitDictionary() {
            DefaultDictionary[t => t.继承自<UnityEngine.Object>()] = t => default;
        }
    }
    [JsonObject(MemberSerialization.OptOut)]
    public class Dictionary<T1, T2> : IEnumerable<KeyValuePair<T1, T2>> {
        public System.Collections.Generic.Dictionary<T1, T2> Data = new();
        [JsonIgnore]
        public int Count => Data.Count;
        [JsonIgnore]
        public T1[] Keys => Data.Keys.ToArray();
        [JsonIgnore]
        public T2[] Values => Data.Values.ToArray();
        public bool DisableZero = false;
        public T2 this[T1 X] {
            get {
                if (X == null) {
                    throw new ArgumentNullException("this[X]中的X不可为null");
                    //return Data[X]=default;
                }
                if (Data.ContainsKey(X)) {
                    return Data[X];
                }
                foreach (var i in DefaultDictionary) {
                    if (i.Key(typeof(T2))) {
                        return Data[X] = (T2)i.Value(typeof(T2));
                    }
                }
                if (typeof(T2).IsClass()) {
                    if (typeof(T2).IsAbstract) {
                        return Data[X] = default;
                    }
                    return Data[X] = CreateObject<T2>();//List自动初始化
                }
                return Data[X] = default;
            }
            set {
                Data[X] = value;
                if (DisableZero && Data[X].IsZero()) {
                    RemoveKey(X);
                }
            }
        }
        public Dictionary() { }
        public Dictionary(System.Collections.Generic.Dictionary<T1, T2> X) {
            foreach (var i in X) {
                this[i.Key] = i.Value;
            }
        }
        //增
        public void Add(T1 X, T2 Y) {
            if (Data.ContainsKey(X)) {
                Print($"警告：字典中有相同的键：{X}");
                Data[X] = Y;
            } else {
                Data.Add(X, Y);
            }
        }
        public void Add(KeyValuePair<T1, T2> X) {
            Data.Add(X.Key, X.Value);
        }
        public void AddRange(Dictionary<T1, T2> X, Func<T2, T2, T2> Y = null) {
            Y ??= (V1, V2) => V2;
            foreach (var i in X) {
                this[i.Key] = ContainsKey(i.Key) ? Y(this[i.Key], i.Value) : i.Value;
            }
        }
        //删除
        public void RemoveKey(T1 X) {
            Data.Remove(X);
        }
        public void RemoveFirst(Func<KeyValuePair<T1, T2>, bool> X) {
            foreach (var i in this.Where(X)) {
                RemoveKey(i.Key);
                return;
            }
        }
        public void RemoveAll(Func<KeyValuePair<T1, T2>, bool> X) {
            List<Action> 删除列表 = new();
            foreach (var i in this.Where(X)) {
                删除列表.Add(() => RemoveKey(i.Key));
            }
            删除列表.ForEach(t => t.Invoke());
        }
        public void Clear() {
            Data.Clear();
        }
        //改
        public Dictionary<T1, T2> ChangeKey(T1 Y, T1 Z) {
            this[Z] = this[Y];
            RemoveKey(Y);
            return this;
        }
        public Dictionary<T3, T2> ChangeKey<T3>(Func<T1, T3> Y) {
            var A = new Dictionary<T3, T2>();
            foreach (var i in this) {
                A[Y(i.Key)] = i.Value;
            }
            return A;
        }
        public Dictionary<T1, T3> Turn<T3>(Func<T2, T3> X) {
            var A = new Dictionary<T1, T3>();
            foreach (var i in this) {
                A[i.Key] = X(i.Value);
            }
            return A;
        }
        public Dictionary<T3, T2> TurnKey<T3>(Func<T1, T3> X) {
            var A = new Dictionary<T3, T2>();
            foreach (var i in this) {
                A[X(i.Key)] = i.Value;
            }
            return A;
        }
        //查
        public bool ContainsKey(T1 X) {
            return Data.ContainsKey(X);
        }
        public bool ContainsValue(T2 X) {
            return Data.ContainsValue(X);
        }
        //其他
        public Dictionary<T1, T2> Clone() {
            var A = new Dictionary<T1, T2>();
            foreach (var i in this) {
                A[i.Key] = i.Value;
            }
            return A;
        }
        //接口与重载
        public IEnumerator<KeyValuePair<T1, T2>> GetEnumerator() {
            return Data.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
        public static implicit operator Dictionary<T1, T2>(System.Collections.Generic.Dictionary<T1, T2> X) {
            return new Dictionary<T1, T2>(X);
        }
        public static implicit operator System.Collections.Generic.Dictionary<T1, T2>(Dictionary<T1, T2> X) {
            return X.Data;
        }
        public static Dictionary<T1, T2> operator +(Dictionary<T1, T2> X, Dictionary<T1, T2> Y) {
            var A = new Dictionary<T1, T2>();
            A.AddRange(X);
            A.AddRange(Y);
            return A;
        }
        public static Dictionary<T1, T2> operator -(Dictionary<T1, T2> X, Dictionary<T1, T2> Y) {
            var A = new Dictionary<T1, T2>();
            A.AddRange(X);
            A.RemoveAll(t => Y.ContainsKey(t.Key));
            return A;
        }
        public KeyValueList<T1, T2> ToKeyValueList() {
            var A = new KeyValueList<T1, T2>();
            foreach (var i in this) {
                A.Add(i.Key, i.Value);
            }
            return A;
        }
    }
}