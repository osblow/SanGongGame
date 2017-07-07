using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using com.sansanbbox.protobuf;
using Osblow.App;
using Osblow.Util;

public class CmdHandler
{
    /// <summary>
    /// 游戏服注册返回
    /// </summary>
    /// <param name="data"></param>
    public static void ServerRegisterResponse(byte[] data)
    {
        ServerRegisterResponse res = GetProtoInstance<ServerRegisterResponse>(data);
        MsgMng.Dispatch(MsgType.Registed);
        
        //Globals.SceneSingleton<DataMng>().GetData<UserData>(DataType.Player).socket_code = res.code;

        //if(res.code == 0)
        //{
        //    // success
        //    Debug.Log("table regist    success");

        //    Globals.SceneSingleton<ContextManager>().Push(new TableUIContext());
        //}
        //else
        //{
        //    // failed
        //    Debug.Log("table regist     failed");
        //}
    }

    /// <summary>
    /// 心跳返回
    /// </summary>
    /// <param name="data"></param>
    public static void ServerHeartbeatResponse(byte[] data)
    {
        ServerHeartbeatResponse res = GetProtoInstance<ServerHeartbeatResponse>(data);
        string serverTime = res.date_time;

        Debug.Log("Heartbeat....time= " + serverTime);
    }

    /// <summary>
    /// 进入房间返回
    /// </summary>
    /// <param name="data"></param>
    public static void EnterRoomResponse(byte[] data)
    {
        EnterRoomResponse res = GetProtoInstance<EnterRoomResponse>(data);
        uint code = res.code;
        if(code == 1) // 请求失败
        {
            Debug.Log("进入房间失败");
            return;
        }

        TableData tableData = new TableData();
        tableData.RoomId = res.room_id;
        tableData.OwnerUUId = res.owner_uuid;
        tableData.ruleName = res.rule_name;
        tableData.CurRound = res.current_round;
        tableData.TotalRound = res.all_round;
        tableData.IsSeated = res.is_seat;
        tableData.SeatNum = res.seat;
        tableData.TotalScore = res.total_score;
        tableData.Icon = res.player_head_img;
        tableData.NickName = res.player_nike_name;
        tableData.UserIp = res.user_ip;
        tableData.EvaluateScore = res.evaluate_score;
        tableData.CanDismiss = res.is_dismiss;
        tableData.PayRule = res.room_rules;

        tableData.BetPoints = new List<BetPointData>();
        res.betPoints.ForEach((x) => 
        {
            BetPointData temp = new BetPointData();
            temp.BetPoint = x.bet_points;
            tableData.BetPoints.Add(temp);
        });

        tableData.Players = new List<TablePlayerData>();
        res.player.ForEach((x) => 
        {
            TablePlayerData temp = new TablePlayerData();
            temp.SeatNum = x.seat;
            temp.PlayerUUID = x.player_uuid;
            temp.AcountId = x.account_id;
            temp.IsOnline = x.is_online;
            temp.TotalScore = x.total_score;
            temp.HeadImg = x.player_head_img;
            temp.NickName = x.player_nike_name;
            temp.UserIp = x.user_ip;
            temp.EvaluateScore = x.evaluate_score;
        });

        Globals.SceneSingleton<DataMng>().SetData(DataType.Table, tableData);

        Globals.SceneSingleton<ContextManager>().Push(new TableUIContext());

        Debug.Log("成功进入房间， 房号： " + tableData.RoomId);
    }

    /// <summary>
    /// 其他玩家进入
    /// </summary>
    /// <param name="data"></param>
    public static void EnterRoomOtherResponse(byte[] data)
    {
        EnterRoomOtherResponse res = GetProtoInstance<EnterRoomOtherResponse>(data);
        TablePlayerData playerData = new TablePlayerData();
        playerData.SeatNum = res.seat;
        playerData.PlayerUUID = res.player_uuid;
        playerData.AcountId = res.account_id;
        playerData.IsOnline = res.is_online;
        playerData.TotalScore = res.total_score;
        playerData.HeadImg = res.player_head_img;
        playerData.NickName = res.player_nike_name;
        playerData.UserIp = res.user_ip;
        playerData.EvaluateScore = res.evaluate_score;

        TableData curTableData = Globals.SceneSingleton<DataMng>().GetData<TableData>(DataType.Table);
        if(curTableData == null)
        {
            return;
        }

        curTableData.Players.Add(playerData);

        Osblow.Util.MsgMng.Dispatch(Osblow.Util.MsgType.OtherPlayerEnter, playerData.PlayerUUID);

        Debug.Log("有玩家进入");
    }

