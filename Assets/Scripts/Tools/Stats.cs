using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu] //Allows user to create this 
public class Stats : ScriptableObject
{
    /*  
        Name: Stats.cs
        Description: A ScriptableObject that holds unit specific variables for each troop and tower
    
    */
    [Header("Information")]
    public int unitID; //The ID number of the unit
    public string unitName; //Name of the unit
    [TextArea(3,10)]
    public string unitDescription; //Description of the unit

    [Header("Stats")]
    public float unitHealth; //Health of the unit
    public float unitAttack; //Attack of the unit
    public float unitSpeed; //Speed of the unit
    public float unitAttackRange; //Attack Range of the unit
    public float unitAttackRate; //Attack Rate of the unit

    [Header("Developer Variables")]
    public StatsHidden unitUtils;

    [Header("Setup Variables")]
    public bool isUnitEnemy; //is the unit an enemy
    public float unitCost; //Cost of the unit
    public Sprite unitThumbnail; //Thumbnail of the unit

}
