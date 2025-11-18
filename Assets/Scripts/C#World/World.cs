using System;
using UnityEngine;//Mono
using static CMKZ.LocalStorage;

namespace CMKZ.风神界 {
    public class 世界类后台 {
        public Number 宽度 = 1000;
        public Number 高度 = 1000;
        public Grid<地块类后台> 所有地块;
        public 玩家类后台 玩家 = new();
        public List<野怪类后台> 所有野怪 = new();
        public 世界类后台 Init() {
            所有地块 = new Grid<地块类后台>(宽度, 高度);
            所有地块.Fill((i, j) => {
                var A = new 地块类后台();
                A.土质层 = new 土质类 {  土质= 土质类.所有土质.草地地块};
                A.空间组件.世界 = this;
                A.空间组件.世界坐标 = new Vector2(i, j);
                return A;
            });
            for (int i = 0; i < (宽度 * 高度).开根(); i++) { //一百区域，每个区域大约十个树木
                所有地块.SetRandomArea(10, t => t.建筑层 = new 树木类后台());
            }
            for (int i = 0; i < 100; i++) {
                var A = new 野怪类后台();
                A.空间组件.世界 = this;
                A.空间组件.世界坐标 = new Vector2(Random(0, 所有地块.Width), Random(0, 所有地块.Height));
                A.ResetTarget();
                所有野怪.Add(A);
            }
            return this;
        }
    }
    public class 地块类后台 {
        public I建筑 建筑层;
        public 土质类 土质层;
        public 空间组件类 空间组件 = new();
    }
    public class 土质类 {
        public enum 所有土质 {
            草地地块,
        }
        public 所有土质 土质;
    }
    public interface I建筑 : IEntity { }
    public class 树木类后台 : I建筑 {
        public string 图片名 { get; set; } = "树木";
        public Number 血量 { get; set; } = 2;
        public event Action<object> OnBeAttacked;
        public event Action<object> OnDie;
        public void BeAttacked(I伤害发起者 X, int Y) {
            血量 -= Y;
            if (血量 <= 0) {
                Die(X);
            }
            OnBeAttacked?.Invoke((X,Y));
        }
        public void Die(I伤害发起者 X) {
            OnDie?.Invoke(X);
        }
    }
    public class 空间组件类 {
        public const float 比例 = 64 / 100f;//素材图片64像素
        public 世界类后台 世界;
        public Vector2 世界坐标;
        public Vector2 坐标偏移 => new Vector2(世界.所有地块.Width, 世界.所有地块.Height) / 2;
        public Vector2 Unity坐标 => new((世界坐标.x - 坐标偏移.x) * 比例, (世界坐标.y - 坐标偏移.y) * 比例);
    }
    public interface I生物:IEntity {

    }
    public class 野怪类后台 : I生物 {
        public string 图片名 { get; set; } = "Error";
        public Number 血量 { get; set; } = 3;
        public float Speed = 2;
        public Vector2 目标坐标;
        public 空间组件类 空间组件 = new();
        public event Action<object> OnBeAttacked;
        public event Action<object> OnDie;
        public void FixedUpdate() {
            if (Vector2.Distance(目标坐标, 空间组件.世界坐标) < 0.1f) {
                ResetTarget();
            } else {
                空间组件.世界坐标 = Vector2.MoveTowards(空间组件.世界坐标, 目标坐标, Speed * Time.deltaTime);
            }
        }
        public void BeAttacked(I伤害发起者 X, int Y) {
            血量 -= Y;
            OnBeAttacked?.Invoke((X,Y));
            if (血量 <= 0) {
                Die(X);
            }
        }
        public void Die(I伤害发起者 X) {
            if (X is 玩家类后台 Y) {
                Y.获得经验(1);
            }
            OnDie?.Invoke(X);
        }
        public 野怪类后台 ResetTarget() {
            目标坐标 = 空间组件.世界坐标 + new Vector2(Random(-5, 5), Random(-5, 5));
            目标坐标.x = Mathf.Clamp(目标坐标.x, 0, 空间组件.世界.所有地块.Width);
            目标坐标.y = Mathf.Clamp(目标坐标.y, 0, 空间组件.世界.所有地块.Height);
            return this;
        }
    }
    public class 玩家类后台 : I生物, I伤害发起者 {
        public string 图片名 { get; set; } = "Error";
        public Number 血量 { get; set; } = 3;
        public Dictionary<KeyCode, Action> 按住快捷键 = new();
        public float Speed;
        public const float Z层级 = -10;//玩家-10，摄像机-11，地图0，矿石-1，跳币-2
        public Number 攻击力 = 1;
        public Number 经验;
        public 空间组件类 空间组件 = new();
        public event Action<object> On获得经验;
        public event Action<object> OnBeAttacked;
        public event Action<object> OnDie;
        public void Start() {
            按住快捷键[KeyCode.W] += () => 空间组件.世界坐标 += new Vector2(0, Speed * Time.deltaTime);
            按住快捷键[KeyCode.S] += () => 空间组件.世界坐标 += new Vector2(0, -Speed * Time.deltaTime);
            按住快捷键[KeyCode.A] += () => 空间组件.世界坐标 += new Vector2(-Speed * Time.deltaTime, 0);
            按住快捷键[KeyCode.D] += () => 空间组件.世界坐标 += new Vector2(Speed * Time.deltaTime, 0);
        }
        public void FixedUpdate() {
            Speed = Input.GetKey(KeyCode.LeftShift) ? 10 : 5;
            foreach (var i in 按住快捷键) {
                if (Input.GetKey(i.Key)) {
                    i.Value();
                }
            }
        }
        public void BeAttacked(I伤害发起者 X, int Y) {
            血量 -= Y;
            OnBeAttacked?.Invoke((X, Y));
            if (血量 <= 0) {
                Die(X);
            }
        }
        public void Die(I伤害发起者 X) {
            OnDie?.Invoke(X);
        }
        public void 获得经验(Number X) {
            经验 += X;
            if (经验 >= 2) {
                经验 -= 2;
                攻击力 += 1;
                On获得经验?.Invoke(X);
            }
        }
    }
}