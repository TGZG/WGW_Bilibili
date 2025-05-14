using UnityEngine;
using UnityEngine.UI;
using static CMKZ.LocalStorage;
using static UnityEngine.Object;

namespace CMKZ {
    public static partial class LocalStorage {
        public static T GetOrAddComponent<T>(this GameObject X) where T : Component => (X.GetComponent<T>() == null) ? X.AddComponent<T>() : X.GetComponent<T>();
        public static T GetOrAddComponent<T>(this Component X) where T : Component => X.gameObject.GetOrAddComponent<T>();
        public static void Destroy(this Object X, bool Y = false) {
            if (Y) {
                DestroyImmediate(X);
            } else {
                Object.Destroy(X);
            }
        }
        public static void DeleteComponent<T>(this GameObject X) where T : Component {
            if (X.TryGetComponent<T>(out var A)) {
                DestroyImmediate(A);
            }
        }
    }
}