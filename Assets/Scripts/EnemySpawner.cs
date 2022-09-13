using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    /*  
        Name: EnemySpawner.cs
        Description: This script spawns the enemies that will continually assault the player's home base. THe spawner will continually increase in spawn rate depending on the difficulty index of the GameManager
    */
    ObjectPool objectPool; //Reference to the object pool
    public float spawnRate; //The spawn rate of the enemies

    /*---      SETUP FUNCTIONS     ---*/
    /*-  Starts on the first frame -*/
    public void Start()
    {
        objectPool = ObjectPool.objectPoolInstance; ///Set objectPool to the objectPool instance 
    }

    /*---      UPDATE FUNCTIONS     ---*/
    /*-  Is called every frame -*/
    void Update()
    {
        //if rng is less than spawn rate
        if(Random.Range(1, 100) < spawnRate)
        {
            objectPool.SpawnFromPool("Enemy", transform.position, Quaternion.identity); //Spawn an enemy from the pool
        }
    }
}
