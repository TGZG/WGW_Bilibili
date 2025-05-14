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
    public static partial class LocalStorage {
        public static GameObject SetCameraFollow(this GameObject X) {
            Camera.main.transform.position = X.transform.position.SetZ(-11);
            var A = Camera.main.transform.position - X.transform.position;
            MainPanel.OnLateUpdate(() => {
                Camera.main.transform.position = X.transform.position + A;
                Camera.main.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * 5f;//缩放速度
                Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 2, 10);//缩放范畴
            });
            return X;
        }
    }
}