using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.net
{
    class NetConst
    {
        /// <summary>ping</summary>
        static public uint PING_S = 0;
        /// <summary>server time</summary>
        static public uint SERVER_TIME_S = 2;

        /// <summary>登录</summary>
        static public uint LOGIN_S = 1002;
        /// <summary>C创建</summary>
        static public uint CREAT_ROLE_S = 1004;
        /// <summary>角色信息</summary>
        static public uint ROLE_INFO_S = 1005;
        /// <summary>改名</summary>
        static public uint RENAME_S = 1007;
        /// <summary>客户端上传玩家数据</summary>
        static public uint UP_BATTLE_DATA_S = 1202;
        /// <summary>服务器返回排行榜列表信息</summary>
        static public uint SROCE_RANK_LIST = 1204;

        /// <summary>角色消息</summary>
        static public uint ROLE_BASE_INFO_S = 1000;
        /// <summary>商店，道具</summary>
        static public uint STORE_PROP_S = 1300;

        /// <summary>任务</summary>
        static public uint TASK_S = 1400;
        /// <summary>quest</summary>
        static public uint QUEST_S = 1500;
        /// <summary>battle</summary>
        static public uint BATTLE_S = 1600;
        /// <summary>award</summary>
        static public uint AWARD_S = 1700;
        /// <summary>season</summary>
        static public uint SEASON_S = 1800;
    }
}
