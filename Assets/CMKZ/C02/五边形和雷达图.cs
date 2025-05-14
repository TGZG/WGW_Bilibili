using Microsoft.CodeAnalysis;
using Newtonsoft.Json;//Json
using System;//Action
using System.IO;//File
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;//List
using System.Diagnostics;
using System.Linq;//from XX select XX
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Timers;//Timer
using TMPro;//InputField
using UnityEngine;//Mono
using UnityEngine.UI;//Image
using UnityEngine.Tilemaps;
using UnityEngine.Video;//Vedio
using static UnityEngine.Object;//Destory
using CMKZ;
using static CMKZ.LocalStorage;
using static UnityEngine.RectTransform;
using Random = UnityEngine.Random;

namespace CMKZ {
    public static partial class LocalStorage {
        public static void 示例() {
            var 上限 = new double[5];
            var 下限 = new double[5];
            var 当前 = new double[5];
            var A = MainPanel.创建雷达图(5);
            A.半径 = 100;
            A.上限 = new double[] { 0.5, 0.6, 0.7, 0.8, 0.5 };
            A.预览 = new double[] { 0.4, 0.5, 0.6, 0.7, 0.4 };
            A.当前 = new double[] { 0.3, 0.4, 0.5, 0.6, 0.3 };
            A.网格位置 = new double[] { 0.25, 0.5, 0.75, 1 };
            A.网格颜色 = Color.black;
            A.上限颜色 = Color.cyan;
            A.预览颜色 = Color.green;
            A.当前颜色 = Color.red;
            A.更新();
        }
        public static 雷达图类 创建雷达图(this GameObject X, int 边数) {
            return new 雷达图类(边数, X);
        }
        public static List<Vector2> 获取X个平分方向(int X) {
            //将360度分成X份，得到X个方向
            var 方向 = new List<Vector2>();
            for (int i = 0; i < X; i++) {
                var 当前角度 = (360 / X) * i + 90;
                var 弧度 = 当前角度 * Mathf.Deg2Rad;
                var X坐标 = (float)(1 * Math.Cos(弧度));
                var Y坐标 = (float)(1 * Math.Sin(弧度));
                方向.Add(new Vector2(X坐标, Y坐标));
            }
            return 方向;
        }
        public static GameObject 创建X边形(this GameObject A, int X, int 半径, Color32 颜色) {
            var B = new GameObject("五边形");
            B.SetParent(A);
            B.AddComponent<CanvasRenderer>();
            B.AddComponent<UI多边形插件>().边数 = X;
            B.GetComponent<UI多边形插件>().半径 = 半径;
            B.GetComponent<UI多边形插件>().color = 颜色;
            return B;
        }
    }
    public class 雷达图类 {
        public GameObject Parent;
        public GameObject 背景层;
        public GameObject 上限层;
        public GameObject 预览层;
        public GameObject 当前层;

        public Color32 网格颜色;
        public Color32 上限颜色;
        public Color32 预览颜色;
        public Color32 当前颜色;

        public double[] 上限;
        public double[] 预览;
        public double[] 当前;

        public double[] 网格位置;

