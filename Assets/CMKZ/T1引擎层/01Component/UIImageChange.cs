using TMPro;
using UnityEngine;//Mono
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CMKZ {
    public class UIImageChange : MonoBehaviour, IPointerClickHandler {
        public Image Target;
        public Sprite Sprite1;
        public Sprite Sprite2;
        public void OnPointerClick(PointerEventData eventData) {
            Target.sprite = Target.sprite == Sprite1 ? Sprite2 : Sprite1;
        }
    }
}