using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Osblow.Game;
using Osblow.Util;

namespace Osblow.App
{
    public class TableUIContext : BaseContext
    {
        public TableUIContext() : base(UIType.TableView) { }
    }

    public class TableView : BaseView
    {
        #region 场景引用 
        public List<GameObject> UserPanels;

        public List<GameObject> CuoPais;

        [Space]
        public Text RoomId;
        public Text Round;
        public Text Mode;
        public Text Owner;
        public Text BaseScore;

        [Space]
        public GameObject MenuPanelObj;

        [Space]
        public GameObject StartGameBtn;
        public GameObject SitDownBtn;
        public GameObject BankerBtn;
        public GameObject CancelBankerBtn;
        public GameObject BetPanel;
        public GameObject CuopaiBtn;
        public GameObject ShowCardBtn;
        public GameObject ReadyBtn;

        [Space]
        public GameObject EmojsPanel;

        [Space]
        public GameObject CuoPaiController;

        [Space]
        public GameObject BetItemGrid;
        public GameObject BetItemTpl;

        [Space]
        public Text Clock;
        public GameObject ClockBG;

        [Space]
        public GameObject Siding; // 旁观
        public GameObject Waiting; // 等待下一局

        public GameObject VoicePanelObj;
        public Button TextMessageBtn;
        public Button VoiceMessageBtn;
        #endregion

        #region 场景事件
        public void OnMenuBtn()
        {
            ShowMenu();
            Globals.SceneSingleton<SoundMng>().PlayCommonButtonSound();
        }

        public void OnExitBtn()
        {
            CmdRequest.ExitRoomRequest();
            MenuPanelObj.SetActive(false);
            //Globals.SceneSingleton<ContextManager>().Push(new LobbyUIContext());
            Globals.SceneSingleton<SoundMng>().PlayCommonButtonSound();
        }

        public void OnDismissBtn()
        {
            CmdRequest.DismissRoomRequest();
            MenuPanelObj.SetActive(false);
            Globals.SceneSingleton<SoundMng>().PlayCommonButtonSound();
        }

        public void OnShareBtn()
        {
            MenuPanelObj.SetActive(false);
            Globals.SceneSingleton<SoundMng>().PlayCommonButtonSound();
        }


        public void StartGame()
        {
            StartGameBtn.SetActive(false);
            //
            CmdRequest.StartGameRequest();
            Globals.SceneSingleton<SoundMng>().PlayCommonButtonSound();
        }


        public void OnCuoPai()
        {
            CuoPai();
            Globals.SceneSingleton<SoundMng>().PlayCommonButtonSound();
        }

        public void OnShowCards()
        {
			CmdRequest.SynchroniseCardsRequest ();
            //ShowCards();

            Globals.SceneSingleton<SoundMng>().PlayCommonButtonSound();
        }

        private bool m_firstSit = true;
        public void OnSitDownBtn()
        {
            SitDownBtn.SetActive(false);
            ReadyBtn.SetActive(false);

            TableData tableData = Globals.SceneSingleton<DataMng>().GetData<TableData>(DataType.Table);
            if (m_isOwner && tableData.CurRound == 0)
            {
                StartGameBtn.SetActive(true);
            }

            for (int i = 0; i < m_usersPanelList.Count; i++)
            {
                m_usersPanelList[i].ShowCards(new uint[] { 54, 54, 54 });
                m_usersPanelList[i].ShowCards(false);
            }

            // 添加判断，如果是游戏当中加入的，则显示等待下一局消息
            if (tableData.SeatNum == 0 && tableData.IsGaming && m_firstSit)
            {
                Waiting.SetActive(true);
            }

            if (!m_firstSit)
            {
                Round.text = tableData.CurRound + "/" + tableData.TotalRound;
                m_firstSit = false;
            }

            

            //ChaseOwnerBtn.SetActive(true);
            //StartCoroutine(TestCreateUsers());
            CmdRequest.ReadyRequest();

            Globals.SceneSingleton<SoundMng>().PlayFrontSound("Audio/pdk/comm_ready");
        }

        public void OnBankeringBtn()
        {
            Debug.Log("3..");
            BankerBtn.SetActive(false);
            CancelBankerBtn.SetActive(false);
            CmdRequest.ClientBankerRequest (true);
            //BetPanel.SetActive(true);

            Globals.SceneSingleton<SoundMng>().PlayCommonButtonSound();
        }

        public void OnCancelBankeringBtn()
        {
            Debug.Log("6..");
            BankerBtn.SetActive(false);
            CancelBankerBtn.SetActive(false);
            CmdRequest.ClientBankerRequest(false);
            //BetPanel.SetActive(true);

            Globals.SceneSingleton<SoundMng>().PlayCommonButtonSound();
        }

        private int m_betNum = 0; // 已下注的次数，第二次下注发送不同请求
        public void OnBetBtn(Text label)
        {
            int betScore = int.Parse(label.text);
            BetPanel.SetActive(false);

            if (m_betNum == 0)
            {
                CmdRequest.ClientBetRequest((uint)betScore);
            }
            else
            {
                CmdRequest.ClientBetAgainRequest((uint)betScore);
            }
            // test
            //m_cardsPanel.ForEach((x) => { x.SetActive(true); });
            //Deal();

            ++m_betNum;

            Globals.SceneSingleton<SoundMng>().PlayCommonButtonSound();
        }

        public void OnShowUserInfo(int index)
        {
            TablePlayerData theData = m_usersPanelList[index].Data;
            UserInfoUIContext context = new UserInfoUIContext();
            context.ThePlayerData = theData;
            Globals.SceneSingleton<ContextManager>().Push(context);

            Globals.SceneSingleton<SoundMng>().PlayCommonButtonSound();
        }

        public void OnClickEmoj(int index)
        {
            HideEmojPanel();

            CmdRequest.ClientExpressionRequest((uint)index);

            Globals.SceneSingleton<SoundMng>().PlayCommonButtonSound();
        }

        public void OnTextMessage()
        {
            ShowEmojPanel();

            Globals.SceneSingleton<SoundMng>().PlayCommonButtonSound();
        }



        public void OnBeginVoice()
        {
            m_isRecordingVoice = true;

            m_voicePanel.Show(true);
            Globals.SceneSingleton<MicrophoneMng>().StartRecord(OnEndRecordingVoice);
            Globals.SceneSingleton<SoundMng>().PauseBgSound();
        }

        public void OnEndVoice()
        {
            m_isRecordingVoice = false;
            m_voicePanel.Show(false);

            Globals.SceneSingleton<MicrophoneMng>().StopRecord(OnEndRecordingVoice);
            Globals.SceneSingleton<SoundMng>().UnPauseBgSound();
        }

