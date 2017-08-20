using Assets.Scripts.net;
using System.Collections.Generic;

public static class MessageDispatcher {
    private static Dictionary<int, IResponseHandler> responseDic = new Dictionary<int, IResponseHandler>();

    public static void RegisterHandler(int messageType, IResponseHandler responseHandler) {
        responseDic[messageType] = responseHandler;
    }

    public static void RemoveHandler(int messageType) {
        responseDic.Remove(messageType);
    }
    public static void RemoveHandler(IResponseHandler responseHandler) {
        foreach (var value in responseDic) {
            if (value.Value == responseHandler) {
                responseDic.Remove(value.Key);
            }
        }
    }
    public static void Receive(MarsMessage marsMsg) {
        int messageType = marsMsg.messageType;
        int cmd = marsMsg.cmd;
        IResponseHandler handler = responseDic[messageType];
        if (handler != null) {
            handler.Handle(cmd, marsMsg.data);
        }
    }
}
