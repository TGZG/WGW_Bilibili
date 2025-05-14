using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CMKZ.LocalStorage;

namespace CMKZ {
    [JsonObject(MemberSerialization.OptOut)]
    public class Grid<T> : IEnumerable<T> {
        public T[,] Data;
        public int Width;
        public int Height;
        public Grid() { }
        public Grid(int X, int Y) {
            //Init(X, Y);
            Width = X;
            Height = Y;
            Data = new T[X, Y];
        }
        public Grid<T> Init(int X, int Y) {
            Width = X;
            Height = Y;
            Data = new T[X, Y];
            var A = typeof(T).GetConstructor(Type.EmptyTypes);
            if (A == null) {
                PrintWarning($"类型 {typeof(T).Name} 没有无参构造函数，无法初始化。");
            } else {
                Fill((i, j) =>(T)A.Invoke(null));
            }
            return this;
        }
        public T this[int X] {
            get => this[X / Height, X % Height];
            set => this[X / Height, X % Height] = value;
        }
        public T this[int X, int Y] {
            get {
                if (X < 0 || X >= Width || Y < 0 || Y >= Height) {
                    throw new Exception($"坐标超出范围 {X},{Y}。范围：{Width},{Height}");
                }
                return Data[X, Y];
            }
            set {
                if (X < 0 || X >= Width || Y < 0 || Y >= Height) {
                    throw new Exception($"坐标超出范围 {X},{Y}。范围：{Width},{Height}");
                }
                Data[X, Y] = value;
            }
        }
        public void Fill(Func<int, int, T> X) {
            for (var i = 0; i < Width; i++) {
                for (var j = 0; j < Height; j++) {
                    Data[i, j] = X(i, j);
                }
            }
        }
        public void FillRandom(Func<int, int, T> X, int Y) { //在随机Y个位置执行X
            var A = new List<int>();
            for (var i = 0; i < Width; i++) {
                for (var j = 0; j < Height; j++) {
                    A.Add(i * Height + j);
                }
            }
            for (var i = 0; i < Y; i++) {
                var B = Random(0, A.Count);
                var C = A[B] / Height;
                var D = A[B] % Height;
                Data[C, D] = X(C, D);
                A.RemoveAt(B);
            }
        }
        public void FillRandom(Func<int, int, T,T> X, int Y) { //在随机Y个位置执行X
            var A = new List<int>();
            for (var i = 0; i < Width; i++) {
                for (var j = 0; j < Height; j++) {
                    A.Add(i * Height + j);
                }
            }
            for (var i = 0; i < Y; i++) {
                var B = Random(0, A.Count);
                var C = A[B] / Height;
                var D = A[B] % Height;
                Data[C, D] = X(C, D, Data[C,D]);
                A.RemoveAt(B);
            }
        }
        public void SetRandomArea(int X,Action<T> Y) {
            //随机选择一个位置，然后将那个位置加入列表。从列表中随机选择一个元素，取它相邻的位置加入列表。如此X次。然后对列表每一项：执行Y
            var A = new List<Vector2Int>();
            var B = new Vector2Int(Random(0, Width), Random(0, Height));
            A.Add(B);
            for (var i = 0; i < X; i++) {
                var D = A.Choice().RandomNear().限制(new Vector2Int(0, 0), new Vector2Int(Width - 1, Height - 1));
                if (!A.Contains(D)) {
                    A.Add(D);
                }
            }
            foreach (var i in A) {
                Y(Data[i.x, i.y]);
            }
        }
        public void ForEach(Action<T> X) => ForEach((i, j, k) => X(k));
        public void ForEach(Action<int, int, T> X) {
            for (var i = 0; i < Width; i++) {
                for (var j = 0; j < Height; j++) {
                    X(i, j, Data[i, j]);
                }
            }
        }
        public Dictionary<坐标类, T> GetRound(int X, int Y, int 宽度 = 1, bool 包括自己 = false) {
            var A = new Dictionary<坐标类, T>();
            for (var i = -宽度; i <= 宽度; i++) {
                for (var j = -宽度; j <= 宽度; j++) {
                    if (!包括自己 && i == 0 && j == 0) {
                        continue;
                    }
                    var B = X + i;
                    var C = Y + j;
                    if (B >= 0 && B < Width && C >= 0 && C < Height) {
                        A.Add(new 坐标类(B, C), Data[B, C]);
                    }
                }
            }
            return A;
        }
        public Number SumRound(int 中央X, int 中央Y, Func<T, Number> 筛选, int 半径 = 1) {
            var A = new Number();
            foreach (var i in GetRound(中央X, 中央Y, 半径).Values) {
                A += 筛选(i);
            }
            return A;
        }
        public IEnumerator<T> GetEnumerator() {
            for (var i = 0; i < Width; i++) {
                for (var j = 0; j < Height; j++) {
                    yield return Data[i, j];
                }
            }
        }
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public void 添加一列(Index 行位置) {
            T[,] 新网格 = new T[Width + 1, Height];
            for (var i = 0; i < Width; i++) {
                for (var j = 0; j < Height; j++) {
                    新网格[i, j] = Data[i, j];
                }
            }
            //把行位置的的元素以及右边的元素都复制到新网格的最右边
            int 位置;
            if (行位置.IsFromEnd) { //如果行位置是从后面算起的
                位置 = Width + 1 - 行位置.Value;
            } else { //如果行位置是从前面算起的
                位置 = 行位置.Value;
            }
            for (var j = 0; j < Height; j++) {
                for (var k = 位置; k < Width; k++) {
                    新网格[k + 1, j] = Data[k, j];
                }
            }
            //把行位置的元素设置为默认值
            for (var j = 0; j < Height; j++) {
                var A = typeof(T).GetConstructor(Type.EmptyTypes);
                新网格[位置, j] = (T)A.Invoke(null);
            }
            Data = 新网格;
            Width++;
        }
        public void 添加一行(Index 列位置) {
            T[,] 新网格 = new T[Width, Height + 1];
            for (var i = 0; i < Width; i++) {
                for (var j = 0; j < Height; j++) {
                    新网格[i, j] = Data[i, j];
                }
            }
            //把列位置的元素以及下边的元素都复制到新网格的最下边
            int 位置;
            if (列位置.IsFromEnd) { //如果列位置是从后面算起的
                位置 = Height + 1 - 列位置.Value;
            } else { //如果列位置是从前面算起的
                位置 = 列位置.Value;
            }
            for (var i = 0; i < Width; i++) {
                for (var k = 位置; k < Height; k++) {
                    新网格[i, k + 1] = Data[i, k];
                }
            }
            //把列位置的元素设置为默认值
            for (var i = 0; i < Width; i++) {
                var A = typeof(T).GetConstructor(Type.EmptyTypes);
                新网格[i, 位置] = (T)A.Invoke(null);
            }
            Data = 新网格;
            Height++;
        }

