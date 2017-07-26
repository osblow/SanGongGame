using UnityEngine;
using UnityEngine.UI;

namespace Osblow.App
{
    public class LoginUIContext : BaseContext
    {
        public LoginUIContext() : base(UIType.LoginView) { }
    }

    public class LoginView : BaseView
    {
        #region 场景引用 
        public InputField UserInput;
        #endregion

        #region 场景事件

        public void OnLoginBtn()
        {
            PlayerPrefs.SetString("username", UserInput.text);
            Login();

            Globals.SceneSingleton<SoundMng>().PlayCommonButtonSound();
        }
        #endregion
        void Login()
        {
            //Globals.SceneSingleton<ContextManager>().Push(new LobbyUIContext());
            Globals.SceneSingleton<ContextManager>().WebBlockUI(true);
            HttpRequest.LoginRequest();
        }


        public override void OnEnter(BaseContext context)
        {
            base.OnEnter(context);

            if (PlayerPrefs.HasKey("username"))
            {
                UserInput.text = PlayerPrefs.GetString("username");
            }
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
