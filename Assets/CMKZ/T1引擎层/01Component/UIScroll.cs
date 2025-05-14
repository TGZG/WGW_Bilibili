using UnityEngine;//Mono
using UnityEngine.EventSystems;
using UnityEngine.UI;//Image

namespace CMKZ {
    public class UIScroll : MonoBehaviour, IScrollHandler {
        public ScrollRect scrollRect;
        public void OnScroll(PointerEventData e) {
            scrollRect.content.anchoredPosition -= new Vector2(0, e.scrollDelta.y * scrollRect.scrollSensitivity);
        }
    }
}