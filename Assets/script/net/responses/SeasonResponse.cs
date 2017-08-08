using Assets.Scripts.manager;
using com.tsixi.mars.protobuf;
using com.tsixi.miner.pbm;
using UnityEngine;

namespace Assets.Scripts.net.responses
{
    class SeasonResponse : IResponse
    {
        static public string EVENT_SEASON_JOIN = "event_season_join";
        static public string EVENT_SEASON_INFO = "event_season_info";
        static public string EVENT_SEASON_ROLEINFO = "event_season_role_info";
        static public string EVENT_SEASON_RANKING = "event_season_ranking";
        public void handler(object msg)
        {
            TXMessage tmeg = (TXMessage)msg;
            if(tmeg.cmd == 1802)//服务器响应加入赛季
            {
                ResJoinSeasonMessage resJoinSeasonMessage = NetManager.DeSerialize<ResJoinSeasonMessage>(tmeg.data_message);
                EventDispatcher.Instance().DispatchEvent(EVENT_SEASON_JOIN, resJoinSeasonMessage);
                
            }else if(tmeg.cmd == 1804)//服务器响应赛季排行数据
            {
                ResSeasonRank resSeasonRank = NetManager.DeSerialize<ResSeasonRank>(tmeg.data_message);
                EventDispatcher.Instance().DispatchEvent(EVENT_SEASON_RANKING, resSeasonRank);
            }
            else if (tmeg.cmd == 1806)//服务器响应赛季情况
            {
                ResSeasonInfo resSeasonInfo = NetManager.DeSerialize<ResSeasonInfo>(tmeg.data_message);
                PlayerManager.getInstance().resSeasonInfo = resSeasonInfo;
                EventDispatcher.Instance().DispatchEvent(EVENT_SEASON_INFO, resSeasonInfo);
            }
            else if (tmeg.cmd == 1808)//服务器返回赛季当前玩家信息
            {
                ResRoleInfo resRoleInfo = NetManager.DeSerialize<ResRoleInfo>(tmeg.data_message);
                PlayerManager.getInstance().resRoleInfo = resRoleInfo;
                EventDispatcher.Instance().DispatchEvent(EVENT_SEASON_ROLEINFO, resRoleInfo);
            }

        }
        
    }
}