    /// <summary>
    /// 退出房间广播
    /// </summary>
    /// <param name="data"></param>
    public static void ExitRoomToOtherResponse(byte[] data)
    {
        ExitRoomToOtherResponse res = GetProtoInstance<ExitRoomToOtherResponse>(data);

        string thePlayerUUID = res.player_uuid;

        Debug.Log("退出房间" + thePlayerUUID);
    }

    /// <summary>
    /// 退出房间返回
    /// </summary>
    /// <param name="data"></param>
    public static void ExitRoomResultResponse(byte[] data)
    {
        ExitRoomResultResponse res = GetProtoInstance<ExitRoomResultResponse>(data);
        if (res.code == 0)
        {
            Debug.Log("退出房间成功");
        }
        else
        {
            // 失败
            Debug.Log("退出房间失败");
        }
    }

    /// <summary>
    /// 解散房间广播
    /// </summary>
    /// <param name="data"></param>
    public static void DismissRoomToOtherResponse(byte[] data)
    {
        DismissRoomToOtherResponse res = GetProtoInstance<DismissRoomToOtherResponse>(data);

        string theUUID = res.dismiss_uuid;
        uint expireTime = res.expire_seconds;

        Debug.Log(theUUID + "解散了房间");
    }

    /// <summary>
    /// 投票信息广播
    /// </summary>
    /// <param name="data"></param>
    public static void PlayerVoteDismissRoomResponse(byte[] data)
    {
        PlayerVoteDismissRoomResponse res = new com.sansanbbox.protobuf.PlayerVoteDismissRoomResponse();

        string theUUID = res.play_uuid;
        bool isAgreeing = res.flag;

        Debug.Log(theUUID + "投票" + isAgreeing);
    }

    /// <summary>
    /// 解散投票结果
    /// </summary>
    /// <param name="data"></param>
    public static void DismissRoomResultResponse(byte[] data)
    {
        DismissRoomResultResponse res = new com.sansanbbox.protobuf.DismissRoomResultResponse();
        if(res.code == 0)
        {
            // 成功
            Debug.Log("成功解散");
        }
        else
        {
            // 失败
            Debug.Log("解散失败");
        }
    }

    /// <summary>
    /// 准备消息广播
    /// </summary>
    /// <param name="data"></param>
    public static void ReadyResponse(byte[] data)
    {
        ReadyResponse res = GetProtoInstance<ReadyResponse>(data);

        string theUUID = res.play_uuid;
        uint theSeat = res.seat;

        Debug.Log(theUUID + "已准备");
    }

    /// <summary>
    /// 断线重连返回
    /// </summary>
    /// <param name="data"></param>
    public static void ReconnectResponse(byte[] data)
    {
        Debug.Log("重连");
    }

    /// <summary>
    /// 玩家上线离线广播
    /// </summary>
    /// <param name="data"></param>
    public static void OnlineStatusResponse(byte[] data)
    {
        OnlineStatusResponse res = GetProtoInstance<OnlineStatusResponse>(data);

        string theUUID = res.player_uuid;
        uint theSeat = res.seat;
        bool isOnline = res.status;

        Debug.Log(theUUID + "玩家上线" + isOnline);
    }

    /// <summary>
    /// 表情和文字同步
    /// </summary>
    /// <param name="data"></param>
    public static void SynchroniseExpressionResponse(byte[] data)
    {
        // 需要ID
    }

    /// <summary>
    /// 语音广播
    /// </summary>
    /// <param name="data"></param>
    public static void AudioStreamBroadcast(byte[] data)
    {
        // 需要ID
    }

    /// <summary>
    /// 下注广播
    /// </summary>
    /// <param name="data"></param>
    public static void ServerBetResponse(byte[] data)
    {
        ServerBetResponse res = GetProtoInstance<ServerBetResponse>(data);

        string theUUID = res.player_uuid;
        uint theSeat = res.seat;
        uint thePoint = res.bet_point;

        Debug.Log(theUUID + "下注" + thePoint);
    }

