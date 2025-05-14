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
    public interface IRelease {
        public void OnRelease();
    }
    public interface IPoolable<T> {
        public T OnGet();
        public T OnRelease();
    }
    public class 目录驱动 : MonoBehaviour, IPoolable<目录驱动> {
        public 仓库驱动 仓库;
        public TextMeshProUGUI 名称;
        public Image 折叠图标;
        public RectTransform 下级;
        public UIClick 条目;
        public UIClick 折叠图标点击;
        public UIMenu2 Menu;
        public UIColors Colors;
        public I仓库数据 Data;
        public UIExplane2 注释;
        public 目录驱动 OnGet() {
            return this;
        }
        public 目录驱动 OnRelease() {
            Menu.Clear();
            折叠图标.gameObject.SetActive(true);
            条目.OnClick = null;
            折叠图标点击.OnClick = null;
            Data = null;
            Colors.OnRelease();
            return this;
        }
        public void SetData(I仓库数据 X) {
            foreach (var i in 下级.GetComponentsInChildren<目录驱动>()) {
                i.释放目录();
            }
            Data = X;
            名称.text = Data.Name;
            SetExpand(Data.IsExpand);
            注释.SetExplane($"体积 {X.RootParent.下级体积}/{X.RootParent.体积} 方\n芥子 {X.Items.Count} 个\n估值 {X.Items.Sum(t => t.估值)} 金币");

            Menu.SetAction("删除", 删除);
            Menu.SetAction("命名", 命名);
            Menu.SetAction("移动", 移动);
            Menu.SetAction("新建", 新建);
            if (Data.IsLast) {
                折叠图标.gameObject.SetActive(false);
            } else {
                折叠图标点击.OnClick += () => SetExpand(!Data.IsExpand);
            }
            条目.OnClick += () => 仓库.Enter(Data);
        }
        public void SetExpand(bool Y) {
            Data.IsExpand = Y;
            foreach (var i in 下级.GetComponentsInChildren<目录驱动>()) {
                i.释放目录();
            }
            if (Y) {
                foreach (var i in Data.List) {
                    生成目录().SetData(i);
                }
            }
            折叠图标.transform.rotation = Quaternion.Euler(0, 0, Data.IsExpand ? 0 : 90);
            Print($"仓库{(Y ? "展开" : "折叠")} {Data.Name}");
        }
        public void 释放目录() {
            gameObject.SetParent(MainHidding);
            OnRelease();
            仓库.目录池.Push(gameObject);
        }
        public 目录驱动 生成目录(RectTransform X = null) {
            X ??= 下级;
            return 仓库.目录池.Count > 0 ? 仓库.目录池.Pop().SetParent(X.gameObject).GetComponent<目录驱动>().OnGet() : Instantiate(仓库.目录模板.gameObject, X).GetComponent<目录驱动>().OnGet();
        }
        //Menu
        public void 删除() {
            Data.Destory();
            仓库.Refresh();
        }
        public void 命名() {
            AlertInput("请输入新名称", t => 名称.text = Data.Name = t);
        }
        public void 移动() {
            AlertChoose("标题", 仓库.RootList.GetAllList(), t => t, null, t => {
                Data.MoveTo(仓库.RootList.GetByPath(t));
                仓库.Refresh();
            });
        }
        public void 新建() {
            AlertInput("请输入新建的仓库名", t => {
                Data.Create(t);
                仓库.Refresh();
            });
        }
        public void 新增右键(int X, string Y, Action Z) {

        }
    }
}