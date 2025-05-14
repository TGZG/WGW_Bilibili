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
    public class 仓库存档类 : List<单仓库存档类> {
        public 仓库存档类(I仓库数据 X) {
            var A = new List<I仓库数据> { X };
            var B = new List<I仓库数据>();
            while (A.Any()) {
                B.AddRange(A);
                A = A.SelectMany(t => t.List).ToList();
            }
            var C = new Dictionary<I仓库数据, uint>();
            uint D = 0;
            B.ForEach(t => C.Add(t, D++));
            B.ForEach(t => Add(new 单仓库存档类 {
                ID = C[t],
                Name = t.Name,
                Children = t.List.Select(x => C[x]).ToArray(),
                Items = JsonConvert.SerializeObject(t.Items),
                IsExpand = t.IsExpand,
                体积 = t.体积
            }));
        }
        //public T To仓库<T>() where T: I仓库数据, new() {
        //    var A = new Dictionary<uint, I仓库数据>();
        //    ForEach(t => A.Add(t.ID, new T {
        //        Name = t.Name,
        //        Items = JsonConvert.DeserializeObject<Dictionary<string, uint>>(t.Items),
        //        IsExpand = t.IsExpand,
        //        体积 = t.体积
        //    }));
        //    ForEach(t => t.Children.ToList().ForEach(x => A[t.ID].List.Add(A[x])));
        //    return A[0] as T;
        //}
    }
    public class 单仓库存档类 {
        public uint ID;
        public string Name;
        public uint[] Children;
        public string Items;
        public bool IsExpand;
        public double 体积;
    }
    public static partial class LocalStorage {
        public static void 仓库Save(this I仓库数据 X, string Y) {

        }
    }
}