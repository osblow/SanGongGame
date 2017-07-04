using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using com.sansanbbox.protobuf;

public class CmdHandler
{
    public static void ServerRegisterResponse(byte[] data)
    {
        
    }

    public static void ServerHeartbeatResponse(byte[] data)
    {

    }

    public static void EnterRoomResponse(byte[] data)
    {

    }

    public static void EnterRoomOtherResponse(byte[] data)
    {

    }

    public static void ExitRoomToOtherResponse(byte[] data)
    {

    }

    public static void ExitRoomResultResponse(byte[] data)
    {

    }

    public static void DismissRoomToOtherResponse(byte[] data)
    {

    }

    public static void PlayerVoteDismissRoomResponse(byte[] data)
    {

    }

    public static void DismissRoomResultResponse(byte[] data)
    {

    }

    public static void ReadyResponse(byte[] data)
    {

    }

    public static void ReconnectResponse(byte[] data)
    {

    }

    public static void OnlineStatusResponse(byte[] data)
    {

    }

    public static void SynchroniseExpressionResponse(byte[] data)
    {

    }

    public static void AudioStreamBroadcast(byte[] data)
    {

    }

    public static void ServerBetResponse(byte[] data)
    {

    }

    public static void ServerBankerNotice1Response(byte[] data)
    {

    }

    public static void ServerBankerNotice2Response(byte[] data)
    {

    }

    public static void ServerBetAgainResponse(byte[] data)
    {

    }

    public static void ServerBankerResponse(byte[] data)
    {

    }

    public static void ServerCardsResponse(byte[] data)
    {

    }

    public static void SynchroniseCardsResponse(byte[] data)
    {

    }

    public static void WinOrLoseResponse(byte[] data)
    {

    }

    public static void UserAllResult(byte[] data)
    {

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
            T res = ProtoBuf.Serializer.Deserialize<T>(ms);
        }

        return t;
    }
}
