using Microsoft.CodeAnalysis;
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
using UnityEngine.Tilemaps;
using UnityEngine.UI;//Image
using UnityEngine.Video;//Vedio
using static CMKZ.LocalStorage;
using static UnityEngine.Object;//Destory
using static UnityEngine.RectTransform;

namespace CMKZ {
    public static partial class LocalStorage {
        public static void Update(this IComponent X, double 时间) {
            if (!X.IsInit) {
                X.IsInit = true;
                X.Start();
            }
            X.OnUpdate(时间);
        }
        public static void UpdateComponents(this IComponentController X, double 时间) {
            foreach (var i in X.Components) {
                i.Update(时间);
            }
        }
        public static T AddComponent<T>(this IComponentController X) where T : IComponent, new() {
            var A = new T();
            A.Parent = X;
            X.Components.Add(A);
            A.Awake();
            return A;
        }
    }
    public interface IComponentController {
        public List<IComponent> Components { get; }
    }
    public interface IComponent {
        public IComponentController Parent { get; set; }
        public bool IsInit { get; set; }
        public void Awake();
        public void Start();
        public void OnUpdate(double 时间);
        public void Destroy();
    }
}