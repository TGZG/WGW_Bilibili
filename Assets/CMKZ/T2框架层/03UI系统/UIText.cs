using Microsoft.CodeAnalysis;
using Newtonsoft.Json;//Json
using System;//Action
using System.IO;//File
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;//List
using System.Diagnostics;
using System.Linq;//from XX select XX
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Timers;//Timer
using TMPro;//InputField
using UnityEngine;//Mono
using UnityEngine.UI;//Image
using UnityEngine.Tilemaps;
using UnityEngine.Video;//Vedio
using static UnityEngine.Object;//Destory
using static CMKZ.LocalStorage;
using static UnityEngine.RectTransform;
using Random = UnityEngine.Random;

/**
 * 本文件包括：
 * 1.给GameObject设置文本，读取文本
 * 2.封装UnityUI，创建静态文本框或动态输入框
 */
namespace CMKZ {
    public static partial class LocalStorage {
        /// <summary>
        /// 注意：只可以用于普通文本框，不可以用于输入框
        /// </summary>
        public static GameObject SetText_Fast(this GameObject X, string Y) {
            X.GetComponentInChildren<TextMeshProUGUI>().text = Y;
            return X;
        }
        [Obsolete("性能太差，已经弃用")]
        public static GameObject SetText(this GameObject X, string Y) {
            if (X.GetComponent<TMP_InputField>() != null) {
                X.GetComponent<TMP_InputField>().text = Y;
            } else {
                X.GetComponentInChildren<TextMeshProUGUI>().text = Y;
                X.Find("Text")?.transform.SetAsFirstSibling();//作用：不要遮挡文本上的可点击图标
            }
            if (X.GetParent().activeSelf) {
                if (X.GetComponent<ContentSizeFitter>() != null) {
                    if (X.GetParent().GetComponent<RectTransform>() != null) {
                        LayoutRebuilder.ForceRebuildLayoutImmediate(X.GetParent().GetComponent<RectTransform>());
                    }
                }
                var A = X.GetComponentInParent<ScrollRect>();
                if (A != null) {
                    LayoutRebuilder.ForceRebuildLayoutImmediate(A.gameObject.Find("Viewport/Content").GetComponent<RectTransform>());
                    //A.verticalNormalizedPosition = 0;//滚动到底部
                }
            }
            return X;
        }
        public static string GetText(this GameObject X) {
            return X.GetComponentInChildren<TextMeshProUGUI>().text.不可见清除();
        }
        public static void AddText(this GameObject X, string Y) {
            X.SetText(X.GetText() + Y);
        }
        public static GameObject SetTextKey(this GameObject X, string Y, PanelConfig Z) {
            X.SetText(Y);
            return X.Find("Text").创建矩形(Z);
        }

        public static GameObject SetChildText(this GameObject X, PanelConfig Y) {
            var A = X.创建矩形("0 0 100% 100%").SetName("Text");
            A.AddComponent<TextMeshProUGUI>().font = Fonts[Y.Font];
            A.GetComponent<TextMeshProUGUI>().fontSize = Y.TextSize;
            A.GetComponent<TextMeshProUGUI>().alignment = Y.TextAlign;
            A.GetComponent<TextMeshProUGUI>().color = Y.TextColor.ToColor();
            A.GetComponent<TextMeshProUGUI>().margin = Y.Margin;
            A.GetComponent<TextMeshProUGUI>().enableWordWrapping = true;//自动换行
            A.GetComponent<TextMeshProUGUI>().raycastTarget = false;//默认文本不遮挡鼠标事件
            //这段代码 导致文字不显示
            //A.GetComponent<TextMeshProUGUI>().overflowMode = TextOverflowModes.Truncate;
            A.GetComponent<TextMeshProUGUI>().text = Y.Text;
            return A;
        }
        public static GameObject SetChildText_单行(this GameObject X, PanelConfig Y) {
            var A = X.创建矩形("0 0 100% 100%").SetName("Text");
            A.AddComponent<TextMeshProUGUI>().font = Fonts[Y.Font];
            A.GetComponent<TextMeshProUGUI>().fontSize = Y.TextSize;
            A.GetComponent<TextMeshProUGUI>().alignment = Y.TextAlign;
            A.GetComponent<TextMeshProUGUI>().color = Y.TextColor.ToColor();
            A.GetComponent<TextMeshProUGUI>().margin = Y.Margin;
            A.GetComponent<TextMeshProUGUI>().enableWordWrapping = false;//单行，不换行
            A.GetComponent<TextMeshProUGUI>().raycastTarget = false;//默认文本不遮挡鼠标事件
            //这段代码 导致文字不显示
            //A.GetComponent<TextMeshProUGUI>().overflowMode = TextOverflowModes.Truncate;
            A.GetComponent<TextMeshProUGUI>().text = Y.Text;
            return A;
        }
        public static void SetChildTextAutoWidth(this GameObject X, PanelConfig Y) {
            var A = X.SetChildText(Y);
            A.AddComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            X.GetOrAddComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.MinSize;
            X.GetOrAddComponent<HorizontalLayoutGroup>().childControlWidth = false;
            X.GetOrAddComponent<HorizontalLayoutGroup>().childForceExpandWidth = false;
        }
        public static void SetChildTextAutoHeight(this GameObject X, PanelConfig Y) {
            var A = X.SetChildText(Y);
            A.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            X.GetOrAddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.MinSize;
            X.GetOrAddComponent<VerticalLayoutGroup>().childControlHeight = false;
            X.GetOrAddComponent<VerticalLayoutGroup>().childForceExpandHeight = false;
        }
        public static void SetInput(this GameObject X, PanelConfig Y) {
            var A = X.创建矩形(Y.Clone().SetModel(矩形模式.固定文本框).SetPosition("0 0 100% 100%").SetColor(default));
            X.AddComponent<RectMask2D>();
            X.AddComponent<UIEvent>();
            X.AddComponent<TMP_InputField>().lineType = TMP_InputField.LineType.MultiLineNewline;//回车后新行
            X.GetComponent<TMP_InputField>().textViewport = A.GetComponent<RectTransform>();
            X.GetComponent<TMP_InputField>().textComponent = A.GetComponentInChildren<TextMeshProUGUI>();
            X.GetComponent<TMP_InputField>().onFocusSelectAll = false;
            X.ChangeActiveTwice();
        }
    }
}