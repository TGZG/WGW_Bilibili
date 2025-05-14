using System.Collections;
using UnityEngine;//Mono

namespace CMKZ {
    public static partial class LocalStorage {
        public static GameObject CreateGameObject(this GameObject X, string Y) {
            var A = new GameObject(Y);
            A.transform.SetParent(X.transform);
            return A;
        }
        public static void 闪烁(this MonoBehaviour X, Color Y) {
            if (X.GetComponent<SpriteRenderer>() == null) {
                PrintWarning("闪烁失败：不存在图片");
                return;
            }
            X.StopAllCoroutines();
            var A = X.gameObject.GetComponent<SpriteRenderer>().color;
            X.GetComponent<SpriteRenderer>().color = Y;
            X.StartCoroutine(F());
            IEnumerator F() {
                yield return new WaitForSeconds(0.1f);
                X.gameObject.GetComponent<SpriteRenderer>().color = A;
            }
        }
    }
}