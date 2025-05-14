using Microsoft.CodeAnalysis;
using System;//Action
using System.Linq;//from XX select XX
using UnityEngine;//Mono
using static CMKZ.LocalStorage;

namespace CMKZ {
    public static partial class LocalStorage {
        public static UI替换面板<T> 创建UI替换面板<T>(this GameObject X, string 位置大小, int 项宽度, int 项高度) {
            var 箭头宽度 = 70;
            var A = new UI替换面板<T>();
            A.背景 = X.创建矩形(位置大小).SetColor(0, 0, 0, 0.5f);
            A.箭头列表 = A.背景.创建矩形(new PanelConfig() {
                矩形模式 = 矩形模式.自动高度无文本,
                Position = $"100%-{箭头宽度} 0 {箭头宽度} 100%",
            });
            A.箭头池.模版 = MainHidding.创建矩形(new PanelConfig() {
                矩形模式 = 矩形模式.固定无文本,
                Position = $"0 0 100% {项高度}",
                //ImageColor = new(255, 0, 0, 0.5f),
                Image = "左箭头",
            });
            A.替换列表 = A.背景.创建拖动排序列表_默认<T>($"100%-{箭头宽度 + 项宽度 + 边距} 0 {项宽度} 100%");
            A.Init();
            return A;
        }
    }
    public class UI替换面板<T> {
        public GameObject 背景;
        public GameObject 箭头列表;
        public UI排序列表<T> 替换列表;
        public List<T> 原数据;
        public T[] 替换数据;
        public 对象池管理器 箭头池 = new();
        public List<GameObject> 箭头缓存 = new();
        public void Init() {
            //拖动替换
            替换列表.拖动开始时((数据, 位置) => {
                箭头缓存[位置].SetColor(255, 255, 255, 0f);
            });
            替换列表.拖动结束时((数据, 位置, 目标位置) => {
                移动替换(数据, 目标位置);
            });
            //位移透明
            替换列表.创建时((本项, 背景, 位置) => {
                if (本项 == null) {
                    背景.SetColor(255, 255, 255, 0.1f);
                } else {
                    背景.SetColor(255, 255, 255, 0.2f);
                }
            });
        }
        public void 加载列表(List<T> 列表数据) {
            原数据 = 列表数据;
            var 原替换数据 = 替换数据;
            替换数据 = new T[原数据.Count];
            if (原替换数据 != null) {
                执行X次(原替换数据.Length, i => {
                    if (i > 替换数据.Length) return;
                    if (原替换数据[i] != null) {
                        替换数据[i] = 原替换数据[i];
                    }
                });
            }
            加载替换列表();
            刷新箭头列表();
        }
        public void 刷新箭头列表() {
            箭头缓存.ForEach((A) => {
                箭头池.回收(A);
            });
            箭头缓存.Clear();
            箭头列表.Clear();
            原数据.ForEach((A) => {
                var 箭头 = 箭头池.取出(箭头列表);
                箭头缓存.Add(箭头);
            });
            //对替换数据里的每个非空数据，显示对应位置的箭头
            for (int i = 0; i < 替换数据.Count(); i++) {
                if (替换数据[i] != null) {
                    箭头缓存[i].SetColor(255, 255, 255, 1f);
                } else {
                    箭头缓存[i].SetColor(255, 255, 255, 0f);
                }
            }
        }
        public void 加载替换列表() {
            替换列表.加载(替换数据.ToList());
        }
        public void 设置替换(T 数据, int 位置) {
            if (位置 > 替换数据.Count()) throw new Exception("位置超出范围");
            替换数据[位置] = 数据;
            替换列表.加载(替换数据.ToList());
            刷新箭头列表();
        }
        public void 移动替换(T 数据, int 位置) {
            if (位置 > 替换数据.Count()) throw new Exception("位置超出范围");
            var 替 = 替换数据.ToList();
            替.Remove(数据);
            替.Insert(位置, 数据);
            替换数据 = 替.ToArray();
            替换列表.加载(替);
            刷新箭头列表();
        }
        public void 取消替换(int 位置) {
            if (位置 > 替换数据.Count()) throw new Exception("位置超出范围");
            替换数据[位置] = default;
            替换列表.加载(替换数据.ToList());
            刷新箭头列表();
        }
        public void Show() {
            背景.SetActive(true);
        }
        public void Hide() {
            背景.SetActive(false);
        }
    }
}

