using System;
using TMPro;//InputField
using UnityEngine;//Mono
using static UnityEngine.Object;//Destory

namespace CMKZ {
    public static partial class LocalStorage {
        public static bool 唯一弹窗 = true;
        public static GameObject 唯一弹窗实例;
        public static UIAlert Alert() {
            if (唯一弹窗) {
                if (唯一弹窗实例 != null) {
                    唯一弹窗实例.Destroy();
                }
                唯一弹窗实例 = Instantiate(AllPrefab["单纯弹窗"], MainPanel.transform).SetOutLineWithBlur_White(true);
                return 唯一弹窗实例.GetComponent<UIAlert>();
            } else {
                return Instantiate(AllPrefab["单纯弹窗"], MainPanel.transform).SetOutLineWithBlur_White(true).GetComponent<UIAlert>();
            }
        }
        public static UIAlert Alert(string X) {
            return Alert().SetText(X);
        }
    }
    public class UIAlert : MonoBehaviour {
        public TextMeshProUGUI 提示词;
        public TextMeshProUGUI 标题;
        public GameObject 确认;
        public UIAlert SetText(string X) {
            提示词.GetComponent<TextMeshProUGUI>().text = X;
            return this;
        }
        public UIAlert Set左对齐() {
            提示词.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.TopLeft;
            return this;
        }
        public UIAlert SetPosition(string X) {
            gameObject.SetPosition(X);
            return this;
        }
        public UIAlert SetTitle(string X) {
            标题.GetComponent<TextMeshProUGUI>().text = X;
            return this;
        }
        public UIAlert SetYes(Action A) {
            确认.OnClick(t => A?.Invoke());
            return this;
        }
    }
}