using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using static CMKZ.LocalStorage;

namespace CMKZ {
    public static partial class LocalStorage {
        public static GameObject 悬浮注释(this GameObject X, Func<GameObject, string> Y) {
            X.GetOrAddComponent<UIExplane>().GetText = Y;
            return X;
        }
        public static GameObject 悬浮注释(this GameObject X, string Y) {
            X.GetOrAddComponent<UIExplane>().GetText = t => Y;
            return X;
        }
        public static GameObject 取消悬浮注释(this GameObject X) {
            if (X.GetComponent<UIExplane>() != null) {
                UnityEngine.Object.Destroy(X.GetComponent<UIExplane>());
            }
            return X;
        }
    }
    public class UIExplane : MonoBehaviour {
        public static GameObject 注释背景;
        public Func<GameObject, string> GetText;
        public bool IsSelfActive;
        public void Start() {
            if (注释背景 == null) {
                注释背景 = MainPanel.创建矩形(new PanelConfig() {
                    Position = "0 0 300 400",
                    矩形模式 = 矩形模式.自动高度文本框,
                    ImageColor = new(200,200,200,1),
                    TextColor = 白色V4,
                    Font = "基督山伯爵",
                    Margin = new(5, 5, 5, 5)
                }).SetName("注释背景");
                注释背景.设置本体模糊(2);
                注释背景.SetActive(false);
            }
            gameObject.OnEnter(t => {
                IsSelfActive = true;
                注释背景.SetActive(true);
                注释背景.transform.SetAsLastSibling();
                设置注释位置();
                注释背景.SetText(GetText(t));
                LayoutRebuilder.ForceRebuildLayoutImmediate(注释背景.GetComponent<RectTransform>());
            });
            gameObject.OnMove(t => {
                设置注释位置();
            });
            gameObject.OnExit(t => {
                注释背景.SetActive(false);
                IsSelfActive = false;
            });
        }
        public void 设置注释位置() {
            var 注释宽度 = 注释背景.GetComponent<RectTransform>().rect.width;
            var 最终位置 = Input.mousePosition;
            //如果注释背景右侧超出屏幕，那么移动到左侧
            if (Input.mousePosition.x + 注释宽度 > Screen.width) {
                最终位置 += new Vector3(-注释宽度 - 5, 0);
            } else {
                最终位置 += new Vector3(20, -20);
            }
            if (Input.mousePosition.y - 注释背景.GetComponent<RectTransform>().rect.height < 0) {
                最终位置 += new Vector3(0, 注释背景.GetComponent<RectTransform>().rect.height + 5);
            }
            注释背景.GetComponent<RectTransform>().position = 最终位置;
        }
        public void OnDestroy() {
            if (注释背景 != null && IsSelfActive) {
                IsSelfActive = false;
                注释背景.SetActive(false);
            }
        }
    }
}