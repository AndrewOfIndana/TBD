using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu] //Allows user create stats
public class StatsList : ScriptableObject
{
    /*  
        Name: StatsList.cs
        Description: A ScriptableObject that holds a array of unit stats

    */    
    [Header("Script Variables")]
    public List<Stats> statsLists = new List<Stats>(); //Array of units
}
