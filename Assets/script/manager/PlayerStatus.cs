using Assets.Scripts.manager;
using org.alan.chess.proto;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.script.manager {
    /// <summary>
    /// 状态管理器
    /// </summary>
    class PlayerStatusManager {
        public PlayerStatus currentStatus = new NormalStatus();
        private PlayerStatus nextStatus;
        /// <summary>
        /// 切换状态
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool Switch(PlayerStatus status) {
            bool b1 = currentStatus != status;
            bool b2 = currentStatus.CanSwitch(status);
            if (b1 && b2) {
                nextStatus = status;
                return true;
            }
            return false;
        }

        private void NextStatus() {
            currentStatus.Finish();
            currentStatus = nextStatus;
            nextStatus = null;
            currentStatus.Start();
        }

        public void Update() {
            if (nextStatus != null) {
                NextStatus();
            }
            currentStatus.Update();
        }
    }

    enum StatusEnum {
        NORMAL = 1,
        MATCH = 2,
        BATTLE = 3
    }

    /// <summary>
    /// 玩家状态类
    /// </summary>
    abstract class PlayerStatus {
        public abstract StatusEnum Status();
        public virtual void Start() { }
        public virtual void Update() { }
        public virtual bool CanSwitch(PlayerStatus playerStatus) {
            return true;
        }
        public virtual void Finish() { }

    }

    /// <summary>
    /// 普通状态
    /// </summary>
    class NormalStatus : PlayerStatus {
        public static NormalStatus INSTANCE = new NormalStatus();
        public override StatusEnum Status() {
            return StatusEnum.NORMAL;
        }
    }

    /// <summary>
    /// 匹配状态
    /// </summary>
    class MatchStatus : PlayerStatus {
        public static MatchStatus INSTANCE = new MatchStatus();
        public long beginTime;
        GameObject gameObj;
        public bool hasCancel;
        public GameResultEnum result;
        public override StatusEnum Status() {
            return StatusEnum.MATCH;
        }
        public override void Start() {
            gameObj = PopupManager.AddWindow(PopupWindowName.MATCH_POP_UP);
            MatchController matchController = gameObj.GetComponent<MatchController>();
            matchController.beginTime = beginTime;
        }
        public override void Finish() {
            PopupManager.RemoveWindow(PopupWindowName.MATCH_POP_UP);
            beginTime = 0;
            gameObj = null;
            hasCancel = false;
        }
        public void CancelMatch(GameResultEnum result) {
            PlayerManager.self.statusManager.Switch(NormalStatus.INSTANCE);
        }
    }
    /// <summary>
    /// 战斗状态
    /// </summary>
    class BattleStatus : PlayerStatus {
        public static BattleStatus INSTANCE = new BattleStatus();
        //游戏初始化数据
        public RespGameInit respGameInit;
        public override StatusEnum Status() {
            return StatusEnum.BATTLE;
        }
        public override void Start() {
            Debug.Log("加载游戏场景");
            //开始初始游戏场景
        }
        public override void Update() {

        }

    }
}
