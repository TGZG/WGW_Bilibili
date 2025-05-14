using UnityEditor;
using UnityEngine;
using System.IO;
using static CMKZ.LocalStorage;

namespace CMKZ {
    public class CreateAssetBundles {
        [MenuItem("Assets/CMKZ/打包资源")]
        public static void BuildAllAssetBundles() {
            var A = Application.streamingAssetsPath + "/AssetBundles";
            if (!Directory.Exists(A)) {
                Directory.CreateDirectory(A);
            }
            标记需要打包();
            BuildPipeline.BuildAssetBundles(A, BuildAssetBundleOptions.UncompressedAssetBundle, BuildTarget.StandaloneWindows);
        }
        [MenuItem("Assets/CMKZ/标记资源")]
        public static void 标记需要打包() {
            标记需要打包("Assets/Resources");
        }
        public static void 标记需要打包(string path) {
            foreach (string i in Directory.GetFiles(path)) {
                if (!i.EndsWith(".meta")) {
                    AssetImporter importer = AssetImporter.GetAtPath(i);
                    if (importer != null) {
                        var A = i.Replace("\\", "/").Replace("//", "/").TrimEnd('/');
                        A = A.Replace("Assets/Resources/","");
                        A = FirstOr(A.Split("/"),("未知"));
                        //importer.assetBundleName = A[..A.LastIndexOf('.')];
                        //importer.assetBundleName = A;
                        importer.assetBundleName = "CMKZ";
                    }
                } else {
                    AssetImporter importer = AssetImporter.GetAtPath(i);
                    if (importer != null) {
                        importer.assetBundleName = "";
                    }
                }
            }
            foreach (string i in Directory.GetDirectories(path)) {
                标记需要打包(i);
            }
        }
        public static string FirstOr(string[] X, string Y) {
            return X.Length > 0 ? X[0] : Y;
        }
    }
}