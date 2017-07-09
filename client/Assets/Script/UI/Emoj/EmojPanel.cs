using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmojPanel : MonoBehaviour 
{
    public Image WordsBtn;
    public Image EmojBtn;

    public GameObject WordsPanel;
    public GameObject EmojsPanel;


	public void OnClickWordsBtn()
    {
        WordsPanel.SetActive(true);
        EmojsPanel.SetActive(false);

        WordsBtn.color = Color.white;
        EmojBtn.color = Color.gray;
    }

    public void OnClickEmojBtn()
    {
        WordsPanel.SetActive(false);
        EmojsPanel.SetActive(true);

        WordsBtn.color = Color.gray;
        EmojBtn.color = Color.white;
    }
}
