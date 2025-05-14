using System;//Action
using UnityEngine;//Mono
using static CMKZ.LocalStorage;

namespace CMKZ.风神界 {
    public class Player : MonoBehaviour, I伤害发起者 {
        public Dictionary<KeyCode, Action> 按住快捷键 = new();
        public float Speed;
        public const float Z层级 = -10;//玩家-10，摄像机-11，地图0，矿石-1，跳币-2
        public Number 攻击力 = 1;
        public Number 经验;
        public void Start() {
            按住快捷键[KeyCode.W] += () => transform.position += new Vector3(0, Speed * Time.deltaTime);
            按住快捷键[KeyCode.S] += () => transform.position += new Vector3(0, -Speed * Time.deltaTime);
            按住快捷键[KeyCode.A] += () => transform.position += new Vector3(-Speed * Time.deltaTime, 0);
            按住快捷键[KeyCode.D] += () => transform.position += new Vector3(Speed * Time.deltaTime, 0);
            按住快捷键[KeyCode.W] += () => GetComponent<SpriteRenderer>().sprite = AllSprite["玩家_后"];
            按住快捷键[KeyCode.S] += () => GetComponent<SpriteRenderer>().sprite = AllSprite["玩家_前"];
            按住快捷键[KeyCode.A] += () => GetComponent<SpriteRenderer>().sprite = AllSprite["玩家_左"];
            按住快捷键[KeyCode.D] += () => GetComponent<SpriteRenderer>().sprite = AllSprite["玩家_右"];
            gameObject.transform.position = transform.position.SetZ(Z层级);
            gameObject.AddComponent<SpriteRenderer>().sprite = AllSprite["玩家_前"];
            gameObject.SetCameraFollow();
        }
        public void Update() {
            if (Input.GetMouseButtonDown(0)) {
                var A = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (A.collider != null) {
                    A.collider.gameObject.GetComponent<IEntity>()?.OnBeAttacked(this, 攻击力);
                }
            }
        }
        public void FixedUpdate() {
            Speed = Input.GetKey(KeyCode.LeftShift) ? 10 : 5;
            foreach (var i in 按住快捷键) {
                if (Input.GetKey(i.Key)) {
                    i.Value();
                }
            }
        }
        public void 获得经验(Number X) {
            经验 += X;
            if (经验 >= 2) {
                经验 -= 2;
                攻击力 += 1;
                Alert($"升级！\n攻击力：{攻击力 - 1} → {攻击力}");
            }
        }
    }
}