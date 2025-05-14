using TMPro;//InputField
using UnityEngine;//Mono
using UnityEngine.EventSystems;

namespace CMKZ {
    public class UIHightLight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
        public TextMeshProUGUI[] Text;
        public Color OnEnter = new(1, 1, 1, 1);
        [HideInInspector]
        [SerializeField]
        private Color _OnExit = new(1, 1, 1, 0.5f);
        public Color OnExit {
            get => _OnExit;
            set {
                _OnExit = value;
                Text?.ForEach(t => {
                    if (t == null) return;
                    t.color = value;
                });
            }
        }
        public void Start() {
            Text ??= GetComponentsInChildren<TextMeshProUGUI>();
        }
        public void OnPointerEnter(PointerEventData X) {
            Text.ForEach(t => t.color = OnEnter);
        }
        public void OnPointerExit(PointerEventData X) {
            Text.ForEach(t => t.color = OnExit);
        }
    }
}