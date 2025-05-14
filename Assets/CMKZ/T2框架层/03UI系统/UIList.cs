using Microsoft.CodeAnalysis;
using Newtonsoft.Json;//Json
using System;//Action
using System.Collections;
using System.Collections.Generic;//List
using System.Linq;//from XX select XX
using TMPro;//InputField
using UnityEngine;//Mono
using UnityEngine.UI;//Image
using static CMKZ.LocalStorage;

namespace CMKZ {
    public static partial class LocalStorage {
        public static UIList<T> 创建UI列表<T>(this GameObject X, List<T> 源, Func<GameObject, T, int, GameObject> OnFileDraw, bool 动态修改 = true) {
            return new UIList<T>(X, 源, 动态修改).OnFileDraw(OnFileDraw);
        }
    }
    public class UIList<T> {
        public GameObject gameObject;
        public List<T> 源;
        public KeyValueList<T, GameObject> 缓存;
        public UIList(GameObject X, List<T> 源, bool 动态修改 = false) {
            gameObject = X.创建矩形(new PanelConfig() {
                矩形模式 = 矩形模式.自动高度无文本,
                Position = "0 0 100% 100%",
                Padding = 配置.边距,
                Spacing = 配置.间距,
                TextSize = 配置.字号,
                Font = 配置.字体,
            });
            this.源 = 源;
            if (动态修改) {
                源.OnAdd += (X, Y) => Add(X, Y);
                源.OnRemove += (X, Y) => Remove(X, Y);
            }
        }
        public void Add(T X, int POS = -1) {
            if (POS == -1) POS = 源.Count;
            if (OnDraw == null) throw new Exception("未设置OnDraw");
            var A = OnDraw?.Invoke(gameObject, X, POS);
            A.transform.SetSiblingIndex(POS);
            缓存.Insert(POS, new(X, A));
            OnAdd?.Invoke(X, POS);
        }
        public void Remove(T X, int POS) {
            缓存[POS].Value.Destroy();
            缓存.RemoveAt(POS);
            OnRemove?.Invoke(X, POS);
        }
        public event Action<T, int> OnAdd;
        public event Action<T, int> OnRemove;
        private event Func<GameObject, T, int, GameObject> OnDraw;
        public UIList<T> OnFileDraw(Func<GameObject, T, int, GameObject> X) {
            OnDraw += X;
            return this;
        }
        public void Draw() {
            gameObject.Clear();
            if (配置.背景路径 != null) gameObject.SetSprite(配置.背景路径);
            if (OnDraw == null) throw new Exception("未设置OnDraw");
            foreach (var i in 源) {
                OnDraw?.Invoke(gameObject, i, 源.IndexOf(i));
            }
        }
        public void 筛选(Func<T, bool> 判断) {
            //满足条件的显示，不满足的隐藏
            foreach (var i in 缓存) {
                if (判断(i.Key)) {
                    i.Value.SetActive(true);
                } else {
                    i.Value.SetActive(false);
                }
            }
        }
        public void 筛选清空() {
            foreach (var i in 缓存) {
                i.Value.SetActive(true);
            }
        }
        public UIListConfig 配置 {
            get => _配置.CloneObject();
            set {
                //如果有一个字段是null，那么就不改变
                var A = value.GetType().GetFields();
                foreach (var i in A) {
                    var B = i.GetValue(value);
                    if (B != null) {
                        Print(B);
                        Print(i);
                        Print(_配置);
                        i.SetValue(_配置, B);
                    }
                }
                Draw();
            }
        }
        public UIListConfig _配置 = 默认配置;

        public static UIListConfig 默认配置 = new UIListConfig {
            边距 = new(5, 5, 5, 5),
            间距 = 10,
            字号 = 20,
            字体 = "F5"
        };
    }
    public class UIListConfig {
        public RectOffset 边距;
        public int 间距;
        public string 背景路径;
        public int 字号;
        public string 字体;
    }
}