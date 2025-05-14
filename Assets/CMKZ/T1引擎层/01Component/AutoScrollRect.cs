using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AutoScrollRect : MonoBehaviour {
    public RectTransform contentRect;
    public TextMeshProUGUI contentText;
    public RectTransform viewportRect;

    void Update() {
        // 获取文本内容的实际高度
        float contentHeight = LayoutUtility.GetPreferredHeight(contentText.rectTransform);

        // 如果文本内容高度大于视口高度，则允许滚动，否则调整内容高度为视口高度
        if (contentHeight > viewportRect.rect.height) {
            contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, contentHeight);
        } else {
            contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, viewportRect.rect.height);
        }
    }
}