    /// <summary>
    /// 开始抢庄广播(直接抢庄)
    /// </summary>
    /// <param name="data"></param>
    public static void ServerBankerNotice1Response(byte[] data)
    {
        ServerBankerNotice1Response res = GetProtoInstance<ServerBankerNotice1Response>(data);
        if(!res.code)
        {
            return;
        }

        uint expireTime = res.expire_seconds;

        Debug.Log("开始抢庄");
    }

    /// <summary>
    /// 先发两张牌，看了牌之后再抢庄
    /// </summary>
    /// <param name="data"></param>
    public static void ServerBankerNotice2Response(byte[] data)
    {
        ServerBankerNotice2Response res = GetProtoInstance<ServerBankerNotice2Response>(data);
        List<uint> cards = new List<uint>();
        res.card.ForEach((x) => { cards.Add(x.card); });

        uint expireTime = res.expire_seconds;

        Debug.Log("先发两张，然后抢庄");
    }

    /// <summary>
    /// 再次下注广播
    /// </summary>
    /// <param name="data"></param>
    public static void ServerBetAgainResponse(byte[] data)
    {
        ServerBetAgainResponse res = GetProtoInstance<ServerBetAgainResponse>(data);
        string theUUID = res.player_uuid;
        uint theSeat = res.seat;
        uint betPoint = res.bet_point;

        Debug.Log(theUUID + "再次下注" + betPoint);
    }

    /// <summary>
    /// 抢庄返回，多人抢庄服务器随机分配，无人抢庄则通比大小
    /// </summary>
    /// <param name="data"></param>
    public static void ServerBankerResponse(byte[] data)
    {
        ServerBankerResponse res = GetProtoInstance<ServerBankerResponse>(data);

        bool hasOwner = res.is_banker;
        string ownerUUID = res.banker_uuid;
        uint ownerSeat = res.banker_seat;
        bool nextBet = res.is_bet;

        Debug.Log("抢庄结果");
    }

    /// <summary>
    /// 发牌
    /// </summary>
    /// <param name="data"></param>
    public static void ServerCardsResponse(byte[] data)
    {
        ServerCardsResponse res = GetProtoInstance<ServerCardsResponse>(data);
        List<uint> cards = new List<uint>();
        res.card.ForEach((x) => { cards.Add(x.card); });

        /*
         * 0:0点	 1: 1点   	2:2点 	3: 3点 	 4:4点   	 5: 5点   	6:6点	7: 7点  	8:8点  	 9:9点    	10：混三公  	11：小三公 	 12：大三公 
         */
        uint cardResult = res.card_result;

        uint autoShowCardInterval = res.expire_seconds;

        Debug.Log("结果" + cardResult);
    }

    /// <summary>
    /// 亮牌
    /// </summary>
    /// <param name="data"></param>
    public static void SynchroniseCardsResponse(byte[] data)
    {
        SynchroniseCardsResponse res = GetProtoInstance<SynchroniseCardsResponse>(data);
        string theUUID = res.player;
        uint theSeat = res.seat;

        List<uint> cards = new List<uint>();
        res.card.ForEach((x) => { { cards.Add(x.card); } });

        uint cardResult = res.card_face;

        Debug.Log("亮牌" + cardResult);
    }

    /// <summary>
    /// 比大小
    /// </summary>
    /// <param name="data"></param>
    public static void WinOrLoseResponse(byte[] data)
    {
        WinOrLoseResponse res = GetProtoInstance<WinOrLoseResponse>(data);

        Debug.Log("比大小" + res.result);
    }

    /// <summary>
    /// 总结算
    /// </summary>
    /// <param name="data"></param>
    public static void UserAllResult(byte[] data)
    {
        UserAllResult res = GetProtoInstance<UserAllResult>(data);

        Debug.Log("总结算" + res.allResult.Count);
    }

    public static T GetProtoInstance<T>(byte[] data)
    {
        T t = default(T);
        int index = 3;
        short length = BitConverter.ToInt16(data, index);
        index += 2;

        using (MemoryStream ms = new MemoryStream())
        {
            //将消息写入流中
            ms.Write(data, index, length);
            //将流的位置归0
            ms.Position = 0;
            //使用工具反序列化对象
            t = ProtoBuf.Serializer.Deserialize<T>(ms);
        }

        return t;
    }
}
