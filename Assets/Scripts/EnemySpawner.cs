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
    public Stats[] typesOfEnemies; //The numbered types of enemies available

    /*---      SETUP FUNCTIONS     ---*/
    /*-  Starts on the first frame -*/
    public void Start()
    {
        objectPool = ObjectPool.objectPoolInstance; ///Set objectPool to the objectPool instance 
        StartCoroutine(SpawnEnemy(spawnRate)); //Calls SpawnEnemy IEnumerator at spawnRate
    }

    /*---      UPDATE FUNCTIONS     ---*/
    /*-  Is called every frame -*/
    private IEnumerator SpawnEnemy(float rate)
    {
        yield return new WaitForSeconds(rate); //Waits for rate
        GameObject enemyObj = objectPool.SpawnFromPool("Enemy", transform.position, Quaternion.identity); //Spawn an enemy from the pool
        EnemyTroopController enemy = enemyObj.GetComponent<EnemyTroopController>(); //Gets the EnemyTroopController component from the spawned enemyObj
        
        //if this enemy exist
        if(enemy != null)
        {
            enemy.SetUnit(typesOfEnemies[Random.Range(0, typesOfEnemies.Length)]); //Sets enemy type based on random number generator
        }

        StartCoroutine(SpawnEnemy(rate)); //Recalls SpawnEnemy IEnumerator at spawnRate
    }
}
