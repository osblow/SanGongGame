﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData : DataBase
{
    public string uuid = "123456";
    public string user_nick_name = "小三";
    public string user_head_img = "default";
    public uint account_id = 0;
    public string login_ip = "127.0.0.1";
    public string play_count = "2";
    public string register_time = "1970.1.1";
    public uint is_enter_room = 0;
    public double evaluate_score = 1;
    public uint user_diamond = 100;
    public string notice_message = "";
    public uint sex = 1;// 1男，2女

    public List<uint> cards = new List<uint>() { 0,0,0 };
    public string cardResult = "";

    public uint socket_code = 0;

    // 主要针对旁观者模式，记录有效玩家，其它的玩家均不发牌
    public List<string> validUUIDs = new List<string>();
}


public class MessageData : DataBase
{
    public string NewsTitle = "";
    public string News = "";
}
