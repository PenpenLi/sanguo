using Assets.Scripts.manager;
using com.tsixi.mars.protobuf;
using com.tsixi.miner.pbm;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.net.responses
{
    class BattleResponse : IResponse
    {
        static public string EVENT_RELIVE_RESULT = "EVENT_RELIVE_RESULT";
        static public string EVENT_GAME_INIT = "EVENT_GAME_INIT";
        static public string EVENT_GAME_START = "EVENT_GAME_START";
        static public string EVENT_GAME_INPUTS = "EVENT_GAME_INPUTS";

        static public List<RoleShowInfo> roles;

        ResReviveResult resReviveResult = new ResReviveResult();

        ResGameInit resGameInit = new ResGameInit();
        ResGameStart resGameStart = new ResGameStart();
        ResGameInputs resGameInputs = new ResGameInputs();
        public void handler(object msg)
        {
            TXMessage tmeg = (TXMessage)msg;
            if (tmeg.cmd == resReviveResult.cmd)//rlive  result
            {
                resReviveResult = NetManager.DeSerialize<ResReviveResult>(tmeg.data_message);
                EventDispatcher.Instance().DispatchEvent(EVENT_RELIVE_RESULT, resReviveResult);
                Debug.Log("resReviveResult :" + resReviveResult.result);
            }
            else if (tmeg.cmd == resGameInit.cmd)
            {
                resGameInit = NetManager.DeSerialize<ResGameInit>(tmeg.data_message);
                int sid = resGameInit.battleSid;
                int uid = resGameInit.battleUid;
                roles = resGameInit.roles;
                EventDispatcher.Instance().DispatchEvent(EVENT_GAME_INIT, resGameInit);
                Debug.Log("resGameInit :" + resGameInit.battleSid);
            }
            else if (tmeg.cmd == resGameStart.cmd)
            {
                resGameStart = NetManager.DeSerialize<ResGameStart>(tmeg.data_message);
                EventDispatcher.Instance().DispatchEvent(EVENT_GAME_START, resGameStart);
                Debug.Log("resGameInit :" + resGameInit.battleSid);
            }
            else if (tmeg.cmd == resGameInputs.cmd)
            {
                resGameInputs = NetManager.DeSerialize<ResGameInputs>(tmeg.data_message);
                EventDispatcher.Instance().DispatchEvent(EVENT_GAME_INPUTS, resGameInputs);
                //EventDispatcher.Instance().DispatchEvent(EVENT_GAME_START, resGameStart);
                //Debug.Log("resGameInputs :" + resGameInit.battleSid);
            }
        }

    }
}
