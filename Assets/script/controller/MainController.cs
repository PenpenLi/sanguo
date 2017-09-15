using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Assets.Scripts.manager;
using System;
using Assets.script.constant;
using Assets.script.manager;

public class MainController : MonoBehaviour {

    public Button tiantiBut;
    public Button pipeiBut;
    public Button buzhenBut;

    public Text diamondText;
    public Canvas popCanvas;

    private void Awake() {
    }

    // Use this for initialization
    void Start() {
        tiantiBut.onClick.AddListener(OnTianTi);
        pipeiBut.onClick.AddListener(OnPiPei);
        pipeiBut.onClick.AddListener(OnBuZhen);
    }

    void OnTianTi() {
        NetManager.QuickMatch(1);
    }

    void OnPiPei() {
    }
    void OnBuZhen() {
    }
    // Update is called once per frame
    void Update() {
        diamondText.text = Convert.ToString(PlayerManager.self.player.role.diamond);
        CheckPlayerStatus();
    }
    /// <summary>
    /// 检查当前用户状态
    /// </summary>
    void CheckPlayerStatus() {
        PlayerManager.self.statusManager.Update();
    }

    private void OnDestroy() {
    }

    private void OnApplicationQuit() {
        Debug.Log("login OnApplicationQuit");
        NetManager.clientSocket.Close();
    }
}
