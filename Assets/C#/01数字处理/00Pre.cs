using System;//Action
using System.Text.RegularExpressions;

namespace CMKZ {
    public static partial class LocalStorage {
        public static string 数字变化(this string X, Func<Number, Number> Y) {
            //从X中正则提取出数字，然后用Y变化，再替换回去
            return Regex.Replace(X, @"\d+", i => Y(i.Value.ToInt()).ToString());
        }
    }
}