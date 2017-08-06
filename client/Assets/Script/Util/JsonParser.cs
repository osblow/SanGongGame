using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using LitJson;

public class JsonParser 
{
    public static string GetUnionIdFromResponse(string response)
    {
        JsonData json = JsonMapper.ToObject(response);

        

        return json["unionid"].ToString();
    }

    public static AuthData GetAuthInfoFromResponse(string response)
    {
        AuthData result = new AuthData();
        
        JsonData json = JsonMapper.ToObject(response);
        result.openid = json["openid"].ToString();
        result.nickname = json["nickname"].ToString();
        result.sex = int.Parse(json["sex"].ToString());
        result.province = json["province"].ToString();
        result.city = json["city"].ToString();
        result.country = json["country"].ToString();
        result.headimgurl = json["headimgurl"].ToString();
        result.unionid = json["unionid"].ToString();

        

        return result;
    }


    /// <summary>  
            /// 字符串转为UniCode码字符串  
            /// </summary>  
            /// <param name="s"></param>  
            /// <returns></returns>  
    public static string StringToUnicode(string s)
    {
        char[] charbuffers = s.ToCharArray();
        byte[] buffer;
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < charbuffers.Length; i++)
        {
            buffer = System.Text.Encoding.Unicode.GetBytes(charbuffers[i].ToString());
            sb.Append(string.Format("//u{0:X2}{1:X2}", buffer[1], buffer[0]));
        }
        return sb.ToString();
    }

    public static string UrlEncode(string url)
    {
        char[] chs = url.ToCharArray();

        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < chs.Length; i++)
        {
            char c = chs[i];
            if((c >= 'a' && c <= 'z') ||
                (c >= 'A' && c <= 'Z') ||
                (c >= '0' && c <= '9') ||
                c == '-' || c == ':' || c == '/' || c == '?' || c == '&')
            {
                builder.Append(c);
            }
            else if(c == ' ')
            {
                builder.Append('+');
            }
            else
            {
                byte[] bytes = Encoding.Unicode.GetBytes(c.ToString());
                builder.Append('%').Append(string.Format("{0:X2}{1:X2}", bytes[1], bytes[0]));
            }
        }

        return builder.ToString();
    }
}
