using System;//Action
using TMPro;//InputField
using UnityEngine;//Mono
using UnityEngine.UI;//Image
using static CMKZ.LocalStorage;


namespace CMKZ {
    public static partial class LocalStorage {
        public static void 拖动排序List() {
            List<string> 数据 = new List<string>{
                "第一项",
                "第二项",
                 "第三项",
                "第四项",
                 "第五项",
                "第六项",
                 "第七项",
                "第八项",
            };

            var 排序列表 = MainPanel.创建拖动排序列表_默认<string>("100 100 200 500");
            排序列表.创建时((本项, 背景, 位置) => {
                背景.Find("标题").SetText(本项);
            });
            排序列表.加载(数据);
        }
        public static UI排序列表<T> 创建拖动排序列表_默认<T>(this GameObject Parent, string 位置大小) {
            var 排序列表 = Parent.创建拖动排序列表<T>(位置大小);
            排序列表.设置项高(30);
            排序列表.允许拖动排序();
            排序列表.默认模板();
            return 排序列表;
        }
        public static UI排序列表<T> 创建拖动排序列表<T>(this GameObject Parent, string 位置大小) {
            var A = new UI排序列表<T>();
            A.背景 = Parent.创建矩形(new PanelConfig() {
                矩形模式 = 矩形模式.滚动条自动高度无文本,
                Position = 位置大小,
                Spacing = 5,
                Padding = new(5, 5, 5, 5),
            });
            A.默认模板();
            return A;
        }
    }
    public class UI排序列表<T> {
        public GameObject 背景;
        public List<T> Data;
        public event Action<T, GameObject,int> _创建时;
        public event Action<T, int, int> _拖动结束时;
        public event Action<T, int> _拖动开始时;
        public 对象池管理器 对象池 = new();
        public KeyValueList<T, GameObject> 项缓存 = new();
        public int 项高 = 30;
        public UI排序列表<T> 加载(List<T> 数据) {
            Data = 数据;
            刷新();
            return this;
        }
        public void 刷新() {
            清除所有项();
            添加所有项();
            背景.强制刷新();
        }
        public void 清除所有项() {
            项缓存.ForEach(项 => {
                对象池.回收(项.Value);
            });
            项缓存.Clear();
        }
        public void 添加所有项() {
            int 当前执行 = 0;
            Data.ForEach(项 => {
                var 项背景 = 对象池.取出(背景);
                项背景.SetScrollItem();
                项背景.Find("标题").SetScrollItem();
                项背景.Find("标题").SetColor(0, 0, 1, 0);
                项背景.Find("标题").GetComponent<Image>().raycastTarget = false;
                项背景.Find("标题").GetComponentInChildren<TextMeshProUGUI>().raycastTarget = false;
                项背景.Find("右侧").SetScrollItem();
                _创建时.Invoke(项, 项背景,当前执行);
                当前执行++;
                项缓存.Add(项, 项背景);
            });
        }
        public void 默认模板() {
            对象池.模版 = MainHidding.创建矩形(new PanelConfig() {
                矩形模式 = 矩形模式.固定无文本,
                Position = $"0 0 100% {项高}",
            });
            var 标题=对象池.模版.创建矩形(new PanelConfig() {
                矩形模式 = 矩形模式.自动宽度文本框,
                Position = "0 0 100% 100%",
                Font = 游戏字体,
                TextAlign = TextAlignmentOptions.Left,
                Margin = new(5, 0, 0, 0),
            }).SetName("标题");
            var 右侧=对象池.模版.创建矩形(new PanelConfig() {
                矩形模式 = 矩形模式.固定文本框,
                Position = "100%-30 0 30 100%",
                Font = 游戏字体,
                TextAlign = TextAlignmentOptions.Right,
                Margin = new(5, 0, 5, 0),
            }).SetName("右侧");
        }
        public void 设置模板(Func<GameObject> 模板) {
            对象池.模版 = 模板.Invoke();
        }
        public UI排序列表<T> 设置项高(int 高度) {
            项高 = 高度;
            return this;
        }
        public UI排序列表<T> 创建时(Action<T, GameObject,int> 创建) {
            _创建时 += 创建;
            return this;
        }
        public void 允许拖动排序() {
            int 位置缓存 = 0;
            _创建时 += (项, 项背景,位置) => {
                项背景.允许拖动();
                项背景.OnMouseUp(t => {
                    刷新();
                });
                项背景.OnDrag(t => {
                    位置缓存 = 获取项位置(项背景);
                    项背景.transform.SetSiblingIndex(位置缓存);

                }); 
                项背景.OnBeginDrag(t => {
                    _拖动开始时?.Invoke(项, Data.IndexOf(项));
                });
                项背景.OnEndDrag(t => {
                    var 起始位置 = Data.IndexOf(项);
                    Data.Remove(项);
                    Data.Insert(位置缓存, 项);
                    刷新();
                    _拖动结束时?.Invoke(项, 起始位置, 位置缓存);
                });
            };
            int 获取项位置(GameObject 项背景) {
                var 位置大小 = 项背景.获取相对位置大小(背景);
                var Y位置 = 位置大小.Split(" ")[1].ToInt();
                var 当前位置 = (int)(Y位置 / (项高+小边距));
                if (当前位置 >= Data.Count) return Data.Count - 1;
                if (当前位置 < 0) return 0;
                return 当前位置;
            }
        }
        public void 拖动结束时(Action<T, int, int> 行为) {
            _拖动结束时 += 行为;
        }
        public void 拖动开始时(Action<T, int> 行为) {
            _拖动开始时 += 行为;
        }
    }
}