        public void OnFingerOutOfVoice()
        {
            if (m_isRecordingVoice)
            {
                m_isRecordingVoice = false;
                m_voicePanel.Show(false);
                Globals.SceneSingleton<SoundMng>().UnPauseBgSound();
            }
        }
        #endregion

        private List<UserPanel> m_usersPanelList = new List<UserPanel>();
        private Dictionary<string, UserPanel> m_usersPanelDic = new Dictionary<string, UserPanel>();
        private int m_curPlayerCount = 0;
        private bool m_isOwner = false;
        private bool m_isEmojHidden = true;
        private bool m_isInitialized = false;

        private VoicePanel m_voicePanel;
        private bool m_isRecordingVoice = false;

        void ShowMenu()
        {
            //Globals.SceneSingleton<ContextManager>().Push(new LobbyUIContext());
            MenuPanelObj.SetActive(!MenuPanelObj.activeInHierarchy);
        }

        void CuoPai()
        {
			// set cards values
			UserData playerData = Globals.SceneSingleton<DataMng>().GetData<UserData>(DataType.Player);

			for (int i = 0; i < CuoPaiController.transform.childCount; i++) {
			
				SingleCard card = CuoPaiController.transform.GetChild (i).GetComponent<SingleCard>();
				card.SetNum (playerData.cards [i]);
			}

            CuoPaiController.SetActive(true);
        }

		void ShowCards(uint[] cardsVals)
        {
            // test
            //int randomSuit = 0;
            //int randomNumber = 0;
            //int value = 0;
            //for(int i = 0; i < m_cards.Count; i++)
            //{
            //    for (int j = 0; j < m_cards[i].Count; j++)
            //    {
            //        value = Random.Range(0, 52);
            //        m_cards[i][j].SetNum(value);
            //    }
            //}

            //// test
            //Invoke("ShowGameResult", 1.0f);

			m_usersPanelList [0].ShowCards (cardsVals);
        }

        void ShowGameResult()
        {
            Globals.SceneSingleton<ContextManager>().Push(new GameResultUIContext());
        }

        /// <summary>
        /// 发牌
        /// </summary>
        public void Deal()
        {
            RoomData roomData = Globals.SceneSingleton<DataMng>().GetData<RoomData>(DataType.Room);
            int mode = roomData.RoomRuleType == 1 ? 2 : 1;
            m_usersPanelList.ForEach((x) => { x.DealAnimation(mode); });
            //UserPanels.ForEach((x) => { DealAnimation(x); });
            //DealAnimation(UserPanels[0]);
        }



        void DealAnimation(GameObject userPanel)
        {
            GameObject card1 = userPanel.transform.Find("MyCardsGrid/card").gameObject;
            GameObject card2 = userPanel.transform.Find("MyCardsGrid/card (1)").gameObject;
            GameObject card3 = userPanel.transform.Find("MyCardsGrid/card (2)").gameObject;

            Vector3 originPos = new Vector3(Screen.width / 2, Screen.height/ 2, 0);
            Vector3 targetPos1 = card1.transform.position;
            Vector3 targetPos2 = card2.transform.position;
            Vector3 targetPos3 = card3.transform.position;

            card1.transform.position = originPos;
            card2.transform.position = originPos;
            card3.transform.position = originPos;
            card1.transform.DOMove(targetPos1, 0.7f);
            card2.transform.DOMove(targetPos2, 0.7f).SetDelay(0.2f);
            card3.transform.DOMove(targetPos3, 0.7f).SetDelay(0.4f);
        }

        IEnumerator TestCreateUsers()
        {
            for(int i = 1; i < 6; i++)
            {
                yield return new WaitForSeconds(0.5f);
                UserPanels[i].SetActive(true);
            }

            BankerBtn.SetActive(true);
        }

        private Vector3 m_emojHiddenPos
        {
            get
            {
                return new Vector3(Screen.width + 218, 376, 0);
            }
        }
        private Vector3 m_emojShowPos
        {
            get
            {
                return new Vector3(Screen.width - 218, 376, 0);
            }
        }
        void ShowEmojPanel()
        {
            if (!m_isEmojHidden)
            {
                return;
            }

            EmojsPanel.transform.position = m_emojHiddenPos;
            EmojsPanel.transform.DOMove(m_emojShowPos, 0.7f);

            m_isEmojHidden = false;
        }

        void HideEmojPanel()
        {
            if (m_isEmojHidden)
            {
                return;
            }

            EmojsPanel.transform.position = m_emojShowPos;
            EmojsPanel.transform.DOMove(m_emojHiddenPos, 0.7f);

            m_isEmojHidden = true;
        }





        //private List<GameObject> m_cardsPanel = new List<GameObject>();
        //private List<List<SingleCard>> m_cards = new List<List<SingleCard>>();

