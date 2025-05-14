using Newtonsoft.Json;//Json
using System;//Action
using System.IO;//File
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;//List
using System.Diagnostics;
using System.Linq;//from XX select XX
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Timers;//Timer
using TMPro;//InputField
using UnityEngine;//Mono
using UnityEngine.UI;//Image
using UnityEngine.Tilemaps;
using UnityEngine.Video;//Vedio
using static UnityEngine.Object;//Destory
using static CMKZ.LocalStorage;
using static UnityEngine.RectTransform;
using Random = UnityEngine.Random;

namespace CMKZ {
    public static partial class LocalStorage {
        public static GameObject SetColors(this GameObject X, Colors Y) {
            X.GetOrAddComponent<ColorConfig>().Colors = Y;
            X.GetOrAddComponent<ColorConfig>().InitColors();
            X.SetColor(X.GetOrAddComponent<ColorConfig>().Colors.常规颜色);
            //检测如果鼠标在本物体内，则执行onenter
            //if (X.GetOrAddComponent<ColorConfig>().IsInit) {
            //    if (IsMouseNear(X, 0)) {
            //        X.SetColor(X.GetOrAddComponent<ColorConfig>().Colors.常规颜色);
            //    } else {
            //        X.SetColor(X.GetOrAddComponent<ColorConfig>().Colors.悬浮颜色);
            //    }
            //}
            return X;
        }
    }
    public class ColorConfig : MonoBehaviour {
        public Colors Colors;
        public bool IsInit;
        public void InitColors() {
            if (IsInit) return;
            IsInit = true;
            gameObject.OnEnter(XX => {
                if (Input.GetMouseButton(0)) return;
                XX.SetColor(XX.GetOrAddComponent<ColorConfig>().Colors.悬浮颜色);
            });
            gameObject.OnMouseDown(XX => {
                XX.SetColor(XX.GetOrAddComponent<ColorConfig>().Colors.按下颜色);
            });
            gameObject.OnMouseUp(XX => {
                XX.SetColor(XX.GetOrAddComponent<ColorConfig>().Colors.悬浮颜色);
            });
            gameObject.OnExit(XX => {
                if (Input.GetMouseButton(0)) return;
                XX.SetColor(XX.GetOrAddComponent<ColorConfig>().Colors.常规颜色);
            });
            gameObject.OnEndDrag(XX => {
                XX.SetColor(XX.GetOrAddComponent<ColorConfig>().Colors.常规颜色);
            });
            gameObject.OnDrop(XX => {
                XX.SetColor(XX.GetOrAddComponent<ColorConfig>().Colors.悬浮颜色);
            });
        }
    }
}