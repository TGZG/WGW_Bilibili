namespace CMKZ {
    public static partial class LocalStorage {
        //千亿、百亿、十亿、亿
        public static double 千亿(this double X) => X * 1000_0000_0000;
        public static double 千亿(this int X) => X * 1000_0000_0000;
        public static double 百亿(this double X) => X * 100_0000_0000;
        public static double 百亿(this int X) => X * 100_0000_0000;
        public static double 十亿(this double X) => X * 10_0000_0000;
        public static double 十亿(this int X) => X * 10_0000_0000;
        public static double 亿(this double X) => X * 10000_0000;
        public static double 亿(this int X) => X * 10000_0000;
        public static double 千万(this double X) => X * 1000_0000;
        public static double 千万(this int X) => X * 1000_0000;
        public static double 百万(this double X) => X * 100_0000;
        public static double 百万(this int X) => X * 100_0000;
        public static double 十万(this double X) => X * 10_0000;
        public static double 十万(this int X) => X * 10_0000;
        public static double 万(this double X) => X * 10000;
        public static double 万(this int X) => X * 10000;
    }
}