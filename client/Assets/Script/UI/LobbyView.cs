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
        public Image Icon;
        
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
            //ShowInviteCodeDialog();
            AlertUIContext context = new AlertUIContext();
            context.Info = "服务器正忙，请稍后再试..";
            Globals.SceneSingleton<ContextManager>().Push(context);
        }

        public void HistoryBtn()
        {
            //Globals.SceneSingleton<ContextManager>().Push(new HistoryUIContext());
            AlertUIContext context = new AlertUIContext();
            context.Info = "服务器正忙，请稍后再试..";
            Globals.SceneSingleton<ContextManager>().Push(context);
        }

        public void MessageBtn()
        {
            //Globals.SceneSingleton<ContextManager>().Push(new MessageUIContext());
            AlertUIContext context = new AlertUIContext();
            context.Info = "服务器正忙，请稍后再试..";
            Globals.SceneSingleton<ContextManager>().Push(context);
        }

        public void ShareBtn()
        {
            //Globals.SceneSingleton<ContextManager>().Push(new ShareUIContext());
            AlertUIContext context = new AlertUIContext();
            context.Info = "服务器正忙，请稍后再试..";
            Globals.SceneSingleton<ContextManager>().Push(context);
        }

        public void SettingBtn()
        {
            //Globals.SceneSingleton<ContextManager>().Push(new SettingUIContext());
            AlertUIContext context = new AlertUIContext();
            context.Info = "服务器正忙，请稍后再试..";
            Globals.SceneSingleton<ContextManager>().Push(context);
        }

        public void PayBtn()
        {
            //Globals.SceneSingleton<ContextManager>().Push(new PayUIContext());
            AlertUIContext context = new AlertUIContext();
            context.Info = "服务器正忙，请稍后再试..";
            Globals.SceneSingleton<ContextManager>().Push(context);
        }

        public void MyRoomsBtn()
        {
            AlertUIContext context = new AlertUIContext();
            context.Info = "服务器正忙，请稍后再试..";
            Globals.SceneSingleton<ContextManager>().Push(context);
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
            if (user == null) return;
            UserName.text = user.user_nick_name;
            UserId.text = user.account_id.ToString();
            Diamond.text = user.user_diamond.ToString();
            Globals.GetHeadImgByUrl(user.user_head_img, SetIcon);
        }

        void SetIcon(Texture tex)
        {
            if(null == tex)
            {
                return;
            }

            Icon.sprite = Sprite.Create(tex as Texture2D, new Rect(0 ,0, tex.width, tex.height), Vector2.one * 0.5f);
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