using System;//Action
using UnityEngine;//Mono

namespace CMKZ {
    public static partial class LocalStorage {
        public static Action<IBuffController, IBuff> OnGlobalGetBuff;
        public static Action<IBuffController, IBuff> OnGlobalRemoveBuff;
        public static void AddBuff(this IBuffController X, IBuff Y) {
            X.BuffList.Add(Y);
            Y.Parent = X;
            Y.OnAwake();
            OnGlobalGetBuff?.Invoke(X, Y);
        }
        public static bool HaveBuff(this IBuffController X, IBuff Y) {
            return X.BuffList.Contains(Y);
        }
        public static bool HaveBuff<T>(this IBuffController X) where T : IBuff {
            return X.BuffList.Exists(t => t is T);
        }
        public static void RemoveBuff(this IBuffController X, IBuff Y) {
            Y.OnFinally();
            X.BuffList.Remove(Y);
            Y.Parent = null;
        }
        public static void RemoveBuff(this IBuffController X, string Y) {
            X.RemoveBuff(t => t.Name == Y);
        }
        public static void RemoveBuff<T>(this IBuffController X) where T : IBuff {
            X.RemoveBuff(t => t is T);
        }
        public static void RemoveBuff(this IBuffController X, Func<IBuff, bool> Y) {
            foreach (var i in X.BuffList.ToArray()) {
                if (Y(i)) {
                    X.RemoveBuff(i);
                }
            }
        }
        public static void UpdateBuffs(this IBuffController X,double 时间) {
            foreach (var i in X.BuffList.ToArray()) {
                i.Update(时间);
            }
        }
        public static void Cancel(IBuff X) {
            X.OnCancel();
            X.Parent.RemoveBuff(X);
        }
        public static void Update(this IBuff X,double 时间) {
            if (X.时间.当前 == 0) {
                X.OnStart();
            }
            X.OnUpdate(时间);
            X.时间.增加(时间);
            X.Timer.增加(时间);
            X.Second.增加(时间);
            if (X.Second.比例 == 1) {
                X.OnSecond();
                X.Second.比例 = 0;
            }
            if (X.Timer.比例 == 1) {
                X.OnTimer();
                X.Timer.比例 = 0;
            }
            if (X.时间.比例 == 1) {
                X.OnEnd();
                X.Parent.RemoveBuff(X);
            }
        }
    }
    public interface IBuffController {
        public List<IBuff> BuffList { get; }
    }
    public interface IBuff {
        public string Name { get; }
        public 限数 时间 { get; }
        public 限数 Timer { get; }
        public 限数 Second { get; }
        public double 层数 { get; }//合并叠加
        public double 强度 { get; }//合并取最大
        public IBuffController Parent { get; set; }
        public void OnAwake();//添加时
        public void OnStart();//第一次运行前
        public void OnUpdate(double 时间);//时刻
        public void OnEnd();//常规结束时
        public void OnCancel();//取消时
        public void OnFinally();//结束或取消时
        public void OnSecond();//激活时每秒执行
        public void OnTimer();//非秒计时器
    }

    public interface ITaskController {
        public List<TimeTask> TaskList { get; }
        public void AddTask(TimeTask X);
    }
    public class TimeTask {
        public 限数 时间;
        public Action OnAwake;//AddTask时
        public Action OnStart;//第一次Update之前
        public Action OnUpdate;
        public Action OnEnd;
        public Action OnCancel;
        public ITaskController Parent;
        public void Cancel() {
            OnCancel?.Invoke();
            Parent.TaskList.Remove(this);
        }
    }
}