using UnityEngine;//Mono
using static CMKZ.LocalStorage;

namespace CMKZ.风神界 {
    public class 地块类 : MonoBehaviour {
        public string 建筑层;
        public string 土质层;
        public 空间组件类 空间组件 = new();
        public void Start() {
            transform.position = 空间组件.Unity坐标.SetZ(0);
            if (建筑层 != null) {
                var A = gameObject.CreateGameObject("矿物");
                A.AddComponent<SpriteRenderer>().sprite = AllSprite[建筑层];
                A.transform.localPosition = new Vector3(0, 0, -1);
                A.AddComponent<矿石类>();
            }
            var B = gameObject.CreateGameObject("土质");
            B.AddComponent<SpriteRenderer>().sprite = AllSprite[土质层];
            B.transform.localPosition = new Vector3(0, 0, 0);
        }
    }
    public class 矿石类 : MonoBehaviour, IEntity {
        public Number 血量 { get; set; } = 2;
        public void Start() {
            gameObject.AddComponent<BoxCollider2D>().isTrigger = true;
            gameObject.GetComponent<BoxCollider2D>().size = GetComponent<SpriteRenderer>().bounds.size;
        }
        public void OnBeAttacked(I伤害发起者 X, int Y) {
            血量 -= Y;
            PlaySound("挖金属");
            this.闪烁(Color.red);
            if (血量 <= 0) {
                OnDie(X);
            }
        }
        public void OnDie(I伤害发起者 X) {
            Destroy(gameObject);//Todo:导致List引用丢失
        }
    }
}