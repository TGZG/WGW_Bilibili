using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
namespace CMKZ {
    public static partial class LocalStorage {
        public static string 默认色str = "</color>";
        public static string 黑色str = "<color=#000000>";
        public static string 蓝色str = "<color=#0000FF>";
        public static string 淡蓝色str = "<color=#286795>";
        public static string 暗蓝色str = "<color=#0000A0>";
        public static string 浅蓝色str = "<color=#ADD8E6>";
        public static string 红色str = "<color=#FF0000>";
        public static string 橙色str = "<color=#FFA500>";
        public static string 黄色str = "<color=#FFFF00>";
        public static string 绿色str = "<color=#00FF00>";
        public static string 紫色str = "<color=#FF00FF>";
        public static string 白色str = "<color=#FFFFFF>";
        public static string 灰白色str = "<color=#C0C0C0>";
        public static string 灰色str = "<color=#808080>";
        public static string 金色str = "<color=#D9BE19>";//标准金色
        public static string 凡红色str = "<color=#bd3b3b>";
        public static string 凡绿色str = "<color=#40bd40>";
        public static string 文字大小(int size) => $"<size={size}>";
        public static string 默认大小 = "</size>";
        public static string 字色(string hex) => $"<color=#{hex}>";

        public static string 游戏字体颜色 = 白色str;

        public static Vector4 白色V4 = new Vector4(255, 255, 255, 1);
        public static Vector4 黑色V4 = new Vector4(0, 0, 0, 1);
        public static Vector4 红色V4 = new Vector4(255, 0, 0, 1);
        public static Vector4 暗红色V4 = new Vector4(128, 0, 0, 1);
        public static Vector4 绿色V4 = new Vector4(0, 255, 0, 1);
        public static Vector4 蓝色V4 = new Vector4(0, 0, 255, 1);
        public static Vector4 黄色V4 = new Vector4(255, 255, 0, 1);
        public static Vector4 紫色V4 = new Vector4(255, 0, 255, 1);
        public static Vector4 灰色V4 = new Vector4(128, 128, 128, 1);
        public static Vector4 灰白色V4 = new Vector4(192, 192, 192, 1);
        public static Vector4 金色V4 = new Vector4(217, 190, 25, 1);

        public static Color 透明 = new(1, 1, 1, 0);
        public static Color 黑一分_Shader = new(230, 230, 230, 0.01f);
        public static Color 黑二分_Shader = new(204, 204, 204, 0.01f);
        public static Color 黑三分_Shader = new(178, 178, 178, 0.01f);
        public static Color 黑四分_Shader = new(153, 153, 153, 0.01f);
        public static Color 黑五分_Shader = new(127, 127, 127, 0.01f);

        public static Color 黑一分 = new(0, 0, 1, 0.1f);
        public static Color 黑二分 = new(0, 0, 1, 0.2f);
        public static Color 黑三分 = new(0, 0, 1, 0.3f);
        public static Color 黑四分 = new(0, 0, 1, 0.4f);
        public static Color 黑五分 = new(0, 0, 1, 0.5f);

        public static Color 白一分 = new(255, 255, 255, 0.1f);
        public static Color 白二分 = new(255, 255, 255, 0.2f);
        public static Color 白三分 = new(255, 255, 255, 0.3f);
        public static Color 白四分 = new(255, 255, 255, 0.4f);
        public static Color 白五分 = new(255, 255, 255, 0.5f);

        public static Color 黄一分 = new(255, 255, 1, 0.1f);
        public static Color 黄二分 = new(255, 255, 1, 0.2f);
        public static Color 黄三分 = new(255, 255, 1, 0.3f);
        public static Color 黄四分 = new(255, 255, 1, 0.4f);
        public static Color 黄五分 = new(255, 255, 1, 0.5f);

        public static string 字号(int 数值) => $"<size={数值}>";

        public static string 获取相对位置大小(this GameObject A, GameObject B) {
            var A屏幕位置 = A.GetComponent<RectTransform>().position;
            var B屏幕位置 = B.GetComponent<RectTransform>().position;
            var A屏幕大小 = A.GetComponent<RectTransform>().rect.size;
            var B屏幕大小 = B.GetComponent<RectTransform>().rect.size;
            var 相对位置 = A屏幕位置 - B屏幕位置;
            var 相对大小 = A屏幕大小 / B屏幕大小;
            return $"{相对位置.x} {-相对位置.y} {相对大小.x * 100}% {相对大小.y * 100}%";
        }
        public static Vector2 获取相对位置(this GameObject X, GameObject Y) {
            var A = X.获取相对位置大小(Y).Split(" ");
            return new(A[0].ToFloat(), A[1].ToFloat());
        }
        //public static GameObject 动画调整(this GameObject A, double 时间, string 目标位置大小) {
        //    var P = 目标位置大小.Split(' ');
        //    var Q = A.GetComponent<MyTransform>();
        //    var 原始 = MyTransform.ParsePosition(Q.PanelConfig.Position);
        //    var 目标 = MyTransform.ParsePosition(目标位置大小);
        //    float X轴数值变化 = 目标[0] - 原始[0];
        //    float X轴百分比变化 = 目标[1] - 原始[1];
        //    float Y轴数值变化 = 目标[2] - 原始[2];
        //    float Y轴百分比变化 = 目标[3] - 原始[3];
        //    float 宽度数值变化 = 目标[4] - 原始[4];
        //    float 宽度百分比变化 = 目标[5] - 原始[5];
        //    float 高度数值变化 = 目标[6] - 原始[6];
        //    float 高度百分比变化 = 目标[7] - 原始[7];


