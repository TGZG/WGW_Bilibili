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
using UnityEngine.Tilemaps;
using UnityEngine.UI;//Image
using UnityEngine.Video;//Vedio
using static CMKZ.LocalStorage;
using static UnityEngine.Object;//Destory
using static UnityEngine.RectTransform;

namespace CMKZ {
    public class 右键驱动 : MonoBehaviour, IPoolable<右键驱动> {
        public static 限数 右键冷却 = new(0.1f);
        public static 右键驱动 Instance;
        public static UIMenu2 Activing;
        public RectTransform 列表背景;//自己
        public GameObject 列表模板;
        public 右键条目驱动 条目模板;
        public GameObject 缝隙模板;
        public void Start() {
            列表模板.gameObject.SetParent(MainHidding);
            条目模板.gameObject.SetParent(MainHidding);
            缝隙模板.gameObject.SetParent(MainHidding);
        }
        public void Update() {
            右键冷却.增加(Time.deltaTime);
            if (Input.GetMouseButtonUp(0) && Instance != null && 右键冷却.已满) {
                Instance.Release();
            }
        }
        public void 绘制分割线(GameObject Y) {
            Instantiate(缝隙模板, Y.transform);
        }
        public void 绘制条目(UIMenuItem X, GameObject Y) {
            var A = Instantiate(条目模板.gameObject, Y.transform).GetComponent<右键条目驱动>();
            A.SetData(X.名称, () => {
                X.点击效果();
                Release();
            });
        }
        public void 创建列表(List<UIMenuItem> X) {
            var A = Instantiate(列表模板, 列表背景.transform);
            A.Clear();//不好
            foreach (var i in X) {
                绘制条目(i, A);
            }
            绘制分割线(A);
            LayoutRebuilder.ForceRebuildLayoutImmediate(列表背景);
        }

        public 右键驱动 OnGet() {
            gameObject.SetParent(MainPanel);
            return this;
        }
        public 右键驱动 OnRelease() {
            列表背景.Clear();
            return this;
        }
        public static Stack<右键驱动> Pool = new();
        public static 右键驱动 Get() {
            return Pool.Count > 0 ? Pool.Pop().OnGet() : MainPanel.LoadPrefab("右键背景").GetComponent<右键驱动>();
        }
        public void Release() {
            OnRelease();
            gameObject.SetParent(MainHidding);
            Pool.Push(this);
            Instance = null;
            Activing = null;
        }
    }
}