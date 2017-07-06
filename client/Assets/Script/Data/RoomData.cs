using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomData : DataBase
{
    public string RoomRuleName = "三公";
    public string RoomTotalRound = "总局数";
    public int RoomCostRule = 0; // 0房主支付，1AA
    public int IsJoin = 0;// 0允许加入，1不允许
    /*
     * // 0先下注，不看牌抢庄。
     * 1先下注，发两张牌后抢庄。
     * 2先下注，发两张牌后抢庄，第二轮投注
     * 3先下注后抢庄，发两张牌比大小
     */
    public int RoomRuleType = 0;

    // 服务返回
    public uint RoomId = 0;
}
