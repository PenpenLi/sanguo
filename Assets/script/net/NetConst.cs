
namespace Assets.Scripts.net {
    /// <summary>
    /// 消息类型枚举定义
    /// </summary>
    class MessageTypeEnum {
        public const int LOGIN = 1000;
        public const int TIPS = 1002;
    }

    class MessageCmdEnum {
        public const int LOGIN_REQ_VERTIFY = 1;
        public const int LOGIN_RESP_CREATE_ROLE = 2;
        public const int LOGIN_REQ_CREATE_ROLE = 3;
        public const int LOGIN_RESP_ENTER_GAME = 4;
        public const int TIPS_RESP_RESULT = 2;
    }
}
