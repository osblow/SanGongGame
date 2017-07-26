using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Osblow.App
{
    public class UserInfoUIContext : BaseContext
    {
        public UserInfoUIContext() : base(UIType.UserInfoDialog) { }

        public TablePlayerData ThePlayerData; 
    }

    public class UserInfoDialog: BaseView
    {
        #region 场景引用 
        public Image Icon;
        public Text Name;
        public Text IP;
        public GameObject[] Stars;
        #endregion

        #region 场景事件
        public void OnExitBtn()
        {
            Globals.SceneSingleton<ContextManager>().Pop();
            Globals.SceneSingleton<SoundMng>().PlayCommonButtonSound();
        }
        #endregion



        public override void OnEnter(BaseContext context)
        {
            base.OnEnter(context);

            UserInfoUIContext theContent = context as UserInfoUIContext;
            Name.text = theContent.ThePlayerData.NickName;
            IP.text = theContent.ThePlayerData.UserIp;
            for(int i = 0; i < Stars.Length; i++)
            {
                Stars[i].SetActive(i < theContent.ThePlayerData.EvaluateScore.Length);
            }
			Globals.GetHeadImgByUrl (theContent.ThePlayerData.HeadImg, SetIcon);
        }

		void SetIcon(Texture tex){
			Icon.sprite = Sprite.Create (tex as Texture2D, new Rect (0, 0, tex.width, tex.height), Vector2.one * 0.5f);	
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

