using UnityEngine;
using UnityEngine.UI;
using static CMKZ.LocalStorage;

namespace CMKZ {
    public static partial class LocalStorage {
        public static GameObject ChangeActive(this GameObject X) {
            X.SetActive(!X.activeSelf);
            return X;
        }
        public static GameObject ChangeNextActive(this GameObject X) {
            X.GetNext().ChangeActive();
            //X的父物体强制刷新
            LayoutRebuilder.ForceRebuildLayoutImmediate(X.GetParent().GetComponent<RectTransform>());
            LayoutRebuilder.ForceRebuildLayoutImmediate(X.GetParent().GetParent().GetComponent<RectTransform>());
            LayoutRebuilder.ForceRebuildLayoutImmediate(X.GetParent().GetParent().GetParent().GetComponent<RectTransform>());
            //X.GetParent().GetComponent<RectTransform>().ForceUpdateRectTransforms();
            return X;
        }
        public static GameObject SetNextActive(this GameObject X, bool Y) {
            X.GetNext().SetActive(Y);
            //X的父物体强制刷新
            LayoutRebuilder.ForceRebuildLayoutImmediate(X.GetParent().GetComponent<RectTransform>());
            LayoutRebuilder.ForceRebuildLayoutImmediate(X.GetParent().GetParent().GetComponent<RectTransform>());
            LayoutRebuilder.ForceRebuildLayoutImmediate(X.GetParent().GetParent().GetParent().GetComponent<RectTransform>());
            //X.GetParent().GetComponent<RectTransform>().ForceUpdateRectTransforms();
            return X;
        }
        public static void Show(this GameObject X) {
            X.GetOrAddComponent<CanvasGroup>().alpha = 1;
            X.transform.SetAsLastSibling();
        }
        public static void Hide(this GameObject X) {
            X.GetOrAddComponent<CanvasGroup>().alpha = 0;
            X.transform.SetAsFirstSibling();
        }
        public static bool IsShow(this GameObject X) {
            return X.GetOrAddComponent<CanvasGroup>().alpha == 1;
        }
        public static void 强制刷新() {
            MainPanel.OnNextFrame(() => 执行X次(2, () => MainPanel.ChangeActive()));
        }
        public static void 强制刷新(this GameObject 刷新物体) => 刷新物体.OnNextFrame(() => 执行X次(2, () => 刷新物体.ChangeActive()));
        public static void ChangeActiveTwice(this GameObject X) {
            X.SetActive(false);
            X.SetActive(true);
        }
        public static void 刷新(this Behaviour X) {
            X.enabled = false;
            X.enabled = true;
        }
    }
}