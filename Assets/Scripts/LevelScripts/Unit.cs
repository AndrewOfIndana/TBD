using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    /*  
        Name: Unit.cs
        Description: This script holds a static list of Units

    */
    public static List<Unit> unitList = new List<Unit>();
    public bool isEnemy;

    /*---      SETUP FUNCTIONS     ---*/
    /*-  OnEnable is called when the object becomes enabled -*/
    private void OnEnable()
    {
        unitList.Add(this); //Adds this unit to unitList
    }

    /*---      FUNCTIONS     ---*/
    /*-  Returns the lists of Units -*/
    public static List<Unit> GetUnitList()
    {
        return unitList;
    }
    /*-  OnDisable is called when the object becomes disabled -*/
    private void OnDisable()
    {
        unitList.Remove(this); //Removes this unit to unitList
    }

    /*---      SET/GET FUNCTIONS     ---*/
    /*-  Get isEnemy -*/
    public bool IsEnemy()
    {
        return isEnemy;
    }
}
