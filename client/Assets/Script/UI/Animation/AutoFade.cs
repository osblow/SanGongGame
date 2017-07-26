using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AutoFade : MonoBehaviour 
{
    private Text m_text;


    private void Awake()
    {
        m_text = GetComponent<Text>();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="state">0抢庄，1不抢</param>
	public void SetState(int state)
    {
        if(state == 0)
        {
            m_text.text = "抢庄";
        }
        else if(state == 1)
        {
            m_text.text = "不抢";
        }

        m_text.color = new Color(1, 0.725f, 0, 1);
        m_text.DOColor(new Color(0, 0, 0, 0), 0.5f).SetDelay(1.5f);
    }
}
