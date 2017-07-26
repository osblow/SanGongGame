using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingNotice : MonoBehaviour 
{
    public float Speed = 10;

    RectTransform m_rectTrans;
    Vector3 m_originPos;


	// Use this for initialization
	void Start () 
	{
        m_rectTrans = GetComponent<RectTransform>();
        m_originPos = m_rectTrans.position;
	}
	

	// Update is called once per frame
	void Update () 
	{
        m_rectTrans.position += Vector3.left * Time.deltaTime * Speed;

        if(m_rectTrans.position.x + m_rectTrans.sizeDelta.x < 0)
        {
            m_rectTrans.position = m_originPos;
        }
	}
}
