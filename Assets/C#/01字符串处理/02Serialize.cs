using Newtonsoft.Json;//Json
using Newtonsoft.Json.Serialization;
using System;//Action
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace CMKZ {
    public static partial class LocalStorage {
        public static string JsonSerialize(this object X, bool W = true) {
            return JsonConvert.SerializeObject(X, new JsonSerializerSettings {
                TypeNameHandling = W ? TypeNameHandling.All : TypeNameHandling.None,//不要还原为基类
                //ContractResolver = new IgnoreActionContractResolver(),//不要序列化委托与属性
                MetadataPropertyHandling = W ? MetadataPropertyHandling.Default :MetadataPropertyHandling.Ignore,
                Formatting = Formatting.Indented,//美化格式
                PreserveReferencesHandling = W ? PreserveReferencesHandling.Objects : PreserveReferencesHandling.None,//保留引用
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,//循环引用
            });
        }
        public static T JsonDeserialize<T>(this string X, bool W = true) {
            return JsonConvert.DeserializeObject<T>(X, new JsonSerializerSettings {
                TypeNameHandling = W ? TypeNameHandling.All : TypeNameHandling.None,
                //ContractResolver = new IgnoreActionContractResolver(),
                MetadataPropertyHandling = W ? MetadataPropertyHandling.Default : MetadataPropertyHandling.Ignore,
                Formatting = Formatting.Indented,
                PreserveReferencesHandling = W ? PreserveReferencesHandling.Objects : PreserveReferencesHandling.None,//保留引用
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
            });
        }
        public static byte[] StringToBytes(this string X) {
            return Encoding.UTF8.GetBytes(X);
        }
        public static string BytesToString(this byte[] X) {
            return Encoding.UTF8.GetString(X);
        }
        public static string Serialize(this object X) {
            var A = "";
            foreach (var B in X.GetType().GetFields()) {
                A += $"{B.Name}：{B.GetValue(X)}\n";
            }
            foreach (var B in X.GetType().GetProperties()) {
                A += $"{B.Name}：{B.GetValue(X)}\n";
            }
            return A;
        }
    }
    public class IgnoreActionContractResolver : DefaultContractResolver {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization) {
            var property = base.CreateProperty(member, memberSerialization);
            if (typeof(Delegate).IsAssignableFrom(property.PropertyType) || (member.MemberType == MemberTypes.Property && member.GetCustomAttribute<JsonPropertyAttribute>() == null)) {
                property.ShouldSerialize = instance => false;
            }
            return property;
        }
    }
}