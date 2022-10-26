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

    [Header("Player Settings")]
    public StatsList availbleUnits; //The available units the player can choose from
    //public cards availableCards;  //The available cards the player can choose from
    public int unitLimit; //the limit of units the player can choose
    public float mana; //The max amount of mana a player has
    public float manaRegen; //The max amount of mana regen a player has
    public float playerHealth; //The max amount of health the player base has

    [Header("Enemy Settings")]
    public StatsList enemyUnits; //the units the enemy has for this level
    //public cards enemyCards; //the cards the enemy has for this level
    public float enemySpawnRate; ///the spawn rate of the enemy spawner
    public float enemyHealth; //The max amount of health the enemy base has
    public float enemyCalmTime; //The amount of time for the enemy's calm state
    public float enemyEnragedTime; //The amount of time for the enemy's enraged state

    /*---      FUNCTIONS     ---*/
    /*-  Gets the total amount of time for each state -*/
    public float GetTotalTime()
    {
        return enemyCalmTime + enemyEnragedTime;
    }
}
