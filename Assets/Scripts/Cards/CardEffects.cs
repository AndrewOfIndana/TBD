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
    public string passiveDescription; 
    public StatusEffect passiveEffect;

    [Header("Active Effect Variables")]
    public string activeName; 
    public string activeDescription; 
    public StatusEffect activeEffect;
}
