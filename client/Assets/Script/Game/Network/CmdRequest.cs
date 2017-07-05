﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Osblow.App;
using com.sansanbbox.protobuf;

namespace Osblow.Game
{
    public class CmdRequest
    {
        /// <summary>
        /// 游戏服注册
        /// </summary>
        public static void ClientRegisterRequest()
        {
            UserData playerdata = Globals.SceneSingleton<DataMng>().GetData<UserData>(DataType.Player);

            ClientRegisterRequest request = new com.sansanbbox.protobuf.ClientRegisterRequest();
            request.uuid = playerdata.uuid;

            Serialize(request);
        }

        /// <summary>
        /// 心跳
        /// </summary>
        public static void ClientHeartBeatRequest()
        {
            UserData playerdata = Globals.SceneSingleton<DataMng>().GetData<UserData>(DataType.Player);

            ClientHeartBeatRequest request = new com.sansanbbox.protobuf.ClientHeartBeatRequest();
            request.uuid = playerdata.uuid;

            Serialize(request);
        }

        /// <summary>
        /// 进入房间
        /// </summary>
        public static void EnterRoomRequest()
        {
            UserData playerdata = Globals.SceneSingleton<DataMng>().GetData<UserData>(DataType.Player);

            EnterRoomRequest request = new com.sansanbbox.protobuf.EnterRoomRequest();
            request.uuid = playerdata.uuid;
            request.room_id = 0;

            Serialize(request);
        }

        /// <summary>
        /// 退出房间
        /// </summary>
        public static void ExitRoomRequest()
        {
            UserData playerdata = Globals.SceneSingleton<DataMng>().GetData<UserData>(DataType.Player);

            ExitRoomRequest request = new com.sansanbbox.protobuf.ExitRoomRequest();
            request.uuid = playerdata.uuid;

            Serialize(request);
        }

        /// <summary>
        /// 解散房间
        /// </summary>
        public static void DismissRoomRequest()
        {
            UserData playerdata = Globals.SceneSingleton<DataMng>().GetData<UserData>(DataType.Player);

            DismissRoomRequest request = new com.sansanbbox.protobuf.DismissRoomRequest();
            request.uuid = playerdata.uuid;

            Serialize(request);
        }

        /// <summary>
        /// 解散房间投票
        /// </summary>
        /// <param name="isAgreeing">是否同意解散</param>
        public static void PlayerVoteDismissRoomRequest(bool isAgreeing)
        {
            UserData playerdata = Globals.SceneSingleton<DataMng>().GetData<UserData>(DataType.Player);

            PlayerVoteDismissRoomRequest request = new com.sansanbbox.protobuf.PlayerVoteDismissRoomRequest();
            request.uuid = playerdata.uuid;
            request.flag = isAgreeing;

            Serialize(request);
        }

        /// <summary>
        /// 准备
        /// </summary>
        public static void ReadyRequest()
        {
            UserData playerdata = Globals.SceneSingleton<DataMng>().GetData<UserData>(DataType.Player);

            ReadyRequest request = new com.sansanbbox.protobuf.ReadyRequest();
            request.uuid = playerdata.uuid;

            Serialize(request);
        }

        /// <summary>
        /// 断线重连请求
        /// </summary>
        public static void ReconnectRequest()
        {
            UserData playerdata = Globals.SceneSingleton<DataMng>().GetData<UserData>(DataType.Player);

            ReconnectRequest request = new com.sansanbbox.protobuf.ReconnectRequest();
            request.uuid = playerdata.uuid;

            Serialize(request);
        }

        /// <summary>
        /// 上传语音
        /// </summary>
        public static void AudioStreamUpload()
        {
            // 需要ID
        }

        /// <summary>
        /// 下注
        /// </summary>
        public static void ClientBetRequest(uint betPoint)
        {
            UserData playerdata = Globals.SceneSingleton<DataMng>().GetData<UserData>(DataType.Player);

            ClientBetRequest request = new com.sansanbbox.protobuf.ClientBetRequest();

            request.uuid = playerdata.uuid;
            request.bet_point = betPoint;

            Serialize(request);
        }

        /// <summary>
        /// 再次下注
        /// </summary>
        public static void ClientBetAgainRequest(uint betPoint)
        {
            UserData playerdata = Globals.SceneSingleton<DataMng>().GetData<UserData>(DataType.Player);

            ClientBetAgainRequest request = new com.sansanbbox.protobuf.ClientBetAgainRequest();
            request.uuid = playerdata.uuid;
            request.bet_point = betPoint;

            Serialize(request);
        }

        /// <summary>
        /// 抢庄
        /// </summary>
        public static void ClientBankerRequest()
        {
            UserData playerdata = Globals.SceneSingleton<DataMng>().GetData<UserData>(DataType.Player);

            ClientBankerRequest request = new com.sansanbbox.protobuf.ClientBankerRequest();
            request.uuid = playerdata.uuid;

            Serialize(request);
        }

        static void Serialize(ProtoBuf.IExtensible proto)
        {
            byte[] result;
            //涉及格式转换，需要用到流，将二进制序列化到流中
            using (MemoryStream ms = new MemoryStream())
            {
                //使用ProtoBuf工具的序列化方法
                ProtoBuf.Serializer.Serialize(ms, proto);
                //定义二级制数组，保存序列化后的结果
                result = new byte[ms.Length];
                //将流的位置设为0，起始点
                ms.Position = 0;
                //将流中的内容读取到二进制数组中
                ms.Read(result, 0, result.Length);
            }


            List<byte> data = new List<byte>();

            byte head = 0x00;
            short cmd = 0x1001;
            short length = (short)result.Length;
            byte tail = 0x00;

            data.Add(head);
            data.AddRange(BitConverter.GetBytes(cmd));
            data.AddRange(BitConverter.GetBytes(length));
            data.AddRange(result);
            data.Add(tail);

            Osblow.App.Globals.SceneSingleton<SocketNetworkMng>().Send(data.ToArray());
        }
    }
}
