using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowMessage : MonoBehaviour 
{
    private string m_emojResourcesPath = "Texture/";
    private Dictionary<int, string> m_textDic = new Dictionary<int, string>()
    {
        {15, "快点吧，我等到花儿都谢了" },
        {16, "我的网络有点差" },
        {17, "别走，我们再战一局" },
        {18, "哈哈，运气" },
        {19, "要洗洗睡了" },
    };

    public Text Txt;
    public Image Emoj;
    public GameObject Txtbg;


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
        }

        StartCoroutine(AutoHide());
    }

    IEnumerator AutoHide()
    {
        yield return new WaitForSeconds(1.0f);
        Hide();
    }

    public void Hide()
    {
        Emoj.gameObject.SetActive(false);
        Txtbg.SetActive(false);
        Txt.text = "";
    }


    private void Awake()
    {
        Hide();
    }
}
