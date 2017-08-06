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

        public static void DoLogin()
        {
#if UNITY_EDITOR
            HttpRequest.LoginRequest();
#elif UNITY_ANDROID
            if (PlayerPrefs.HasKey("username") && PlayerPrefs.GetString("username") != null
                && PlayerPrefs.GetString("username").Length > 0)
            {
                Debug.Log("已有用户id，直接登录" + PlayerPrefs.GetString("username"));
                HttpRequest.LoginRequest();
            }
            else
            {
                AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");//获取到当前的activity
                                                                                          //jo.Call("RegistWXApi");   //第一个是方法名  第二个是参数
                jo.Call("weiLogin");
            }
#endif
        }


        void Login()
        {
            //Globals.SceneSingleton<ContextManager>().Push(new LobbyUIContext());
            Globals.SceneSingleton<ContextManager>().WebBlockUI(true, "正在登录...");

            DoLogin();
        }


        public override void OnEnter(BaseContext context)
        {
            base.OnEnter(context);

            if (PlayerPrefs.HasKey("username"))
            {
                UserInput.text = PlayerPrefs.GetString("username");
            }

            WeixinHandler.RequestRoomNumber();
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
