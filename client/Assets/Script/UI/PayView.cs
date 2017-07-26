using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Osblow.App
{
    public class PayUIContext : BaseContext
    {
        public PayUIContext() : base(UIType.PayView) { }

        public List<RechargeData> Records;
    }

    public class PayView : BaseView
    {
        #region 场景引用 
        public GameObject PayPanel;
        public GameObject RecordPanel;
        public Transform RecordRoot;

        public ButtonTexChange PayTabBtn;
        public ButtonTexChange RecordTabBtn;
        #endregion

        #region 场景事件
        public void OnExitBtn()
        {
            Globals.SceneSingleton<ContextManager>().Pop();
            Globals.SceneSingleton<SoundMng>().PlayCommonButtonSound();
        }

        public void OnClickPayBtn()
        {
            PayPanel.SetActive(true);
            RecordPanel.SetActive(false);

            PayTabBtn.Set(1);
            RecordTabBtn.Set(0);
            Globals.SceneSingleton<SoundMng>().PlayCommonButtonSound();
        }

        public void OnClickRecordBtn()
        {
            PayPanel.SetActive(false);
            RecordPanel.SetActive(true);

            PayTabBtn.Set(0);
            RecordTabBtn.Set(1);
            Globals.SceneSingleton<SoundMng>().PlayCommonButtonSound();
        }
        #endregion
        private const string c_itemPath = "Prefab/UI/PayView/RawContent";


        public override void OnEnter(BaseContext context)
        {
            base.OnEnter(context);

            PayUIContext theContent = context as PayUIContext;
            for (int i = 0; i < theContent.Records.Count; i++)
            {
                GameObject newItem = Instantiate(Resources.Load(c_itemPath) as GameObject);
                newItem.transform.SetParent(RecordRoot, false);

                new RawContent(newItem, theContent.Records[i]);
            }
        }

        public override void OnExit(BaseContext context)
        {
            base.OnExit(context);
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
            for (int i = RecordRoot.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(RecordRoot.GetChild(i).gameObject);
            }
        }
    }


    class RawContent
    {
        public GameObject RootObj;

        private Text Num;
        private Text Count;
        private Text Money;
        private Text Code;
        private Text Status;
        private Text Time;


        public RawContent(GameObject obj, RechargeData data)
        {
            RootObj = obj;

            Init(data);
        }

        void Init(RechargeData data)
        {
            Num = RootObj.transform.Find("Num").GetComponent<Text>();
            Count = RootObj.transform.Find("Count").GetComponent<Text>();
            Money = RootObj.transform.Find("Money").GetComponent<Text>();
            Code = RootObj.transform.Find("Code").GetComponent<Text>();
            Status = RootObj.transform.Find("Status").GetComponent<Text>();
            Time = RootObj.transform.Find("Time").GetComponent<Text>();

            Num.text = data.Number;
            Count.text = data.Count.ToString();
            Money.text = data.Money.ToString();
            Code.text = data.InviteCode;
            Status.text = data.Status;
            Time.text = data.Time;
        }
    }
}
