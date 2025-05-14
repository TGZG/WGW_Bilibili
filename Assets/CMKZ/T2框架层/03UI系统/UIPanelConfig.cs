using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static CMKZ.LocalStorage;
using static UnityEngine.Object;

namespace CMKZ {
    public static partial class LocalStorage {
        public static GameObject 创建矩形(this GameObject X) {
            return new GameObject().SetParent(X);
        }
        public static GameObject 创建矩形(this GameObject X, PanelConfig Y) {
            return new GameObject().SetParent(X).AddComponent<MyTransform>().SetConfig(Y);
        }
        public static GameObject 创建矩形(this GameObject X, string Y) {
            return new GameObject().SetParent(X).AddComponent<MyTransform>().SetConfig(new PanelConfig { Position = Y });
        }
        public static GameObject 调整矩形(this GameObject X, ChangePanelConfig Y) {
            return X.GetComponent<MyTransform>().Change(Y);
        }
        public static GameObject 调整矩形(this GameObject X, string[] Y) {
            var A = X.GetComponent<MyTransform>().PanelConfig;
            A.Image = Y[0];
            A.悬浮.Image = Y[1];
            A.按下.Image = Y[2];
            if (X.GetComponent<MyTransform>().ColorObject.IsMouseHere()) {
                X.GetComponent<MyTransform>().ColorObject.SetSprite(A.悬浮.Image);
            } else {
                X.GetComponent<MyTransform>().ColorObject.SetSprite(A.Image);
            }
            return X;
        }
        public static GameObject 调整矩形(this GameObject X, string Y) {
            return X.GetComponent<MyTransform>().SetPosition(Y);
        }
        public static PanelConfig GetConfig(this GameObject X) {
            return X.GetComponent<MyTransform>().PanelConfig;
        }
    }
    public static partial class LocalStorage {
        public static Dictionary<string, TMP_FontAsset> _Fonts;
        public static Dictionary<string, TMP_FontAsset> Fonts {
            get {
                if (_Fonts == null) {
                    _Fonts = new Dictionary<string, TMP_FontAsset>();
                    _Fonts["F5"] = Resources.Load<TMP_FontAsset>("F5");
                    _Fonts["Gao"] = Resources.Load<TMP_FontAsset>("Gao");
                    _Fonts["黑体"] = Resources.Load<TMP_FontAsset>("黑体");
                    _Fonts["基督山伯爵"] = Resources.Load<TMP_FontAsset>("基督山伯爵");
                }
                return _Fonts;
            }
        }
        public static int 默认字号 = 16;
    }
    public static partial class LocalStorage {
        public static Dictionary<矩形模式, Action<MyTransform>> _矩形模式定义;
        public static Dictionary<矩形模式, Action<MyTransform>> 矩形模式定义 {
            get {
                if (_矩形模式定义 == null) {
                    _矩形模式定义 = new();
                    #region 矩形模式定义[矩形模式.固定文本框] = t => { }
                    矩形模式定义[矩形模式.固定文本框] = t => {
                        t.SetChildText();
                    };
                    矩形模式定义[矩形模式.固定单行文本框] = t => {
                        t.SetChildText_单行();
                    };
                    矩形模式定义[矩形模式.自动高度文本框] = t => {
                        t.SetChildTextAutoHeight();
                        t.SetLayoutElement();
                    };
                    矩形模式定义[矩形模式.自动宽度文本框] = t => {
                        t.SetChildTextAutoWidth();
                    };
                    矩形模式定义[矩形模式.固定多行输入框] = t => {
                        t.SetInput();
                    };
                    矩形模式定义[矩形模式.固定单行输入框] = t => {
                        t.SetInput();
                        t.gameObject.GetComponent<TMP_InputField>().lineType = TMP_InputField.LineType.SingleLine;//单行提交
                    };
                    矩形模式定义[矩形模式.固定可选文本框] = t => {
                        t.SetInput();
                        t.gameObject.GetComponent<TMP_InputField>().readOnly = true;
                    };
                    矩形模式定义[矩形模式.固定无文本] = t => {

                    };
                    矩形模式定义[矩形模式.固定均匀水平列表] = t => {
                        t.gameObject.AddComponent<HorizontalLayoutGroup>().childControlWidth = true;
                        t.gameObject.GetComponent<HorizontalLayoutGroup>().childControlHeight = true;
                        t.gameObject.GetComponent<HorizontalLayoutGroup>().childForceExpandWidth = true;
                        t.gameObject.GetComponent<HorizontalLayoutGroup>().childForceExpandHeight = true;
                        t.gameObject.GetComponent<HorizontalLayoutGroup>().spacing = t.PanelConfig.Spacing;
                        t.gameObject.GetComponent<HorizontalLayoutGroup>().padding = t.PanelConfig.Padding;
                    };
                    矩形模式定义[矩形模式.固定均匀垂直列表] = t => {
                        t.gameObject.AddComponent<VerticalLayoutGroup>().childControlWidth = true;
                        t.gameObject.GetComponent<VerticalLayoutGroup>().childControlHeight = true;
                        t.gameObject.GetComponent<VerticalLayoutGroup>().childForceExpandWidth = true;
                        t.gameObject.GetComponent<VerticalLayoutGroup>().childForceExpandHeight = true;
                        t.gameObject.GetComponent<VerticalLayoutGroup>().spacing = t.PanelConfig.Spacing;
                        t.gameObject.GetComponent<VerticalLayoutGroup>().padding = t.PanelConfig.Padding;
                    };
                    矩形模式定义[矩形模式.自动高度无文本] = t => {
                        t.SetHtmlLayout();
                        t.SetLayoutElement();
                    };
                    矩形模式定义[矩形模式.自动宽度无文本] = t => {
                        t.SetHorizontalLayout();
                    };
                    矩形模式定义[矩形模式.滚动条自动高度无文本] = t => {
                        t.SetScroll();
                        t.SetHtmlLayout();
                        t.ColorObject = t.gameObject;
                        t.Real.GetOrAddComponent<UIEvent>().SetScroll();
                    };
                    矩形模式定义[矩形模式.滚动条自动高度自动宽度无文本] = t => {
                        t.设置横纵滚动();
                        t.Real.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.MinSize;
                        t.Real.GetComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.MinSize;
                    };
                    矩形模式定义[矩形模式.滚动条自动高度文本框] = t => {
                        t.SetScroll();
                        t.SetHtmlLayout();
                        var 新样式 = t.PanelConfig.Clone();
                        新样式.矩形模式 = 矩形模式.自动高度文本框;
                        新样式.ImageColor = new(0, 0, 0, 0);
                        var A = t.Real.创建矩形(新样式).SetName("核心文本");
                        var B = t.Real.创建矩形(new PanelConfig { 矩形模式 = 矩形模式.固定无文本 }).SetName("垫尾");
                        B.AddComponent<LayoutElement>().preferredHeight = t.gameObject.GetComponent<RectTransform>().rect.height * 0.25f;
                        t.Real = A;
                        t.Real.GetOrAddComponent<UIEvent>().SetScroll();
                    };
                    //Content垂直布局，分为核心与垫材。核心为自动大小的InputField。
                    矩形模式定义[矩形模式.滚动条自动高度可选文本框] = t => {
                        t.SetScroll();
                        t.SetHtmlLayout();
                        var 新样式 = t.PanelConfig.Clone();
                        新样式.矩形模式 = 矩形模式.固定多行输入框;
                        新样式.ImageColor = new(0, 0, 0, 0);
                        var A = t.Real.创建矩形(新样式).SetName("核心文本");
                        var B = t.Real.创建矩形(new PanelConfig { 矩形模式 = 矩形模式.固定无文本 }).SetName("垫材");
                        B.AddComponent<LayoutElement>().preferredHeight = t.gameObject.GetComponent<RectTransform>().rect.height * 0.25f;
                        t.Real = A;
                        t.Real.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.MinSize;
                        t.Real.GetComponent<TMP_InputField>().readOnly = true;
                        t.Real.GetOrAddComponent<UIEvent>().SetScroll();
                    };
                    矩形模式定义[矩形模式.滚动条自动高度网格布局] = t => {
                        t.SetScroll();
                        t.Real.AddComponent<GridLayoutGroup>().padding = t.PanelConfig.Padding ?? new();
                        t.Real.GetComponent<GridLayoutGroup>().spacing = t.PanelConfig.GridSpacing;
                        t.Real.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.MinSize;
                        if (t.PanelConfig.GridSize != default) t.Real.GetComponent<GridLayoutGroup>().cellSize = t.PanelConfig.GridSize;
                        t.Real.GetOrAddComponent<UIEvent>().SetScroll();
                    };
                    #endregion
                }
                return _矩形模式定义;
            }
        }
    }
    public class PanelConfig {
        //Transform
        public 矩形模式 矩形模式 = 矩形模式.固定无文本;
        //常规属性
        public string Position;
        public Vector4 Element;//最小高度，最大高度，最小宽度，最大宽度
        public Vector4 ImageColor;
        public string Image;
        //Layout
        public RectOffset Padding = new();//矩形的偏移，左右上下
        public int Spacing;
        public Vector2 GridSpacing;
        public Vector2 GridSize;
        //Text
        public string Font = "F5";
        public float TextSize = 16;
        public TextAlignmentOptions TextAlign = TextAlignmentOptions.TopLeft;
        public Vector4 TextColor = new(0, 0, 0, 1);
        public Vector4 Margin;//文本的偏移
        public string Text;
        //Event
        public ChangePanelConfig 常规 {
            get {
                Init();
                return new ChangePanelConfig {
                    Position = Position,
                    Image = Image,
                    ImageColor = ImageColor,
                };
            }
        }
        public ChangePanelConfig 悬浮;
        public ChangePanelConfig 按下;
        public Colors Colors;
        public string[] Images;
        //函数
        public PanelConfig Clone() => ((PanelConfig)MemberwiseClone()).Init();
        public PanelConfig SetModel(矩形模式 X) {
            矩形模式 = X;
            return this;
        }
        public PanelConfig SetPosition(string X) {
            Position = X;
            return this;
        }
        public PanelConfig SetColor(Vector4 X) {
            ImageColor = X;
            return this;
        }
        public bool IsInit;
        public PanelConfig Init() {
            if (IsInit) return this;
            IsInit = true;
            if (Colors != default || Images != default) {
                if (悬浮 == null) {
                    悬浮 = new();
                    按下 = new();
                }
            }
            if (Colors != default) {
                ImageColor = Colors.常规颜色;
                悬浮.ImageColor = Colors.悬浮颜色;
                按下.ImageColor = Colors.按下颜色;
            }
            if (Images != default) {
                Image = Images[0];
                悬浮.Image = Images[1];
                按下.Image = Images[2];
            }
            return this;
        }
    }
    public class ChangePanelConfig {
        public string Position;
        public Vector4 ImageColor;
        public string Image;
    }
    public class MyTransform : MonoBehaviour {
        public PanelConfig PanelConfig = new();
        public GameObject Real;
        public GameObject ColorObject;
        public GameObject SetConfig(PanelConfig X) {
            PanelConfig = X.Clone();
            Draw();
            return Real;
        }
        public GameObject Change(ChangePanelConfig X) {
            SetPosition(X.Position);
            if (X.ImageColor != default) ColorObject.SetColor(X.ImageColor);
            if (X.Image != default) ColorObject.SetSprite(X.Image);
            return Real;
        }
        public void Draw() {
            Real = gameObject;
            ColorObject = gameObject;
            SetPosition(PanelConfig.Position);
            矩形模式定义[PanelConfig.矩形模式](this);
            if (PanelConfig.ImageColor != default) ColorObject.SetColor(PanelConfig.ImageColor);
            if (PanelConfig.Image != default) ColorObject.SetSprite(PanelConfig.Image);
            if (PanelConfig.悬浮 != default && PanelConfig.按下 != default) {
                Real.OnEnter(XX => {
                    if (Input.GetMouseButton(0)) return;
                    XX.GetComponent<MyTransform>().Change(PanelConfig.悬浮);
                });
                Real.OnMouseDown(XX => {
                    XX.GetComponent<MyTransform>().Change(PanelConfig.按下);
                });
                Real.OnMouseUp(XX => {
                    XX.GetComponent<MyTransform>().Change(PanelConfig.悬浮);
                });
                Real.OnExit(XX => {
                    if (Input.GetMouseButton(0)) return;
                    XX.GetComponent<MyTransform>().Change(PanelConfig.常规);
                });
                Real.OnEndDrag(XX => {
                    XX.GetComponent<MyTransform>().Change(PanelConfig.常规);
                });
                Real.OnDrop(XX => {
                    XX.GetComponent<MyTransform>().Change(PanelConfig.悬浮);
                });
            }
        }
        #region SetPosition();SetHtmlLayout();
        public PanelConfig 滚动条样式 = new() {
            Position = "0 0 100% 100%",
            ImageColor = new Vector4(255, 255, 255, 1)
        };
        public GameObject SetPosition(string X) {
            PanelConfig.Position = X;
            gameObject.GetOrAddComponent<RectTransform>().pivot = new Vector2(0, 1);
            if (X == null) return gameObject;
            if (Regex.Match(X, "[WH]").Success) {
                gameObject.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
                gameObject.GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);
                //gameObject.GetComponent<RectTransform>().offsetMin = new Vector2(0,0);
                //gameObject.GetComponent<RectTransform>().offsetMax = new Vector2(0,0);
                X = X.Replace("W", "*" + gameObject.transform.parent.GetComponent<RectTransform>().rect.width.ToString());
                X = X.Replace("H", "*" + gameObject.transform.parent.GetComponent<RectTransform>().rect.height.ToString());
                var A = X.Split(" ");
                gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(A[0].Calculate().ToFloat(), -A[1].Calculate().ToFloat());
                gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(A[2].Calculate().ToFloat(), A[3].Calculate().ToFloat());
            } else if (Regex.Match(X, "R").Success) {
                //var A = ParsePosition(X.Remove("R"));
                //gameObject.GetComponent<RectTransform>().anchorMin = new Vector2((-A[1] - A[5]).ToFloat(), (1 - A[3] - A[7]).ToFloat());//左比例+宽比例，1-上比例-高比例=下比例
                //gameObject.GetComponent<RectTransform>().anchorMax = new Vector2(-A[1].ToFloat(), (1 - A[3]).ToFloat());//右比例=右比例，1-上比例=顶比例
                //gameObject.GetComponent<RectTransform>().offsetMin = new Vector2((-A[0] - A[4]).ToFloat(), (-A[2] - A[6]).ToFloat());//右数值+宽数值=左数值，-上数值-高数值=下数值
                //gameObject.GetComponent<RectTransform>().offsetMax = new Vector2(-A[0].ToFloat(), -A[2].ToFloat());//右数值=右数值，-上数值=顶数值
            } else {
                var A = ParsePosition(X);
                gameObject.GetComponent<RectTransform>().anchorMin = new Vector2(A[1].ToFloat(), (1 - A[3] - A[7]).ToFloat());//左比例，1-上比例-高比例=下比例
                gameObject.GetComponent<RectTransform>().anchorMax = new Vector2((A[1] + A[5]).ToFloat(), (1 - A[3]).ToFloat());//左比例+宽比例=右比例，1-上比例=顶比例
                gameObject.GetComponent<RectTransform>().offsetMin = new Vector2(A[0].ToFloat(), (-A[2] - A[6]).ToFloat());//左数值，-上数值-高数值=下数值
                gameObject.GetComponent<RectTransform>().offsetMax = new Vector2((A[0] + A[4]).ToFloat(), -A[2].ToFloat());//左数值+宽数值=右数值，-上数值=顶数值
            }
            return gameObject;
        }
        public void SetHtmlLayout() {
            Real.AddComponent<VerticalLayoutGroup>().childControlWidth = true;
            Real.GetComponent<VerticalLayoutGroup>().childControlHeight = false;
            Real.GetComponent<VerticalLayoutGroup>().childForceExpandWidth = true;
            Real.GetComponent<VerticalLayoutGroup>().childForceExpandHeight = false;
            Real.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.MinSize;
            Real.GetComponent<VerticalLayoutGroup>().spacing = PanelConfig.Spacing;
            Real.GetComponent<VerticalLayoutGroup>().padding = PanelConfig.Padding ?? new();
        }
        public GameObject SetHorizontalLayout() {
            gameObject.AddComponent<HorizontalLayoutGroup>().childControlWidth = false;
            gameObject.GetComponent<HorizontalLayoutGroup>().childControlHeight = true;
            gameObject.GetComponent<HorizontalLayoutGroup>().childForceExpandWidth = false;
            gameObject.GetComponent<HorizontalLayoutGroup>().childForceExpandHeight = true;
            gameObject.AddComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.MinSize;
            gameObject.GetComponent<HorizontalLayoutGroup>().spacing = PanelConfig.Spacing;
            gameObject.GetComponent<HorizontalLayoutGroup>().padding = PanelConfig.Padding ?? new();
            return gameObject;
        }
        public void SetScroll() {
            gameObject.SetName("Scroll");
            var B = gameObject.创建矩形("0 0 100%-10 100%").SetName("Viewport");
            var C = B.创建矩形("0 0 100% 100%").SetName("Content");
            var D = gameObject.创建矩形("100%-10 5 5 100%-5").SetName("Scrollbar");
            B.AddComponent<RectMask2D>();
            gameObject.AddComponent<ScrollRect>().viewport = B.GetComponent<RectTransform>();
            gameObject.GetComponent<ScrollRect>().content = C.GetComponent<RectTransform>();
            gameObject.GetComponent<ScrollRect>().verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport;
            gameObject.GetComponent<ScrollRect>().movementType = ScrollRect.MovementType.Clamped;//无惯性滚动
            gameObject.GetComponent<ScrollRect>().decelerationRate = 0f;//无溢出滚动
            gameObject.GetComponent<ScrollRect>().scrollSensitivity = 50;//滚动速度
            D.AddComponent<Scrollbar>().handleRect = D.创建矩形(滚动条样式).GetComponent<RectTransform>();
            D.GetComponent<Scrollbar>().direction = Scrollbar.Direction.BottomToTop;
            Real = C;
            gameObject.GetComponent<ScrollRect>().verticalScrollbar = D.GetComponent<Scrollbar>();
        }
        public void 设置横纵滚动() {
            gameObject.SetName("Scroll");
            var B = gameObject.创建矩形("0 0 100%-10 100%").SetName("Viewport");
            var C = B.创建矩形("0 0 100% 100%").SetName("Content");
            var 右侧滚动条 = gameObject.创建矩形("100%-10 5 5 100%-5").SetName("ScrollbarV");
            var 下侧滚动条 = gameObject.创建矩形("5 100%-10 100%-5 5").SetName("ScrollbarH");
            B.AddComponent<RectMask2D>();
            gameObject.AddComponent<ScrollRect>().viewport = B.GetComponent<RectTransform>();
            gameObject.GetComponent<ScrollRect>().content = C.GetComponent<RectTransform>();
            gameObject.GetComponent<ScrollRect>().verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.Permanent;
            gameObject.GetComponent<ScrollRect>().horizontalScrollbarVisibility = ScrollRect.ScrollbarVisibility.Permanent;
            gameObject.GetComponent<ScrollRect>().movementType = ScrollRect.MovementType.Clamped;//无惯性滚动
            gameObject.GetComponent<ScrollRect>().decelerationRate = 0f;//无溢出滚动
            gameObject.GetComponent<ScrollRect>().scrollSensitivity = 50;//滚动速度

            右侧滚动条.AddComponent<Scrollbar>().handleRect = 右侧滚动条.创建矩形(滚动条样式).GetComponent<RectTransform>();
            右侧滚动条.GetComponent<Scrollbar>().direction = Scrollbar.Direction.BottomToTop;
            下侧滚动条.AddComponent<Scrollbar>().handleRect = 下侧滚动条.创建矩形(滚动条样式).GetComponent<RectTransform>();
            下侧滚动条.GetComponent<Scrollbar>().direction = Scrollbar.Direction.LeftToRight;

            Real = C;
            gameObject.GetComponent<ScrollRect>().verticalScrollbar = 右侧滚动条.GetComponent<Scrollbar>();
            gameObject.GetComponent<ScrollRect>().horizontalScrollbar = 下侧滚动条.GetComponent<Scrollbar>();
        }
        public GameObject SetChildText() {
            var A = Real.创建矩形("0 0 100% 100%").SetName("Text");
            A.AddComponent<TextMeshProUGUI>().font = Fonts[PanelConfig.Font];
            A.GetComponent<TextMeshProUGUI>().fontSize = PanelConfig.TextSize;
            A.GetComponent<TextMeshProUGUI>().alignment = PanelConfig.TextAlign;
            A.GetComponent<TextMeshProUGUI>().color = PanelConfig.TextColor.ToColor();
            A.GetComponent<TextMeshProUGUI>().margin = PanelConfig.Margin;
            A.GetComponent<TextMeshProUGUI>().enableWordWrapping = true;
            //这段代码 导致文字不显示
            //A.GetComponent<TextMeshProUGUI>().overflowMode = TextOverflowModes.Truncate;
            A.GetComponent<TextMeshProUGUI>().text = "";
            return A;
        }
        public GameObject SetChildText_单行() {
            var A = Real.创建矩形("0 0 100% 100%").SetName("Text");
            A.AddComponent<TextMeshProUGUI>().font = Fonts[PanelConfig.Font];
            A.GetComponent<TextMeshProUGUI>().fontSize = PanelConfig.TextSize;
            A.GetComponent<TextMeshProUGUI>().alignment = PanelConfig.TextAlign;
            A.GetComponent<TextMeshProUGUI>().color = PanelConfig.TextColor.ToColor();
            A.GetComponent<TextMeshProUGUI>().margin = PanelConfig.Margin;
            A.GetComponent<TextMeshProUGUI>().enableWordWrapping = false;
            //这段代码 导致文字不显示
            //A.GetComponent<TextMeshProUGUI>().overflowMode = TextOverflowModes.Truncate;
            A.GetComponent<TextMeshProUGUI>().text = "";
            return A;
        }
        public void SetChildTextAutoWidth() {
            var A = SetChildText();
            A.AddComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            Real.GetOrAddComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.MinSize;
            Real.GetOrAddComponent<HorizontalLayoutGroup>().childControlWidth = false;
            Real.GetOrAddComponent<HorizontalLayoutGroup>().childForceExpandWidth = false;
        }
        public void SetChildTextAutoHeight() {
            var A = SetChildText();
            A.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            Real.GetOrAddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.MinSize;
            Real.GetOrAddComponent<VerticalLayoutGroup>().childControlHeight = false;
            Real.GetOrAddComponent<VerticalLayoutGroup>().childForceExpandHeight = false;
        }
        /// <summary>
        /// 以当前Real为InputField，创建一个Text
        /// </summary>
        public void SetInput() {
            var A = Real.创建矩形(PanelConfig.Clone().SetModel(矩形模式.固定文本框).SetPosition("0 0 100% 100%").SetColor(default));
            Real.AddComponent<RectMask2D>();
            Real.AddComponent<UIEvent>();
            Real.AddComponent<TMP_InputField>().lineType = TMP_InputField.LineType.MultiLineNewline;//回车后新行
            Real.GetComponent<TMP_InputField>().textViewport = A.GetComponent<RectTransform>();
            Real.GetComponent<TMP_InputField>().textComponent = A.GetComponentInChildren<TextMeshProUGUI>();
            Real.GetComponent<TMP_InputField>().onFocusSelectAll = false;
            Real.ChangeActiveTwice();
        }
        /// <summary>
        /// 左 上 宽 高。数值(127) 比例(0.31)
        /// </summary>
        public static double[] ParsePosition(string Y) {
            return Y.Replace(" 0% ", " 0 ").Split(" ").ToString(X => {
                X = (X[0] is not '+' and not '-') ? "+" + X : X;//补齐正号
                X = (X.IndexOf("%") == -1) ? X + "+0%" : X;//补齐百分位
                X = (Regex.Matches(X, "[+-]").Count == 1) ? "+0" + X : X;//补齐像素位
                var Z = X.Replace("-", "+-").Split("+", StringSplitOptions.RemoveEmptyEntries);//分割为数值与百分比两部分
                Array.Sort(Z, (A, B) => A.EndsWith("%") ? 1 : -1);//排序：数值在前，百分比在后
                return Z[0] + " " + double.Parse(Z[1].TrimEnd('%')) / 100;//组装成新的字符串
            }).Split(" ").ParseTo(t => double.Parse(t));//把新字符串分割为float数组
        }
        public void SetLayoutElement() {
            if (PanelConfig.Element != default) {
                Real.GetOrAddComponent<LayoutElement>().minHeight = PanelConfig.Element.x;
                Real.GetOrAddComponent<LayoutElement>().preferredHeight = PanelConfig.Element.y;
                Real.GetOrAddComponent<LayoutElement>().minWidth = PanelConfig.Element.z;
                Real.GetOrAddComponent<LayoutElement>().preferredWidth = PanelConfig.Element.w;
            }
        }
        #endregion
    }
    public enum 矩形模式 {
        固定无文本,
        固定文本框,
        固定单行文本框,
        固定可选文本框,

        自动高度无文本,
        自动宽度无文本,
        自动高度文本框,
        自动宽度文本框,

        固定多行输入框,
        固定单行输入框,
        固定均匀垂直列表,
        固定均匀水平列表,

        滚动条自动高度无文本,
        滚动条自动高度文本框,
        滚动条自动高度可选文本框,
        滚动条自动高度网格布局,
        滚动条自动高度自动宽度无文本
    }
}
