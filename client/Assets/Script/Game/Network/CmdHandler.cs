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
    public static void ServerRegisterResponse(byte[] data, int index)
    {
        ServerRegisterResponse res = GetProtoInstance<ServerRegisterResponse>(data, index);

        Debug.Log("注册成功");
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
    public static void ServerHeartbeatResponse(byte[] data, int index)
    {
        ServerHeartbeatResponse res = GetProtoInstance<ServerHeartbeatResponse>(data, index);
        string serverTime = res.date_time;

        //Debug.Log("Heartbeat....time= " + serverTime);
    }

    /// <summary>
    /// 进入房间返回
    /// </summary>
    /// <param name="data"></param>
    public static void EnterRoomResponse(byte[] data, int index)
    {
        EnterRoomResponse res = GetProtoInstance<EnterRoomResponse>(data, index);
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
            tableData.Players.Add(temp);
        });

        Globals.SceneSingleton<DataMng>().SetData(DataType.Table, tableData);

        Globals.SceneSingleton<AsyncInvokeMng>().EventsToAct += delegate ()
        {
            Globals.SceneSingleton<ContextManager>().Push(new TableUIContext());
        };

        Debug.Log("成功进入房间， 房号： " + tableData.RoomId);
    }

    /// <summary>
    /// 其他玩家进入
    /// </summary>
    /// <param name="data"></param>
    public static void EnterRoomOtherResponse(byte[] data, int index)
    {
        Debug.Log("其它玩家进入");

        EnterRoomOtherResponse res = GetProtoInstance<EnterRoomOtherResponse>(data, index);
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

        Osblow.Util.MsgMng.Dispatch(Osblow.Util.MsgType.PlayerEnter, playerData.PlayerUUID, playerData);

        Debug.Log("有玩家进入");
    }

    /// <summary>
    /// 退出房间广播
    /// </summary>
    /// <param name="data"></param>
    public static void ExitRoomToOtherResponse(byte[] data, int index)
    {
        ExitRoomToOtherResponse res = GetProtoInstance<ExitRoomToOtherResponse>(data, index);

        string thePlayerUUID = res.player_uuid;

        Debug.Log("退出房间" + thePlayerUUID);
    }

    /// <summary>
    /// 退出房间返回
    /// </summary>
    /// <param name="data"></param>
    public static void ExitRoomResultResponse(byte[] data, int index)
    {
        ExitRoomResultResponse res = GetProtoInstance<ExitRoomResultResponse>(data, index);
        if (res.code == 0)
        {
            Debug.Log("退出房间成功");
            Globals.SceneSingleton<AsyncInvokeMng>().EventsToAct += delegate ()
            {
                Globals.SceneSingleton<StateMng>().ChangeState(StateType.Lobby);
                Globals.RemoveSceneSingleton<Osblow.Game.SocketNetworkMng>();
                Globals.SceneSingleton<UIManager>().DestroySingleUI(UIType.TableView);
                Globals.SceneSingleton<ContextManager>().Push(new LobbyUIContext());
            };
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
    public static void DismissRoomToOtherResponse(byte[] data, int index)
    {
        DismissRoomToOtherResponse res = GetProtoInstance<DismissRoomToOtherResponse>(data, index);

        string theUUID = res.dismiss_uuid;
        uint expireTime = res.expire_seconds;



        Debug.Log(theUUID + "解散了房间");
    }

    /// <summary>
    /// 投票信息广播
    /// </summary>
    /// <param name="data"></param>
    public static void PlayerVoteDismissRoomResponse(byte[] data, int index)
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
    public static void DismissRoomResultResponse(byte[] data, int index)
    {
        DismissRoomResultResponse res = new com.sansanbbox.protobuf.DismissRoomResultResponse();
        if(res.code == 0)
        {
            // 成功
            Debug.Log("成功解散");

            Globals.SceneSingleton<AsyncInvokeMng>().EventsToAct += delegate ()
            {
                Globals.RemoveSceneSingleton<Osblow.Game.SocketNetworkMng>();
                Globals.SceneSingleton<UIManager>().DestroySingleUI(UIType.TableView);
                Globals.SceneSingleton<ContextManager>().Push(new LobbyUIContext());
            };
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
    public static void ReadyResponse(byte[] data, int index)
    {
        ReadyResponse res = GetProtoInstance<ReadyResponse>(data, index);

        string theUUID = res.play_uuid;
        uint theSeat = res.seat;

        MsgMng.Dispatch(MsgType.UI_PlayerReady, theUUID);
        Debug.Log(theUUID + "已准备");
    }

    /// <summary>
    /// 断线重连返回
    /// </summary>
    /// <param name="data"></param>
    public static void ReconnectResponse(byte[] data, int index)
    {
        Debug.Log("重连");
    }

    /// <summary>
    /// 玩家上线离线广播
    /// </summary>
    /// <param name="data"></param>
    public static void OnlineStatusResponse(byte[] data, int index)
    {
        OnlineStatusResponse res = GetProtoInstance<OnlineStatusResponse>(data, index);

        string theUUID = res.player_uuid;
        uint theSeat = res.seat;
        bool isOnline = res.status;

        Debug.Log(theUUID + "玩家上线" + isOnline);
    }

    /// <summary>
    /// 表情和文字同步
    /// </summary>
    /// <param name="data"></param>
    public static void SynchroniseExpressionResponse(byte[] data, int index)
    {
        SynchroniseExpressionResponse res = GetProtoInstance<SynchroniseExpressionResponse>(data, index);

        string uuid = res.uuid;
        uint expression_id = res.expression_id;


        MsgMng.Dispatch(MsgType.UI_Expression, uuid, expression_id);
        Debug.Log("表情" + expression_id);
    }

    /// <summary>
    /// 语音广播
    /// </summary>
    /// <param name="data"></param>
    public static void AudioStreamBroadcast(byte[] data, int index)
    {
        // 需要ID

    }

    /// <summary>
    /// 下注广播
    /// </summary>
    /// <param name="data"></param>
    public static void ServerBetResponse(byte[] data, int index)
    {
        ServerBetResponse res = GetProtoInstance<ServerBetResponse>(data, index);

        string theUUID = res.player_uuid;
        uint theSeat = res.seat;
        uint thePoint = res.bet_point;

        Debug.Log(theUUID + "下注" + thePoint);
        MsgMng.Dispatch(MsgType.UI_PlayerBet, theUUID, thePoint);
    }

    /// <summary>
    /// 开始抢庄广播(直接抢庄)
    /// </summary>
    /// <param name="data"></param>
    public static void ServerBankerNotice1Response(byte[] data, int index)
    {
        ServerBankerNotice1Response res = GetProtoInstance<ServerBankerNotice1Response>(data, index);
        if(!res.code)
        {
            return;
        }

        uint expireTime = res.expire_seconds;
        MsgMng.Dispatch(MsgType.UpdateClock, expireTime);

		MsgMng.Dispatch (MsgType.StartBanker);
        Debug.Log("开始抢庄");
    }

    /// <summary>
    /// 先发两张牌，看了牌之后再抢庄
    /// </summary>
    /// <param name="data"></param>
    public static void ServerBankerNotice2Response(byte[] data, int index)
    {
        ServerBankerNotice2Response res = GetProtoInstance<ServerBankerNotice2Response>(data, index);
        List<uint> cards = new List<uint>();
        res.card.ForEach((x) => { cards.Add(x.card); });

        uint expireTime = res.expire_seconds;
        MsgMng.Dispatch(MsgType.UpdateClock, expireTime);

        MsgMng.Dispatch (MsgType.ShowTwoCardsAndStartBanker, cards.ToArray());
        Debug.Log("先发两张，然后抢庄");
    }

    /// <summary>
    /// 再次下注广播
    /// </summary>
    /// <param name="data"></param>
    public static void ServerBetAgainResponse(byte[] data, int index)
    {
        ServerBetAgainResponse res = GetProtoInstance<ServerBetAgainResponse>(data, index);
        string theUUID = res.player_uuid;
        uint theSeat = res.seat;
        uint betPoint = res.bet_point;

        MsgMng.Dispatch(MsgType.UI_PlayerBet, theUUID, betPoint);
        Debug.Log(theUUID + "再次下注" + betPoint);
    }

    /// <summary>
    /// 抢庄返回，多人抢庄服务器随机分配，无人抢庄则通比大小
    /// </summary>
    /// <param name="data"></param>
    public static void ServerBankerResponse(byte[] data, int index)
    {
        ServerBankerResponse res = GetProtoInstance<ServerBankerResponse>(data, index);

        bool hasOwner = res.is_banker;
        string ownerUUID = res.banker_uuid;
        uint ownerSeat = res.banker_seat;
        bool nextBet = res.is_bet;

		if (hasOwner)
		{
			MsgMng.Dispatch (MsgType.UI_ConfirmOwner, ownerUUID);
		}

        if (nextBet)
        {
            MsgMng.Dispatch(MsgType.UI_StartBet);
        }

        Debug.Log("抢庄结果");
    }

    static Dictionary<uint, string> s_cardResultDic = new Dictionary<uint, string>()
    {
        {0, "0点" },
        {1, "1点" },
        {2, "2点" },
        {3, "3点" },
        {4, "4点" },
        {5, "5点" },
        {6, "6点" },
        {7, "7点" },
        {8, "8点" },
        {9, "9点" },
        {10, "混三公" },
        {11, "小三公" },
        {12, "大三公" },
    };
    /// <summary>
    /// 发牌
    /// </summary>
    /// <param name="data"></param>
    public static void ServerCardsResponse(byte[] data, int index)
    {
        ServerCardsResponse res = GetProtoInstance<ServerCardsResponse>(data, index);
        List<uint> cards = new List<uint>();
        res.card.ForEach((x) => { cards.Add(x.card); });

        if(cards.Count > 3)
        {
            Debug.LogError("发了" + cards.Count + "张牌");
            cards.RemoveRange(3, cards.Count - 3);
        }

        /*
         * 0:0点	 1: 1点   	2:2点 	3: 3点 	 4:4点   	 5: 5点   	6:6点	7: 7点  	8:8点  	 9:9点    	10：混三公  	11：小三公 	 12：大三公 
         */

        UserData playerData = Globals.SceneSingleton<DataMng>().GetData<UserData>(DataType.Player);
        playerData.cards = cards;
        playerData.cardResult = s_cardResultDic[res.card_result];
        
        uint expireTime = res.expire_seconds;
        MsgMng.Dispatch(MsgType.UpdateClock, expireTime);

        MsgMng.Dispatch(MsgType.DealCards);

        Debug.Log("结果" + s_cardResultDic[res.card_result]);
    }

    /// <summary>
    /// 亮牌
    /// </summary>
    /// <param name="data"></param>
    public static void SynchroniseCardsResponse(byte[] data, int index)
    {
        SynchroniseCardsResponse res = GetProtoInstance<SynchroniseCardsResponse>(data, index);
        string theUUID = res.player;
        uint theSeat = res.seat;
        


        List<uint> cards = new List<uint>();
        res.card.ForEach((x) => { { cards.Add(x.card); } });

        uint cardResult = res.card_face;

		MsgMng.Dispatch (MsgType.SynchroniseCards, cards, s_cardResultDic [cardResult], theUUID);

        Debug.Log("亮牌" + cardResult);
    }

    /// <summary>
    /// 比大小
    /// </summary>
    /// <param name="data"></param>
    public static void WinOrLoseResponse(byte[] data, int index)
    {
        WinOrLoseResponse res = GetProtoInstance<WinOrLoseResponse>(data, index);
        List<ResultEffectData> results = new List<ResultEffectData>();
        res.result.ForEach((x) => 
        {
            ResultEffectData e = new ResultEffectData();
            e.FromUser = x.from_user_uuid;
            e.ToUser = x.to_user_uuid;
            e.Point = x.point;
            results.Add(e);
        });

        uint expireTime = res.expire_seconds;
        MsgMng.Dispatch(MsgType.UpdateClock, expireTime);

        MsgMng.Dispatch(MsgType.Compare, results, expireTime);
        Debug.Log("比大小" + res.result);
    }

    /// <summary>
    /// 开始游戏
    /// </summary>
    /// <param name="data"></param>
    public static void StartGameResponse(byte[] data, int index)
    {
        StartGameResponse res = GetProtoInstance<StartGameResponse>(data, index);

        if(res.code != 1)
        {
            return;
        }
        uint expireTime = res.expire_seconds;
        MsgMng.Dispatch(MsgType.UpdateClock, expireTime);
        // 开始游戏，准备下注
        Debug.Log("开始游戏，准备下注");
        MsgMng.Dispatch(MsgType.UI_StartBet, expireTime);
    }

    /// <summary>
    /// 下注结束广播
    /// </summary>
    /// <param name="data"></param>
    public static void ServerBetOverResponse(byte[] data, int index)
    {
        ServerBetOverResponse res = GetProtoInstance<ServerBetOverResponse>(data, index);

        if(res.code == 0)
        {
            return;
        }

        // 时间到了

    }

    /// <summary>
    /// 总结算
    /// </summary>
    /// <param name="data"></param>
    public static void UserAllResult(byte[] data, int index)
    {
        UserAllResult res = GetProtoInstance<UserAllResult>(data, index);

        List<ResultUser> results = new List<ResultUser>();
        res.allResult.ForEach((x) => {
            ResultUser r = new ResultUser();
            r.Name = x.nick_name;
            r.Icon = x.head_img;
            r.Point = x.point;
            r.UUID = x.uuid;
            r.WinCount = x.win_count;
            results.Add(r);
        });

        MsgMng.Dispatch(MsgType.GameEnd, results);


        Debug.LogError("结束啦！！！");
        Debug.Log("总结算" + res.allResult.Count);
    }

    /// <summary>
    /// 抢庄广播
    /// </summary>
    /// <param name="data"></param>
    public static void ServerToBankerResponse(byte[] data, int index)
    {
        ServerToBankerResponse res = GetProtoInstance<ServerToBankerResponse>(data, index);

        Debug.Log("抢庄 " + res.uuid);
    }

    public static T GetProtoInstance<T>(byte[] data, int index)
    {
        T t = default(T);
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
