using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using org.alan.chess.proto;

/// <summary>
/// 游戏通用返回值的一个描述映射
/// </summary>
public static class GameTipsDic {
    private static Dictionary<GameResultEnum, string> resultDes = new Dictionary<GameResultEnum, string>();
    private static Dictionary<GameEnum, string> gameEnumDes = new Dictionary<GameEnum, string>();
    static GameTipsDic() {
        resultDes[GameResultEnum.SUCCESS] = "成功";
        resultDes[GameResultEnum.FAILURE] = "失败";
        resultDes[GameResultEnum.ILLEGAL] = "非法操作";
        resultDes[GameResultEnum.ERROR] = "未知错误";
        resultDes[GameResultEnum.ROLE_CREATED] = "创建角色";
        resultDes[GameResultEnum.ROLE_REPEAT] = "角色名重复";
        resultDes[GameResultEnum.NOT_ENOUGH_GOLD] = "金币不足";
        resultDes[GameResultEnum.NOT_ENOUGH_DIAMOND] = "钻石不足";
        resultDes[GameResultEnum.FINISH_QUEST] = "完成任务";
        resultDes[GameResultEnum.LENGTH_TOO_LONG] = "字符串长度过长";
        resultDes[GameResultEnum.NEED_GET_AWARD] = "奖励未领取";
        resultDes[GameResultEnum.SEASON_NOT_OPENED] = "赛季未开启";
        gameEnumDes[GameEnum.LOGIN_SUCCESS] = "登录成功";
    }

    public static string GetTips(GameResultEnum gre) {
        if (resultDes.ContainsKey(gre)) {
            return resultDes[gre];
        }
        return null;
    }
    public static string GetTips(GameEnum gre) {
        if (gameEnumDes.ContainsKey(gre)) {
            return gameEnumDes[gre];
        }
        return null;
    }
}

/// <summary>
/// 游戏通用返回值
/// </summary>
public enum GameEnum {
    //客户端自定义部分枚举结果
    LOGIN_SUCCESS = 10001
}