        public override void OnEnter(BaseContext context)
        {
            base.OnEnter(context);
            if (m_isInitialized)
            {
                return;
            }
            m_isInitialized = true;
            Globals.SceneSingleton<GameMng>().GameStart = true;

            // sound
            Globals.SceneSingleton<SoundMng>().PlayBackSound("Audio/pdk_bg_sound_001", true);

            TableData tableData = Globals.SceneSingleton<DataMng>().GetData<TableData>(DataType.Table);
            RoomData roomData = Globals.SceneSingleton<DataMng>().GetData<RoomData>(DataType.Room);
            if (null != tableData)
            {
                Mode.text = tableData.SanGong == 0 ? "三公" : "双公";
                Owner.text = roomData.IsJoin == 0 ? "是" : "否";
                BaseScore.text = roomData.MaxPlayerCount.ToString();
            }
            RoomId.text = roomData.RoomId.ToString();
            Round.text = tableData.CurRound + "/" + tableData.TotalRound;
            // cards panel
            for(int i = 0; i < UserPanels.Count; i++)
            {
                UserPanel thePanel = new UserPanel(UserPanels[i]);
                m_usersPanelList.Add(thePanel);
                
                if(i == 0)
                {
                    UserData mainplayerData = Globals.SceneSingleton<DataMng>().GetData<UserData>(DataType.Player);
                    thePanel.Reset(mainplayerData);
					m_usersPanelDic.Add (mainplayerData.uuid, thePanel);
                }
                if (i != 0) thePanel.Show(false);

                //GameObject theCardsPanel = UserPanels[i].transform.Find("MyCardsGrid").gameObject;
                //m_cardsPanel.Add(theCardsPanel);
                //theCardsPanel.SetActive(false);

                //List<SingleCard> userCards = new List<SingleCard>();
                //GameObject card1 = UserPanels[i].transform.Find("MyCardsGrid/card").gameObject;
                //GameObject card2 = UserPanels[i].transform.Find("MyCardsGrid/card (1)").gameObject;
                //GameObject card3 = UserPanels[i].transform.Find("MyCardsGrid/card (2)").gameObject;
                //userCards.Add(card1.AddComponent<SingleCard>());
                //userCards.Add(card2.AddComponent<SingleCard>());
                //userCards.Add(card3.AddComponent<SingleCard>());
                //m_cards.Add(userCards);

                //if(i > 0)
                //{
                //    UserPanels[i].SetActive(false);
                //}
            }

            

            UserData playerData = Globals.SceneSingleton<DataMng>().GetData<UserData>(DataType.Player);
            if (tableData.OwnerUUId == playerData.uuid)
            {
                m_isOwner = true;
                StartGameBtn.SetActive(true);
            }
            SitDownBtn.SetActive(true);
            

            int mySeat = -1;
            for (int i = 0; i < tableData.Players.Count; i++)
            {
                TablePlayerData theData = tableData.Players[i];
                if (theData.PlayerUUID == playerData.uuid)
                {
                    mySeat = i;
                    break;
                }
            }
            for (int i = 0; i < tableData.Players.Count; i++)
            {
                int j = i - mySeat;
                j = j < 0 ? j + tableData.Players.Count : j;
                TablePlayerData theData = tableData.Players[i];
                ++m_curPlayerCount;
                m_usersPanelList[j].Show(true);
                m_usersPanelList[j].Reset(tableData.Players[i]);
				m_usersPanelDic.Add (tableData.Players [i].PlayerUUID, m_usersPanelList [j]);
            }

            CuoPaiController.SetActive(false);

            m_voicePanel = new VoicePanel(VoicePanelObj);

            // bet points panel
            //string betPrefabPath = "UI/TableView/Bet";
            GameObject betPrefab = BetItemTpl;
            for(int i = 0; i < tableData.BetPoints.Count; i++)
            {
                GameObject newBet = Instantiate(betPrefab);
                newBet.GetComponentInChildren<Text>().text = 
                    tableData.BetPoints[i].BetPoint.ToString();
                newBet.SetActive(true);

                newBet.transform.SetParent(BetItemGrid.transform, false);
            }

            
            //MsgMng.AddListener(MsgType.OtherPlayerEnter, OnOtherPlayerEnter);
            MsgMng.AddListener(MsgType.UI_AddPlayer, OnOtherPlayerEnter);
            MsgMng.AddListener(MsgType.UI_PlayerReady, OnPlayerReady);
            //MsgMng.AddListener(MsgType.UI_ShowCards, OnShowCards);
            MsgMng.AddListener(MsgType.UI_StartBet, OnStartBet);
            MsgMng.AddListener(MsgType.UI_PlayerBet, OnPlayerBet);
            MsgMng.AddListener(MsgType.UI_Expression, OnExpression);
            MsgMng.AddListener(MsgType.DealCards, OnDealCards);
			MsgMng.AddListener (MsgType.StartBanker, OnStartBanker);
			MsgMng.AddListener (MsgType.ShowTwoCardsAndStartBanker, OnShowTwoCardsAndBanker);
			MsgMng.AddListener (MsgType.UI_ConfirmOwner, OnConfirmBanker);
			MsgMng.AddListener (MsgType.UI_Bankering, OnBankering);
			MsgMng.AddListener (MsgType.SynchroniseCards, EndARound);
            MsgMng.AddListener(MsgType.Compare, CompareCards);
            MsgMng.AddListener(MsgType.GameEnd, GameEnd);
            MsgMng.AddListener(MsgType.UpdateClock, OnUpdateClock);
            MsgMng.AddListener(MsgType.StartGameBtnEnabled, OnStartBtnEnabled);
            MsgMng.AddListener(MsgType.BankeringStatus, OnUpdateBankerState);
            MsgMng.AddListener(MsgType.OnlineStatus, OnlineStatusChange);
            MsgMng.AddListener(MsgType.ReConnected, Reconnected);
            MsgMng.AddListener(MsgType.ShowVoiceImg, ShowVoiceImg);
            MsgMng.AddListener(MsgType.ShowMessageBtn, ShowMessageBtn);

            // 如果是游戏当中加入的，显示坐下按钮
            if (tableData.SeatNum == 0 && tableData.IsGaming)
            {
                SitDownBtn.SetActive(true);
            }

            // 如果是旁观者，将所有操作按钮移到屏幕外
            if (!tableData.IsReady)
            {
                SitDownBtn.transform.position = Vector3.one * 10000;
                ReadyBtn.transform.position = Vector3.one * 10000;
                StartGameBtn.transform.position = Vector3.one * 10000;
                BetPanel.transform.position = Vector3.one * 10000;
                Debug.Log("1..");
                BankerBtn.transform.position = Vector3.one * 10000;
                CancelBankerBtn.transform.position = Vector3.one * 10000;
                CuopaiBtn.transform.position = Vector3.one * 10000;
                ShowCardBtn.transform.position = Vector3.one * 10000;

                transform.Find("TextMessage").position = Vector3.one * 10000;
                transform.Find("VoiceMessage").position = Vector3.one * 10000;

                Siding.SetActive(true);
            }
        }

        public override void OnExit(BaseContext context)
        {
            base.OnExit(context);
            Globals.SceneSingleton<UIManager>().DestroySingleUI(UIType.TableView);

            //MsgMng.RemoveListener(MsgType.OtherPlayerEnter, OnOtherPlayerEnter);
            MsgMng.RemoveListener(MsgType.UI_AddPlayer, OnOtherPlayerEnter);
            MsgMng.RemoveListener(MsgType.UI_PlayerReady, OnPlayerReady);
            //MsgMng.RemoveListener(MsgType.UI_ShowCards, OnShowCards);
            MsgMng.RemoveListener(MsgType.UI_StartBet, OnStartBet);
            MsgMng.RemoveListener(MsgType.UI_PlayerBet, OnPlayerBet);
            MsgMng.RemoveListener(MsgType.UI_Expression, OnExpression);
            MsgMng.RemoveListener(MsgType.DealCards, OnDealCards);
            MsgMng.RemoveListener(MsgType.StartBanker, OnStartBanker);
            MsgMng.RemoveListener(MsgType.ShowTwoCardsAndStartBanker, OnShowTwoCardsAndBanker);
            MsgMng.RemoveListener(MsgType.UI_ConfirmOwner, OnConfirmBanker);
            MsgMng.RemoveListener(MsgType.UI_Bankering, OnBankering);
            MsgMng.RemoveListener(MsgType.SynchroniseCards, EndARound);
            MsgMng.RemoveListener(MsgType.Compare, CompareCards);
            MsgMng.RemoveListener(MsgType.GameEnd, GameEnd);
            MsgMng.RemoveListener(MsgType.UpdateClock, OnUpdateClock);
            MsgMng.RemoveListener(MsgType.StartGameBtnEnabled, OnStartBtnEnabled);
            MsgMng.RemoveListener(MsgType.BankeringStatus, OnUpdateBankerState);
            MsgMng.RemoveListener(MsgType.OnlineStatus, OnlineStatusChange);
            MsgMng.RemoveListener(MsgType.ReConnected, Reconnected);
            MsgMng.RemoveListener(MsgType.ShowVoiceImg, ShowVoiceImg);
            MsgMng.RemoveListener(MsgType.ShowMessageBtn, ShowMessageBtn);
        }

