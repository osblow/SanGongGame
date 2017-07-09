using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Osblow.App;

namespace Osblow.Util
{
    public enum MsgType
    {
        None = 0,
        OtherPlayerEnter = 1, // 有其它玩家进入

        Connected = 2, // socket 连接成功
        Registed = 3, // socket 注册成功
        Ready = 4, // 准备

        PlayerEnter = 5, // 玩家进入
        UI_AddPlayer = 6, 

        UI_PlayerReady = 7,
        UI_ShowCards = 8,

        UI_ConfirmOwner = 9,
        UI_Bankering = 10,

        UI_PlayerBet = 11,

        UI_StartBet = 12, // 游戏开始，准备下注

        UI_Expression = 13, // 表情

        UI_CuoPaiEnd = 14, // 搓牌结束 

        DealCards = 15, // 发牌
    }



    public class MsgMng
    {
        private static MsgMng s_instance;
        public static MsgMng Instance
        {
            get
            {
                if(s_instance == null)
                {
                    s_instance = new MsgMng();
                }

                return s_instance;
            }
        }


        private Dictionary<MsgType, Action<Msg>> m_msgDic = new Dictionary<MsgType, Action<Msg>>();

        private MsgMng()
        {

        }

        public void _addListener(MsgType type, Action<Msg> listener)
        {
            if (!m_msgDic.ContainsKey(type))
            {
                m_msgDic.Add(type, listener);
            }
            else
            {
                m_msgDic[type] += listener;
            }
        }

        public void _dispatch(MsgType type, params object[] args)
        {
            if (!m_msgDic.ContainsKey(type))
            {
                return;
            }

            Msg msg = new Msg();
            msg.Params = args;

            m_msgDic[type].Invoke(msg);
        }

        public void _removeListener(MsgType type, Action<Msg> listener)
        {
            if (!m_msgDic.ContainsKey(type))
            {
                return;
            }

            m_msgDic[type] -= listener;
        }




        public static void AddListener(MsgType type, Action<Msg> listener)
        {
            Instance._addListener(type, listener);
        }

        public static void Dispatch(MsgType type, params object[] args)
        {
            Instance._dispatch(type, args);
        }

        public static void RemoveListener(MsgType type, Action<Msg> listener)
        {
            Instance._removeListener(type, listener);
        }


    }


    public class Msg
    {
        public object[] Params;

        public T Get<T>(int index)
        {
            return (T)Params[index];
        }
    }
}
