using Microsoft.CodeAnalysis;
using Newtonsoft.Json;//Json
using Steamworks;
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
        [初始化函数(typeof(CMKZProject), 初始化优先级.暂时废弃)]
        public static bool InitSteam() {
            new GameObject("Steam").AddComponent<SteamManager>();
            设置Steam账号状态("在线");
            每帧(() => {
                SteamAPI.RunCallbacks();
            });
            return SteamManager.Initialized;
        }
        public static void 检测Steam() {
            if (!SteamManager.Initialized) {
                Alert("Steam API初始化失败。确保Steam正在运行，并且应用已通过Steam启动。").SetYes(Application.Quit);
                throw new Exception("Steam API初始化失败");
            }
        }
        public static void 打开玩家资料(CSteamID ID) {
            打开玩家资料(ID.m_SteamID);
        }
        public static void 打开玩家资料(ulong ID) {
            检测Steam();
            //var id = 获取SteamID();
            string profileURL = "https://steamcommunity.com/profiles/" + ID.ToString();
            SteamFriends.ActivateGameOverlayToWebPage(profileURL);
        }
        public static CSteamID 获取SteamID() {
            检测Steam();
            CSteamID steamID = SteamUser.GetSteamID();
            PrintSystem($"已获得玩家SteamID: {steamID}");
            return steamID;
        }
        public static string 获取Steam昵称(ulong? ID = null) {
            检测Steam();
            if (ID == null) {
                return SteamFriends.GetPersonaName();
            }
            return SteamFriends.GetFriendPersonaName(new CSteamID() { m_SteamID = (ulong)ID });
        }
        public static Sprite 获取Steam头像(CSteamID steamID) {
            检测Steam();
            int avatarInt = SteamFriends.GetLargeFriendAvatar(steamID);
            if (!SteamUtils.GetImageSize(avatarInt, out uint 宽, out uint 高)) {
                Print($"玩家{steamID}的头像加载失败");
                return null;
            }
            byte[] 数据流 = new byte[宽 * 高 * 4];
            if (!SteamUtils.GetImageRGBA(avatarInt, 数据流, (int)(宽 * 高 * 4))) {
                Print($"玩家{steamID}的头像加载失败");
                return null;
            }
            Texture2D texture = new((int)宽, (int)高, TextureFormat.RGBA32, false);
            texture.LoadRawTextureData(数据流);
            //上下翻转
            for (int i = 0; i < texture.height / 2; i++) {
                for (int j = 0; j < texture.width; j++) {
                    Color temp = texture.GetPixel(j, i);
                    texture.SetPixel(j, i, texture.GetPixel(j, texture.height - i - 1));
                    texture.SetPixel(j, texture.height - i - 1, temp);
                }
            }
            texture.Apply();
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
        public static void 解锁Steam成就(string 成就名) {
            检测Steam();
            SteamUserStats.SetAchievement(成就名);
            SteamUserStats.StoreStats();//同步到服务器
        }
        public static void 设置成就进度(string 成就名, int 当前值) {
            检测Steam();
            SteamUserStats.SetStat(成就名, 当前值);
            SteamUserStats.StoreStats();
        }
        public static void 重置所有成就() {
            检测Steam();
            foreach (string i in 所有Steam成就().Keys) {
                SteamUserStats.ClearAchievement(i);
            }
            SteamUserStats.StoreStats();
        }
        public static KeyValueList<string, bool> 所有Steam成就() {
            检测Steam();
            var A = new KeyValueList<string, bool>();
            for (uint i = 0; i < SteamUserStats.GetNumAchievements(); i++) {
                string B = SteamUserStats.GetAchievementName(i);
                SteamUserStats.GetAchievement(B, out bool C);
                A.Add(B, C);
            }
            return A;
        }
        public static void 设置Steam账号状态(string 状态) {
            检测Steam();
            SteamFriends.SetRichPresence("status", 状态);
        }
    }
}