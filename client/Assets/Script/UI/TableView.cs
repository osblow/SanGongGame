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
        public GameObject BetPanel;
        public GameObject CuopaiBtn;
        public GameObject ShowCardBtn;

        [Space]
        public GameObject EmojsPanel;

        [Space]
        public GameObject CuoPaiController;

        [Space]
        public GameObject BetItemGrid;
        public GameObject BetItemTpl;
        #endregion

        #region 场景事件
        public void OnMenuBtn()
        {
            ShowMenu();
        }

        public void OnExitBtn()
        {
            CmdRequest.ExitRoomRequest();
            MenuPanelObj.SetActive(false);
            //Globals.SceneSingleton<ContextManager>().Push(new LobbyUIContext());
        }

        public void OnDismissBtn()
        {
            CmdRequest.DismissRoomRequest();
            MenuPanelObj.SetActive(false);
        }

        public void OnShareBtn()
        {
            MenuPanelObj.SetActive(false);
        }


        public void StartGame()
        {
            //
            CmdRequest.StartGameRequest();
        }


        public void OnCuoPai()
        {
            CuoPai();
        }

        public void OnShowCards()
        {
            
            //ShowCards();
        }

        public void OnSitDownBtn()
        {
            SitDownBtn.SetActive(false);

            if (m_isOwner)
            {
                StartGameBtn.SetActive(true);
            }

            //ChaseOwnerBtn.SetActive(true);
            //StartCoroutine(TestCreateUsers());
            CmdRequest.ReadyRequest();
        }

        public void OnBankeringBtn()
        {
            BankerBtn.SetActive(false);
            BetPanel.SetActive(true);
        }

        public void OnBetBtn(Text label)
        {
            int betScore = int.Parse(label.text);
            BetPanel.SetActive(false);

            // test
            //m_cardsPanel.ForEach((x) => { x.SetActive(true); });
            //Deal();
        }

        public void OnShowUserInfo(int index)
        {
            TablePlayerData theData = m_usersPanelList[index].Data;
            UserInfoUIContext context = new UserInfoUIContext();
            context.ThePlayerData = theData;
            Globals.SceneSingleton<ContextManager>().Push(context);
        }

        public void OnClickEmoj(int index)
        {
            HideEmojPanel();

            CmdRequest.ClientExpressionRequest((uint)index);
        }

        public void OnTextMessage()
        {
            ShowEmojPanel();
        }
        #endregion

        private List<UserPanel> m_usersPanelList = new List<UserPanel>();
        private Dictionary<string, UserPanel> m_usersPanelDic = new Dictionary<string, UserPanel>();
        private int m_curPlayerCount = 0;
        private bool m_isOwner = false;
        private bool m_isEmojHidden = true;

        void ShowMenu()
        {
            //Globals.SceneSingleton<ContextManager>().Push(new LobbyUIContext());
            MenuPanelObj.SetActive(!MenuPanelObj.activeInHierarchy);
        }

        void CuoPai()
        {
            CuoPaiController.SetActive(true);
        }

        void ShowCards()
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
            m_usersPanelList.ForEach((x) => { x.DealAnimation(); });
            UserPanels.ForEach((x) => { DealAnimation(x); });
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

        private readonly Vector3 m_emojHiddenPos = new Vector3(1416, 376, 0);
        private readonly Vector3 m_emojShowPos = new Vector3(926, 376, 0);
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

                m_usersPanelList[j].Show(true);
                m_usersPanelList[j].Reset(tableData.Players[i]);
            }

            CuoPaiController.SetActive(false);

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

            Globals.SceneSingleton<AsyncInvokeMng>().EventsToAct += delegate ()
            {
                m_usersPanelList[m_curPlayerCount].Show(true);
                m_usersPanelList[m_curPlayerCount].Reset(theData);
                m_usersPanelDic.Add(theUUID, m_usersPanelList[m_curPlayerCount]);
            };
        }

        void OnPlayerReady(Msg msg)
        {
            Globals.SceneSingleton<AsyncInvokeMng>().EventsToAct += delegate ()
            {
                string theUUID = msg.Get<string>(0);

                if (theUUID == Globals.SceneSingleton<DataMng>().GetData<UserData>(DataType.Player).uuid)
                {
                    //StartGameBtn.SetActive(true);
                }

                if (!m_usersPanelDic.ContainsKey(theUUID))
                {
                    return;
                }
                m_usersPanelDic[theUUID].SetReady(true);
            };
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
            Globals.SceneSingleton<AsyncInvokeMng>().EventsToAct += delegate ()
            {
                SitDownBtn.SetActive(false);
                StartGameBtn.SetActive(false);
                BetPanel.SetActive(true);
            };
        }

        void OnPlayerBet(Msg msg)
        {
            string theUUID = msg.Get<string>(0);
            uint thePoint = msg.Get<uint>(1);

            Globals.SceneSingleton<AsyncInvokeMng>().EventsToAct += delegate ()
            {
                m_usersPanelDic[theUUID].SetBet((int)thePoint);
            };
        }

        void OnExpression(Msg msg)
        {
            string uuid = msg.Get<string>(0);
            uint expressionId = msg.Get<uint>(1);

            Globals.SceneSingleton<AsyncInvokeMng>().EventsToAct += delegate ()
            {
                if (!m_usersPanelDic.ContainsKey(uuid))
                {
                    return;
                }
                m_usersPanelDic[uuid].ShowMessage((int)expressionId);
            };
        }

        void OnDealCards(Msg msg)
        {
            Globals.SceneSingleton<AsyncInvokeMng>().EventsToAct += delegate ()
            {
                for (int i = 0; i < m_usersPanelList.Count; i++)
                {
                    if (m_usersPanelList[i].Data != null || i == 0)
                    {
                        m_usersPanelList[i].DealAnimation();
                    }
                }

                CuopaiBtn.SetActive(true);
                ShowCardBtn.SetActive(true);
            };
        }
        

        class UserPanel
        {
            public TablePlayerData Data;

            private GameObject m_panelObj;
            private Text m_name;
            private Image m_icon;
            private Text m_score;

            private Text m_betNum;
            private Text m_betNumEffect;
            private Image m_bankerImg;
            private Image m_bankerImgEffect;

            private Image m_readyImg;

            private Image m_offLineImg;

            private Text m_cardResult;

            private SingleCard[] m_cards;

            private ShowMessage m_message;


            public UserPanel(GameObject objRoot)
            {
                m_panelObj = objRoot;
                m_name = objRoot.transform.Find("name").GetComponent<Text>();
                m_icon = objRoot.transform.Find("icon").GetComponent<Image>();
                m_score = objRoot.transform.Find("score").GetComponent<Text>();

                m_betNum = objRoot.transform.Find("BetPanel/bet").GetComponent<Text>();
                m_betNumEffect = objRoot.transform.Find("BetPanel/betFx").GetComponent<Text>();

                m_bankerImg = objRoot.transform.Find("ownerTag").GetComponent<Image>();
                m_bankerImgEffect = objRoot.transform.Find("ownerTagFx").GetComponent<Image>();

                m_readyImg = objRoot.transform.Find("readyImg").GetComponent<Image>();

                m_cardResult = objRoot.transform.Find("cardResult").GetComponent<Text>();

                m_message = objRoot.transform.Find("Message").GetComponent<ShowMessage>();

                m_cards = objRoot.GetComponentsInChildren<SingleCard>();

                Reset();
            }

            public void Reset()
            {
                m_name.text = "";
                m_score.text = "0";


                m_betNum.text = "0";
                m_betNumEffect.text = "0";
                m_bankerImg.gameObject.SetActive(false);
                m_bankerImgEffect.gameObject.SetActive(false);

                m_readyImg.gameObject.SetActive(false);
                m_cardResult.text = "";

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
                m_score.text = "0";


                m_betNum.text = "0";
                m_betNumEffect.text = "0";
                m_bankerImg.gameObject.SetActive(false);
                m_bankerImgEffect.gameObject.SetActive(false);

                m_readyImg.gameObject.SetActive(false);
                m_cardResult.text = "";

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
                m_score.text = "0";


                m_betNum.text = "0";
                m_betNumEffect.text = "0";
                m_bankerImg.gameObject.SetActive(false);
                m_bankerImgEffect.gameObject.SetActive(false);

                m_readyImg.gameObject.SetActive(false);
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
                m_betNumEffect.text = num.ToString();
                m_betNumEffect.transform.position = new Vector3(Screen.width / 2, Screen.height / 2, 0);
                m_betNumEffect.transform.DOMove(m_betNum.transform.position, 0.6f).OnComplete(delegate () 
                {
                    m_betNumEffect.text = "";
                    m_betNum.text = num.ToString();
                });
            }

            public void SetBanker()
            {
                m_bankerImgEffect.gameObject.SetActive(true);
                m_bankerImgEffect.transform.position = new Vector3(Screen.width / 2, Screen.height / 2, 0);
                m_bankerImgEffect.transform.DOMove(m_bankerImg.transform.position, 0.6f).OnComplete(delegate ()
                {
                    m_bankerImgEffect.gameObject.SetActive(false);
                    m_bankerImg.gameObject.SetActive(true);
                });
            }

            public void SetCardResult(string result)
            {
                m_cardResult.text = result;
                m_cardResult.transform.localScale = Vector3.one * 5;
                m_cardResult.transform.DOScale(Vector3.one, 1.0f).SetEase(Ease.InCubic);

                m_cardResult.color = new Color(0, 0, 0, 0);
                m_cardResult.DOColor(Color.white, 0.3f);

                m_cardResult.transform.DOPunchPosition(Vector3.up, 0.2f).SetDelay(1.0f);
            }

            public void SetReady(bool isReady)
            {
                m_readyImg.gameObject.SetActive(isReady);
            }

            public void SetOnline(bool isOnline)
            {
                m_offLineImg.gameObject.SetActive(!isOnline);
            }

            public void DealAnimation()
            {
                GameObject card1 = m_cards[0].gameObject;
                GameObject card2 = m_cards[1].gameObject;
                GameObject card3 = m_cards[2].gameObject;

                Vector3 originPos = new Vector3(Screen.width / 2, Screen.height / 2, 0);
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

            public void ShowCards(uint[] cards)
            {
                for(int i = 0; i < cards.Length; i++)
                {
                    m_cards[i].SetNum(cards[i]);
                }
            }

            public void ShowMessage(int index)
            {
                m_message.ShowExpression(index);
            }
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