using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Osblow.Util;


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

        public GameObject MorePanel;

        public Text NoticeTxt;
        public Transform RoomListRoot;
        #endregion

        #region 场景事件
        public void CreateRoomBtn()
        {
            ShowCreateView();

            Globals.SceneSingleton<SoundMng>().PlayCommonButtonSound();
        }

        public void EnterRoomBtn()
        {
            Globals.SceneSingleton<ContextManager>().Push(new EnterRoomUIContext());
            Globals.SceneSingleton<SoundMng>().PlayCommonButtonSound();
        }

        public void InviteCodeBtn()
        {
            //ShowInviteCodeDialog();
            Globals.SceneSingleton<ContextManager>().Push(new InviteCodeUIContext());
            HttpRequest.InviteCodeViewRequest();
            
            Globals.SceneSingleton<SoundMng>().PlayCommonButtonSound();
        }

        public void HistoryBtn()
        {
            Globals.SceneSingleton<ContextManager>().Push(new HistoryUIContext());
            HttpRequest.GameRecordRequest();

            Globals.SceneSingleton<SoundMng>().PlayCommonButtonSound();
        }

        public void MessageBtn()
        {
            //Globals.SceneSingleton<ContextManager>().Push(new MessageUIContext());
            //AlertUIContext context = new AlertUIContext();
            //context.Info = "服务器正忙，请稍后再试..";
            //Globals.SceneSingleton<ContextManager>().Push(context);

            Globals.SceneSingleton<ContextManager>().Push(new MessageUIContext());
            HttpRequest.NewsRequest();
            Globals.SceneSingleton<ContextManager>().WebBlockUI(true);

            Globals.SceneSingleton<SoundMng>().PlayCommonButtonSound();
        }

        public void ShareBtn()
        {
            //Globals.SceneSingleton<ContextManager>().Push(new ShareUIContext());
            AlertUIContext context = new AlertUIContext();
            context.Info = "服务器正忙，请稍后再试..";
            Globals.SceneSingleton<ContextManager>().Push(context);

            Globals.SceneSingleton<SoundMng>().PlayCommonButtonSound();
        }

        public void MoreBtn()
        {
            MorePanel.SetActive(!MorePanel.activeInHierarchy);
            Globals.SceneSingleton<SoundMng>().PlayCommonButtonSound();
        }

        public void RuleBtn()
        {
            Globals.SceneSingleton<ContextManager>().Push(new RuleUIContext());
            HttpRequest.RuleRequest();
            MorePanel.SetActive(false);
            Globals.SceneSingleton<SoundMng>().PlayCommonButtonSound();
        }

        public void ContactUsBtn()
        {
            Globals.SceneSingleton<ContextManager>().Push(new ContactUsUIContext());
            HttpRequest.ContactUsRequest();
            MorePanel.SetActive(false);
            Globals.SceneSingleton<SoundMng>().PlayCommonButtonSound();
        }

        public void HelpBtn()
        {
            MorePanel.SetActive(false);
            Globals.SceneSingleton<SoundMng>().PlayCommonButtonSound();
        }

        public void SettingBtn()
        {
            Globals.SceneSingleton<ContextManager>().Push(new SettingUIContext());
            

            Globals.SceneSingleton<SoundMng>().PlayCommonButtonSound();
            MorePanel.SetActive(false);
        }

        public void ExitBtn()
        {
            AlertUIContext context = new AlertUIContext();
            context.HasCancel = true;
            context.HasOK = true;
            context.OKCallback += delegate () 
            {
                Application.Quit();
            };
            context.Info = "确定要退出吗？";
            Globals.SceneSingleton<ContextManager>().Push(context);
            MorePanel.SetActive(false);
            Globals.SceneSingleton<SoundMng>().PlayCommonButtonSound();
        }

        public void PayBtn()
        {
            Globals.SceneSingleton<ContextManager>().Push(new PayUIContext());
            
            Globals.SceneSingleton<SoundMng>().PlayCommonButtonSound();
        }

        public void MyRoomsBtn()
        {
            AlertUIContext context = new AlertUIContext();
            context.Info = "服务器正忙，请稍后再试..";
            Globals.SceneSingleton<ContextManager>().Push(context);

            Globals.SceneSingleton<SoundMng>().PlayCommonButtonSound();
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

        void OnClickInvite(uint roomNum)
        {
            HttpRequest.ExistRoomRequest(roomNum.ToString());
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

            // 请求房间列表
            HttpRequest.RoomListRequest();
            NoticeTxt.text = user.notice_message;
            MsgMng.AddListener(MsgType.RoomList, RoomListListener);

            // 背景音乐
            Globals.SceneSingleton<SoundMng>().PlayBackSound("Audio/lobbyBg", true);
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
            Clear();
        }

        public override void OnPause(BaseContext context)
        {
            base.OnPause(context);
            Clear();
        }

        public override void OnResume(BaseContext context)
        {
            base.OnResume(context);
            HttpRequest.RoomListRequest();
        }





        private const string c_roomItemPath = "Prefab/UI/RoomList/item";
        private List<uint> m_savedRoomNumbers = new List<uint>();
        void RoomListListener(Msg msg)
        {
            List<LobbyRoomData> data = msg.Get<List<LobbyRoomData>>(0);

            for(int i = 0; i < data.Count; i++)
            {
                if (m_savedRoomNumbers.Contains(data[i].RoomNumber))
                {
                    continue;
                }
                m_savedRoomNumbers.Add(data[i].RoomNumber);


                GameObject newItem = Instantiate(Resources.Load(c_roomItemPath) as GameObject);
                newItem.transform.SetParent(RoomListRoot, false);

                newItem.transform.Find("roomNum").GetComponent<Text>().text = data[i].RoomNumber.ToString();
                newItem.transform.Find("round").GetComponent<Text>().text = data[i].TotalRound.ToString();
                newItem.transform.Find("userCount").GetComponent<Text>().text = data[i].UserCount;

                int curIndex = i;
                newItem.transform.Find("Button").GetComponent<Button>()
                    .onClick.AddListener(() => OnClickInvite(data[curIndex].RoomNumber));
            }
        }

        void Clear()
        {
            for (int i = RoomListRoot.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(RoomListRoot.GetChild(i).gameObject);
            }
            m_savedRoomNumbers.Clear();
        }
    }
}