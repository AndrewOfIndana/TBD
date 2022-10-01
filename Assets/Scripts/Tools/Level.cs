using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu] //Allows user create stats
public class Level : ScriptableObject
{
    [Header("Information")]
    public int levelID; //The ID number of the unit
    public string levelName; //Name of the unit

    public StatsList availbleUnits;
    public int unitLimit;
    //public cards availableCards;
    public int cardLimit;

    public StatsList enemyUnits;
    //public cards enemyCards;
    public float enemySpawnRate;
}
