using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadeIn : MonoBehaviour 
{
    private Image m_image;


	// Use this for initialization
	void Awake () 
	{
        m_image = GetComponent<Image>();
	}

    private void OnEnable()
    {
        m_image.color = new Color(0, 0, 0, 0);
        m_image.DOColor(Color.white, 0.4f).OnComplete(()=> m_image.color = new Color(0,0,0,0));
        
        m_image.DOColor(Color.white, 0.08f).SetLoops(3, LoopType.Yoyo).SetDelay(0.4f);
    }

    // Update is called once per frame
    void Update () 
	{
		
	}
}
