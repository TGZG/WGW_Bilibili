using System;//Action
using TMPro;//InputField
using UnityEngine;//Mono
using static UnityEngine.Object;//Destory

namespace CMKZ {
    public static partial class LocalStorage {
        public static UIAlertInput AlertInput() {
            return Instantiate(AllPrefab["输入弹窗"], MainPanel.transform).SetOutLineWithBlur_White(true).GetComponent<UIAlertInput>();
        }
    }
    public class UIAlertInput : MonoBehaviour {
        public UIAlertInput SetText(string X) {
            gameObject.Find("提示词").GetComponent<TextMeshProUGUI>().text = X;
            return this;
        }
        public UIAlertInput SetPosition(string X) {
            gameObject.SetPosition(X);
            return this;
        }
        public UIAlertInput SetYes(Action<string> X) {
            gameObject.Find("确认").GetComponent<UIClick>().OnClick += () => {
                X(GetComponentInChildren<TMP_InputField>().text);
            };
            return this;
        }
        public UIAlertInput SetTitle(string X) {
            gameObject.Find("标题栏/标题").GetComponent<TextMeshProUGUI>().text = X;
            return this;
        }
        public UIAlertInput 限制数字() {
            gameObject.GetComponentInChildren<TMP_InputField>().contentType = TMP_InputField.ContentType.IntegerNumber;
            return this;
        }
    }
}