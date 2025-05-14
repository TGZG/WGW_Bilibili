using TMPro;
using UnityEngine;//Mono
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CMKZ {
    public class UIClickActive : MonoBehaviour, IPointerClickHandler {
        public GameObject Target;
        private bool ActiveSelf = true;
        public void OnPointerClick(PointerEventData eventData) {
            if (ActiveSelf) {
                Target.SetActive(!Target.activeSelf);
                foreach (var i in Target.transform.GetComponentsInParent<LayoutGroup>()) {
                    LayoutRebuilder.ForceRebuildLayoutImmediate(i.GetComponent<RectTransform>());
                }
            }
        }
        public void SetActive(bool X) {
            ActiveSelf = X;
        }
    }
}