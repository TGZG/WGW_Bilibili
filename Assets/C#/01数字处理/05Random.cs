using System;//Action
using System.Linq;
using System.Security.Cryptography;
using static CMKZ.LocalStorage;

namespace CMKZ {
    public static partial class LocalStorage {
        public static int RandomSeed = DateTime.Now.Millisecond;
        public static Random _Randomer;
        public static Random Randomer {
            get {
                if (_Randomer == null) {
                    _Randomer = new Random(RandomSeed);
                }
                return _Randomer;
            }
        }
        public static void SetRandomSeed(int X = 0) {
            if (X == 0) {
                X = DateTime.Now.Millisecond;
            }
            RandomSeed = X;
            _Randomer = new Random(RandomSeed);
        }
        public static int GetHash(int X) {
            //哈希运算：即混沌运算。把一个数变成另一个数。输入信息的一点微小变化，都会导致输出的巨大变化，变化的毫无规律、但又确切唯一。
            byte[] dataBytes = BitConverter.GetBytes(X);
            byte[] hashBytes = SHA256.Create().ComputeHash(dataBytes);
            return int.Parse(BitConverter.ToString(hashBytes).Replace("-", string.Empty).Substring(0, 8));
        }
        public static int RandomBySeed(this int 标准值, params int[] 种子) {
            //要求：【标准值、小种子、种子】确定唯一结果。结果接近标准值，接近程度与小种子线性无关、与种子线性无关、与标准值线性无关。
            int A = 0;
            foreach (var i in 种子) {
                A += GetHash(i);
            }
            return Math.Max(1, 标准值 * Math.Max(2, (A + GetHash(标准值)) % 100) / 20);
        }
        ///<summary>0到1的随机数</summary>
        public static float RandomFloat() {
            return Random(0.0f, 1.0f);
        }
        public static Guid RandomGuid() {
            return Guid.NewGuid();
        }
        public static bool Random(double X) {
            return Random() < X;
        }
        public static void Random(double X, Action Y) {
            if (Random(X)) {
                Y();
            }
        }
        public static float Random(float X, float Y) {
            return (float)(Randomer.NextDouble() * (Y - X) + X);
        }
        public static double Random() {
            return Randomer.NextDouble();
        }
        public static double Random(double X, double Y) {
            return Randomer.NextDouble() * (Y - X) + X;
        }
        /// <summary>
        /// 有左无右
        /// </summary>
        public static int Random(int X, int Y) {
            return Randomer.Next(X, Y);
        }
        /// <summary>
        /// 获取从X到Y的一个随机值
        /// 对数均匀分布。例如，如果X=1，Y=1000，那么1-10的概率是33%，10-100的概率是33%，100-1000的概率是33%。
        /// </summary>
        public static double RandomLog(double X,double Y) {
            return Math.Pow(Y / X, Random()) * X;
        }
        public static int RightRandom(int X, int Y) {
            if (Y - X < 2) {
                PrintWarning("随机数差距小于2");
                return Y;
            }
            var A = Randomer.Next(X, Y);
            if (A < (Y + X) / 2) {
                A = RightRandom(X, Y);
            }
            return A;
        }
        /// <summary>
        /// 左可取，右不可
        /// </summary>
        public static List<int> Range(int A, int B) {
            var C = new List<int>();
            for (int i = A; i < B; i++) {
                C.Add(i);
            }
            return C;
        }
    }
}