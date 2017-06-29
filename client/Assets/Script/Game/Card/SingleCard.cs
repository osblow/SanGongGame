using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Osblow.App;

namespace Osblow.Game
{
    public class SingleCard : MonoBehaviour
    {
        public void SetNum(int cardVal)
        {
            Image img = GetComponent<Image>();
            img.sprite = Globals.SceneSingleton<CardMng>().GetSprite(cardVal);
        }
    }
}
