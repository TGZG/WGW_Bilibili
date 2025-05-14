using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static CMKZ.LocalStorage;

namespace CMKZ {
    public static partial class LocalStorage {
        public static GameObject 右键菜单(this GameObject X, KeyValueList<string, Action<GameObject>> Y) {
            if (!X.GetOrAddComponent<UIMenu>().分组.Any()) {
                X.GetOrAddComponent<UIMenu>().创建分组();
            }
            X.GetOrAddComponent<UIMenu>().分组.First().Menu.AddRange(Y);
            return X;
        }
        public static GameObject 右键菜单(this GameObject X, string Y, Action<GameObject> Z) {
            X.GetOrAddComponent<UIMenu>().FindOrCreate(Y).Value = Z;
            return X;
        }
        public static GameObject 右键菜单(this GameObject X, int 分组号, string Y, Action<GameObject> Z) {
            if (!X.GetOrAddComponent<UIMenu>().分组.Any()) {
                X.GetOrAddComponent<UIMenu>().创建分组();
            }
            if (X.GetOrAddComponent<UIMenu>().分组.Count <= 分组号) {
                (分组号 + 1 - X.GetOrAddComponent<UIMenu>().分组.Count).TimesToDo(() => X.GetOrAddComponent<UIMenu>().创建分组());
            }
            X.GetOrAddComponent<UIMenu>().分组[分组号].Menu.Add(Y, Z);
            return X;
        }
        public static GameObject 修改右键菜单(this GameObject X, string 原名称, string Y, Action<GameObject> Z) {
            var A = X.GetOrAddComponent<UIMenu>().FindOrCreate(原名称);
            A.Key = Y;
            A.Value = Z;
            return X;
        }
        public static GameObject 删除右键菜单(this GameObject X, string Y) {
            X.GetOrAddComponent<UIMenu>().分组.ForEach(t => t.Menu.RemoveAll(t => t.Key == Y));
            return X;
        }
        public static GameObject 清空右键菜单_Fast(this GameObject X) {
            X?.GetOrAddComponent<UIMenu>().Clear();
            return X;
        }
        public static GameObject 删除右键菜单(this GameObject X) {
            X?.GetOrAddComponent<UIMenu>().Destroy(true);
            return X;
        }
    }
    public class UIMenu : MonoBehaviour {
        public static GameObject 右键背景;
        public static 限数 右键冷却 = new(0.1f);
        public static PanelConfig UIMenu默认样式 = new() {
            矩形模式 = 矩形模式.自动高度文本框,
            Position = "0 0 100 400",
            ImageColor = new Vector4(200, 200, 200, 0f),
            Spacing = 5
        };
        public static PanelConfig UIMenu条目样式 = new() {
            矩形模式 = 矩形模式.固定文本框,
            Colors = 按钮四分之一黑,
            Margin = new Vector4(5, 5, 5, 5),
            Position = "0 0 100% 30",
            TextColor = 白色V4
        };
        public static PanelConfig UIMenu条目背景样式 = new() {
            矩形模式 = 矩形模式.固定无文本,
            Margin = new Vector4(5, 5, 5, 5),
            Position = "0 0 100% 30",
            TextColor = 白色V4
        };
        public static PanelConfig UIMenu分割线样式 = new() {
            矩形模式 = 矩形模式.固定无文本,
            Margin = new Vector4(5, 5, 5, 5),
            Position = "0 0 100 3",
            //ImageColor = 白五分
        };
        public bool IsSelfActive;
        public List<UIMenu分组项> 分组 = new();
        public void Start() {
            if (右键背景 == null) {
                右键背景 = MainPanel.创建矩形(UIMenu默认样式).SetName("右键背景");
                右键背景.SetActive(false);
                MainPanel.OnUpdate(() => {
                    右键冷却.增加(Time.deltaTime);
                    if (右键冷却.已满) {
                        if (Input.GetMouseButtonUp(0)) {
                            右键背景.Clear();
                            右键背景.SetActive(false);
                            IsSelfActive = false;
                        }
                    }
                });
            }
            gameObject.OnRightClick(XX => {
                IsSelfActive = true;
                gameObject.InvokeOnExit();
                右键冷却.SetToMin();
                //设置显示和位置
                右键背景.Clear();
                右键背景.SetActive(true);
                右键背景.transform.SetAsLastSibling();
                LayoutRebuilder.ForceRebuildLayoutImmediate(右键背景.GetComponent<RectTransform>());
                右键背景.transform.position = Input.mousePosition + new Vector3(8, -8);
                分组.Count.TimesToDo(i => {
                    分组[i].渲染(右键背景);
                    if (i != 分组.Count - 1) {
                        右键背景.创建矩形(UIMenu分割线样式);
                    }
                });
                //如果菜单的最下边缘超出屏幕，就让菜单在鼠标上方显示
                var A = 右键背景.GetComponent<RectTransform>();
                if (A.position.y - A.rect.height < 0) {
                    A.position = new Vector3(A.position.x, A.position.y + A.rect.height);
                }
                //如果菜单的最右边缘超出屏幕，就让菜单在鼠标左边显示
                if (A.position.x + A.rect.width > Screen.width) {
                    A.position = new Vector3(A.position.x - A.rect.width, A.position.y);
                }
            });
        }
        public void 创建分组() {
            分组.Add(new UIMenu分组项(gameObject));
        }
        public KeyValue<string, Action<GameObject>> FindOrCreate(string X) {
            if (!分组.Any()) {
                创建分组();
            }
            //筛选出所有分组里的所有项
            var A = Find(X);
            if (A == null) {
                分组.Last().Menu.Add(X, XX => { });
                return 分组.Last().Menu.Last();
            }
            return A;
        }
        public KeyValue<string, Action<GameObject>> Find(string X) {
            return 分组.SelectMany(t => t.Menu).FirstOrDefault(t => t.Key == X);
        }
        public void Clear() {
            分组.Clear();
        }
        public void OnDestroy() {
            if (右键背景 != null && IsSelfActive) {
                IsSelfActive = false;
                右键背景.Clear();
                右键背景.SetActive(false);
            }
        }
    }
    public class UIMenu分组项 {
        public GameObject gameObject;
        public KeyValueList<string, Action<GameObject>> Menu = new();
        public UIMenu分组项(GameObject 父按钮) {
            gameObject = 父按钮;
        }
        public void 渲染(GameObject X) {
            var A = X.创建矩形(UIMenu.UIMenu默认样式);
            Menu.ForEach(t => {
                var 按钮 = A.创建矩形(UIMenu.UIMenu条目背景样式).设置模糊(3).创建矩形(UIMenu.UIMenu条目样式).SetText($"{t.Key}");
                //.SetSprite("界面素材/右键菜单")
                按钮.OnClick(XX => {
                    X.Clear();
                    X.SetActive(false);
                    t.Value(gameObject);
                });
            });
        }
        //public void Destroy() {
        //    gameObject.Destroy();
        //    Menu.Clear();
        //}
    }
}