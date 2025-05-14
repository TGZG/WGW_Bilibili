using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using System;
using System.Linq;//from XX select XX
using UnityEngine;
using static CMKZ.LocalStorage;

namespace CMKZ {
    public static partial class LocalStorage {
        //查找仓库里是否包含物品
        public static bool HaveItem(this IItemController X, Func<KeyValue<string, double>, bool> Y) {
            return X.Items.Any(Y);
        }
        public static bool HaveItem(this IItemController X, string Y, double Z = 1) {
            return X.GetItem(Y)?.Value >= Z;
        }
        public static bool HaveItem(this IItemController X, int Y, double Z = 1) {
            return X.GetItem(Y)?.Value >= Z;
        }
        public static void AddItem(this IItemController X, string Y, double Z = 1) {
            X.Items.Add(Y, Z);
        }
        public static void AddItems(this IItemController X, IItemController Y) {
            foreach (var i in Y.Items) {
                X.Items.Add(i);
            }
        }
        public static KeyValue<string, double> GetItem(this IItemController X, string Y) {
            return X.Items.Where(t => t.Key == Y).OrderByDescending(t => t.Value).FirstOrDefault();
        }
        public static KeyValue<string, double> GetItem(this IItemController X, int Y) {
            return X.Items.ElementAt(Y);
        }
        public static string GetItemName(this IItemController X, int Y) {
            return X.Items.ElementAt(Y).Key;
        }
        public static void RemoveItems(this IItemController X, List<int> 位置) {
            RemoveItems(X, t => 位置.Contains(t.位置));
        }
        public static void RemoveItems(this IItemController X, Func<(KeyValue<string, double> 项, int 位置), bool> Y) {
            //注：不能直接用foreach，因为RemoveAt会改变索引
        }
        public static bool RemoveItem(this IItemController X, string Y, double Z = 1) {
            var A = X.GetItem(Y);
            if (A == null) {
                return false;
            }
            if (A.Value < Z) {
                return false;
            }
            A.Value -= Z;
            if (A.Value == 0) {
                X.Items.Remove(A);
            }
            return true;
        }
        public static bool RemoveItem(this IItemController X, int Y, double Z = 1) {
            if (X.Items.Count() < Y + 1) {
                return false;
            }
            if (X.Items.ElementAt(Y).Value < Z) {
                return false;
            }
            X.Items.ElementAt(Y).Value -= Z;
            if (X.Items.ElementAt(Y).Value == 0) {
                X.Items.Remove(X.Items.ElementAt(Y));
            }
            return true;
        }
        public static void MoveTo(this IItemController X, IItemController Y, string Z, double A = 1) {
            if (X.RemoveItem(Z, A)) {
                Y.AddItem(Z, A);
            }
        }
        public static void MoveTo(this IItemController X, IItemController Y, KeyValue<string, double> Z) {
            X.MoveTo(Y, Z.Key, Z.Value);
        }
        public static void AllMoveTo(this IItemController X, IItemController Y) {
            foreach (var i in X.Items.ToArray()) {
                X.MoveTo(Y, i);
            }
        }
        public static void Clear(this IItemController X) {
            X.Items.Clear();
        }
        public static void MergeItem(this IItemController X) {
            Dictionary<string, double> 新列表 = new();
            X.Items.ForEach(t => {
                新列表[t.Key] += t.Value;
            });
            X.Clear();
            新列表.ForEach(t => {
                X.AddItem(t.Key, t.Value);
            });
        }
        public static void SplitItem(this IItemController X, int 位置, double Z) {
            var A = X.GetItem(位置);
            if (A == null) {
                return;
            }
            if (A.Value < Z) {
                return;
            }
            X.RemoveItem(位置, Z);
            X.AddItem(A.Key, Z);
        }
    }
    public interface IItemController {
        public string ItemControllerName { get; }
        public KeyValueList<string, double> Items { get; }
        public void CheckItems();
        public 排序方式枚举类型 排序方式 {
            get => _排序方式;
            set {
                _排序方式 = value;
                SortItems();
            }
        }
        public 升降序枚举类型 升降序 {
            get => _升降序;
            set {
                _升降序 = value;
                SortItems();
            }
        }
        public 排序方式枚举类型 _排序方式 { get; set; }
        public 升降序枚举类型 _升降序 { get; set; }
        public void SortItems();
    }

    public enum 升降序枚举类型 {
        升序,
        降序,
    }
    public enum 排序方式枚举类型 {
        按名称,
        按类型,
        按数量,
        按估值,
    }
    public static partial class LocalStorage {
        public static Dictionary<Type, I设定> AllInstanceType = new();
        public static Dictionary<string, I设定> _AllInstance;
        public static Dictionary<string, I设定> AllInstance => _AllInstance ??= MainTypesNotAbstract.Where(t => t.继承自(typeof(I设定)) && t.GetConstructor(Type.EmptyTypes) != null).ToDictionary(i => i.Name, i => i.创建实例() as I设定);
        public static int NowID;
        public static T GetInstance<T>(this string X) where T : class, I设定 {
            return AllInstance[X] as T;
        }
        public static T GetInstance<T>() where T : class, I设定 {
            var A = typeof(T);
            if (AllInstanceType.ContainsKey(A)) {
                return AllInstanceType[A] as T;
            } else {
                var B = AllInstance.Values.FirstOrDefault(t => t is T);
                if (B != null) {
                    return (T)(AllInstanceType[A] = B);
                }
                throw new Exception($"{A.Name} 不是设定");
            }
        }
        public static I设定 GetInstance(this string X) {
            return AllInstance[X];
        }
        public static I设定 GetInstance(this Type X) {
            return AllInstance[X.Name];
        }
        [初始化函数(typeof(CMKZProject), 初始化优先级.引擎库)]
        public static void InitID() {
            NowID = PlayerPrefs.GetInt("NowID", 0);
        }
        public static int CreateID() {
            NowID++;
            PlayerPrefs.SetInt("NowID", NowID);
            return NowID;
        }
        public static string 数字处理(this object X) {
            return X is double A ? A.To万单位() : X.ToString();
        }
    }
    public interface I设定 {
        public string Name { get; }
        public int ID { get; }
    }
    public interface I解包设定 {

    }
    public abstract class 设定类 : I设定 {
        [JsonProperty]
        public virtual string Name => GetType().Name;
        [JsonProperty]
        public virtual int ID { get; } = CreateID();
        public override string ToString() {
            if (this is I解包设定) {
                var 删除 = new string[] { "Name", "ID" };
                var A = $"";
                foreach (var B in GetType().GetFields().Reverse()) {
                    if (删除.Contains(B.Name)) continue;
                    A += $"{B.Name}：{B.GetValue(this).数字处理()}\n";
                }
                foreach (var B in GetType().GetProperties().Reverse()) {
                    if (删除.Contains(B.Name)) continue;
                    A += $"{B.Name}：{B.GetValue(this).数字处理()}\n";
                }
                return A;
            } else {
                return Name;
            }
        }
    }
    public interface I介绍 : I设定 {
        public string 介绍 { get; }
    }
}