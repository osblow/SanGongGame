using UnityEngine;
using UnityEngine.UI;

public class VoiceTimerAnim : MonoBehaviour 
{
    public float Speed = 1.0f;

    private Image m_effect;

    private float m_fillAmount = 0;
    private bool m_isAnimating = false;

	// Use this for initialization
	void Start () 
	{
        m_effect = GetComponent<Image>();
	}
	
    public void StartAnim()
    {
        m_fillAmount = 1.0f;
        m_isAnimating = true;
    }

	// Update is called once per frame
	void Update () 
	{
        if (!m_isAnimating)
        {
            return;
        }

        m_effect.fillAmount = m_fillAmount;
        m_fillAmount -= Time.deltaTime * Speed;
        
        if(m_fillAmount < 0)
        {
            m_isAnimating = false;
        }
    }
}
