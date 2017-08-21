using Assets.Scripts.manager;
using Assets.Scripts.net;
using org.alan.chess.proto;
using UnityEngine;

public class GameTipsController : MonoBehaviour, IResponseHandler {

    // Use this for initialization
    void Start() {
        //注册登录消息处理
        MessageDispatcher.RegisterHandler(MessageTypeEnum.TIPS, this);
    }

    // Update is called once per frame
    void Update() {

    }

    private void OnDestroy() {
        MessageDispatcher.RemoveHandler(MessageTypeEnum.TIPS);
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
            case MessageCmdEnum.TIPS_RESP_RESULT:
                GameTips gameTips = ProtobufTool.DeSerialize<GameTips>(data);
                PopupTips(gameTips);
                break;
            default:
                break;
        }
    }

}
