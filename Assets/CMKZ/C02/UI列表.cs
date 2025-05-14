using System;//Action
using TMPro;//InputField
using UnityEngine;//Mono
using UnityEngine.UI;//Image

namespace CMKZ {
    public static partial class LocalStorage {
        public static void 列表背景测试() {

        }
        public static UI列表<T> 创建UI列表<T>(this GameObject 背景, Color 背景颜色, string 位置大小, List<T> 卡片数据) {
            var 网格桌面 = 背景.创建矩形(位置大小).设置为UI列表<T>(背景颜色);
            网格桌面.设置数据映射(卡片数据);
            网格桌面.设置列表样式();
            return 网格桌面;
        }
        public static UI列表<T> 创建UI列表<T>(this GameObject 背景, Color 背景颜色, string 位置大小) {
            var UI列表 = 背景.创建矩形(位置大小).设置为UI列表<T>(背景颜色);
            UI列表.设置列表样式();
            return UI列表;
        }

        public static void SetColors<T>(this UI列表<T> X, Colors CLs) {
            X.项颜色 = CLs;
        }
        public static UI列表<T> 设置为UI列表<T>(this GameObject 背景, Color 背景颜色) {
            var 卡片背景 = new UI列表<T>();
            卡片背景.背景 = 背景;
            卡片背景.桌面 = 背景.创建矩形(new PanelConfig() {
                矩形模式 = 矩形模式.滚动条自动高度无文本,
                Position = $"{边距} {边距} 100%-{边距 * 2} 100%-{边距 * 2}",
                ImageColor = 背景颜色
            });
            return 卡片背景;
        }
        public static void 设置数据映射<T>(this UI列表<T> 背景, List<T> 数据) {
            背景.Data = 数据;
        }
        public static void 设置列表样式<T>(this UI列表<T> 背景, int 项高 = 30, int 文字大小 = 17) {
            背景.对象池.模版 = MainHidding.创建矩形(new PanelConfig() {
                矩形模式 = 矩形模式.固定无文本,
                Position = $"0 0 100% {项高}",
            });
            var 图片 = 背景.对象池.模版.创建矩形(new PanelConfig() {
                矩形模式 = 矩形模式.固定无文本,
                Position = $"0 0 0 0",
                ImageColor = new(255, 255, 255, 1f),
            }).SetName("图片").GetComponent<Image>().raycastTarget = false;//补丁。应当加入样式库
            var 文字 = 背景.对象池.模版.创建矩形(new PanelConfig() {
                矩形模式 = 矩形模式.自动宽度文本框,
                Position = $"0 0 0 100%",
                Font = 游戏字体,
                TextSize = 文字大小,
                TextAlign = TextAlignmentOptions.Center,
                Margin = new(10, 0, 0, 0),
                TextColor = 白色V4
            }).SetName("文字").SetText("").GetComponentInChildren<TextMeshProUGUI>().raycastTarget = false;//补丁。应当加入样式库
            var 右侧文字 = 背景.对象池.模版.创建矩形(new PanelConfig() {
                矩形模式 = 矩形模式.固定单行文本框,
                Position = $"0 0 100% 100%",
                Font = 游戏字体,
                TextSize = 文字大小,
                TextAlign = TextAlignmentOptions.Right,
                Margin = new(0, 0, 10, 0),
                TextColor = 白色V4
            }).SetName("右侧文字").SetText("").GetComponentInChildren<TextMeshProUGUI>().raycastTarget = false;//补丁。应当加入样式库
        }
        public static UI列表<T> 列表创建时<T>(this UI列表<T> UI列表, Action<T, GameObject, GameObject, GameObject,GameObject> A) {
            UI列表.创建时 += A;
            return UI列表;
        }
        public class UI列表<T> {
            public GameObject 背景;
            public List<T> Data;
            public Dictionary<T, GameObject> 项缓存 = new Dictionary<T, GameObject>();
            public event Action<T, GameObject, GameObject, GameObject,GameObject> 创建时;
            public Colors 项颜色 = 按钮四分之一黑;

            public GameObject 桌面;
            public 对象池管理器 对象池 = new();
            public void 创建所有项() {
                Data.ForEach(t => {
                    创建项(t);
                });
            }
            public void 销毁所有项() {
                项缓存.ForEach(t => 对象池.回收(t.Value));
                项缓存.Clear();
            }
            public GameObject 创建项(T 数据) {
                var 项背景 = 对象池.取出(桌面);
                项背景.SetColors(项颜色);
                var 图片物体 = 项背景.Find("图片");
                var 文字物体 = 项背景.Find("文字");
                var 右侧文字物体 = 项背景.Find("右侧文字");
                项缓存[数据] = 项背景;
                创建时?.Invoke(数据, 项背景, 图片物体, 文字物体, 右侧文字物体);
                return 项背景;
            }
            public void 销毁项(T X) {
                对象池.回收(项缓存[X]);
                项缓存.RemoveKey(X);
            }
            public void 销毁项(int X) {
                销毁项(Data[X]);
            }
            public void 刷新() {
                销毁所有项();
                创建所有项();
            }
            public void 加载(List<T> 原数据) {
                this.设置数据映射(原数据);
                刷新();
            }
            public GameObject 项(T X) { 
                return 项缓存[X];
            }
        }
    }
}


