using System;//Action
using UnityEngine;//Mono
using static CMKZ.LocalStorage;

namespace CMKZ.风神界 {
    public class Player : MonoBehaviour {
        public 玩家类后台 后台;
        public Dictionary<KeyCode, Action> 按住快捷键 = new();
        public const float Z层级 = -10;//玩家-10，摄像机-11，地图0，矿石-1，跳币-2
        public void Start() {
            Transform();
            gameObject.SetCameraFollow();
            gameObject.AddComponent<SpriteRenderer>().sprite = AllSprite["玩家_下"];
            按住快捷键[KeyCode.W] += () => GetComponent<SpriteRenderer>().sprite = AllSprite["玩家_上"];
            按住快捷键[KeyCode.S] += () => GetComponent<SpriteRenderer>().sprite = AllSprite["玩家_下"];
            按住快捷键[KeyCode.A] += () => GetComponent<SpriteRenderer>().sprite = AllSprite["玩家_左"];
            按住快捷键[KeyCode.D] += () => GetComponent<SpriteRenderer>().sprite = AllSprite["玩家_右"];
            后台.On获得经验 += On获得经验;
        }
        public void Update() {
            if (Input.GetMouseButtonDown(0)) {
                var A = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (A.collider != null) {
                    A.collider.gameObject.GetComponent<IEntity>()?.BeAttacked(后台, 后台.攻击力);
                }
            }
        }
        public void FixedUpdate() {
            foreach (var i in 按住快捷键) {
                if (Input.GetKey(i.Key)) {
                    i.Value();
                }
            }
            Transform();
        }
        public void On获得经验(object X) {
            Alert($"升级！\n攻击力：{后台.攻击力 - 1} → {后台.攻击力}");
        }
        public void OnDestroy() {
            后台.On获得经验 -= On获得经验;
        }
        public Player Transform() {
            gameObject.transform.position = 后台.空间组件.Unity坐标.SetZ(Z层级);
            return this;
        }
    }
}