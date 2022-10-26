using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu] //Allows user to create this 
public class StatsTags : ScriptableObject
{    
    /*  
        Name: StatsTags.cs
        Description: A ScriptableObject that holds all the shared tags for either faction

    */    
    [Header("Projectiles Tags")]
    public string bulletTag; //Tag of what the unit will fire

    [Header("Oncoming Tags")]
    public string oncomingBulletTag; //Tags of what bullets should take damage from
    public List<string> oncomingTags; //Tags of what the units should take damage from
}