        //    动画调整(时间, 比例 => {
        //        var 本帧所需X轴百分比变化 = X轴百分比变化 * 比例;
        //        var 本帧所需X轴数值变化 = X轴数值变化 * 比例;
        //        var 本帧所需Y轴百分比变化 = Y轴百分比变化 * 比例;
        //        var 本帧所需Y轴数值变化 = Y轴数值变化 * 比例;
        //        var 本帧所需宽度百分比变化 = 宽度百分比变化 * 比例;
        //        var 本帧所需宽度数值变化 = 宽度数值变化 * 比例;
        //        var 本帧所需高度百分比变化 = 高度百分比变化 * 比例;
        //        var 本帧所需高度数值变化 = 高度数值变化 * 比例;

        //        var 目标X位置数值 = 原始[0] + 本帧所需X轴数值变化;
        //        var 目标X位置比例 = 原始[1] + 本帧所需X轴百分比变化;
        //        var 目标Y位置数值 = 原始[2] + 本帧所需Y轴数值变化;
        //        var 目标Y位置比例 = 原始[3] + 本帧所需Y轴百分比变化;
        //        var 目标宽度数值 = 原始[4] + 本帧所需宽度数值变化;
        //        var 目标宽度比例 = 原始[5] + 本帧所需宽度百分比变化;
        //        var 目标高度数值 = 原始[6] + 本帧所需高度数值变化;
        //        var 目标高度比例 = 原始[7] + 本帧所需高度百分比变化;


        //        var 正负号1 = 原始[0] + 本帧所需X轴数值变化 < 0 ? "" : "+";
        //        var 正负号2 = 原始[2] + 本帧所需Y轴数值变化 < 0 ? "" : "+";
        //        var 正负号3 = 原始[4] + 本帧所需宽度数值变化 < 0 ? "" : "+";
        //        var 正负号4 = 原始[6] + 本帧所需高度数值变化 < 0 ? "" : "+";

