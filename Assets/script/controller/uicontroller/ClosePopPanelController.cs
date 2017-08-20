using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// 需要手动关闭类型弹出框控制脚本
/// </summary>
public class ClosePopPanelController : MonoBehaviour {

    public Text message;
    public Button closeBut;
    // Use this for initialization
    void Start() {
        //登录按钮增加事件
        closeBut.onClick.AddListener(OnClose);
    }

    // Update is called once per frame
    void Update() {

    }

    private void OnClose() {
        Destroy(gameObject);
    }
}