        public int 边数;
        public double 半径;
        public 雷达图类(int 边数, GameObject parent) {
            this.边数 = 边数;
            Parent = parent;
            背景层 = Parent.创建矩形("50% 50% 0 0").SetName("雷达图背景");
            上限层 = Parent.创建矩形("50% 50% 0 0");
            预览层 = Parent.创建矩形("50% 50% 0 0");
            当前层 = Parent.创建矩形("50% 50% 0 0");
            背景层.AddComponent<CanvasRenderer>();
            上限层.AddComponent<CanvasRenderer>();
            预览层.AddComponent<CanvasRenderer>();
            当前层.AddComponent<CanvasRenderer>();
        }
        public void 更新() {
            var 上限 = this.上限;
            var 当前 = this.当前;
            var 预览 = this.预览;

            var 当前占据上限的比例 = 当前.Zip(上限, (x, y) => {
                if (y == 0) return 1;
                else if (x == 0) return 0.01;
                else return x / y;
                }).ToArray();
            var 预览占据上限的比例 = 预览.Zip(上限, (x, y) => {
                if (y == 0) return 1;
                else if (x == 0) return 0.01;
                else return x / y;
            }).ToArray();

            //为背景层添加网格
            //为三层添加多边形边框
            //var 上限层五边形 = 上限层.GetOrAddComponent<UI多边形边框插件>();
            //上限层五边形.color = 上限颜色;
            //上限层五边形.位置比例 = 上限;
            //上限层五边形.半径 = 半径;
            //上限层五边形.边数 = 边数;
            //上限层五边形.粗细 = 5;
            var 预览层五边形 = 预览层.GetOrAddComponent<UI多边形边框插件>();
            预览层五边形.color = 预览颜色;
            预览层五边形.位置比例 = 预览占据上限的比例;
            预览层五边形.半径 = 半径;
            预览层五边形.边数 = 边数;
            预览层五边形.粗细 = 5;
            var 当前层五边形 = 当前层.GetOrAddComponent<UI多边形边框插件>();
            当前层五边形.color = 当前颜色;
            当前层五边形.位置比例 = 当前占据上限的比例;
            当前层五边形.半径 = 半径;
            当前层五边形.边数 = 边数;
            当前层五边形.粗细 = 5;
            var 背景层线段 = 背景层.GetOrAddComponent<UI多边形中心连接顶点线段插件>();
            背景层线段.数值 = 上限;
            背景层线段.color = 网格颜色;
            背景层线段.边数 = 边数;
            背景层线段.半径 = 半径;
            背景层线段.粗细 = 2;
            背景层线段.字体大小 = 13;


            var 网格数量 = 网格位置.Length;
            var 所有网格物体 = 背景层.GetChildren();
            UI多边形边框插件[] 所有网格 = new UI多边形边框插件[网格数量];
            if (所有网格物体.Count != 网格数量) {
                foreach (var item in 所有网格物体) {
                    item.Destroy();
                }
                for (int i = 0; i < 网格数量; i++) {
                    var 网格 = 背景层.创建矩形("0 0 100% 100%");
                    网格.AddComponent<CanvasRenderer>();
                    所有网格[i] = 网格.AddComponent<UI多边形边框插件>();
                }
            }
            所有网格 = 背景层.GetComponentsInChildren<UI多边形边框插件>();
            for (int i = 0; i < 网格数量; i++) {
                所有网格[i].color = 网格颜色;
                所有网格[i].半径 = 半径;
                所有网格[i].边数 = 边数;
                所有网格[i].位置比例 = new double[边数].Select(t => 网格位置[i]).ToArray();
                所有网格[i].粗细 = 2;
                所有网格[i].color = 网格颜色;
            }
        }
    }
    public class UI多边形中心连接顶点线段插件 : Graphic {
        public Vector2[] 线段;
        public double 粗细;
        public int 边数;
        public double 半径;

