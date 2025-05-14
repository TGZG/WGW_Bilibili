using UnityEngine;

public class TextBackGround : MonoBehaviour {
    public GameObject Text;
    public bool Need刷新 = true;
    public void Update() {
        if (Need刷新) {
            GetComponent<RectTransform>().sizeDelta = Text.GetComponent<RectTransform>().sizeDelta + new Vector2(4, 3);
            Need刷新 = false;
        }
    }
    public void SetNeed() => Need刷新 = true;
}