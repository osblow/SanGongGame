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
    }
}
