using Assets.Scripts.manager;
using com.tsixi.mars.protobuf;
using com.tsixi.miner.pbm;
using UnityEngine;

namespace Assets.Scripts.net.responses
{
    class QuestResponse : IResponse
    {
        public static string QUEST_ROLE_QUESTS = "resRoleQuests";
        public void handler(object msg)
        {
            TXMessage tmeg = (TXMessage)msg;
            if(tmeg.cmd == 1502)//玩家的所有任务
            {
                ResRoleQuests resRoleQuests = NetManager.DeSerialize<ResRoleQuests>(tmeg.data_message);
                PlayerManager.getInstance().RoleQuests = resRoleQuests;
                EventDispatcher.Instance().DispatchEvent(QUEST_ROLE_QUESTS, resRoleQuests);
                Debug.Log(resRoleQuests.questData.Count);
            }

        }
        
    }
}