        public override void OnPause(BaseContext context)
        {
            base.OnPause(context);
        }

        public override void OnResume(BaseContext context)
        {
            base.OnResume(context);
        }



        void OnOtherPlayerEnter(Msg msg)
        {
            string theUUID = msg.Get<string>(0);
            TablePlayerData theData = Globals.SceneSingleton<GameMng>().UsersDic[theUUID];

            m_curPlayerCount += 1;
            if(m_curPlayerCount > 5)
            {
                Debug.LogError("人数超出上限");
                return;
            }

            UserPanel theUserPanel = m_usersPanelList[m_curPlayerCount];
            Globals.SceneSingleton<AsyncInvokeMng>().Events.Add(delegate ()
            {
                theUserPanel.Show(true);
                theUserPanel.Reset(theData);
                m_usersPanelDic.Add(theUUID, theUserPanel);
            });
        }

        void OnPlayerReady(Msg msg)
        {
            Globals.SceneSingleton<AsyncInvokeMng>().Events.Add(delegate ()
            {
                string theUUID = msg.Get<string>(0);

                if (theUUID == Globals.SceneSingleton<DataMng>().GetData<UserData>(DataType.Player).uuid)
                {
                    //StartGameBtn.SetActive(true);
                    TextMessageBtn.interactable = true;
                    VoiceMessageBtn.interactable = true;
                }

                if (!m_usersPanelDic.ContainsKey(theUUID))
                {
                    return;
                }
                m_usersPanelDic[theUUID].SetReady(true);
            });
        }

        void OnShowCards(Msg msg)
        {
            string theUUID = msg.Get<string>(0);

            if (!m_usersPanelDic.ContainsKey(theUUID))
            {
                return;
            }

            uint[] cardsVal = msg.Get<uint[]>(1);
            m_usersPanelDic[theUUID].ShowCards(cardsVal);
        }
        
        void OnStartBet(Msg msg)
        {
            Globals.SceneSingleton<AsyncInvokeMng>().Events.Add(delegate ()
            {
                for(int i = 0; i < m_usersPanelList.Count; i++)
                {
                    m_usersPanelList[i].SetReady(false);
                }

                Debug.Log("开始下注。。。。。。。。。。。。。。。。");

                //CancelInvoke();
                ReadyBtn.SetActive(false);
                StartGameBtn.SetActive(false);
                BetPanel.SetActive(true);
                Debug.Log("1..");
                BankerBtn.SetActive(false);
                CancelBankerBtn.SetActive(false);
                CuopaiBtn.SetActive(false);
                ShowCardBtn.SetActive(false);
                Waiting.SetActive(false);

                m_usersPanelList.ForEach((x) => { x.ShowCards(false); });

                TableData tableData = Globals.SceneSingleton<DataMng>().GetData<TableData>(DataType.Table);
                //tableData.CurRound++;
                Round.text = tableData.CurRound.ToString() + "/" + tableData.TotalRound;

                if (msg.Get<int>(0) == 0)
                {
                    m_betNum = 0;
                }
            });
        }

        void OnPlayerBet(Msg msg)
        {
            string theUUID = msg.Get<string>(0);
            uint thePoint = msg.Get<uint>(1);

            Globals.SceneSingleton<AsyncInvokeMng>().Events.Add(delegate ()
            {
				//Debug.Log(m_usersPanelDic.Count);
                m_usersPanelDic[theUUID].SetBet((int)thePoint);

                
                if (!IsSelf(theUUID))
                {
                    return;
                }

                //BankerBtn.SetActive(false);
                //CuopaiBtn.SetActive(false);
                //ShowCardBtn.SetActive(false);
                //SitDownBtn.SetActive(false);
                //StartGameBtn.SetActive(false);

                //m_usersPanelList.ForEach((x) => { x.ShowCards(false); });
            });
        }

        void OnExpression(Msg msg)
        {
            string uuid = msg.Get<string>(0);
            uint expressionId = msg.Get<uint>(1);

            Globals.SceneSingleton<AsyncInvokeMng>().Events.Add(delegate ()
            {
                if (!m_usersPanelDic.ContainsKey(uuid))
                {
                    return;
                }
                m_usersPanelDic[uuid].ShowMessage((int)expressionId);
            });
        }

        void OnDealCards(Msg msg)
        {
            Globals.SceneSingleton<AsyncInvokeMng>().Events.Add(delegate ()
            {
                TableData tableData = Globals.SceneSingleton<DataMng>().GetData<TableData>(DataType.Table);
                int mode = tableData.ruleType == 1 ? 2 : 1;

                for (int i = 0; i < m_usersPanelList.Count; i++)
                {
                    if (m_usersPanelList[i].Data != null || i == 0)
                    {
                        if (mode == 1)
                        {
                            m_usersPanelList[i].ShowCards(new uint[] { 54, 54, 54 });
                        }
                        else
                        {
                            m_usersPanelList[i].ShowCards(new uint[] { 100, 100, 54 });
                        }
                        m_usersPanelList[i].DealAnimation(mode);
                    }
                }

                // 音效


                Debug.Log("开始发牌。。。。。。。。");

                CuopaiBtn.SetActive(true);
                ShowCardBtn.SetActive(true);
                Debug.Log("2..");
                BankerBtn.SetActive(false);
                CancelBankerBtn.SetActive(false);
                ReadyBtn.SetActive(false);
                StartGameBtn.SetActive(false);
                BetPanel.SetActive(false);

                //if (m_bankerEffect != null)
                //{
                //    StopCoroutine(m_bankerEffect);
                //}
            });
        }

