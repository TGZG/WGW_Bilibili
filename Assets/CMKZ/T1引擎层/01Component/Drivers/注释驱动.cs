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
    public class 注释驱动 : MonoBehaviour {
        private static 注释驱动 _Instance;
        public static 注释驱动 Instance {
            get {
                if (_Instance == null) {
                    _Instance = MainPanel.LoadPrefab("注释背景").GetComponent<注释驱动>();
                    _Instance.gameObject.SetActive(false);
                }
                return _Instance;
            }
        }
        public static UIExplane2 Activing;
        public TextMeshProUGUI 文本;
        public void SetText(string X) {
            文本.text = X;
        }
    }
}