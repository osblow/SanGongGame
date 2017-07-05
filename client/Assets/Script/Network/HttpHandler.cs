using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using com.sansanbbox.protobuf;

namespace Osblow.App
{
    public class HttpHandler
    {
        public static void LoginResponse(byte[] data)
        {
            LoginResponse res = GetProtoInstance<LoginResponse>(data);

            UserData userData = new UserData();
            if (res != null)
            {
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
            }

            Globals.SceneSingleton<DataMng>().SetData(DataType.Player, userData);

            LobbyUIContext context = new LobbyUIContext();
            context.UserData = userData;
            Globals.SceneSingleton<ContextManager>().Push(context);
        }

        public static void ExistRoomWebResponse(byte[] data)
        {
            ExistRoomWebResponse res = GetProtoInstance<ExistRoomWebResponse>(data);

            bool isRoomExist = res.flag;
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
