using Newtonsoft.Json;//Json
using System;//Action
using System.IO;//File
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;//List
using System.Diagnostics;
using System.Linq;//from XX select XX
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Timers;//Timer
using TMPro;//InputField
using UnityEngine;//Mono
using UnityEngine.UI;//Image
using UnityEngine.Tilemaps;
using UnityEngine.Video;//Vedio
using static UnityEngine.Object;//Destory
using static CMKZ.LocalStorage;

namespace CMKZ {
    public static partial class LocalStorage {
        public static Dictionary<string, Tile> AllTile = new();
        public static Tile CreateTile(string X,float 不透明度, bool Y = false) {

            //Print(不透明度);
            if (!AllTile.ContainsKey(X+ 不透明度)) {
                AllTile[X+ 不透明度] = ScriptableObject.CreateInstance<Tile>();
                AllTile[X+ 不透明度].sprite = AllSprite[X]; 

                // 设置透明度
                Color tileColor = AllTile[X + 不透明度].color;
                tileColor.a = 不透明度;
                AllTile[X + 不透明度].color = tileColor;

                AllTile[X + 不透明度].colliderType = Y ? Tile.ColliderType.Sprite : Tile.ColliderType.None;
            }
               return AllTile[X + 不透明度];
        }
        public static void SetTile(int X, int Y, string Z, float 不透明度 = 1, int W = 0,  bool 碰撞 = false) {
            MainTilemap.SetTile(new Vector3Int(X, Y, W), CreateTile(Z, 不透明度, 碰撞));
        }
        public static void MapOnClick(Action<int, int> X) {
            MainPanel.OnUpdate(() => {
                if (Input.GetMouseButtonUp(0)) {
                    var B = MainTilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                    B.z = 0;
                    var C = MainTilemap.GetTile(B);
                    if (C != null) {
                        X(B.x, B.y);
                    }
                }
            });
        }
        public static GameObject SetFreeSprite(this GameObject X, string Y) {
            X.GetOrAddComponent<SpriteRenderer>().sprite = AllSprite[Y];
            return X;
        }
        public static GameObject SetCollider(this GameObject X) {
            X.AddComponent<Rigidbody2D>().gravityScale = 0;
            X.GetComponent<Rigidbody2D>().freezeRotation = true;
            X.AddComponent<BoxCollider2D>();
            return X;
        }
    }
}