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
    public class UIExplane2 : MonoBehaviour {
        public void SetExplane(string X) {
            gameObject.OnEnter(t => {
                注释驱动.Activing = this;
                注释驱动.Instance.gameObject.SetActive(true);
                注释驱动.Instance.transform.SetAsLastSibling();
                设置注释位置();
                注释驱动.Instance.SetText(X);
            });
            gameObject.OnMove(t => {
                设置注释位置();
            });
            gameObject.OnExit(t => {
                注释驱动.Instance.gameObject.SetActive(false);
                注释驱动.Activing = null;
            });
        }
        public void 设置注释位置() {
            var 注释宽度 = 注释驱动.Instance.GetComponent<RectTransform>().rect.width;
            var 最终位置 = Input.mousePosition;
            //如果注释背景右侧超出屏幕，那么移动到左侧
            if (Input.mousePosition.x + 注释宽度 > Screen.width) {
                最终位置 += new Vector3(-注释宽度 - 5, 0);
            } else {
                最终位置 += new Vector3(20, -20);
            }
            if (Input.mousePosition.y - 注释驱动.Instance.GetComponent<RectTransform>().rect.height < 0) {
                最终位置 += new Vector3(0, 注释驱动.Instance.GetComponent<RectTransform>().rect.height + 5);
            }
            注释驱动.Instance.GetComponent<RectTransform>().position = 最终位置;
        }
        public void OnDestroy() {
            if (注释驱动.Activing == this) {
                注释驱动.Instance.gameObject.SetActive(false);
                注释驱动.Activing = null;
            }
        }
    }
}