        //        var X = $"{目标X位置比例 * 100}%{正负号1}{目标X位置数值}";
        //        var Y = $"{目标Y位置比例 * 100}%{正负号2}{目标Y位置数值}";
        //        var 宽度 = $"{目标宽度比例 * 100}%{正负号3}{目标宽度数值}";
        //        var 高度 = $"{目标高度比例 * 100}%{正负号4}{目标高度数值}";
        //        A.调整矩形($"{X} {Y} {宽度} {高度}");
        //        //计算所有值，如果所有值都达到或超过了目标值，返回true，并将所有值设置为目标值
        //        if (比例 >= 1) {
        //            A.调整矩形(目标位置大小);
        //            return true;
        //        }
        //        return false;
        //    });
        //    return A;
        //}
        //public static void 动画调整(double 时间, Func<double, bool> Update) {
        //    Action X = () => { };
        //    float 当前时间 = 0;
        //    X += () => {
        //        当前时间 += Time.deltaTime;
        //        if (Update(当前时间 / 时间)) {
        //            MainPanel.RemoveOnUpdate(X);
        //        }
        //    };
        //    MainPanel.OnUpdate(X);
        //}
    }
    public static partial class LocalStorage {
        //2D世界实验代码
        //创建地图(10, 10, "沙漠");
        //var 物体 = 创建物体(1, 1, "木墙").设置碰撞箱(true).锁定方向(true).显示文本框("你好");
        //var 角色 = 创建物体(1, 1, "木墙").设置碰撞箱(true).锁定方向(true).设置WASD移动(true).摄像机跟随(true);
        //var 建筑 = 创建物体(1, 1, "木墙").设置碰撞箱(true).锁定方向(true).锁定位置(true);
        //MainPanel.创建矩形("100 100 100 100").SetColor(255, 255, 255, 0.5f).允许缩放();
        public static GameObject 创建进度条(this GameObject X, int R, int G, int B, float A) {
            X.创建矩形("0 0 100% 100%").SetColor(R, G, B, A).SetName("进度条");
            return X;
        }
        public static GameObject 设置进度(this GameObject X, float Z) {
            X.Find("进度条").调整矩形($"0 0 {(int)(Z * 100)}% 100%");
            return X;
        }
        public static GameObject 创建滑条(this GameObject X, GameObject 滑条柄, Action<float> Y, float 最大值 = 1) {
            X.AddComponent<Slider>().handleRect = 滑条柄.GetComponent<RectTransform>();
            X.GetComponent<Slider>().maxValue = 最大值;
            X.GetComponent<Slider>().onValueChanged.AddListener(new UnityAction<float>(Y));
            return X;
        }
        public static GameObject 创建滑条(this GameObject X, string Y, Color COLOR, Action<float> Z, float 最大值 = 1) {
            X.AddComponent<Slider>().handleRect = X.创建矩形(Y).SetColor(COLOR).GetComponent<RectTransform>();
            X.GetComponent<Slider>().maxValue = 最大值;
            X.GetComponent<Slider>().onValueChanged.AddListener(new UnityAction<float>(Z));
            return X;
        }
        public static float 滑条数值(this GameObject X) {
            return X.GetComponent<Slider>().value;
        }
        /// <summary>
        /// 计算正态函数。（性能开销略大） X是自变量的值，返回值为f(X)。最大值是正态函数最高点的位置，最大位置是正态函数最高点的X值，半概率半径是当【正态函数值为最大值的一半】时【X和最大位置的距离】（精度0.00001）
        /// </summary>
        /// <param name="X">自变量的值，返回值为f(X)</param>
        /// <param name="最大值">正态函数最高点的位置</param>
        /// <param name="最大位置">正态函数最高点的X值</param>
        /// <param name="半概率半径">当正态函数值为最大值的一半时，X和最大位置的距离（精度0.00001）</param>
        /// <returns>正态函数的计算值</returns>
        public static float 正态函数_半概率半径(this float X, float 最大值, float 最大位置, float 半概率半径) {
            //数学公式表达式：（可在desmos中显示）
            //y=\frac{1}{a\ \sqrt{2\pi}}e^{-\frac{\left(\left(x-b\right)\cdot\frac{0.94012}{2c}\right)^{2}}{2a^{2}}}
            //其中：a=\frac{0.4}{d}

            var a = 0.4 / 最大值;
            var 半径乘数 = 0.94012 / (半概率半径 * 2);//0.94012是计算参数。当此项为【1.5/半概率半径】时，【半概率半径】为全概率半径。
            //半概率半径大约是全概率半径的三分之一。
            var 根号2π = Math.Sqrt(2 * Math.PI);

            var 底数 = Math.E / (a * 根号2π);
            var 指数 = -Math.Pow((X - 最大位置) * 半径乘数, 2) / (2 * Math.Pow(a, 2));
            return (float)Math.Pow(底数, 指数);
        }
        /// <summary>
        /// 计算正态函数。（性能开销略大） X是自变量的值，返回值为f(X)。最大值是正态函数最高点的位置，最大位置是正态函数最高点的X值，全概率半径是当【正态函数值几乎等于0时】时【X和最大位置的距离】
        /// </summary>
        /// <param name="X">自变量的值，返回值为f(X)</param>
        /// <param name="最大值">正态函数最高点的位置</param>
        /// <param name="最大位置">正态函数最高点的X值</param>
        /// <param name="全概率半径">当正态函数值几乎等于0时，X和最大位置的距离</param>
        /// <returns>正态函数的计算值</returns>
        public static float 正态函数_全概率半径(this float X, float 最大值, float 最大位置, float 全概率半径) {
            //数学公式表达式：（可在desmos中显示）
            //y=\frac{1}{a\ \sqrt{2\pi}}e^{-\frac{\left(\left(x-b\right)\cdot\frac{1.5}{c}\right)^{2}}{2a^{2}}}
            //其中：a=\frac{0.4}{d}

            var a = 0.4 / 最大值;
            var 半径乘数 = 1.5 / 全概率半径;
            var 根号2π = Math.Sqrt(2 * Math.PI);

            var 底数 = Math.E / (a * 根号2π);
            var 指数 = -Math.Pow((X - 最大位置) * 半径乘数, 2) / (2 * Math.Pow(a, 2));
            return (float)Math.Pow(底数, 指数);
        }
    }
    public class DataManager<T> {
        public T Data;
        public Action<T> BeforeSet;
        public Action<T> AfterSet;
        public DataManager() { }
        public DataManager(T X) {
            Data = X;
        }
        public T Get() {
            return Data;
        }
        public void Set(T X) {
            BeforeSet?.Invoke(Data);
            Data = X;
            AfterSet?.Invoke(Data);
        }
    }
}