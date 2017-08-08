using Assets.Scripts.manager;
using com.tsixi.mars.protobuf;
using com.tsixi.miner.pbm;
using UnityEngine;

namespace Assets.Scripts.net.responses
{
    class TaskResponse : IResponse
    {
        static public string EVENT_CUP_INFOLIST = "cup_infolist_result";
        public void handler(object msg)
        {
            TXMessage tmeg = (TXMessage)msg;
            if(tmeg.cmd == 1402)//服务器响应购买cup的消息
            {
                ResBuyCup resBuyCup = NetManager.DeSerialize<ResBuyCup>(tmeg.data_message);
                EventDispatcher.Instance().DispatchEvent("cup_buy_result",resBuyCup);
                Debug.Log(resBuyCup.result);
            }else if(tmeg.cmd == 1404)//前玩家所有奖杯信息
            {
                ResCupInfo resCupInfo = NetManager.DeSerialize<ResCupInfo>(tmeg.data_message);
                PlayerManager.getInstance().resCupInfo = resCupInfo;
                EventDispatcher.Instance().DispatchEvent(EVENT_CUP_INFOLIST, resCupInfo);
                Debug.Log("resCupInfo.count:"+resCupInfo.cups.Count);
            }
            else if (tmeg.cmd == 1406)//当前生效的奖杯信息
            {
                ResCurrentCupInfo resCurrentCupInfo = NetManager.DeSerialize<ResCurrentCupInfo>(tmeg.data_message);
                PlayerManager.getInstance().resCurrentCupInfo = resCurrentCupInfo;
                EventDispatcher.Instance().DispatchEvent("cup_currentCupInfo", resCurrentCupInfo);
                
            }

        }
        
    }
}
