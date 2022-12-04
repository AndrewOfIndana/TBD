using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardInfo : MonoBehaviour
{
    /*  
        Name: CardInfo.cs

    */    
    public TextMeshProUGUI cardName;
    public TextMeshProUGUI cardPassiveEffect;
    public TextMeshProUGUI cardActiveEffect;

    /*---      SETUP FUNCTIONS     ---*
    /*-  Start is called before the first frame update -*/
    private void Start()
    {    
        this.gameObject.SetActive(false);
    }

    public void SetCardInfo(Card info)
    {
        cardName.text = info.Name;
        cardPassiveEffect.text = "Passive: " + info.value.passiveDescription;
        cardActiveEffect.text = "Active: " + info.value.activeDescription;
    }
}
