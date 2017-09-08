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

public class BattleHandler : MonoBehaviour, IResponseHandler {

    //Queue<UnityAction> actions = new Queue<UnityAction>();
    public void Update() {
        //if (actions.Count > 0) {
        //    actions.Dequeue().DynamicInvoke();
        //}
    }

    public void Handle(int cmd, byte[] data) {
        switch (cmd) {
            case MessageConst.Battle.RESP_GAME_INIT://初始化游戏
                RespGameInit respGameInit = ProtobufTool.DeSerialize<RespGameInit>(data);
                BattleStatus.INSTANCE.respGameInit = respGameInit;
                PlayerManager.self.statusManager.Switch(BattleStatus.INSTANCE);
                break;
            case MessageConst.Battle.RESP_MOVE_CHESS:
                RespMoveChess respMoveChess = ProtobufTool.DeSerialize<RespMoveChess>(data);
                break;
            case MessageConst.Battle.RESP_CURRENT_GO_INFO:
                RespCurrentGoInfo respCurrentGoInfo = ProtobufTool.DeSerialize<RespCurrentGoInfo>(data);
                break;
            default:
                break;
        }
    }

}
