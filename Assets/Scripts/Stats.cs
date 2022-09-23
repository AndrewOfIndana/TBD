using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu] //Allows user create stats
public class Stats : ScriptableObject
{
    [Header("Information")]
    public int unitID; //The ID number of the unit
    public string unitName; //Name of the unit
    public string unitDescription; //Description of the unit

    [Header("Stats")]
    public float unitHealth; //Health of the unit
    public float unitAttack; //Attack of the unit
    public float unitSpeed; //Speed of the unit
    public float unitAttackRate; //Attack Rate of the unit
    public float unitAttackRange; //Attack Range of the unit

    [Header("Attributes")]
    public Sprite unitSprite; //Sprite of the unit
    public Vector3 unitSize; //Size of the BoxCollider of unit
    public float unitCost; //Cost of the unit
    public bool isUnitEnemy; //is the unit an enemy
    public int unitBehavior;
}
