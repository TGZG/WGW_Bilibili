//using Microsoft.CodeAnalysis;
//using Microsoft.CodeAnalysis.CSharp;
//using Microsoft.CodeAnalysis.CSharp.Scripting;
//using Microsoft.CodeAnalysis.Emit;
//using Microsoft.CodeAnalysis.Scripting;
//using Newtonsoft.Json;//Json
//using Steamworks;
//using System;//Action
//using System.Collections;
//using System.Collections.Generic;//List
//using System.Diagnostics;
//using System.IO;//File
//using System.Linq;//from XX select XX
//using System.Reflection;
//using System.Runtime.InteropServices;
//using System.Runtime.Serialization.Formatters.Binary;
//using System.Security.Cryptography;
//using System.Text;
//using System.Text.RegularExpressions;
//using System.Timers;//Timer
//using TMPro;//InputField
//using UnityEngine;//Mono
//using UnityEngine.Tilemaps;
//using UnityEngine.UI;//Image
//using UnityEngine.Video;//Vedio
//using static CMKZ.LocalStorage;
//using static UnityEngine.Object;//Destory
//using static UnityEngine.RectTransform;

//namespace CMKZ {
//    public static partial class LocalStorage {
//        public static void UploadWorkshopItem(string contentPath, string title, string description) {
//            // 创建一个新的工坊项目
//            PublishedFileId_t workshopItemId = new PublishedFileId_t(); // 假定这是一个有效的ID
//            SteamAPICall_t call = SteamUGC.CreateItem(AppId_t.Invalid, EWorkshopFileType.k_EWorkshopFileTypeCommunity);
//            call.SetTitle(title);
//            call.SetDescription(description);
//            call.SetContentFolder(contentPath);
//            // 设置其他必要的信息，如标签等
//            // 调用SteamUGC.SubmitItemUpdate提交更新
//        }
//    }
//}