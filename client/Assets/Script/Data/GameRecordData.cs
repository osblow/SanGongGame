using System.Collections;
using System.Collections.Generic;


public class GameRecords : DataBase
{
    public List<SingleGameRecord> Records;
}

public class SingleGameRecord : DataBase
{
    public uint RoomNumber = 0;        // 房间号
    public uint RoomId = 0;            // 房间唯一ID
    public uint TotalRound = 0;        // 总局数
    public string RoomRuleName = "";   // 规则名
    public string GameTime = "";       // 总时间
    public List<RecordPlayer> Players; // 所有玩家信息
}

public class SmallGameRecord : DataBase
{
    public List<RecordPlayer> Players;
    public List<SingleSmallGameRecord> SingleRecords;
}

public class SingleSmallGameRecord : DataBase
{
    public uint Index = 0; // 局
    public List<SmallGameRecPlayer> Players; // 所有玩家的成绩
}

public class SmallGameRecPlayer : DataBase
{
    public bool IsBanker = false; // 是否庄家
    public int Point = 0; // 下注分数
    public uint CardResult = 0; // 牌面的结果（牌型）
    public List<uint> Cards; // 所有牌
}




public class RecordPlayer : DataBase
{
    public bool IsOwner = false; // 是否房主
    public uint AccountId = 0; // 玩家ID
    public string Icon = ""; // 头像
    public string NickName = ""; // 昵称
    public int Score = 0; // 总成绩
}





public class ContactUsData: DataBase
{
    public string Key = "";
    public string Value = "";
}

public class RechargeData : DataBase
{
    public string Number;
    public uint Count;
    public uint Money;
    public string InviteCode;
    public string Status;
    public string Time;
}
