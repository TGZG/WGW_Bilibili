using UnityEngine;//Mono
using static CMKZ.LocalStorage;

namespace CMKZ.风神界 {
    public class 地块类前台 : MonoBehaviour {
        public 地块类后台 后台;
        public void Start() {
            transform.position = 后台.空间组件.Unity坐标.SetZ(0);
            if (后台.建筑层 != null) {
                var A = gameObject.CreateGameObject("矿物");
                A.AddComponent<SpriteRenderer>().sprite = AllSprite[后台.建筑层.图片名];
                A.transform.localPosition = new Vector3(0, 0, -1);
                A.AddComponent<树木类>();
            }
            var B = gameObject.CreateGameObject("土质");
            B.AddComponent<SpriteRenderer>().sprite = AllSprite[后台.土质层.土质.ToString()];
            B.transform.localPosition = new Vector3(0, 0, 0);
        }
    }
    public class 树木类 : MonoBehaviour {
        public 树木类后台 后台;
        public void Start() {
            gameObject.AddComponent<BoxCollider2D>().isTrigger = true;
            gameObject.GetComponent<BoxCollider2D>().size = GetComponent<SpriteRenderer>().bounds.size;
            后台.OnBeAttacked += BeAttacked;
            后台.OnDie += Die;
        }
        public void BeAttacked(object X) {
            PlaySound("挖金属");
            this.闪烁(Color.red);
        }
        public void Die(object X) {
            Destroy(gameObject);//Todo:导致List引用丢失
        }
        public void OnDestroy() {
            后台.OnBeAttacked -= BeAttacked;
            后台.OnDie -= Die;
        }
    }
}