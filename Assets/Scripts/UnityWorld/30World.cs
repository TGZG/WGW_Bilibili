using UnityEngine;//Mono
using static CMKZ.LocalStorage;

namespace CMKZ.风神界 {
    public class 世界类前台 {
        public 世界类后台 后台;
        public Player 玩家;
        public List<野怪类> 所有野怪 = new();
        public Grid<地块类前台> 所有地块;
        public 世界类前台 Init(世界类后台 X) {
            后台 = X;
            玩家 = new GameObject($"玩家").AddComponent<Player>();
            for (int i = 0; i < 后台.所有野怪.Count; i++) {
                var A = new GameObject($"野怪{i}").AddComponent<野怪类>();
                A.后台 = 后台.所有野怪[i];
                所有野怪.Add(A);
            }


            所有地块.Fill((i, j) => {
                var A = new GameObject($"地块{i}_{j}").AddComponent<地块类前台>();
                A.土质层 = $"草地地块";
                A.空间组件.世界 = this;
                A.空间组件.世界坐标 = new Vector2(i, j);
                return A;
            });
            for (int i = 0; i < (宽度 * 高度).开根(); i++) { //一百区域，每个区域大约十个树木
                所有地块.SetRandomArea(10, t => t.建筑层 = $"树木");
            }
            

            return this;
        }
    }
    public class 地区类前台 {
        public const int 宽度 = 200;
        public const int 高度 = 100;
        public const int 卸载距离 = 400;
        public const int 加载距离 = 50;
        public Grid<地块类前台> 所有地块;
        public List<I生物> 所有生物;//会与外界重复
    }
}