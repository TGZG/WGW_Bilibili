using System;
using UnityEngine;

namespace CMKZ {
    public enum Direction {
        无 = 0b1000_0000,
        右 = 0b0000_0001,
        上 = 0b0000_0010,
        左 = 0b0000_0100,
        下 = 0b0000_1000,
    }
    public static partial class LocalStorage {
        public static Dictionary<Direction, Texture2D> 鼠标图片;
        public static GameObject 允许缩放(this GameObject X, Action<Vector2> OnResize = null) {
            if (鼠标图片 == null) {
                鼠标图片 = new Dictionary<Direction, Texture2D>();
                鼠标图片[Direction.上] = AllT2D["游戏素材/鼠标/上"];
                //都使用ALLT2D
                鼠标图片[Direction.左] = AllT2D["游戏素材/鼠标/左"];
                鼠标图片[Direction.下] = AllT2D["游戏素材/鼠标/下"];
                鼠标图片[Direction.右] = AllT2D["游戏素材/鼠标/右"];
                鼠标图片[Direction.左 | Direction.上] = AllT2D["游戏素材/鼠标/左上"];
                鼠标图片[Direction.左 | Direction.下] = AllT2D["游戏素材/鼠标/左下"];
                鼠标图片[Direction.右 | Direction.上] = AllT2D["游戏素材/鼠标/右上"];
                鼠标图片[Direction.右 | Direction.下] = AllT2D["游戏素材/鼠标/右下"];
            }
            Direction 拖动方向 = Direction.无;
            Vector3 上个位置 = Vector3.zero;
            X.OnMouseNear((t) => {
                Print("AAAa");
                var A = X.GetMouseDirection();
                Print(A);
                Cursor.SetCursor(鼠标图片[A], Vector2.zero, CursorMode.ForceSoftware);
            });
            X.OnMouseEndNear((t) => {
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            });
            bool 边缘拖动中 = false;
            //X.OnDrag(t => {
            //    if (边缘拖动中) {
            //        Vector2 变化量 = Input.mousePosition - 上个位置;
            //        X.调整尺寸(拖动方向, 变化量);
            //        OnResize?.Invoke(变化量);
            //        上个位置 = Input.mousePosition;
            //    }
            //    if (边缘拖动中) {
            //        Vector2 变化量 = Input.mousePosition - 上个位置;
            //        X.调整尺寸(拖动方向, 变化量);
            //        OnResize?.Invoke(变化量);
            //        边缘拖动中 = false;
            //        拖动方向 = Direction.无;
            //    }
            //});
            X.OnBeginDrag(t => {
                if (X.IsMouseNear(MouseNear.边缘距离)) {
                    边缘拖动中 = true;
                    拖动方向 = X.GetMouseDirection();
                    上个位置 = Input.mousePosition;
                }
            });
            X.OnEndDrag(t => {
                if (边缘拖动中) {
                    Vector2 变化量 = Input.mousePosition - 上个位置;
                    X.调整尺寸(拖动方向, 变化量);
                    OnResize?.Invoke(变化量);
                    边缘拖动中 = false;
                    拖动方向 = Direction.无;
                }
            });
            return X;
        }
        public static Direction GetMouseDirection(this GameObject X) {
            Vector3[] corners = X.GetComponent<RectTransform>().四角坐标();
            KeyValueList<Direction, float> 距离 = new();
            距离.Add(Direction.左, Math.Abs(Input.mousePosition.x - corners[0].x));
            距离.Add(Direction.上, Math.Abs(Input.mousePosition.y - corners[1].y));
            距离.Add(Direction.右, Math.Abs(Input.mousePosition.x - corners[2].x));
            距离.Add(Direction.下, Math.Abs(Input.mousePosition.y - corners[3].y));
            距离.Sort((X, Y) => X.Value < Y.Value);
            if (距离[0].Value < MouseNear.边缘距离 && 距离[1].Value < MouseNear.边缘距离) return 距离[0].Key | 距离[1].Key;
            if (距离[0].Value < MouseNear.边缘距离) return 距离[0].Key;
            throw null;
        }
        public static void 调整尺寸(this GameObject X, Direction 拖拽的边, Vector2 变化区域) {
            Vector2 size = X.GetComponent<RectTransform>().rect.size;
            Vector2 当前宽高 = size;
            if (拖拽的边 == Direction.无) return;
            if (拖拽的边.Have(Direction.左)) size.x = X.GetComponent<RectTransform>().rect.size.x - 变化区域.x;
            if (拖拽的边.Have(Direction.上)) size.y = X.GetComponent<RectTransform>().rect.size.y + 变化区域.y;
            if (拖拽的边.Have(Direction.下)) size.y = X.GetComponent<RectTransform>().rect.size.y - 变化区域.y;
            if (拖拽的边.Have(Direction.右)) size.x = X.GetComponent<RectTransform>().rect.size.x + 变化区域.x;
            X.SetSize(size = size.ShouldBiggerThan(100));
            if (拖拽的边.Have(Direction.左)) X.GetComponent<RectTransform>().anchoredPosition += new Vector2((当前宽高.x - size.x) / 2, 0);
            if (拖拽的边.Have(Direction.上)) X.GetComponent<RectTransform>().anchoredPosition += new Vector2(0, (size.y - 当前宽高.y) / 2);
            if (拖拽的边.Have(Direction.下)) X.GetComponent<RectTransform>().anchoredPosition += new Vector2(0, (当前宽高.y - size.y) / 2);
            if (拖拽的边.Have(Direction.右)) X.GetComponent<RectTransform>().anchoredPosition += new Vector2((size.x - 当前宽高.x) / 2, 0);
        }
        public static bool Have(this Direction A, Direction B) {
            return (A & B) == B;
        }
        public static int 计算角度(Direction X) {
            if (X == Direction.右) return 0;
            if (X == Direction.上) return 2;
            if (X == Direction.左) return 4;
            if (X == Direction.下) return 6;
            if (X == (Direction.右 | Direction.上)) return 1;
            if (X == (Direction.左 | Direction.上)) return 3;
            if (X == (Direction.左 | Direction.下)) return 5;
            if (X == (Direction.右 | Direction.下)) return 7;
            return -1;
        }
        public static Direction ToDirection(this string X) {
            if (X == "左") {
                return Direction.左;
            }
            throw new Exception();
        }
    }
}