        IEnumerator DealCardSound()
        {
            int loop = 20;
            while((loop--) < 0)
            {
                yield return new WaitForSeconds(0.1f);
                Globals.SceneSingleton<SoundMng>().PlayFrontSound("Audio/pdk/comm_deal_sound");
            }
        }



		void OnStartBanker(Msg msg)
		{
            Debug.Log("显示抢庄按钮");

			Globals.SceneSingleton<AsyncInvokeMng> ().Events.Add(delegate () {
				BankerBtn.SetActive (true);
                CancelBankerBtn.SetActive(true);
                BetPanel.SetActive(false);
			});
		}

        Coroutine m_bankerEffect = null;
        IEnumerator BankerEffect(string uuid)
        {
            List<UserPanel> bankeringUsers = new List<UserPanel>();
            if(m_usersPanelList[0].IsBankering) bankeringUsers.Add(m_usersPanelList[0]);
            for (int i = 1; i < m_usersPanelList.Count; i++)
            {
                if(m_usersPanelList[i].Data != null && m_usersPanelList[i].IsBankering)
                {
                    bankeringUsers.Add(m_usersPanelList[i]);
                }
            }

            int index = 0;
            while (bankeringUsers.Count > 1)
            {
                yield return new WaitForSeconds(0.3f);

                for (int i = 0; i < bankeringUsers.Count; i++)
                {
                    bankeringUsers[i].SetBankerEffect(i == (index % bankeringUsers.Count));
                }

                ++index;

                if(index / bankeringUsers.Count > 3)
                {
                    break;
                }
            }

            //Debug.LogError(".............................");
            m_usersPanelDic[uuid].SetBanker();
        }




		void OnShowTwoCardsAndBanker(Msg msg)
		{
			Globals.SceneSingleton<AsyncInvokeMng> ().Events.Add(delegate () {
				uint[] cards = msg.Get<uint[]>(0);
				ShowCards(cards);
				BankerBtn.SetActive (true);
                CancelBankerBtn.SetActive(true);
                BetPanel.SetActive(false);
			});
		}
        

		void OnBankering(Msg msg)
		{	
			Globals.SceneSingleton<AsyncInvokeMng> ().Events.Add(delegate () {
				BankerBtn.SetActive(true);
                CancelBankerBtn.SetActive(true);
                BetPanel.SetActive(false);
			});
		}

		void OnConfirmBanker(Msg msg)
		{
			Globals.SceneSingleton<AsyncInvokeMng> ().Events.Add(delegate () {
				string uuid = msg.Get<string> (0);

                if (!m_usersPanelDic.ContainsKey(uuid))
                {
                    return;
                }

                //if (m_bankerEffect != null)
                //{
                //    StopCoroutine(m_bankerEffect);
                //}

                if (m_bankerEffect != null)
                {
                    StopCoroutine(m_bankerEffect);
                }
                m_bankerEffect = StartCoroutine(BankerEffect(uuid));
            });
		}

		bool hasEnd = false;
		void EndARound(Msg msg)
		{
			//if (hasEnd)
			//	return;

			Globals.SceneSingleton<AsyncInvokeMng> ().Events.Add(delegate() {
                string theUUID = msg.Get<string>(2);
                if(!m_usersPanelDic.ContainsKey(theUUID))
                {
                    return;
                }


                List<uint> cards = msg.Get<List<uint>>(0);

                m_usersPanelDic[theUUID].ShowCards(cards.ToArray());
                m_usersPanelDic[theUUID].SetCardResult(msg.Get<string>(1));






                UserData playerData = Globals.SceneSingleton<DataMng>().GetData<UserData>(DataType.Player);


                if (playerData.uuid == theUUID)
                {
                    CuoPaiController.SetActive(false);
                    CuopaiBtn.SetActive(false);
                    ShowCardBtn.SetActive(false);
                }
				


                //UserData playerdata = Globals.SceneSingleton<DataMng>().GetData<UserData>(DataType.Player);
                //m_usersPanelList[0].ShowCards(playerdata.cards.ToArray());
                //m_usersPanelList[0].SetCardResult(playerdata.cardResult);

                //Invoke("NextRound", 1.0f);
            });

			//hasEnd = true;
				
		}

        void CompareCards(Msg msg)
        {
            List<ResultEffectData> eDatas = msg.Get<List<ResultEffectData>>(0);
            uint expireTime = msg.Get<uint>(1);
            Dictionary<string, CompareSingleUser> userResults = msg.Get<Dictionary<string, CompareSingleUser>>(2);

            Globals.SceneSingleton<AsyncInvokeMng>().Events.Add(delegate ()
            {
                Debug.Log("比大小中.....");

                NextRound();

                for (int i = 0; i < eDatas.Count; i++)
                {
                    string fromId = eDatas[i].FromUser;
                    string toId = eDatas[i].ToUser;
                    uint point = eDatas[i].Point;

                    if (!m_usersPanelDic.ContainsKey(toId) || !m_usersPanelDic.ContainsKey(fromId))
                    {
                        Debug.Log("比大小中用户不存在");
                        Globals.Instance.LogCallbackRequest("比大小中用户不存在"+ toId + " " + fromId);
                        continue;
                    }

                    m_usersPanelDic[toId].ShowUpdatedScore(m_usersPanelDic[fromId]);
                }

                foreach(KeyValuePair<string, CompareSingleUser> kval in userResults)
                {
                    m_usersPanelDic[kval.Key].ScoreTxt.text = kval.Value.Point.ToString();
                    m_usersPanelDic[kval.Key].SetBet(0);
                }
                //Invoke("NextRound", 1.0f);

                
            });
        }

		void NextRound()
		{
            //for(int i = 0; i < m_usersPanelList.Count; i++)
            //{
            //    m_usersPanelList[i].ShowCards(new uint[] { 54, 54, 54 });
            //    m_usersPanelList[i].ShowCards(false);
            //}

            //TableData tableData = Globals.SceneSingleton<DataMng>().GetData<TableData>(DataType.Table);

            //Round.text = (++tableData.CurRound + 1) + "/" + tableData.TotalRound;

            for (int i = 0; i < m_usersPanelList.Count; i++)
            {
                m_usersPanelList[i].SetBanker(false);
            }

            ReadyBtn.SetActive (true);
		}

        void GameEnd(Msg msg)
        {
            Globals.SceneSingleton<AsyncInvokeMng>().Events.Add(delegate ()
            {
                List<ResultUser> results = msg.Get<List<ResultUser>>(0);

                GameResultUIContext context = new GameResultUIContext();
                context.Results = results;

                
                Globals.SceneSingleton<ContextManager>().Push(context);
                Globals.SceneSingleton<StateMng>().ChangeState(StateType.GameResult);
            });
        }

