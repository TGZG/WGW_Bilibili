using System;
using System.IO;
using TMPro;
using UnityEngine;
using static CMKZ.LocalStorage;

namespace CMKZ {
    public static partial class LocalStorage {
        public static bool Inside(this Vector2 坐标, Vector2 左上, Vector2 右下) {
            return 坐标.Inside(左上, new Vector2(左上.x, 右下.y), 右下, new Vector2(右下.x, 左上.y));
        }
        public static bool Inside(this Vector2 点, params Vector2[] 多边形点阵) {
            if (多边形点阵.Length < 3) {
                return false;
            }
            bool A = false;
            for (int i = 0, j = 多边形点阵.Length - 1; i < 多边形点阵.Length; j = i, i++) {
                var 线头 = 多边形点阵[i];
                var 线尾 = 多边形点阵[j];
                if (点.IsOnLine(线头, 线尾)) {
                    return true;
                }
                if (点.y < 线尾.y) {
                    if (线头.y <= 点.y) {
                        if ((点.y - 线头.y) * (线尾.x - 线头.x) >= (点.x - 线头.x) * (线尾.y - 线头.y)) {
                            A = !A;
                        }
                    }
                } else if (点.y < 线头.y) {
                    if ((点.y - 线头.y) * (线尾.x - 线头.x) <= (点.x - 线头.x) * (线尾.y - 线头.y)) {
                        A = !A;
                    }
                }
            }
            return A;
        }
        // 检查点是否在线段的范围内
        private static bool IsOnLine(this Vector2 点, Vector2 线头, Vector2 线尾) {
            if (点.x >= Math.Min(线头.x, 线尾.x) && 点.x <= Math.Max(线头.x, 线尾.x) &&
                点.y >= Math.Min(线头.y, 线尾.y) && 点.y <= Math.Max(线头.y, 线尾.y)) {
                return Math.Abs((线尾.y - 线头.y) * (点.x - 线头.x) - (线尾.x - 线头.x) * (点.y - 线头.y)) < 1e-7; // 检查叉积是否为0 允许一定的误差
            }
            return false;
        }
        public static GameObject 字体颜色(this GameObject X, Color color) {
            UnityEngine.Debug.Log("字体颜色修改！");
            //将颜色转换为十六进制数
            var 颜色 = ColorUtility.ToHtmlStringRGB(color);
            X.SetText($"<color=#{颜色}>{X.GetText().RemoveBetween("<", ">")}");
            return X;
        }
        public static string RemoveBetween(this string X, string start, string end) {
            int startIndex = X.IndexOf(start);
            int endIndex = X.IndexOf(end, startIndex + start.Length);
            if (startIndex != -1 && endIndex != -1) {
                return X.Remove(startIndex, endIndex - startIndex + end.Length);
            }
            return X;
        }
        //移除文本最后的对应字符
        public static string RemoveLast(this string X, string Y) {
            if (X.EndsWith(Y)) {
                return X.Remove(X.Length - Y.Length);
            }
            return X;
        }
        //找到两个字符串之间的内容
        public static string FindBetween(this string X, string start, string end) {
            int startIndex = X.IndexOf(start);
            int endIndex = X.IndexOf(end, startIndex + start.Length);
            if (startIndex != -1 && endIndex != -1) {
                return X.Substring(startIndex + start.Length, endIndex - startIndex - start.Length);
            }
            return X;
        }
        public static KeyValueList<Action, double> 延迟执行列表 = new();
        public static void 秒后(this Double X, Action action) {
            延迟执行列表.Add(action, X);
        }
        public static void 秒后(this int X, Action action) {
            ((double)X).秒后(action);
        }
        [初始化函数(typeof(CMKZProject))]
        public static void 初始化延迟() {
            MainPanel.OnUpdate(() => {
                List<Action> A = new();
                延迟执行列表.ForEach(i => {
                    i.Value -= Time.deltaTime;
                    if (i.Value <= 0) {
                        i.Key?.Invoke();
                        A.Add(() => 延迟执行列表.Remove(i));
                    }
                });
                A.ForEach(i => i?.Invoke());
            });
        }
        public static void 每_秒(this int X, Action action) {
            ((double)X).每_秒(action);
        }
        public static void 每_秒(this double X, Action Y) {
            double time = 0;
            每帧(() => {
                time += Time.deltaTime;
                if (time >= X) {
                    time = 0;
                    Y?.Invoke();
                }
            });
        }
        public static void 每秒(Action action) {
            1.每_秒(action);
        }
        public class Double2 {
            public double X;
            public double Y;
            public Double2(double x, double y) {
                X = x;
                Y = y;
            }
            public override string ToString() {
                return $"({X} {Y})";
            }
            //自动转换为Vector2
            public static implicit operator Vector2(Double2 X) {
                return new((float)X.X, (float)X.Y);
            }
        }



