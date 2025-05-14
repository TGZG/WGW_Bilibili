using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static CMKZ.LocalStorage;

public class 仓库卡牌 : MonoBehaviour {
    public TextMeshProUGUI 名称;
    public TextMeshProUGUI 数字;
    public Image 图片;
    public 仓库卡牌 SetName(string X) {
        名称.text = X;
        return this;
    }
    public 仓库卡牌 SetNumber(double X) {
        数字.text = X.To万单位();
        GetComponentInChildren<TextBackGround>()?.SetNeed();
        return this;
    }
    public 仓库卡牌 SetSprite(string X) {
        图片.sprite = AllSprite[X];
        return this;
    }
}