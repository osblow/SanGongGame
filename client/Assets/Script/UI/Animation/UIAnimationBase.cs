using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum AnimType
{
    None,
    Vertical,
    Horizontal,
    Scale,
    ScaleY
}

public class UIAnimationBase : MonoBehaviour 
{
    public AnimType AnimType = AnimType.None;
    public float From;
    public float Duration = 1.0f;
    public float Delay = 0;

    public bool AutoPlay = true;

    private bool m_isInitialized = false;
    private Vector3 m_originPos;
    private Vector3 m_originScale;


	// Use this for initialization
	void Start () 
	{
        
        if (AutoPlay)
        {
            Anim();
        }
    }

    void InitOrigin()
    {
        if (m_isInitialized)
        {
            return;
        }

        m_originPos = transform.position;
        m_originScale = transform.localScale;

        m_isInitialized = true;
    }

    private void OnEnable()
    {
        if (AutoPlay && m_isInitialized)
        {
            Anim();
        }
    }

    public void Anim()
    {
        InitOrigin();

        Vector3 position;
        switch (AnimType)
        {
            case AnimType.Vertical:
                position = m_originPos;
                transform.position += Vector3.up * From;
                transform.DOMove(position, Duration).SetDelay(Delay);
                break;
            case AnimType.Horizontal:
                position = m_originPos;
                transform.position += Vector3.right * From;
                transform.DOMove(position, Duration).SetDelay(Delay);
                break;
            case AnimType.Scale:
                Vector3 scale = m_originScale;
                float scaleFactor = (1 + From);
                if (scaleFactor < 0) scaleFactor = 0;
                transform.localScale *= scaleFactor;
                transform.DOScale(scale, Duration).SetDelay(Delay);
                break;
            case AnimType.ScaleY:
                scale = m_originScale;
                scaleFactor = (1 + From);
                if (scaleFactor < 0) scaleFactor = 0;
                transform.localScale = new Vector3(scale.x, scaleFactor * scale.y, scale.z);
                transform.DOScaleY(scale.y, Duration).SetDelay(Delay);
                break;
        }
    }

	// Update is called once per frame
	void Update () 
	{
		
	}
}
