using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitType {TROOP, TOWER, BOSS }; //The states of whether a unit is a troop or a tower
public enum Behaviour {MELEE, RANGED, DEFEND, KAMIKAZE, SUPPORT, AOE }; //The states of behavior the unit has

[CreateAssetMenu] //Allows user to create this 
public class StatsHidden : ScriptableObject
{
    /*  
        Name: Stats.cs
        Description: A ScriptableObject that holds dev hidden unit specific variables for each troop and tower
    
    */
    [Header("Physical Variables")]
    public Sprite unitSprite; //Sprite of the unit
    public Vector3 unitSize; //Size of the BoxCollider of unit
    public StatsAudio unitsSfx; //Stores the audio of the a unit
    public float unitAnimationSpeed; //The animation speed of the unit

    [Header("Distinction Variables")]
    public UnitType unitType; //Type of the unit
    public Behaviour unitBehaviour; //The behavior of the unit
    public string unitTag;  //Tag of the unit
    public string[] targetTags; //Tags of what the unit should face
    public StatsTags sharedTags; //Tags of what the units share with other units of the same faction

    [Header("Status Effects Variables")]
    public List<StatusEffect> unitKeyEffects = new List<StatusEffect>();
}
