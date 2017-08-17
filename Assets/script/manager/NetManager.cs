using Assets.Scripts.net;
using Assets.Scripts.net.responses;
using Assets.Scripts.tool;
using com.tsixi.mars.protobuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.manager {
    class NetManager {
        /// <summary>
        /// game socket
        /// </summary>
        static public ClientSocket clientSocket;

        static public Dictionary<uint, IResponse> ResponseDic = new Dictionary<uint, IResponse>();
        public NetManager() {

        }

        static private NetManager _instance;
        static public NetManager GetIntance() {
            if (_instance == null) {
                _instance = new NetManager();
            }
            return _instance;
        }
        public void InitDic() {
            ResponseDic.Add(NetConst.PING_S, new PingResponse());
            ResponseDic.Add(NetConst.SERVER_TIME_S, new ServerTimeResponse());
            ResponseDic.Add(NetConst.LOGIN_S, new LoginResponse());
            ResponseDic.Add(NetConst.CREAT_ROLE_S, new CreatRoleResponse());
            ResponseDic.Add(NetConst.ROLE_INFO_S, new RoleInfoResponse());
            ResponseDic.Add(NetConst.UP_BATTLE_DATA_S, new UpBattleDataResponse());
            ResponseDic.Add(NetConst.SROCE_RANK_LIST, new ScoreRankListResponse());
            ResponseDic.Add(NetConst.RENAME_S, new ReNameResponse());

            ResponseDic.Add(NetConst.ROLE_BASE_INFO_S, new RoleBaseInfoResponse());
            ResponseDic.Add(NetConst.STORE_PROP_S, new StorePropResponse());
            ResponseDic.Add(NetConst.TASK_S, new TaskResponse());
            ResponseDic.Add(NetConst.QUEST_S, new QuestResponse());
            ResponseDic.Add(NetConst.AWARD_S, new AwardResponse());
            ResponseDic.Add(NetConst.BATTLE_S, new BattleResponse());
            ResponseDic.Add(NetConst.SEASON_S, new SeasonResponse());
        }

        public void Handle(TXMessage tmeg) {
            if (ResponseDic.Count <= 0) {
                InitDic();
            }

            if (tmeg.message_type == 1 && tmeg.cmd == NetConst.PING_S)//ping 
            {
                IResponse response = ResponseDic[tmeg.cmd];
                response.handler(tmeg.data_message);
            } else if ((tmeg.message_type == 1000 && tmeg.cmd != 1009) || tmeg.message_type == 1200) {
                IResponse response = ResponseDic[tmeg.cmd];
                if (response == null) {
                    Debug.Log("cmd::" + tmeg.cmd + " is not in dictionary");
                } else {
                    response.handler(tmeg.data_message);
                }
            } else {
                if (ResponseDic.ContainsKey(tmeg.message_type)) {
                    IResponse response = ResponseDic[tmeg.message_type];
                    response.handler(tmeg);
                } else {
                    Debug.Log("message_type::" + tmeg.message_type + " is not in dictionary");
                }
            }
        }


        public DateTime beginTime;
        public void SynServerTime() {
            Debug.Log("syn server time.");
            ReqServerTime reqServerTime = new ReqServerTime();
            beginTime = DateTime.Now;
            NetManager.clientSocket.WriteSend(reqServerTime);

        }

        System.Timers.Timer t;


        /// <summary>
        /// 
        /// </summary>
        public void StopNet() {
            Debug.Log("stopNet timer");
            if (clientSocket != null) {
                clientSocket.Close();
            }
        }
    }
}
