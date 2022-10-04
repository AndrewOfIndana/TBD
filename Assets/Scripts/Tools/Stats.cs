using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitType {TROOP, TOWER }; //The states of whether a unit is a troop or a tower
public enum Behaviour {MELEE, RANGED, DEFEND, KAMIKAZE, SUPPORT, AOE }; //The states of behavior the unit has

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
    public string unitDescription; //Description of the unit

    [Header("Stat Variables")]
    public float unitHealth; //Health of the unit
    public float unitAttack; //Attack of the unit
    public float unitSpeed; //Speed of the unit
    public float unitAttackRange; //Attack Range of the unit
    public float unitAttackRate; //Attack Rate of the unit

    [Header("Physical Variables")]
    public Sprite unitSprite; //Sprite of the unit
    public Vector3 unitSize; //Size of the BoxCollider of unit

    [Header("Tag Variables")]
    public bool isUnitEnemy; //is the unit an enemy
    public string unitTag;  //Tag of the unit
    public string[] targetTags; //Tags of what the unit should face
    public StatsTags sharedTags; //Tags of what the units share with other units of the same faction

    [Header("Other Variables")]
    public float unitCost; //Cost of the unit
    public UnitType unitType; //Type of the unit
    public Behaviour unitBehaviour; //The behavior of the unit
}
