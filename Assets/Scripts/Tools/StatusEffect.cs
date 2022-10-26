using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu] //Allows user create stats
public class StatusEffect : ScriptableObject
{
    /*  
        Name: StatusEffect.cs
        Description: A ScriptableObject that holds an list StatusStatBuffs increase and the stat it is buffing

    */    
    [Header("Status Effect Variables")]
    public int effectId;
    public string effectName; //The name of the effect
    public List<StatusStatBuff> effects = new List<StatusStatBuff>(); //A lists of StatusStatBuff that is applied to a units stats
    public float effectLifetime; //The amount of time a status effect lasts
}
