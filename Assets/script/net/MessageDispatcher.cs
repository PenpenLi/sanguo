using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.net.responses;
using System;

public struct MessageType {
    public int messageType;
    public int cmd;

    public MessageType(int messageType, int cmd) {
        this.messageType = messageType;
        this.cmd = cmd;
    }
}

public class MessageDispatcher {
    public Dictionary<uint, IResponse> ResponseDic = new Dictionary<uint, IResponse>();

    public Dictionary<Type, MessageType> RequestTypeDic = new Dictionary<Type, MessageType>();

    public void Receive(MarsMessage marsMsg) {

    }

    public MarsMessage GetMessage(object msg) {
        MessageType mt = RequestTypeDic[msg.GetType()];
        MarsMessage marsMessage = new MarsMessage() {
            messageType = mt.messageType,
            cmd = mt.cmd,
            data = ProtobufTool.Serialize(msg)
        };
        return marsMessage;
    }

}
