using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuffedStats { attack, health, speed, attackRate, attackRange } //The stats that can buffed

[CreateAssetMenu] //Allows user create stats
public class StatusEffect : ScriptableObject
{
    /*  
        Name: StatusEffect.cs
        Description: A ScriptableObject that holds a stats increase and the stat it is buffing

    */    

    public float effectIncrease; //The amount of stat increases for this effect  
    public BuffedStats effectedStat; //The stat that will be buffed

    /*---      FUNCTIONS     ---*/
    /*-  Gets the multiplicate bonus by checking if the buffedStat is the effectedStat -*/
    public float GetStatusBonus(BuffedStats buffedStat)
    {
        if(effectedStat == buffedStat)
        {
            float bonus = effectIncrease;
            return bonus;
        }
        else
        {
            Debug.Log("shit didn't apply");
            return 1f;
        }
    }
}
