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
        public static Vector2 ShouldBiggerThan(this Vector2 X, int Y) {
            return X.ShouldBiggerThan(new Vector2(Y, Y));
        }
        public static Vector2 ShouldBiggerThan(this Vector2 X, Vector2 Y) {
            return new Vector2(Math.Max(X.x, Y.x), Math.Max(X.y, Y.y));
        }
        public static Vector2 ShouldSmallerThan(this Vector2 X, int Y) {
            return X.ShouldSmallerThan(new Vector2(Y, Y));
        }
        public static Vector2 ShouldSmallerThan(this Vector2 X, Vector2 Y) {
            return new Vector2(Math.Min(X.x, Y.x), Math.Min(X.y, Y.y));
        }
        public static Vector3 SetZ(this Vector3 X, float Z) {
            return new Vector3(X.x, X.y, Z);
        }
        public static Vector3 SetZ(this Vector2 X, float Z) {
            return new Vector3(X.x, X.y, Z);
        }
        public static Vector2Int RandomNear(this Vector2Int X) {
            return new Vector2Int(X.x + Random(-1, 2), X.y + Random(-1, 2));
        }
        public static Vector2Int 限制(this Vector2Int X, Vector2Int Y, Vector2Int Z) {
            X.Clamp(Y, Z);
            return X;
        }
        public static Vector2 SetY(this Vector2 X, Func<float, float> Y) {
            X.y = Y(X.y);
            return X;
        }
        public static float 获得两向量间的夹角(Vector3 fromVector, Vector3 toVector) {
            float angle = Vector3.Angle(fromVector, toVector); //求出两向量之间的夹角
            Vector3 normal = Vector3.Cross(fromVector, toVector);//叉乘求出法线向量
            angle *= Mathf.Sign(Vector3.Dot(normal, new Vector3(0, 0, 1)));
            return angle;
        }
        public static bool IsIn(this Vector3 A, Vector3[] B) {
            return A.x > B[0].x && A.x < B[2].x && A.y < B[1].y && A.y > B[3].y;
        }
    }
}