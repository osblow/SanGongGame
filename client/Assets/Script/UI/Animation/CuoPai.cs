using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Osblow.Util;
using DG.Tweening;
using Osblow.Game;


public class CuoPai : MonoBehaviour 
{
    public int Index = 0;
    public Transform AnimTarget;

    RectTransform m_recttrans;
    bool m_isInAnimation = false;

    readonly Vector3 c_orginPos = new Vector3(-211.5f, -306.5f, 0);
    readonly float c_targetScale = 0.247f;

    private void Awake()
    {
        MsgMng.AddListener(MsgType.UI_CuoPaiEnd, CuoPaiEndListener);
    }

    private void OnEnable()
    {
        m_recttrans = GetComponent<RectTransform>();
        m_recttrans.localPosition = c_orginPos;
        m_recttrans.localScale = Vector3.one;
        m_recttrans.rotation = Quaternion.identity;
        m_isInAnimation = false;
    }

    // Update is called once per frame
    void Update () 
	{
		
	}



    public void Show(bool isShow)
    {
        gameObject.SetActive(isShow);
    }



    private Vector3 m_lastBtnDownPos;
    private float m_lastRotZ;
    public void OnDragBegin()
    {
        m_lastBtnDownPos = Input.mousePosition;
        m_lastRotZ = m_recttrans.rotation.eulerAngles.z;
    }

    public void OnDrag()
    {
        float dir = Input.mousePosition.x - m_lastBtnDownPos.x;
        m_recttrans.rotation = Quaternion.Euler(0, 0, m_lastRotZ - dir * 0.1f);
    }

    public void OnDragEnd()
    {
        if(Index >= 1 && m_recttrans.rotation.eulerAngles.z > 180 
            && m_recttrans.rotation.eulerAngles.z < 344)
        {
            MsgMng.Dispatch(MsgType.UI_CuoPaiEnd);
        }
    }

    
    void CuoPaiEndListener(Msg msg)
    {
        if (m_isInAnimation)
        {
            return;
        }

        Animate();
    }

    void Animate()
    {
        m_isInAnimation = true;

        Vector3 step1_pos = m_recttrans.position;
        if(Index == 0)
        {
            step1_pos = m_recttrans.position + Vector3.right * 100;
        }else if(Index == 2)
        {
            step1_pos = m_recttrans.position - Vector3.right * 100;
        }

        m_recttrans.DORotate(Vector3.zero, 0.3f);
        m_recttrans.DOMove(step1_pos, 0.3f);

        m_recttrans.DOScale(Vector3.one * c_targetScale, 0.4f).SetDelay(0.3f);
        m_recttrans.DOMove(AnimTarget.position - new Vector3(40, 57, 0), 0.4f).SetDelay(0.3f).OnComplete(delegate() 
        {
            CmdRequest.SynchroniseCardsRequest();
        });
    }
}
