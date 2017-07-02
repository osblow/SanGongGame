using UnityEngine;
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
    }
}
