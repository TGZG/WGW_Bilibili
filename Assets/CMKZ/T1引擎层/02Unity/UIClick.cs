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
    public static partial class LocalStorage {
        public static void OnStayClick(this GameObject X, Action Y) => X.GetOrAddComponent<StayClick>().Click += Y;
    }
    public class StayClick : MonoBehaviour {
        public Action Click;
        private void Update() {
            if (Input.GetMouseButtonDown(0)) if (gameObject.IsMouseHere()) Click?.Invoke();
        }
    }
}