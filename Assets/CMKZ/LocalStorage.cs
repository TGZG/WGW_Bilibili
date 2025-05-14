using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using static UnityEngine.Object;
using static CMKZ.LocalStorage;
using System.Reflection;
using TMPro;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Globalization;

namespace CMKZ {
    public static partial class LocalStorage {
        public static byte Hash(this string X) {
            return (byte)X.Sum(t => t);
        }
        //X最多四项。将四个byte拼成一个int
        //注意：X可能不足四项，不足的部分会被当作0
        public static int 组合(this byte[] X) {
            return X.Length switch {
                0 => 0,
                1 => X[0] << 24,
                2 => X[0] << 24 | X[1] << 16,
                3 => X[0] << 24 | X[1] << 16 | X[2] << 8,
                _ => X[0] << 24 | X[1] << 16 | X[2] << 8 | X[3],
            };
        }
        public static T[] Turn<T, T2>(this T2[] X, Func<T2, T> Y) {
            var A = new T[X.Length];
            for (int i = 0; i < X.Length; i++) {
                A[i] = Y(X[i]);
            }
            return A;
        }
        public static void Clear(this RectTransform X) {
            X.gameObject.Clear();
        }
        public static RectTransform GetParentRectTransform(this Component A) => A.transform.parent.GetComponent<RectTransform>();
        public static IEnumerable RemoveFirst(this IEnumerable A) {
            var B = true;
            foreach (var C in A) {
                if (B) {
                    B = false;
                    continue;
                }
                yield return C;
            }
        }
        public static uint ToUint(this string X) {
            if (uint.TryParse(X, out uint number)) {
                return number;
            }
            throw new Exception($"文本转uint失败：{X}");
        }
        public static Dictionary<string, int> 位数 = new() {
            ["K"] = 3,
            ["万"] = 4,
            ["M"] = 6,
            ["亿"] = 8,
            ["G"] = 9,
            ["万亿"] = 12,
            ["T"] = 12,
        };
        public static Number ToNumber(this string X) {
            if (double.TryParse(X, out double number)) {
                return number;
            }
            foreach (var entry in 位数.OrderByDescending(u => u.Key.Length)) {
                if (X.EndsWith(entry.Key)) {
                    var baseNumber = X[..^entry.Key.Length].Trim();
                    if (double.TryParse(baseNumber, NumberStyles.Any, CultureInfo.InvariantCulture, out number)) {
                        return number * Math.Pow(10, entry.Value);
                    } else {
                        throw new Exception($"文本转数字失败：{baseNumber}");
                    }
                }
            }
            throw new Exception($"文本转数字失败，无单位匹配：{X}");
        }
        public static Color ToColor(this string X) {//FFFFFF
            return new Color(
                Convert.ToInt32(X.Substring(0, 2), 16) / 255f,
                Convert.ToInt32(X.Substring(2, 2), 16) / 255f,
                Convert.ToInt32(X.Substring(4, 2), 16) / 255f
            );
        }
        public static string Clear0(this string X) {
            return X == "0" ? "" : X;
        }
        public static void RemoveFirst<T>(this List<T> A) {
            A.RemoveAt(0);
        }
        public static GameObject 创建自动高度文本框(this GameObject 父物体, string 位置, Color 背景色, string 文本) {
            var A = Instantiate(AllPrefab["自动高度可选文本框"], 父物体.transform);
            A.SetPosition(位置);
            A.GetComponent<Image>().color = 背景色;
            //A.GetComponent<TMP_Text>().OnEnable();
            A.GetComponentInChildren<TextMeshProUGUI>().enableWordWrapping = true;
            A.GetComponentInChildren<TextMeshProUGUI>().color = !背景色.IsWhite() ? new(1, 1, 1) : new(0, 0, 0);
            A.GetComponentInChildren<Scrollbar>().GetComponent<Image>().color = !背景色.IsWhite() ? 背景色.AddForScroll(0.2f) : 背景色.AddForScroll(0f);
            A.GetComponentInChildren<Scrollbar>().colors = !背景色.IsWhite()
                ? new ColorBlock() {
                    normalColor = 背景色.AddForScroll(0.1f).Solid(),
                    highlightedColor = 背景色.AddForScroll(0.2f).Solid(),
                    pressedColor = 背景色.AddForScroll(0.3f).Solid(),
                    selectedColor = 背景色.AddForScroll(0.1f).Solid(),
                    disabledColor = new(0.5f, 0.5f, 0.5f),
                    colorMultiplier = 1,
                    fadeDuration = 0.1f
                }
                : new ColorBlock() {
                    normalColor = 背景色.AddForScroll(-0.1f),
                    highlightedColor = 背景色.AddForScroll(-0.2f),
                    pressedColor = 背景色.AddForScroll(-0.3f),
                    selectedColor = 背景色.AddForScroll(-0.1f),
                    disabledColor = new(0.5f, 0.5f, 0.5f),
                    colorMultiplier = 1,
                    fadeDuration = 0.1f
                };
            A.GetComponentInChildren<TMP_InputField>().text = 文本;
            return A;
        }
        public static Color Add(this Color A, float B) => new(A.r + B, A.g + B, A.b + B, A.a);
        public static Color Solid(this Color A) => new(A.r, A.g, A.b, 1);
        public static Color AddForScroll(this Color A, float B) => new(A.r + B, A.g + B, A.b + B, A.a);
        public static bool IsWhite(this Color A) => A.r + A.g + A.b > 1.5;
        /// <summary>
        /// t从0到1。t越接近1，最终值越更大的接近1
        /// </summary>
        public static float 标准凸函数(this float t) {
            return 1 - (1 - t) * (1 - t);
        }
        public static Color SetAlpha(this Color color, float alpha) {
            return new Color(color.r, color.g, color.b, alpha);
        }
        public static bool 辗转属于(this string 装备全名, IEnumerable<string> 掉落范围) {
            foreach (var i in 掉落范围) {
                if (Regex.Match(装备全名, i).Success) {
                    return true;
                }
            }
            return false;
        }
        public static int GetID(string path) {
            var ID = FileRead(path).IntAdd();
            FileWrite(path, ID);
            return ID;
        }
    }
}