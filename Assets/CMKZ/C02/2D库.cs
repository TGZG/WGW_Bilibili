using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Reflection;
using Microsoft.CodeAnalysis;
using System.IO;
using System.Linq;
using static CMKZ.LocalStorage;

namespace CMKZ {
    public static partial class LocalStorage {
        public static void 创建地图(int 宽, int 高, string 贴图路径) {
            for (int i = 0; i < 宽; i++) {
                for (int j = 0; j < 高; j++) {
                    SetTile(i, j, 贴图路径);
                }
            }
        }
        public static GameObject 创建物体(this GameObject X, int x, int y, string 贴图路径) {
            var go = new GameObject();
            go.AddComponent<SpriteRenderer>().sprite = AllSprite[贴图路径];
            go.transform.position = new Vector3(x, y, 0);
            go.SetParent(X);
            return go;
        }
        public static GameObject 创建物体(float x, float y, string 贴图路径) {
            var go = new GameObject();
            go.AddComponent<SpriteRenderer>().sprite = AllSprite[贴图路径];
            go.transform.position = new Vector3(x, y, 0);
            return go;
        }
        public static GameObject 设置碰撞箱(this GameObject X, bool Y) {
            X.AddComponent<Rigidbody2D>().gravityScale = 0;
            X.AddComponent<BoxCollider2D>();
            X.设置阻力(3);
            return X;
        }
        //设置物体的WASD移动
        public static GameObject 设置WASD移动(this GameObject X, bool Y) {
            X.GetOrAddComponent<控制插件>().激活();
            return X;
        }
        public static GameObject 设置阻力(this GameObject X, float Y) {
            X.GetComponent<Rigidbody2D>().drag = Y;
            return X;
        }
        public static GameObject 禁用移动(this GameObject X) {
            X.GetOrAddComponent<控制插件>().禁用();
            return X;
        }
        public static GameObject 摄像机跟随(this GameObject X, bool Y) {
            if (Y) {
                Camera.main.transform.SetParent(X.transform);
            } else {
                Camera.main.transform.SetParent(null);
            }
            //把摄像机放到物体的位置
            Camera.main.transform.position = X.transform.position;
            //防止摄像机被物体遮挡
            Camera.main.transform.position += new Vector3(0, 0, -10);
            return X;
        }
        public static GameObject 设置摄像机显示范围(Vector2 左下角, Vector2 右上角) { 
            Camera.main.orthographicSize = (右上角.y - 左下角.y) / 2;
            Camera.main.transform.position = new Vector3((右上角.x + 左下角.x) / 2, (右上角.y + 左下角.y) / 2, -10);
            return Camera.main.gameObject;
        }
        public static GameObject 显示FPS() {
            PanelConfig FPS样式 = new PanelConfig() {
                矩形模式 = 矩形模式.固定文本框,
                Position = "0 100%-30 130 30",
                Font = "基督山伯爵",
                TextSize = 20,
                TextAlign = TextAlignmentOptions.Left,
                Margin = new Vector4(10, 0, 0, 0),
            };
            var FPS物体 = MainPanel.创建矩形(FPS样式);
            var A = FPS物体.GetComponentInChildren<TextMeshProUGUI>();
            计时执行 计时 = new 计时执行(0.2f);
            计时.行为 = () => {
                A.text = $"FPS:{(int)((1 / Time.deltaTime) * Time.timeScale)}";
            };
            MainPanel.OnUpdate(() => {
                计时.TryToExecute();
            });
            return FPS物体;
        }
        public static GameObject 锁定方向(this GameObject X, bool Y) {
            X.GetOrAddComponent<Rigidbody2D>().constraints = X.GetOrAddComponent<Rigidbody2D>().constraints | RigidbodyConstraints2D.FreezeRotation;
            return X;
        }
        public static GameObject 锁定位置(this GameObject X, bool Y) {
            X.GetOrAddComponent<Rigidbody2D>().constraints = X.GetOrAddComponent<Rigidbody2D>().constraints | RigidbodyConstraints2D.FreezePosition;
            return X;
        }
        public static GameObject 显示文本框(this GameObject X, string Y) {
            // 创建一个新的文本框游戏对象
            GameObject textBox = new GameObject("TextBox");

            // 将新创建的文本框设为X的子对象
            textBox.transform.SetParent(X.transform);

            // 添加文本组件
            Text textComponent = textBox.AddComponent<Text>();

            // 设置文本内容
            textComponent.text = Y;

            return textBox;
        }
        public static GameObject 设置动力(this GameObject X,float  Y) {
            X.GetComponent<控制插件>().移动力 = Y;
            return X;
        }

    }
    public class 控制插件 : MonoBehaviour {
        public bool Enable = false;
        public float 移动力 = 5;
        private Rigidbody2D 刚体;
        void Start() {
            刚体 = GetComponent<Rigidbody2D>();
        }
        public void Update() {
            if (Enable) {
                if (Input.GetKey(KeyCode.W)) {
                    刚体.AddForce(Camera.main.transform.up * 移动力, ForceMode2D.Force);
                }
                if (Input.GetKey(KeyCode.S)) {
                    刚体.AddForce(-Camera.main.transform.up * 移动力, ForceMode2D.Force);
                }
                if (Input.GetKey(KeyCode.A)) {
                    刚体.AddForce(-Camera.main.transform.right * 移动力, ForceMode2D.Force);
                }
                if (Input.GetKey(KeyCode.D)) {
                    刚体.AddForce(Camera.main.transform.right * 移动力, ForceMode2D.Force);
                }
            }
        }
        public void 激活() {
            Enable = true;
        }
        public void 禁用() {
            Enable = false;
        }
    }

    public class 计时执行 {
        public 限数 计时缓存;
        public Action 行为;
        public 计时执行(float 时间) {
            计时缓存 = new 限数(时间);
        }
        public void TryToExecute() {
            if (计时缓存.已满) {
                行为();
                计时缓存.SetToMin();
            }
            计时缓存.增加(Time.deltaTime);
        }
    }
    public static partial class LocalStorage {
        public static GameObject 设置UI跟随(this GameObject 世界物体, GameObject UI物体) {
            世界物体.设置UI联动(UI物体, (世界物体, UI物体) => {
                //使用recttransform设置UI物体的位置
                UI物体.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(世界物体.transform.position);
            });
            return 世界物体;
        }
        public static GameObject 设置UI联动(this GameObject 世界物体, GameObject UI物体, Action<GameObject, GameObject> 联动事件) {
            var A = 世界物体.AddComponent<UI联动插件>();
            A.UI物体 = UI物体;
            A.联动事件 += 联动事件;
            return 世界物体;
        }
    }
    public class UI联动插件 : MonoBehaviour {
        public GameObject UI物体;
        public event Action<GameObject, GameObject> 联动事件;
        public void Update() {
            联动事件?.Invoke(gameObject, UI物体);
        }
    }
}