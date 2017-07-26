using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Osblow.App
{
    public class TableData : DataBase
    {
        public int SanGong = 0;
        public int HasOwner = 0;
        public int BaseScore = 0;

        public uint RoomId = 0;
        public string OwnerUUId = "";
        public string ruleName = "";
        public uint CurRound = 0;
        public uint TotalRound = 0;
        public bool IsSeated = false;
        public uint SeatNum = 0;
        public uint TotalScore = 0;
        public string Icon = "";
        public string NickName = "";
        public string UserIp = "";
        public string EvaluateScore = "";
        public bool CanDismiss = false;
        public string PayRule = ""; // AA 或 房主支付
        public List<BetPointData> BetPoints = null;
        public List<TablePlayerData> Players = null;

        public uint RuleStep = 0;// 0：准备中   1：下注中  2： 抢庄中  3： 再次下注中 4：已发牌，载游戏中   5：一局已结束
        public uint RuleOperation = 0; // 是否已操作
        public uint BetPoint = 0;
        public List<uint> Cards = null;
        public uint CardFace = 0;
        public uint DismissRemainTime = 0;

        public bool IsReady = true; // 是否可以准备（为旁观者）
        public bool IsGaming = false; // 当前牌桌状态是否为正在进行中
    }

    public class BetPointData: DataBase
    {
        public uint BetPoint = 0;
    }

    public class TablePlayerData : DataBase
    {
        public uint SeatNum = 0;
        public string PlayerUUID = "";
        public uint AcountId = 0;
        public bool IsOnline = false;
        public uint TotalScore = 0;
        public string HeadImg = "";
        public string NickName = "";
        public string UserIp = "";
        public string EvaluateScore = "";

        public bool IsSeated = false;
        public bool IsBanker = false;
        public uint RuleStep = 0;// 0：准备中   1：下注中  2： 抢庄中  3： 再次下注中 4：已发牌，载游戏中   5：一局已结束
        public uint RuleOperation = 0; // 是否已操作
        public uint BetPoint = 0;
        public List<uint> Cards = null;
        public uint CardFace = 0;
    }

    public class ResultEffectData : DataBase
    {
        public string FromUser;
        public string ToUser;
        public uint Point;
    }

    public class ResultUser : DataBase
    {
        public string UUID;
        public string Icon;
        public string Name;
        public int Point;
        public uint WinCount;
    }

    public class CompareSingleUser
    {
        public string Uuid;
        public int Point;
    }
}
