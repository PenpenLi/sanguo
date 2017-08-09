using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class SkillCanvasController : MonoBehaviour {

    public Image cardImage;
    public Button cardName;
    public Text skillName;
    public Text skillDesc;
    public int showSecond = 1;
    private int beginTime;
    private int countDown;

    private Canvas canvas;

    // Use this for initialization
    void Start() {
        canvas = gameObject.GetComponent<Canvas>();
    }

    // Update is called once per frame
    void Update() {
        if (canvas.enabled) {
            //int diff = TimeUtils.CurrentGMTSeconds() - beginTime;
            if (countDown-- < 0) {
                canvas.enabled = false;
            }
        }
    }

    public SkillCanvasController Reset(Sprite sprite) {
        cardImage.sprite = sprite;
        return this;
    }


    public void Show() {
        canvas.enabled = true;
        countDown = 50;
        //beginTime = TimeUtils.CurrentGMTSeconds();
    }
}
