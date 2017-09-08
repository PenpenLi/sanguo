using Assets.script.constant;
using Assets.Scripts.manager;
using Assets.Scripts.net;
using org.alan.chess.proto;
using UnityEngine;

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
            messageType = MessageConst.Login.TYPE,
            cmd = MessageConst.Login.REQ_VERTIFY,
            data = ProtobufTool.Serialize(vertifyUserInfo)
        };
        clientSocket.Send(marsMessage);
    }
    static public void SendCreateRole(string roleName) {
        VertifyUserInfo _vertifyUserInfo = new VertifyUserInfo() {
            token = PlayerManager.self.loginDataCenter.data.token,
            userId = (long)PlayerManager.self.loginDataCenter.data.accountId,
            zoneId = 1
        };
        ReqCreateRole reqCreateRole = new ReqCreateRole() {
            vertifyUserInfo = _vertifyUserInfo,
            name = roleName
        };
        MarsMessage marsMessage = new MarsMessage() {
            messageType = MessageConst.Login.TYPE,
            cmd = MessageConst.Login.REQ_CREATE_ROLE,
            data = ProtobufTool.Serialize(reqCreateRole)
        };
        clientSocket.Send(marsMessage);
    }
    static public void QuickMatch(int roomType) {
        ReqCreateRoom createRoom = new ReqCreateRoom {
            roomType = roomType
        };
        MarsMessage marsMessage = new MarsMessage() {
            messageType = MessageConst.Room.TYPE,
            cmd = MessageConst.Room.REQ_QUICK_MATCH,
            data = ProtobufTool.Serialize(createRoom)
        };
        clientSocket.Send(marsMessage);

    }
    static public void CancelMatch() {
        MarsMessage marsMessage = new MarsMessage() {
            messageType = MessageConst.Room.TYPE,
            cmd = MessageConst.Room.REQ_CANEL_MATCH
        };
        clientSocket.Send(marsMessage);

    }
    static public void SendGameInitDone() {
        MarsMessage marsMessage = new MarsMessage() {
            messageType = MessageConst.Battle.TYPE,
            cmd = MessageConst.Battle.REQ_GAME_INIT_DONE
        };
        clientSocket.Send(marsMessage);
    }
    static public void SendMoveChess(MoveChess moveChess) {
        MarsMessage marsMessage = new MarsMessage() {
            messageType = MessageConst.Battle.TYPE,
            cmd = MessageConst.Battle.REQ_MOVE_CHESS,
            data = ProtobufTool.Serialize(moveChess)

        };
        clientSocket.Send(marsMessage);
    }
}

