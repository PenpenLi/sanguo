using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.login {
    [Serializable]
    public class LoginData {
        public string des;
        public string data;
        public int state;
    }
    [Serializable]
    public class LoginDataS {
        //  public string data;
        public int state;
        public LoginDataSuccess data;
    }
    [Serializable]
    public class LoginDataSuccess {
        public int userID;
        public string username;
        public string token;
    }

    //qucik
    [Serializable]
    public class QuickRegisters {
        //  public string data;
        public int state;
        public QuickRegisterSuccess data;
    }
    [Serializable]
    public class QuickRegisterSuccess {
        public string guestName;
        public string psw;
    }

    //中心服 返回
    //{"code":0,"data":{"accountId":3,"logicServer":{"host":"192.168.2.21","port":9001},"token":"d3e8c8f2279f0e1578152926e5d40856"},"dec":"验证成功"}

    [Serializable]
    public class LoginDataCenter {
        public int code;
        public LoginDataCenterAccount data;
        public string dec;
    }
    [Serializable]
    public class LoginDataCenterAccount {
        public ulong accountId;
        public LoginDataCenterAccountIp logicServer;
        public string token;
    }
    [Serializable]
    public class LoginDataCenterAccountIp {
        public int port;
        public string host;
    }

}
