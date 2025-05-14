namespace CMKZ {
    public static partial class LocalStorage {
        public static double ShouldBetween(this double X, double A, double B) {
            if (X > B) {
                return B;
            }
            if (X < A) {
                return A;
            }
            return X;
        }
        public static int ShouldBiggerThan(this int X, int Y) {
            return X >= Y ? X : Y;
        }
        public static int ShouldSmallerThan(this int X, int Y) {
            return X <= Y ? X : Y;
        }
        public static double ShouldBiggerThan(this double X, double Y) {
            return X >= Y ? X : Y;
        }
        public static double ShouldSmallerThan(this double X, double Y) {
            return X <= Y ? X : Y;
        }
        public static bool IsBetween(this float X, float A, float B) {
            return X >= A && X <= B;
        }
    }
}