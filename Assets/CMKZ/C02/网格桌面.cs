//using System;//Action
//using UnityEngine;//Mono
//using static CMKZ.LocalStorage;

//namespace CMKZ {
//    public static partial class LocalStorage {
//        public static 网格桌面<T> 创建网格桌面<T>(this GameObject 背景, string 位置大小,Color 背景颜色, List<T> 卡片数据) {
//            var 网格桌面 = 背景.创建矩形(位置大小).设置为网格背景<T>(背景颜色);
//            网格桌面.设置数据映射(卡片数据);
//            网格桌面.设置网格样式();
//            return 网格桌面;
//        }
//        public static 网格桌面<T> 创建网格桌面<T>(this GameObject 背景, Color 背景颜色, string 位置大小) {
//            var 网格桌面 = 背景.创建矩形(位置大小).设置为网格背景<T>(背景颜色);
//            网格桌面.设置网格样式();
//            return 网格桌面;
//        }
//        public static void SetColors<T>(this 网格桌面<T> X, Colors CLs) {
//            X.卡片颜色 = CLs;
//        }
//        public static 网格桌面<T> 设置为网格背景<T>(this GameObject 背景, Color 背景颜色) {
//            var 卡片背景 = new 网格桌面<T>();
//            卡片背景.背景 = 背景;
//            卡片背景.桌面 = 背景.创建矩形(new PanelConfig() {
//                矩形模式 = 矩形模式.滚动条自动高度网格布局,
//                Position = $"0 0 100% 100%",
//                ImageColor = 背景颜色,
//                GridSpacing = new((float)仓库项目间距, (float)仓库项目间距),
//                GridSize = 仓库项目大小
//            });
//            return 卡片背景;
//        }
//        public static void 设置数据映射<T>(this 网格桌面<T> 背景, List<T> 数据) {
//            背景.Data = 数据;
//        }
//        public static void 设置网格样式<T>(this 网格桌面<T> 背景) {
//            背景.对象池.模版 = MainHidding.加载预制体("仓库卡牌");
//        }
//        public static 网格桌面<T> 网格创建时<T>(this 网格桌面<T> 网格桌面, Action<T, GameObject,int> A) {
//            网格桌面.创建时 += A;
//            return 网格桌面;
//        }
//    }
//    public class 网格桌面<T> {
//        public GameObject 背景;
//        public List<T> Data;
//        public Dictionary<T, GameObject> 卡片缓存 = new();
//        public event Action<T, GameObject,int> 创建时;
//        public Colors 卡片颜色 = 按钮四分之一黑;

//        public GameObject 桌面;
//        public 对象池管理器 对象池 = new();
//        public void 创建所有卡片() {
//            Data.ForEach(t => {
//                创建卡片(t);
//            });
//        }
//        public void 销毁所有卡片() {
//            卡片缓存.ForEach(t => 对象池.回收(t.Value));
//            卡片缓存.Clear();
//        }
//        //public GameObject 创建卡片(T X) {
//        //    return 创建卡片(X, new(0, 0));
//        //}
//        public GameObject 创建卡片(T 数据) {
//            var 卡片背景 = 对象池.取出(桌面);
//            卡片背景.SetColors(卡片颜色);
//            卡片背景.SetScrollItem();
//            卡片缓存[数据] = 卡片背景;
//            创建时?.Invoke(数据, 卡片背景, 桌面.transform.childCount-1);
//            return 卡片背景;
//        }
//        public void 销毁卡片(T X) {
//            对象池.回收(卡片缓存[X]);
//            卡片缓存.RemoveKey(X);
//        }
//        public void 销毁卡片(int X) {
//            销毁卡片(Data[X]);
//        }
//        public void 刷新() {
//            销毁所有卡片();
//            创建所有卡片();
//        }
//        public Dictionary<T, Vector2> 获得UI位置数据() {
//            var A = new Dictionary<T, Vector2>();
//            卡片缓存.ForEach(t => {
//                var 位置大小 = t.Value.获取相对位置大小(桌面).Split(" ");
//                Vector2 Pos = new(位置大小[0].ToFloat(), 位置大小[1].ToFloat());
//                A.Add(t.Key, Pos);
//            });
//            return A;
//        }
//        public void 加载(List<T> 原数据) {
//            this.设置数据映射(原数据);
//            刷新();
//        }
//    }
//}