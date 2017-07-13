using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Osblow.Util;
using Osblow.App;

public class GameMng : MonoBehaviour 
{
    public List<TablePlayerData> Users = new List<TablePlayerData>();
    public Dictionary<string, TablePlayerData> UsersDic = new Dictionary<string, TablePlayerData>();

    public bool IsGaming = false;

    private void Awake()
    {
        MsgMng.AddListener(MsgType.PlayerEnter, OnplayerEner);
    }

    void OnplayerEner(Msg msg)
    {
        string uuid = msg.Get<string>(0);

        UserData playerData = Globals.SceneSingleton<DataMng>().GetData<UserData>(DataType.Player);

        if (playerData.uuid == uuid || UsersDic.ContainsKey(uuid))
        {
            return;
        }

        TablePlayerData newData = msg.Get<TablePlayerData>(1);
        UsersDic.Add(uuid, newData);
        MsgMng.Dispatch(MsgType.UI_AddPlayer, uuid);
    }

    public void ClearAll()
    {
        Users.Clear();
        UsersDic.Clear();
    }
}
