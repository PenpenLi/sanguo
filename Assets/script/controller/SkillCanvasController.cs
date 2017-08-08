using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class SkillCanvasController : MonoBehaviour {

    public Image cardImage;
    public Button cardName;
    public Text skillName;
    public Text skillDesc;
    public int showSecond = 2;
    private DateTime beginTime;

    private Canvas canvas;

    // Use this for initialization
    void Start() {
        canvas = gameObject.GetComponent<Canvas>();
    }

    // Update is called once per frame
    void Update() {
        if (canvas.enabled) {
            DateTime currentTime = System.DateTime.Now;
            int diff = currentTime.Second - beginTime.Second;
            if (diff > showSecond) {
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
        beginTime = System.DateTime.Now;
    }
}
