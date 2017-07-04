using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Osblow.App
{
    public class LobbyUIContext : BaseContext
    {
        public LobbyUIContext() : base(UIType.LobbyView) { }

        public UserData UserData;
    }

    public class LobbyView : BaseView
    {
        #region 场景引用 
        public Text UserName;
        public Text UserId;
        public Text Diamond;
        #endregion

        #region 场景事件
        public void CreateRoomBtn()
        {
            ShowCreateView();
        }

        public void EnterRoomBtn()
        {
            Globals.SceneSingleton<ContextManager>().Push(new EnterRoomUIContext());
        }

        public void InviteCodeBtn()
        {
            ShowInviteCodeDialog();
        }
        #endregion
        void ShowCreateView()
        {
            Globals.SceneSingleton<ContextManager>().Push(new CreateRoomUIContext());
        }

        void ShowInviteCodeDialog()
        {
            Globals.SceneSingleton<ContextManager>().Push(new InviteCodeUIContext());
        }





        public override void OnEnter(BaseContext context)
        {
            base.OnEnter(context);
            LobbyUIContext lobbyContext = context as LobbyUIContext;
            UserData user = lobbyContext.UserData;
            if (user == null)
            {
                user = Globals.SceneSingleton<DataMng>().GetData<UserData>(DataType.Player);
            }
            UserName.text = user.user_nick_name;
            UserName.text = user.account_id.ToString();
            Diamond.text = user.user_diamond.ToString();
        }

        public override void OnExit(BaseContext context)
        {
            base.OnExit(context);
        }

        public override void OnPause(BaseContext context)
        {
            base.OnPause(context);
        }

        public override void OnResume(BaseContext context)
        {
            base.OnResume(context);
        }
    }
}