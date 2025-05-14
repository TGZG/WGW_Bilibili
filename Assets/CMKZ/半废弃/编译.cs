using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;

/* 四则运算解析。本类用于：
 * 1.开发网游外挂时，解析【定义：选择总览 X=左键单击 100+50*X 200】
 * 2.符文树中，解析数学表达式。
 */
namespace CMKZ {
    public static partial class LocalStorage {
        /// <summary>
        /// 代码中必须有一个静态类Program，其中必须有一个静态方法Main
        /// </summary>
        public static void RunCode(this string X) {
            var 编译器 = CSharpCompilation.Create("Program")
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                .AddReferences(AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic).Select(a => MetadataReference.CreateFromFile(a.Location)))
                .AddSyntaxTrees(CSharpSyntaxTree.ParseText(X));
            using var 二进制数据流 = new MemoryStream();
            var 编译结果 = 编译器.Emit(二进制数据流);
            if (!编译结果.Success) {
                PrintWarning("代码编译错误：" + 编译结果.Diagnostics.First());
                return;
            }
            二进制数据流.Position = 0;  // 读取位置重置到开头
            Assembly.Load(二进制数据流.GetBuffer())?.GetType("Program")?.GetMethod("Main", BindingFlags.Static | BindingFlags.Public)?.Invoke(null, null);
        }
        public static void RunCode_顶级语句(this string code) {
            // 创建脚本选项
            var options = ScriptOptions.Default
                .AddImports("System")
                .AddReferences(AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic));
            // 解析语法树
            var syntaxTree = SyntaxFactory.ParseSyntaxTree(code);
            // 获取顶级语句
            var compilationUnit = syntaxTree.GetCompilationUnitRoot();
            var statements = compilationUnit.Members.OfType<Microsoft.CodeAnalysis.CSharp.Syntax.GlobalStatementSyntax>();
            // 执行顶级语句
            foreach (var statement in statements) {
                try {
                    var script = CSharpScript.Create(statement.ToString(), options);
                    script.RunAsync().Wait();
                    //Console.WriteLine("顶级语句执行成功！");
                } catch (CompilationErrorException ex) {
                    foreach (var diagnostic in ex.Diagnostics) {
                        //    Console.WriteLine("顶级语句执行错误：" + diagnostic);
                    }
                } catch (Exception) {
                    //Console.WriteLine("顶级语句执行错误：" + ex.Message);
                }
            }
        }
        public static void RunCodeDemo() {
            var code = @"
                using System;
                using CMKZ;
                using static CMKZ.LocalStorage;
                public static class Program{
                    public static void Main(){
                        Print(""Hello World!"");
                        MainPanel.创建矩形(""100 100 500 500"").SetColor(255, 255, 255, 0.5f);
                    }
                }
            ";
            code.RunCode();
        }
        public static double Calculate(this string input) {
            return ExpressionEvaluator.Calculate(input);
        }
        public static float Calculate0(string input) {
            var _precedence = new Dictionary<char, int> { { '+', 1 }, { '-', 1 }, { '*', 2 }, { '/', 2 }, { '(', 0 } };
            if (input.IndexOfAny(new char[] { '+', '-', '*', '/' }) == -1) {
                return float.Parse(input);
            }
            input = input.Replace(" ", "");
            var values = new Stack<float>();
            var ops = new Stack<char>();
            for (int i = 0; i < input.Length; i++) {
                if (char.IsDigit(input[i])) {
                    string value = string.Empty;
                    while (i < input.Length && (char.IsDigit(input[i]) || input[i] == '.')) value += input[i++];
                    values.Push(float.Parse(value));
                    i--;
                } else if (input[i] == '(') {
                    ops.Push(input[i]);
                } else if (input[i] == ')') {
                    while (ops.Peek() != '(') values.Push(ApplyOp(ops.Pop(), values.Pop(), values.Pop()));
                    ops.Pop();
                } else if (_precedence.ContainsKey(input[i])) {
                    while (ops.Count > 0 && _precedence[ops.Peek()] >= _precedence[input[i]]) values.Push(ApplyOp(ops.Pop(), values.Pop(), values.Pop()));
                    ops.Push(input[i]);
                }
            }
            while (ops.Count > 0) values.Push(ApplyOp(ops.Pop(), values.Pop(), values.Pop()));
            return values.Pop();
            static float ApplyOp(char op, float b, float a) {
                switch (op) {
                    case '+': return a + b;
                    case '-': return a - b;
                    case '*': return a * b;
                    case '/':
                        if (b == 0) throw new DivideByZeroException();
                        return a / b;
                }
                return 0;
            }
        }
        public static string NumToChinese(string x) {
            string[] pArrayNum = { "零", "一", "二", "三", "四", "五", "六", "七", "八", "九" };
            string[] pArrayDigit = { "", "十", "百", "千" };
            string[] pArrayUnits = { "", "万", "亿", "万亿" };
            var pStrReturnValue = ""; //返回值
            var finger = 0; //字符位置指针
            var pIntM = x.Length % 4; //取模
            int pIntK;
            if (pIntM > 0)
                pIntK = x.Length / 4 + 1;
            else
                pIntK = x.Length / 4;
            for (var i = pIntK; i > 0; i--) {
                var pIntL = 4;
                if (i == pIntK && pIntM != 0)
                    pIntL = pIntM;
                var four = x.Substring(finger, pIntL);
                var P_int_l = four.Length;
                for (int j = 0; j < P_int_l; j++) {
                    int n = Convert.ToInt32(four.Substring(j, 1));
                    if (n == 0) {
                        if (j < P_int_l - 1 && Convert.ToInt32(four.Substring(j + 1, 1)) > 0 && !pStrReturnValue.EndsWith(pArrayNum[n])) {
                            pStrReturnValue += pArrayNum[n];
                        }
                    } else {
                        if (!(n == 1 && (pStrReturnValue.EndsWith(pArrayNum[0]) | pStrReturnValue.Length == 0) && j == P_int_l - 2))
                            pStrReturnValue += pArrayNum[n];
                        pStrReturnValue += pArrayDigit[P_int_l - j - 1];
                    }
                }
                finger += pIntL;
                if (i < pIntK) {
                    if (Convert.ToInt32(four) != 0) {
                        pStrReturnValue += pArrayUnits[i - 1];
                    }
                } else {
                    pStrReturnValue += pArrayUnits[i - 1];
                }
            }
            return pStrReturnValue;
        }
    }
    public static partial class LocalStorage {
        public static void 半成品编译器程序() {

            var 背景 = MainPanel.创建矩形("10 10 100%-20 100%-20");

            var 编译按钮 = 背景.创建矩形(new PanelConfig() {
                矩形模式 = 矩形模式.固定文本框,
                Position = "100%-130 100%-70 120 30",
                TextAlign = TMPro.TextAlignmentOptions.Center
            }).SetColors(Colors.Default).SetText("编译");
            var 检测报错按钮 = 背景.创建矩形(new PanelConfig() {
                矩形模式 = 矩形模式.固定文本框,
                Position = "100%-130 100%-30 120 30",
                TextAlign = TMPro.TextAlignmentOptions.Center
            }).SetColors(Colors.Default).SetText("检测报错");
            var 输入框 = 背景.创建矩形(new PanelConfig() {
                矩形模式 = 矩形模式.固定多行输入框,
                Position = "0 40 100% 100%-240",
            }).SetColor(255, 255, 255, 0.5f);
            var 文件名输入框 = 背景.创建矩形(new PanelConfig() {
                矩形模式 = 矩形模式.固定多行输入框,
                Position = "0 0 100% 30",
                TextAlign = TMPro.TextAlignmentOptions.Left
            }).SetColor(255, 255, 255, 0.5f).SetText("P.X");
            var 报错消息 = 背景.创建矩形(new PanelConfig() {
                矩形模式 = 矩形模式.固定多行输入框,
                Position = "0 100%-190 100%-150 190",
            }).SetColor(255, 255, 255, 0.5f).SetText("无错误");
            var 清空报错消息按钮 = 背景.创建矩形(new PanelConfig() {
                矩形模式 = 矩形模式.固定文本框,
                Position = "100%-130 100%-110 120 30",
                TextAlign = TMPro.TextAlignmentOptions.Center
            }).SetColors(Colors.Default).SetText("清空报错消息");

            编译按钮.OnClick(t => {
                输入框.GetText().编译并运行(文件名输入框.GetText());
            });
            检测报错按钮.OnClick(t => {
                var Alert = new Alert_NoButton("加载中……");
                var A = 输入框.GetText().编译并获得报错消息(文件名输入框.GetText());
                Alert.Destroy();
                //在报错消息里显示
                if (A == null) {
                    报错消息.SetText(报错消息.GetText() + "\n无错误");
                    return;
                }
                报错消息.SetText($"{报错消息.GetText()}+\n===报错===\n{A.Join("\n")}");
            });
            清空报错消息按钮.OnClick(t => {
                报错消息.SetText("");
            });
        }
        //public static void Execute(string X) {
        //    var Y = CSharpScript.Create(X);
        //    //向Y导入当前程序集
        //    Y = Y.WithOptions(Y.Options.WithReferences(Assembly.GetExecutingAssembly()));
        //    var Z = Y.Compile();
        //    if (Z.Any()) {
        //        foreach (var error in Z) UnityEngine.Debug.LogError($"编译时报错：{error}");
        //    } else Y.RunAsync();
        //}
        public static CMKZ.List<PortableExecutableReference> 动态加载的程序集 = new();
        public static void 编译并运行(this string X, string 文件名) {
            var 内存块 = new MemoryStream();
            var DLL = 获取程序域的所有DLL();
            //创建编译集、将此编译集设置为动态链接库（允许其他程序使用此程序中的变量）、并向编译集中添加语法树（语法树是一个抽象语法树，用于表示源代码，如果没有语法树，编译器无法解析代码。语法数就是解析器。）
            var 编译集 = CSharpCompilation.Create(文件名)
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                .AddReferences(DLL)
                .AddSyntaxTrees(CSharpSyntaxTree.ParseText(X));
            编译集.编译(内存块);
            内存块.Seek(0, SeekOrigin.Begin);
            //获得命名空间内的类型
            var A = Assembly.Load(内存块.ToArray());
            var types = A.GetTypes();
            foreach (var type in types) {
                Print(type.Name);
                if (type.Name == "Script") type.GetMethod("Main").Invoke(null, null);
            }
            内存块.Seek(0, SeekOrigin.Begin);
            动态加载的程序集.Add(MetadataReference.CreateFromStream(内存块));//将编译结果添加到动态加载的程序集中
        }
        private static void 编译(this CSharpCompilation X, MemoryStream Y) {
            var 编译结果 = X.Emit(Y);//将编译集编译到内存中
            if (!编译结果.Success) {//检查编译错误
                foreach (var error in 编译结果.Diagnostics) UnityEngine.Debug.LogError($"编译时报错：{error}");
                return;
            }
        }
        public static string[] 编译并获得报错消息(this string X, string 文件名) {
            var 内存块 = new MemoryStream();
            var DLL = 获取程序域的所有DLL();
            //创建编译集、将此编译集设置为动态链接库（允许其他程序使用此程序中的变量）、并向编译集中添加语法树（语法树是一个抽象语法树，用于表示源代码，如果没有语法树，编译器无法解析代码。语法数就是解析器。）
            var 编译集 = CSharpCompilation.Create(文件名)
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                .AddReferences(DLL)
                .AddSyntaxTrees(CSharpSyntaxTree.ParseText(X));
            var 编译结果 = 编译集.Emit(内存块);//将编译集编译到内存中
            if (!编译结果.Success) return 编译结果.Diagnostics.Select(t => t.ToString()).ToArray();
            return null;
        }
        public static IEnumerable<PortableExecutableReference> 获取程序域的所有DLL() {
            //获取程序集引用
            int i = 0;
            return AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.IsDynamic)
                .Select(DLL => {
                    try {
                        var A = MetadataReference.CreateFromFile(DLL.Location);
                        return A;
                    } catch (Exception e) {
                        if (e.Message == "Path is empty") {
                            var A = 动态加载的程序集[i];
                            i++;
                            return A;
                        } else {
                            throw new Exception("程序域DLL异常！有未知DLL加载！（此DLL在本地找不到路径，且非CMKZ动态加载！）");
                        }
                    }
                });
        }
        //public class Script {
        //    public static void Main() {
        //        UnityEngine.Debug.Log("Hello World!");
        //    }
        //}
    }
    public static class ExpressionEvaluator {
        private static readonly Dictionary<char, int> _precedence = new() { { '+', 1 }, { '-', 1 }, { '*', 2 }, { '/', 2 }, { '(', 0 }};
        public static double Calculate(string input) {
            input = input.Replace(" ", "");
            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentException($"算式不可为空。十六进制：{input.ToHex()}");
            if (!Regex.IsMatch(input, @"^[\d\+\-\*\/\(\)\.]+$"))
                throw new ArgumentException($"算式中存在非法字符：{input}");
            var values = new Stack<double>();
            var ops = new Stack<char>();
            for (int i = 0; i < input.Length; i++) {
                if (char.IsDigit(input[i]) || input[i] == '.') {
                    string value = string.Empty;
                    while (i < input.Length && (char.IsDigit(input[i]) || input[i] == '.'))
                        value += input[i++];
                    values.Push(double.Parse(value));
                    i--;
                } else if (input[i] == '(') {
                    ops.Push(input[i]);
                } else if (input[i] == ')') {
                    while (ops.Peek() != '(')
                        values.Push(ApplyOp(ops.Pop(), values.Pop(), values.Pop()));
                    ops.Pop();
                } else if (_precedence.ContainsKey(input[i])) {
                    // Handle negative numbers
                    if (input[i] == '-' && (i == 0 || input[i - 1] == '(' || _precedence.ContainsKey(input[i - 1]))) {
                        string value = "-";
                        i++;
                        while (i < input.Length && (char.IsDigit(input[i]) || input[i] == '.'))
                            value += input[i++];
                        values.Push(double.Parse(value));
                        i--;
                    } else {
                        while (ops.Count > 0 && _precedence[ops.Peek()] >= _precedence[input[i]])
                            values.Push(ApplyOp(ops.Pop(), values.Pop(), values.Pop()));
                        ops.Push(input[i]);
                    }
                }
            }
            while (ops.Count > 0)
                values.Push(ApplyOp(ops.Pop(), values.Pop(), values.Pop()));
            return values.Pop();
        }
        private static double ApplyOp(char op, double b, double a) {
            return op switch {
                '+' => a + b,
                '-' => a - b,
                '*' => a * b,
                '/' => b == 0 ? throw new DivideByZeroException() : a / b,
                _ => throw new ArgumentException($"意外的符号：{op}")
            };
        }
    }
}