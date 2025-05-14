using System;//Action
using System.Collections.Generic;
using System.Linq;//from XX select XX
using System.Reflection;
using static CMKZ.LocalStorage;

namespace CMKZ {
    public enum 初始化优先级 {
        暂时废弃,
        CSharp库,
        引擎库,
        无依赖,//遍历设定
        第二级,//基于设定，再遍历得到二级设定
    }
    /// <summary>
    /// 标记为此特性的函数会在游戏启动时自动执行
    /// </summary>
    public class 初始化函数Attribute : Attribute {
        public 初始化优先级 优先级;
        public Type Project;
        public 初始化函数Attribute(Type X, 初始化优先级 Y = 初始化优先级.无依赖) {
            Project = X;
            优先级 = Y;
        }
    }
    public interface IProject { }
    public class CMKZProject : IProject { }
    public static partial class LocalStorage {
        private static Type[] _MainTypes;
        /// <summary>
        /// 包含类内的类
        /// </summary>
        public static Type[] MainTypes => _MainTypes ??= Assembly.GetExecutingAssembly().GetTypes().Where(i => i.Namespace?.StartsWith(nameof(CMKZ)) == true).ToArray();
        private static Type[] _MainTypesNotAbstract;
        /// <summary>
        /// 包含类内的类
        /// </summary>
        public static Type[] MainTypesNotAbstract => _MainTypesNotAbstract ??= MainTypes.Where(i => !i.IsAbstract).ToArray();
        private static IEnumerable<(MethodInfo 函数, 初始化函数Attribute 特性)> _AllInit;
        /// <summary>
        /// 包含非静态类内的静态函数 <br />
        /// 不包含泛型接口内的静态函数
        /// </summary>
        public static IEnumerable<(MethodInfo 函数, 初始化函数Attribute 特性)> AllInit => _AllInit ??= from i in MainTypes from 函数 in i.GetMethods() let 特性 = 函数.GetCustomAttribute<初始化函数Attribute>() where 特性 != null select (函数, 特性);
        public static void InitCMKZ() => InitProject<CMKZProject>();
        public static void InitProject<T>() where T : IProject {
            var A = AllInit.Where(t => t.特性.Project == typeof(T));
            A.Where(t => t.特性.优先级 == 初始化优先级.CSharp库).ForEach(t => t.函数.Invoke(null, null));
            A.Where(t => t.特性.优先级 == 初始化优先级.引擎库).ForEach(t => t.函数.Invoke(null, null));
            A.Where(t => t.特性.优先级 == 初始化优先级.无依赖).ForEach(t => t.函数.Invoke(null, null));
            A.Where(t => t.特性.优先级 == 初始化优先级.第二级).ForEach(t => t.函数.Invoke(null, null));
        }
    }
}