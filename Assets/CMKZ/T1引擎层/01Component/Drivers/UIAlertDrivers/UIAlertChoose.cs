using System;//Action
using System.Collections.Generic;//List
using TMPro;//InputField
using UnityEngine;//Mono
using static CMKZ.LocalStorage;
using static UnityEngine.Object;//Destory

namespace CMKZ {
    public static partial class LocalStorage {
        public static UIAlertChoose AlertChoose() {
            if (唯一弹窗) {
                if (唯一弹窗实例 != null) {
                    唯一弹窗实例.Destroy();
                }
                唯一弹窗实例 = Instantiate(AllPrefab["AlertChoose"], MainPanel.transform).SetOutLineWithBlur_White(true);
                return 唯一弹窗实例.GetComponent<UIAlertChoose>();
            } else {
                return Instantiate(AllPrefab["AlertChoose"], MainPanel.transform).SetOutLineWithBlur_White(true).GetComponent<UIAlertChoose>();
            }
        }
        [Obsolete]
        public static void AlertChoose<T>(string 标题, List<T> data, Func<T, string> 显示文字, Func<T, string> 右侧显示文字 = null, Action<T> 确定事件 = null) {
            右侧显示文字 ??= t => "";
            确定事件 ??= t => { };
            AlertChoose().SetTitle(标题).SetData(data, x => (显示文字(x), 右侧显示文字(x)), 确定事件);
        }
        public static UIAlertChoose AlertChoose<T>(this List<T> X, Action<UIAlertChooseNode, T> Y, Action<T> 确定) {
            return AlertChoose().SetData(X, Y, 确定);
        }
    }
    public class UIAlertChoose : MonoBehaviour {
        public TextMeshProUGUI 标题;
        public GameObject 内容区;
        public GameObject 模板;
        public Stack<GameObject> 对象池 = new();
        public UIAlertChoose SetTitle(string X) {
            标题.text = X;
            return this;
        }
        [Obsolete]
        public UIAlertChoose SetData<T>(IEnumerable<T> X, Func<T, (string, string)> 显示, Action<T> 点击) {
            foreach (var i in 内容区.GetChildren()) {
                回收(i);
            }
            foreach (var i in X) {
                取出(内容区).OnClick(t => {
                    点击(i);
                    gameObject.Destroy();
                }).GetComponent<UIAlertChooseNode>().SetText(显示(i));
            }
            return this;
        }
        public UIAlertChoose SetData<T>(IEnumerable<T> X, Action<UIAlertChooseNode, T> Y, Action<T> 确定) {
            foreach (var i in 内容区.GetChildren()) {
                回收(i);
            }
            foreach (var i in X) {
                Y(取出(内容区).OnClick(t => {
                    确定(i);
                    gameObject.Destroy();
                }).GetComponent<UIAlertChooseNode>(), i);
            }
            return this;
        }
        [Obsolete]
        public UIAlertChoose SetData<T>(IEnumerable<T> X, Func<T, (string, string)> 显示, Action<T, int> 点击, Func<T, bool> 筛选) {
            foreach (var i in 内容区.GetChildren()) {
                回收(i);
            }
            var A = -1;
            foreach (var i in X) {
                A++;
                if (!筛选(i)) continue;
                var B = A;
                取出(内容区).OnClick(t => 点击(i, B)).GetComponent<UIAlertChooseNode>().SetText(显示(i));
            }
            return this;
        }
        public GameObject 取出(GameObject Parent) {
            if (对象池.Count == 0) {
                return Instantiate(模板).SetParent(Parent).SetName("已苏醒模板");
            } else {
                return 对象池.Pop().SetParent(Parent).SetName("已苏醒模板");
            }
        }
        public void 回收(GameObject X) {
            X.SetName("已移除");
            X.DeleteOnClick();
            X.SetParent(MainHidding);
            对象池.Push(X);
        }
    }
}