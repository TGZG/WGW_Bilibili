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
        public static List<string> GetAllList(this I仓库数据 X) {
            var A = new List<string>();
            A.Add(X.Name);
            A.AddRange(X.List.SelectMany(t => t.GetAllList().Select(x => $"{X.Name}/{x}".Trim('/'))));
            return A;
        }
        /// <summary>
        /// 从下级开始寻找。不要带本体名字
        /// </summary>
        public static I仓库数据 GetByPath(this I仓库数据 X, string Y) {
            var A = Y.Split('/');
            var B = X;
            for (int i = 0; i < A.Length; i++) {
                if (B == null) throw new Exception($"路径错误。基物体：{X.Name}。原路径：{Y}");
                if (A[i] == "..") {
                    B = B.Parent;
                } else {
                    B = B.List.First(t => t.Name == A[i], new Exception($"路径错误。{B.Name} 下不存在 {A[i]}"));
                }
            }
            return B;
        }
        public static void MoveTo(this I仓库数据 X, I仓库数据 Y) {
            if (X.Parent == null) throw new Exception("根目录不可移动");
            if (X.IsAFather(Y)) throw new Exception("不可移动到自己的子目录");
            if (X.RootParent != Y.RootParent) throw new Exception("非同根目录不可移动");
            X.Parent.List.Remove(X);
            Y.List.Add(X);
            X.Parent = Y;
        }
        public static bool IsAFather(this I仓库数据 X, I仓库数据 Y) {
            if (X == Y) return true;
            if (Y.Parent == null) return false;
            return IsAFather(X, Y.Parent);
        }
        public static string GetName(this uint X) {
            return I仓库元素设定.AllSettings[X].Name;
        }
    }
    public interface I仓库数据 {
        public I仓库数据 RootParent => (Parent == null || Parent.Name.IsNullOrEmpty()) ? this : Parent.RootParent;
        public I仓库数据 Parent { get; set; }
        public string Name { get; set; }
        public List<I仓库数据> List { get; }
        public bool IsLast { get; }
        public List<I仓库元素> Items { get; set; }
        public Number 下级体积 => Items.Sum(t => t.体积 * t.Count) + List.Sum(t => t.下级体积);
        public bool IsExpand { get; set; }
        public Number 体积 { get; set; }
        public I仓库数据 Create(string X);
        public I仓库元素 AddItem(uint ID, Number 数量);
        public void RemoveItem(I仓库元素 X);
        public void Destory();
    }
    public interface I仓库元素 {
        public I仓库数据 RootParent => Parent.RootParent;
        public I仓库数据 Parent { get; set; }
        public I仓库元素设定 设定 { get; }
        public Number Count { get; set; }
        public uint ID => 设定.ID;
        public string Name => 设定.Name;
        public Number 估值 => 设定.估值;
        public Number 体积 => 设定.体积;
        public Sprite Sprite => 设定.Sprite;
        public string ToArchiveString();
        public void FromArchiveString(string X);
    }
    public interface I仓库元素设定 {
        public static Dictionary<uint, I仓库元素设定> AllSettings = new();
        public uint ID { get; }
        public string Name { get; }
        public string[] 目录 { get; }
        public int 目录权重 => 目录.Turn(t => t.Hash()).组合();
        public Number 估值 { get; }
        public Number 体积 { get; }
        public Sprite Sprite { get; }
    }
    public class 仓库驱动 : MonoBehaviour {
        public RectTransform 目录列表;
        public RectTransform 内容网格;
        public 目录驱动 目录模板;
        public 芥子驱动 芥子模板;
        public TMP_InputField 搜索框;
        public TextMeshProUGUI 统计数据;
        public Image 体积条;
        public TextMeshProUGUI 体积文本;
        public I仓库数据 RootList;
        public I仓库数据 当前;
        public void Awake() {
            搜索框.onEndEdit.AddListener((X) => SetFilter(X));
            体积条.fillAmount = 0;
            体积文本.text = "";
            统计数据.text = "";
            目录模板.gameObject.SetParent(MainHidding);
            芥子模板.gameObject.SetParent(MainHidding);
            Print("仓库驱动初始化完成");
        }
        public 仓库驱动 SetData(I仓库数据 X) {
            RootList = X;
            foreach (var i in 目录列表.GetComponentsInChildren<目录驱动>()) {
                i.释放目录();
            }
            foreach (var i in X.List) {
                目录模板.生成目录(目录列表).SetData(i);
            }
            Print($"仓库设置目录数据 项目{X.List.Count}");
            return this;
        }
        public void Refresh() {
            SetData(RootList);
            Enter(当前);
        }
        public void Enter(I仓库数据 X) {
            当前 = X;
            foreach (var i in 内容网格.GetComponentsInChildren<芥子驱动>()) {
                i.释放();
            }
            foreach (var i in X.Items) {
                芥子模板.生成().SetData(i);
            }
            统计数据.text = $"芥子 {X.Items.Count}个    估值 {X.Items.Sum(t => t.估值)}金币";
            体积条.fillAmount = (float)X.RootParent.下级体积 / X.RootParent.体积;
            体积文本.text = $"{X.RootParent.下级体积}/{X.RootParent.体积}";
            Print($"进入仓库 {X.Name}");
        }
        public void SetFilter(string X) {
            foreach (var i in 内容网格.GetComponentsInChildren<芥子驱动>(true)) {
                i.gameObject.SetActive(i.名称.text.Contains(X));
            }
            Print($"仓库过滤 {X}");
        }
        public void SetExpand(I仓库数据 X, bool Y) {
            foreach (var i in 目录列表.GetComponentsInChildren<目录驱动>()) {
                if (i.Data == X) {
                    i.SetExpand(Y);
                    break;
                }
            }
        }
        public Stack<GameObject> 目录池 = new();
        public Stack<GameObject> 芥子池 = new();
    }
}