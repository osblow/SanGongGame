using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Osblow.App
{
    public class HttpRequest
    {
        private const string c_testUUID = "cf32ecbb64ba4daca59366d81b4ebaef";


        /// <summary>
        /// 登录请求
        /// </summary>
        public static void LoginRequest()
        {
            string username = PlayerPrefs.GetString("username");
            string url = Globals.Instance.Settings.WebUrlBase + "loginAction/login.do";

            // test
            //url += "?unionId=" + username;
            ///////////

            WWWForm form = new WWWForm();
            //form.AddField("unionId", username);
            //form.headers["Content-Type"] = "application/x-www-form-urlencoded";
            
            Globals.SceneSingleton<HttpNetworkMng>().Send(url, form, HttpHandler.LoginResponse);
        }

        /// <summary>
        /// 查询房间请求
        /// </summary>
        /// <param name="roomId"></param>
        public static void ExistRoomRequest(string roomId)
        {
            string url = Globals.Instance.Settings.WebUrlBase + "roomAction/existRoomWebRequest.do";

            RoomData roomData = new RoomData();
            roomData.RoomId = uint.Parse(roomId);
            Globals.SceneSingleton<DataMng>().SetData(DataType.Room, roomData);

            UserData playerdata = Globals.SceneSingleton<DataMng>().GetData<UserData>(DataType.Player);

            url += "?uuid=" + playerdata.uuid;
            url += "&roomId=" + roomId;

            WWWForm form = new WWWForm();
            form.AddField("uuid", playerdata.uuid);
            form.AddField("roomId", roomId);
            Globals.SceneSingleton<HttpNetworkMng>().Send(url, form, HttpHandler.ExistRoomWebResponse);
        }

        /// <summary>
        /// 创建房间请求
        /// </summary>
        public static void CreateRoomRequest()
        {
            string url = Globals.Instance.Settings.WebUrlBase + "roomAction/createRoomWebRequest.do";

            UserData playerdata = Globals.SceneSingleton<DataMng>().GetData<UserData>(DataType.Player);
            RoomData roomData = Globals.SceneSingleton<DataMng>().GetData<RoomData>(DataType.Room);

            // test 
            url += "?uuid=" + playerdata.uuid;
            url += "&roomRuleName=" + roomData.RoomRuleName;
            url += "&roomAllCount=" + roomData.RoomTotalRound;
            url += "&roomCostRule=" + roomData.RoomCostRule;
            url += "&isJoin=" + roomData.IsJoin;
            url += "&roomRuleType=" + roomData.RoomRuleType;
            url += "&maxPlayer=" + roomData.MaxPlayerCount;
            //////

            WWWForm form = new WWWForm();
            form.AddField("uuid", playerdata.uuid);
            form.AddField("roomRuleName", roomData.RoomRuleName);
            form.AddField("roomAllCount", roomData.RoomTotalRound);
            form.AddField("roomCostRule", roomData.RoomCostRule);
            form.AddField("isJoin", roomData.IsJoin);
            form.AddField("roomRuleType", roomData.RoomRuleType);
            form.AddField("maxPlayer", roomData.MaxPlayerCount);

            Globals.SceneSingleton<HttpNetworkMng>().Send(url, form, HttpHandler.CreateRoomWebResponse);
        }

        /// <summary>
        /// 新闻请求
        /// </summary>
        public static void NewsRequest()
        {
            string url = Globals.Instance.Settings.WebUrlBase + "hallAction/newsResponse.do";
            UserData playerdata = Globals.SceneSingleton<DataMng>().GetData<UserData>(DataType.Player);

            //test
            url += "?uuid=" + playerdata.uuid;
            /////////////////////

            WWWForm form = new WWWForm();
            form.AddField("uuid", playerdata.uuid);

            Globals.SceneSingleton<HttpNetworkMng>().Send(url, form, HttpHandler.NewsResponse);
        }


        /// <summary>
        /// 邀请码展示请求
        /// </summary>
        public static void InviteCodeViewRequest()
        {
            string url = Globals.Instance.Settings.WebUrlBase + "hallAction/invitationRuleResponse.do";
            UserData playerdata = Globals.SceneSingleton<DataMng>().GetData<UserData>(DataType.Player);

            //test
            url += "?uuid=" + playerdata.uuid;
            /////////////////////

            WWWForm form = new WWWForm();
            form.AddField("uuid", playerdata.uuid);

            Globals.SceneSingleton<HttpNetworkMng>().Send(url, form, HttpHandler.InviteCodeViewResponse);
        }

        /// <summary>
        /// 绑定邀请码请求
        /// </summary>
        /// <param name="code"></param>
        public static void BindInviteCodeRequest(string code)
        {
            string url = Globals.Instance.Settings.WebUrlBase + "hallAction/invitationResponse.do";
            UserData playerdata = Globals.SceneSingleton<DataMng>().GetData<UserData>(DataType.Player);

            //test
            url += "?uuid=" + playerdata.uuid;
            url += "&invitationCode" + code;
            /////////////////////

            WWWForm form = new WWWForm();
            form.AddField("uuid", playerdata.uuid);
            form.AddField("invitationCode", code);

            Globals.SceneSingleton<HttpNetworkMng>().Send(url, form, HttpHandler.BindInviteCodeResponse);
        }

        /// <summary>
        /// 战绩查询
        /// </summary>
        public static void GameRecordRequest()
        {
            string url = Globals.Instance.Settings.WebUrlBase + "hallAction/gameRecordResponse.do";
            UserData playerdata = Globals.SceneSingleton<DataMng>().GetData<UserData>(DataType.Player);

            //test
            url += "?uuid=" + playerdata.uuid;
            /////////////////////

            WWWForm form = new WWWForm();
            form.AddField("uuid", playerdata.uuid);

            Globals.SceneSingleton<HttpNetworkMng>().Send(url, form, HttpHandler.GameRecordResponse);
        }

        /// <summary>
        ///  单局查询请求
        /// </summary>
        /// <param name="roomId"></param>
        public static void SmallGameRecordRequest(string roomId)
        {
            string url = Globals.Instance.Settings.WebUrlBase + "hallAction/gameRecordDetail.do";
            UserData playerdata = Globals.SceneSingleton<DataMng>().GetData<UserData>(DataType.Player);

            //test
            url += "?uuid=" + playerdata.uuid;
            url += "&roomId=" + roomId;
            /////////////////////

            WWWForm form = new WWWForm();
            form.AddField("uuid", playerdata.uuid);
            form.AddField("roomId", roomId);

            Globals.SceneSingleton<HttpNetworkMng>().Send(url, form, HttpHandler.SmallGameRecordResponse);
        }

        /// <summary>
        /// 客服信息查询请求
        /// </summary>
        public static void ContactUsRequest()
        {
            string url = Globals.Instance.Settings.WebUrlBase + "hallAction/customerResponse.do";
            UserData playerdata = Globals.SceneSingleton<DataMng>().GetData<UserData>(DataType.Player);

            //test
            url += "?uuid=" + playerdata.uuid;
            /////////////////////

            WWWForm form = new WWWForm();
            form.AddField("uuid", playerdata.uuid);

            Globals.SceneSingleton<HttpNetworkMng>().Send(url, form, HttpHandler.ContactUsResponse);
        }

        /// <summary>
        /// 充值记录查询 
        /// </summary>
        public static void RechargeRecordRequest()
        {
            string url = Globals.Instance.Settings.WebUrlBase + "hallAction/rechargeResponseList.do";
            UserData playerdata = Globals.SceneSingleton<DataMng>().GetData<UserData>(DataType.Player);

            //test
            url += "?uuid=" + playerdata.uuid;
            /////////////////////

            WWWForm form = new WWWForm();
            form.AddField("uuid", playerdata.uuid);

            Globals.SceneSingleton<HttpNetworkMng>().Send(url, form, HttpHandler.RechargeRecordResponse);
        }

        public static void RuleRequest()
        {
            string url = Globals.Instance.Settings.WebUrlBase + "hallAction/gameRuleResponseList.do";
            UserData playerdata = Globals.SceneSingleton<DataMng>().GetData<UserData>(DataType.Player);

            //test
            url += "?uuid=" + playerdata.uuid;
            /////////////////////

            WWWForm form = new WWWForm();
            form.AddField("uuid", playerdata.uuid);

            Globals.SceneSingleton<HttpNetworkMng>().Send(url, form, HttpHandler.RuleResponse);
        }

        public static void RoomListRequest()
        {
            string url = Globals.Instance.Settings.WebUrlBase + "hallAction/roomList.do";
            UserData playerdata = Globals.SceneSingleton<DataMng>().GetData<UserData>(DataType.Player);

            //test
            url += "?uuid=" + playerdata.uuid;
            /////////////////////

            WWWForm form = new WWWForm();
            form.AddField("uuid", playerdata.uuid);

            Globals.SceneSingleton<HttpNetworkMng>().Send(url, form, HttpHandler.RoomListResponse);
        }
        #region 不一定用得到
        /*
        static byte[] Serialize(ProtoBuf.IExtensible proto)
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


            return data.ToArray();
        }*/
        #endregion
    }
}
