using Assets.Scripts.login;
using com.tsixi.miner.pbm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.manager
{
    class PlayerManager
    {
        static private PlayerManager _instance;
        static public PlayerManager getInstance()
        {
            if(_instance == null)
            {
                _instance = new PlayerManager();
                _instance.wearDic.Add(PropType.BAG,0);
                _instance.wearDic.Add(PropType.LANTERN, 0);
                _instance.wearDic.Add(PropType.ROLE_MODEL, 0);
                _instance.wearDic.Add(PropType.WEAPON, 0);
            }
            return _instance;
        }
        /// <summary>role 模型</summary>
        public string roleMode = "";
        /// <summary>所穿装备 皮肤sid</summary>
        public Dictionary<PropType,int> wearDic= new Dictionary<PropType, int>();

        private ResRoleInfoMessage roleInfo;
        /// <summary>
        /// zhong
        /// </summary>
        private LoginDataCenter loginDataCenter;
        /// <summary>
        /// 登录结果 是否需要创建角色
        /// </summary>
        private ResLoginResultMessage loginResult;
        /// <summary>
        /// 服务器返回指定类型已有道具列表
        /// </summary>
        public ResPropList resPropList;
        /// <summary>
        /// 服务器返回玩家装备数据
        /// </summary>
        public ResWearInfo resWearInfo;

        /// <summary>
        /// 前玩家所有奖杯信息
        /// </summary>
        public ResCupInfo resCupInfo;
        /// <summary>
        /// 玩家生效奖杯信息
        /// </summary>
        public ResCurrentCupInfo resCurrentCupInfo;
        /// <summary>
        /// 玩家的所有任务
        /// </summary>
        public ResRoleQuests RoleQuests;
        /// <summary>
        /// 赛季信息
        /// </summary>
        public ResSeasonInfo resSeasonInfo;
        /// <summary>
        /// 赛季信息个人信息
        /// </summary>
        public ResRoleInfo resRoleInfo;

        public ResRoleInfoMessage RoleInfo
        {
            get
            {
                return roleInfo;
            }

            set
            {
                roleInfo = value;
            }
        }

        public LoginDataCenter LoginDataCenter
        {
            get
            {
                return loginDataCenter;
            }

            set
            {
                loginDataCenter = value;
            }
        }

        public ResLoginResultMessage LoginResult
        {
            get
            {
                return loginResult;
            }

            set
            {
                loginResult = value;
            }
        }
    }
}
