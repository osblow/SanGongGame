using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingRotation : MonoBehaviour 
{
    public float Speed = 1;

    private float m_curAngle = 0;

    private float m_timer = 0;
	// Update is called once per frame
	void Update () 
	{
        m_timer += Time.deltaTime * Speed;
        if(m_timer < 1)
        {
            return;
        }
        m_timer = 0;

        m_curAngle -= 45;
        transform.rotation = Quaternion.Euler(0, 0, m_curAngle);
	}
}
