using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Osblow.App
{
    public class HttpRequest
    {
        public static void LoginRequest()
        {
            string url = Globals.Instance.Settings.WebUrlBase + "loginAction/login.do";

            // test
            url += "?unionId=0000000000";
            ///////////

            WWWForm form = new WWWForm();
            form.AddField("unionId", 0);
            Globals.SceneSingleton<HttpNetworkMng>().Send(url, form, HttpHandler.LoginResponse);
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
