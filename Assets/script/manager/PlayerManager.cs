using Assets.script.manager;
using Assets.Scripts.login;
using org.alan.chess.proto;

namespace Assets.Scripts.manager {
    class PlayerManager {

        public static PlayerManager self = new PlayerManager();
        /// <summary>
        /// 中心服务器返回数据
        /// </summary>
        public LoginDataCenter loginDataCenter;
        /// <summary>
        /// 玩家状态机
        /// </summary>
        public PlayerStatusManager statusManager;
        /// <summary>
        /// 玩家数据结构
        /// </summary>
        public Player player;
    }
}
