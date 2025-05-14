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
using System.Reflection;

namespace CMKZ {
    public static partial class LocalStorage {
        public static GameObject Clone(this GameObject X) {
            var A = Instantiate(X);
            var 所有子物体 = X.GetComponentsInChildren<Transform>(); //包括自身
            var 克隆子物体 = A.GetComponentsInChildren<Transform>();
            for (var i = 0; i < 所有子物体.Length; i++) {
                var 子物体事件 = 所有子物体[i].GetComponent<UIEvent>();
                //克隆事件
                if (子物体事件 != null) {
                    var 克隆事件 = 克隆子物体[i].GetComponent<UIEvent>();
                    克隆所有字段(子物体事件, 克隆事件);
                }
                //克隆菜单
                if (所有子物体[i].GetComponent<UIMenu>() != null) {
                    克隆子物体[i].GetComponent<UIMenu>().分组 = 所有子物体[i].GetComponent<UIMenu>().分组.Clone();
                    //克隆子物体[i].GetComponent<UIMenu>().Start();
                }
                //克隆colors
                if (所有子物体[i].GetComponent<ColorConfig>() != null) {
                    克隆子物体[i].GetComponent<ColorConfig>().Colors = 所有子物体[i].GetComponent<ColorConfig>().Colors;
                    克隆子物体[i].GetComponent<ColorConfig>().IsInit = 所有子物体[i].GetComponent<ColorConfig>().IsInit;
                }
            }
            return A;
        }
        public static void 克隆所有字段<T>(T 原, T 克隆) {
            foreach (var 字段 in typeof(T).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)) {
                字段.SetValue(克隆, 字段.GetValue(原));
            }
        }
    }
}