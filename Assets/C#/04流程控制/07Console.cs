using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using static CMKZ.LocalStorage;

namespace CMKZ {
    public static partial class LocalStorage {
        public static Console 控制台 = new();
        public static Action<I文本空格函数> OnCommandBefore;
        public static Action<I文本空格函数> OnCommandSuccess;
        public static Action<I文本空格函数> OnCommandFailed;
        public static Action<I文本空格函数> OnCommandAfter;
        public static bool Invoke(this I指令 X) {
            OnCommandBefore?.Invoke(X);
            bool 成功执行 = X._Invoke();
            if (成功执行) {
                OnCommandSuccess?.Invoke(X);
            } else {
                OnCommandFailed?.Invoke(X);
            }
            OnCommandAfter?.Invoke(X);
            return 成功执行;
        }
        public static string Invoke(this I查询 X) {
            OnCommandBefore?.Invoke(X);
            string A = X._Invoke();
            if (A.IsNullOrEmpty()) {
                OnCommandSuccess?.Invoke(X);
            } else {
                OnCommandFailed?.Invoke(X);
            }
            OnCommandAfter?.Invoke(X);
            return A;
        }
        public static void InitConsole() {
            OnCommandBefore += X => Print($"指令执行：{X.Value}");
            OnCommandSuccess += X => Print($"指令执行成功：{X.Value}");
            OnCommandFailed += X => Print($"指令执行失败：{X.Value}");
            //查询结果显示在控制台中，并打印输出
        }
        public static Dictionary<Type, List<string>> 缓存指令参数 = new();
        public static List<string> 指令参数(this I文本空格函数 X) {
            if (缓存指令参数.ContainsKey(X.GetType())) {
                return 缓存指令参数[X.GetType()];
            }
            var A = new List<(int, string)>();
            foreach (var i in X.GetType().GetProperties()) {
                var C = i.GetCustomAttribute<排序Attribute>();
                if (C != null) {
                    A.Add((C.顺序, i.Name));
                    goto T;
                }
                foreach (var j in X.GetType().GetInterfaces()) {
                    var D = j.GetProperty(i.Name);
                    if (D != null) {
                        var E = D.GetCustomAttribute<排序Attribute>();
                        if (E != null) {
                            A.Add((E.顺序, i.Name));
                            goto T;
                        }
                    }
                }
                PrintWarning($"{X.GetType().Name} {i.Name} 未找到排序属性");
            T:;
            }
            return 缓存指令参数[X.GetType()] = A.OrderBy(t => t.Item1).Select(t => t.Item2).ToList();
        }
    }
    public interface I文本空格函数 { string Value { get; } }
    public interface I指令 : I文本空格函数 { bool _Invoke(); }//写
    public interface I查询 : I文本空格函数 { string _Invoke(); }//读
    public interface I开发者指令 : I指令 { }
    public class 文本空格类 {
        public string Value { get; set; }
    }
    public class 排序Attribute : Attribute {
        public int 顺序;
        public 排序Attribute(int 顺序) {
            this.顺序 = 顺序;
        }
    }
    public interface I查询帮助 : I查询 { [排序(1)] bool 详细 { get; } }
    public class 查询帮助 : 文本空格类, I查询 {
        string I查询._Invoke() {
            return 控制台.帮助文本;
        }
    }
    public static partial class LocalStorage {
        public static string Get注释(this I指令 X) {
            return X.GetType().Name.Remove("指令") + " " + X.指令参数().Join(" ");
        }
    }
    //public class 注释Attribute : Attribute {
    //    public string 内容;
    //    public 注释Attribute(string 内容) {
    //        this.内容 = 内容;
    //    }
    //}
    public class Console {
        public List<I指令> 所有指令 = new();
        public Dictionary<Type, Func<string, object>> 转化器 = new();
        private string _帮助文本;
        public string 帮助文本 => _帮助文本 ??= "所有指令：\n" + 所有指令.Where(t => t is not I开发者指令).Select(t => t.Get注释()).Join("\n");
        public Console() {
            MainTypes.Where(t => t.继承自<I指令>() && t.IsClass).ForEach(t => 所有指令.Add((I指令)Activator.CreateInstance(t)));
            转化器[typeof(short)] = t => Convert.ChangeType(t, typeof(short));
            转化器[typeof(ushort)] = t => Convert.ChangeType(t, typeof(ushort));
            转化器[typeof(int)] = t => Convert.ChangeType(t, typeof(int));
            转化器[typeof(uint)] = t => Convert.ChangeType(t, typeof(uint));
            转化器[typeof(long)] = t => Convert.ChangeType(t, typeof(long));
            转化器[typeof(ulong)] = t => Convert.ChangeType(t, typeof(ulong));
            转化器[typeof(float)] = t => Convert.ChangeType(t, typeof(float));
            转化器[typeof(double)] = t => Convert.ChangeType(t, typeof(double));
            转化器[typeof(char)] = t => Convert.ChangeType(t, typeof(char));
            转化器[typeof(byte)] = t => Convert.ChangeType(t, typeof(byte));
            转化器[typeof(string)] = t => t;
            OnPrint += t => {
                if (t.Level == LogLevel.Debug) {
                    //UI
                } else if (t.Level == LogLevel.Warning) {

                } else if (t.Level == LogLevel.Error) {

                }
            };
        }
        public void 执行(string X) {
            X = Regex.Replace(X, @"\s+", " ");
            X = X.Trim();
            PrintSystem($"> {X}");
            if (X == "") {
                PrintWarning($"指令不可为空");
                return;
            }
            var 引导词 = X.Split(' ')[0];
            var A = 匹配引导词(引导词);
            if (A == null) {
                PrintWarning($"指令引导词 {引导词} 不存在", LocalStorage.System);
                return;
            }
            匹配参数(X, A);
        }
        private I指令 匹配引导词(string X) {
            foreach (var a in 所有指令) {
                if (a.GetType().Name.Remove("指令") == X) return a;
            }
            return null;
        }
        private void 匹配参数(string X, I指令 指令) {
            var 整段指令 = X.Split(' ');//带引导词
            var 参数数量 = 整段指令.Length - 1;
            var 所有字段 = 指令.GetType().GetProperties().Where(t => t.GetCustomAttribute<排序Attribute>() != null);
            if (参数数量 != 所有字段.Count()) {
                PrintWarning($"指令【{指令}】的参数数量不对，需要{所有字段.Count()}个参数，实际{参数数量}个", LocalStorage.System);
                return;
            }
            for (int i = 1; i < 整段指令.Length; i++) {
                var 当前字段 = 所有字段.Find(t => t.Name == 指令.指令参数()[i - 1]);
                var value = 转化器[当前字段.PropertyType]?.Invoke(整段指令[i]);
                if (value == null) {
                    PrintWarning($"参数{当前字段.Name}的类型从string到{当前字段.PropertyType}转化失败，可能是因为并未注册转化器、或者转化器运行结果为null", LocalStorage.System);
                    return;
                }
                当前字段.SetValue(指令, value);
            }
            指令.Invoke();
        }
    }
}