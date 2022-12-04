using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject hiddenUI;

    /*---      FUNCTIONS     ---*/
    /*-  Changes the level based game state based on what has happened in game -*/
    public void OnPointerEnter(PointerEventData eventData)
    {
        hiddenUI.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        hiddenUI.SetActive(false);
    }
}
