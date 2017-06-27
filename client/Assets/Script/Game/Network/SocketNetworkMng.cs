using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Osblow.Game
{
    public class SocketNetworkMng : MonoBehaviour
    {
        private CmdHandler m_handler;
        private Dictionary<short, Action<byte[]>> m_hadlerDic = new Dictionary<short, Action<byte[]>>();


        private void Awake()
        {
            m_hadlerDic.Add(Cmd.ServerRegisterResponse, CmdHandler.ServerRegisterResponse);
            m_hadlerDic.Add(Cmd.ServerHeartbeatResponse, CmdHandler.ServerHeartbeatResponse);
            m_hadlerDic.Add(Cmd.EnterRoomResponse, CmdHandler.EnterRoomResponse);
            m_hadlerDic.Add(Cmd.EnterRoomOtherResponse, CmdHandler.EnterRoomOtherResponse);
            m_hadlerDic.Add(Cmd.ExitRoomToOtherResponse, CmdHandler.ExitRoomToOtherResponse);
            m_hadlerDic.Add(Cmd.ExitRoomResultResponse, CmdHandler.ExitRoomResultResponse);
            m_hadlerDic.Add(Cmd.DismissRoomToOtherResponse, CmdHandler.DismissRoomToOtherResponse);
            m_hadlerDic.Add(Cmd.PlayerVoteDismissRoomResponse, CmdHandler.PlayerVoteDismissRoomResponse);
            m_hadlerDic.Add(Cmd.DismissRoomResultResponse, CmdHandler.DismissRoomResultResponse);
            m_hadlerDic.Add(Cmd.ReadyResponse, CmdHandler.ReadyResponse);
            m_hadlerDic.Add(Cmd.ReconnectResponse, CmdHandler.ReconnectResponse);
            m_hadlerDic.Add(Cmd.OnlineStatusResponse, CmdHandler.OnlineStatusResponse);
            m_hadlerDic.Add(Cmd.SynchroniseExpressionResponse, CmdHandler.SynchroniseExpressionResponse);
            m_hadlerDic.Add(Cmd.AudioStreamBroadcast, CmdHandler.AudioStreamBroadcast);
            m_hadlerDic.Add(Cmd.ServerBetResponse, CmdHandler.ServerBetResponse);
            m_hadlerDic.Add(Cmd.ServerBankerNotice1Response, CmdHandler.ServerBankerNotice1Response);
            m_hadlerDic.Add(Cmd.ServerBankerNotice2Response, CmdHandler.ServerBankerNotice2Response);
            m_hadlerDic.Add(Cmd.ServerBetAgainResponse, CmdHandler.ServerBetAgainResponse);
            m_hadlerDic.Add(Cmd.ServerBankerResponse, CmdHandler.ServerBankerResponse);
            m_hadlerDic.Add(Cmd.ServerCardsResponse, CmdHandler.ServerCardsResponse);
            m_hadlerDic.Add(Cmd.SynchroniseCardsResponse, CmdHandler.SynchroniseCardsResponse);
            m_hadlerDic.Add(Cmd.WinOrLoseResponse, CmdHandler.WinOrLoseResponse);
            m_hadlerDic.Add(Cmd.UserAllResult, CmdHandler.UserAllResult);
        }

        public void Execute(short cmd, byte[] data)
        {
            if (!m_hadlerDic.ContainsKey(cmd))
            {
                return;
            }

            m_hadlerDic[cmd].Invoke(data);
        }
    }
}
