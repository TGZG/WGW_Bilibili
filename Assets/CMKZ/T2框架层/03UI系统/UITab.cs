using System;
using UnityEngine;
using static UnityEngine.Object;

namespace CMKZ {
    public static partial class LocalStorage {
        //public static void DemoTab() {
        //    var A = 左上.SetTab(标题, 正文, 按钮常规, 按钮激活);
        //    A.AddTab("装备", 背景 => {
        //        背景.DrawTree(数据);
        //    });
        //    A.AddTab("角色", 背景 => {
        //        背景.DrawTree(数据);
        //    });
        //    A.AddTab("方案", 背景 => {
        //        背景.DrawTree(数据);
        //    });
        //}
        public static UITab SetTab(this GameObject X, PanelConfig 标题, PanelConfig 正文, PanelConfig 按钮常规, PanelConfig 按钮激活) {
            if (X.GetComponent<UITab>() != null) UnityEngine.Object.Destroy(X.GetComponent<UITab>());
            var A = X.AddComponent<UITab>();
            A.标题样式 = 标题;
            A.正文样式 = 正文;
            A.按钮常规样式 = 按钮常规;
            A.按钮激活样式 = 按钮激活;
            A.Init();
            return A;
        }
        public static UITab SetTab(this GameObject X, PanelConfig 标题, PanelConfig 正文) {
            if (X.GetComponent<UITab>() != null) UnityEngine.Object.Destroy(X.GetComponent<UITab>());
            var A = X.AddComponent<UITab>();
            A.标题样式 = 标题;
            A.正文样式 = 正文;
            A.Init();
            return A;
        }
    }
    public class UITab : MonoBehaviour {
        public GameObject 标题;
        public GameObject 正文;
        public PanelConfig 标题样式;
        public PanelConfig 正文样式;
        public PanelConfig 按钮常规样式;
        public PanelConfig 按钮激活样式;
        public KeyValueList<GameObject, GameObject> 所有组 = new();//标题，正文
        public void Init() {
            标题 = gameObject.创建矩形(标题样式);
            正文 = gameObject.创建矩形(正文样式);
        }
        public void AddTab(string X, Action<GameObject> Y) {
            var 按钮 = 标题.创建矩形(按钮常规样式).SetText(X).SetName(X + "按钮");
            var 内容 = 正文.创建矩形("0 0 100% 100%").SetName(X + "内容");
            Y(内容);
            所有组.Add(按钮, 内容);
            按钮.OnClick(t => {
                foreach (var i in 所有组) {
                    i.Key.调整矩形(按钮常规样式.常规);
                    i.Value.Hide();
                }
                按钮.调整矩形(按钮激活样式.常规);
                内容.Show();
            });
            按钮.InvokeOnClick();
        }
        public void AddTab(string X, PanelConfig Z, Action<GameObject> Y) {
           }
        public Dictionary<GameObject, string[]> 默认样式 = new();
        public GameObject AddTab(PanelConfig 按钮样式, string[] 按钮常规, string[] 按钮激活, Action<GameObject> Y) {
            var 按钮 = 标题.创建矩形(按钮样式).SetName("按钮");
            var 内容 = 正文.创建矩形("0 0 100% 100%").SetName("内容");
            Y(内容);
            所有组.Add(按钮, 内容);
            默认样式[按钮] = 按钮常规;
            按钮.OnClick(t => {
                foreach (var i in 所有组) {
                    i.Key.调整矩形(默认样式[i.Key]);
                    i.Value.Hide();
                }
                按钮.调整矩形(按钮激活);
                var A = t.GetComponent<MyTransform>().PanelConfig;
                内容.Show();
            });
            按钮.InvokeOnClick();
            return 按钮;
        }
        public GameObject AddTab(string X, string[] 按钮常规, string[] 按钮激活, Action<GameObject> Y, Action<GameObject> 按钮创建事件=null) {
            var 按钮 = 标题.创建矩形(按钮常规样式).SetText(X).SetName(X+"按钮");
            var 内容 = 正文.创建矩形("0 0 100% 100%").SetName(X+"内容");
            按钮创建事件?.Invoke(按钮);
            Y(内容);
            所有组.Add(按钮, 内容);
            默认样式[按钮] = 按钮常规;
            按钮.OnClick(t => {
                foreach (var i in 所有组) {
                    i.Key.调整矩形(默认样式[i.Key]);
                    i.Value.Hide();
                }
                按钮.调整矩形(按钮激活);
                var A = t.GetComponent<MyTransform>().PanelConfig;
                内容.Show();
            });
            按钮.InvokeOnClick();
            return 按钮;
        }
        public GameObject AddTab(string X, PanelConfig 按钮样式, string[] 按钮常规, string[] 按钮激活, Action<GameObject> Y) {
            var 按钮 = 标题.创建矩形(按钮样式).SetText(X).SetName(X + "按钮");
            var 内容 = 正文.创建矩形("0 0 100% 100%").SetName(X + "内容");
            Y(内容);
            所有组.Add(按钮, 内容);
            默认样式[按钮] = 按钮常规;
            按钮.OnClick(t => {
                foreach (var i in 所有组) {
                    i.Key.调整矩形(默认样式[i.Key]);
                    i.Value.Hide();
                }
                按钮.调整矩形(按钮激活);
                var A = t.GetComponent<MyTransform>().PanelConfig;
                内容.Show();
            });
            按钮.InvokeOnClick();
            return 按钮;
        }
        public void ActiveTab(int X) {
            标题.transform.GetChild(X).gameObject.InvokeOnClick();
        }
    }
}
