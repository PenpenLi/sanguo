using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Timers;
using Assets.Scripts.manager;
using Assets.script.manager;

public class MatchController : MonoBehaviour {

    public long beginTime;
    public Text countDownText;
    public Button cancelBut;

    public float time;

    // Use this for initialization
    void Start() {
        //登录按钮增加事件
        cancelBut.onClick.AddListener(Cancel);
    }

    // Update is called once per frame
    void Update() {
        time += Time.deltaTime;
        countDownText.text = System.Convert.ToString((int)time);
    }

    void Cancel() {
        NetManager.CancelMatch();
    }
}
