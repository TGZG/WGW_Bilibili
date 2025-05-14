using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis.Scripting;
using Newtonsoft.Json;//Json
using System;//Action
using System.Collections;
using System.Collections.Generic;//List
using System.Diagnostics;
using System.IO;//File
using System.Linq;//from XX select XX
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Timers;//Timer
using TMPro;//InputField
using UnityEngine;//Mono
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;//Image
using UnityEngine.Video;//Vedio
using static CMKZ.LocalStorage;
using static UnityEngine.Object;//Destory
using static UnityEngine.RectTransform;

namespace CMKZ {
    public static partial class LocalStorage {
        public static void OnKeyDown(this KeyCode X, Action Y) {
            MainPanel.OnUpdate(() => {
                if (Input.GetKeyDown(X)) {
                    Y();
                }
            });
        }
        public static GameObject 创建滚动条自动宽度自动高度(this GameObject X, string Y) {
            return X.创建矩形(new PanelConfig() {
                矩形模式 = 矩形模式.滚动条自动高度自动宽度无文本,
                Position = Y,
            });
        }
        public static GameObject 修正悬浮(this GameObject X) {
            if (X.GetComponent<Image>() != null) {
                X.GetComponent<Image>().raycastTarget = false;
            } else if (X.GetComponent<TextMeshProUGUI>() != null) {
                X.GetComponent<TextMeshProUGUI>().raycastTarget = false;
            }
            return X;
        }
    }
    public class 对象池管理器 {
        public GameObject 模版;
        public List<GameObject> 所有模板 = new();
        public List<GameObject> 所有已取出 = new();
        public GameObject 取出(GameObject Parent) {
            if (所有模板.Count == 0) {
                var A = Instantiate(模版).SetParent(Parent);
                所有已取出.Add(A);
                return A;
            } else {
                var A = 所有模板[0];
                所有模板.RemoveAt(0);
                if (A.GetComponent<UIEvent>() != null) {
                    DestroyImmediate(A.GetComponent<UIEvent>());
                }
                if (A.GetComponent<ColorConfig>() != null) {
                    DestroyImmediate(A.GetComponent<ColorConfig>());
                }
                if (A.GetComponent<UIMenu>() != null) {
                    DestroyImmediate(A.GetComponent<UIMenu>());
                }
                if (A.GetComponent<UIExplane>() != null) {
                    DestroyImmediate(A.GetComponent<UIExplane>());
                }
                所有已取出.Add(A);
                return A.SetParent(Parent);
            }
        }
        public void 回收所有() {
            List<Action> actions = new();
            foreach (var i in 所有已取出) {
                actions.Add(() => {
                    回收(i);
                });
            }
            foreach (var i in actions) i();
            所有已取出.Clear();
        }
        public void 回收(GameObject X) {
            X.SetName("已移除");
            X.SetParent(MainHidding);
            所有模板.Add(X);
            所有已取出.Remove(X);
        }
    }
}