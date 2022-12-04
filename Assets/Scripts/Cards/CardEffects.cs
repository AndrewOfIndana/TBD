using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu] //Allows user create stats
public class CardEffects : ScriptableObject
{
    /*  
        Name: StatusEffect.cs
        Description: A ScriptableObject that holds a passive and active effect for the card

    */    
    public int cardID;

    [Header("Passive Effect Variables")]
    public string passiveName; 
    
    [TextArea(3,10)] public string passiveDescription; 
    public StatusEffect passiveEffect;

    [Header("Active Effect Variables")]
    public string activeName; 
    [TextArea(3,10)] public string activeDescription; 
    public StatusEffect activeEffect;
    public bool isAppliedToEnemy;
}
