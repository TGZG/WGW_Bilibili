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
using System.Threading.Tasks;
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
        public static SteamInventoryResult_t 获取物品请求信道() {
            if (SteamInventory.GetAllItems(out SteamInventoryResult_t _请求信息)) {
                return _请求信息;
            } else {
                throw new Exception("获取物品请求信道失败");
            }
        }
        public static 物品信息[] 获取所有物品Async(SteamInventoryResult_t 请求信道) {
            uint 物品数量 = 0;
            SteamInventory.GetResultItems(请求信道, null, ref 物品数量);
            SteamItemDetails_t[] 物品数组 = new SteamItemDetails_t[物品数量];
            SteamInventory.GetResultItems(请求信道, 物品数组, ref 物品数量);
            //foreach (var 物品 in 物品数组) {
            //    string 物品名称 = 获取物品名称(物品.m_iDefinition);
            //    Print($"物品名称: {物品名称}, 物品ID: {物品.m_itemId}, 数量: {物品.m_unQuantity}");
            //}
            SteamInventory.DestroyResult(请求信道);

            return 物品数组.Select(物品 => new 物品信息 {
                名称 = 获取物品名称(物品.m_iDefinition),
                物品ID = 物品.m_itemId,
                数量 = 物品.m_unQuantity
            }).ToArray();
        }
        public struct 物品信息 {
            public string 名称;
            public SteamItemInstanceID_t 物品ID;
            public uint 数量;
        }

        private static string 获取物品名称(SteamItemDef_t 定义ID) {
            string 物品名称 = "未知物品";
            uint 缓冲区大小 = 256; // 根据预期的物品名称长度调整
            if (SteamInventory.GetItemDefinitionProperty(定义ID, "name", out string 名称, ref 缓冲区大小)) {
                物品名称 = 名称;
            }
            return 物品名称;
        }

        public static async Task<云端物品类[]> 获取所有可用物品价格() {
            TaskCompletionSource<云端物品类[]> tcs = new TaskCompletionSource<云端物品类[]>();
            // 发起价格请求

            CallResult<SteamInventoryRequestPricesResult_t> 价格请求 = new CallResult<SteamInventoryRequestPricesResult_t>((result, biofailure) => {
                if (biofailure || result.m_result != EResult.k_EResultOK) {
                    tcs.SetException(new Exception("请求价格失败"));
                    return;
                }
                Print("请求价格成功");
                tcs.SetResult(获取物品价格());
            });
            价格请求.Set(SteamInventory.RequestPrices());
            return await tcs.Task;
        }
        public static 云端物品类[] 获取物品价格() {
            uint numItemsWithPrices = SteamInventory.GetNumItemsWithPrices();
            Print("物品数量: " + numItemsWithPrices);

            if (numItemsWithPrices > 0) {
                SteamItemDef_t[] itemDefs = new SteamItemDef_t[numItemsWithPrices];
                ulong[] itemPrices = new ulong[numItemsWithPrices];
                ulong[] itemDiscounts = new ulong[numItemsWithPrices];
                uint itemCount = numItemsWithPrices;

                // 获取物品详细信息
                SteamInventory.GetItemsWithPrices(itemDefs, itemPrices, itemDiscounts, itemCount);
                Print("获取物品详细信息成功");

                for (int i = 0; i < itemCount; i++) {
                    Print($"物品定义ID: {itemDefs[i]}, 价格: {itemPrices[i]}, 折扣: {itemDiscounts[i]}");
                }
                return itemDefs.Select((itemDef, index) => new 云端物品类 {
                    物品ID = itemDef,
                    价格 = itemPrices[index],
                    折扣 = itemDiscounts[index]
                }).ToArray();
            }
            return null;
        }
        public struct 云端物品类 {
            public SteamItemDef_t 物品ID;
            public ulong 价格;
            public ulong 折扣;
        }
    }

}