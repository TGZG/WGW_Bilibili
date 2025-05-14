using TMPro;//InputField
using UnityEngine;//Mono

namespace CMKZ {
    public class UIAlertChooseNode : MonoBehaviour {
        public TextMeshProUGUI 左侧;
        public TextMeshProUGUI 右侧;
        public UIAlertChooseNode SetText(string X) {
            左侧.text = X;
            return this;
        }
        public UIAlertChooseNode SetText(string X, string Y) {
            左侧.text = X;
            右侧.text = Y;
            return this;
        }
        public UIAlertChooseNode SetText((string, string) X) {
            左侧.text = X.Item1;
            右侧.text = X.Item2;
            return this;
        }
    }
}