using System;//Action

namespace CMKZ {
    public class 全域值 {
        public double[] 上限前五项 = new double[] { 0, 0, 0, 0, 0 };
        public double[] 当前前五项 = new double[] { 0, 0, 0, 0, 0 };
        public Dictionary<long, double> 上限 = new();//不包含前五项
        public Dictionary<long, double> 当前 = new();
        public 全域值() { }
        public 全域值(double X, double Y, double Z, double W, double V) {
            当前前五项 = new double[] { X, Y, Z, W, V };
            上限前五项 = new double[] { X, Y, Z, W, V };
        }
        public 全域值(double[] 数值) {
            if (数值.Length != 5) throw new Exception($"全域值数组长度应当为5！当前{数值}");
            //这里必须分开赋值，使用【当前前五项 = 上限前五项 = 数值;】会导致连锁引用赋值。
            当前前五项 = 数值;
            上限前五项 = 数值;
        }
        public double this[long X] {
            get {
                if (X < 5) {
                    return 上限前五项[X];
                } else {
                    return 上限[X];
                }
            }
            set {
                if (X < 5) {
                    上限前五项[X] = value;
                } else {
                    上限[X] = value;
                }
            }
        }
        public 全域值 With(long X,double Y) {
            this[X] = Y;
            return this;
        }
        public double 获取当前(long X) {
            return X < 5 ? 当前前五项[X] : 当前[X];
        }
        public double 获取装备五行(long X) => 获取当前(X);
        public double 获取角色五行(long X) => 获取当前(X);
        public double 获取角色五道(long X) => 获取当前(X);
        public double 获取五道上限(long X) => 获取原始(X);
        public string 角色五行() => 当前前五项.ToString(t => t.ToString());
        /// <summary>
        /// 格式例如：10/10 5/10 10/10 10/10 10/10
        /// 第一项是当前，第二项是上线
        /// </summary>
        public string 角色五道() => 当前前五项.ToString((i, t) => $"{t}/{上限前五项[i]}");
        public string 角色五变() => 当前前五项.ToString(t => t.ToString());
        public string 装备五行() => 当前前五项.ToString(t => t.ToString());
        public string 装备五道() => 当前前五项.ToString(t => t.ToString());
        public string 装备五变() => 当前前五项.ToString(t => t.ToString());
        public double 获取原始(long X) {
            return X < 5 ? 上限前五项[X] : 上限[X];
        }
        public void AddAllNow(全域值 X) {
            AddFiveNow(X);
            foreach (var i in X.当前) {
                当前[i.Key] += i.Value;
            }
        }
        public void AddFiveNow(全域值 X) {
            for (var i = 0; i < 5; i++) {
                当前前五项[i] += X.当前前五项[i];
            }
        }
        public 全域值 Multiply(全域值 X) {
            var A = new 全域值();
            foreach (var i in 当前) {
                if (X.当前.ContainsKey(i.Key)) {
                    A.当前[i.Key] = i.Value * X.当前[i.Key];
                }
            }
            for (var i = 0; i < 5; i++) {
                A.当前前五项[i] = 当前前五项[i] * X.当前前五项[i];
            }
            return A;
        }
        public 全域值 Clone() {
            return new 全域值 {
                上限前五项 = 上限前五项,
                当前前五项 = 当前前五项,
                上限 = 上限.Clone(),
                当前 = 当前.Clone()
            };
        }
        public long 溢出 {
            get {
                for (int i = 0; i < 5; i++) {
                    if (当前前五项[i] > 上限前五项[i]) {
                        return i;
                    }
                }
                foreach (var i in 当前) {
                    if (i.Value > 上限[i.Key]) {
                        return i.Key;
                    }
                }
                return -1;
            }
        }
        public 全域值 SetNowZero() {
            当前 = new();
            当前前五项 = new double[5];
            return this;
        }
        //重载大于号，定义为：当前值大于目标值
        public static bool operator >(全域值 X, 全域值 Y) {
            for (int i = 0; i < 5; i++) {
                if (X.当前前五项[i] < Y.当前前五项[i]) {
                    return false;
                }
            }
            foreach (var i in X.当前) {
                if (i.Value < Y.当前[i.Key]) {
                    return false;
                }
            }
            return true;
        }
        public static bool operator <(全域值 X, 全域值 Y) {
            for (int i = 0; i < 5; i++) {
                if (X.当前前五项[i] > Y.当前前五项[i]) {
                    return false;
                }
            }
            foreach (var i in X.当前) {
                if (i.Value > Y.当前[i.Key]) {
                    return false;
                }
            }
            return true;
        }
        public long 当前高于(全域值 Y) {
            for (int i = 0; i < 5; i++) {
                if (当前前五项[i] < Y.当前前五项[i]) {
                    return i;
                }
            }
            foreach (var i in 当前) {
                if (i.Value < Y.当前[i.Key]) {
                    return i.Key;
                }
            }
            return -1;
        }
        public override string ToString() {
            return 当前前五项.ToString(t => t.ToString());
        }
    }
}