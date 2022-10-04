using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour, IUnitController
{
    /*  
        Name: EnemySpawner.cs
        Description: This script spawns enemies and sets the stats of the enemy

    */
    /*[Header("Static References")]*/
    LevelManager levelManager;
    LevelUI levelUI;
    ObjectPool objectPool;

    [Header("Health Variables")]
    public float health = 1000;
    [HideInInspector] public float maxHealth;

    [Header("Spawn References")]
    public float spawnRate; 
    public StatsList unitsLists; //A StatsList that is retrieved from the LevelManager
    // [HideInInspector] public StatsList unitsLists; //A StatsList that is retrieved from the LevelManager
    private Stats[] typesOfEnemies; //An array of stats retrieved from unitsLists

    /*---      SETUP FUNCTIONS     ---*/
    /*-  Start is called before the first frame update -*/
    private void Start()
    {
        typesOfEnemies = unitsLists.statsLists; //Retrieves unitsLists.statsLists and sets it to typesOfEnemies 

        /* Gets the static instances and stores them in the Static References */
        levelManager = LevelManager.levelManagerInstance;
        levelUI = LevelUI.levelUIinstance;
        objectPool = ObjectPool.objectPoolInstance;

        maxHealth = health;
    }
    /*-  StartGame is called when the game has started -*/
    public void StartGame()
    {
        StartCoroutine(SpawnEnemy(spawnRate));
    }

    /*---      FUNCTIONS     ---*/
    /*-  Repeatedly spawns Enemy takes a float for the time -*/
    private IEnumerator SpawnEnemy(float rate)
    {
        yield return new WaitForSeconds(rate); 
        GameObject enemyObj = objectPool.SpawnFromPool("Enemy", transform.position, Quaternion.identity);
        TroopController enemy = enemyObj.GetComponent<TroopController>();
        
        //if this enemy exist
        if(enemy != null)
        {
            enemy.SetUnit(typesOfEnemies[Random.Range(0, typesOfEnemies.Length)]); //Sets enemy type and stats based on random number generator
        }
        StartCoroutine(SpawnEnemy(rate)); 
    }
    /*-  Handles taking damage takes a float that is the oncoming damage value -*/
    public void TakeDamage(float damage)
    {
        health -= damage; 
        levelUI.UpdateUI(); //Updates UI in the levelUI

        //if health is less than or equal to 0
        if(health <= 0)
        {
            levelManager.ChangeState(GameStates.WIN); //Sets GameStates to WIN in the levelManager
            this.gameObject.SetActive(false); //deactivate the troop
        }
    }
}
