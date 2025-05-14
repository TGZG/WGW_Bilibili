using System;
using TMPro;//InputField
using UnityEngine;//Mono
using static UnityEngine.Object;//Destory
using static CMKZ.LocalStorage;
using System.Collections.Generic;
using System.Linq;

namespace CMKZ {
    public static partial class LocalStorage {
        public static UITree CreateUITree(GameObject X = null) {
            X ??= MainPanel;
            var A = Instantiate(AllPrefab["目录树"], X.transform);
            return A.GetComponent<UITree>();
        }
    }
    public class UITree : MonoBehaviour {
        public GameObject Template;
        public GameObject Content;
        public Action<GameObject, Tree> OnDirDraw;
        public Action<GameObject, Tree> OnLastDirDraw;
        public Action<GameObject, Tree> OnFileDraw;
        private Tree Data;
        public string 字体颜色 = "";

        public KeyValueList<Tree, GameObject> 项缓存 = new();
        public KeyValueList<Tree, GameObject> 目录缓存 = new();

        public UITree SetPosition(string X) {
            gameObject.SetPosition(X);
            return this;
        }
        public UITree SetTree(Tree X) {
            项缓存.Clear();
            Data = X;
            foreach (var i in X.Children) {
                _SetTree(i, Content);
            }
            return this;
        }
        private void _SetTree(Tree X, GameObject Y) {
            var A = NewItem(X.Name, Y);
            A.SetScrollItem();
            A.Find("本体/展开折叠").SetScrollItem();
            A.Find("本体").SetScrollItem();
            if (X.IsNoChild) { //如果没有子节点，那么就不显示展开折叠图标
                A.Find("下级").SetActive(false);
                A.Find("本体/展开折叠").SetActive(false);
                A.Find("本体").GetComponent<UIClickActive>().SetActive(false);
                项缓存.Add(X, A);
                OnFileDraw?.Invoke(A.Find("本体"), X);
            } else {
                A.Find("下级").SetActive(false);
                A.Find("本体/展开折叠").SetActive(true);
                foreach (var i in X.Children) {
                    _SetTree(i, A.Find("下级"));
                }
                目录缓存.Add(X, A);
                OnDirDraw?.Invoke(A.Find("本体"), X);
                if (X.Children.All(t => t.IsNoChild)) {
                    OnLastDirDraw?.Invoke(A.Find("本体"), X);
                }
            }
        }
        public void 刷新() {
            RemoveAll();
            SetTree(Data);
        }
        public void 筛选(Func<Tree, bool> X) {
            显示所有目录();
            项缓存.ForEach(t => {
                if (X(t.Key)) {
                    t.Value.SetActive(true);
                } else {
                    t.Value.SetActive(false);
                }
            });
            隐藏无文件目录();
        }
        public void 隐藏无文件目录() {
            Data.Children.ForEach(t => {
                _隐藏无文件目录(t, Data);
            });
        }
        private bool _隐藏无文件目录(Tree t, Tree Parent) {
            if (t.Children.Count != 0) {//如果是目录
                List<bool> 隐藏情况 = new();
                //递归获取下级的隐藏情况
                t.Children.ForEach(i => {
                    if (_隐藏无文件目录(i, t)) {
                        隐藏情况.Add(true);
                    } else {
                        隐藏情况.Add(false);
                    }
                });
                if (隐藏情况.All(s => s)) {//如果所有的下级都隐藏：全部是true
                    //那么本级也隐藏
                    目录缓存[t].SetActive(false);
                    return true;
                } else {
                    //下级有文件
                    return false;
                }
            }
            //如果是文件
            //如果文件显示，那么返回false
            //如果文件隐藏，那么返回true
            if (项缓存[t].activeSelf) {
                return false;
            } else {
                return true;
            }
        }
        private void 显示所有目录() {
            目录缓存.ForEach(t => {
                t.Value.SetActive(true);
            });
        }
        public void 筛选清空() {
            显示所有目录();
            项缓存.ForEach(t => {
                t.Value.SetActive(true);
            });
        }

        //对象池
        public List<GameObject> 所有沉睡模板 = new();
        public Stack<GameObject> 所有活跃模板 = new();
        public GameObject NewItem(string X, GameObject Y) {
            if (所有沉睡模板.Count == 0) {
                var A = Instantiate(Template, Y.transform);
                A.SetActive(true);
                A.Find("本体/Text").GetComponent<TextMeshProUGUI>().text = 字体颜色 + X;
                A.Find("本体").GetComponent<UIClickActive>().SetActive(true);
                所有活跃模板.Push(A);
                return A;
            } else {
                var A = 所有沉睡模板[0];
                所有沉睡模板.RemoveAt(0);
                A.SetActive(true);
                A.Find("本体/Text").GetComponent<TextMeshProUGUI>().text = 字体颜色 + X;
                A.Find("本体").GetComponent<UIClickActive>().SetActive(true);
                所有活跃模板.Push(A);
                return A.SetParent(Y);
            }
        }
        public void RemoveAll() {
            项缓存.Clear();
            目录缓存.Clear();
            while (所有活跃模板.Count != 0) {
                var A = 所有活跃模板.Pop();
                A.SetName("已移除");
                A.SetParent(MainHidding);
                所有沉睡模板.Add(A);
            }
        }
    }
}