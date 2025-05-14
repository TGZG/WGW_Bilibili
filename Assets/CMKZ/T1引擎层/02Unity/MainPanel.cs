using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using static CMKZ.LocalStorage;
using static UnityEngine.Object;

namespace CMKZ {
    public static partial class LocalStorage {
        public static GameObject _MainPanel;
        public static GameObject MainPanel {
            get {
                if (_MainPanel == null) {
                    var Canvas = GameObject.Find("Canvas");
                    if (Canvas == null) {
                        Canvas = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
                    }
                    _MainPanel = Canvas.创建矩形("0 0 100% 100%");
                    _MainPanel.GetComponentInParent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
                    _MainPanel.SetAsFirst();
                    var EventSystem = GameObject.Find("EventSystem");
                    if (EventSystem == null) {
                        new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule), typeof(BaseInput));
                    }
                    //DontDestroyOnLoad(MainPanel.GetParent());
                }
                return _MainPanel;
            }
        }
        public static GameObject _MainSystem;
        public static GameObject MainSystem {
            get {
                if (_MainSystem == null) {
                    _MainSystem = new GameObject("MainSystem");
                }
                return _MainSystem;
            }
        }
        public static GameObject _MainHidding;
        public static GameObject MainHidding {
            get {
                if (_MainHidding == null) {
                    _MainHidding = new();
                    _MainHidding.SetActive(false);
                }
                return _MainHidding;
            }
        }
        public static Tilemap _MainTilemap;
        public static Tilemap MainTilemap {
            get {
                if (_MainTilemap == null) {
                    _MainTilemap = new GameObject("TileMap", typeof(TilemapRenderer), typeof(TilemapCollider2D)).SetParent(new GameObject("Grid", typeof(Grid))).GetComponent<Tilemap>();
                    _MainTilemap.GetComponent<TilemapCollider2D>().usedByComposite = true;
                    //B.AddComponent<CompositeCollider2D>();
                }
                return _MainTilemap;
            }
        }
        public static Camera MainCamera = Camera.main;
    }
}