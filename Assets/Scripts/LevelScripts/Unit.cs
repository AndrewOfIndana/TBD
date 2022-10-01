using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public static List<Unit> unitList = new List<Unit>();

    public static List<Unit> GetUnitList()
    {
        return unitList;
    }

    private void OnEnable()
    {
        unitList.Add(this);
    }
    
    private void OnDisable()
    {
        unitList.Remove(this);
    }
}
