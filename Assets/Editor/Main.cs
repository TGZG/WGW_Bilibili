using UnityEngine;
using UnityEditor;
using static UnityEngine.Object;
using static CMKZ.LocalStorage;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

//叉号，创建弹窗
namespace CMKZ {
    public static class CustomCreateMenu {
        /// <summary>
        /// 图片，叉号
        /// </summary>
        [MenuItem("GameObject/CMKZ/创建叉号", false, 10)]
        public static void 创建叉号(MenuCommand X) {
            var A = X.Create("叉号");
        }
        /// <summary>
        /// 标题栏，叉号，内容区
        /// </summary>
        [MenuItem("GameObject/CMKZ/创建窗口", false, 10)]
        public static void 创建窗口(MenuCommand X) {
            var A = X.CreateFromPrefab("窗口");
        }
        public static GameObject Create(this MenuCommand X, string Y = "CMKZ") {
            var A = new GameObject(Y);
            GameObjectUtility.SetParentAndAlign(A, X.context as GameObject);
            Undo.RegisterCreatedObjectUndo(A, Y);//注册撤销操作
            Selection.activeObject = A;//选中新创建的对象
            return A;
        }
        public static GameObject CreateFromPrefab(this MenuCommand X, string Y, string Z = "CMKZ") {
            var A = Instantiate(Resources.Load<GameObject>($"Prefab/{Y}")).SetName(Z);
            GameObjectUtility.SetParentAndAlign(A, X.context as GameObject);
            Undo.RegisterCreatedObjectUndo(A, Z);//注册撤销操作
            Selection.activeObject = A;//选中新创建的对象
            return A;
        }
    }
    [CustomEditor(typeof(UIHightLight))]
    public class UIHightLightEditor : Editor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            var A = (UIHightLight)target;
            A.OnExit = EditorGUILayout.ColorField("On Exit", A.OnExit);
            if (GUI.changed) {
                EditorUtility.SetDirty(target);
            }
        }
    }
    [CustomEditor(typeof(UIColors))]
    public class UIColorsEditor : Editor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            var A = (UIColors)target;
            A.OnExit = EditorGUILayout.ColorField("On Exit", A.OnExit);
            if (GUI.changed) {
                EditorUtility.SetDirty(A);
            }
        }
    }
}