        public void 减少一列(Index 行位置) {
            T[,] 新网格 = new T[Width - 1, Height];
            int 位置;
            if (行位置.IsFromEnd) { //如果行位置是从后面算起的
                位置 = Width + 1 - 行位置.Value;
            } else { //如果行位置是从前面算起的
                位置 = 行位置.Value;
            }
            for (var i = 0; i < Width - 1; i++) {
                for (var j = 0; j < Height; j++) {
                    //行位置左边正常复制
                    if (i < 位置) {
                        新网格[i, j] = Data[i, j];
                    } else if (i == 位置) {//行位置右边跳过此列，后面复制
                        continue;
                    } else {
                        新网格[i, j] = Data[i + 1, j];
                    }
                }
            }
            Data = 新网格;
            Width--;
        }
        public void 减少一行(Index 列位置) {
            T[,] 新网格 = new T[Width, Height - 1];
            int 位置;
            if (列位置.IsFromEnd) { //如果列位置是从后面算起的
                位置 = Height + 1 - 列位置.Value;
            } else { //如果列位置是从前面算起的
                位置 = 列位置.Value;
            }
            for (var i = 0; i < Width; i++) {
                for (var j = 0; j < Height - 1; j++) {
                    //列位置上边正常复制
                    if (j < 位置) {
                        新网格[i, j] = Data[i, j];
                    } else if (j == 位置) {//列位置下边跳过此行，后面复制
                        continue;
                    } else {
                        新网格[i, j] = Data[i, j + 1];
                    }
                }
            }
            Data = 新网格;
            Height--;
        }
    }
    public class 坐标类 {
        public int X;
        public int Y;
        public 坐标类(int A, int B) {
            X = A;
            Y = B;
        }
    }
}