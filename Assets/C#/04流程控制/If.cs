using System;
using System.Collections.Generic;

namespace CMKZ {
    public static partial class LocalStorage {
        /// <![CDATA[
        /// A.IfNotEmpty(() => F()).Else(() => G());
        /// ]]>
        public static IfElse IfNotEmpty<T>(this IEnumerable<T> X, Action Y) {
            if (X != null) {
                Y();
                return new IfElse { Right = true };
            }
            return new IfElse();
        }
    }
    public class IfElse {
        public bool Right;
        public void Else(Action X) {
            if (!Right) {
                X();
            }
        }
    }
}