using System;//Action
using UnityEngine;//Mono
using UnityEngine.EventSystems;
using static CMKZ.LocalStorage;

namespace CMKZ {
    public class UIClick : MonoBehaviour, IPointerClickHandler {
        public Action OnClick;
        public void OnPointerClick(PointerEventData eventData) {
            if (eventData.button == PointerEventData.InputButton.Left) {
                OnClick?.Invoke();
                //Print($"UIClick {gameObject.name}");
            }
        }
        public void Invoke() {
            OnClick?.Invoke();
        }
    }
}