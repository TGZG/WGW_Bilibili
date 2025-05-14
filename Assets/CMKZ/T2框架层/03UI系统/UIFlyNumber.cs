using TMPro;//InputField
using UnityEngine;//Mono

namespace CMKZ {
    public static partial class LocalStorage {
        public static List<GameObject> 飘字物体 = new();
        public static GameObject GetNew飘字物体() {
            if (飘字物体.Count == 0) {
                return MainPanel.创建矩形(new PanelConfig {
                    矩形模式 = 矩形模式.固定文本框,
                    Position = "0 0 100 100",
                    TextSize = 30,
                });
            } else {
                var A = 飘字物体[0];
                飘字物体.RemoveAt(0);
                A.SetActive(true);
                return A;
            }
        }
        public static void Release飘字物体(GameObject X) {
            飘字物体.Add(X);
            X.SetActive(false);
        }
        public static void 战斗飘字(string 文本, Vector2 起点, Vector3 颜色 = default) {
            if (颜色 == default) { //0-255
                var R域 = new int[] { 200, 225, 250 };
                var G域 = new int[] { 0, 25, 50, 75, 100, 125, 150, 200, 250 };
                var B域 = new int[] { 0, 25, 50, 75, 100, 125, 150, 200, 250 };
                颜色 = new Vector3(R域.Choice(), G域.Choice(), B域.Choice());
            }
            var 实际颜色 = new Color(颜色.x / 255, 颜色.y / 255, 颜色.z / 255, 1);
            GameObject A = GetNew飘字物体();
            var 组件 = A.GetComponentInChildren<TextMeshProUGUI>();
            组件.text = 文本;
            组件.raycastTarget = false;
            var 方向 = new Vector2(Random(-100, 100), Random(50, 100));
            A.动画变化(3, t => {
                var s = (float)t / 3f;
                A.transform.position = 起点 + 方向 * s.标准凸函数();//越来越慢
                组件.color = 实际颜色.SetAlpha(1 - s);
            }, () => Release飘字物体(A));
        }
    }
}