        public double[] 数值;
        public int 字体大小 = 17;
        public GameObject[] 数值物体缓存;
        public int 外围大小 = 10;
        public new void Start() {
            数值物体缓存 = new GameObject[边数];
            执行X次(边数, t => {
                数值物体缓存[t]=gameObject.GetParent().创建矩形(new PanelConfig() {
                    矩形模式 = 矩形模式.固定文本框,
                    Position = "0 0 100% 0",
                    Font = "基督山伯爵",
                    TextSize = 字体大小,
                    TextAlign=TextAlignmentOptions.Center,
                    TextColor = 白色V4
                });
            });
        }
        protected override void OnPopulateMesh(VertexHelper vh) {
            vh.Clear();
            var 方向 = 获取X个平分方向(边数);
            线段 = 方向.Select(x => x * (float)半径).ToArray();
            var 线段顶点 = new List<List<Vector2>>();
            for (int i = 0; i < 边数; i++) {
                //获取线段的法线方向。
                var 法线方向 = new Vector2(线段[i].y, -线段[i].x).normalized;
                //获取终点在法线方向上左右两侧各粗细/2的位置。
                var 终点左侧 = 线段[i] + 法线方向 * (float)粗细 / 2;
                var 终点右侧 = 线段[i] - 法线方向 * (float)粗细 / 2;
                //获取起点在法线方向上左右两侧各粗细/2的位置。
                var 起点左侧 = 法线方向 * (float)粗细 / 2;
                var 起点右侧 = -法线方向 * (float)粗细 / 2;
                线段顶点.Add(new List<Vector2> { 起点左侧, 起点右侧, 终点左侧, 终点右侧 });
            }
            线段顶点.ForEach(x => x.ForEach(y => vh.AddVert(y, color, new())));
            for (int i = 0; i < 边数; i++) {
                var 左下角 = i * 4;
                var 左上角 = i * 4 + 1;
                var 右上角 = i * 4 + 2;
                var 右下角 = i * 4 + 3;
                vh.AddTriangle(右下角, 右上角, 左上角);
                vh.AddTriangle(右下角, 左上角, 左下角);
            }
            for (int i = 0; i < 边数; i++) {
                数值物体缓存[i].SetText(数值[i].ToString()).SetName(数值[i].ToString());
                //让坐标向此方向延长10个单位长度
                var 坐标 = 线段[i] + 线段[i].normalized * 外围大小;
                var WH = gameObject.获取相对位置大小(gameObject.GetParent()).Split(" ");
                var x = WH[0].ToFloat();
                var y = WH[1].ToFloat();
                数值物体缓存[i].调整矩形($"{(int)坐标.x} 100%-{坐标.y + y} 100% 0");
            }
        }
    }
    public class UI多边形边框插件 : Graphic {
        public double 半径;
        public double 粗细;
        public int 边数;
        public Vector2[] 边框内顶点;
        public Vector2[] 边框外顶点;
        public double[] 位置比例;
        protected override void OnPopulateMesh(VertexHelper vh) {
            vh.Clear();
            var 方向 = 获取X个平分方向(边数);
            边框内顶点 = 方向.Select(x => x * (float)半径).ToArray();
            边框外顶点 = 方向.Select(x => x * (float)(半径)).ToArray();
            for (int i = 0; i < 边数; i++) {
                边框内顶点[i] = 边框内顶点[i] * (float)位置比例[i];
            }
            for (int i = 0; i < 边数; i++) {
                边框外顶点[i] = 边框外顶点[i] * (float)位置比例[i];
                var 距离零点 = 边框外顶点[i].magnitude;
                边框外顶点[i] = 边框外顶点[i] * (float)((距离零点 + 粗细) / 距离零点);
            }
            边框内顶点.ForEach(x => vh.AddVert(x, color, new()));
            边框外顶点.ForEach(x => vh.AddVert(x, color, new()));
            for (int i = 0; i < 边数; i++) {
                var 右下角 = i;
                var 右上角 = i + 边数;
                var 左上角 = ((i + 1) % 边数) + 边数;
                var 左下角 = (i + 1) % 边数;
                vh.AddTriangle(右下角, 右上角, 左上角);
                vh.AddTriangle(右下角, 左上角, 左下角);
            }
        }
    }
    public class UI多边形插件 : Graphic {
        public double 半径 = 100;
        public int 边数;
        public Vector2[] 顶点;
        protected override void OnPopulateMesh(VertexHelper vh) {
            vh.Clear();
            vh.AddVert(new Vector2(0, 0), color, new());
            顶点 = 获取X个平分方向(边数).Select(x => x * (float)半径).ToArray();
            顶点.ForEach(x => vh.AddVert(x, color, new()));
            for (int i = 0; i < 顶点.Length; i++) {
                vh.AddTriangle(0, i + 1, (i + 1) % 顶点.Length + 1);
            }
        }
        private void AddBorder(VertexHelper vh) {
            var color = Color.black; // 边框颜色
            var thickness = 100; // 假设的边框厚度，实际实现中可能需要以不同方式处理

            // 真正的线宽调整需要通过添加额外的顶点和构建复杂的三角形来实现
            vh.AddVert(new Vector2(0, 100 + thickness), color, new Vector2(0, 1));
            vh.AddVert(new Vector2(-100 - thickness, 50 + thickness), color, new Vector2(0, 1));
            vh.AddVert(new Vector2(-50 - thickness, -100 - thickness), color, new Vector2(0, 1));
            vh.AddVert(new Vector2(50 + thickness, -100 - thickness), color, new Vector2(0, 1));
            vh.AddVert(new Vector2(100 + thickness, 50 + thickness), color, new Vector2(0, 1));

            // 假设添加线条作为边框，实际上需要通过绘制更多的顶点和三角形来实现真正的边框效果
        }
    }
}