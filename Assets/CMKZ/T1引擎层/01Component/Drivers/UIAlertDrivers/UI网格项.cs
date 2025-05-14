using System;
using TMPro;//InputField
using UnityEngine;//Mono
using UnityEngine.UI;
using static UnityEngine.Object;//Destory
using CMKZ;
using static CMKZ.LocalStorage;

namespace CMKZ {
    public class UI网格项 : MonoBehaviour {
        public TextMeshProUGUI 数字;
        public GameObject 图标;
        public UI网格项 SetIcon(string X) {
            图标.SetSprite(X);
            return this;
        }
        public UI网格项 SetNum(double X) {
            数字.text = X.ToString();
            return this;
        }
        public UI网格项 SetClick(Action A) {
            图标.OnClick(t => A?.Invoke());
            return this;
        }
    }
}