using Microsoft.CodeAnalysis;
using System.Collections.Generic;//List
using System.Linq;//from XX select XX
using TMPro;//InputField
using UnityEngine;//Mono
using UnityEngine.UI;//Image
using static CMKZ.LocalStorage;

namespace CMKZ {
    public interface I滚动消息插件 {
        void 添加滚动(string X);
    }
    [RequireComponent(typeof(Mask))]
    public class 滚动消息插件 : MonoBehaviour, I滚动消息插件 {
        public List<string> 槽位 = new();
        public double 保留时间 = 3;
        public double 当前速度 = 0.2;
        public double 当前时间;
        public int 槽位数量 => 6;
        public Queue<string> 消息队列 = new();
        public KeyValueList<GameObject, double> 当前显示 = new();
        public void Awake() {
            var 槽位高度= 100d / 槽位数量;
            槽位.Add($"0 -{槽位高度.两位小数()}% 100% {槽位高度.两位小数()}%");
            for (int i = 0; i < 槽位数量; i++) {
                槽位.Add($"0 {i * 槽位高度}% 100% {槽位高度.两位小数()}%");
            }
            槽位.Add($"0 100% 100% {槽位高度.两位小数()}%");
            //Print(槽位.Count());
        }
        public void Update() {
            当前时间 += Time.deltaTime;
            if (当前速度 == 0.2 && 消息队列.Count > 20) {
                当前速度 = 0.05;
            }
            if (当前速度 == 0.05 && 消息队列.Count == 0) {
                当前速度 = 0.2;
            }
            foreach (var i in 当前显示) {
                i.Value += Time.deltaTime;
            }
            if (当前时间 >= 当前速度) {
                当前时间 = 0;
                OnTick();
            }
        }
        /// <summary>
        /// 每0.2秒：尝试插入一个，挤走上方多余的。
        /// 同时，如果有超时的，那么自动死亡
        /// </summary>
        public void OnTick() {
            foreach (var i in 当前显示.ToArray()) {
                if (i.Value >= 保留时间) {
                    var 物体 = i.Key;
                    i.Key.动画变化(当前速度, 槽位.First()).动画等待(0, () => 物体.Destroy());
                    当前显示.Remove(i);
                }
            }
            if (消息队列.Any() && 当前显示.Count(t => t.Value > 当前速度) <= 槽位数量) {
                塞入一项(消息队列.Dequeue());
            }
        }
        /// <summary>
        /// 对外接口
        /// </summary>
        public void 添加滚动(string X) {
            消息队列.Enqueue(X);
        }
        public void 塞入一项(string X) {
            var 消息物体 = gameObject.创建矩形(new PanelConfig() {
                矩形模式 = 矩形模式.固定文本框,
                Position = 槽位.Last(),
                Font = "基督山伯爵",
                TextSize = 20,
                TextAlign = TextAlignmentOptions.Right,
                Margin = new(10, 0, 10, 0),
                TextColor = 白色V4
            }).SetText(X).修正悬浮();
            升高一项();
            消息物体.动画变化(当前速度, 槽位[槽位数量]);
            当前显示.Add(消息物体, 0);
        }
        public void 升高一项() {
            //如果0（4）物体，那么将其坐标设置为3
            //如果01（34）物体，那么将其坐标设置为23
            //如果012（234）物体，那么将其坐标设置为123
            for (int i = 0; i < 当前显示.Count; i++) {
                当前显示[i].Key.动画变化(当前速度, 槽位[槽位数量 - 当前显示.Count + i]);
            }
            if (当前显示.Count == 槽位数量) {
                var 零位物体 = 当前显示[0];
                当前显示[0].Key.动画变化(当前速度, 槽位.First()).动画等待(0, () => 零位物体.Key.Destroy());
                当前显示.RemoveAt(0);
            }
        }
    }
}