        bool m_timerStart = false;
        float m_timer = -1;
        void OnUpdateClock(Msg msg)
        {
            uint sec = msg.Get<uint>(0);

            Globals.SceneSingleton<AsyncInvokeMng>().Events.Add(delegate ()
            {
                m_timer = sec;
                Clock.text = sec.ToString();
                m_timerStart = true;
            });
        }

        private bool m_alarmVoice = true;
        private void Update()
        {
            if (!m_timerStart)
            {
                m_alarmVoice = true;
                return;
            }

            if(m_timer < 0)
            {
                m_timer = 0;
                Clock.text = "0";

                Clock.transform.DOScale(Vector3.one * 4, 0.85f);
                Clock.DOColor(new Color(0, 0, 0, 0), 0.86f).OnComplete(delegate () 
                {
                    Clock.text = "";
                    Clock.transform.localScale = Vector3.one;
                    Clock.color = Color.black;
                });

                m_alarmVoice = true;
                return;
            }

            m_timer -= Time.deltaTime;
            Clock.text = Mathf.CeilToInt(m_timer).ToString();


            if(Mathf.Abs(m_timer - 4) < 0.1f && m_alarmVoice)
            {
                //Debug.LogError("asdfasdfasdfa");
                Globals.SceneSingleton<SoundMng>().PlayFrontSound("Audio/pdk/comm_timeup_alarm");
                m_alarmVoice = false;
            }
        }


        bool IsSelf(string uuid)
        {
            UserData playerData = Globals.SceneSingleton<DataMng>().GetData<UserData>(DataType.Player);
            if (playerData.uuid == uuid)
            {
                return true;
            }

            return false;
        }

        void OnStartBtnEnabled(Msg msg)
        {
            Globals.SceneSingleton<AsyncInvokeMng>().Events.Add(delegate () 
            {
                StartGameBtn.GetComponent<Button>().interactable = true;
            });
        }

        void OnUpdateBankerState(Msg msg)
        {
            Globals.SceneSingleton<AsyncInvokeMng>().Events.Add(delegate ()
            {
                string theUUID = msg.Get<string>(0);
                if (!m_usersPanelDic.ContainsKey(theUUID))
                {
                    return;
                }

                m_usersPanelDic[theUUID].UpdateBankerState(msg.Get<bool>(1));
            });
        }

        void OnlineStatusChange(Msg msg)
        {
            Globals.SceneSingleton<AsyncInvokeMng>().Events.Add(delegate ()
            {
                string theUUID = msg.Get<string>(0);
                if (!m_usersPanelDic.ContainsKey(theUUID))
                {
                    return;
                }

                bool isOnline = msg.Get<bool>(1);

                m_usersPanelDic[theUUID].SetOnline(isOnline);
            });
        }

        void Reconnected(Msg msg)
        {
            Globals.SceneSingleton<AsyncInvokeMng>().Events.Add(delegate ()
            {
                TableData tableData = Globals.SceneSingleton<DataMng>().GetData<TableData>(DataType.Table);
                uint curStep = tableData.RuleStep;
                switch (curStep)
                {
                    case 0: // 准备中
                        ReadyBtn.SetActive(true);
                        break;
                    case 1: // 下注中
                        BetPanel.SetActive(true);
                        break;
                    case 2: // 抢庄中
                        BankerBtn.SetActive(true);
                        CancelBankerBtn.SetActive(true);
                        break;

                    case 3: // 已发牌再次下注
                        BetPanel.SetActive(true);
                        break;
                    case 4: // 已发牌
                        CuopaiBtn.SetActive(true);
                        ShowCardBtn.SetActive(true);
                        break;
                    case 5: // 一局已结束
                        ReadyBtn.SetActive(true);
                        break;
                }
            });
        }

        void OnEndRecordingVoice(AudioClip clip)
        {
            float[] sampleData = new float[clip.samples];
            clip.GetData(sampleData, 0);

            short[] preCompressedData = CompressUtil.PreCompressAudio(sampleData);

            List<byte> byteData = new List<byte>();
            for (int i = 0; i < preCompressedData.Length; i++)
            {
                byteData.AddRange(System.BitConverter.GetBytes(preCompressedData[i]));
            }

            List<byte> testByteData = new List<byte>();
            for (int i = 0; i < sampleData.Length; i++)
            {
                testByteData.AddRange(System.BitConverter.GetBytes(sampleData[i]));
            }

            byte[] testCompressData = CompressUtil.Compress(testByteData.ToArray());

            byte[] compressedData = CompressUtil.Compress(byteData.ToArray());
            
            CmdRequest.AudioStreamUpload(compressedData);
        }

        void ShowVoiceImg(Msg msg)
        {
            string uuid = msg.Get<string>(0);
            float time = msg.Get<float>(1);

            if (m_usersPanelDic.ContainsKey(uuid))
            {
                m_usersPanelDic[uuid].ShowVoice(time);
            }
        }

        void ShowMessageBtn(Msg msg)
        {
            Globals.SceneSingleton<AsyncInvokeMng>().Events.Add(delegate () 
            {
                TextMessageBtn.interactable = true;
                VoiceMessageBtn.interactable = true;
            });
        }



        class UserPanel
        {
            public TablePlayerData Data;

            private GameObject m_panelObj;
            private Text m_name;
            private Image m_icon;
            public Text ScoreTxt { get { return m_score; } }
            private Text m_score;

            private GameObject m_bgEffect;
            private GameObject m_iconEffect;

            private GameObject m_betRoot;
            private Text m_betNum;
            private Text m_betNumEffect;

            private Image m_bankerImg;
            private Image m_bankerImgEffect;

            private Image m_readyImg;

            private Image m_offLineImg;

            private GameObject m_cuoPaiZhong;

            private GameObject m_cardResultBg;
            private Text m_cardResult;

            private SingleCard[] m_cards;

            private ShowMessage m_message;

            private AutoFade m_bankerState;

            private GameObject m_voiceImg;


