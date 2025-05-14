using System.Timers;//Timer
using static CMKZ.LocalStorage;
using System;
using Timer = System.Timers.Timer;

namespace CMKZ {
    public interface I容器<T> {
        public T Value { get; set; }
    }
    /// <summary>
    /// 1.本类是对T的容器（箱子）
    /// 2.本类是一个缓存箱子。即，当装箱后，X秒后自动清空箱子。
    /// 适用使用：数据会变，查询一次数据成本高，需要短时间多次查询某一个数据，但相信这个数据在短时间内不会改变。
    /// 3.构造函数的参数函数中，必须使用Value的Set。也只有在这里才允许使用Value的Set。
    /// 4.获取数据时不要直接访问Value，而是使用StartGet_CallBack，并在其中传入获取到数据后的处理数据的办法。
    /// </summary>
    public class 缓存<T> : I容器<T> {
        private T _Value;
        public T Value {
            get => _Value;
            set {
                _Value = value;
                TimerClear?.Stop();
                TimerClear = new Timer(缓存秒数 * 1000); // 将秒数转换为毫秒
                TimerClear.Elapsed += (sender, e) => {
                    _Value = default;
                    TimerClear?.Stop();
                    TimerClear = null;
                };
                TimerClear.AutoReset = false; // 只触发一次
                TimerClear.Start();
                OnNextValueSet?.Invoke(value);
                OnNextValueSet = null;
                TimerWarning?.Stop();
                TimerWarning = null;
            }
        }
        public Timer TimerClear;
        public Timer TimerWarning;
        public double 缓存秒数;
        public Action<缓存<T>> OnValueTrySet;
        public Action<T> OnNextValueSet;
        public 缓存(double X, Action<缓存<T>> Y) {
            缓存秒数 = X;
            OnValueTrySet = Y;
        }
        /// <summary>
        /// 如果当前Value有效，那么立刻执行
        /// 如果当前Value无效，并且正在获取中，那么等到有效时执行
        /// 如果当前Value无效，并且没有在获取，那么获取，并等到有效时执行
        /// </summary>
        public void StartGet_CallBack(Action<T> X) {
            if (!_Value.Equals(default(T))) {
                X(_Value);
                return;
            }
            if (TimerWarning != null) {
                OnNextValueSet += X;
                return;
            }
            TimerWarning = new Timer(10 * 1000);
            TimerWarning.Elapsed += (sender, e) => {
                PrintWarning("超时");
                TimerWarning.Stop();
                TimerWarning = null;
            };
            TimerWarning.Start();
        }
        public static void Demo() {
            var A = new 缓存<int>(1, t => {
                t.Value = 1;
                Print($"Init:{t.Value}");
            });
            执行X次(3, () => {
                A.StartGet_CallBack(t => Print($"CallBack:{t}"));
            });
        }
    }
}