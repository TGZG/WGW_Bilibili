using Microsoft.CodeAnalysis;
using Newtonsoft.Json;//Json
using System;//Action
using System.IO;//File
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;//List
using System.Diagnostics;
using System.Linq;//from XX select XX
using System.Reflection;
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
    public enum MouseType {
        常规,
        手指,
        引用,
        缩放,
        上,
        左,
        下,
        右,
        左上,
        左下,
        右上,
        右下,
    }
    public static partial class LocalStorage {
        public static void SetCursor(Texture2D X) {
            Cursor.SetCursor(X, new Vector2(X.width / 2, X.height / 2), CursorMode.Auto);
        }
        public static void SetCursor(string X) {
            SetCursor(AllT2D[X]);
        }
        public static void SetCursor(MouseType X) {
            SetCursor(X.ToString());
        }
    }
}