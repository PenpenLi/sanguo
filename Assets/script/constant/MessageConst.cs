using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.script.constant {

    public class MessageConst {
        public static class Login {
            public const int TYPE = 1000;
            public const int REQ_VERTIFY = 1;
            public const int RESP_CREATE_ROLE = 2;
            public const int REQ_CREATE_ROLE = 3;
            public const int RESP_ENTER_GAME = 4;
        }


        public static class Tips {
            public const int TYPE = 1002;
            public const int TIPS_RESP_RESULT = 2;
        }

        public static class Room {
            public const int TYPE = 1100;
            public const int REQ_CREATE_ROOM = 1;
            public const int RESP_CREATE_ROOM = 2;
            public const int REQ_BEGIN_MATCH = 3;
            public const int RESP_BEGIN_MATCH = 4;
            public const int REQ_CANEL_MATCH = 5;
            public const int RESP_CANEL_MATCH = 6;
            public const int REQ_QUICK_MATCH = 7;
        }

        public static class Battle {
            public const int TYPE = 1200;
            public const int RESP_GAME_INIT = 2;
            public const int REQ_GAME_INIT_DONE = 3;
            public const int REQ_MOVE_CHESS = 5;
            public const int RESP_MOVE_CHESS = 6;
            public const int RESP_CURRENT_GO_INFO = 8;
        }
    }
}
