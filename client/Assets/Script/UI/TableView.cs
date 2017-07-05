using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Osblow.Game;


namespace Osblow.App
{
    public class TableUIContext : BaseContext
    {
        public TableUIContext() : base(UIType.TableView) { }

        public TableData TableData;
    }

    public class TableView : BaseView
    {
        #region 场景引用 
        public List<GameObject> UserPanels;

        [Space]
        public Text RoomId;
        public Text Round;
        public Text Mode;
        public Text Owner;
        public Text BaseScore;

        [Space]
        public GameObject MenuPanelObj;

        [Space]
        public GameObject SitDownBtn;
        public GameObject ChaseOwnerBtn;
        public GameObject BetPanel;
        #endregion

        #region 场景事件
        public void OnMenuBtn()
        {
            ShowMenu();
        }

        public void OnExitBtn()
        {
            Globals.SceneSingleton<ContextManager>().Push(new LobbyUIContext());
        }

        public void OnDismissBtn()
        {

        }

        public void OnShareBtn()
        {

        }



        public void OnCuoPai()
        {
            //CuoPai();
        }

        public void OnShowCards()
        {
            ShowCards();
        }

        public void OnSitDownBtn()
        {
            SitDownBtn.SetActive(false);
            //ChaseOwnerBtn.SetActive(true);
            StartCoroutine(TestCreateUsers());
        }

        public void OnChaseOwnerBtn()
        {
            ChaseOwnerBtn.SetActive(false);
            BetPanel.SetActive(true);
        }

        public void OnBetBtn(Text label)
        {
            int betScore = int.Parse(label.text);
            BetPanel.SetActive(false);

            // test
            m_cardsPanel.ForEach((x) => { x.SetActive(true); });
            Deal();
        }
        #endregion
        void ShowMenu()
        {
            //Globals.SceneSingleton<ContextManager>().Push(new LobbyUIContext());
            MenuPanelObj.SetActive(!MenuPanelObj.activeInHierarchy);
        }

        void CuoPai()
        {

        }

        void ShowCards()
        {
            // test
            int randomSuit = 0;
            int randomNumber = 0;
            int value = 0;
            for(int i = 0; i < m_cards.Count; i++)
            {
                for (int j = 0; j < m_cards[i].Count; j++)
                {
                    value = Random.Range(0, 52);
                    m_cards[i][j].SetNum(value);
                }
            }

            // test
            Invoke("ShowGameResult", 1.0f);
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

            ChaseOwnerBtn.SetActive(true);
        }



        private List<GameObject> m_cardsPanel = new List<GameObject>();
        private List<List<SingleCard>> m_cards = new List<List<SingleCard>>();

        public override void OnEnter(BaseContext context)
        {
            base.OnEnter(context);
            TableUIContext theContext = context as TableUIContext;
            if (null != theContext.TableData)
            {
                Mode.text = theContext.TableData.SanGong == 0 ? "三公" : "双公";
                Owner.text = theContext.TableData.HasOwner == 0 ? "是" : "否";
                BaseScore.text = theContext.TableData.BaseScore.ToString();
            }
            // cards panel
            for(int i = 0; i < UserPanels.Count; i++)
            {
                GameObject theCardsPanel = UserPanels[i].transform.Find("MyCardsGrid").gameObject;
                m_cardsPanel.Add(theCardsPanel);
                theCardsPanel.SetActive(false);

                List<SingleCard> userCards = new List<SingleCard>();
                GameObject card1 = UserPanels[i].transform.Find("MyCardsGrid/card").gameObject;
                GameObject card2 = UserPanels[i].transform.Find("MyCardsGrid/card (1)").gameObject;
                GameObject card3 = UserPanels[i].transform.Find("MyCardsGrid/card (2)").gameObject;
                userCards.Add(card1.AddComponent<SingleCard>());
                userCards.Add(card2.AddComponent<SingleCard>());
                userCards.Add(card3.AddComponent<SingleCard>());
                m_cards.Add(userCards);

                if(i > 0)
                {
                    UserPanels[i].SetActive(false);
                }
            }
        }

        public override void OnExit(BaseContext context)
        {
            base.OnExit(context);
            Globals.SceneSingleton<UIManager>().DestroySingleUI(UIType.TableView);
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