        public static List<Action> 每帧执行列表 = new();
        public static void 每帧(Action X) {
            每帧执行列表.Add(X);
        }
        public static void 移除每帧(Action X) {
            每帧执行列表.Remove(X);
        }
        [初始化函数(typeof(CMKZProject))]
        public static void 初始化每帧() {
            MainPanel.OnUpdate(() => {
                每帧执行列表.ForEach(i => i?.Invoke());
            });
        }
        public static List<Action> 下帧执行列表 = new();
        public static void 下帧(Action X) {
            下帧执行列表.Add(X);
        }
        public static void 移除下帧(Action X) {
            下帧执行列表.Remove(X);
        }
        [初始化函数(typeof(CMKZProject))]
        public static void 初始化下帧() {
            MainPanel.OnLateUpdate(() => {
                下帧执行列表.ForEach(i => i?.Invoke());
                下帧执行列表.Clear();
            });
        }

        public static TimeSpan ToTimeSpan(this double X) {
            return TimeSpan.FromSeconds(X);
        }
        public static TimeSpan ToTimeSpan(this float X) {
            return TimeSpan.FromSeconds(X);
        }
        public static DateTime 当前时间 => DateTime.Now;
        public static string 动态转化为年月日时分秒(this TimeSpan X) {
            return To时间(X.TotalSeconds);
        }
        public static string To时间(this double time) {
            //将秒数转化为datetime
            //根据分钟、时、天、月来显示
            var A = TimeSpan.FromSeconds(time);
            if (A.TotalDays > 1) {
                return $"{A.Days}天{A.Hours}时";
            } else if (A.TotalHours > 1) {
                return $"{A.Hours}时{A.Minutes}分";
            } else if (A.TotalMinutes > 1) {
                return $"{A.Minutes}分{A.Seconds}秒";
            } else {
                return $"{A.Seconds}秒";
            }
        }
        public static T CloneObject<T>(this T X) {
            return X.JsonSerialize().JsonDeserialize<T>();
        }
        public static bool 显示窗口(string 窗口标题) {
            IntPtr windowHandle = FindWindow(null, 窗口标题);
            if (windowHandle != IntPtr.Zero) {
                //0隐藏窗口。
                //1正常显示窗口。
                //2最小化窗口。
                //3最大化窗口。
                //4正常显示窗口，但不激活窗口。
                //5正常显示窗口，激活窗口。
                //6最小化窗口。
                //7最小化窗口，但不激活窗口。
                //8正常显示窗口，但不激活窗口。
                //9还原窗口。
                ShowWindow(windowHandle, 9);
                SetForegroundWindow(windowHandle);
                return true;
            }
            throw new Exception("找不到窗口：" + 窗口标题);
        }
        public static List<string> 所有可打开的窗口() {
            List<string> windowTitles = new List<string>();
            EnumWindows((hWnd, lParam) => {
                if (IsWindowVisible(hWnd)) {
                    int length = GetWindowTextLength(hWnd);
                    if (length > 0) {
                        char[] title = new char[length + 1];
                        GetWindowText(hWnd, title, title.Length);
                        windowTitles.Add(new string(title));
                    }
                }
                return true;
            }, IntPtr.Zero);
            foreach (var item in windowTitles) {
                Print(item);
            }
            return windowTitles;
        }
        public static Texture2D 加载图片(string 图片路径) {
            var A = new Texture2D(1, 1);
            A.LoadImage(File.ReadAllBytes(图片路径));
            Texture2D texture = new Texture2D(A.width, A.height, TextureFormat.ARGB32, false);
            texture.SetPixels32(A.GetPixels32());
            texture.Apply();
            return texture;
        }
        public static string 图片识别文字(string 语言代码, Texture2D texture) {
            //语言代码：chi_sim 简体中文
            //语言代码：eng 英文
            //其他语言代码需要安装对应的语言包，语言包下载地址：https://codeload.github.com/tesseract-ocr/tessdata/zip/refs/heads/main
            var OCR程序 = new TesseractWrapper();
            string 模型文件夹 = Path.Combine(Application.streamingAssetsPath, "tessdata");

            if (OCR程序.Init(语言代码, 模型文件夹)) return OCR程序.Recognize(texture);
            else throw new Exception(OCR程序.GetErrorMessage());
        }

