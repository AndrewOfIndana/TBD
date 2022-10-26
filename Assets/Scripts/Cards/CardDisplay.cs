using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    //calls UI elemets of card
    public Image cardImage;
    //creates an array of the cards created
    public Card[] cardList;

    public void fillTheCards(int cardIndex)
    {

        cardImage.sprite = cardList[cardIndex].cardImage;
    }
}
