﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 *	
 *  Define View's Path And Name
 *
 *	by Xuanyi
 *
 */

namespace Osblow.App
{
	public class UIType {

        public string Path { get; private set; }

        public string Name { get; private set; }

        public UIType(string path)
        {
            Path = path;
            Name = path.Substring(path.LastIndexOf('/') + 1);
        }

        public override string ToString()
        {
            return string.Format("path : {0} name : {1}", Path, Name);
        }

        public static readonly UIType LoginView = new UIType("Prefab/UI/LoginView");
        public static readonly UIType LobbyView = new UIType("Prefab/UI/LobbyView");
        public static readonly UIType TableView = new UIType("Prefab/UI/TableView");
        public static readonly UIType CreateRoomView = new UIType("Prefab/UI/CreateRoomView");
        public static readonly UIType InviteCodeDialog = new UIType("Prefab/UI/InviteCodeDialog");
        public static readonly UIType EnterRoomView = new UIType("Prefab/UI/EnterRoomView");
        public static readonly UIType LoadingView = new UIType("Prefab/UI/LoadingView");
        public static readonly UIType GameResultView = new UIType("Prefab/UI/GameResultView");
        public static readonly UIType HistoryView = new UIType("Prefab/UI/HistoryView");
        public static readonly UIType MessageView = new UIType("Prefab/UI/MessageView");
        public static readonly UIType ShareView = new UIType("Prefab/UI/ShareView");
        public static readonly UIType SettingView = new UIType("Prefab/UI/SettingView");
        public static readonly UIType PayView = new UIType("Prefab/UI/PayView");
        public static readonly UIType UserInfoDialog = new UIType("Prefab/UI/UserInfoDialog");
        
        public static readonly UIType AlertDialog = new UIType("Prefab/UI/AlertDialog");
        public static readonly UIType HistoryDetail = new UIType("Prefab/UI/HistoryDetailedView");
        public static readonly UIType ContactUsView = new UIType("Prefab/UI/ContactUsView");
        public static readonly UIType RuleView = new UIType("Prefab/UI/RuleView");

        public static readonly UIType GameLoadingView = new UIType("Prefab/UI/GameLoadingView");
    }
}
