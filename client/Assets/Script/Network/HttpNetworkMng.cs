using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HttpNetworkMng : MonoBehaviour 
{
    private const string c_testUrl = "http://bbox.sansanbbox.com:6080/loginAction/login.do?unionId=0000000000";

    struct SingleRequest
    {
        public string Url;
        public WWWForm Form;
    }
    private List<SingleRequest> m_requestList = new List<SingleRequest>();
    private bool m_isWaiting = false;


    public void Send(string url, WWWForm form, Action<byte[]> handler)
    {
        if (!m_isWaiting)
        {
            StartCoroutine(GetBytes(url, form, handler));
        }
    }

    IEnumerator GetBytes(string url, WWWForm form, Action<byte[]> handler)
    {
        m_isWaiting = true;

        WWW www = new WWW(c_testUrl);
        //WWW www = new WWW(url, form);
        yield return www;
        Debug.Log(www.bytes.Length);
        m_isWaiting = false;

        if(handler != null)
        {
            handler.Invoke(www.bytes);
        }
        /*
        using (MemoryStream ms = new MemoryStream())
        {
            //将消息写入流中
            ms.Write(www.bytes, 0, www.bytes.Length);
            //将流的位置归0
            ms.Position = 0;
            //使用工具反序列化对象
            LoginResponse res =
                ProtoBuf.Serializer.Deserialize<LoginResponse>(ms);
            Debug.Log("uuid = " + res.uuid + " " + res.login_ip);
        }
        */
        
    }
}
