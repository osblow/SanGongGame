using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using Osblow.Util;
using com.sansanbbox.protobuf;
using com.sansanbbox.protobuf.lobby;

namespace Osblow.App
{
    public class HttpHandler
    {
        /// <summary>
        /// 登录返回
        /// </summary>
        /// <param name="data"></param>
        public static void LoginResponse(byte[] data)
        {
            LoginResponse res = GetProtoInstance<LoginResponse>(data);

            UserData userData = new UserData();
            userData.uuid = res.uuid;
            userData.user_nick_name = res.user_nick_name;
            userData.user_head_img = res.user_head_img;
            userData.account_id = res.account_id;
            userData.login_ip = res.login_ip;
            userData.play_count = res.play_count;
            userData.register_time = res.register_time;
            userData.is_enter_room = res.is_enter_room;
            userData.evaluate_score = res.evaluate_score;
            userData.user_diamond = res.user_diamond;
            userData.notice_message = res.notice_message;

            Globals.SceneSingleton<DataMng>().SetData(DataType.Player, userData);

            LobbyUIContext context = new LobbyUIContext();
            context.UserData = userData;
            Globals.SceneSingleton<ContextManager>().Push(context);
        }


        /// <summary>
        /// 查询房间返回
        /// </summary>
        /// <param name="data"></param>
        public static void ExistRoomWebResponse(byte[] data)
        {
            ExistRoomWebResponse res = GetProtoInstance<ExistRoomWebResponse>(data);

            bool isRoomExist = res.flag;

            Debug.Log("房间存在" + isRoomExist);

            if (isRoomExist)
            {
                Globals.SceneSingleton<AsyncInvokeMng>().Events.Add(delegate ()
                {
                    Globals.SceneSingleton<StateMng>().ChangeState(StateType.Game);
                });
            }
        }

        /// <summary>
        /// 创建房间返回
        /// </summary>
        /// <param name="data"></param>
        public static void CreateRoomWebResponse(byte[] data)
        {
            CreateRoomWebResponse res = GetProtoInstance<CreateRoomWebResponse>(data);

            //if(res.code == 1)
            //{
            //    UnityEngine.Debug.Log("创建房间失败");
            //    return;
            //}

            //Debug.Log("创建房间成功， 房间号" + res.room_id);
            Globals.SceneSingleton<DataMng>().GetData<RoomData>(DataType.Room).RoomId = res.room_id;

            Globals.SceneSingleton<AsyncInvokeMng>().Events.Add(delegate () 
            {
                Globals.SceneSingleton<StateMng>().ChangeState(StateType.Game);
            });
        }

        /// <summary>
        /// 新闻请求返回
        /// </summary>
        /// <param name="data"></param>
        public static void NewsResponse(byte[] data)
        {
            NewsResponse res = GetProtoInstance<NewsResponse>(data);

            MessageData lobbyData = new MessageData();
            lobbyData.NewsTitle = res.rule_title;
            lobbyData.News = res.news_message;

            MessageUIContext context = new MessageUIContext();
            context.Data = lobbyData;
            Globals.SceneSingleton<ContextManager>().Push(context);
            Debug.Log("收到新闻：" + lobbyData.News);
        }

        /// <summary>
        /// 邀请码界面展示返回
        /// </summary>
        /// <param name="data"></param>
        public static void InviteCodeViewResponse(byte[] data)
        {
            InvitationRuleResponse res = GetProtoInstance<InvitationRuleResponse>(data);

            InviteCodeUIContext context = new InviteCodeUIContext();
            context.Binded = res.code == 0;
            context.Notice = res.invitation_rule_message;
            context.BindedCode = res.invitation_code;

            Globals.SceneSingleton<ContextManager>().Push(context);
            Debug.Log("进入邀请码界面");
        }

        /// <summary>
        /// 绑定邀请码返回
        /// </summary>
        /// <param name="data"></param>
        public static void BindInviteCodeResponse(byte[] data)
        {
            InvitationResponse res = GetProtoInstance<InvitationResponse>(data);
            
            if (res.code == 1)
            {
                Debug.Log("绑定失败");
            }
            else
            {
                Debug.Log("绑定成功");
            }
            AlertUIContext context = new AlertUIContext();
            context.Info = res.message;
            context.HasOK = true;

            Globals.SceneSingleton<ContextManager>().Push(context);

        }

        /// <summary>
        /// 战绩返回
        /// </summary>
        /// <param name="data"></param>
        public static void GameRecordResponse(byte[] data)
        {
            /*message  GameRecord{
	required  uint32  	room_number =1;			//房间号
	required  uint32  game_count=2;			//游戏总局数
	required  string   room_rule_name=3;		//房间规则名称
	required  string   game_time=4;			//时间
	required  uint32  	room_id =5;				//房间唯一标识
	message		Player {		// 其它玩家信息
       	required		bool	    is_owner = 1;		// 是否是房主
	   	optional		uint32	account_id=2;		// 玩家主键ID
		optional		string	player_head_img=3;	// 玩家头像
		optional		string	player_nike_name=4;	// 玩家昵称
		optional		sint32	point=5;			//总成绩
	}
	repeated Player player=6;
}

             */

            GameRecordList res = GetProtoInstance<GameRecordList>(data);
            if (!res.result)
            {
                Debug.Log("没有记录");
                return;
            }

            GameRecords record = new GameRecords();
            record.Records = new List<SingleGameRecord>();
            res.game_record.ForEach((x) => 
            {
                SingleGameRecord theRecord = new SingleGameRecord();
                theRecord.RoomNumber = x.room_number;
                theRecord.RoomId = x.room_id;
                theRecord.RoomRuleName = x.room_rule_name;
                theRecord.TotalRound = x.game_count;
                theRecord.GameTime = x.game_time;
                theRecord.Players = new List<RecordPlayer>();
                x.player.ForEach((y) => 
                {
                    RecordPlayer player = new RecordPlayer();
                    player.AccountId = y.account_id;
                    player.Icon = y.player_head_img;
                    player.IsOwner = y.is_owner;
                    player.NickName = y.player_nike_name;
                    player.Score = y.point;
                    theRecord.Players.Add(player);
                });
                record.Records.Add(theRecord);
            });


            HistoryUIContext context = new HistoryUIContext();
            context.Records = record;
            Globals.SceneSingleton<ContextManager>().Push(context);
        }

