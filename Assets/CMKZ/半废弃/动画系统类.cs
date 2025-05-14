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
        public static bool 开启动画 = true;
        /// <summary>
        /// var 矩形 = MainPanel.创建矩形("0 0 100 100").SetColor(255, 255, 255, 0.2f);<br/>
        /// 矩形.动画变化(1, "0 0 200 100")                                            <br/>
        ///     .动画等待(1)                                                           <br/>
        ///     .动画变化(1, "0 0 200 200")                                            <br/>
        ///     .动画变化(1, "0 0 100 100");                                           <br/>
        /// </summary>
        public static GameObject 动画变化(this GameObject X, double 时间, string 位置) {
            var 原始 = new double[8];
            double X轴数值变化 = 0;
            double X轴百分比变化 = 0;
            double Y轴数值变化 = 0;
            double Y轴百分比变化 = 0;
            double 宽度数值变化 = 0;
            double 宽度百分比变化 = 0;
            double 高度数值变化 = 0;
            double 高度百分比变化 = 0;
            void OnStart() {
                var P = 位置.Split(' ');
                var Q = X.GetComponent<MyTransform>();
                原始 = MyTransform.ParsePosition(X.获取相对位置大小(X.GetParent()));
                var 目标 = MyTransform.ParsePosition(位置);
                X轴数值变化 = 目标[0] - 原始[0];
                X轴百分比变化 = 目标[1] - 原始[1];
                Y轴数值变化 = 目标[2] - 原始[2];
                Y轴百分比变化 = 目标[3] - 原始[3];
                宽度数值变化 = 目标[4] - 原始[4];
                宽度百分比变化 = 目标[5] - 原始[5];
                高度数值变化 = 目标[6] - 原始[6];
                高度百分比变化 = 目标[7] - 原始[7];
            }
            void 移动动画(double time) {
                var 比例 = (float)(time / 时间);
                var 本帧所需X轴百分比变化 = X轴百分比变化 * 比例;
                var 本帧所需X轴数值变化 = X轴数值变化 * 比例;
                var 本帧所需Y轴百分比变化 = Y轴百分比变化 * 比例;
                var 本帧所需Y轴数值变化 = Y轴数值变化 * 比例;
                var 本帧所需宽度百分比变化 = 宽度百分比变化 * 比例;
                var 本帧所需宽度数值变化 = 宽度数值变化 * 比例;
                var 本帧所需高度百分比变化 = 高度百分比变化 * 比例;
                var 本帧所需高度数值变化 = 高度数值变化 * 比例;

                var 目标X位置数值 = 原始[0] + 本帧所需X轴数值变化;
                var 目标X位置比例 = 原始[1] + 本帧所需X轴百分比变化;
                var 目标Y位置数值 = 原始[2] + 本帧所需Y轴数值变化;
                var 目标Y位置比例 = 原始[3] + 本帧所需Y轴百分比变化;
                var 目标宽度数值 = 原始[4] + 本帧所需宽度数值变化;
                var 目标宽度比例 = 原始[5] + 本帧所需宽度百分比变化;
                var 目标高度数值 = 原始[6] + 本帧所需高度数值变化;
                var 目标高度比例 = 原始[7] + 本帧所需高度百分比变化;


                var 正负号1 = 原始[0] + 本帧所需X轴数值变化 < 0 ? "" : "+";
                var 正负号2 = 原始[2] + 本帧所需Y轴数值变化 < 0 ? "" : "+";
                var 正负号3 = 原始[4] + 本帧所需宽度数值变化 < 0 ? "" : "+";
                var 正负号4 = 原始[6] + 本帧所需高度数值变化 < 0 ? "" : "+";

                var X位置 = $"{目标X位置比例 * 100}%{正负号1}{目标X位置数值}";
                var Y位置 = $"{目标Y位置比例 * 100}%{正负号2}{目标Y位置数值}";
                var 宽度 = $"{目标宽度比例 * 100}%{正负号3}{目标宽度数值}";
                var 高度 = $"{目标高度比例 * 100}%{正负号4}{目标高度数值}";
                X.调整矩形($"{X位置} {Y位置} {宽度} {高度}");
                //计算所有值，如果所有值都达到或超过了目标值，返回true，并将所有值设置为目标值
                if (比例 >= 1) {
                    X.调整矩形(位置);
                }
            }
            X.GetOrAddComponent<动画任务插件>().AddTask(new(时间, OnStart, 移动动画, () => X.调整矩形(位置)));
            return X;
        }
        public static GameObject 动画变化(this GameObject X, double 时间, Action<double> 帧行动,Action 结束) {
            X.GetOrAddComponent<动画任务插件>().AddTask(new(1, null, 帧行动, 结束));
            return X;
        }
        public static GameObject 动画等待(this GameObject X, double 时间, Action 延迟事件 = null) {
            X.GetOrAddComponent<动画任务插件>().AddTask(new(时间, null, null, 延迟事件));
            return X;
        }
        public static GameObject 等待(double 时间, Action 延迟事件 = null) {
            MainPanel.GetOrAddComponent<动画任务插件>().AddTask(new(时间, null, null, 延迟事件));
            return MainPanel;
        }
        public static GameObject 等待(this GameObject X, double 时间, Action 延迟事件 = null) {
            X.GetOrAddComponent<动画任务插件>().AddTask(new(时间, null, null, 延迟事件));
            return X;
        }
    }
    public class 动画任务插件 : MonoBehaviour {
        private List<动画任务类> 任务列表 = new();
        public void AddTask(动画任务类 X) {
            if (!gameObject.activeSelf) {
                PrintWarning($"{gameObject.name} 未激活，不应当添加动画");
                return;
            }
            任务列表.Add(X);
        }
        public void Update() {
            if (任务列表.Count == 0) return;
            if (开启动画) {
                任务列表[0].OnStart?.Invoke();
                任务列表[0].OnStart = null;
                任务列表[0].OnUpdate?.Invoke(任务列表[0].时间);
                任务列表[0].时间.增加(Time.deltaTime);
                if (任务列表[0].时间.已满) {
                    任务列表[0].OnEnd?.Invoke();
                    任务列表.RemoveAt(0);
                }
            } else {
                任务列表[0].OnEnd?.Invoke();
                任务列表.RemoveAt(0);
            }
        }
    }
    public class 动画任务类 {
        public Action OnStart;
        public Action<double> OnUpdate;
        public Action OnEnd;
        public 限数 时间;
        public 动画任务类(double 时间, Action OnStart, Action<double> OnUpdate, Action OnEnd) {
            this.OnStart = OnStart;
            this.OnUpdate = OnUpdate;
            this.OnEnd = OnEnd;
            this.时间 = new(时间);
            this.时间.比例 = 0;
        }
    }
}