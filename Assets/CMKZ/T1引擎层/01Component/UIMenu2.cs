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
    public class UIMenu2 : MonoBehaviour, IPointerClickHandler {
        public System.Collections.Generic.List<UIMenuItem> _Items = new();
        public System.Collections.Generic.Dictionary<int, List<UIMenuItem>> Items => _Items.GroupBy(X => X.分组).ToDictionary(X => X.Key, X => X.ToList());
        public void Start() {

        }
        public void Update() {

        }
        public void SetAction(string X, Action Y) {
            var A = Items.Values.SelectMany(t => t).Find(Z => Z.名称 == X);
            if (A == null) {
                PrintWarning($"右键错误：找不到{X}");
                return;
            }
            A.点击效果 = Y;
        }
        public void Clear() {
            Items.Values.SelectMany(t => t).ForEach(X => X.点击效果 = null);
        }

        public void OnPointerClick(PointerEventData e) {
            if (e.button == PointerEventData.InputButton.Right) {
                右键驱动.Activing = this;
                右键驱动.右键冷却.SetToMin();
                右键驱动.Instance ??= 右键驱动.Get();
                右键驱动.Instance.transform.SetAsLastSibling();
                右键驱动.Instance.transform.position = Input.mousePosition + new Vector3(8, -8);
                //如果菜单的最下边缘超出屏幕，就让菜单在鼠标上方显示
                var A = 右键驱动.Instance.GetComponent<RectTransform>();
                if (A.position.y - A.rect.height < 0) {
                    A.position = new Vector3(A.position.x, A.position.y + A.rect.height);
                }
                //如果菜单的最右边缘超出屏幕，就让菜单在鼠标左边显示
                if (A.position.x + A.rect.width > Screen.width) {
                    A.position = new Vector3(A.position.x - A.rect.width, A.position.y);
                }
                foreach (var i in Items.Keys) {
                    右键驱动.Instance.创建列表(Items[i]);
                }
            }
        }
    }
    [Serializable]
    public class UIMenuItem {
        public int 分组;
        public string 名称;//不可重复
        public Action 点击效果;
    }
}