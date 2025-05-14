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

/**
 * 1.设置一个物体的图片
 * 2.设置图片颜色
 * 3.设置图片的填充度，填充方向
 */
namespace CMKZ {
    public static partial class LocalStorage {
        public static GameObject SetSprite(this GameObject X, string Y) {
            X.GetOrAddComponent<Image>().sprite = AllSprite[Y];
            return X;
        }
        public static GameObject SetSprite(this GameObject X, Sprite Y) {
            X.GetOrAddComponent<Image>().sprite = Y;
            return X;
        }
        public static GameObject SetColor(this GameObject X, int A, int B, int C, float D = 1) {
            return X.SetColor(new Vector4(A, B, C, D));
        }
        public static GameObject SetColor(this GameObject X, Vector4 Y) {
            X.GetOrAddComponent<Image>().color = Y.ToColor();
            return X;
        }
        public static Color ToColor(this Vector4 X) {
            return new Color(X.x / 255f, X.y / 255f, X.z / 255f, X.w);
        }

        public static void AddRadial(this GameObject X, double Y) {
            X.GetComponentInChildren<Image>().fillAmount += (float)Y;
        }
        public static void SetRadial(this GameObject X, double Y) {
            X.GetComponentInChildren<Image>().fillAmount = (float)Y;
        }
        public static GameObject 初始化扇形进度条(this GameObject X, string 图片名 = "白色图片") {
            if (X.GetComponent<Image>() == null || X.GetComponent<Image>().sprite == null) {
                X.SetSprite(图片名);
            }
            X.GetComponent<Image>().type = Image.Type.Filled;
            X.GetComponent<Image>().fillMethod = Image.FillMethod.Radial360;
            X.GetComponent<Image>().fillOrigin = 0;//起始位置
            X.GetComponent<Image>().fillAmount = 0;//初始填充量
            return X;
        }
    }
}