            public UserPanel(GameObject objRoot)
            {
                m_panelObj = objRoot;
                m_name = objRoot.transform.Find("name").GetComponent<Text>();
                m_icon = objRoot.transform.Find("icon_parent/icon").GetComponent<Image>();
                m_score = objRoot.transform.Find("score").GetComponent<Text>();

                m_bgEffect = objRoot.transform.Find("bgFx").gameObject;
                m_iconEffect = objRoot.transform.Find("icon_fx").gameObject;

                m_betRoot = objRoot.transform.Find("BetPanel").gameObject;
                m_betNum = objRoot.transform.Find("BetPanel/bet").GetComponent<Text>();
                m_betNumEffect = objRoot.transform.Find("BetPanel/betFx").GetComponent<Text>();

                m_bankerImg = objRoot.transform.Find("ownerTag").GetComponent<Image>();
                m_bankerImgEffect = objRoot.transform.Find("ownerTagFx").GetComponent<Image>();

                m_readyImg = objRoot.transform.Find("readyImg").GetComponent<Image>();

                m_cardResultBg = objRoot.transform.Find("cardResult_bg").gameObject;
                m_cardResult = objRoot.transform.Find("cardResult").GetComponent<Text>();

                m_message = objRoot.transform.Find("Message").GetComponent<ShowMessage>();

                m_cuoPaiZhong = objRoot.transform.Find("CuoPaiZhong").gameObject;

                m_cards = objRoot.GetComponentsInChildren<SingleCard>();

                m_bankerState = objRoot.transform.Find("bankerState").GetComponent<AutoFade>();

                m_offLineImg = objRoot.transform.Find("offline").GetComponent<Image>();

                Reset();
            }

            public void Reset()
            {
                m_name.text = "";
                m_score.text = "0";

                m_iconEffect.SetActive(false);
                m_bgEffect.SetActive(false);

                m_betRoot.SetActive(false);
                m_betNum.text = "0";
                m_betNumEffect.text = "0";

                m_bankerImg.gameObject.SetActive(false);
                m_bankerImgEffect.gameObject.SetActive(false);

                m_readyImg.gameObject.SetActive(false);
                m_cardResult.text = "";
                m_cardResultBg.SetActive(false);

                m_cuoPaiZhong.SetActive(false);

                m_offLineImg.gameObject.SetActive(false);

                for (int i = 0; i < m_cards.Length; i++)
                {
                    m_cards[i].gameObject.SetActive(false);
                }

                //m_panelObj.SetActive(false);
            }

            public void Reset(UserData userdata)
            {
                m_name.text = userdata.user_nick_name;
                Globals.GetHeadImgByUrl(userdata.user_head_img, (x) =>
                {
                    if (null == x)
                    {
                        return;
                    }

                    m_icon.sprite = Sprite.Create(x as Texture2D, new Rect(0, 0, x.width, x.height), Vector2.one * 0.5f);
                });
                //m_score.text = "0";


                m_betNum.text = "0";
                m_betNumEffect.text = "0";
                m_bankerImg.gameObject.SetActive(false);
                m_bankerImgEffect.gameObject.SetActive(false);

                m_readyImg.gameObject.SetActive(false);

                m_cardResultBg.SetActive(false);
                m_cardResult.text = "";

                m_cuoPaiZhong.SetActive(false);

                for (int i = 0; i < m_cards.Length; i++)
                {
                    m_cards[i].gameObject.SetActive(false);
                }
            }

            public void Reset(TablePlayerData userdata)
            {
                Data = userdata;

                m_name.text = userdata.NickName;
                Globals.GetHeadImgByUrl(userdata.HeadImg, (x) => 
                {
                    if (null == x)
                    {
                        return;
                    }

                    m_icon.sprite = Sprite.Create(x as Texture2D, new Rect(0, 0, x.width, x.height), Vector2.one * 0.5f);
                });
                //m_score.text = "0";


                m_betNum.text = "0";
                m_betNumEffect.text = "0";
                m_bankerImg.gameObject.SetActive(false);
                m_bankerImgEffect.gameObject.SetActive(false);

                m_readyImg.gameObject.SetActive(false);

                m_cardResultBg.SetActive(false);
                m_cardResult.text = "";

                for (int i = 0; i < m_cards.Length; i++)
                {
                    m_cards[i].gameObject.SetActive(false);
                }
            }

            public void Show(bool isShow)
            {
                m_panelObj.SetActive(isShow);
            }

            public void SetBet(int num)
            {
                if(num == 0)
                {
                    m_betNum.text = num.ToString();
                }

                m_betRoot.SetActive(true);

                m_betNumEffect.gameObject.SetActive(true);
                m_betNumEffect.text = num.ToString();
                m_betNumEffect.transform.position = new Vector3(Screen.width / 2, Screen.height / 2, 0);
                m_betNumEffect.transform.DOMove(m_betNum.transform.position, 0.6f).OnComplete(delegate () 
                {
                    m_betNumEffect.gameObject.SetActive(false);
                    m_betNumEffect.text = "";
                    m_betNum.text = num.ToString();
                });
            }

            public void SetBanker()
            {
                m_bgEffect.SetActive(true);
                m_bankerImgEffect.gameObject.SetActive(true);
                m_bankerImgEffect.transform.position = new Vector3(Screen.width / 2, Screen.height / 2, 0);
                m_bankerImgEffect.transform.DOMove(m_bankerImg.transform.position, 0.6f).OnComplete(delegate ()
                {
                    m_bankerImgEffect.gameObject.SetActive(false);
                    m_bankerImg.gameObject.SetActive(true);
                    m_bgEffect.SetActive(false);
                });
            }

            public void SetBanker(bool isShow)
            {
                m_bgEffect.SetActive(false);
                m_bankerImgEffect.gameObject.SetActive(false);
                m_bankerImg.gameObject.SetActive(false);
            }

            public void SetBankerEffect(bool isShow)
            {
                m_bgEffect.SetActive(isShow);
            }

            public void SetCardResult(string result)
            {
                m_cardResultBg.SetActive(true);

                m_cardResult.text = result;
                m_cardResult.transform.localScale = Vector3.one * 5;
                m_cardResult.transform.DOScale(Vector3.one, 1.0f).SetEase(Ease.InCubic);

                m_cardResult.color = new Color(0, 0, 0, 0);
                m_cardResult.DOColor(new Color(1.0f, 0.85f, 0, 1.0f), 0.3f);

                m_cardResult.transform.DOPunchPosition(Vector3.up, 0.2f).SetDelay(1.0f);
            }

            public void SetReady(bool isReady)
            {
                m_iconEffect.SetActive(isReady);
                m_readyImg.gameObject.SetActive(isReady);
            }

