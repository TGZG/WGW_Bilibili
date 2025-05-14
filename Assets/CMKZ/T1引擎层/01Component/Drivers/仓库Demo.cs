using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis.Scripting;
using Newtonsoft.Json;//Json
using Newtonsoft.Json.Serialization;
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
    public class 仓库数据 : I仓库数据 {
        public I仓库数据 Parent { get; set; }
        public string Name { get; set; }
        public List<I仓库数据> List { get; set; } = new();
        public bool IsLast => List.Count == 0;
        [JsonIgnore]
        public List<I仓库元素> Items { get; set; } = new();
        [JsonProperty]
        private string _Items {
            get {
                return Items.ToString(t => t.ToArchiveString());
            }
            set {
                Items = value.Trim(' ').Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(t => {
                    var A = new 仓库元素();
                    A.FromArchiveString(t);
                    A.Parent = this;
                    return A;
                }).Cast<I仓库元素>().ToList();
            }
        }
        public bool IsExpand { get; set; }
        public Number 体积 { get; set; }
        public I仓库数据 Create(string X) {
            var A = new 仓库数据 { Name = X };
            A.Parent = this;
            List.Add(A);
            return A;
        }
        public void Destory() {
            Parent.List.Remove(this);
            Items.ForEach(t => t.Parent = Parent);
            Parent.Items.AddRange(Items);
            Parent = null;
            Items.Clear();
        }
        public I仓库元素 AddItem(uint ID, Number 数量) {
            var A = new 仓库元素 { 设定 = I仓库元素设定.AllSettings[ID], Count = 数量, Parent = this };
            Items.Add(A);
            return A;
        }
        public void RemoveItem(I仓库元素 X) {
            Items.Remove(X);
            X.Parent = null;
        }
    }
    public class 仓库元素 : I仓库元素 {
        public I仓库数据 Parent { get; set; }
        public I仓库元素设定 设定 { get; set; }
        public Number Count { get; set; }
        public void FromArchiveString(string X) {
            设定 = I仓库元素设定.AllSettings[X.Split(":")[0].ToUint()];
            Count = X.Split(":")[1].ToNumber();
        }
        public string ToArchiveString() {
            return $"{设定.ID}:{Count}";
        }
    }
    public class 仓库元素设定 : I仓库元素设定 {
        public uint ID { get; set; }
        public string Name { get; set; }
        public string[] 目录 { get; set; }
        public Number 估值 { get; set; }
        public Number 体积 { get; set; }
        public Number 重量 { get; set; }
        public Sprite Sprite { get; set; }
    }
    public static class 仓库驱动Demo {
        public static 仓库数据 仓库数据唯一;
        public static void Main() {
            //Number.模式 = NumberMode.千兆规范 | NumberMode.移除超三位有效;
            Number.模式 = NumberMode.万亿规范 | NumberMode.移除超四位有效;
            var A = ReadExcel("仓库设定表", t => { //ID 目录 名称 图标 体积 重量
                return new 仓库元素设定 {
                    ID = t[0].Remove(".").ToUint(),
                    目录 = t[1].Split("/"),
                    Name = t[2],
                    Sprite = AllSprite[t[3]],
                    体积 = t[4].ToNumber(),
                    重量 = t[5].ToNumber(),
                };
            });
            foreach (var i in A) {
                I仓库元素设定.AllSettings.Add(i.ID, i);//报错则意味着存在重复ID
            }
            MainPanel.LoadPrefab("仓库").GetComponent<仓库驱动>().SetData(Load());
            OnAppQuit(Save);
        }
        public static I仓库数据 Load() {
            var B = new 仓库数据();//不能带名字
            var C = B.Create("根仓库");
            C.体积 = 100_0000_0000;
            执行X次(10, i => C.AddItem(I仓库元素设定.AllSettings.Keys.Choice(), RandomLog(1, 10000_0000)));
            执行X次(10, i => {
                var D = C.Create($"子仓库{i}");
                执行X次(10, i => D.AddItem(I仓库元素设定.AllSettings.Keys.Choice(), RandomLog(1, 10000_0000)));
            });
            return 仓库数据唯一 = TryFileRead("仓库存档.json", B);
        }
        public static void Save() {
            FileWrite("仓库存档.json", 仓库数据唯一);
        }
    }
}