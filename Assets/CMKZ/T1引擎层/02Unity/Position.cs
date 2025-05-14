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
        public static bool Is在其中(this GameObject 子, GameObject 父) {
            var 父四角 = 父.GetComponent<RectTransform>().四角坐标();
            var 子四角 = 子.GetComponent<RectTransform>().四角坐标();
            float[] 父边 = new float[4] { 父四角[0].x, 父四角[1].y, 父四角[2].x, 父四角[3].y };
            float[] 子边 = new float[4] { 子四角[0].x, 子四角[1].y, 子四角[2].x, 子四角[3].y };
            return 父边[0] <= 子边[0] && 父边[1] >= 子边[1] && 父边[2] >= 子边[2] && 父边[3] <= 子边[3];
        }
        public static bool Is在其中(this GameObject 子, Vector3[] 父四角) {
            var 子四角 = 子.GetComponent<RectTransform>().四角坐标();
            float[] 父边 = new float[4] { 父四角[0].x, 父四角[1].y, 父四角[2].x, 父四角[3].y };
            float[] 子边 = new float[4] { 子四角[0].x, 子四角[1].y, 子四角[2].x, 子四角[3].y };
            return 父边[0] <= 子边[0] && 父边[1] >= 子边[1] && 父边[2] >= 子边[2] && 父边[3] <= 子边[3];
        }
        public static bool Is有重叠(this GameObject 子, GameObject 父) {
            var 父四角 = 父.GetComponent<RectTransform>().四角坐标();
            var 子四角 = 子.GetComponent<RectTransform>().四角坐标();
            if (子四角[0].IsIn(父四角) || 子四角[1].IsIn(父四角) || 子四角[2].IsIn(父四角) || 子四角[3].IsIn(父四角)) {
                return true;
            }
            return false;
        }
        public static bool IsMouseHere(this GameObject X) => RectTransformUtility.RectangleContainsScreenPoint(X.GetOrAddComponent<RectTransform>(), Input.mousePosition, MainPanel.transform.parent.GetComponent<Canvas>().worldCamera);
        public static bool IsMouseNear(this GameObject 此物体, float 扩展像素) {
            Vector3[] A = 此物体.GetComponent<RectTransform>().四角坐标();
            Vector2 mouse = Input.mousePosition;
            return mouse.x > A[0].x - 扩展像素
                && mouse.x < A[2].x + 扩展像素
                && mouse.y < A[1].y + 扩展像素
                && mouse.y > A[3].y - 扩展像素;
        }
        /// <summary>
        /// 左下，左上，右上，右下
        /// </summary>
        public static Vector3[] 四角坐标(this RectTransform 矩形) {
            Vector3[] A = new Vector3[4];
            矩形.GetWorldCorners(A);
            return A;
        }
        public static bool 重合于(this GameObject X, GameObject Y) {
            return X.GetComponent<RectTransform>().IsOverlapping(Y.GetComponent<RectTransform>());
        }
        public static bool IsOverlapping(this RectTransform rect1, RectTransform rect2) {
            Rect rect1World = GetWorldRect(rect1);
            Rect rect2World = GetWorldRect(rect2);
            return rect1World.Overlaps(rect2World);
        }
        public static Rect GetWorldRect(RectTransform rectTransform) {
            Vector3[] corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);
            Vector3 bottomLeft = corners[0];
            Vector3 topRight = corners[2];
            return new Rect(bottomLeft.x, bottomLeft.y, topRight.x - bottomLeft.x, topRight.y - bottomLeft.y);
        }
    }
}