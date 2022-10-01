using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Behaviour {MELEE, RANGED, DEFEND, KAMIKAZE, SUPPORT, TOWER, AOETOWER }; //The states the player can be in when in the air

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
    public float unitAttackRange; //Attack Range of the unit
    public float unitAttackRate; //Attack Rate of the unit

    [Header("Physical Attributes")]
    public Sprite unitSprite; //Sprite of the unit
    public Vector3 unitSize; //Size of the BoxCollider of unit

    [Header("Tag Attributes")]
    public bool isUnitEnemy; //is the unit an enemy
    public string unitTag;
    public string bulletTag;
    public string[] targetTags;
    public StatsTags oncomingTags;
    public string oncomingBulletTag;
    public string oncomingTroopTag;
    public string oncomingAssassinTag;
    public string oncomingTowerTag;
    public string oncomingBaseTag;

    [Header("Other Attributes")]
    public float unitCost; //Cost of the unit
    public Behaviour unitBehaviour;
}
