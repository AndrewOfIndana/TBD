using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour, Idamageable
{
    /*  
        Name: EnemySpawner.cs
        Description: This script spawns enemies and sets the stats of the enemy

    */
    /*[Header("Static References")]*/
    GameManager gameManager;
    LevelManager levelManager;
    LevelUI levelUI;
    ObjectPool objectPool;
    BossManager bossManager;

    [Header("Health Settings")]
    private float health;
    private float maxHealth;

    /*[Header("Script Settings")]*/
    private float spawnRate; 
    private List<Stats> typesOfEnemies = new List<Stats>();

    /*---      SETUP FUNCTIONS     ---*/
    /*-  Start is called before the first frame update -*/
    private void Start()
    {
        /* Gets the static instances and stores them in the Static References */
        gameManager = GameManager.instance;
        levelManager = LevelManager.instance;
        levelUI = LevelUI.instance;
        objectPool = ObjectPool.instance;

        health = levelManager.GetLevel().enemyHealth;
        maxHealth = health;

        /* Gets and sets variables form the level manager */
        typesOfEnemies = levelManager.GetEnemyUnits();
        spawnRate = levelManager.GetLevel().enemySpawnRate;
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

        //if gameStates is PLAYING
        if(gameManager.CheckIfPlaying())
        {
            GameObject enemyObj = objectPool.SpawnFromPool("Enemy", transform.position, Quaternion.identity);
            TroopController enemy = enemyObj.GetComponent<TroopController>();
            
            //if this enemy exist
            if(enemy != null)
            {
                enemy.SetUnit(typesOfEnemies[Random.Range(0, typesOfEnemies.Count)]); //Sets enemy type and stats based on random number generator
                enemy.StartController(); //Starts the enemy controller
            }
        }

        //if gameStates isn't WIN or LOSE
        if(!gameManager.CheckIfWinOrLose())
        {
            StartCoroutine(SpawnEnemy(rate)); 
        }
    }
    /*-  Handles taking damage takes a float that is the oncoming damage value -*/
    public void TakeDamage(float damage)
    {
        health -= damage; 
        levelUI.UpdateUI(); //Updates UI in the levelUI

        //if health is less than or equal to 0
        if(health <= 0)
        {
            //if BossManager does exist
            if(BossManager.instance != null)
            {
                //if local bossManager doesn't exist
                if(bossManager == null)
                {
                    /* STARTS BOSS FIGHT */
                    bossManager = BossManager.instance;
                    bossManager.StartBoss();
                }
            }
            //if BossManager doesn't exist 
            else if(BossManager.instance == null)
            {
                gameManager.SetGameState(GameStates.WIN); //Sets GameStates to WIN
                levelManager.ChangeState(); //Changes State for level
                this.gameObject.SetActive(false); 
            }
        }
    }

    /*---      SET/GET FUNCTIONS     ---*/
    /*-  Gets health -*/
    public float GetHealth()
    {
        return health;
    }
    /*-  Gets max health -*/
    public float GetMaxHealth()
    {
        return maxHealth;
    }
}
