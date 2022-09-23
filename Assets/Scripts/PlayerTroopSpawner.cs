using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTroopSpawner : MonoBehaviour
{
    /*  
        Name: PlayerTroopSpawner.cs
        Description: This script spawns the Player's troops that will. The player controller script to handle what units need to spawn and such

    */
    ObjectPool objectPool; //Reference to the object pool
    
    /*---      SETUP FUNCTIONS     ---*/
    /*-  Starts on the first frame -*/
    public void Start()
    {
        objectPool = ObjectPool.objectPoolInstance; //Set objectPool to the objectPool instance 
    }

    /*---      FUNCTIONS     ---*/
    /*-  Spawns troops takes a Stats  -*/
    public void SpawnTroop(Stats unitToSpawn)
    {
        GameObject allyObj = objectPool.SpawnFromPool("Ally", transform.position, Quaternion.identity); //Spawn an player troop from the pool
        TroopController ally = allyObj.GetComponent<TroopController>(); //Gets the TroopController component from the spawned allyObj
        
        //if this unit exist
        if(ally != null)
        {
            ally.SetUnit(unitToSpawn); //Sets ally type and stats using the unitToSpawn Stats
        }
    }
}

