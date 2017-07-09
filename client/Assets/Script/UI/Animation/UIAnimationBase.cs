using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum AnimType
{
    None,
    Vertical,
    Horizontal,
    Scale
}

public class UIAnimationBase : MonoBehaviour 
{
    public AnimType AnimType = AnimType.None;
    public float From;
    public float Duration = 1.0f;
    public float Delay = 0;

    public bool AutoPlay = true;


	// Use this for initialization
	void Start () 
	{
        if (AutoPlay)
        {
            Anim();
        }
	}
	
    public void Anim()
    {
        Vector3 position;
        switch (AnimType)
        {
            case AnimType.Vertical:
                position = transform.position;
                transform.position += Vector3.up * From;
                transform.DOMove(position, Duration).SetDelay(Delay);
                break;
            case AnimType.Horizontal:
                position = transform.position;
                transform.position += Vector3.right * From;
                transform.DOMove(position, Duration).SetDelay(Delay);
                break;
            case AnimType.Scale:
                Vector3 scale = transform.localScale;
                float scaleFactor = (1 + From);
                if (scaleFactor < 0) scaleFactor = 0;
                transform.localScale *= scaleFactor;
                transform.DOScale(scale, Duration).SetDelay(Delay);
                break;
        }
    }

	// Update is called once per frame
	void Update () 
	{
		
	}
}
