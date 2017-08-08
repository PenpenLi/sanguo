using Assets.Scripts.manager;
using com.tsixi.mars.protobuf;
using com.tsixi.miner.pbm;
using UnityEngine;

namespace Assets.Scripts.net.responses
{
    class AwardResponse : IResponse
    {
        static public string EVENT_AWARD_GET = "EVENT_AWARD_GET";
        public void handler(object msg)
        {
            TXMessage tmeg = (TXMessage)msg;
            if(tmeg.cmd == 1702)//服务器响应领奖
            {
                ResSendAwardByQuest resSendAwardByQuest = NetManager.DeSerialize<ResSendAwardByQuest>(tmeg.data_message);
                EventDispatcher.Instance().DispatchEvent(EVENT_AWARD_GET, resSendAwardByQuest);
                Debug.Log("resSendAwardByQuest :"+resSendAwardByQuest.result);
            }

        }
        
    }
}
