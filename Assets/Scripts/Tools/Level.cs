using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu] //Allows user create stats
public class Level : ScriptableObject
{
    /*  
        Name: Level.cs
        Description: A ScriptableObject that holds level specific variables for each level

    */    

    [Header("Information")]
    public int levelID; //The ID number of the level
    public string levelName; //Name of the level

    [Header("Player Variables")]
    public StatsList availbleUnits; //The available units the player can choose from
    //public cards availableCards;  //The available cards the player can choose from
    public int unitLimit; //the limit of units the player can choose
    public float mana;
    public float manaRegen;
    public float playerHealth;

    [Header("Enemy Variables")]
    public StatsList enemyUnits; //the units the enemy has for this level
    //public cards enemyCards; //the cards the enemy has for this level
    public float enemySpawnRate; ///the spawn rate of the enemy spawner
    public float enemyHealth;
    public float enemyCalmTime;
    public float enemyEnragedTime;

    public float GetTotalTime()
    {
        return enemyCalmTime + enemyEnragedTime;
    }
}
