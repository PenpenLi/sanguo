using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

/// <summary>
/// 需要手动关闭类型弹出框控制脚本
/// </summary>
public class TimePopPanelController : MonoBehaviour {

    public Text message;
    public int showSecond =2;
    private DateTime beginTime;
    // Use this for initialization
    void Start() {
        beginTime = System.DateTime.Now;
    }

    // Update is called once per frame
    void Update() {
        DateTime currentTime = System.DateTime.Now;
        int diff = currentTime.Second - beginTime.Second;
        if (diff >= showSecond) {
            OnClose();
        } else {
            //Image image = gameObject.GetComponent<Image>();
            //image.color.a = (image.color.a*(showSecond - diff) / showSecond);
            //image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a * (showSecond - diff) / showSecond);
        }
    }

    private void OnClose() {
        Destroy(this.gameObject);
    }
}
