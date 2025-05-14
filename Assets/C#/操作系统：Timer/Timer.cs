using System;
using System.Collections.Generic;
using System.Linq;
using static CMKZ.LocalStorage;

namespace CMKZ {
    public static partial class LocalStorage {
        public static BaseWaiter WaitFor(Func<bool> X) => new WaitForWaiter(X);
        public static BaseWaiter SetTimeOut(float 秒数, Action Y) => new TimerWaiter(秒数).Then(Y);
        public static BaseWaiter SetLoop(float 秒数, Action Y) => new LoopWaiter(秒数).Then(Y);
        public static BaseWaiter SetLoop(float 秒数, float 次数, Action Y) {
            var A = 0;
            var B = new LoopWaiter(秒数);
            B.Then(() => {
                Y();
                A++;
                if (A >= 次数) {
                    B.End();
                }
            });
            return B;
        }
        public static BaseWaiter SetRandomLoop(float 秒数下限, float 秒数上限, Action Y) => new RandomLoopWaiter(秒数下限, 秒数上限).Then(Y);
        public static BaseWaiter SetUpdateLoop(Action X) => new UpdateLoopWaiter().Then(X);
    }
    public abstract class BaseWaiter {
        public long ID;
        public static long WaitForID = 0;
        public BaseWaiter() {
            ID = WaitForID++;
        }
        public abstract BaseWaiter Then(Action X);
        public BaseWaiter ExitOn(Func<bool> X) {
            OnAppUpdate("WaitForExitOn" + ID, t => {
                if (X()) {
                    End();
                }
            });
            return this;
        }
        public void End() {
            RemoveAppUpdate("WaitFor" + ID);
            RemoveAppUpdate("WaitForExitOn" + ID);
        }
    }
    public class WaitForWaiter : BaseWaiter {
        public Func<bool> WaiteFor;
        public WaitForWaiter(Func<bool> X) : base() {
            WaiteFor = X;
        }
        public override BaseWaiter Then(Action X) {
            OnAppUpdate("WaitFor" + ID, t => {
                if (WaiteFor()) {
                    X();
                    End();
                }
            });
            return this;
        }
    }
    public class UpdateLoopWaiter : BaseWaiter {
        public UpdateLoopWaiter() : base() { }
        public override BaseWaiter Then(Action X) {
            OnAppUpdate("WaitFor" + ID, t => {
                X();
            });
            return this;
        }
    }
    public class TimerWaiter : BaseWaiter {
        public float 秒数;
        public float 当前秒数;
        public TimerWaiter(float X) : base() {
            秒数 = X;
        }
        public override BaseWaiter Then(Action X) {
            OnAppUpdate("WaitFor" + ID, t => {
                当前秒数 += t;
                if (当前秒数 > 秒数) {
                    X();
                    End();
                }
            });
            return this;
        }
    }
    public class LoopWaiter : BaseWaiter {
        public float 秒数;
        public float 当前秒数;
        public LoopWaiter(float X) : base() {
            秒数 = X;
        }
        public override BaseWaiter Then(Action X) {
            OnAppUpdate("WaitFor" + ID, t => {
                当前秒数 += t;
                if (当前秒数 > 秒数) {
                    当前秒数 = 0;
                    X();
                }
            });
            return this;
        }
    }
    public class RandomLoopWaiter : BaseWaiter {
        public float 秒数下限;
        public float 秒数上限;
        public float 秒数实际;
        public float 当前秒数;
        public RandomLoopWaiter(float X, float Y) : base() {
            秒数下限 = X;
            秒数上限 = Y;
            秒数实际 = Random(X, Y);
        }
        public override BaseWaiter Then(Action X) {
            OnAppUpdate("WaitFor" + ID, t => {
                当前秒数 += t;
                if (当前秒数 > 秒数实际) {
                    当前秒数 = 0;
                    秒数实际 = Random(秒数下限, 秒数上限);
                    X();
                }
            });
            return this;
        }
    }
}