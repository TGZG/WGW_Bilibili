using Microsoft.CodeAnalysis;
using Newtonsoft.Json;//Json
using System;//Action
using System.Collections;
using System.Collections.Generic;//List
using System.Diagnostics;
using System.IO;//File
using System.Linq;//from XX select XX
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Timers;//Timer
using TMPro;//InputField
using UnityEngine;//Mono
using UnityEngine.Tilemaps;
using UnityEngine.UI;//Image
using UnityEngine.Video;//Vedio
using static CMKZ.LocalStorage;
using static UnityEngine.Object;//Destory
using static UnityEngine.RectTransform;

namespace CMKZ {
    public static partial class LocalStorage {
        public static void SetOutLine(this GameObject X, Color Y = default, float 边框宽度 = 2) {
            X.SetOutLineWithBlur(Y, 边框宽度, 1, Mix: true);
        }
        public static void RemoveOutLine(this GameObject X) {
            X.GetComponent<Image>().material = null;
        }
        public static GameObject SetOutLineWithBlur_White(this GameObject X, bool Mix = false) {
            X.SetOutLineWithBlur(new Color(1, 1, 1, 1), 1, 5, Mix);
            return X;
        }
        public static void SetOutLineWithBlur(this GameObject X, Color 边框颜色 = default, float 边框宽度 = 2, float 模糊度 = 5, bool Mix = false) {
            if (边框颜色 == default) {
                边框颜色 = Color.yellow;
            }
            X.GetOrAddComponent<ShaderOutLine>().Color = 边框颜色;
            X.GetComponent<ShaderOutLine>().边框宽度 = 边框宽度;
            X.GetComponent<ShaderOutLine>().模糊度 = 模糊度;
            X.GetComponent<ShaderOutLine>().Mix = Mix;
            X.GetComponent<ShaderOutLine>().Active();
        }
        public static GameObject 设置模糊(this GameObject X, float size) {
            X.SetOutLineWithBlur(new Color(1, 1, 1, 0), 0, size, true);
            return X;
        }
        public static GameObject 设置本体模糊(this GameObject X, float size) {
            X.SetOutLineWithBlur(new Color(1, 1, 1, 0), 0, size, true);
            return X;
        }
        public static void 模糊Demo() {
            MainPanel.创建矩形("0 0 100 100").SetColor(255, 255, 255);
            MainPanel.创建矩形("50 50 100 100").SetColor(255, 0, 0, 0.5f).允许拖动().SetOutLineWithBlur_White(true);

            MainPanel.创建矩形("200 200 100 100").SetColor(255, 255, 255);
            MainPanel.创建矩形("250 250 100 100").SetColor(255, 0, 0, 0.5f).允许拖动();
        }
    }
    public class ShaderOutLine : MonoBehaviour {
        public Color Color = new(1, 1, 1, 1);
        public float 边框宽度 = 1;
        public float 模糊度 = 5;
        public bool Mix = false;
        public void Start() {
            
        }
        public void OnValidate() {
            Active();
        }
        //当焦点变化时刷新，避免Unity bug
        public void OnApplicationFocus(bool focus) {
            //Active();
        }
        public void Active() {
            if (GetComponent<Image>() == null) {
                gameObject.AddComponent<Image>().color = new Color(1, 1, 1, 0.1f);//白色
            }
            GetComponent<Image>().material = new Material(Shader.Find("Custom/模糊与描边3"));
            GetComponent<Image>().material.SetColor("BorderC", Color);
            GetComponent<Image>().material.SetColor("SelfColor", gameObject.GetComponent<Image>().color);
            GetComponent<Image>().material.SetFloat("Size", 模糊度);
            GetComponent<Image>().material.SetFloat("BorderW", 边框宽度);
            GetComponent<Image>().material.SetInt("Mix", Mix ? 1 : 0);
            GetComponent<Image>().material.SetVector("Rect", new Vector2(GetComponent<RectTransform>().rect.width, GetComponent<RectTransform>().rect.height));
            if (GetComponent<Image>().sprite != null) {
                //GetComponent<Image>().material.SetTexture("_MainTex", GetComponent<Image>().sprite.texture);
            }
        }
        //当尺寸变化时刷新，重新计算边框
        public void OnRectTransformDimensionsChange() {
            GetComponent<Image>().material.SetVector("Rect", new Vector2(GetComponent<RectTransform>().rect.width, GetComponent<RectTransform>().rect.height));
        }
    }
}