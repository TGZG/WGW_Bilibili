using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static CMKZ.LocalStorage;

namespace CMKZ {
    public static partial class LocalStorage {
        public static void AlertInput(string 标题, Action<string> 确定事件) {
            AlertInput().SetTitle(标题).SetYes(确定事件);
        }
        public static void AlertNumberInput(string 标题, Action<double> 确定事件) {
            AlertInput().SetTitle(标题).SetYes(t => {
                if (double.TryParse(t, out double A)) {
                    确定事件(A);
                } else {
                    Alert("输入必须是数字");
                }
            });
        }
        public static void AlertCancel(string 标题, Action 确定事件, Action 取消事件 = null) {
            var A = MainPanel.创建矩形(默认Alert弹窗样式).SetName("XXX").AddComponent<UIAlertCancel>();
            A.标题 = 标题;
            A.OnEnter = 确定事件;
            A.OnExit = 取消事件;
            A.Start();
        }
    }
    public class UIAlertCancel : MonoBehaviour {
        public string 标题;
        public Action OnEnter;
        public Action OnExit;
        public void Start() {
            gameObject.创建矩形(UIAlert标题样式).SetText("警告").允许拖动(gameObject);
            gameObject.创建矩形(UIAlert正文样式).SetText(标题);
            gameObject.创建矩形(UIAlert确认按钮样式).SetText("确定").OnClick(t => {
                OnEnter?.Invoke();
                Destroy(gameObject);
            });
            gameObject.创建矩形(UIAlertInput取消按钮样式).SetText("取消").OnClick(t => {
                OnExit?.Invoke();
                Destroy(gameObject);
            });
        }
    }
    public static partial class LocalStorage {
        public static PanelConfig 默认Alert弹窗样式 = new() {
            矩形模式 = 矩形模式.固定无文本,
            Position = "50%-200 50%-100 400 200",
            ImageColor = new(255, 255, 255, 0.5f),
        };
        public static PanelConfig UIAlert标题样式 = new() {
            矩形模式 = 矩形模式.固定文本框,
            Position = "0 0 100% 30",
            Margin = new(10, 5, 10, 5),
        };
        public static PanelConfig UIAlert正文样式 = new() {
            矩形模式 = 矩形模式.固定文本框,
            Position = "10 30 100%-20 100%-60",
            Font = "基督山伯爵"
        };
        public static PanelConfig UIAlert确认按钮样式 = new() {
            矩形模式 = 矩形模式.固定文本框,
            Position = "10 100%-30 55 25",
            Colors = Colors.Default,
            TextAlign = TMPro.TextAlignmentOptions.Center,
            Font = "基督山伯爵"
        };
        public static PanelConfig UIAlertInput标题样式 = new() {
            矩形模式 = 矩形模式.固定文本框,
            Position = "10 5 100%-20 25",
        };
        public static PanelConfig UIAlertInput输入框样式 = new() {
            矩形模式 = 矩形模式.固定多行输入框,
            Position = "10 35 100%-20 100%-70",
            ImageColor = new Vector4(200, 200, 200, 0.1f),
        };
        public static PanelConfig UIAlertInput确认按钮样式 = new() {
            矩形模式 = 矩形模式.固定文本框,
            Position = "10 100%-30 60 25",
            Colors = Colors.Default,
            TextAlign = TMPro.TextAlignmentOptions.Center,
            Font = "基督山伯爵"
        };
        public static PanelConfig UIAlertInput取消按钮样式 = new() {
            矩形模式 = 矩形模式.固定文本框,
            Position = "80 100%-30 60 25",
            Colors = Colors.透明,
            TextAlign = TMPro.TextAlignmentOptions.Center,
            Font = "基督山伯爵"
        };
    }
}