using UnityEngine;//Mono
using static CMKZ.LocalStorage;

namespace CMKZ.风神界 {
    public class 野怪类 : MonoBehaviour {
        public 野怪类后台 后台;
        public const float Z层级 = -10;//玩家-10，摄像机-11，地图0，矿石-1，跳币-2
        public void Start() {
            Transform();
            gameObject.AddComponent<SpriteRenderer>().sprite = AllSprite["野猪_前"];
            gameObject.AddComponent<BoxCollider2D>().isTrigger = true;
            gameObject.GetComponent<BoxCollider2D>().size = gameObject.GetComponent<SpriteRenderer>().bounds.size;
            后台.OnDie += OnDie;
            后台.OnBeAttacked += OnBeAttacked;
        }
        public void FixedUpdate() {
            gameObject.GetComponent<SpriteRenderer>().sprite = Mathf.Abs(后台.目标坐标.x - 后台.空间组件.世界坐标.x) > Mathf.Abs(后台.目标坐标.y - 后台.空间组件.世界坐标.y) * 0.5
                ? 后台.目标坐标.x > 后台.空间组件.世界坐标.x ? AllSprite["野猪_右"] : AllSprite["野猪_左"]
                : 后台.目标坐标.y > 后台.空间组件.世界坐标.y ? AllSprite["野猪_后"] : AllSprite["野猪_前"];
            Transform();
        }
        public void OnBeAttacked(object X) {
            PlaySound("挖金属");
            this.闪烁(Color.red);
        }
        public void OnDie(object X) {
            Destroy(gameObject);//Todo:导致List引用丢失
        }
        public 野怪类 Transform() {
            gameObject.transform.position = 后台.空间组件.Unity坐标.SetZ(Z层级);
            return this;
        }
    }
}