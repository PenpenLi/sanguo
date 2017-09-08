using Assets.script.constant;
using Assets.Scripts.manager;
using Assets.Scripts.net;
using org.alan.chess.proto;
using System.Collections.Generic;
using UnityEngine;

public class GameTipsController : MonoBehaviour, IResponseHandler {

    public Queue<GameTips> message = new Queue<GameTips>();

    // Use this for initialization
    void Start() {
        //注册登录消息处理
        MessageDispatcher.RegisterHandler(MessageConst.Tips.TYPE, this);
    }

    // Update is called once per frame
    void Update() {
        if (message.Count > 0) {
            GameTips gameTips = message.Dequeue();
            PopupTips(gameTips);
        }
    }

    private void OnDestroy() {
        MessageDispatcher.RemoveHandler(MessageConst.Tips.TYPE);
    }

    public void PopupTips(GameTips gameTips) {
        GameResultEnum code = (GameResultEnum)gameTips.resultCode;
        string tips = GameTipsDic.GetTips(code);
        tips = tips ?? gameTips.resultDes;
        if (gameTips.tipsType == 1) {//定时关闭窗口
            PopupManager.ShowTimerPopUp(tips);
        } else if (gameTips.tipsType == 2) {//手动关闭
            PopupManager.ShowClosePopUp(tips);
        }
    }

    public void Handle(int cmd, byte[] data) {
        switch (cmd) {
            case MessageConst.Tips.TIPS_RESP_RESULT:
                GameTips gameTips = ProtobufTool.DeSerialize<GameTips>(data);
                message.Enqueue(gameTips);
                break;
            default:
                break;
        }
    }

}