            public void SetOnline(bool isOnline)
            {
                m_offLineImg.gameObject.SetActive(!isOnline);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="mode">1 发三张牌，2发一张牌</param>
            public void DealAnimation(int mode)
            {
                GameObject card1 = m_cards[0].gameObject;
                GameObject card2 = m_cards[1].gameObject;

				card1.SetActive (true);
				card2.SetActive (true);

                Vector3 originPos = new Vector3(Screen.width / 2, Screen.height / 2, 0);
                Vector3 targetPos1 = card1.transform.position;
                Vector3 targetPos2 = card2.transform.position;

                card1.transform.position = originPos;
                card2.transform.position = originPos;
                card1.transform.DOMove(targetPos1, 0.7f);


                if (mode == 1)
                {
                    card2.transform.DOMove(targetPos2, 0.7f).SetDelay(0.2f);
                }
                else
                {
                    card2.transform.DOMove(targetPos2, 0.7f).SetDelay(0.2f).OnComplete(delegate() 
                    {
                        m_cuoPaiZhong.SetActive(true);
                    });
                    return;
                }











                GameObject card3 = m_cards[2].gameObject;

                card3.SetActive(true);

                originPos = new Vector3(Screen.width / 2, Screen.height / 2, 0);
                Vector3 targetPos3 = card3.transform.position;
                
                card3.transform.position = originPos;
                card3.transform.DOMove(targetPos3, 0.7f).SetDelay(0.4f).OnComplete(() =>
                {
                    m_cuoPaiZhong.SetActive(true);
                });
            }

            public void ShowCards(uint[] cards)
            {
                for(int i = 0; i < cards.Length; i++)
                {
                    if(cards[i] > 54)
                    {
                        continue;
                    }

                    m_cards[i].SetNum(cards[i]);
                    m_cards[i].gameObject.SetActive(true);
                }

                m_cuoPaiZhong.SetActive(false);
            }

			public void ShowCards(bool isShow)
			{
				for(int i = 0; i < m_cards.Length; i++)
				{
					m_cards [i].SetNum (54);
					m_cards [i].gameObject.SetActive (isShow);
				}

                m_cardResultBg.SetActive(false);
				m_cardResult.text = "";

			}

            public void ShowMessage(int index)
            {
                m_message.ShowExpression(index);
            }

            public void ShowVoice(float time)
            {
                m_message.ShowVoice(time);
            }

            //public void SetScore(int point)
            //{
            //    m_score.text = point.ToString();
            //}

            public void ShowUpdatedScore(UserPanel from)
            {
                Transform fromPos = from.ScoreTxt.transform;

                //int fromPoint = int.Parse(from.ScoreTxt.text) - (int)point;
                //int toPoint = int.Parse(m_score.text) + (int)point;

                Debug.Log("积分动画:" + from.m_name.text + " " + m_name.text);

                List<GameObject> scoreEffects = new List<GameObject>();
                for (int i = 0; i < 20; i++)
                {
                    GameObject scoreEffect = Instantiate(Resources.Load("Prefab/UI/TableView/scoreFx") as GameObject);
                    scoreEffect.transform.SetParent(m_panelObj.transform, false);
                    scoreEffect.transform.position = fromPos.position + Vector3.right * Random.Range(-25, 25);
                    scoreEffect.transform.DOMove(m_score.transform.position + Vector3.right * Random.Range(-25, 25), 0.5f).SetDelay(Random.Range(0f, 0.5f));
                    scoreEffects.Add(scoreEffect);
                }
                
                m_score.transform.DOScale(Vector3.one, 1.0f).OnComplete(delegate () 
                {
                    //from.ScoreTxt.text = fromPoint.ToString();
                    //m_score.text = finalPoint.ToString();

                    scoreEffects.ForEach((x) => { GameObject.Destroy(x); });
                    scoreEffects.Clear();
                });
            }


            public bool IsBankering = false;
            public void UpdateBankerState(bool isBankering)
            {
                IsBankering = isBankering;
                m_bankerState.SetState(isBankering ? 0 : 1);
            }
        }


        class VoicePanel
        {
            public GameObject RootObj;
            private VoiceTimerAnim m_timerAnim;


            public VoicePanel(GameObject obj)
            {
                RootObj = obj;

                Init();
            }

            void Init()
            {
                m_timerAnim = RootObj.transform.Find("outline").GetComponent<VoiceTimerAnim>();
            }

            public void Show(bool isShow)
            {
                RootObj.SetActive(isShow);

                if (isShow)
                {
                    m_timerAnim.StartAnim();
                }
            }
        }


















        private void LateUpdate()
        {
            ClockBG.SetActive(!(Clock.text.Length <= 0 || Clock.text == "0"));
        }


        private void OnEnable()
        {
            //UserPanel p = new UserPanel(UserPanels[0]);
            //p.Reset();
            ////p.SetBanker();
            //p.SetBet(100);
            //p.SetCardResult("大罗");
            //p.ShowCards(new uint[] { 13,31,42 });
        }


        public void OnGUI()
        {
			/*
            if (GUI.Button(new Rect(0, 0, 100, 50), "发牌"))
            {
                //CmdRequest.EnterRoomRequest();
            }

            if (GUI.Button(new Rect(0, 100, 100, 50), "小结算"))
            {
                for(int i = 0; i < m_usersPanelList.Count; i++)
                {
                    if(i != 0 && m_usersPanelList[i].Data == null)
                    {
                        break;
                    }

                    m_usersPanelList[i].ShowCards(new uint[] { 3, 7, 6 });
                    m_usersPanelList[i].SetCardResult("九点");
                }
            }

            if (GUI.Button(new Rect(0, 200, 100, 50), "大结算"))
            {
                Globals.SceneSingleton<ContextManager>().Push(new GameResultUIContext());
            }

            /*
            if (GUI.Button(new Rect(0, 0, 100, 50), "进入房间"))
            {
                CmdRequest.EnterRoomRequest();
            }

            if (GUI.Button(new Rect(0, 100, 100, 50), "退出房间"))
            {
                CmdRequest.ExitRoomRequest();
            }

            if (GUI.Button(new Rect(0, 200, 100, 50), "解散房间"))
            {
                CmdRequest.DismissRoomRequest();
            }

            if (GUI.Button(new Rect(0, 300, 100, 50), "投票"))
            {
                CmdRequest.PlayerVoteDismissRoomRequest(false);
            }

            if (GUI.Button(new Rect(0, 400, 100, 50), "准备"))
            {
                CmdRequest.ReadyRequest();
            }

            if (GUI.Button(new Rect(0, 500, 100, 50), "重连请求"))
            {
                CmdRequest.ReconnectRequest();
            }

            if (GUI.Button(new Rect(0, 600, 100, 50), "下注"))
            {
                CmdRequest.ClientBetRequest(10);
            }

            if (GUI.Button(new Rect(200, 0, 100, 50), "再次下注"))
            {
                CmdRequest.ClientBetAgainRequest(10);
            }

            if (GUI.Button(new Rect(200, 100, 100, 50), "抢庄"))
            {
                CmdRequest.ClientBankerRequest();


            }
            */
        }
    }
}