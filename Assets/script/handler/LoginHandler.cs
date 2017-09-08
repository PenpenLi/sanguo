using UnityEngine;
using System.Collections;
using System;
using Assets.Scripts.net;
using org.alan.chess.proto;
using Assets.Scripts.manager;
using Assets.Scripts.tool;
using org.alan;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Assets.script.constant;
using Assets.script.manager;

public class LoginHandler : MonoBehaviour, IResponseHandler {

    Queue<UnityAction> actions = new Queue<UnityAction>();
    private void Awake() {
        MessageDispatcher.RegisterHandler(MessageConst.Login.TYPE, this);
    }

    public void Update() {
        if (actions.Count > 0) {
            actions.Dequeue().DynamicInvoke();
        }
    }

    private void OnDestroy() {
        MessageDispatcher.RemoveHandler(MessageConst.Login.TYPE);
    }

    private void LoginSuccess() {
        SaveUser();
        PopupManager.ShowTimerPopUp(GameTipsDic.GetTips(GameEnum.LOGIN_SUCCESS));
        StartCoroutine(SceneTool.LoadScene("scene/main"));
    }

    private void CreateRole() {
        //StartCoroutine(SceneTool.LoadScene("scene/create_role"));
        SceneManager.LoadScene("scene/create_role");
    }

    private void SaveUser() {
        UserMeta userMeta = new UserMeta {
            userName = PlayerPrefs.GetString("userName"),
            password = PlayerPrefs.GetString("password")
        };
        ApplicationManager.SaveUserInfo(userMeta);
    }

    public void Handle(int cmd, byte[] data) {
        switch (cmd) {
            case MessageConst.Login.RESP_CREATE_ROLE:
                //CreateRole createRole = ProtobufTool.DeSerialize<CreateRole>(data);
                actions.Enqueue(CreateRole);
                break;
            case MessageConst.Login.RESP_ENTER_GAME:
                EnterGame enterGame = ProtobufTool.DeSerialize<EnterGame>(data);
                PlayerManager.self.player = enterGame.player;
                PlayerManager.self.statusManager = new PlayerStatusManager();
                actions.Enqueue(LoginSuccess);
                break;
            default:
                break;
        }
    }

}
