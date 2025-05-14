using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static CMKZ.LocalStorage;

namespace CMKZ {
    public class GameObjectData : MonoBehaviour {
        public Dictionary<string, string> StringData = new();
        public Dictionary<string, object> ObjectData = new();
    }
    public static partial class LocalStorage {
        public static GameObject SetStringData(this GameObject X, string Y, string Z) {
            X.GetOrAddComponent<GameObjectData>().StringData[Y] = Z;
            return X;
        }
        public static string GetStringData(this GameObject X, string Y) {
            return X.GetOrAddComponent<GameObjectData>().StringData[Y];
        }
        public static GameObject SetObjectData(this GameObject X, string Y, object Z) {
            X.GetOrAddComponent<GameObjectData>().ObjectData[Y] = Z;
            return X;
        }
        public static T GetObjectData<T>(this GameObject X, string Y) {
            return (T)X.GetOrAddComponent<GameObjectData>().ObjectData[Y];
        }
    }
}