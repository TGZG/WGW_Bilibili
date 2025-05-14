using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CMKZ {
    public static partial class LocalStorage {
        //单击事件
        public static GameObject OnClick(this GameObject X, Action<GameObject> Y) {
            X.GetOrAddComponent<UIEvent>().onClick += Y;
            return X;//因为是最常用的功能，所以允许返回自身以用于链式调用。其他事件必须单开一行。
        }
        public static void InvokeOnClick(this GameObject X) => X.GetComponent<UIEvent>()?.onClick?.Invoke(X);
        public static void InvokeMenu(this GameObject X, string Y) => X.GetComponent<UIMenu>()?.Find(Y)?.Value.Invoke(X);
        public static void OnMouseDown(this GameObject X, Action<GameObject> Y) => X.GetOrAddComponent<UIEvent>().onMouseDown += Y;
        public static void InvokeOnMouseDown(this GameObject X) => X.GetComponent<UIEvent>()?.onMouseDown?.Invoke(X);
        public static void OnMouseUp(this GameObject X, Action<GameObject> Y) => X.GetOrAddComponent<UIEvent>().onMouseUp += Y;
        public static void InvokeOnMouseUp(this GameObject X) => X.GetComponent<UIEvent>()?.onMouseUp?.Invoke(X);
        public static void OnDoubleClick(this GameObject X, Action<GameObject> Y) => X.GetOrAddComponent<UIEvent>().onDoubleClick += Y;
        public static void OnRightClick(this GameObject X, Action<GameObject> Y) => X.GetOrAddComponent<UIEvent>().onRightClick += Y;
        public static void InvokeOnRightClick(this GameObject X) => X.GetComponent<UIEvent>()?.onRightClick?.Invoke(X);
        //拖动事件
        public static void InvokeOnDrag(this GameObject X) => X.GetComponent<UIEvent>()?.onDrag?.Invoke(X);
        public static void InvokeOnBeginDrag(this GameObject X) => X.GetComponent<UIEvent>()?.onBeginDrag?.Invoke(X);
        public static void OnBeginDrag(this GameObject X, Action<GameObject> Y) => X.GetOrAddComponent<UIEvent>().onBeginDrag += Y;
        public static void OnDrag(this GameObject X, Action<GameObject> Y) => X.GetOrAddComponent<UIEvent>().onDrag += Y;
        public static void OnEndDrag(this GameObject X, Action<GameObject> Y) => X.GetOrAddComponent<UIEvent>().onEndDrag += Y;
        public static void OnDrop(this GameObject X, Action<GameObject> Y) => X.GetOrAddComponent<UIEvent>().onDrop += Y;
        //鼠标事件
        public static void OnEnter(this GameObject X, Action<GameObject> Y) => X.GetOrAddComponent<UIEvent>().onEnter += Y;
        public static Action<GameObject> GetOnEnter(this GameObject X) => X.GetOrAddComponent<UIEvent>().onEnter;
        public static void InvokeOnEnter(this GameObject X) => X.GetComponent<UIEvent>()?.onEnter?.Invoke(X);
        public static void OnMove(this GameObject X, Action<GameObject> Y) => X.GetOrAddComponent<UIEvent>().onMove += Y;
        public static void OnExit(this GameObject X, Action<GameObject> Y) => X.GetOrAddComponent<UIEvent>().onExit += Y;
        public static Action<GameObject> GetOnExit(this GameObject X) => X.GetOrAddComponent<UIEvent>().onExit;
        public static void InvokeOnExit(this GameObject X) => X.GetComponent<UIEvent>()?.onExit?.Invoke(X);
        public static void OnMouseBeginNear(this GameObject X, Action<GameObject> Y) => X.GetOrAddComponent<MouseNear>().OnBeginNear += Y;
        public static void OnMouseNear(this GameObject X, Action<GameObject> Y) => X.GetOrAddComponent<MouseNear>().OnNear += Y;
        public static void OnMouseEndNear(this GameObject X, Action<GameObject> Y) => X.GetOrAddComponent<MouseNear>().OnEndNear += Y;
        public static void OnScroll(this GameObject X, Action<GameObject, PointerEventData> Y) => X.GetOrAddComponent<UIEvent>().onScroll += Y;
        //物体生命周期
        public static void OnNextFrame(this GameObject X, Action Y) => X.GetOrAddComponent<UIEvent>().nextFrameDo += Y;
        public static void OnUpdate(this GameObject X, Action Y) => X.GetOrAddComponent<UIEvent>().updateDo += Y;
        public static void OnSecond(this GameObject X, Action Y) => X.GetOrAddComponent<UIEvent>().secondDo += Y;
        public static void DeleteOnSecond(this GameObject X, Action Y) => X.GetOrAddComponent<UIEvent>().secondDo -= Y;
        public static void RemoveOnUpdate(this GameObject X, Action Y) => X.GetOrAddComponent<UIEvent>().updateDo -= Y;
        public static void OnFixedUpdate(this GameObject X, Action Y) => X.GetOrAddComponent<UIEvent>().fixedUpdateDo += Y;
        public static void OnLateUpdate(this GameObject X, Action Y) => X.GetOrAddComponent<UIEvent>().lateUpdateDo += Y;
        public static void OnFocusIn(this GameObject X, Action<GameObject> Y) => X.GetOrAddComponent<UIEvent>().onSelect += Y;
        public static void OnFocusOut(this GameObject X, Action<GameObject> Y) => X.GetOrAddComponent<UIEvent>().onDeselect += Y;
        public static void OnDestory(this GameObject X, Action<GameObject> Y) => X.GetOrAddComponent<UIEvent>().onDestory += Y;
        //其他组件
        public static void OnSubmit(this GameObject X, UnityAction<string> Y) => X.GetComponent<TMP_InputField>().onEndEdit.AddListener(Y);
        public static void OnTextChange(this GameObject X, Action<GameObject, string> Y) {
            X.GetComponent<TMP_InputField>().onValueChanged.AddListener(t => Y(X, t));
        }
        public static void DeleteOnTextChange(this GameObject X) {
            X.GetComponent<TMP_InputField>()?.onValueChanged.RemoveAllListeners();
        }
        //删除
        public static void DeleteOnClick(this GameObject X, Action<GameObject> Y = null) {
            if (Y == null) X.GetOrAddComponent<UIEvent>().onClick = null;
            else X.GetOrAddComponent<UIEvent>().onClick -= Y;
        }
        public static void DeleteOnMouseDown(this GameObject X, Action<GameObject> Y = null) {
            if (Y == null) X.GetOrAddComponent<UIEvent>().onMouseDown = null;
            else X.GetOrAddComponent<UIEvent>().onMouseDown -= Y;
        }
        public static void DeleteOnMouseUp(this GameObject X, Action<GameObject> Y = null) {
            if (Y == null) X.GetOrAddComponent<UIEvent>().onMouseUp = null;
            else X.GetOrAddComponent<UIEvent>().onMouseUp -= Y;
        }
        public static void DeleteOnDoubleClick(this GameObject X, Action<GameObject> Y = null) {
            if (Y == null) X.GetOrAddComponent<UIEvent>().onDoubleClick = null;
            else X.GetOrAddComponent<UIEvent>().onDoubleClick -= Y;
        }
        public static void DeleteOnRightClick(this GameObject X, Action<GameObject> Y = null) {
            if (Y == null) X.GetOrAddComponent<UIEvent>().onRightClick = null;
            else X.GetOrAddComponent<UIEvent>().onRightClick -= Y;
        }
        //拖动事件
        public static void DeleteOnBeginDrag(this GameObject X, Action<GameObject> Y = null) {
            if (Y == null) X.GetOrAddComponent<UIEvent>().onBeginDrag = null;
            else X.GetOrAddComponent<UIEvent>().onBeginDrag -= Y;
        }
        public static void DeleteOnDrag(this GameObject X, Action<GameObject> Y = null) {
            if (Y == null) X.GetOrAddComponent<UIEvent>().onDrag = null;
            else X.GetOrAddComponent<UIEvent>().onDrag -= Y;
        }
        public static void DeleteOnEndDrag(this GameObject X, Action<GameObject> Y = null) {
            if (Y == null) X.GetOrAddComponent<UIEvent>().onEndDrag = null;
            else X.GetOrAddComponent<UIEvent>().onEndDrag -= Y;
        }
        public static void DeleteOnDrop(this GameObject X, Action<GameObject> Y = null) {
            if (Y == null) X.GetOrAddComponent<UIEvent>().onDrop = null;
            else X.GetOrAddComponent<UIEvent>().onDrop -= Y;
        }
        //鼠标事件    
        public static void DeleteOnEnter(this GameObject X, Action<GameObject> Y = null) {
            if (Y == null) X.GetOrAddComponent<UIEvent>().onEnter = null;
            else X.GetOrAddComponent<UIEvent>().onEnter -= Y;
        }
        public static void DeleteOnMove(this GameObject X, Action<GameObject> Y = null) {
            if (Y == null) X.GetOrAddComponent<UIEvent>().onMove = null;
            else X.GetOrAddComponent<UIEvent>().onMove -= Y;
        }
        public static void DeleteOnExit(this GameObject X, Action<GameObject> Y = null) {
            if (Y == null) X.GetOrAddComponent<UIEvent>().onExit = null;
            else X.GetOrAddComponent<UIEvent>().onExit -= Y;
        }
        public static bool IsPanelAtTop(this GameObject X) {
            //检查一个面板与鼠标之间是否存在其他面板
            //思路：发射一个射线，检查碰到的第一个物体是否是此面板
            var A = X.GetComponent<RectTransform>();
            var B = A.四角坐标();
            Vector2[] 四角坐标 = new Vector2[4];
            for (int i = 0; i < 4; i++) {
                四角坐标[i] = RectTransformUtility.WorldToScreenPoint(null, B[i]);
            }
            for (int i = 0; i < 4; i++) {
                var D = Physics2D.Raycast(四角坐标[i], 四角坐标[(i + 1) % 4] - 四角坐标[i], Vector2.Distance(四角坐标[i], 四角坐标[(i + 1) % 4]));
                //Tobo：D.collider永远为空
                Print(D.collider);
                if (D.collider != null) {
                    if (D.collider.gameObject == X) {
                        Print("当前物体在最上层");
                        return true;
                    } else {
                        Print("当前物体不在最上层");
                        return false;
                    }
                }
            }
            return false;
        }
        public static GameObject SetScrollItem(this GameObject X) { //用于解决滚动阻挡的bug
            X.GetOrAddComponent<UIEvent>().SetScroll();
            return X;
        }
    }
    public static partial class LocalStorage {
        public static GameObject 允许拖动(this GameObject X, GameObject Y = null) {
            if (Y == null) Y = X;
            Vector2 A = Vector2.zero;
            X.OnBeginDrag(t => {
                A = Input.mousePosition - Y.transform.position;
            });
            X.OnDrag(t => {
                Y.transform.position = (Vector2)Input.mousePosition - A;
            });
            return X;
        }
        public static GameObject 允许在其内拖动(this GameObject X, GameObject Y) {
            Vector2 A = Vector2.zero;
            X.OnBeginDrag(t => {
                A = Input.mousePosition - X.transform.position;
            });
            X.OnDrag(t => {
                var 缓存 = X.transform.position;
                X.transform.position = (Vector2)Input.mousePosition - A;
                if (!X.Is在其中(Y)) { //如果物体超出了父物体的范围，将其限制在父物体的范围内
                    X.transform.position = 缓存;
                }
            });
            return X;
        }
    }
    public class MouseNear : MonoBehaviour {
        public const float 边缘距离 = 10f;
        public bool 上一帧在边缘 = false;
        public Action<GameObject> OnBeginNear;
        public Action<GameObject> OnNear;
        public Action<GameObject> OnEndNear;
        public void Update() {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), Input.mousePosition, null, out Vector2 A);
            var B = IsNearEdge(A);
            if (B) {
                OnNear(gameObject);
                if (!上一帧在边缘) {
                    OnBeginNear(gameObject);
                }
            } else if (上一帧在边缘) {
                OnEndNear(gameObject);
            }
            上一帧在边缘 = B;
        }
        bool IsNearEdge(Vector2 X) {
            if (Input.GetMouseButton(0)) return false;
            if (!gameObject.IsPanelAtTop()) return false;
            return Mathf.Abs(X.x) > GetComponent<RectTransform>().rect.width / 2 - 边缘距离 || Mathf.Abs(X.y) > GetComponent<RectTransform>().rect.height / 2 - 边缘距离;
        }
    }
    public class UIEvent : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler,
    IPointerClickHandler, IPointerUpHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler,
    IScrollHandler, ISelectHandler, IUpdateSelectedHandler, IDeselectHandler {
        public Action<GameObject> onClick;
        public Action<GameObject> onDoubleClick;
        public Action<GameObject> onRightClick;
        public Action<GameObject> onMouseDown;
        public Action<GameObject> onMouseUp;
        public Action<GameObject> onEnter;
        public Action<GameObject> onExit;
        public Action<GameObject> onSelect;
        public Action<GameObject> onUpdateSelected;
        public Action<GameObject> onDeselect;
        public Action<GameObject> onMove;
        public Action<GameObject> onBeginDrag;
        public Action<GameObject> onDrag;
        public Action<GameObject> onDrop;
        public Action<GameObject> onEndDrag;
        public Action<GameObject> onDestory;
        public Action<GameObject, PointerEventData> onScroll;
        public Action updateDo;
        public Action nextFrameDo;
        public Action fixedUpdateDo;
        public Action lateUpdateDo;
        public Action secondDo;
        public void Awake() {
            if (GetComponent<Image>() == null && GetComponent<TextMeshProUGUI>() == null) gameObject.SetColor(0, 0, 0, 0);//添加一个透明图片，从而能够遮挡鼠标事件
        }
        private float 当前时间;
        public void Update() {
            updateDo?.Invoke();
            当前时间 += Time.deltaTime;
            if (当前时间 >= 1) {
                当前时间 = 0;
                secondDo?.Invoke();
            }
        }
        public void FixedUpdate() => fixedUpdateDo?.Invoke();
        public void LateUpdate() {
            nextFrameDo?.Invoke();
            nextFrameDo = null;
            lateUpdateDo?.Invoke();
        }
        public void OnDestroy() => onDestory?.Invoke(gameObject);
        public void SetScroll() { //避免物体阻挡拖动
            onScroll = (t, e) => {
                var A = GetComponentInParent<ScrollRect>();
                if (A == null) return;
                A.content.anchoredPosition -= new Vector2(0, e.scrollDelta.y * 50f);
            };
        }
        public void OnPointerClick(PointerEventData X) {
            if (X.clickCount == 2) onDoubleClick?.Invoke(gameObject);
            if (X.clickCount == 1) {
                if (X.button == PointerEventData.InputButton.Left) onClick?.Invoke(gameObject);
                if (X.button == PointerEventData.InputButton.Right) onRightClick?.Invoke(gameObject);
            }
        }
        public void OnPointerDown(PointerEventData X) => onMouseDown?.Invoke(gameObject);
        public void OnPointerUp(PointerEventData X) => onMouseUp?.Invoke(gameObject);
        public void OnPointerEnter(PointerEventData X) => onEnter?.Invoke(gameObject);
        public void OnPointerExit(PointerEventData X) => onExit?.Invoke(gameObject);
        public void OnPointerMove(PointerEventData X) => onMove?.Invoke(gameObject);
        public void OnScroll(PointerEventData X) => onScroll?.Invoke(gameObject, X);
        public void OnSelect(BaseEventData X) => onSelect?.Invoke(gameObject);
        public void OnUpdateSelected(BaseEventData X) => onUpdateSelected?.Invoke(gameObject);
        public void OnDeselect(BaseEventData X) => onDeselect?.Invoke(gameObject);
        public void OnBeginDrag(PointerEventData X) => onBeginDrag?.Invoke(gameObject);
        public void OnDrag(PointerEventData X) => onDrag?.Invoke(gameObject);
        public void OnDrop(PointerEventData X) => onDrop?.Invoke(gameObject);
        public void OnEndDrag(PointerEventData X) => onEndDrag?.Invoke(gameObject);
    }
}