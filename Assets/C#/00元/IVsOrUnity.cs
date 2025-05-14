using System;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CMKZ {
    public static partial class  LocalStorage {
        [初始化函数(typeof(CMKZProject), 初始化优先级.CSharp库)]
        public static void InitIDebug() {
            IVsOrUnity<IDebug>.Single = new VsDebug();
            IVsOrUnity<ILife>.Single = new VsConsoleLife();
        }
    }
    public interface IVsOrUnity<T> {
        public static T Single { get; set; }
    }
}