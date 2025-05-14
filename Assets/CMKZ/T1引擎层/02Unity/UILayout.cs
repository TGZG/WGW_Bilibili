using Newtonsoft.Json;//Json
using System;//Action
using System.IO;//File
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;//List
using System.Diagnostics;
using System.Linq;//from XX select XX
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Timers;//Timer
using TMPro;//InputField
using UnityEngine;//Mono
using UnityEngine.UI;//Image
using UnityEngine.Tilemaps;
using UnityEngine.Video;//Vedio
using static UnityEngine.Object;//Destory
using static CMKZ.LocalStorage;

namespace CMKZ {
    public static partial class LocalStorage {
        public static GameObject 设置网格布局(this GameObject X, int 宽度, int 高度, int 边距 = 0, Vector2 spacing = default) {
            X.GetOrAddComponent<GridLayoutGroup>().cellSize = new(宽度, 高度);
            X.GetComponent<GridLayoutGroup>().padding = new(边距, 边距, 边距, 边距);
            X.GetComponent<GridLayoutGroup>().spacing = spacing;
            return X;
        }
        public static GameObject SetGridLayout(this GameObject X) {
            X.AddComponent<GridLayoutGroup>().spacing = new Vector2(8, 6);
            return X;
        }
        public static GameObject SetLayoutSize(this GameObject X, Vector2 Size) {
            X.GetOrAddComponent<LayoutElement>().preferredWidth = Size.x;
            X.GetOrAddComponent<LayoutElement>().preferredHeight = Size.y;
            return X;
        }
        public static GameObject SetLayoutSize(this GameObject X, float Y, float Z) {
            X.GetOrAddComponent<LayoutElement>().preferredWidth = Y;
            X.GetOrAddComponent<LayoutElement>().preferredHeight = Z;
            return X;
        }
    }
}