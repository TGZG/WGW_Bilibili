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
    //public static partial class LocalStorage {
    //    public static Dictionary<Type, Stack> AllPool = new();
    //    public static void Get<T>(this IPoolable<T> X,GameObject Y) where T:MonoBehaviour {
    //        return AllPool[typeof(T)].Count > 0 ? AllPool[typeof(T)].Pop().Cast<T>().GetComponent<IPoolable<T>>().OnGet() : Instantiate(仓库.目录模板.gameObject, 下级).GetComponent<目录驱动>().OnGet();
    //    }
    //}
    public class 检查类 {
        public Action 检查;
        public Action 动作;
    }
    public class 芥子驱动 : MonoBehaviour, IPoolable<芥子驱动> {
        public 仓库驱动 仓库;
        public TextMeshProUGUI 名称;
        public TextMeshProUGUI 数量;
        public Image 图标;
        public UIMenu2 Menu;
        public UIColors Colors;
        public UIExplane2 注释;
        public I仓库元素 Data;
        public event Func<I仓库元素, bool> On芥子销毁;
        public event Func<I仓库元素, I仓库数据, bool> On芥子转移;
        public 检查类 删除检查;
        public 芥子驱动 OnGet() {
            名称.text = "OnGet";
            数量.text = "-1";
            图标.sprite = null;
            gameObject.SetActive(true);
            return this;
        }
        public 芥子驱动 OnRelease() {
            名称.text = "OnRelease";
            数量.text = "-1";
            图标.sprite = null;
            Menu.Clear();
            Colors.OnRelease();
            return this;
        }
        public void SetData(I仓库元素 X) {
            名称.text = X.Name;
            数量.text = X.Count.ToString();
            图标.sprite = X.Sprite;
            注释.SetExplane($"{X.Name}×{X.Count}\n{X.体积 * X.Count} 方");
            Menu.SetAction("排序", 排序);
            Menu.SetAction("合并", 合并);
            Menu.SetAction("拆分", 拆分);
            Menu.SetAction("查看", 查看);
            Menu.SetAction("出售", 出售);
            Menu.SetAction("丢弃", 丢弃);
            Menu.SetAction("发送", 发送);
            LayoutRebuilder.ForceRebuildLayoutImmediate(数量.GetParentRectTransform());
        }
        public void 释放() {
            gameObject.SetParent(MainHidding);
            OnRelease();
            仓库.芥子池.Push(gameObject);
        }
        public 芥子驱动 生成() {
            return 仓库.芥子池.Count > 0 ? 仓库.芥子池.Pop().SetParent(仓库.内容网格.gameObject).GetComponent<芥子驱动>().OnGet() : Instantiate(仓库.芥子模板.gameObject, 仓库.内容网格).GetComponent<芥子驱动>().OnGet();
        }
        //Menu
        public void 排序() {
            Data.Parent.Items = Data.Parent.Items.OrderBy(t => t.设定.目录权重).ToList();
            仓库.Refresh();
        }
        public void 合并() {
            var A = Data.Parent.Items;
            A.GroupBy(t => t.ID).Select(t => (t.Key, t.Sum(x => x.Count))).ForEach(t => {
                var B = A.FirstOrDefault(x => x.ID == t.Key);
                if (B != null) {
                    B.Count = t.Item2;
                    A.RemoveAll(x => x.ID == t.Key && x != B);
                } else {
                    PrintWarning($"意外错误：合并时在仓库 {Data.Parent.Name} 中找不到{t.Key}-{t.Key.GetName()}");
                }
            });
            仓库.Refresh();
        }
        public void 拆分() {
            AlertNumberInput("输入分离数量", t => {
                if (t > Data.Count) {
                    Alert("数量不足");
                } else {
                    Data.Count -= t;
                    Data.Parent.AddItem(Data.设定.ID, t);
                    仓库.Refresh();
                }
            });
        }
        public void 查看() {
            Alert("XX");
        }
        public void 出售() {
        }
        public void 丢弃() {
            Data.Parent.RemoveItem(Data);
            仓库.Refresh();
        }
        public void 发送() {
            仓库.RootList.GetAllList().AlertChoose((节点, 数据) => 节点.SetText(数据), t => {
                仓库.当前.RemoveItem(Data);
                //仓库.当前.RootParent.GetByPath(t).AddItem(Data.设定.ID, Data.Count);//这不等于下面的？下面的似乎是根的父
                仓库.RootList.GetByPath(t).AddItem(Data.设定.ID, Data.Count);
                仓库.Refresh();
            });
        }
        public void 新增右键(int X,string Y,Action Z) {

        }
    }
}