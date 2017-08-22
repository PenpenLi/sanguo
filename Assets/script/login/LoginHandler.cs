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

public class LoginHandler : MonoBehaviour, IResponseHandler {

    Queue<UnityAction> actions = new Queue<UnityAction>();

    public void Update() {
        if (actions.Count > 0) {
            actions.Dequeue().DynamicInvoke();
        }
    }

    private void LoginSuccess(EnterGame enterGame) {
        PopupManager.ShowTimerPopUp(GameTipsDic.GetTips(GameResultEnum.LOGIN_SUCCESS));
        SaveUser();
        SceneManager.LoadScene("scene/main");
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
            case MessageCmdEnum.LOGIN_RESP_CREATE_ROLE:
                //CreateRole createRole = ProtobufTool.DeSerialize<CreateRole>(data);
                actions.Enqueue(CreateRole);
                break;
            case MessageCmdEnum.LOGIN_RESP_ENTER_GAME:
                EnterGame enterGame = ProtobufTool.DeSerialize<EnterGame>(data);
                LoginSuccess(enterGame);
                break;
            default:
                break;
        }
    }

}
