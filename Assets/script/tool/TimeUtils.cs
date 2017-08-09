using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// 时间工具类
/// </summary>
public static class TimeUtils {

    /// <summary>
    /// 将一个时间转成格林威治毫秒时间
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public static long ToGMTMillis(DateTime time) {
        System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
        return (long)(time - startTime).TotalMilliseconds; // 相差毫秒数
    }
    /// <summary>
    /// 将一个时间转成格林威治秒时间
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public static int ToGMTSeconds(DateTime time) {
        System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
        return (int)(time - startTime).TotalSeconds; // 相差秒数
    }
    /// <summary>
    /// 获得当前格林威治秒级时间
    /// </summary>
    /// <returns></returns>
    public static int CurrentGMTSeconds() {
        return ToGMTSeconds(System.DateTime.UtcNow);
    }

    /// <summary>
    /// 获得当前格林威治毫秒时间
    /// </summary>
    /// <returns></returns>
    public static long CurrentGMTMillis() {
        return ToGMTMillis(System.DateTime.UtcNow);
    }
}
