using System;
using UnityEngine;//Mono
using UnityEngine.EventSystems;

namespace CMKZ {
    public class UIDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
        public GameObject target;//写编辑器扩展来重命名
        public GameObject 范畴;
        public Action<PointerEventData> onEndDrag;
        private Vector3[] 父四角;
        public void OnBeginDrag(PointerEventData eventData) {
            target.transform.SetAsLastSibling();
            if (范畴 != null) {
                父四角 = 范畴.GetComponent<RectTransform>().四角坐标();
            }
        }
        public void OnDrag(PointerEventData eventData) {
            //if (父四角 == null || target.Is在其中(父四角)) {
            //    t
            //}
            //当物体在外面时，令其在边界上
            target.transform.position += (Vector3)eventData.delta;
            if (父四角 != null) {
                var 宽度 = target.GetComponent<RectTransform>().sizeDelta.x;
                var 高度 = target.GetComponent<RectTransform>().sizeDelta.y;
                var 位置 = target.transform.position;
                if (位置.x < 父四角[0].x) {
                    target.transform.position = new(父四角[0].x, 位置.y);
                }
                if (位置.x + 宽度 > 父四角[2].x) {
                    target.transform.position = new(父四角[2].x - 宽度, 位置.y);
                }
                if (位置.y > 父四角[1].y) {
                    target.transform.position = new(位置.x, 父四角[1].y);
                }
                if (位置.y - 高度 < 父四角[0].y) {
                    target.transform.position = new(位置.x, 父四角[0].y + 高度);
                }
            }
        }
        public void OnEndDrag(PointerEventData eventData) {
            onEndDrag?.Invoke(eventData);
        }
    }
}