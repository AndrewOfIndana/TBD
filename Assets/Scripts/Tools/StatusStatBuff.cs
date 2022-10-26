using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuffedStats { attack, health, speed, attackRate } //The stats that can buffed

[CreateAssetMenu] //Allows user create stats
public class StatusStatBuff : ScriptableObject
{
    /*  
        Name: StatusStatBuff.cs
        Description: A ScriptableObject that holds a status effect and what stats it buffs

    */    
    public float effectIncrease; //The amount of stat increases for this effect
    public BuffedStats effectedStat; //The stat that will be buffed
    
    public float GetStatusBonus(BuffedStats buffedStat)
    {
        if(buffedStat == effectedStat)
        {
            return effectIncrease;
        }
        return 1;
    }
}
