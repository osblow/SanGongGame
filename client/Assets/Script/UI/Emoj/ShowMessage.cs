using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Osblow.App;

public class ShowMessage : MonoBehaviour 
{
    private string m_emojResourcesPath = "Texture/";
    private Dictionary<int, string> m_textDic = new Dictionary<int, string>()
    {
        {15, "不要走，决战到天亮" },
        {16, "一手牌烂到底" },
        {17, "又断线了，郁闷" },
        {18, "不要吵啦，认真玩游戏吧" },
        {19, "大家好" },
    };

    public Text Txt;
    public Image Emoj;
    public GameObject Txtbg;
    public GameObject VoiceImg;


	public void ShowExpression(int index)
    {
        if(index < 15)
        {
            Txt.text = "";
            Txtbg.SetActive(false);
            Emoj.gameObject.SetActive(true);
            Emoj.sprite = Resources.Load<Sprite>(m_emojResourcesPath + index);
        }
        else if(index < 20)
        {
            Emoj.gameObject.SetActive(false);
            Txtbg.SetActive(true);
            Txt.text = m_textDic[index];
            Globals.SceneSingleton<SoundMng>().PlayFrontSound("Audio/fw/fw_female_" + (index - 15));
        }

        AutoHide();
    }

    void AutoHide()
    {
        if (null != m_hideLaterCoroutine)
        {
            StopCoroutine(m_hideLaterCoroutine);
        }

        m_hideLaterCoroutine = StartCoroutine(HideLater(1.0f));
    }

    public void Hide()
    {
        Emoj.gameObject.SetActive(false);
        Txtbg.SetActive(false);
        VoiceImg.SetActive(false);
        Txt.text = "";
    }


    public void ShowVoice(float time)
    {
        VoiceImg.SetActive(true);

        if(null != m_hideLaterCoroutine)
        {
            StopCoroutine(m_hideLaterCoroutine);
        }

        m_hideLaterCoroutine = StartCoroutine(HideLater(time));
    }

    private Coroutine m_hideLaterCoroutine;
    IEnumerator HideLater(float duration)
    {
        yield return new WaitForSeconds(duration);
        Hide();
    }

    private void Awake()
    {
        Hide();
    }
}
