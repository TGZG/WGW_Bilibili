using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis.Scripting;
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
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UI;//Image
using UnityEngine.Video;//Vedio
using static CMKZ.LocalStorage;
using static UnityEngine.Object;//Destory
using static UnityEngine.RectTransform;

namespace CMKZ {
    public static partial class LocalStorage {
        public static Dictionary<string, GameObject> AllSoundObject = new();
        public static GameObject MusicObject;
        public static void PlaySound(string X) {
            if (!AllSoundObject.ContainsKey(X)) {
                AllSoundObject[X] = new GameObject("音效");
                AllSoundObject[X].AddComponent<AudioSource>();
            }
            AllSoundObject[X].GetComponent<AudioSource>().clip = AllSound[X];
            AllSoundObject[X].GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("音效音量", 0.5f);
            AllSoundObject[X].GetComponent<AudioSource>().Play();
        }
        public static void PlayMusic(string X) {
            if (MusicObject == null) {
                MusicObject = new GameObject("音乐");
                MusicObject.AddComponent<MusicManager>();
            }
            MusicObject.GetComponent<AudioSource>().clip = AllSound[X];
            MusicObject.GetComponent<AudioSource>().loop = true;
            MusicObject.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("音乐音量", 0.5f);
            MusicObject.GetComponent<AudioSource>().Play();
        }
        public static void PlayAllMusic(params string[] X) {
            if (MusicObject == null) {
                MusicObject = new GameObject("音乐");
                MusicObject.AddComponent<MusicManager>();
            }
            MusicObject.GetComponent<MusicManager>().Resources = X.Select(Y => AllSound[Y]).ToArray();
            MusicObject.GetComponent<AudioSource>().clip = X.Length > 0 ? AllSound[X[0]] : null;
            MusicObject.GetComponent<AudioSource>().loop = false;
            MusicObject.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("音乐音量", 0.5f);
            MusicObject.GetComponent<AudioSource>().Play();
        }
        /// <summary>
        /// 从0到1
        /// </summary>
        public static void SetMusicVolume(float X) {
            MusicObject.GetComponent<AudioSource>().volume = X;
            PlayerPrefs.SetFloat("音乐音量", X);
        }
        /// <summary>
        /// 从0到1
        /// </summary>
        public static void SetSoundVolume(float X) {
            foreach (var Y in AllSoundObject.Values) {
                Y.GetComponent<AudioSource>().volume = X;
            }
            PlayerPrefs.SetFloat("音效音量", X);
        }
    }
    [RequireComponent(typeof(AudioSource))]
    public class MusicManager : MonoBehaviour { //挂载在MusicObject上
        public AudioClip[] Resources;
        public int Index;
        public void Update() {
            if (!GetComponent<AudioSource>().isPlaying && Resources.Length != 0) {
                if (Index >= Resources.Length) {
                    Index = 0;
                }
                GetComponent<AudioSource>().clip = Resources[Index];
                GetComponent<AudioSource>().Play();
                Index++;
            }
        }
    }
}