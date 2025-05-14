using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static CMKZ.LocalStorage;

namespace CMKZ {
    public class 背景数据 {
        public string 左上宽高;
        public List<string> 标签名列表 = new();
    }
    public static partial class LocalStorage {
        public static List<选项卡元素> 所有选项卡元素 = new();
        public static T FindTab<T>() where T : 选项卡元素 {
            foreach (var i in 所有选项卡元素) {
                if (i is T j) {
                    return j;
                }
            }
            return null;
        }
        public static 选项卡元素 FindTab(Type X) {
            foreach (var i in 所有选项卡元素) {
                if (i.GetType() == X) {
                    return i;
                }
            }
            return null;
        }
        public static void SaveAllTab(string X) {
            List<背景数据> B = new();
            foreach (var i in 所有选项卡元素) {
                RectTransform 背景 = i.背景.GetComponent<RectTransform>();
                var A = $"{背景.offsetMin.x} {-背景.offsetMax.y} {背景.sizeDelta.x} {背景.sizeDelta.y}";
                if (!B.Contains(t => t.左上宽高 == A)) {
                    B.Add(new 背景数据 { 左上宽高 = A });
                }
                B.Find(t => t.左上宽高 == A).标签名列表.Add(i.标题.name);
            }
            FileWrite(X, B);
        }
        public static void LoadAllTab(string X) {
            List<背景数据> A = FileRead<List<背景数据>>(X);
            foreach (var i in A) {
                GameObject 背景 = MainPanel.创建矩形(i.左上宽高);
                foreach (var j in i.标签名列表) {
                    if (FindTab(Type.GetType(j)) != null) {
                        typeof(选项卡背景).GetMethod("AddTab").MakeGenericMethod(Type.GetType(j)).Invoke(背景.GetComponent<选项卡背景>(), null);
                    }
                }
            }
        }
    }
    public class 选项卡背景 : MonoBehaviour {
        public GameObject 标题栏;
        public GameObject 内容区;
        public void Awake() {
            gameObject.OnFocusIn(XX => gameObject.transform.SetAsLastSibling());
            gameObject.SetColor(255, 255, 255, 0.2f);
            gameObject.允许缩放();
            gameObject.AddComponent<Mask>();
            标题栏 = gameObject.创建矩形("0 0 100% 40");
            标题栏.SetColor(200, 200, 200, 0.5f);
            标题栏.AddComponent<HorizontalLayoutGroup>().childForceExpandWidth = false;
            标题栏.GetComponent<HorizontalLayoutGroup>().spacing = 8;
            标题栏.允许拖动(gameObject);
            内容区 = gameObject.创建矩形("8 40 100%-16 100%-48");
        }
        public 选项卡元素 AddTab<T>() where T : 选项卡元素, new() {
            var A = FindTab<T>();
            if (A == null) {
                var B = new T();
                B.背景 = gameObject;
                B.标题 = 标题栏.创建矩形("0 0 0 0").SetColors(Colors.Default).SetText("标题");
                B.标题.AddComponent<LayoutElement>().minWidth = 60;
                B.内容区 = 内容区.创建矩形("0 0 100% 100%").SetName("content").SetColor(0, 0, 0, 0);
                B.标题.OnClick(XX => B.Invoke());
                Vector3 startOffset = Vector3.zero;
                List<GameObject> 临时标题 = new();
                GameObject 临时面板 = null;
                B.标题.OnBeginDrag(XX => {
                    int 标题位置 = B.标题.transform.GetSiblingIndex();
                    临时面板 = MainPanel.创建矩形("0 0 0 0");//原本Append
                    临时面板.GetOrAddComponent<RectTransform>().anchorMin = B.背景.GetComponent<RectTransform>().anchorMin;
                    临时面板.GetComponent<RectTransform>().anchorMax = B.背景.GetComponent<RectTransform>().anchorMax;
                    临时面板.GetComponent<RectTransform>().offsetMin = B.背景.GetComponent<RectTransform>().offsetMin;
                    临时面板.GetComponent<RectTransform>().offsetMax = B.背景.GetComponent<RectTransform>().offsetMax;
                    临时面板.AddComponent<选项卡背景>().AddTab<T>();
                    for (int i = 0; i < 标题位置; i++) {
                        GameObject C = 临时面板.GetComponent<选项卡背景>().标题栏.创建矩形("0 0 0 0");
                        C.AddComponent<LayoutElement>().minWidth = 60;
                        临时标题.Add(C);
                    }
                    B.标题.transform.SetAsLastSibling();
                    startOffset = Input.mousePosition - 临时面板.transform.position;
                });
                B.标题.OnDrag(XX => {
                    临时面板.transform.position = Input.mousePosition - startOffset;
                });
                B.标题.OnEndDrag(XX => {
                    foreach (var item in 临时标题) {
                        DestroyImmediate(item);
                    }
                    foreach (var i in MainPanel.GetComponentsInChildren<选项卡背景>()) {
                        if (i.gameObject.IsMouseHere()) {
                            i.AddTab<T>();
                            return;
                        }
                    }
                });
                B.Create();
                B.Invoke();
                return B;
            } else {
                var B = A.背景;
                A.背景 = gameObject;
                A.标题.SetParent(标题栏);
                A.内容区.SetParent(内容区);
                if (B.transform.GetChild(0).childCount == 0) {
                    Destroy(B);
                }
                A.Invoke();
                所有选项卡元素.Add(A);
                return A;
            }
        }
    }
    public class 选项卡元素 {
        public GameObject 背景;
        public GameObject 内容区;
        public GameObject 标题;
        public virtual void Create() { }
        public virtual void 刷新() { }
        public void SetTitle(string X) {
            标题.SetText(X).SetName(X);
        }
        public 选项卡元素 Invoke() {
            内容区.transform.SetAsLastSibling();
            foreach (Transform i in 内容区.transform.parent) {
                i.gameObject.Hide();
            }
            内容区.Show();
            foreach (Transform i in 标题.transform.parent) {
                i.gameObject.SetColor(200, 200, 200);
            }
            标题.SetColor(120, 120, 120);
            return this;
        }
    }
}
