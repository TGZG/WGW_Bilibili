using System;
using TMPro;//InputField
using UnityEngine;//Mono
using UnityEngine.UI;
using static UnityEngine.Object;//Destory
using CMKZ;
using static CMKZ.LocalStorage;

namespace CMKZ {
    public static partial class LocalStorage {
        public static UIAlertGrid AlertGrid() {
            return Instantiate(AllPrefab["网格弹窗"], MainPanel.transform).SetOutLineWithBlur_White(true).GetComponent<UIAlertGrid>();
        }
    }
    public class UIAlertGrid : MonoBehaviour {
        public TextMeshProUGUI 提示词;
        public TextMeshProUGUI 标题;
        public GameObject 确认;
        public GameObject 网格背景;
        public UI网格项 模板;
        public UIAlertGrid SetText(string X) {
            提示词.text = X;
            return this;
        }
        public UIAlertGrid Set左对齐() {
            提示词.alignment = TextAlignmentOptions.TopLeft;
            return this;
        }
        public UIAlertGrid SetPosition(string X) {
            gameObject.SetPosition(X);
            return this;
        }
        public UIAlertGrid SetTitle(string X) {
            标题.text = X;
            return this;
        }
        public UIAlertGrid AddGrid(Action<UI网格项> 渲染事件) {
            var A=Instantiate(模板, 网格背景.transform).GetComponent<UI网格项>();
            A.gameObject.SetActive(true);
            渲染事件.Invoke(A);
            return this;
        }
        public UIAlertGrid SetYes(Action A) {
            确认.OnClick(t => A?.Invoke());
            return this;
        }
    }
}