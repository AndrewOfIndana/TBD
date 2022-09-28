using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour, IUnitController
{
    /*  
        Name: EnemySpawner.cs
        Description: This script spawns the enemies that will continually assault the player's home base. The enemies will spawn randomly from an array of enemies.

    */
    ObjectPool objectPool; //Reference to the object pool

    public Image healthBar; //Reference to the health bar image of the troop
    public float health;
    private float maxHealth = 1000;

    public float spawnRate; //The spawn rate of the enemies
    public StatsList unitsLists;
    private Stats[] typesOfEnemies; //The types of enemies available

    /*---      SETUP FUNCTIONS     ---*/
    /*-  Starts on the first frame -*/
    private void Start()
    {
        objectPool = ObjectPool.objectPoolInstance; ///Set objectPool to the objectPool instance 
        typesOfEnemies = unitsLists.statsLists;
        StartCoroutine(SpawnEnemy(spawnRate)); //Calls SpawnEnemy IEnumerator at spawnRate
    }

    /*---      FUNCTIONS     ---*/
    /*-  Repeatedly spawns Enemy takes a float for the time -*/
    private IEnumerator SpawnEnemy(float rate)
    {
        yield return new WaitForSeconds(rate); //Waits for rate
        GameObject enemyObj = objectPool.SpawnFromPool("Enemy", transform.position, Quaternion.identity); //Spawn an enemy from the pool
        TroopController enemy = enemyObj.GetComponent<TroopController>(); //Gets the EnemyTroopController component from the spawned enemyObj
        
        //if this enemy exist
        if(enemy != null)
        {
            enemy.SetUnit(typesOfEnemies[Random.Range(0, typesOfEnemies.Length)]); //Sets enemy type and stats based on random number generator
        }

        StartCoroutine(SpawnEnemy(rate)); //Recalls SpawnEnemy IEnumerator at spawnRate
    }

     /*-  Handles taking damage takes a float that is the oncoming damage value -*/
    public void TakeDamage(float damage)
    {
        health -= damage; //Subtracts from health with damage
        healthBar.fillAmount = health/maxHealth; //Resets healthBar by dividing health by maxHealth

        //if health is less than or equal to 0
        if(health <= 0)
        {
            this.gameObject.SetActive(false); //deactivate the troop
            Debug.Log("Enemies Wins");
        }
    }
}
