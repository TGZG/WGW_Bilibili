using System;//Action
using TMPro;//InputField
using UnityEngine;//Mono
using static UnityEngine.Object;//Destory

namespace CMKZ {
    public static partial class LocalStorage {
        public static UIAlertConfirm AlertConfirm() {
            return Instantiate(AllPrefab["确认弹窗"], MainPanel.transform).SetOutLineWithBlur_White(true).GetComponent<UIAlertConfirm>();
        }
    }
    public class UIAlertConfirm : MonoBehaviour {
        public UIAlertConfirm SetText(string X) {
            gameObject.Find("提示词").GetComponent<TextMeshProUGUI>().text = X;
            return this;
        }
        public UIAlertConfirm SetPosition(string X) {
            gameObject.SetPosition(X);
            return this;
        }
        public UIAlertConfirm SetYes(Action X) {
            gameObject.Find("确认").GetComponent<UIClick>().OnClick += () => {
                X();
            };
            return this;
        }
        public UIAlertConfirm SetTitle(string X) {
            gameObject.Find("标题栏/标题").GetComponent<TextMeshProUGUI>().text = X;
            return this;
        }
    }
}