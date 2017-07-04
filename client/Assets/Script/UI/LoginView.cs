
namespace Osblow.App
{
    public class LoginUIContext : BaseContext
    {
        public LoginUIContext() : base(UIType.LoginView) { }
    }

    public class LoginView : BaseView
    {
        #region 场景引用 

        #endregion

        #region 场景事件

        public void OnLoginBtn()
        {
            Login();
        }
        #endregion
        void Login()
        {
            //Globals.SceneSingleton<ContextManager>().Push(new LobbyUIContext());
            HttpRequest.LoginRequest();
        }


        public override void OnEnter(BaseContext context)
        {
            base.OnEnter(context);
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
