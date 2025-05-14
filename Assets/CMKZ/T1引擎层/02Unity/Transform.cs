using UnityEngine;
using UnityEngine.UI;
using static CMKZ.LocalStorage;
using static UnityEngine.RectTransform;

namespace CMKZ {
    public static partial class LocalStorage {
        public static GameObject SetAsFirst(this GameObject X) {
            X.transform.SetAsFirstSibling();
            return X;
        }
        public static GameObject SetAsLast(this GameObject X) {
            X.transform.SetAsLastSibling();
            return X;
        }
        public static GameObject SetBefore(this GameObject X, GameObject Y) {
            if (X.GetParent() != Y.GetParent()) {
                PrintWarning($"错误：无法设置 {X.name} 与 {Y.name} 的层级，层级关系非并列！");
                return X;
            }
            X.transform.SetSiblingIndex(Y.transform.GetSiblingIndex() + 1);
            return X;
        }
        public static GameObject SetAfter(this GameObject X, GameObject Y) {
            if (X.GetParent() != Y.GetParent()) {
                PrintWarning($"错误：无法设置 {X.name} 与 {Y.name} 的层级，层级关系非并列！");
                return X;
            }
            X.transform.SetSiblingIndex(Y.transform.GetSiblingIndex());
            return X;
        }
        public static GameObject Find(this GameObject X, string Y) {
            var A = X.transform.Find(Y);
            return A == null ? null : A.gameObject;
        }
        public static GameObject GetParent(this GameObject X) => X.transform.parent.gameObject;
        public static GameObject GetNext(this GameObject X) => X.transform.Next()?.gameObject;
        public static Transform Next(this Transform currentTransform) {
            if (currentTransform == null || currentTransform.parent == null) {
                return null;
            }
            int siblingIndex = currentTransform.GetSiblingIndex();
            int nextSiblingIndex = siblingIndex + 1;
            if (nextSiblingIndex < currentTransform.parent.childCount) {
                return currentTransform.parent.GetChild(nextSiblingIndex);
            }
            return null;
        }
        public static GameObject SetParent(this GameObject X, GameObject Y) {
            X.transform.SetParent(Y.transform, false);
            return X;
        }
        public static GameObject SetName(this GameObject X, string Y) {
            X.name = Y;
            return X;
        }
        public static GameObject AppendNext(this GameObject X) {
            var A = new GameObject();
            A.transform.SetParent(X.transform.parent);
            A.transform.SetSiblingIndex(X.transform.GetSiblingIndex() + 1);
            A.GetOrAddComponent<RectTransform>().pivot = new Vector2(0, 1);
            return A;
        }
        /// <summary>
        /// 1.只有一阶子物体，不包括高阶 <br/>
        /// 2.不包括自身
        /// </summary>
        public static void Clear(this GameObject X) {
            foreach (Transform i in X.transform) {
                if (i.gameObject != X) {
                    i.gameObject.Destroy();
                }
            }
        }
        /// <summary>
        /// 1.只有一阶子物体，不包括高阶 <br/>
        /// 2.不包括自身
        /// </summary>
        public static void ClearWithOut(this GameObject X, GameObject Y, bool Z = false) {
            foreach (Transform i in X.transform) {
                if (i.gameObject != X && i.gameObject != Y) {
                    i.gameObject.Destroy(Z);
                }
            }
        }
        public static void AllClear(this GameObject X) {
            X.Clear();
            foreach (var i in X.GetComponents<Component>()) {
                if (i is not Transform) {
                    i.Destroy();
                }
            }
        }
        /// <summary>
        /// 1.只有一阶子物体，不包括高阶 <br/>
        /// 2.不包括自身
        /// </summary>
        public static List<GameObject> GetChildren(this GameObject X) {
            var A = new List<GameObject>();
            for (int i = 0; i < X.transform.childCount; i++) {
                A.Add(X.transform.GetChild(i).gameObject);
            }
            return A;
        }
        public static List<GameObject> GetChildren(this RectTransform X) {
            var A = new List<GameObject>();
            for (int i = 0; i < X.childCount; i++) {
                A.Add(X.GetChild(i).gameObject);
            }
            return A;
        }
        public static GameObject SetPosition(this GameObject X, string Y) {
            X.GetOrAddComponent<MyTransform>().SetPosition(Y);
            return X;
        }
        public static GameObject SetSize(this GameObject X, Vector2 Size) {
            X.GetOrAddComponent<RectTransform>().SetSizeWithCurrentAnchors(Axis.Horizontal, Size.x);
            X.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(Axis.Vertical, Size.y);
            return X;
        }
    }
}