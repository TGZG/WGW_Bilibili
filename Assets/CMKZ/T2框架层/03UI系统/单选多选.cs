using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using static UnityEngine.Object;
using static CMKZ.LocalStorage;

namespace CMKZ {
    public static partial class LocalStorage {
        public static void MultiSelect(this GameObject X, MultiSelect Y) {
            X.SetStringData("选择情况", "非选");
            X.OnClick(t => {
                if (X.GetStringData("选择情况") == "非选") {
                    X.调整矩形(Y.选择.常规);
                    X.SetStringData("选择情况", "选择");
                } else {
                    X.调整矩形(Y.非选.常规);
                    X.SetStringData("选择情况", "非选");
                }
            });
        }
        public static void SingleSelect(this GameObject X, SingleSelect Y) {
            X.SetStringData("选择情况", "非选");
            Y.组.Add(X);
            X.OnClick(t => {
                foreach (var i in Y.组) {
                    X.调整矩形(Y.非选.常规);
                    X.SetStringData("选择情况", "非选");
                }
                X.调整矩形(Y.选择.常规);
                X.SetStringData("选择情况", "选择");
            });
        }
        public static void SingleSelect(this GameObject X, SingleSelect Y, PanelConfig Z1, PanelConfig Z2) {
            X.SetStringData("选择情况", "非选");
            Y.组.Add(X);
            X.OnClick(t => {
                foreach (var i in Y.组) {
                    X.调整矩形(Z1.常规);
                    X.SetStringData("选择情况", "非选");
                }
                X.调整矩形(Z2.常规);
                X.SetStringData("选择情况", "选择");
            });
        }
    }
    public class MultiSelect {
        public PanelConfig 选择;
        public PanelConfig 非选;
        public MultiSelect(PanelConfig X, PanelConfig Y) {
            选择 = X;
            非选 = Y;
        }
    }
    public class SingleSelect {
        public PanelConfig 选择;
        public PanelConfig 非选;
        public Action 选择事件;
        public Action 非选事件;
        public List<GameObject> 组 = new();
        public SingleSelect() {

        }
        public SingleSelect(PanelConfig X, PanelConfig Y) {
            非选 = X;
            选择 = Y;
        }
    }
}