        /// <summary>
        /// 单局战绩返回
        /// </summary>
        /// <param name="data"></param>
        public static void SmallGameRecordResponse(byte[] data)
        {
            UserGameRecodeList res = GetProtoInstance<UserGameRecodeList>(data);

            if (!res.result)
            {
                Debug.Log("");
                return;
            }

            SmallGameRecord record = new SmallGameRecord();
            record.Players = new List<RecordPlayer>();
            record.SingleRecords = new List<SingleSmallGameRecord>();

            res.user_Name.ForEach((x) => 
            {
                RecordPlayer player = new RecordPlayer();
                player.NickName = x.user_name;
                player.Score = x.point;
                record.Players.Add(player);
            });

            res.user_game_recode.ForEach((x) => 
            {
                SingleSmallGameRecord theRecord = new SingleSmallGameRecord();
                theRecord.Index = x.game_index_name;
                theRecord.Players = new List<SmallGameRecPlayer>();
                x.user.ForEach((y) => 
                {
                    SmallGameRecPlayer player = new SmallGameRecPlayer();
                    player.IsBanker = y.is_bank;
                    player.Point = y.point;
                    player.CardResult = y.card_face;
                    player.Cards = new List<uint>();
                    y.card.ForEach((z) => 
                    {
                        player.Cards.Add(z.card);
                    });
                    theRecord.Players.Add(player);
                });
                record.SingleRecords.Add(theRecord);
            });

            //MsgMng.Dispatch(MsgType.LobbySingleGameRecord, record);
            HistoryDetailUIContext context = new HistoryDetailUIContext();
            context.Data = record;
            Globals.SceneSingleton<ContextManager>().Push(context);

            Debug.Log("收到详细战绩");
        }

        /// <summary>
        /// 客服信息返回
        /// </summary>
        /// <param name="data"></param>
        public static void ContactUsResponse(byte[] data)
        {
            CustomerResponse res = GetProtoInstance<CustomerResponse>(data);

            List<ContactUsData> contactDatas = new List<ContactUsData>();
            res.customer.ForEach((x) => 
            {
                ContactUsData theData = new ContactUsData();
                theData.Key = x.name;
                theData.Value = x.value;
                contactDatas.Add(theData);
            });

            ContactUsUIContext context = new ContactUsUIContext();
            context.Datas = contactDatas;
            Globals.SceneSingleton<ContextManager>().Push(context);
            Debug.Log("客服信息返回");
        }

        /// <summary>
        /// 充值信息返回
        /// </summary>
        /// <param name="data"></param>
        public static void RechargeRecordResponse(byte[] data)
        {
            RechargeResponseList res = GetProtoInstance<RechargeResponseList>(data);

            List<RechargeData> rechargeDatas = new List<RechargeData>();
            res.recharge_response.ForEach((x) => 
            {
                RechargeData theData = new RechargeData();
                theData.Number = x.recharge_no;
                theData.Count = x.recharge_count;
                theData.Money = x.recharge_money;
                theData.Status = x.recharge_state;
                theData.Time = x.recharge_time;
                rechargeDatas.Add(theData);
            });

            PayUIContext context = new PayUIContext();
            context.Records = rechargeDatas;
            Globals.SceneSingleton<ContextManager>().Push(context);
        }

        /// <summary>
        /// 规则信息返回
        /// </summary>
        /// <param name="data"></param>
        public static void RuleResponse(byte[] data)
        {
            GameRuleResponseList res = GetProtoInstance<GameRuleResponseList>(data);

            List<RuleData> rules = new List<RuleData>();

            res.game_rule_response.ForEach((x) => 
            {
                RuleData theData = new RuleData();
                theData.Tile = x.rule_title;
                theData.Content = x.rule_message;
                rules.Add(theData);    
            });

            RuleUIContext context = new RuleUIContext();
            context.Datas = rules;
            Globals.SceneSingleton<ContextManager>().Push(context);

            Debug.Log("收到规则。。");
        }

        /// <summary>
        /// 房间列表
        /// </summary>
        public static void RoomListResponse(byte[] data)
        {
            RoomList res = GetProtoInstance<RoomList>(data);

            List<LobbyRoomData> rooms = new List<LobbyRoomData>();
            res.room.ForEach((x) => 
            {
                LobbyRoomData theData = new LobbyRoomData();
                theData.RoomNumber = x.room_number;
                theData.UserCount = x.all_user;
                theData.TotalRound = x.all_round;
                rooms.Add(theData);
            });

            MsgMng.Dispatch(MsgType.RoomList, rooms);
            Debug.Log("收到房间列表");
        }

        public static T GetProtoInstance<T>(byte[] data)
        {
            T t = default(T);
            

            using (MemoryStream ms = new MemoryStream())
            {
                //将消息写入流中
                ms.Write(data, 0, data.Length);
                //将流的位置归0
                ms.Position = 0;
                //使用工具反序列化对象
                t = ProtoBuf.Serializer.Deserialize<T>(ms);
            }

            return t;
        }
    }
}
