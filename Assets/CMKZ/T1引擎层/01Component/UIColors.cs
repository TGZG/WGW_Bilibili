using System;
using TMPro;//InputField
using UnityEngine;//Mono
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static CMKZ.LocalStorage;

namespace CMKZ {
    public class UIColors : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler {
        public Color OnEnter = ToColor("4d4d4dFF");
        public Color OnClick = ToColor("777777FF");
        [HideInInspector]
        [SerializeField]
        private Color _OnExit = ToColor("191919FF");
        public Color OnExit {
            get => _OnExit;
            set {
                _OnExit = value;
                if (Target != null) {
                    Target.color = value;
                }
            }
        }
        public Image Target;
        public static Dictionary<string, UIColors> Groups = new();//每个分组的当前激活
        public string ID;
        private bool IsFocous;
        public void OnPointerEnter(PointerEventData X) {
            if (Target != null) {
                Target.color = OnEnter;
            }
        }
        public void OnPointerExit(PointerEventData X) {
            if (Target != null) {
                if (IsFocous) {
                    Target.color = OnClick;
                } else {
                    Target.color = OnExit;
                }
            }
        }
        public void OnPointerDown(PointerEventData X) {
            if (Target != null) {
                if (Groups.ContainsKey(ID)) {
                    Groups[ID].Target.color = OnExit;
                    Groups[ID].IsFocous = false;
                }
                Groups[ID] = this;
                Groups[ID].Target.color = OnClick;
                Groups[ID].IsFocous = true;
            }
        }
        public void OnRelease() {
            Target.color = OnExit;
            IsFocous = false;
        }
        //public void SetFocous(bool X) {
        //    IsFocous = X;
        //    if (Target != null) {
        //        if (IsFocous) {
        //            Target.color = OnClick;
        //        } else if (Target.gameObject.IsMouseHere()) {
        //            Target.color = OnEnter;
        //        } else {
        //            Target.color = OnExit;
        //        }
        //    }
        //}
        public static Color ToColor(string X) {//FFFFFFFF
            return new Color(
                Convert.ToInt32(X.Substring(0, 2), 16) / 255f,
                Convert.ToInt32(X.Substring(2, 2), 16) / 255f,
                Convert.ToInt32(X.Substring(4, 2), 16) / 255f,
                Convert.ToInt32(X.Substring(6, 2), 16) / 255f
            );
        }
    }
}