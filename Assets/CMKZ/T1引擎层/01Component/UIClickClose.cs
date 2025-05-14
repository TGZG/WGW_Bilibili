using TMPro;
using UnityEngine;//Mono
using UnityEngine.EventSystems;

namespace CMKZ {
    public class UIClickClose : MonoBehaviour, IPointerClickHandler {
        public GameObject Target;
        public bool IsDestroy = true;
        public void OnPointerClick(PointerEventData eventData) {
            if (IsDestroy) {
                Destroy(Target);
            } else {
                Target.SetActive(false);
            }
        }
    }
}