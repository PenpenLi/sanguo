using Assets.Scripts.manager;
using Assets.Scripts.net;
using org.alan.chess.proto;

class NetManager {
    /// <summary>
    /// game socket
    /// </summary>
    static public ClientSocket clientSocket;

    public static void CreateScoket() {
        if (clientSocket == null || !clientSocket.socket.Connected) {
            string host = PlayerManager.self.loginDataCenter.data.logicServer.host;
            int port = PlayerManager.self.loginDataCenter.data.logicServer.port;
            NetManager.clientSocket = new ClientSocket(host, port);
        }
    }

    static public void LoginGameServer() {
        CreateScoket();
        VertifyUserInfo vertifyUserInfo = new VertifyUserInfo() {
            token = PlayerManager.self.loginDataCenter.data.token,
            userId = (long)PlayerManager.self.loginDataCenter.data.accountId,
            zoneId = 1
        };
        MarsMessage marsMessage = new MarsMessage() {
            messageType = MessageTypeEnum.LOGIN,
            cmd = MessageCmdEnum.LOGIN_REQ_VERTIFY,
            data = ProtobufTool.Serialize(vertifyUserInfo)
        };
        clientSocket.Send(marsMessage);
    }
}