        public static bool Contains(this string X, string[] Y) {
            foreach (var item in Y) {
                if (X.Contains(item)) return true;
            }
            return false;
        }
        public static bool Contains(this string[] X, string Y) {
            foreach (var item in X) {
                if (item == Y) return true;
            }
            return false;
        }
    }
    public class Alert_NoButton {
        public GameObject gameObject;
        public Alert_NoButton(string Text, Action<Alert_NoButton> T = null) {
            gameObject = MainPanel.创建矩形(new PanelConfig() {
                Position = "50%-100 50%-50 200 100",
                矩形模式 = 矩形模式.固定文本框,
                Font = "黑体",
                ImageColor = new(255, 255, 255, 0.5f),
                Margin = new(5, 5, 5, 5),
                TextAlign = TextAlignmentOptions.Center
            }).SetText(Text);
            if (T != null) T(this);
        }
        public void Destroy() {
            gameObject.Destroy();
        }
    }

    /// <summary>
    /// TimeLine是一个时间轴，可以用来添加延时事件、循环事件等。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TimeLine<T> {
        public T 父;
        public TimeLine(T 父) {
            this.父 = 父;
        }
        public List<TimeLineItem<T>> 时刻列表 = new();
        public void AddLater(double 秒数, Action<T> 运行事件) {
            AddTimeLine(new TimeLineItem<T> {
                名称 = "无名延时事件",
                类型 = 时延类型.一次性,
                时间 = new(秒数),
                F = 运行事件,
            });
        }
        public void AddLoop(double 秒数, Action<T> 运行事件) {
            AddTimeLine(new TimeLineItem<T> {
                名称 = "无名循环事件",
                类型 = 时延类型.循环,
                时间 = new(秒数),
                F = 运行事件,
            });
        }
        public void AddLoop(double 秒数, string 名称, Action<T> 运行事件) {
            AddTimeLine(new TimeLineItem<T> {
                名称 = "无名循环事件",
                类型 = 时延类型.循环,
                时间 = new(秒数),
                F = 运行事件,
            });
        }
        public void AddTimeLine(TimeLineItem<T> X) {
            X.时间.当前 = 0;
            时刻列表.Add(X);
            X.OnAwake?.Invoke(父);
        }
        public void Update() {
            foreach (var i in 时刻列表.ToArray()) {
                if (i.时间.已满) {
                    if (i.类型 == 时延类型.一次性) {
                        i.Invoke(父);
                        i.OnDispose?.Invoke(父);
                        时刻列表.Remove(i);
                    } else {
                        i.Invoke(父);
                    }
                    i.时间.SetToMin();
                }
                i.时间.增加(Time.deltaTime);
            }
        }
        public void Stop() {
            foreach (var i in 时刻列表) {
                i.OnDispose?.Invoke(父);
            }
            时刻列表.Clear();
        }
        public TimeLineItem<T> FindItem(string 名称) {
            if (string.IsNullOrEmpty(名称)) return null;
            if (时刻列表 == null) return null;
            if (名称 == "无名循环事件") throw new Exception("无法使用默认名称获取循环");
            foreach (var i in 时刻列表) {
                if (i.名称 == 名称) {
                    return i;
                }
            }
            return null;
        }
        public 限数 获取剩余时间(string 名称) {
            var i = FindItem(名称);
            if (i != null) {
                return i.时间;
            }
            return null;
        }
    }
    public class TimeLineItem<T> : Function<T> {
        public string 名称;
        public 时延类型 类型;
        public 限数 时间;
        public Action<T> OnAwake;//添加时执行
        public Action<T> OnDispose;//结束或意外关闭时执行，用于释放资源
        public static TimeLineItem<T> operator +(TimeLineItem<T> X, Action<T> Y) {
            X.F += Y;
            return X;
        }
    }
    public class Function {
        public Action F;
        public Function() {

        }
        public Function(Action X) {
            F = X;
        }
        public void Invoke() {
            F?.Invoke();
        }
    }
    public class Function<T> {
        public Action<T> F;
        public Function() {

        }
        public Function(Action<T> X) {
            F = X;
        }
        public void Invoke(T X) {
            F?.Invoke(X);
        }
        public void Add(Action<T> X) {
            F += X;
        }
    }
    public enum 时延类型 {
        一次性,
        循环,
    }
}