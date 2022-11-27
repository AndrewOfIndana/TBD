using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour, Idamageable
{
    /*  
        Name: PlayerSpawner.cs
        Description: This script spawns the player's troops and sets the stats of the troop

    */
    /*[Header("Static References")]*/
    GameManager gameManager;
    LevelManager levelManager;
    LevelUI levelUI;
    ObjectPool objectPool;

    [Header("Health Settings")]
    private float health;
    private float maxHealth;

    /*---      SETUP FUNCTIONS     ---*/
    /*-  Start is called before the first frame update -*/
    private void Start()
    {
        /* Gets the static instances and stores them in the Static References */
        gameManager = GameManager.instance;
        levelManager = LevelManager.instance;
        levelUI = LevelUI.instance;
        objectPool = ObjectPool.instance;

        health = levelManager.GetLevel().playerHealth;
        maxHealth = health;
    }

    /*---      FUNCTIONS     ---*/
    /*-  Spawns troops takes a Stats  -*/
    public void SpawnTroop(Stats unitToSpawn)
    {
        GameObject allyObj = objectPool.SpawnFromPool("Ally", transform.position, Quaternion.identity);
        TroopController ally = allyObj.GetComponent<TroopController>(); 
        
        //if this unit exist
        if(ally != null)
        {
            ally.SetUnit(unitToSpawn); //Sets ally type and stats using the unitToSpawn Stats
            ally.StartController(); //Sets enemy type and stats based on random number generator
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
            gameManager.SetGameState(GameStates.LOSE); //Sets GameStates to LOSE
            levelManager.ChangeState(); //Changes State for level
            this.gameObject.SetActive(false);
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

