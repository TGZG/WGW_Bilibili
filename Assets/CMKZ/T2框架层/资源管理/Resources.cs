using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using static CMKZ.LocalStorage;
using System.Collections;
using static UnityEngine.Object;
using System;
using TMPro;

namespace CMKZ {
    public class ResourcesLoader<T> : Dictionary<string, T> where T : UnityEngine.Object {
        public ResourcesLoader() {
            foreach (var X in AssetBundles.LoadAllAssets<T>()) {
                if (ContainsKey(X.name)) {
                    PrintWarning("资源重复：" + X.name);
                    continue;
                }
                base[X.name] = X;
            }
        }
        public new T this[string X] {
            get {
                if (X.IsNullOrEmpty()) {
                    PrintWarning("正在尝试获取null或empty");
                    return null;
                }
                X = X.Split("/").Last();//兼容旧版
                if (!ContainsKey(X)) {
                    PrintWarning("资源不存在：" + X);
                    return null;
                }
                return base[X];
            }
        }
    }
    public class TextResourcesLoader : ResourcesLoader<TextAsset> {
        public new string this[string X] {
            get {
                var A = base[X];
                return A != null ? A.text.不可见清除() : null;
            }
        }
    }
    public static partial class LocalStorage {
        public static AssetBundle _AssetBundles;
        public static AssetBundle AssetBundles => _AssetBundles ??= AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/AssetBundles/CMKZ");
        public static ResourcesLoader<Texture2D> _AllT2D;
        public static ResourcesLoader<Texture2D> AllT2D => _AllT2D ??= new();
        public static ResourcesLoader<Sprite> _AllSprite;
        public static ResourcesLoader<Sprite> AllSprite => _AllSprite ??= new();
        public static ResourcesLoader<AudioClip> _AllSound;
        public static ResourcesLoader<AudioClip> AllSound => _AllSound ??= new();
        public static ResourcesLoader<VideoClip> _AllVideo;
        public static ResourcesLoader<VideoClip> AllVideo => _AllVideo ??= new();
        public static ResourcesLoader<GameObject> _AllPrefab;
        public static ResourcesLoader<GameObject> AllPrefab => _AllPrefab ??= new();
        public static TextResourcesLoader _AllText;
        public static TextResourcesLoader AllText => _AllText ??= new();
        public static GameObject LoadPrefab(this GameObject X, string Y) {
            return Instantiate(AllPrefab[Y], X.transform);
        }
        public static GameObject LoadPrefab(this Transform X, string Y) {
            return Instantiate(AllPrefab[Y], X);
        }
        /// <summary>
        /// 慎用。建议仅在编辑器模式使用，且做好备份。
        /// 此函数会永久修改所有预制体的原件。
        /// </summary>
        /// <![CDATA[
        /// SetAllPrefab(t => t.GetComponentsInChildren<TextMeshProUGUI>().ForEach(t => t.fontSize = 15));
        /// ]]>
        public static void SetAllPrefab(Action<GameObject> X) {
            foreach (var Y in AllPrefab.Values) {
                X(Y);
            }
        }
    }
}