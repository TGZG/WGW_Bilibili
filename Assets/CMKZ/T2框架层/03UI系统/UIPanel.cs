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
        public static Action<Panel> OnPanelOpen;
        public static T FindPanel<T>() where T : Panel {
            return Panel.All.Find(t => t is T) as T;
        }
    }
    public class Panel {
        public static List<Panel> All = new();
        public GameObject Parent;
        public GameObject 背景;
        public bool IsShow => 背景.activeSelf;
        public Panel(GameObject X) {
            Parent = X;
            All.Add(this);
        }
        public virtual void Init() {

        }
        public virtual void Hide() {
            背景.SetActive(false);
            背景.transform.SetAsFirstSibling();
        }
        public virtual void Show() {
            背景.SetActive(true);
            背景.transform.SetAsLastSibling();
            OnPanelOpen?.Invoke(this);
        }
        public virtual void ChangeActive() {
            if (IsShow) {
                Hide();
            } else {
                Show();
            }
        }
        public virtual void Destroy() {
            All.Remove(this);
            UnityEngine.Object.Destroy(背景);
        }
    }
}