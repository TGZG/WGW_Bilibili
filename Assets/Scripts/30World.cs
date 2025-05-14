using UnityEngine;//Mono
using static CMKZ.LocalStorage;

namespace CMKZ.风神界 {
    public class 世界类 {
        public Grid<地块类> 所有地块 = new(100, 100);
        public Player 玩家;
        public List<野怪类> 所有野怪 = new();
        public 世界类 Init() {
            所有地块.Fill((i, j) => {
                var A = new GameObject($"地块{i}_{j}").AddComponent<地块类>();
                A.土质层 = $"草地地块";
                A.空间组件.世界 = this;
                A.空间组件.世界坐标 = new Vector2(i, j);
                return A;
            });
            for (int i = 0; i < 100; i++) { //一百区域，每个区域大约十个树木
                所有地块.SetRandomArea(10, t => t.建筑层 = $"树木");
            }
            玩家 = new GameObject($"玩家").AddComponent<Player>();
            for (int i = 0; i < 10; i++) {
                var A = new GameObject($"野怪{i}").AddComponent<野怪类>();
                A.空间组件.世界 = this;
                A.空间组件.世界坐标 = new Vector2(Random(0, 所有地块.Width), Random(0, 所有地块.Height));
                A.ResetTarget();
                所有野怪.Add(A);
            }
            return this;
        }
    }
    public class 空间组件类 {
        public const float 比例 = 64 / 100f;//素材图片64像素
        public 世界类 世界;
        public Vector2 世界坐标;
        public Vector2 坐标偏移 => new Vector2(世界.所有地块.Width, 世界.所有地块.Height) / 2;
        public Vector2 Unity坐标 => new((世界坐标.x - 坐标偏移.x) * 比例, (世界坐标.y - 坐标偏移.y) * 比例);
    }
}
