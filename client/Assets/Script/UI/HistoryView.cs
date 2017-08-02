using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Osblow.Util;


namespace Osblow.App
{
    public class HistoryUIContext : BaseContext
    {
        public HistoryUIContext() : base(UIType.HistoryView) { }

        public GameRecords Records;
    }

    public class HistoryView : BaseView
    {
        #region 场景引用 
        public Transform GridRoot;
        //public GameObject ItemTpl;
        #endregion

        #region 场景事件
        public void OnExitBtn()
        {
            Globals.SceneSingleton<ContextManager>().Pop();
            Globals.SceneSingleton<SoundMng>().PlayCommonButtonSound();
        }
        #endregion
        private void OnRecieveRecords(Msg msg)
        {
            GameRecords records = msg.Get<GameRecords>(0);

            for (int i = 0; i < records.Records.Count; i++)
            {
                GameObject newItem = Instantiate(Resources.Load("Prefab/UI/HistoryView/RoundItem") as GameObject);
                newItem.transform.SetParent(GridRoot, false);
                RoundPanel singlePanel = new RoundPanel(newItem, records.Records[i]);
            }
        }


        public override void OnEnter(BaseContext context)
        {
            base.OnEnter(context);
            MsgMng.AddListener(MsgType.OnRecieveHistory, OnRecieveRecords);
        }

        public override void OnExit(BaseContext context)
        {
            base.OnExit(context);
            MsgMng.RemoveListener(MsgType.OnRecieveHistory, OnRecieveRecords);
            Clear();
        }

        public override void OnPause(BaseContext context)
        {
            base.OnPause(context);
        }

        public override void OnResume(BaseContext context)
        {
            base.OnResume(context);
        }






        void Clear()
        {
            for (int i = GridRoot.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(GridRoot.GetChild(i).gameObject);
            }
        }




        class RoundPanel
        {
            public GameObject RootObj;

            private Text m_roomNumerTxt;
            private Text m_totalRoundTxt;
            private Text m_ruleTxt;
            private Text m_timeTxt;
            private Button m_detailBtn;
            private Button m_shareBtn;

            private Transform m_gridRoot;

            private SingleGameRecord m_record;


            public RoundPanel(GameObject obj, SingleGameRecord record)
            {
                RootObj = obj;
                m_record = record;
                Init();
            }

            void Init()
            {
                m_roomNumerTxt = RootObj.transform.Find("Head/RoomNum").GetComponent<Text>();
                m_totalRoundTxt = RootObj.transform.Find("Head/Round").GetComponent<Text>();
                m_ruleTxt = RootObj.transform.Find("Head/Rule").GetComponent<Text>();
                m_timeTxt = RootObj.transform.Find("Head/Time").GetComponent<Text>();

                m_roomNumerTxt.text = m_record.RoomNumber.ToString();
                m_totalRoundTxt.text = m_record.TotalRound.ToString();
                m_ruleTxt.text = m_record.RoomRuleName;
                m_timeTxt.text = m_record.GameTime;




                m_detailBtn = RootObj.transform.Find("Head/Detail").GetComponent<Button>();
                m_shareBtn = RootObj.transform.Find("Head/Share").GetComponent<Button>();

                m_detailBtn.onClick.AddListener(OnClickDetail);
                m_shareBtn.onClick.AddListener(OnClickShare);


                m_gridRoot = RootObj.transform.Find("Scroll View/Grid");
                InitItems();
            }

            void InitItems()
            {
                for (int i = 0; i < m_record.Players.Count; i++)
                {
                    GameObject newItem = GameObject.Instantiate(Resources.Load("Prefab/UI/HistoryView/SinglePlayer") as GameObject);
                    newItem.transform.SetParent(m_gridRoot, false);

                    Player p = new Player(newItem, m_record.Players[i]);
                }
            }


            void OnClickDetail()
            {
                Globals.SceneSingleton<ContextManager>().WebBlockUI(true);
                Globals.SceneSingleton<ContextManager>().Push(new HistoryDetailUIContext());
                HttpRequest.SmallGameRecordRequest(m_record.RoomId.ToString());
            }

            void OnClickShare()
            {

            }
        }

        class Player
        {
            public GameObject RootObj;

            private RawImage m_icon;
            private Text m_name;
            private Text m_id;
            private GameObject m_winner;
            private GameObject m_loser;
            private Text m_score;
            private GameObject m_owner;

            private RecordPlayer m_record;


            public Player(GameObject obj, RecordPlayer record)
            {
                RootObj = obj;
                m_record = record;
                Init();
            }

            void Init()
            {
                m_icon = RootObj.transform.Find("icon").GetComponent<RawImage>();
                m_name = RootObj.transform.Find("name").GetComponent<Text>();
                m_id = RootObj.transform.Find("id").GetComponent<Text>();
                m_score = RootObj.transform.Find("totalScore").GetComponent<Text>();
                m_owner = RootObj.transform.Find("owner").gameObject;

                m_name.text = m_record.NickName;
                m_id.text = m_record.AccountId.ToString();
                m_score.text = m_record.Score.ToString();
                Globals.GetHeadImgByUrl(m_record.Icon, SetIcon);
                m_owner.SetActive(m_record.IsOwner);
            }

            void SetIcon(Texture tex)
            {
                m_icon.texture = tex;
            }
        }
    }
}
