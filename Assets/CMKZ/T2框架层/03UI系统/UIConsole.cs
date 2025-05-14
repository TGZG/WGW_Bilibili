using System;
using TMPro;//InputField
using UnityEngine;//Mono
using System.Linq;
using static UnityEngine.Object;//Destory
using static CMKZ.LocalStorage;
using UnityEngine.EventSystems;
using UnityEngine.UI;
//using CMKZ.战斗游戏91;

namespace CMKZ {
    public static partial class LocalStorage {
        public static UIConsole CreateConsole() {
            var A = Instantiate(AllPrefab["控制台"], MainPanel.transform);
            A.设置模糊(1);
            return A.GetComponent<UIConsole>();
        }
        public static List<LogData> 筛选消息(this List<LogData> 源列表, ConsoleFilter 筛选配置) {
            return 源列表.Where(t =>
                t.Level >= 筛选配置.最低错误等级
                && (筛选配置.屏蔽类型 == null || !筛选配置.屏蔽类型.Contains(t.Sender))
                && (筛选配置.保留关键字 == null || 筛选配置.保留关键字.All(t.Message.Contains))
            ).TakeLast(筛选配置.最多条数).ToList();
        }
    }
    public class UIConsole : MonoBehaviour {
        public TMP_InputField InputField;
        public TextMeshProUGUI Text;
        public TextMeshProUGUI 筛选文字;
        public UIClick 筛选按钮;
        public UIClick Button;
        public UIClick Close;
        public ScrollRect Scroll;
        private double 间距 = 7;
        public void Start() {
            Button.OnClick += () => {
                控制台.执行(InputField.text);
                InputField.text = "";
            };
            MainPanel.OnUpdate(() => {
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) {
                    if (EventSystem.current.currentSelectedGameObject == InputField.gameObject) {
                        InputField.text = InputField.text.Replace("\n", "");
                        Button.Invoke();
                        InputField.ActivateInputField();
                    }
                }
            });
        }
        public UIConsole Filter(ConsoleFilter 筛选配置) {
            var 待显示消息 = LogList.筛选消息(筛选配置);//筛选逻辑
            //添加时间戳
            if (筛选配置.时间戳格式 == TimeType.无) {
                Text.text = 待显示消息.Select(t => t.Message).Join("\n");
            } else if (筛选配置.时间戳格式 == TimeType.日期时间) {
                Text.text = 待显示消息.Select(t => $"{绿色str}[ {淡蓝色str}{t.Time:yyyy.MM.dd_HH:mm:ss}{默认色str} | {橙色str}{t.Sender}{默认色str} ]{默认色str}" +
                    $"\n{t.Message}").Join($"\n<size={间距}>\n</size>");
            } else {
                Text.text = 待显示消息.Select(t => $"{绿色str}[ {淡蓝色str}{t.Time:HH:mm:ss}{默认色str} | {橙色str}{t.Sender}{默认色str} ]{默认色str}" +
                    $"\n{t.Message}").Join($"\n<size={间距}>\n</size>");
            }
            gameObject.OnNextFrame(() => {
                执行X次(2, () => gameObject.ChangeActive());
                Scroll.滚动到最底部();
            });
           
            return this;
        }
        public UIConsole Clear() {
            Text.text = NowTime + " Clear\n";
            return this;
        }
        public UIConsole SetPosition(string X) {
            gameObject.SetPosition(X);
            return this;
        }
    }
    public class ConsoleFilter {
        public LogLevel 最低错误等级;
        public object[] 屏蔽类型;//任一存在
        public string[] 保留关键字;//同时存在
        public TimeType 时间戳格式;
        public int 最多条数;
    }
    public enum TimeType {
        无,
        时间,
        日期时间,
    }
}