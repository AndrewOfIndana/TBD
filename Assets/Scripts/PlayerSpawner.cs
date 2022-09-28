using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSpawner : MonoBehaviour, IUnitController
{
    /*  
        Name: PlayerSpawner.cs
        Description: This script spawns the Player's troops that will. The player controller script to handle what units need to spawn and such

    */
    ObjectPool objectPool; //Reference to the object pool
    public Image healthBar; //Reference to the health bar image of the troop
    public float health;
    private float maxHealth = 1000;

    /*---      SETUP FUNCTIONS     ---*/
    /*-  Starts on the first frame -*/
    private void Start()
    {
        objectPool = ObjectPool.objectPoolInstance; //Set objectPool to the objectPool instance 
        health = maxHealth;
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
    public void TakeDamage(float damage)
    {
        health -= damage; //Subtracts from health with damage
        healthBar.fillAmount = health/maxHealth; //Resets healthBar by dividing health by maxHealth

        //if health is less than or equal to 0
        if(health <= 0)
        {
            this.gameObject.SetActive(false); //deactivate the troop
            Debug.Log("Player Wins");
        }
    }
}

