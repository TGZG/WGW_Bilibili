using UnityEngine;

namespace CMKZ {
    public partial class LocalStorage {
        public static Colors 固定时颜色 = new() {
            常规颜色 = 黑二分,
            悬浮颜色 = 黑二分,
            按下颜色 = 黑二分
        };
        public static Colors 固定透明色 = new() {
            常规颜色 = 透明,
            悬浮颜色 = 透明,
            按下颜色 = 透明
        };
        public static Colors 按钮黑色 = new() {
            常规颜色 = 黑五分,
            悬浮颜色 = 黑二分,
            按下颜色 = 透明,
        };
        public static Colors 按钮四分之一黑 = new() {
            常规颜色 = 黑三分,
            悬浮颜色 = 黑二分,
            按下颜色 = 透明,
        };
        public static Colors 按钮八分之一黑 = new() {
            常规颜色 = 黑二分,
            悬浮颜色 = 黑一分,
            按下颜色 = 透明,
        };
        public static Colors 按钮透明四分之一黑 = new() {
            常规颜色 = 透明,
            悬浮颜色 = 黑二分,
            按下颜色 = 黑三分,
        };
    }
    public struct Colors {
        public Vector4 常规颜色;
        public Vector4 悬浮颜色;
        public Vector4 按下颜色;
        public static Colors Default = new() {
            常规颜色 = new Vector4(255, 255, 255, 1),
            悬浮颜色 = new Vector4(200, 200, 200, 1),
            按下颜色 = new Vector4(150, 150, 150, 1)
        };
        public static Colors Transparent = new() {
            常规颜色 = new Vector4(255, 255, 255, 0),
            悬浮颜色 = new Vector4(255, 255, 255, 0.5f),
            按下颜色 = new Vector4(150, 150, 150, 0.5f)
        };
        public static Colors Grey = new() {
            常规颜色 = new Vector4(200, 200, 200, 1),
            悬浮颜色 = new Vector4(150, 150, 150, 1),
            按下颜色 = new Vector4(120, 120, 120, 1)
        };
        public static Colors Grey2 = new() {
            常规颜色 = new Vector4(150, 150, 150, 1),
            悬浮颜色 = new Vector4(120, 120, 120, 1),
            按下颜色 = new Vector4(100, 100, 100, 1)
        };
        public static Colors Grey3 = new() {
            常规颜色 = new Vector4(120, 120, 120, 1),
            悬浮颜色 = new Vector4(100, 100, 100, 1),
            按下颜色 = new Vector4(80, 80, 80, 1)
        };
        public static bool operator ==(Colors X, Colors Y) {
            return X.常规颜色 == Y.常规颜色 && X.悬浮颜色 == Y.悬浮颜色 && X.按下颜色 == Y.按下颜色;
        }
        public static bool operator !=(Colors X, Colors Y) {
            return X.常规颜色 != Y.常规颜色 || X.悬浮颜色 != Y.悬浮颜色 || X.按下颜色 != Y.按下颜色;
        }
        public override bool Equals(object obj) => base.Equals(obj);
        public override int GetHashCode() => base.GetHashCode();
        public static Colors 按下 = new() {
            常规颜色 = new Vector4(150, 150, 150, 1),
            悬浮颜色 = new Vector4(200, 200, 200, 1),
            按下颜色 = new Vector4(255, 255, 255, 1)
        };
        public static Colors 透明 = new() {
            常规颜色 = new(1, 0, 0, 0),//不要使用0000，因为会被识别为default从而被忽略
            悬浮颜色 = new(255, 255, 255, 0.5f),
            按下颜色 = new(255, 255, 255, 0.9f)
        };
        public static Colors 透明变黑 = new() {
            常规颜色 = new(1, 0, 0, 0),//不要使用0000，因为会被识别为default从而被忽略
            悬浮颜色 = new(0, 0, 0, 0.5f),
            按下颜色 = new(0, 0, 0, 0.9f)
        };
        public static Colors 半透明 = new() {
            常规颜色 = new(255, 255, 255, 0.5f),
            悬浮颜色 = new(255, 255, 255, 0.3f),
            按下颜色 = new(1, 0, 0, 0),//不要使用0000，因为会被识别为default从而被忽略
        };
        public static Colors 四分之一透明 = new() {
            常规颜色 = new(255, 255, 255, 0.3f),
            悬浮颜色 = new(255, 255, 255, 0.2f),
            按下颜色 = new(1, 0, 0, 0),//不要使用0000，因为会被识别为default从而被忽略
        };
        public static Colors 半透明变蓝 = new() {
            常规颜色 = new(255, 255, 255, 0.5f),
            悬浮颜色 = new(0, 0, 255, 0.3f),
            按下颜色 = new(1, 0, 0, 0),//不要使用0000，因为会被识别为default从而被忽略
        };
        public static Colors 半透明变绿 = new() {
            常规颜色 = new(255, 255, 255, 0.5f),
            悬浮颜色 = new(0, 255, 0, 0.3f),
            按下颜色 = new(255, 255, 255, 0.2f),
        };
        public static Colors 半透明变红 = new() {
            常规颜色 = new(255, 255, 255, 0.5f),
            悬浮颜色 = new(255, 0, 0, 0.3f),
            按下颜色 = new(1, 0, 0, 0),//不要使用0000，因为会被识别为default从而被忽略
        };
        public static Colors 蓝色调 = new() {
            常规颜色 = new(0, 0, 255, 0.5f),
            悬浮颜色 = new(0, 0, 255, 0.3f),
            按下颜色 = new(0, 0, 255, 0.2f),
        };
        public static Colors 绿色调 = new() {
            常规颜色 = new(0, 255, 0, 0.5f),
            悬浮颜色 = new(0, 255, 0, 0.3f),
            按下颜色 = new(0, 255, 0, 0.2f),
        };
        public static Colors 红色调 = new() {
            常规颜色 = new(255, 0, 0, 0.5f),
            悬浮颜色 = new(255, 0, 0, 0.3f),
            按下颜色 = new(255, 0, 0, 0.2f),
        };
    }
    public static partial class LocalStorage {
        public static float 模糊 = 4;
        public static string 游戏字体 = "基督山伯爵";
        public static double 方案目录区宽度 = 300;
        public static double 大边距 = 20;
        public static double 边距 = 10;
        public static double 小边距 = 5;
        public static double 方案网格项高度 = 80;
        public static double 方案网格项宽度 = 200;
        public static double 装备下区高度 = 100;
        public static int 目录树间距 = 10;
        public static double 词条高度 = 30;
        public static RectOffset 目录树边距 = new(10, 10, 10, 10);
        public static Vector2 仓库项目大小 = new(60, 100);
        public static double 仓库文字高度 = 40;
        public static double 仓库项目间距 = 10;
        public static Colors DefaultColors = new() {
            常规颜色 = new Vector4(255, 255, 255, 1),
            悬浮颜色 = new Vector4(200, 200, 200, 1),
            按下颜色 = new Vector4(150, 150, 150, 1)
        };
    }
}
