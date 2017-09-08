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

public class RoomHandler : MonoBehaviour, IResponseHandler {

    // Queue<UnityAction> actions = new Queue<UnityAction>();
    public void Update() {
        // if (actions.Count > 0) {
        //    actions.Dequeue().DynamicInvoke();
        //}
    }

    public void BeginMatch(long beginTime) {
        MatchStatus.INSTANCE.beginTime = beginTime;
        PlayerManager.self.statusManager.Switch(MatchStatus.INSTANCE);
    }

    public void CancelMatch(GameResultEnum result) {
        if (PlayerManager.self.statusManager.currentStatus.Status() == StatusEnum.MATCH) {
            MatchStatus ms = PlayerManager.self.statusManager.currentStatus as MatchStatus;
            ms.CancelMatch(result);
        }
    }

    public void Handle(int cmd, byte[] data) {
        switch (cmd) {
            case MessageConst.Room.RESP_BEGIN_MATCH:
                RespBeginMatch respBeginMatch = ProtobufTool.DeSerialize<RespBeginMatch>(data);
                BeginMatch(respBeginMatch.beginTime);
                break;
            case MessageConst.Room.RESP_CANEL_MATCH:
                RespCancelMatch respCancelMatch = ProtobufTool.DeSerialize<RespCancelMatch>(data);
                CancelMatch(respCancelMatch.gameResultEnum);
                break;
            default:
                break;
        }
    }

}
