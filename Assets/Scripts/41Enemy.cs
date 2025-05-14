using UnityEngine;//Mono
using static CMKZ.LocalStorage;

namespace CMKZ.风神界 {
    public class 野怪类 : MonoBehaviour, IEntity {
        public float Speed = 2;
        public Number 血量 { get; set; } = 3;
        public const float Z层级 = -10;//玩家-10，摄像机-11，地图0，矿石-1，跳币-2
        public Vector2 目标坐标;
        public 空间组件类 空间组件 = new();
        public void Start() {
            Transform();
            gameObject.AddComponent<SpriteRenderer>().sprite = AllSprite["野猪_前"];
            gameObject.AddComponent<BoxCollider2D>().isTrigger = true;
            gameObject.GetComponent<BoxCollider2D>().size = gameObject.GetComponent<SpriteRenderer>().bounds.size;
        }
        public void FixedUpdate() {
            if (Vector2.Distance(目标坐标, 空间组件.世界坐标) < 0.1f) {
                ResetTarget();
            } else {
                gameObject.GetComponent<SpriteRenderer>().sprite = Mathf.Abs(目标坐标.x - 空间组件.世界坐标.x) > Mathf.Abs(目标坐标.y - 空间组件.世界坐标.y) * 0.5
                    ? 目标坐标.x > 空间组件.世界坐标.x ? AllSprite["野猪_右"] : AllSprite["野猪_左"]
                    : 目标坐标.y > 空间组件.世界坐标.y ? AllSprite["野猪_后"] : AllSprite["野猪_前"];
                空间组件.世界坐标 = Vector2.MoveTowards(空间组件.世界坐标, 目标坐标, Speed * Time.deltaTime);
                Transform();
            }
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
            if (X is Player Y) {
                Y.获得经验(1);
            }
        }
        public 野怪类 ResetTarget() {
            目标坐标 = 空间组件.世界坐标 + new Vector2(Random(-5, 5), Random(-5, 5));
            目标坐标.x = Mathf.Clamp(目标坐标.x, 0, 空间组件.世界.所有地块.Width);
            目标坐标.y = Mathf.Clamp(目标坐标.y, 0, 空间组件.世界.所有地块.Height);
            return this;
        }
        public 野怪类 Transform() {
            gameObject.transform.position = 空间组件.Unity坐标.SetZ(Z层级);
            return this;
        }
    }
}